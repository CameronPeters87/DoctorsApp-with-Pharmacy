using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.PharmacyEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint33.Areas.Pharmacist.HelperMethods.Tests
{
    [TestClass()]
    public class ProductExtensionTests
    {
        [TestMethod()]
        public void IsOutOfStockTest_InputQuantityGreaterThanProductQuantity_ReturnTrue()
        {
            var product = new Product();
            product.Quantity = 0;
            var result = product.IsOutOfStock(2);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsOutOfStockTest_InputQuantityLessThanProductQuantity_ReturnFalse()
        {
            var product = new Product();
            product.Quantity = 10;
            var result = product.IsOutOfStock(1);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsOutOfStockTest_InputQuantityEqualsProductQuantity_ReturnFalse()
        {
            var product = new Product();
            product.Quantity = 10;
            var result = product.IsOutOfStock(10);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsItemInCartTest_ItemIsInCart_ReturnTrue()
        {
            Product product = new Product
            {
                Id = 1
            };
            List<InStoreCart> currentCart = new List<InStoreCart>();
            currentCart.Add(new InStoreCart
            {
                Id = 1,
                InStoreSaleId = null,
                Price = 50,
                ProductId = product.Id,
                Quantity = 1,
                VatAmount = 10
            });

            var result = product.IsItemInCart(currentCart);

            Assert.IsTrue(result);
        }
        [TestMethod()]
        public void IsItemInCartTest_ItemIsNotInCart_ReturnFalse()
        {
            Product product = new Product
            {
                Id = 1
            };
            List<InStoreCart> currentCart = new List<InStoreCart>();
            currentCart.Add(new InStoreCart
            {
                Id = 1,
                InStoreSaleId = null,
                Price = 50,
                ProductId = 2,
                Quantity = 1,
                VatAmount = 10
            });

            var result = product.IsItemInCart(currentCart);

            Assert.IsFalse(result);
        }
    }
}