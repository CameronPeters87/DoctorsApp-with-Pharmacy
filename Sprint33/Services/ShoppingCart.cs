using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sprint33.Services
{
    public class ShoppingCart
    {
        ApplicationDbContext db;
        int patientId;
        IQueryable<CustomerCart> currentCart;

        public ShoppingCart(HttpContextBase httpContext, ApplicationDbContext db)
        {
            this.db = db;

            patientId = Convert.ToInt32(httpContext.Session["id"]);
            currentCart = db.CustomerCarts
                .Where(c => c.CustomerOrderId == null && c.CustomerId == patientId);
        }

        public void AddToCart(Product product, int quantity)
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
                db.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public IEnumerable<CustomerCart> GetCartItems()
        {
            return currentCart.ToList();
        }

        public int GetCount()
        {
            return currentCart.ToList().Count;
        }

        public float GetTotalPrice()
        {
            return currentCart.Select(c => c.Price).Sum();
        }

        public float GetTotalTax()
        {
            return currentCart.Select(c => c.VatAmount).Sum();
        }

        public void UpdateCart(Product product, int Quantity)
        {
            throw new System.NotImplementedException();
        }
    }
}