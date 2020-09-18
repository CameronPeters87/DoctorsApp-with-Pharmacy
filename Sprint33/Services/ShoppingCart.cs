using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using Sprint33.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sprint33.Services
{
    public class ShoppingCart : IShoppingCart
    {
        ApplicationDbContext db;

        List<CustomerCart> customerCart;
        int count;

        public ShoppingCart(ApplicationDbContext db)
        {
            this.db = db;
        }


        public void AddToCart(Product product, int quantity, int patientId)
        {
            var patient = db.Patients.Find(patientId);

            float vat;

            if (product.IsOnSale)
            {
                vat = Calculation.CalculateVatOnProduct(product.DiscountedPrice, quantity, 0.15f);
            }
            else
                vat = Calculation.CalculateVatOnProduct(product.SellingPrice, quantity, 0.15f);

            try
            {
                db.CustomerCarts.Add(new CustomerCart
                {
                    Product = product,
                    Quantity = quantity,
                    Price = product.IsOnSale ? product.DiscountedPrice : product.SellingPrice,
                    VatAmount = vat,
                    CustomerId = patient.UserID,
                    Patient = patient,
                    ProductId = product.Id
                });
            }
            catch (Exception e)
            {

                throw e;
            }

            db.SaveChanges();
        }

        public ShoppingCart GetCart(HttpContextBase httpContext)
        {
            var cart = new ShoppingCart();
            int patientId = Convert.ToInt32(httpContext.Session["id"]);

            cart.customerCart = db.CustomerCarts.Where(c => c.CustomerOrderId == null &&
            c.CustomerId == patientId).ToList();
            cart.count = cart.customerCart.Count;

            return cart;
        }

        public IEnumerable<CustomerCart> GetCartItems()
        {
            return customerCart;
        }

        public int GetCount()
        {
            throw new System.NotImplementedException();
        }

        public float GetTotalPrice()
        {
            throw new System.NotImplementedException();
        }

        public float GetTotalTax()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCart(Product product, int Quantity)
        {
            throw new System.NotImplementedException();
        }
    }
}