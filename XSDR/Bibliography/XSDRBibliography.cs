using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR.Bibliography
{
    public class XSDRBibliography
    {
        public IList<XSDRReference> References { get; set; }
        public IList<string> ReferenceOrder { get; set; }

        public XSDRBibliography()
        {
            References = new List<XSDRReference>();
            ReferenceOrder = new List<string>();
        }
    }
}
