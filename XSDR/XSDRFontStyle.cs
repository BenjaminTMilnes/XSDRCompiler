using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public enum XSDRFontAngle
    {
        Normal = 0,
        Italic = 1
    }

    public enum XSDRFontWeight
    {
        Normal = 0,
        Bold = 1
    }

    public enum XSDRUnderlineStyle
    {
        None = 0,
        Underlined = 1
    }

    public enum XSDRStrikethroughStyle
    {
        None = 0,
        Strikethrough = 1
    }

    public class XSDRFontStyle
    {
        public string FontName { get; set; }
        public XSDRLength FontHeight { get; set; }
        public XSDRFontAngle FontAngle { get; set; }
        public XSDRFontWeight FontWeight { get; set; }
        public XSDRUnderlineStyle UnderlineStyle { get; set; }
        public XSDRStrikethroughStyle StrikethroughStyle { get; set; }

        public XSDRFontStyle(string fontName = "Times New Roman", string fontHeight = "10pt", XSDRFontAngle fontAngle = XSDRFontAngle.Normal, XSDRFontWeight fontWeight = XSDRFontWeight.Normal, XSDRUnderlineStyle underlineStyle = XSDRUnderlineStyle.None, XSDRStrikethroughStyle strikethroughStyle = XSDRStrikethroughStyle.None)
        {
            FontName = fontName;
            FontHeight = XSDRLength.FromText(fontHeight);
            FontAngle = fontAngle;
            FontWeight = fontWeight;
            UnderlineStyle = underlineStyle;
            StrikethroughStyle = strikethroughStyle;
        }
    }
}
