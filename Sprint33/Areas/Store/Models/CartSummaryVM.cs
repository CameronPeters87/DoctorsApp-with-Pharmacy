using System.Collections.Generic;

namespace Sprint33.Areas.Store.Models
{
    public class CartSummaryVM
    {
        public CartSummaryVM()
        {
            CartItems = new List<CartItemsSummary>();
        }
        public virtual ICollection<CartItemsSummary> CartItems { get; set; }

        // Order Details
        public float SubTotal { get; set; }
        public float VatTotal { get; set; }
        public float DiscountFromCouponTotal { get; set; }
        public float TotalCost { get; set; }

        // Coupon Code
        public string CouponCode { get; set; }
    }

    public class CartItemsSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ImageUrl { get; set; }
    }
}