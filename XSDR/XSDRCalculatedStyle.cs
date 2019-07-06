using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRCalculatedStyle
    {
        public XSDRFontStyle FontStyle { get; set; }
        public XSDRLength ParagraphIndentation { get; set; }

        public XSDRCalculatedStyle()
        {
            FontStyle = new XSDRFontStyle();
            ParagraphIndentation = XSDRLength.FromText("0pt");
        }
    }
}
