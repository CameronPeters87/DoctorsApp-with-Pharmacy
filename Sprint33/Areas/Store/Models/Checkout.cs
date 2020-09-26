using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    public class BillingForm
    {
        public int OrderId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string PaymentMethod { get; set; }
    }
}