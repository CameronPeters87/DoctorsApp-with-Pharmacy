using Sprint33.PharmacyEntities;

namespace Sprint33.Areas.Store.Models
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string ImageUrl { get; set; }
        public string BarcodeUrl { get; set; }
        public string PackSize { get; set; }
        public int Quantity { get; set; }
        public float SellingPrice { get; set; }
        public bool IsOnSale { get; set; }
        public float DiscountedPrice { get; set; }
        public virtual Category Category { get; set; }
    }
}