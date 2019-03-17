using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace XSDR
{
    public class WordExporter
    {
        public void ExportXSDRDocument(XSDRDocument document, string filePath)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();

                mainPart.Document = new Document();

                var body = mainPart.Document.AppendChild(new Body());

                foreach (var s in document.Sections)
                {
                    foreach (var e1 in s.Subelements)
                    {
                        if (e1 is XSDRParagraph || e1 is XSDRHeading)
                        {
                            var paragraph = body.AppendChild(new Paragraph());

                              foreach(var e2 in (e1 as XSDRContentElement).Subelements)
                            {
                                  if (e2 is XSDRTextElement)
                                {
                                    var text = (e2 as XSDRTextElement).Text;

                                    var run = paragraph.AppendChild(new Run());
                                    var runProperties = new RunProperties(new RunFonts() { Ascii = "Book Antiqua" }, new FontSize() { Val = "20" });

                                    run.PrependChild(runProperties);
                                    run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
                                }
                                if (e2 is XSDRItalic)
                                {
                                    var text =( (e2 as XSDRItalic).Subelements[0] as XSDRTextElement).Text;

                                    var run = paragraph.AppendChild(new Run());
                                    var runProperties = new RunProperties(new RunFonts() { Ascii = "Book Antiqua" }, new FontSize() { Val = "20" }, new Italic());

                                    run.PrependChild(runProperties);
                                    run.AppendChild(new Text() { Text = text,   Space = SpaceProcessingModeValues.Preserve });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
