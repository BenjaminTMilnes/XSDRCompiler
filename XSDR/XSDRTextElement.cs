using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSDR
{
    public class XSDRTextElement : IXSDRPageElement
    {
        public string Text { get; set; }

        public XSDRTextElement() { }

        public XSDRTextElement(string text)
        {
            Text = text;
        }
    }
}
