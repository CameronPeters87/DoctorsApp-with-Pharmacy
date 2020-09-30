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

        public static Coupon GetLastCoupon(this IDbSet<Coupon> coupon)
        {
            return coupon.OrderByDescending(c => c.Id).FirstOrDefault();
        }
        public static Coupon GetCouponByCode(this IDbSet<Coupon> coupon, string code)
        {
            return coupon.Where(c => c.Code == code).FirstOrDefault();
        }

        public static bool IsCodeUnique(this IDbSet<Coupon> coupon, string code)
        {
            return !(coupon.Any(c => c.Code == code));
        }

        public static bool IsCouponCodeEnteredValid(this IDbSet<Coupon> coupon,
            string code, float OrderTotal)
        {
            string actual = code.ToUpper();

            var model = coupon.Where(c => c.Code == actual &&
                DateTime.Today >= c.EndDate &&
                c.MinimumOrderAmount <= OrderTotal).FirstOrDefault();

            if (model == null)
            {
                return false;
            }
            else
                return true;
        }
    }
}