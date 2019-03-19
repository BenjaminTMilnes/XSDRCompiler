using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS
{
    public class DSSStyleRule
    {
        public IList<IDSSSelector> Selectors { get; set; }
        public IList<DSSProperty> Properties { get; set; }

        public DSSStyleRule()
        {
            Selectors = new List<IDSSSelector>();
            Properties = new List<DSSProperty>();
        }
    }
}
