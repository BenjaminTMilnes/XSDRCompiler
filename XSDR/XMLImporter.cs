using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using XSDR.Bibliography;
using DSS;

namespace XSDR
{
    public class XMLImporter
    {
        private DSSImporter _dssImporter;
        private XSDRDocument _xsdrDocument;

        public XMLImporter()
        {
            _dssImporter = new DSSImporter();
        }

        public XSDRDocument ImportDocument(string xmlFilePath, string dssFilePath = "")
        {
            var xsdrDocument = new XSDRDocument();

            _xsdrDocument = xsdrDocument;

            var xmlDocument = new XmlDocument();

            xmlDocument.Load(xmlFilePath);

            var version = xmlDocument.SelectSingleNode("/document").Attributes["version"].Value;
            var title = xmlDocument.SelectSingleNode("/document/title").InnerText;
            var subtitle = xmlDocument.SelectSingleNode("/document/subtitle").InnerText;
            var keywords = xmlDocument.SelectSingleNode("/document/keywords").InnerText;

            xsdrDocument.Version = version;
            xsdrDocument.Title = title;
            xsdrDocument.Subtitle = subtitle;
            xsdrDocument.Keywords = keywords.Split(',').Select(s => s.Trim());

            var sections = xmlDocument.SelectNodes("/document/sections/section");

            if (xmlDocument.SelectSingleNode("/document/bibliography") != null && xmlDocument.SelectSingleNode("/document/bibliography/references") != null)
            {
                var references = xmlDocument.SelectNodes("/document/bibliography/references/reference");

                foreach (XmlNode reference in references)
                {
                    if (reference.Attributes["type"] != null)
                    {
                        if (reference.Attributes["type"].Value == "book")
                        {
                            var book = new XSDRBookReference();

                            book.Name = reference.Attributes["name"].Value;
                            book.Title = reference.SelectSingleNode("title").InnerText;

                            xsdrDocument.Bibliography.References.Add(book);
                        }
                        if (reference.Attributes["type"].Value == "website")
                        {
                            var website = new XSDRWebsiteReference();

                            website.Name = reference.Attributes["name"].Value;
                            website.Title = reference.SelectSingleNode("title").InnerText.Trim();
                            website.URL = reference.SelectSingleNode("url").InnerText.Trim();

                            xsdrDocument.Bibliography.References.Add(website);
                        }
                    }
                }
            }

            foreach (XmlNode section in sections)
            {
                var s = new XSDRSection();

                s.Subelements = GetPageElementsFromXML(section.ChildNodes);

                xsdrDocument.Sections.Add(s);
            }

            if (dssFilePath != "")
            {
                var dss = File.ReadAllText(dssFilePath);
                var dssDocument = _dssImporter.ImportDocument(dss);

                var dssResolver = new DSSResolver();

                dssResolver.ResolveDSS(dssDocument, xsdrDocument);
            }

            return xsdrDocument;
        }

        private bool BibliographicReferenceExists(string referenceName)
        {
            return _xsdrDocument.Bibliography.References.Any(r => r.Name == referenceName);
        }

        private int GetBibliographicReferenceNumber(string referenceName)
        {
            if (referenceName == null || !BibliographicReferenceExists(referenceName))
            {
                return -1;
            }

            if (!_xsdrDocument.Bibliography.ReferenceOrder.Any(rn => rn == referenceName))
            {
                _xsdrDocument.Bibliography.ReferenceOrder.Add(referenceName);
            }

            return _xsdrDocument.Bibliography.ReferenceOrder.IndexOf(referenceName) + 1;
        }

        private IList<IXSDRPageElement> GetPageElementsFromXML(XmlNodeList xmlNodes)
        {
            var elements = new List<IXSDRPageElement>();

            foreach (XmlNode xmlNode in xmlNodes)
            {
                elements.Add(GetPageElementFromXML(xmlNode));
            }

            return elements;
        }

        private IXSDRPageElement GetPageElementFromXML(XmlNode xmlNode)
        {
            if (xmlNode.NodeType == XmlNodeType.Text)
            {
                return new XSDRTextElement(xmlNode.InnerText);
            }
            if (xmlNode.NodeType == XmlNodeType.Element)
            {
                if (xmlNode.Name == "p" || xmlNode.Name == "paragraph")
                {
                    var p = new XSDRParagraph();

                    ApplyInlineStyling(xmlNode, p);
                    p.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return p;
                }
                if (xmlNode.Name == "i" || xmlNode.Name == "italic")
                {
                    var i = new XSDRItalic();

                    ApplyInlineStyling(xmlNode, i);
                    i.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return i;
                }
                if (xmlNode.Name == "b" || xmlNode.Name == "bold")
                {
                    var b = new XSDRBold();

                    ApplyInlineStyling(xmlNode, b);
                    b.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return b;
                }
                if (xmlNode.Name == "u" || xmlNode.Name == "underline")
                {
                    var u = new XSDRUnderline();

                    ApplyInlineStyling(xmlNode, u);
                    u.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return u;
                }
                if (xmlNode.Name == "s" || xmlNode.Name == "strikethrough")
                {
                    var s = new XSDRStrikethrough();

                    ApplyInlineStyling(xmlNode, s);
                    s.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return s;
                }
                if (xmlNode.Name == "ol" || xmlNode.Name == "ordered-list")
                {
                    var ol = new XSDROrderedList();

                    ApplyInlineStyling(xmlNode, ol);
                    ol.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return ol;
                }
                if (xmlNode.Name == "ul" || xmlNode.Name == "unordered-list")
                {
                    var ul = new XSDRUnorderedList();

                    ApplyInlineStyling(xmlNode, ul);
                    ul.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return ul;
                }
                if (xmlNode.Name == "li" || xmlNode.Name == "list-item")
                {
                    var li = new XSDRListItem();

                    ApplyInlineStyling(xmlNode, li);
                    li.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return li;
                }
                if ((new string[] { "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8", "h9", "h10", "heading1", "heading2", "heading3", "heading4", "heading5", "heading6", "heading7", "heading8", "heading9", "heading10" }).Any(t => t == xmlNode.Name))
                {
                    var level = 0;

                    switch (xmlNode.Name)
                    {
                        case "h1":
                        case "heading1":
                            level = 1;
                            break;
                        case "h2":
                        case "heading2":
                            level = 2;
                            break;
                        case "h3":
                        case "heading3":
                            level = 3;
                            break;
                        case "h4":
                        case "heading4":
                            level = 4;
                            break;
                        case "h5":
                        case "heading5":
                            level = 5;
                            break;
                        case "h6":
                        case "heading6":
                            level = 6;
                            break;
                        case "h7":
                        case "heading7":
                            level = 7;
                            break;
                        case "h8":
                        case "heading8":
                            level = 8;
                            break;
                        case "h9":
                        case "heading9":
                            level = 9;
                            break;
                        case "h10":
                        case "heading10":
                            level = 10;
                            break;
                    }

                    var h = new XSDRHeading(level);

                    ApplyInlineStyling(xmlNode, h);
                    h.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return h;
                }
                if (xmlNode.Name == "pb" || xmlNode.Name == "page-break")
                {
                    var pb = new XSDRPageBreak();

                    return pb;
                }
                if (xmlNode.Name == "c" || xmlNode.Name == "citation")
                {
                    var c = new XSDRCitation();

                    if (xmlNode.Attributes["rn"] != null)
                    {
                        c.ReferenceName = xmlNode.Attributes["rn"].Value;
                    }
                    if (xmlNode.Attributes["reference-name"] != null)
                    {
                        c.ReferenceName = xmlNode.Attributes["reference-name"].Value;
                    }

                    c.Number = GetBibliographicReferenceNumber(c.ReferenceName);

                    return c;
                }
            }

            throw new NotImplementedException();
        }

        public void ApplyInlineStyling(XmlNode xmlNode, XSDRContentElement element)
        {
            if (xmlNode.Attributes["style"] != null)
            {
                element.Style.Properties = _dssImporter.GetInlineProperties(xmlNode.Attributes["style"].Value).ToList();
            }
        }
    }
}
