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
        public int? CouponId { get; set; }
        public string CouponCode { get; set; }
        public float CouponDiscount { get; set; }
    }

    public class Confirm
    {
        public int BillingId { get; set; }

        public string FirstName { get; set; }
        public string Surname { get; set; }

        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int OrderId { get; set; }
    }
}