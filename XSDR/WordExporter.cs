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
        private string _defaultFontName = "Garamond";
        private int _defaultFontHeight = 10;

        private FontSize _getFontSize(int fontHeight)
        {
            return new FontSize() { Val = (fontHeight * 2).ToString() };
        }

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
                        if (e1 is XSDRParagraph || e1 is XSDRHeading )
                        {
                            var paragraph = body.AppendChild(new Paragraph());

                            foreach (var e2 in (e1 as XSDRContentElement).Subelements)
                            {
                                if (e2 is XSDRTextElement)
                                {
                                    var text = (e2 as XSDRTextElement).Text;

                                    var run = paragraph.AppendChild(new Run());
                                    var runProperties = new RunProperties(new RunFonts() { Ascii = _defaultFontName }, _getFontSize(_defaultFontHeight));

                                    run.PrependChild(runProperties);
                                    run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
                                }
                                if (e2 is XSDRItalic)
                                {
                                    var text = ((e2 as XSDRItalic).Subelements[0] as XSDRTextElement).Text;

                                    var run = paragraph.AppendChild(new Run());
                                    var runProperties = new RunProperties(new RunFonts() { Ascii = _defaultFontName }, _getFontSize(_defaultFontHeight), new Italic());

                                    run.PrependChild(runProperties);
                                    run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
                                }
                                if (e2 is XSDRBold)
                                {
                                    var text = ((e2 as XSDRBold).Subelements[0] as XSDRTextElement).Text;

                                    var run = paragraph.AppendChild(new Run());
                                    var runProperties = new RunProperties(new RunFonts() { Ascii = _defaultFontName }, _getFontSize(_defaultFontHeight), new Bold());

                                    run.PrependChild(runProperties);
                                    run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
                                }
                                if (e2 is XSDRUnderline)
                                {
                                    var text = ((e2 as XSDRUnderline).Subelements[0] as XSDRTextElement).Text;

                                    var run = paragraph.AppendChild(new Run());
                                    var runProperties = new RunProperties(new RunFonts() { Ascii = _defaultFontName }, _getFontSize(_defaultFontHeight), new Underline());

                                    run.PrependChild(runProperties);
                                    run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
                                }
                                if (e2 is XSDRStrikethrough)
                                {
                                    var text = ((e2 as XSDRStrikethrough).Subelements[0] as XSDRTextElement).Text;

                                    var run = paragraph.AppendChild(new Run());
                                    var runProperties = new RunProperties(new RunFonts() { Ascii = _defaultFontName }, _getFontSize(_defaultFontHeight), new Strike());

                                    run.PrependChild(runProperties);
                                    run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
                                }
                              
                            }
                        }
                        if (e1 is XSDRPageBreak)
                        {
                            var paragraph = body.AppendChild(new Paragraph());
                            var run = paragraph.AppendChild(new Run());

                            run.AppendChild(new Break() { Type = BreakValues.Page });
                        }
                    }
                }
            }
        }
    }
}
