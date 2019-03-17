using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XSDR
{
    public class XMLImporter
    {
        public XSDRDocument ImportDocument(string filePath)
        {
            var document = new XSDRDocument();

            var xmlDocument = new XmlDocument();

            xmlDocument.Load(filePath);

            var version = xmlDocument.SelectSingleNode("/document").Attributes["version"].Value;
            var title = xmlDocument.SelectSingleNode("/document/title").InnerText;
            var subtitle = xmlDocument.SelectSingleNode("/document/subtitle").InnerText;
            var keywords = xmlDocument.SelectSingleNode("/document/keywords").InnerText;

            document.Version = version;
            document.Title = title;
            document.Subtitle = subtitle;
            document.Keywords = keywords.Split(',').Select(s => s.Trim());

            var sections = xmlDocument.SelectNodes("/document/sections/section");

            foreach (XmlNode section in sections)
            {
                var s = new XSDRSection();

                s.Subelements = GetPageElementsFromXML(section.ChildNodes);

                document.Sections.Add(s);
            }

            return document;
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

                    p.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return p;
                }
                if (xmlNode.Name == "i" || xmlNode.Name == "italic")
                {
                    var i = new XSDRItalic();

                    i.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return i;
                }
                if (xmlNode.Name == "b" || xmlNode.Name == "bold")
                {
                    var b = new XSDRBold();

                    b.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return b;
                }
                if (xmlNode.Name == "u" || xmlNode.Name == "underline")
                {
                    var u = new XSDRUnderline();

                    u.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return u;
                }
                if (xmlNode.Name == "s" || xmlNode.Name == "strikethrough")
                {
                    var s = new XSDRStrikethrough();

                    s.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return s;
                }
                if ((new string[] { "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8", "h9", "h10", "heading1", "heading2", "heading3", "heading4", "heading5", "heading6", "heading7", "heading8", "heading9", "heading10" }).Any(t => t == xmlNode.Name))
                {
                    var h = new XSDRHeading();

                    switch (xmlNode.Name)
                    {
                        case "h1":
                        case "heading1":
                            h.Level = 1;
                            break;
                        case "h2":
                        case "heading2":
                            h.Level = 2;
                            break;
                        case "h3":
                        case "heading3":
                            h.Level = 3;
                            break;
                        case "h4":
                        case "heading4":
                            h.Level = 4;
                            break;
                        case "h5":
                        case "heading5":
                            h.Level = 5;
                            break;
                        case "h6":
                        case "heading6":
                            h.Level = 6;
                            break;
                        case "h7":
                        case "heading7":
                            h.Level = 7;
                            break;
                        case "h8":
                        case "heading8":
                            h.Level = 8;
                            break;
                        case "h9":
                        case "heading9":
                            h.Level = 9;
                            break;
                        case "h10":
                        case "heading10":
                            h.Level = 10;
                            break;
                    }

                    h.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return h;
                }
                if (xmlNode.Name == "pb" || xmlNode.Name == "page-break")
                {
                    var pb = new XSDRPageBreak();

                    return pb;
                }
            }

            throw new NotImplementedException();
        }
    }
}
