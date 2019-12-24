using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace XSDR
{
    public class XSDRMargin
    {
        public XSDRLength Top { get; set; }
        public XSDRLength Bottom { get; set; }
        public XSDRLength Left { get; set; }
        public XSDRLength Right { get; set; }

        public XSDRMargin(XSDRLength top, XSDRLength right, XSDRLength bottom, XSDRLength left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public static XSDRMargin Zero { get { return new XSDRMargin(XSDRLength.Zero, XSDRLength.Zero, XSDRLength.Zero, XSDRLength.Zero); } }

        public static XSDRMargin FromText(string text)
        {
            text = text.Trim();

            var r1 = new Regex(@"^([\d]+(\.[\d]+)?)[\s]*(pt|pc|in|mm|cm|dm|m)$");
            var r2 = new Regex(@"^([\d]+(\.[\d]+)?)[\s]*(pt|pc|in|mm|cm|dm|m)[\s]+([\d]+(\.[\d]+)?)[\s]*(pt|pc|in|mm|cm|dm|m)$");
            var r4 = new Regex(@"^([\d]+(\.[\d]+)?)[\s]*(pt|pc|in|mm|cm|dm|m)[\s]+([\d]+(\.[\d]+)?)[\s]*(pt|pc|in|mm|cm|dm|m)[\s]+([\d]+(\.[\d]+)?)[\s]*(pt|pc|in|mm|cm|dm|m)[\s]+([\d]+(\.[\d]+)?)[\s]*(pt|pc|in|mm|cm|dm|m)$");

            if (r1.IsMatch(text))
            {
                var l1 = XSDRLength.FromText(text);

                var margin = new XSDRMargin(l1, l1, l1, l1);

                return margin;
            }
            else if (r2.IsMatch(text))
            {
                var m = r2.Match(text);

                var s1 = m.Groups[1].Value;
                var u1 = m.Groups[3].Value;
                var l1 = XSDRLength.FromText(s1 + u1);

                var s2 = m.Groups[4].Value;
                var u2 = m.Groups[6].Value;
                var l2 = XSDRLength.FromText(s2 + u2);

                var margin = new XSDRMargin(l1, l2, l1, l2);

                return margin;
            }
            else if (r4.IsMatch(text))
            {
                var m = r4.Match(text);

                var s1 = m.Groups[1].Value;
                var u1 = m.Groups[3].Value;
                var l1 = XSDRLength.FromText(s1 + u1);

                var s2 = m.Groups[4].Value;
                var u2 = m.Groups[6].Value;
                var l2 = XSDRLength.FromText(s2 + u2);

                var s3 = m.Groups[7].Value;
                var u3 = m.Groups[9].Value;
                var l3 = XSDRLength.FromText(s3 + u3);

                var s4 = m.Groups[10].Value;
                var u4 = m.Groups[12].Value;
                var l4 = XSDRLength.FromText(s4 + u4);

                var margin = new XSDRMargin(l1, l2, l3, l4);

                return margin;
            }

            return Zero;
        }
    }
}
