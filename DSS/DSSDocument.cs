using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS
{
    public class DSSDocument
    {
        public IList<DSSStyleRule> StyleRules { get; set; }

        public DSSDocument()
        {
            StyleRules = new List<DSSStyleRule>();
        }
    }
}
