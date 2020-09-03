using System.Collections.Generic;

namespace Sprint33.Areas.Store.Models
{
    public class MainVM
    {
        public IEnumerable<ProductContentVM> ProductContents { get; set; }
    }

    public class ProductContentVM
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public bool IsOnSale { get; set; }
        public float DiscountedPrice { get; set; }
        public string ProductLink { get; set; }
    }
}