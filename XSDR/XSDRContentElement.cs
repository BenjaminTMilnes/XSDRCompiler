using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public abstract class XSDRContentElement : IXSDRPageElement
    {
            public IList<IXSDRPageElement> Subelements { get; set; }

          public XSDRContentElement()
        {
            Subelements = new List<IXSDRPageElement>();
        }
    }
}
