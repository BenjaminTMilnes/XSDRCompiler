using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRSection
    {
        private XSDRDocument _document;

        public string PageTemplateReference { get; set; }
        public XSDRPageTemplate PageTemplate { get { return _document.Templates.FirstOrDefault(t => t is XSDRPageTemplate && t.Reference == PageTemplateReference) as XSDRPageTemplate; } }

        public XSDRPageSize PageSize { get; set; }
        public XSDRMargin PageMargin { get; set; }

        public IList<IXSDRPageElement> Subelements { get; set; }

        public XSDRSection(XSDRDocument document)
        {
            _document = document;

            PageSize = new XSDRPageSize();
            PageMargin = XSDRMargin.Zero;

            Subelements = new List<IXSDRPageElement>();
        }
    }
}
