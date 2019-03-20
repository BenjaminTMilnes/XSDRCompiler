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

        public void GetElementNameSelector(string dss, DSSStyleRule container, Marker marker)
        {
            if (marker.Position >= dss.Length)
            {
                return;
            }

            var elementName = "";

            for (var i = marker.Position; i < dss.Length; i++)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_0123456789".Any(c => c.ToString() == dss.Substring(i, 1)))
                {
                    elementName += dss.Substring(i, 1);
                }
                else
                {
                    break;
                }
            }

            if (elementName.Length < 1)
            {
                return;
            }

            marker.Position += elementName.Length;

            container.Selectors.Add(new DSSElementNameSelector(elementName));
        }

        public void GetClassSelector(string dss, DSSStyleRule container, Marker marker)
        {
            if (marker.Position >= dss.Length)
            {
                return;
            }

            var className = "";

            if (dss.Substring(marker.Position, 1) != ".")
            {
                return;
            }

            marker.Position += 1;

            for (var i = marker.Position; i < dss.Length; i++)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_0123456789".Any(c => c.ToString() == dss.Substring(i, 1)))
                {
                    className += dss.Substring(i, 1);
                }
                else
                {
                    break;
                }
            }

            marker.Position += className.Length;

            container.Selectors.Add(new DSSClassSelector(className));
        }

        public void GetProperties(string dss, DSSStyleRule container, Marker marker)
        {
            if (marker.Position >= dss.Length)
            {
                return;
            }

            var i = -1;
            var haveHadOpeningBracket = false;
            var haveHadClosingBracket = false;

            while (marker.Position != i)
            {
                i = marker.Position;

                GetWhitespace(dss, marker);

                if (!haveHadOpeningBracket)
                {
                    if (dss.Substring(marker.Position, 1) == "{")
                    {
                        haveHadOpeningBracket = true;
                        marker.Position += 1;
                    }
                }
                else
                {
                    if (dss.Substring(marker.Position, 1) == "{")
                    {
                        break;
                    }

                    GetWhitespace(dss, marker);
                    GetProperty(dss, container, marker);
                    GetWhitespace(dss, marker);

                    if (!haveHadClosingBracket)
                    {
                        if (dss.Substring(marker.Position, 1) == "}")
                        {
                            haveHadClosingBracket = true;
                            marker.Position += 1;
                            break;
                        }
                    }
                }
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

                if (c == "{" || c == "}" || c == ";")
                {
                    return;
                }
                if (c == ":")
                {
                    marker.Position += 1;
                    break;
                }

                propertyName += c;

                marker.Position += 1;
            }

            while (marker.Position < dss.Length)
            {
                var c = dss.Substring(marker.Position, 1);

                if (c == "{" || c == "}" || c == ":")
                {
                    return;
                }
                if (c == ";")
                {
                    marker.Position += 1;
                    break;
                }

                propertyValue += c;

                marker.Position += 1;
            }

            var property = new DSSProperty(propertyName.Trim(), propertyValue.Trim());

            container.Properties.Add(property);
        }

        public void GetWhitespace(string dss, Marker marker)
        {
            for (var i = marker.Position; i < dss.Length; i++)
            {
                if (" \t\n".Any(c => c.ToString() == dss.Substring(i, 1)))
                {
                    marker.Position += 1;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
