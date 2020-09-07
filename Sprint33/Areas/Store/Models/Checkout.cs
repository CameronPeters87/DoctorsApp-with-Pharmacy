using Sprint33.PharmacyEntities;
using System.Collections.Generic;

namespace Sprint33.Areas.Store.Models
{
    public class Checkout
    {
        public Checkout()
        {
            CartItems = new List<CartItemsSummary>();
        }
        public CustomerOrder CustomerOrder { get; set; }

        public virtual ICollection<CartItemsSummary> CartItems { get; set; }

        //Payment method
        public string PaymentMethod { get; set; }

        // Coupon Code
        public string CouponCode { get; set; }
    }
}