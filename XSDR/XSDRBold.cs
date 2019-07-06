using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRBold : XSDRContentElement
    {
        public XSDRBold()
        {
            ElementNames = new string[] { "bold", "b" };

            CalculatedStyle.FontStyle.FontWeight = XSDRFontWeight.Bold;
        }
    }
}
