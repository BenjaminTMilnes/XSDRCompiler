using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using XSDR.Bibliography;
using DSS;
using System.Text.RegularExpressions;

namespace XSDR
{
    public class XMLImporter
    {
        private DSSImporter _dssImporter;
        private XSDRDocument _xsdrDocument;

        private string[] _headingTags;
        private int[] _headingTagLevels;

        public XMLImporter()
        {
            _dssImporter = new DSSImporter();

            _headingTags = new string[] { "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8", "h9", "h10", "heading1", "heading2", "heading3", "heading4", "heading5", "heading6", "heading7", "heading8", "heading9", "heading10" };
            _headingTagLevels = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        public XSDRDocument ImportDocument(string xmlFilePath, string dssFilePath = "")
        {
            var xsdrDocument = new XSDRDocument();

            _xsdrDocument = xsdrDocument;

            var xmlDocument = new XmlDocument();

            xmlDocument.Load(xmlFilePath);

            ImportMetadata(xmlDocument, xsdrDocument);
            ImportTemplates(xmlDocument, xsdrDocument);
            ImportSections(xmlDocument, xsdrDocument);
            ImportBibliography(xmlDocument, xsdrDocument);

            if (dssFilePath != "")
            {
                var dss = File.ReadAllText(dssFilePath);
                var dssDocument = _dssImporter.ImportDocument(dss);

                var dssResolver = new DSSResolver();

                dssResolver.ResolveDSS(dssDocument, xsdrDocument);
            }

            return xsdrDocument;
        }

        private void ImportMetadata(XmlDocument xmlDocument, XSDRDocument xsdrDocument)
        {
            var version = xmlDocument.SelectSingleNode("/document").Attributes["version"].Value;
            var title = xmlDocument.SelectSingleNode("/document/title").InnerText;
            var subtitle = xmlDocument.SelectSingleNode("/document/subtitle").InnerText;
            var keywords = xmlDocument.SelectSingleNode("/document/keywords").InnerText;

            xsdrDocument.Version = version;
            xsdrDocument.Title = title;
            xsdrDocument.Subtitle = subtitle;
            xsdrDocument.Keywords = keywords.Split(',').Select(s => s.Trim());
        }

        private void ImportTemplates(XmlDocument xmlDocument, XSDRDocument xsdrDocument)
        {
            var pageTemplates = xmlDocument.SelectNodes("/document/templates/page-template");

            foreach (XmlNode pageTemplate in pageTemplates)
            {
                var pt = new XSDRPageTemplate();

                pt.Subelements = GetPageElementsFromXML(pageTemplate.ChildNodes);

                xsdrDocument.Templates.Add(pt);
            }
        }

        private void ImportSections(XmlDocument xmlDocument, XSDRDocument xsdrDocument)
        {
            var sections = xmlDocument.SelectNodes("/document/sections/section");

            foreach (XmlNode section in sections)
            {
                var s = new XSDRSection(xsdrDocument);

                s.Subelements = GetPageElementsFromXML(section.ChildNodes);

                xsdrDocument.Sections.Add(s);
            }
        }

        private void ImportBibliography(XmlDocument xmlDocument, XSDRDocument xsdrDocument)
        {
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

        private string CompressWhiteSpace(string text)
        {
            return Regex.Replace(text, "\\s+", " ");
        }

        private IXSDRPageElement GetPageElementFromXML(XmlNode xmlNode)
        {
            if (xmlNode.NodeType == XmlNodeType.Text)
            {
                var text = xmlNode.InnerText;

                text = CompressWhiteSpace(text);

                return new XSDRTextElement(text);
            }
            if (xmlNode.NodeType == XmlNodeType.Element)
            {
                if (xmlNode.Name == "p" || xmlNode.Name == "paragraph")
                {
                    var p = new XSDRParagraph();

                    ApplyInlineStyling(xmlNode, p);
                    p.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    if (p.Subelements.Any())
                    {
                        if (p.Subelements.First() is XSDRTextElement)
                        {
                            var e = p.Subelements.First() as XSDRTextElement;

                            e.Text = e.Text.TrimStart();
                        }
                        if (p.Subelements.Last() is XSDRTextElement)
                        {
                            var e = p.Subelements.Last() as XSDRTextElement;

                            e.Text = e.Text.TrimEnd();
                        }
                    }

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
                if (_headingTags.Any(t => t == xmlNode.Name))
                {
                    var i = Array.IndexOf(_headingTags, xmlNode.Name);
                    var level = _headingTagLevels[i];

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
                if (xmlNode.Name == "header")
                {
                    var header = new XSDRHeader();

                    ApplyInlineStyling(xmlNode, header);
                    header.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return header;
                }
                if (xmlNode.Name == "footer")
                {
                    var footer = new XSDRFooter();

                    ApplyInlineStyling(xmlNode, footer);
                    footer.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return footer;
                }
                if (xmlNode.Name == "v" || xmlNode.Name == "pv" || xmlNode.Name == "page-variable")
                {
                    var pv = new XSDRPageVariable();

                    if (xmlNode.Attributes["name"] != null)
                    {
                        pv.Name = xmlNode.Attributes["name"].Value;
                    }

                    if (pv.Name == "title")
                    {
                        pv.Value = _xsdrDocument.Title;
                    }
                    if (pv.Name == "subtitle")
                    {
                        pv.Value = _xsdrDocument.Subtitle;
                    }

                    ApplyInlineStyling(xmlNode, pv);

                    return pv;
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
