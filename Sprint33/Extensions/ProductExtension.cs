using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sprint33.Areas.Pharmacist.HelperMethods
{
    public static class ProductExtension
    {
        public static bool IsInStock(this Product product)
        {
            var result = product.Quantity > 0;
            return true;
        }
        public static bool IsOutOfStock(this Product product, int input_Quantity)
        {
            if (product.Quantity <= 0 || input_Quantity > product.Quantity)
            {
                return true;
            }
            else
                return false;
        }
        public static bool IsItemInCart(this Product product,
            IEnumerable<InStoreCart> currentCart)
        {
            if (currentCart.Any(c => c.ProductId.Equals(product.Id)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsItemInCustomerCart(this Product product,
            IEnumerable<CustomerCart> currentCart)
        {
            if (currentCart.Any(c => c.ProductId.Equals(product.Id)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Product FindBySlug(this IDbSet<Product> products,
            string slug)
        {
            return products.Where(p => p.Slug == slug).FirstOrDefault();
        }
    }
}