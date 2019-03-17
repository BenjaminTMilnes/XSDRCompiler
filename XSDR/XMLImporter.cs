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

                foreach (XmlNode e in section.ChildNodes)
                {
                    if (e.NodeType == XmlNodeType.Element)
                    {
                        if (e.Name == "p" || e.Name == "paragraph")
                        {
                            var p = new XSDRParagraph();

                              foreach(XmlNode e2 in e.ChildNodes)
                            {
                                  if (e2.NodeType == XmlNodeType.Text)
                                {
                                    p.Subelements.Add(new XSDRTextElement(e2.InnerText));
                                }
                                    if (e2.NodeType == XmlNodeType.Element &&( e2.Name == "i" || e2.Name == "italic"))
                                {
                                    var i = new XSDRItalic();

                                    i.Subelements.Add(new XSDRTextElement(e2.InnerText));

                                    p.Subelements.Add(i);
                                }
                            }

                            s.Subelements.Add(p);
                        }
                        if (e.Name == "h1" || e.Name == "heading1")
                        {
                            var h1 = new XSDRHeading();

                            h1.Subelements.Add(new XSDRTextElement(e.InnerText));

                            s.Subelements.Add(h1);
                        }
                    }
                }

                document.Sections.Add(s);
            }

            return document;
        }

    }
}
