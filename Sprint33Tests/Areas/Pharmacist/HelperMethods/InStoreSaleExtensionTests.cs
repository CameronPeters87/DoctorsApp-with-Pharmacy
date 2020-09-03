using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint33.Areas.Pharmacist.HelperMethods.Tests
{
    [TestClass()]
    public class InStoreSaleExtensionTests
    {
        [TestMethod()]
        public void CheckCashTenderedExistsTest_NoCashTendered_ReturnFalse()
        {
            NewInStoreSaleVM model = new NewInStoreSaleVM();

            model.CashTendered = 0;

            var result = model.CheckCashTenderedExists();

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void CheckCashTenderedExistsTest_CashTendered_ReturnTrue()
        {
            NewInStoreSaleVM model = new NewInStoreSaleVM();

            model.CashTendered = 200;

            var result = model.CheckCashTenderedExists();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void GetLastInStoreSaleTest_ReturnObj()
        {
            // Arrange
            ApplicationDbContext db = new ApplicationDbContext();
            var actual = db.InStoreSales.OrderByDescending(s => s.Id)
                .FirstOrDefault();

            // Act 
            var expected = db.InStoreSales.GetLastInStoreSale();

            Assert.AreEqual(expected, actual);
        }
    }
}