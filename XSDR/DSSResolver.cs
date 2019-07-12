using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSS;

namespace XSDR
{
    public class DSSResolver
    {
        public void ResolveDSS(DSSDocument dssDocument, XSDRDocument xsdrDocument)
        {
            foreach (var styleRule in dssDocument.StyleRules)
            {
                foreach (var section in xsdrDocument.Sections)
                {
                    ResolveElements(styleRule, section.Subelements);
                }
            }

            foreach (var section in xsdrDocument.Sections)
            {
                ApplyInlineStylingToElements(section.Subelements);
            }
        }

        protected void ApplyInlineStylingToElements(IEnumerable<IXSDRPageElement> elements)
        {
            foreach (var element in elements)
            {
                if (element is XSDRContentElement)
                {
                    ApplyInlineStylingToElement(element as XSDRContentElement);
                }
            }
        }

        protected void ApplyInlineStylingToElement(XSDRContentElement element)
        {
            ApplyStyleRulePropertiesToElement(element.Style.Properties, element);

            ApplyInlineStylingToElements(element.Subelements);
        }

        protected void ApplyStyleRulePropertiesToElement(IEnumerable<DSSProperty> properties, XSDRContentElement element)
        {
            foreach (var property in properties)
            {
                if (property.Name == "font-name")
                {
                    element.CalculatedStyle.FontStyle.FontName = property.Value;
                }
                if (property.Name == "font-height")
                {
                    element.CalculatedStyle.FontStyle.FontHeight = XSDRLength.FromText(property.Value);
                }
                if (property.Name == "font-angle")
                {
                    if (property.Value == "italic")
                    {
                        element.CalculatedStyle.FontStyle.FontAngle = XSDRFontAngle.Italic;
                    }
                }
                if (property.Name == "font-weight")
                {
                    if (property.Value == "bold")
                    {
                        element.CalculatedStyle.FontStyle.FontWeight = XSDRFontWeight.Bold;
                    }
                }
                if (property.Name == "paragraph-alignment")
                {
                    if (property.Value == "left")
                    {
                        element.CalculatedStyle.ParagraphAlignment = XSDRParagraphAlignment.LeftAlign;
                    }
                    if (property.Value == "right")
                    {
                        element.CalculatedStyle.ParagraphAlignment = XSDRParagraphAlignment.RightAlign;
                    }
                    if (property.Value == "centred")
                    {
                        element.CalculatedStyle.ParagraphAlignment = XSDRParagraphAlignment.Centred;
                    }
                    if (property.Value == "justified")
                    {
                        element.CalculatedStyle.ParagraphAlignment = XSDRParagraphAlignment.LeftJustified;
                    }
                }
                if (property.Name == "paragraph-indentation")
                {
                    element.CalculatedStyle.ParagraphIndentation = XSDRLength.FromText(property.Value);
                }
            }
        }

        protected void ResolveElements(DSSStyleRule styleRule, IEnumerable<IXSDRPageElement> elements)
        {
            foreach (var element in elements)
            {
                if (element is XSDRContentElement)
                {
                    ResolveElement(styleRule, element as XSDRContentElement);
                }
            }
        }

        protected void ResolveElement(DSSStyleRule styleRule, XSDRContentElement element)
        {
            var isMatchingElement = false;

            if (styleRule.Selectors.Any() && styleRule.Selectors[0] is DSSElementNameSelector && element.ElementNames.Any() && element.ElementNames.Any(name => name == (styleRule.Selectors[0] as DSSElementNameSelector).ElementName))
            {
                isMatchingElement = true;
            }

            if (isMatchingElement)
            {
                ApplyStyleRulePropertiesToElement(styleRule.Properties, element);
            }

            ResolveElements(styleRule, element.Subelements);
        }
    }
}
