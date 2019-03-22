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

            if (styleRule.Selectors.Any() && styleRule.Selectors[0] is DSSElementNameSelector && element.ElementNames.Any(name => name == (styleRule.Selectors[0] as DSSElementNameSelector).ElementName))
            {
                isMatchingElement = true;
            }

            if (isMatchingElement)
            {
                foreach (var property in styleRule.Properties)
                {
                    if (property.Name == "font-name")
                    {
                        element.CalculatedStyle.FontName = property.Value;
                    }
                    if (property.Name == "font-height")
                    {
                        element.CalculatedStyle.FontHeight = XSDRLength.FromText(property.Value);
                    }
                }
            }

            ResolveElements(styleRule, element.Subelements);
        }
    }
}
