using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRCalculatedStyle
    {
        public string FontName { get; set; }
        public XSDRLength FontHeight { get; set; }
            public XSDRLength ParagraphIndentation { get; set; }

        public XSDRCalculatedStyle()
        {
            FontName = "Times New Roman";
            FontHeight = XSDRLength.FromText("10pt");
            ParagraphIndentation = XSDRLength.FromText("0pt");
        }
    }
}
