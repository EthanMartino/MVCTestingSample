using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCTestingSample.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVCTestingSample.Models.Tests
{
    [TestClass()]
    public class ValidationHelperTests
    {
        [TestMethod]
        [DataRow("9.99")]
        [DataRow("23")]
        [DataRow("$3456.3")] //USD only
        [DataRow(".99")]
        [DataRow("0.99")]
        [DataRow("0")]
        [DataRow("10000000")]
        public void IsValidPrice_ValidPrice_ReturnsTrue(string input)
        {
            bool result = ValidationHelper.IsValidPrice(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("five")]
        [DataRow("3 and 5 cents")]
        [DataRow("5 dollars")]
        [DataRow("12.57.3")]
        [DataRow("234$")]
        [DataRow("$234$")]
        [DataRow("500,000")]
        [DataRow("$500,000")]
        public void IsValidPrice_InvalidPrice_ReturnFalse(string input)
        {
            bool result = ValidationHelper.IsValidPrice(input);

            Assert.IsFalse(result);
        }
    }
}