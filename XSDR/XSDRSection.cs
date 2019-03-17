using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRSection
    {
        public string PageTemplateReference { get; set; }
        public IList<IXSDRPageElement> Subelements { get; set; }

        public XSDRSection()
        {
            Subelements = new List<IXSDRPageElement>();
        }
    }
}
