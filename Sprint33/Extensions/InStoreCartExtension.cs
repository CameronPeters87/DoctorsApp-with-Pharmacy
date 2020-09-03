using Sprint33.Areas.Pharmacist.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sprint33.Areas.Pharmacist.HelperMethods
{
    public static class InStoreCartExtension
    {
        public static IEnumerable<InStoreCart> GetCurrentCartItems(this IDbSet<InStoreCart> dto)
        {
            return dto.Where(c => c.InStoreSaleId.Equals(null))
                .ToList();
        } 

        public static float GetTotalPriceOfCurrentCart(this IEnumerable<InStoreCart> currentCart)
        {
            var result = currentCart.Select(c => c.Price).Sum();
            return result;
        }

        public static float GetTaxOfCurrentCart(this IEnumerable<InStoreCart> currentCart)
        {
            var result = currentCart.Select(c => c.VatAmount).Sum();
            return result;
        }

        //public static GetLatestItem()
    }
}