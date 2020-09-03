using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint33.Areas.Pharmacist.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint33.Areas.Pharmacist.HelperMethods.Tests
{
    [TestClass()]
    public class CalculationTests
    {
        [TestMethod()]
        public void CalculateVatOnProductTest_SingleProduct_AreEqual()
        {
            var actual = Calculation.CalculateVatOnProduct(50, 1, 0.15f);
            var expected = 7.5f;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CalculateVatOnProductTest_MultiProduct_AreEqual()
        {
            var actual = Calculation.CalculateVatOnProduct(50, 10, 0.15f);
            var expected = 75f;

            Assert.AreEqual(expected, actual);
        }
    }
}