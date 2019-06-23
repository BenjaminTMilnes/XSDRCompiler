using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR.Bibliography
{
    public class XSDRWebsiteReference : XSDRReference
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public DateTime DateAccessed { get; set; }
    }
}
