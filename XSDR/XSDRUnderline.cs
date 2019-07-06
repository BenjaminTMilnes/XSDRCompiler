using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRUnderline : XSDRContentElement
    {
        public XSDRUnderline()
        {
            ElementNames = new string[] { "underline", "u" };

            CalculatedStyle.FontStyle.UnderlineStyle = XSDRUnderlineStyle.Underlined;
        }
    }
}
