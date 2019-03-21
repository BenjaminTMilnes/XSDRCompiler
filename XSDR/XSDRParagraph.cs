using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRParagraph : XSDRContentElement
    {
        public XSDRParagraph()
        {
            ElementNames = new string[] { "paragraph", "p" };
        }
    }
}
