using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public abstract class XSDRContentElement : IXSDRPageElement
    {
        public string[] ElementNames { get; protected set; }
        public IList<IXSDRPageElement> Subelements { get; set; }

        public string Class { get; set; }

        public XSDRCalculatedStyle CalculatedStyle { get; set; }

        public XSDRContentElement()
        {
            Subelements = new List<IXSDRPageElement>();
            CalculatedStyle = new XSDRCalculatedStyle();
        }
    }
}
