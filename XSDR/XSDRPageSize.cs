using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRPageSize
    {
        public XSDRLength Width { get; set; }
        public XSDRLength Height { get; set; }

        public XSDRPageSize(string width = "2cm", string height = "2cm")
        {
            Width = XSDRLength.FromText(width);
            Height = XSDRLength.FromText(height);
        }
    }
}
