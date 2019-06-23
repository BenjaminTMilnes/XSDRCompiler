using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using XSDR.Bibliography;

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

        private int ConvertFromMillimetres(double mm)
        {
            return (int)Math.Round(mm * 72 * 20 / 25.4);
        }

        public void ExportXSDRDocument(XSDRDocument document, string filePath)
        {
            OpenSettings openSettings = new OpenSettings();

            openSettings.MarkupCompatibilityProcessSettings = new MarkupCompatibilityProcessSettings(MarkupCompatibilityProcessMode.ProcessAllParts, FileFormatVersions.Office2013);

            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();

                mainPart.Document = new Document();

                var header = mainPart.AddNewPart<HeaderPart>();
                var footer = mainPart.AddNewPart<FooterPart>();

                var body = mainPart.Document.AppendChild(new Body());

                var sectionProperties = new SectionProperties();
                var pageSize = new PageSize();
                pageSize.Width = (uint)ConvertFromMillimetres(128.5);
                pageSize.Height = (uint)ConvertFromMillimetres(198.4);

                var pageMargin = new PageMargin() { Top = ConvertFromMillimetres(20), Bottom = ConvertFromMillimetres(20), Left = (uint)ConvertFromMillimetres(20), Right = (uint)ConvertFromMillimetres(20) };

                sectionProperties.Append(pageSize);
                sectionProperties.Append(pageMargin);
                body.Append(sectionProperties);

                foreach (var s in document.Sections)
                {
                    foreach (var e1 in s.Subelements)
                    {
                        if (e1 is XSDRParagraph || e1 is XSDRHeading)
                        {
                            var paragraphProperties = new ParagraphProperties();

                            paragraphProperties.Append(new Indentation() { Hanging = (e1 as XSDRContentElement).CalculatedStyle.ParagraphIndentation.Times(-1).MSWUnits.ToString() });
                            
                            paragraphProperties.Append(new Justification() { Val = JustificationValues.Both });

                            var paragraph = body.AppendChild(new Paragraph(paragraphProperties));

                            foreach (var e2 in (e1 as XSDRContentElement).Subelements)
                            {
                                if (e2 is XSDRTextElement)
                                {
                                    var text = (e2 as XSDRTextElement).Text;

                                    AddRunToParagraph(paragraph, text, _defaultFontName, _defaultFontHeight);
                                }
                                if (e2 is XSDRItalic)
                                {
                                    var text = ((e2 as XSDRItalic).Subelements[0] as XSDRTextElement).Text;

                                    AddRunToParagraph(paragraph, text, _defaultFontName, (int)Math.Round((e2 as XSDRItalic).CalculatedStyle.FontHeight.Points), true);
                                }
                                if (e2 is XSDRBold)
                                {
                                    var text = ((e2 as XSDRBold).Subelements[0] as XSDRTextElement).Text;

                                    AddRunToParagraph(paragraph, text, _defaultFontName, (int)Math.Round((e2 as XSDRBold).CalculatedStyle.FontHeight.Points), false, true);
                                }
                                if (e2 is XSDRUnderline)
                                {
                                    var text = ((e2 as XSDRUnderline).Subelements[0] as XSDRTextElement).Text;

                                    AddRunToParagraph(paragraph, text, _defaultFontName, (int)Math.Round((e2 as XSDRUnderline).CalculatedStyle.FontHeight.Points), false, false, true);
                                }
                                if (e2 is XSDRStrikethrough)
                                {
                                    var text = ((e2 as XSDRStrikethrough).Subelements[0] as XSDRTextElement).Text;

                                    AddRunToParagraph(paragraph, text, _defaultFontName, (int)Math.Round((e2 as XSDRStrikethrough).CalculatedStyle.FontHeight.Points), false, false, false, true);
                                }
                                if (e2 is XSDRCitation)
                                {
                                    var text = " [" + (e2 as XSDRCitation).Number  + "]";

                                    AddRunToParagraph(paragraph, text, _defaultFontName, (int)Math.Round((e1 as XSDRContentElement).CalculatedStyle.FontHeight.Points), false, false, false, false);
                                }
                            }
                        }
                        if (e1 is XSDRPageBreak)
                        {
                            AddPageBreakToBody(body);
                        }
                    }
                }
            }
        }

        public void AddPageBreakToBody(Body body)
        {
            var paragraph = body.AppendChild(new Paragraph());
            var run = paragraph.AppendChild(new Run());

            run.AppendChild(new Break() { Type = BreakValues.Page });
        }

        public void AddRunToParagraph(Paragraph paragraph, string text, string fontName, int fontHeight, bool isItalic = false, bool isBold = false, bool isUnderlined = false, bool isStruckThrough = false)
        {
            var run = paragraph.AppendChild(new Run());
            var runProperties = new RunProperties(new RunFonts() { Ascii = fontName }, _getFontSize(fontHeight));

            if (isItalic)
            {
                runProperties.Append(new Italic());
            }
            if (isBold)
            {
                runProperties.Append(new Bold());
            }
            if (isUnderlined)
            {
                runProperties.Append(new Underline());
            }
            if (isStruckThrough)
            {
                runProperties.Append(new Strike());
            }

            run.PrependChild(runProperties);
            run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
        }
    }
}
