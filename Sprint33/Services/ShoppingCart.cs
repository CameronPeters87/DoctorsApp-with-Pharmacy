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

            float vat = GetProductVat(product, quantity);
            float price = GetProductPrice(product, quantity);

            try
            {
                db.CustomerCarts.Add(new CustomerCart
                {
                    Product = product,
                    Quantity = quantity,
                    Price = price,
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
        public CustomerCart GetCartItem(int id)
        {
            return currentCart.Where(cart => cart.Id == id).FirstOrDefault();
        }
        public int UpdateCartItem(int cartItemId, int quantity)
        {
            var cartItem = GetCartItem(cartItemId);

            cartItem.Quantity = quantity;
            cartItem.VatAmount = GetProductVat(cartItem.Product, quantity);
            cartItem.Price = GetProductPrice(cartItem.Product, quantity);

            db.Entry(cartItem).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return cartItem.Id;
        }

        public int RemoveCartItem(int cartItemId)
        {
            var cartItem = GetCartItem(cartItemId);

            if (cartItem == null)
            {
                return 0;
            }

            db.CustomerCarts.Remove(cartItem);
            db.SaveChanges();

            return cartItemId;
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



        private float GetProductVat(Product product, int quantity)
        {
            if (product.IsOnSale)
            {
                return Calculation.CalculateVatOnProduct(product.DiscountedPrice, quantity, 0.15f);
            }
            else
                return Calculation.CalculateVatOnProduct(product.SellingPrice, quantity, 0.15f);

        }
        private float GetProductPrice(Product product, int quantity)
        {
            if (product.IsOnSale)
                return product.DiscountedPrice * quantity;
            else
                return product.SellingPrice * quantity;

        }

    }
}