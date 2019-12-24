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

    public class XSDRPageSizes
    {
        public static XSDRPageSize A1 { get { return new XSDRPageSize("594mm", "841mm"); } }
        public static XSDRPageSize A2 { get { return new XSDRPageSize("420mm", "594mm"); } }
        public static XSDRPageSize A3 { get { return new XSDRPageSize("297mm", "420mm"); } }
        public static XSDRPageSize A4 { get { return new XSDRPageSize("210mm", "297mm"); } }
        public static XSDRPageSize A5 { get { return new XSDRPageSize("148mm", "210mm"); } }
        public static XSDRPageSize A6 { get { return new XSDRPageSize("105mm", "148mm"); } }

        public static XSDRPageSize FromText(string text)
        {
            if (text.Trim().ToLower() == "a1") { return A1; }
            if (text.Trim().ToLower() == "a2") { return A2; }
            if (text.Trim().ToLower() == "a3") { return A3; }
            if (text.Trim().ToLower() == "a4") { return A4; }
            if (text.Trim().ToLower() == "a5") { return A5; }
            if (text.Trim().ToLower() == "a6") { return A6; }

            return A4;
        }

    }
}
