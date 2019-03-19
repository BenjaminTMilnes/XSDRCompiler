using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS
{
    public class Marker
    {
        public int Position { get; set; }
    }

    public class DSSImporter
    {
        public void GetStyleRule(string dss, IList<DSSStyleRule> container, Marker marker)
        {
            if (marker.Position >= dss.Length)
            {
                return;
            }
        }

        public void GetProperty(string dss, DSSStyleRule container, Marker marker)
        {
            if (marker.Position >= dss.Length)
            {
                return;
            }

            var propertyName = "";
            var propertyValue = "";

            while (marker.Position < dss.Length)
            {
                var c = dss.Substring(marker.Position, 1);

                if (c == "{" || c == "}")
                {
                    return;
                }
                if (c == ":")
                {
                    break;
                }

                propertyName += c;

                marker.Position += 1;
            }

            while (marker.Position < dss.Length)
            {
                var c = dss.Substring(marker.Position, 1);

                if (c == "{" || c == "}")
                {
                    return;
                }
                if (c == ";")
                {
                    break;
                }

                propertyValue += c;

                marker.Position += 1;
            }

            var property = new DSSProperty(propertyName.Trim(), propertyValue.Trim());

            container.Properties.Add(property);
        }
    }
}
