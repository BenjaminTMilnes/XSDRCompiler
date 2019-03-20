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

        [TestMethod]
        public void GetProperties1()
        {
            var dss = "   {   page-width: 20cm; page-height: 30cm;   }   ";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetProperties(dss, styleRule, marker);

            Assert.AreEqual(2, styleRule.Properties.Count);
            Assert.AreEqual("page-width", styleRule.Properties[0].Name);
            Assert.AreEqual("20cm", styleRule.Properties[0].Value);
            Assert.AreEqual("page-height", styleRule.Properties[1].Name);
            Assert.AreEqual("30cm", styleRule.Properties[1].Value);
        }

        [TestMethod]
        public void GetProperties2()
        {
            var dss = "   {   page-width: 20cm; page-height: 30cm;   font-height   :   10pt;   }   ";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetProperties(dss, styleRule, marker);

            Assert.AreEqual(3, styleRule.Properties.Count);
            Assert.AreEqual("page-width", styleRule.Properties[0].Name);
            Assert.AreEqual("20cm", styleRule.Properties[0].Value);
            Assert.AreEqual("page-height", styleRule.Properties[1].Name);
            Assert.AreEqual("30cm", styleRule.Properties[1].Value);
            Assert.AreEqual("font-height", styleRule.Properties[2].Name);
            Assert.AreEqual("10pt", styleRule.Properties[2].Value);
        }

        [TestMethod]
        public void GetProperties3()
        {
            var dss = "   {   }   page-width: 20cm; page-height: 30cm;   font-height   :   10pt;   }   ";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetProperties(dss, styleRule, marker);

            Assert.AreEqual(0, styleRule.Properties.Count);
        }

        [TestMethod]
        public void GetProperties4()
        {
            var dss = "   {   ;   page-width: 20cm; page-height: 30cm;   font-height   :   10pt;   }   ";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetProperties(dss, styleRule, marker);

            Assert.AreEqual(0, styleRule.Properties.Count);
        }

        [TestMethod]
        public void GetProperties5()
        {
            var dss = "   page-width: 20cm; page-height: 30cm;   font-height   :   10pt;   }   ";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetProperties(dss, styleRule, marker);

            Assert.AreEqual(0, styleRule.Properties.Count);
        }

        [TestMethod]
        public void GetProperties6()
        {
            var dss = "   {{   page-width: 20cm; page-height: 30cm;   font-height   :   10pt;   }   ";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetProperties(dss, styleRule, marker);

            Assert.AreEqual(0, styleRule.Properties.Count);
        }

        [TestMethod]
        public void GetSelectorTest1()
        {
            var dss = "h1";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetSelectors(dss, styleRule, marker);

            Assert.AreEqual(1, styleRule.Selectors.Count);
            Assert.AreEqual("h1", (styleRule.Selectors[0] as DSSElementNameSelector).ElementName);
        }

        [TestMethod]
        public void GetSelectorTest2()
        {
            var dss = "   h1";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetSelectors(dss, styleRule, marker);

            Assert.AreEqual(1, styleRule.Selectors.Count);
            Assert.AreEqual("h1", (styleRule.Selectors[0] as DSSElementNameSelector).ElementName);
        }

        [TestMethod]
        public void GetSelectorTest3()
        {
            var dss = ".bluebox";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetSelectors(dss, styleRule, marker);

            Assert.AreEqual(1, styleRule.Selectors.Count);
            Assert.AreEqual("bluebox", (styleRule.Selectors[0] as DSSClassSelector).Class);
        }

        [TestMethod]
        public void GetSelectorTest4()
        {
            var dss = "   .bluebox   ";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetSelectors(dss, styleRule, marker);

            Assert.AreEqual(1, styleRule.Selectors.Count);
            Assert.AreEqual("bluebox", (styleRule.Selectors[0] as DSSClassSelector).Class);
        }

        [TestMethod]
        public void GetSelectorTest5()
        {
            var dss = "h1.bluebox";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetSelectors(dss, styleRule, marker);

            Assert.AreEqual(2, styleRule.Selectors.Count);
            Assert.AreEqual("h1", (styleRule.Selectors[0] as DSSElementNameSelector).ElementName);
            Assert.AreEqual("bluebox", (styleRule.Selectors[1] as DSSClassSelector).Class);
        }

        [TestMethod]
        public void GetSelectorTest6()
        {
            var dss = "h1.bluebox.redbox";
            var styleRule = new DSSStyleRule();
            var marker = new Marker();

            _dssImporter.GetSelectors(dss, styleRule, marker);

            Assert.AreEqual(3, styleRule.Selectors.Count);
            Assert.AreEqual("h1", (styleRule.Selectors[0] as DSSElementNameSelector).ElementName);
            Assert.AreEqual("bluebox", (styleRule.Selectors[1] as DSSClassSelector).Class);
            Assert.AreEqual("redbox", (styleRule.Selectors[2] as DSSClassSelector).Class);
        }

        [TestMethod]
        public void GetDocumentTest1()
        {
            var dss = "   h1 { font-height: 12pt; font-family: Garamond; } \n p { font-height: 12pt; font-family: Garamond;    }";
            var document = _dssImporter.ImportDocument(dss);

            Assert.AreEqual(2, document.StyleRules.Count);
            Assert.AreEqual(1, document.StyleRules[0].Selectors.Count);
            Assert.AreEqual("h1", (document.StyleRules[0].Selectors[0] as DSSElementNameSelector).ElementName);
            Assert.AreEqual(2, document.StyleRules[0].Properties.Count);
            Assert.AreEqual(1, document.StyleRules[1].Selectors.Count);
            Assert.AreEqual("p", (document.StyleRules[1].Selectors[0] as DSSElementNameSelector).ElementName);
            Assert.AreEqual(2, document.StyleRules[1].Properties.Count);
            Assert.AreEqual("font-height", document.StyleRules[1].Properties[0].Name);
            Assert.AreEqual("12pt", document.StyleRules[1].Properties[0].Value);
        }
    }
}
