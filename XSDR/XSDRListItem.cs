using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRListItem : XSDRContentElement
    {
        public XSDRListItem()
        {
            ElementNames = new string[] { "list-item", "li" };
        }
    }
}
