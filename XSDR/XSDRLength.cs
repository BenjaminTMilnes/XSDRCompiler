using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace XSDR
{
    public class XSDRLength
    {
        private double _points;

        public static XSDRLength operator +(XSDRLength length1, XSDRLength length2)
        {
            return new XSDRLength(length1.Points + length2.Points);
        }

        public static XSDRLength operator -(XSDRLength length1, XSDRLength length2)
        {
            return new XSDRLength(length1.Points - length2.Points);
        }

        public static XSDRLength operator *(XSDRLength length1, double scalar1)
        {
            return new XSDRLength(length1.Points * scalar1);
        }

        public static XSDRLength operator *(double scalar1, XSDRLength length1)
        {
            return length1 * scalar1;
        }

        public XSDRLength Times(double scalar)
        {
            return this * scalar;
        }

        public double MSWUnits
        {
            get { return _points * 20.0; }
            set { _points = value / 20.0; }
        }

        public double Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public double Picas
        {
            get { return _points / 12.0; }
            set { _points = value * 12.0; }
        }

        public double Inches
        {
            get { return _points / 72.0; }
            set { _points = value * 72.0; }
        }

        public double Millimetres
        {
            get { return Inches * 25.4; }
            set { Inches = value / 25.4; }
        }

        public double Centimetres
        {
            get { return Millimetres / 10.0; }
            set { Millimetres = value * 10.0; }
        }

        public double Decimetres
        {
            get { return Centimetres / 10.0; }
            set { Centimetres = value * 10.0; }
        }

        public double Metres
        {
            get { return Decimetres / 10.0; }
            set { Decimetres = value * 10.0; }
        }

        public XSDRLength() { }

        public XSDRLength(double points)
        {
            _points = points;
        }

        public static XSDRLength Zero { get { return new XSDRLength(0); } }

        public static XSDRLength FromText(string text)
        {
            var r = new Regex(@"^([\d]+(\.[\d]+)?)[\s]*(pt|pc|in|mm|cm|dm|m)$");

            var m = r.Match(text.Trim());

            if (m.Success)
            {
                var magnitude = m.Groups[1].Value;
                var units = m.Groups[3].Value;

                var length = new XSDRLength();

                if (units == "pt")
                {
                    length.Points = double.Parse(magnitude);
                }
                if (units == "pc")
                {
                    length.Picas = double.Parse(magnitude);
                }
                if (units == "in")
                {
                    length.Inches = double.Parse(magnitude);
                }
                if (units == "mm")
                {
                    length.Millimetres = double.Parse(magnitude);
                }
                if (units == "cm")
                {
                    length.Centimetres = double.Parse(magnitude);
                }
                if (units == "dm")
                {
                    length.Decimetres = double.Parse(magnitude);
                }
                if (units == "m")
                {
                    length.Metres = double.Parse(magnitude);
                }

                return length;
            }

            throw new ArgumentException($"'{text}' is not a valid length unit.");
        }
    }
}
