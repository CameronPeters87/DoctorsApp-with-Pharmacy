using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class DiscountVM
    {
        public int Id { get; set; }
        public bool IsOnSale { get; set; }
        public float DiscountedPrice { get; set; }
        public int DiscountedPercentage { get; set; }
        public IEnumerable<Product> AllProductList { get; set; }
        public IEnumerable<Product> DiscountedProducts { get; set; }
        // Form: Discount by Category
        public IEnumerable<SelectListItem> Categories { get; set; }
        public int CategoryId { get; set; }
    }
}