using System;
using System.Text;
using System.Collections.Generic;
using Carubbi.Utils.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carubbi.Utils.Test.Data
{
    /// <summary>
    /// Summary description for ConversionExtensionsTestFixture
    /// </summary>
    [TestClass]
    public class ConversionExtensionsTestFixture
    {
       
        [DataRow("on", true)]
        [DataRow("1", true)]
        [DataRow("yes", true)]
        [DataRow("true", true)]
        [DataRow("false", false)]
        [DataRow("fail", false)]
        [DataRow("default", false)]
        [DataRow("anything", false)]
        [DataTestMethod]
        public void ConvertBoolean(string test, bool expected)
        {
            var result = test.To<bool>(false);
            Assert.AreEqual(result, expected);
        }

        [DataRow("on", true)]
        [DataRow("1", true)]
        [DataRow("yes", true)]
        [DataRow("true", true)]
        [DataRow("false", false)]
        [DataRow("fail", null)]
        [DataRow("default", null)]
        [DataRow("anything", null)]
        [DataTestMethod]
        public void ConvertNullableBoolean(string test, bool? expected)
        {
            var result = test.To<bool>();
            Assert.AreEqual(result, expected);
        }


        [DataRow("-4", -4)]
        [DataRow("1", 1)]
        [DataRow("0", 0)]
        [DataRow("1000", 1000)]
        [DataRow("9999999999999999999", -1)]
        [DataRow("default", -1)]
        [DataTestMethod]
        public void ConvertInt(string test, int expected)
        {
            var result = test.To<int>(-1);
            Assert.AreEqual(result, expected);
        }

        [DataRow("-4", -4)]
        [DataRow("1", 1)]
        [DataRow("0", 0)]
        [DataRow("1000", 1000)]
        [DataRow("9999999999999999999", null)]
        [DataRow("default",null)]
        [DataTestMethod]
        public void ConvertNullableInt(string test, int? expected)
        {
            var result = test.To<int>();
            Assert.AreEqual(result, expected);
        }

   

        [TestMethod]
        public void ConvertDateTime()
        {
            var result = "01/01/1900".To<DateTime>();
            Assert.AreEqual(result, new DateTime(1900, 01, 01));

            result = "01/12/1900".To<DateTime>();
            Assert.AreEqual(result, new DateTime(1900, 12, 01));

            result = "1900-12-01".To<DateTime>();
            Assert.AreEqual(result, new DateTime(1900, 12, 01));
        }
    }
}
