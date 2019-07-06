using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSS;

namespace XSDR
{
    public abstract class XSDRContentElement : IXSDRPageElement
    {
        public string[] ElementNames { get; protected set; }
        public IList<IXSDRPageElement> Subelements { get; set; }

        public string Class { get; set; }
        public DSSStyleRule Style { get; set; }

        public XSDRCalculatedStyle CalculatedStyle { get; set; }

        public XSDRContentElement()
        {
            ElementNames = new string[] { };
            Subelements = new List<IXSDRPageElement>();
            Style = new DSSStyleRule();
            CalculatedStyle = new XSDRCalculatedStyle();
        }
    }
}
