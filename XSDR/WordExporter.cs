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
    public class WordExportContext
    {
        private WordprocessingDocument _document;
        private MainDocumentPart _mainPart;

        private SectionProperties _currentSectionProperties;
        private ParagraphProperties _currentParagraphProperties;
        private RunProperties _currentRunProperties;

        private Body _body;
        private Paragraph _currentParagraph;

        public WordExportContext(WordprocessingDocument document)
        {
            _document = document;

            _mainPart = _document.AddMainDocumentPart();
            _mainPart.Document = new Document();

            var header = _mainPart.AddNewPart<HeaderPart>();
            var footer = _mainPart.AddNewPart<FooterPart>();

            _body = _mainPart.Document.AppendChild(new Body());

            _currentSectionProperties = new SectionProperties();

            _body.Append(_currentSectionProperties);
        }

        private FontSize _getFontSize(int fontHeight)
        {
            return new FontSize() { Val = (fontHeight * 2).ToString() };
        }

        private int ConvertFromMillimetres(double mm)
        {
            return (int)Math.Round(mm * 72 * 20 / 25.4);
        }

        public void SetPageSizeForCurrentSection(double width = 128.5, double height = 198.4)
        {
            var pageSize = new PageSize();

            pageSize.Width = (uint)ConvertFromMillimetres(width);
            pageSize.Height = (uint)ConvertFromMillimetres(height);

            _currentSectionProperties.Append(pageSize);
        }

        public void SetPageMarginForCurrentSection(double top = 20, double bottom = 20, double left = 20, double right = 20)
        {
            var pageMargin = new PageMargin();

            pageMargin.Top = ConvertFromMillimetres(top);
            pageMargin.Bottom = ConvertFromMillimetres(bottom);
            pageMargin.Left = (uint)ConvertFromMillimetres(left);
            pageMargin.Right = (uint)ConvertFromMillimetres(right);

            _currentSectionProperties.Append(pageMargin);
        }

        public void BeginNewParagraph()
        {
            _currentParagraphProperties = new ParagraphProperties();
            _currentParagraph = _body.AppendChild(new Paragraph(_currentParagraphProperties));
        }

        public void SetIndentationForCurrentParagraph(XSDRLength length)
        {
            _currentParagraphProperties.Append(new Indentation() { Hanging = length.Times(-1).MSWUnits.ToString() });
        }

        public void SetJustificationForCurrentParagraph()
        {
            _currentParagraphProperties.Append(new Justification() { Val = JustificationValues.Both });
        }

        public void SetFontStyle(XSDRFontStyle fontStyle)
        {
            var fontSize = _getFontSize((int)Math.Round(fontStyle.FontHeight.Points));

            var runFonts = new RunFonts();

            runFonts.Ascii = fontStyle.FontName;

            _currentRunProperties = new RunProperties(runFonts, fontSize);

            if (fontStyle.FontAngle == XSDRFontAngle.Italic)
            {
                _currentRunProperties.Append(new Italic());
            }
            if (fontStyle.FontWeight == XSDRFontWeight.Bold)
            {
                _currentRunProperties.Append(new Bold());
            }
            if (fontStyle.UnderlineStyle == XSDRUnderlineStyle.Underlined)
            {
                _currentRunProperties.Append(new Underline());
            }
            if (fontStyle.StrikethroughStyle == XSDRStrikethroughStyle.Strikethrough)
            {
                _currentRunProperties.Append(new Strike());
            }
        }

        public void AddTextToParagraph(string text)
        {
            var run = _currentParagraph.AppendChild(new Run());

            run.PrependChild(_currentRunProperties);
            run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
        }

        public void AddPageBreakToBody()
        {
            var paragraph = _body.AppendChild(new Paragraph());
            var run = paragraph.AppendChild(new Run());

            run.AppendChild(new Break() { Type = BreakValues.Page });

        }
    }

    public class WordExporter
    {
        public void ExportXSDRDocument(XSDRDocument document, string filePath)
        {
            OpenSettings openSettings = new OpenSettings();

            openSettings.MarkupCompatibilityProcessSettings = new MarkupCompatibilityProcessSettings(MarkupCompatibilityProcessMode.ProcessAllParts, FileFormatVersions.Office2013);

            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var context = new WordExportContext(wordDocument);

                var n = 1;

                context.SetPageSizeForCurrentSection();
                context.SetPageMarginForCurrentSection();

                foreach (var s in document.Sections)
                {
                    foreach (var e1 in s.Subelements)
                    {
                        if (e1 is XSDRParagraph || e1 is XSDRHeading)
                        {
                            context.BeginNewParagraph();

                            context.SetIndentationForCurrentParagraph((e1 as XSDRContentElement).CalculatedStyle.ParagraphIndentation);
                            context.SetJustificationForCurrentParagraph();

                            context.SetFontStyle((e1 as XSDRContentElement).CalculatedStyle.FontStyle);

                            ExportXSDRPageElements(context, e1 as XSDRContentElement, (e1 as XSDRContentElement).Subelements);
                        }
                        /*   if (e1 is XSDRUnorderedList || e1 is XSDROrderedList)
                           {
                               var numberingPart = mainPart.AddNewPart<NumberingDefinitionsPart>("n" + n);

                               var numberingFormat = new NumberingFormat() { Val = NumberFormatValues.Bullet };
                               var levelText = new LevelText() { Val = "-" };
                               var level = new Level(numberingFormat, levelText) { LevelIndex = 0 };

                               var abstractNum = new AbstractNum(level) { AbstractNumberId = 1 };
                               var abstractNumId = new AbstractNumId() { Val = 1 };
                               var numberingInstance = new NumberingInstance(abstractNumId) { NumberID = 1 };
                               var numbering = new Numbering(abstractNum, numberingInstance);

                               numbering.Save(numberingPart);

                               foreach (var e2 in (e1 as XSDRContentElement).Subelements)
                               {
                                   if (e2 is XSDRListItem)
                                   {
                                       var paragraphProperties = new ParagraphProperties();

                                       var numberingLevelReference = new NumberingLevelReference() { Val = 0 };
                                       var numberingId = new NumberingId() { Val = 1 };
                                       var numberingProperties = new NumberingProperties(numberingLevelReference, numberingId);

                                       paragraphProperties.Append(numberingProperties);

                                       var paragraph = body.AppendChild(new Paragraph(paragraphProperties));

                                       ExportXSDRPageElements(body, paragraph, e2 as XSDRContentElement, (e2 as XSDRContentElement).Subelements);
                                   }
                               }
                           }*/
                        if (e1 is XSDRPageBreak)
                        {
                            context.AddPageBreakToBody();
                        }
                    }
                }
            }
        }

        protected void ExportXSDRPageElements(WordExportContext context, XSDRContentElement container, IEnumerable<IXSDRPageElement> xsdrPageElements)
        {
            foreach (var xsdrPageElement in xsdrPageElements)
            {
                ExportXSDRPageElement(context, container, xsdrPageElement);
            }
        }

        protected void ExportXSDRPageElement(WordExportContext context, XSDRContentElement container, IXSDRPageElement xsdrPageElement)
        {
            if (xsdrPageElement is XSDRTextElement) { ExportXSDRTextElement(context, xsdrPageElement as XSDRTextElement); }
            if (xsdrPageElement is XSDRItalic) { ExportXSDRItalic(context, container, xsdrPageElement as XSDRItalic); }
            if (xsdrPageElement is XSDRBold) { ExportXSDRBold(context, container, xsdrPageElement as XSDRBold); }
            if (xsdrPageElement is XSDRUnderline) { ExportXSDRUnderline(context, container, xsdrPageElement as XSDRUnderline); }
            if (xsdrPageElement is XSDRStrikethrough) { ExportXSDRStrikethrough(context, container, xsdrPageElement as XSDRStrikethrough); }
            if (xsdrPageElement is XSDRCitation) { ExportXSDRCitation(context, container, xsdrPageElement as XSDRCitation); }
        }

        protected void ExportXSDRTextElement(WordExportContext context, XSDRTextElement xsdrTextElement)
        {
            context.AddTextToParagraph(xsdrTextElement.Text);
        }

        protected void ExportXSDRItalic(WordExportContext context, XSDRContentElement container, XSDRItalic xsdrItalic)
        {
            context.SetFontStyle(xsdrItalic.CalculatedStyle.FontStyle);

            ExportXSDRPageElements(context, xsdrItalic, xsdrItalic.Subelements);

            context.SetFontStyle(container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRBold(WordExportContext context, XSDRContentElement container, XSDRBold xsdrBold)
        {
            context.SetFontStyle(xsdrBold.CalculatedStyle.FontStyle);

            ExportXSDRPageElements(context, xsdrBold, xsdrBold.Subelements);

            context.SetFontStyle(container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRUnderline(WordExportContext context, XSDRContentElement container, XSDRUnderline xsdrUnderline)
        {
            context.SetFontStyle(xsdrUnderline.CalculatedStyle.FontStyle);

            ExportXSDRPageElements(context, xsdrUnderline, xsdrUnderline.Subelements);

            context.SetFontStyle(container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRStrikethrough(WordExportContext context, XSDRContentElement container, XSDRStrikethrough xsdrStrikethrough)
        {
            context.SetFontStyle(xsdrStrikethrough.CalculatedStyle.FontStyle);

            ExportXSDRPageElements(context, xsdrStrikethrough, xsdrStrikethrough.Subelements);

            context.SetFontStyle(container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRCitation(WordExportContext context, XSDRContentElement container, XSDRCitation xsdrCitation)
        {
            var text = " [" + xsdrCitation.Number + "]";

            context.SetFontStyle(container.CalculatedStyle.FontStyle);
            context.AddTextToParagraph(text);
        }
    }
}
