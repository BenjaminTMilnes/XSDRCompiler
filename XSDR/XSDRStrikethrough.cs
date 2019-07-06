using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRStrikethrough : XSDRContentElement
    {
        public XSDRStrikethrough()
        {
            ElementNames = new string[] { "strikethrough", "s" };

            CalculatedStyle.FontStyle.StrikethroughStyle = XSDRStrikethroughStyle.Strikethrough;
        }
    }
}
