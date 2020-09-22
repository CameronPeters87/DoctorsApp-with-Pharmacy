using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sprint33.Extensions
{
    public static class CustomerCartExtensions
    {
        public static IEnumerable<CustomerCart> GetCurrentCartItems(this IDbSet<CustomerCart> dto,
            int customerId)
        {
            return dto.Where(c => c.CustomerOrderId == null &&
            c.CustomerId == customerId).ToList();
        }

        public static IEnumerable<CustomerCart> GetCartItemsFromOrder(this IDbSet<CustomerCart> dto,
            int customerId, int orderId)
        {
            return dto.Where(c => c.CustomerOrderId == orderId &&
            c.CustomerId == customerId).ToList();
        }

        public static float GetTotalPrice(this IEnumerable<CustomerCart> currentCart)
        {
            var result = currentCart.Select(c => c.Price).Sum();
            return result;
        }

        public static float GetTotalTax(this IEnumerable<CustomerCart> currentCart)
        {
            var result = currentCart.Select(c => c.VatAmount).Sum();
            return result;
        }

    }
}