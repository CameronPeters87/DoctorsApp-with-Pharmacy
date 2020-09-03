using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint33.Areas.Pharmacist.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint33.Models;
using System.Data.Entity;
using Sprint33.PharmacyEntities;

namespace Sprint33.Areas.Pharmacist.HelperMethods.Tests
{
    [TestClass()]
    public class InStoreCartExtensionTests
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [TestMethod()]
        public void GetCurrentCartItemsTest()
        {
            var actual_cartItems = db.InStoreCarts.Where(c => c.InStoreSaleId.Equals(null))
                .ToList();
            var expected_cartItems = db.InStoreCarts.GetCurrentCartItems();

            Assert.AreEqual(expected_cartItems.Any(), actual_cartItems.Any());
        }

        [TestMethod()]
        public void GetTotalPriceOfCurrentCartTest()
        {
            var currentCart = db.InStoreCarts.GetCurrentCartItems();

            var actualPrice = currentCart.Select(c => c.Price).Sum();
            var expectedPrice = currentCart.GetTotalPriceOfCurrentCart();

            Assert.AreEqual(expectedPrice, actualPrice);
        }
    }
}