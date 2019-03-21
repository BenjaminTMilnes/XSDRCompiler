using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRHeading : XSDRContentElement
    {
        public int Level { get; protected set; }

        public XSDRHeading(int level = 1)
        {
            Level = level;
            ElementNames = new string[] { $"heading{Level}", $"h{Level}" };
        }
    }
}
