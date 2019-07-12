using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public enum XSDRParagraphAlignment
    {
        LeftAlign = 0,
        RightAlign = 1,
        Centred = 2,
        LeftJustified = 3,
        RightJustified = 4
    }

    public class XSDRCalculatedStyle
    {
        public XSDRFontStyle FontStyle { get; set; }
        public XSDRLength ParagraphIndentation { get; set; }
        public XSDRParagraphAlignment ParagraphAlignment { get; set; }

        public XSDRCalculatedStyle()
        {
            FontStyle = new XSDRFontStyle();
            ParagraphIndentation = XSDRLength.FromText("0pt");
            ParagraphAlignment = XSDRParagraphAlignment.LeftAlign;
        }
    }
}
