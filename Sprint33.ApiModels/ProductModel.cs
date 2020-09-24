namespace Sprint33.ApiModels
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string ImageUrl { get; set; }
        public string BarcodeUrl { get; set; }
        public string SkuCode { get; set; }
        public string PackSize { get; set; }
        public int Quantity { get; set; }
        public float SupplierPrice { get; set; }
        public float SellingPrice { get; set; }
        public bool IsOnSale { get; set; }
        public float DiscountedPrice { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
    }
}