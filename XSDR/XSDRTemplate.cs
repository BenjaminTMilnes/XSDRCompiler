using System.Collections.Generic;

namespace XSDR
{
    public abstract class XSDRTemplate
    {
        public string Reference { get; set; }
        public IList<IXSDRPageElement> Subelements { get; set; }

        public XSDRTemplate()
        {
            Subelements = new List<IXSDRPageElement>();
        }
    }
}
