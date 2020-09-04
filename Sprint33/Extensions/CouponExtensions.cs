using Sprint33.Areas.Pharmacist.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Sprint33.Extensions
{
    public static class CouponExtensions
    {
        public static bool IsDatesValid(this CouponVM model)
        {
            if (model.StartDate < DateTime.Today || model.EndDate < DateTime.Today ||
                model.EndDate < model.StartDate)
            {
                return false;
            }
            else
                return true;
        }

        public static bool IsCategoryIdEmpty(this CouponVM model)
        {
            if (model.CategoryId == null)
            {
                return true;
            }
            else
                return false;
        }

        public static Coupon GetLastCoupon(this IDbSet<Coupon> coupon)
        {
            return coupon.OrderByDescending(c => c.Id).FirstOrDefault();
        }

        public static bool IsCodeUnique(this IDbSet<Coupon> coupon, string code)
        {
            return !(coupon.Any(c => c.Code == code));
        }
    }
}