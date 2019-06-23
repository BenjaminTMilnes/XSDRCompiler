using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR.Bibliography
{
    public abstract class XSDRReference
    {
        public string Name { get; set; }
        public IList<XSDRContributor> Contributors { get; set; }

        public XSDRReference()
        {
            Contributors = new List<XSDRContributor>();
        }
    }
}
