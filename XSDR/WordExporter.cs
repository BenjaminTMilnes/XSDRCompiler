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

                            ExportXSDRPageElements(body, paragraph, e1 as XSDRContentElement, (e1 as XSDRContentElement).Subelements);
                        }
                        if (e1 is XSDRPageBreak)
                        {
                            AddPageBreakToBody(body);
                        }
                    }
                }
            }
        }

        protected void ExportXSDRPageElements(Body body, Paragraph paragraph, XSDRContentElement container, IEnumerable<IXSDRPageElement> xsdrPageElements)
        {
            foreach (var xsdrPageElement in xsdrPageElements)
            {
                ExportXSDRPageElement(body, paragraph, container, xsdrPageElement);
            }
        }

        protected void ExportXSDRPageElement(Body body, Paragraph paragraph, XSDRContentElement container, IXSDRPageElement xsdrPageElement)
        {
            if (xsdrPageElement is XSDRTextElement) { ExportXSDRTextElement(body, paragraph, container, xsdrPageElement as XSDRTextElement); }
            if (xsdrPageElement is XSDRItalic) { ExportXSDRItalic(body, paragraph, container, xsdrPageElement as XSDRItalic); }
            if (xsdrPageElement is XSDRBold) { ExportXSDRBold(body, paragraph, container, xsdrPageElement as XSDRBold); }
            if (xsdrPageElement is XSDRUnderline) { ExportXSDRUnderline(body, paragraph, container, xsdrPageElement as XSDRUnderline); }
            if (xsdrPageElement is XSDRStrikethrough) { ExportXSDRStrikethrough(body, paragraph, container, xsdrPageElement as XSDRStrikethrough); }
            if (xsdrPageElement is XSDRCitation) { ExportXSDRCitation(body, paragraph, container, xsdrPageElement as XSDRCitation); }
        }

        protected void ExportXSDRTextElement(Body body, Paragraph paragraph, XSDRContentElement container, XSDRTextElement xsdrTextElement)
        {
            var text = xsdrTextElement.Text;

            AddRunToParagraph(paragraph, text, container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRItalic(Body body, Paragraph paragraph, XSDRContentElement container, XSDRItalic xsdrItalic)
        {
            var text = (xsdrItalic.Subelements[0] as XSDRTextElement).Text;

            AddRunToParagraph(paragraph, text, container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRBold(Body body, Paragraph paragraph, XSDRContentElement container, XSDRBold xsdrBold)
        {
            var text = (xsdrBold.Subelements[0] as XSDRTextElement).Text;

            AddRunToParagraph(paragraph, text, container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRUnderline(Body body, Paragraph paragraph, XSDRContentElement container, XSDRUnderline xsdrUnderline)
        {
            var text = (xsdrUnderline.Subelements[0] as XSDRTextElement).Text;

            AddRunToParagraph(paragraph, text, container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRStrikethrough(Body body, Paragraph paragraph, XSDRContentElement container, XSDRStrikethrough xsdrStrikethrough)
        {
            var text = (xsdrStrikethrough.Subelements[0] as XSDRTextElement).Text;

            AddRunToParagraph(paragraph, text, container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRCitation(Body body, Paragraph paragraph, XSDRContentElement container, XSDRCitation xsdrCitation)
        {
            var text = " [" + xsdrCitation.Number + "]";

            AddRunToParagraph(paragraph, text,  container.CalculatedStyle.FontStyle);
        }

        public void AddPageBreakToBody(Body body)
        {
            var paragraph = body.AppendChild(new Paragraph());
            var run = paragraph.AppendChild(new Run());

            run.AppendChild(new Break() { Type = BreakValues.Page });
        }

        public void AddRunToParagraph(Paragraph paragraph, string text,  XSDRFontStyle fontStyle)
        {
            var run = paragraph.AppendChild(new Run());
            var runProperties = new RunProperties(new RunFonts() { Ascii = fontStyle.FontName }, _getFontSize((int)Math.Round(fontStyle.FontHeight.Points)));

            if ( fontStyle.FontAngle == XSDRFontAngle.Italic)
            {
                runProperties.Append(new Italic());
            }
            if ( fontStyle.FontWeight == XSDRFontWeight.Bold)
            {
                runProperties.Append(new Bold());
            }
            if (  fontStyle.UnderlineStyle == XSDRUnderlineStyle.Underlined)
            {
                runProperties.Append(new Underline());
            }
            if (fontStyle.StrikethroughStyle == XSDRStrikethroughStyle.Strikethrough)
            {
                runProperties.Append(new Strike());
            }

            run.PrependChild(runProperties);
            run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
        }
    }
}
