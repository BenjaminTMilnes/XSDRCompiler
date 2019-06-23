using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDROrderedList : XSDRContentElement
    {
        public XSDROrderedList()
        {
            ElementNames = new string[] { "ordered-list", "ol" };
        }
    }
}
