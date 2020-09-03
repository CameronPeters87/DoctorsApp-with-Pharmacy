using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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
    }
}