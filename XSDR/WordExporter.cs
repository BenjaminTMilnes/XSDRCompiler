using System;
using System.Collections.Generic;
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

            _body = new Body();

            _mainPart.Document.AppendChild(_body);

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
            _currentParagraph = new Paragraph(_currentParagraphProperties);

            _body.AppendChild(_currentParagraph);
        }

        public void SetIndentationForCurrentParagraph(XSDRLength length)
        {
            var indentation = new Indentation();

            indentation.Hanging = length.Times(-1).MSWUnits.ToString();

            _currentParagraphProperties.Append(indentation);
        }

        public void SetJustificationForCurrentParagraph(XSDRParagraphAlignment paragraphAlignment)
        {
            var justification = new Justification();

            if (paragraphAlignment == XSDRParagraphAlignment.LeftAlign)
            {
                justification.Val = JustificationValues.Left;
            }
            if (paragraphAlignment == XSDRParagraphAlignment.RightAlign)
            {
                justification.Val = JustificationValues.Right;
            }
            if (paragraphAlignment == XSDRParagraphAlignment.Centred)
            {
                justification.Val = JustificationValues.Center;
            }
            if (paragraphAlignment == XSDRParagraphAlignment.LeftJustified)
            {
                justification.Val = JustificationValues.Both;
            }

            _currentParagraphProperties.Append(justification);
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
            var run = new Run();

            _currentParagraph.AppendChild(run);

            run.PrependChild(_currentRunProperties);
            run.AppendChild(new Text() { Text = text, Space = SpaceProcessingModeValues.Preserve });
        }

        public void AddPageBreakToBody()
        {
            var paragraph = new Paragraph();
            var run = new Run();

            _body.AppendChild(paragraph);
            paragraph.AppendChild(run);
            run.AppendChild(new Break() { Type = BreakValues.Page });
        }
        public void AddSectionBreakToBody()
        {
            var paragraph = new Paragraph();
            var paragraphProperties = new ParagraphProperties();
            var sectionProperties = new SectionProperties();
            var sectionType = new SectionType() { Val = SectionMarkValues.NextPage };

            sectionProperties.Append(sectionType);
            paragraphProperties.Append(sectionProperties);
            paragraph.Append(paragraphProperties);

            _body.AppendChild(paragraph);
        }
    }

    public class WordExporter
    {
        public void ExportXSDRDocument(XSDRDocument document, string filePath)
        {
            var openSettings = new OpenSettings();

            openSettings.MarkupCompatibilityProcessSettings = new MarkupCompatibilityProcessSettings(MarkupCompatibilityProcessMode.ProcessAllParts, FileFormatVersions.Office2013);

            using (var wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var context = new WordExportContext(wordDocument);

                var n = 0;

                foreach (var s in document.Sections)
                {
                    if (n > 0)
                    {
                        context.AddSectionBreakToBody();
                    }

                    context.SetPageSizeForCurrentSection(s.PageSize.Width.Millimetres, s.PageSize.Height.Millimetres);
                    context.SetPageMarginForCurrentSection(s.PageMargin.Top.Millimetres, s.PageMargin.Bottom.Millimetres, s.PageMargin.Left.Millimetres, s.PageMargin.Right.Millimetres);

                    foreach (var e1 in s.Subelements)
                    {
                        if (e1 is XSDRParagraph || e1 is XSDRHeading)
                        {
                            var e2 = e1 as XSDRContentElement;

                            context.BeginNewParagraph();

                            context.SetIndentationForCurrentParagraph(e2.CalculatedStyle.ParagraphIndentation);
                            context.SetJustificationForCurrentParagraph(e2.CalculatedStyle.ParagraphAlignment);
                            context.SetFontStyle(e2.CalculatedStyle.FontStyle);

                            ExportXSDRPageElements(context, e2, e2.Subelements);
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

                    n++;
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

            var isInlineElement = (xsdrPageElement is XSDRItalic || xsdrPageElement is XSDRBold || xsdrPageElement is XSDRUnderline || xsdrPageElement is XSDRStrikethrough);

            if (isInlineElement) { ExportXSDRInlineElement(context, container, xsdrPageElement as XSDRContentElement); }

            if (xsdrPageElement is XSDRCitation) { ExportXSDRCitation(context, container, xsdrPageElement as XSDRCitation); }

            if (xsdrPageElement is XSDRPageVariable) { ExportXSDRVariable(context, container, xsdrPageElement as XSDRPageVariable); }
        }

        protected void ExportXSDRTextElement(WordExportContext context, XSDRTextElement xsdrTextElement)
        {
            context.AddTextToParagraph(xsdrTextElement.Text);
        }

        protected void ExportXSDRInlineElement(WordExportContext context, XSDRContentElement container, XSDRContentElement inlineElement)
        {
            context.SetFontStyle(inlineElement.CalculatedStyle.FontStyle);

            ExportXSDRPageElements(context, inlineElement, inlineElement.Subelements);

            context.SetFontStyle(container.CalculatedStyle.FontStyle);
        }

        protected void ExportXSDRCitation(WordExportContext context, XSDRContentElement container, XSDRCitation xsdrCitation)
        {
            var text = " [" + xsdrCitation.Number + "]";

            context.SetFontStyle(container.CalculatedStyle.FontStyle);
            context.AddTextToParagraph(text);
        }

        protected void ExportXSDRVariable(WordExportContext context, XSDRContentElement container, XSDRPageVariable pageVariable)
        {
            context.SetFontStyle(container.CalculatedStyle.FontStyle);
            context.AddTextToParagraph(pageVariable.Value);
        }
    }
}
