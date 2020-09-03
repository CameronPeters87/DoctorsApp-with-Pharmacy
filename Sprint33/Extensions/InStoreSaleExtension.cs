using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Sprint33.Areas.Pharmacist.HelperMethods
{
    public static class InStoreSaleExtension
    {
        public static bool CheckCashTenderedExists(this NewInStoreSaleVM model)
        {
            if (model.CashTendered != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static InStoreSale GetLastInStoreSale(this IDbSet<InStoreSale> dto)
        {
            return dto.OrderByDescending(s => s.Id).FirstOrDefault();
        }

    }
}