using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.PharmacyEntities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public int DiscountedRate { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
    }
}