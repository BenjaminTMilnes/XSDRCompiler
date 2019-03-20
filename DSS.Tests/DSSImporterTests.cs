using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSS.Tests
{
    [TestClass]
    public class DSSImporterTests
    {
        private DSSImporter _dssImporter;

        public DSSImporterTests()
        {
            _dssImporter = new DSSImporter();
        }

        [TestMethod]
        public void GetPropertyTest1()
        {
            var styleRule = new DSSStyleRule();

            _dssImporter.GetProperty("page-width: 20cm;", styleRule, new Marker());

            Assert.AreEqual("page-width", styleRule.Properties[0].Name);
            Assert.AreEqual("20cm", styleRule.Properties[0].Value);
        }

        [TestMethod]
        public void GetPropertyTest2()
        {
            var styleRule = new DSSStyleRule();

            _dssImporter.GetProperty(" page-width: 20cm;", styleRule, new Marker());

            Assert.AreEqual("page-width", styleRule.Properties[0].Name);
            Assert.AreEqual("20cm", styleRule.Properties[0].Value);
        }

        [TestMethod]
        public void GetPropertyTest3()
        {
            var styleRule = new DSSStyleRule();

            _dssImporter.GetProperty("page-width   : 20cm;", styleRule, new Marker());

            Assert.AreEqual("page-width", styleRule.Properties[0].Name);
            Assert.AreEqual("20cm", styleRule.Properties[0].Value);
        }

        [TestMethod]
        public void GetPropertyTest4()
        {
            var styleRule = new DSSStyleRule();

            _dssImporter.GetProperty("   page-   width   : 20cm;", styleRule, new Marker());

            Assert.AreEqual("page-   width", styleRule.Properties[0].Name);
            Assert.AreEqual("20cm", styleRule.Properties[0].Value);
        }

        [TestMethod]
        public void GetPropertyTest5()
        {
            var styleRule = new DSSStyleRule();

            _dssImporter.GetProperty("page-width: 20cm     ;", styleRule, new Marker());

            Assert.AreEqual("page-width", styleRule.Properties[0].Name);
            Assert.AreEqual("20cm", styleRule.Properties[0].Value);
        }

        [TestMethod]
        public void GetPropertyTest6()
        {
            var styleRule = new DSSStyleRule();

            _dssImporter.GetProperty("page-width: 20   cm;", styleRule, new Marker());

            Assert.AreEqual("page-width", styleRule.Properties[0].Name);
            Assert.AreEqual("20   cm", styleRule.Properties[0].Value);
        }

        [TestMethod]
        public void GetPropertyTest7()
        {
            var dss = "page-width: 20cm; page-height: 30cm;";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetProperty(dss, styleRule, marker);
            _dssImporter.GetProperty(dss, styleRule, marker);

            Assert.AreEqual("page-width", styleRule.Properties[0].Name);
            Assert.AreEqual("20cm", styleRule.Properties[0].Value);
            Assert.AreEqual("page-height", styleRule.Properties[1].Name);
            Assert.AreEqual("30cm", styleRule.Properties[1].Value);
        }

        [TestMethod]
        public void GetClassSelectorTest1()
        {
            var dss = ".bluebox";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetClassSelector(dss, styleRule, marker);

            Assert.AreEqual("bluebox", (styleRule.Selectors[0] as DSSClassSelector).Class);
        }

        [TestMethod]
        public void GetClassSelectorTest2()
        {
            var dss = ".bluebox   ";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetClassSelector(dss, styleRule, marker);

            Assert.AreEqual("bluebox", (styleRule.Selectors[0] as DSSClassSelector).Class);
        }

        [TestMethod]
        public void GetClassSelectorTest3()
        {
            var dss = ".bluebox.redbox";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetClassSelector(dss, styleRule, marker);

            Assert.AreEqual("bluebox", (styleRule.Selectors[0] as DSSClassSelector).Class);
        }

        [TestMethod]
        public void GetClassSelectorTest4()
        {
            var dss = " .bluebox";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetClassSelector(dss, styleRule, marker);

            Assert.AreEqual(0, styleRule.Selectors.Count);
        }

        [TestMethod]
        public void GetElementNameSelectorTest1()
        {
            var dss = "h1";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetElementNameSelector(dss, styleRule, marker);

            Assert.AreEqual("h1", (styleRule.Selectors[0] as DSSElementNameSelector).ElementName);
        }

        [TestMethod]
        public void GetElementNameSelectorTest2()
        {
            var dss = "h1   ";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetElementNameSelector(dss, styleRule, marker);

            Assert.AreEqual("h1", (styleRule.Selectors[0] as DSSElementNameSelector).ElementName);
        }

        [TestMethod]
        public void GetElementNameSelectorTest3()
        {
            var dss = "h1?";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetElementNameSelector(dss, styleRule, marker);

            Assert.AreEqual("h1", (styleRule.Selectors[0] as DSSElementNameSelector).ElementName);
        }

        [TestMethod]
        public void GetElementNameSelectorTest4()
        {
            var dss = ".h1";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetElementNameSelector(dss, styleRule, marker);

            Assert.AreEqual(0, styleRule.Selectors.Count);
        }

        [TestMethod]
        public void GetElementNameSelectorTest5()
        {
            var dss = " h1";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetElementNameSelector(dss, styleRule, marker);

            Assert.AreEqual(0, styleRule.Selectors.Count);
        }
    }
}
