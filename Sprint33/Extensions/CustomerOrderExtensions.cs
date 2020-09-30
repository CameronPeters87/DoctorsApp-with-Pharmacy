using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sprint33.Extensions
{
    public static class CustomerOrderExtensions
    {
        public static CustomerOrder GetLastOrder(this IDbSet<CustomerOrder> orders)
        {
            return orders.OrderByDescending(o => o.Id).FirstOrDefault();
        }

        public static List<CustomerOrder> GetCustomerOrders(this IDbSet<CustomerOrder> orders,
            int patieintId)
        {
            return orders.Where(o => o.CustomerId == patieintId).ToList();
        }

        public static bool IsCouponNull(this CustomerOrder order)
        {
            if (order.CouponId == null)
                return true;
            else
                return false;
        }

    }
}