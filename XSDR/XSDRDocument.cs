using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSDR.Bibliography;

namespace XSDR
{
    public class XSDRDocument
    {
        public string Version { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public IEnumerable<string> Keywords { get; set; }
        public IEnumerable<XSDRContributor> Contributors { get; set; }
        public DateTime PublicationDate { get; set; }
        public XSDRBibliography Bibliography { get; set; }
        public IList<XSDRTemplate> Templates { get; set; }
        public IList<XSDRSection> Sections { get; set; }

        public XSDRDocument()
        {
            Keywords = new List<string>();
            Contributors = new List<XSDRContributor>();
            PublicationDate = DateTime.UtcNow;
            Bibliography = new XSDRBibliography();
            Templates = new List<XSDRTemplate>();
            Sections = new List<XSDRSection>();
        }
    }
}
