using Sprint33.PharmacyEntities;
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
    }
}