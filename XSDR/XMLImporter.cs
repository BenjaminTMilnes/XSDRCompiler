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
                if (xmlNode.Name == "h1" || xmlNode.Name == "heading1")
                {
                    var h1 = new XSDRHeading();

                    h1.Level = 1;
                    h1.Subelements = GetPageElementsFromXML(xmlNode.ChildNodes);

                    return h1;
                }
            }

            throw new NotImplementedException();
        }
    }
}
