using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XSDR.Tests
{
    [TestClass]
    public class XSDRLengthTests
    {
        [TestMethod]
        public void FromTextTest1()
        {
            var length = XSDRLength.FromText("12pt");

            Assert.AreEqual(12.0, length.Points);
        }

        [TestMethod]
        public void FromTextTest2()
        {
            var length = XSDRLength.FromText("12.0pt");

            Assert.AreEqual(12.0, length.Points);
        }

        [TestMethod]
        public void FromTextTest3()
        {
            var length = XSDRLength.FromText("12.000pt");

            Assert.AreEqual(12.0, length.Points);
        }

        [TestMethod]
        public void FromTextTest4()
        {
            var length = XSDRLength.FromText("12 pt");

            Assert.AreEqual(12.0, length.Points);
        }

        [TestMethod]
        public void FromTextTest5()
        {
            var length = XSDRLength.FromText("12.000   pt");

            Assert.AreEqual(12.0, length.Points);
        }

        [TestMethod]
        public void FromTextTest6()
        {
            var length = XSDRLength.FromText("12pc");

            Assert.AreEqual(12.0, length.Picas);
        }

        [TestMethod]
        public void FromTextTest7()
        {
            var length = XSDRLength.FromText("0.5in");

            Assert.AreEqual(0.5, length.Inches);
        }

        [TestMethod]
        public void FromTextTest8()
        {
            var length = XSDRLength.FromText("12mm");

            Assert.AreEqual(12.0, length.Millimetres);
        }

        [TestMethod]
        public void FromTextTest9()
        {
            var length = XSDRLength.FromText("2cm");

            Assert.AreEqual(2.0, length.Centimetres);
        }

        [TestMethod]
        public void FromTextTest10()
        {
            var length = XSDRLength.FromText("0.2dm");

            Assert.AreEqual(0.2, length.Decimetres);
        }

        [TestMethod]
        public void FromTextTest11()
        {
            var length = XSDRLength.FromText("0.05m");

            Assert.AreEqual(0.05, length.Metres);
        }
    }
}
