
using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;


namespace Sprint33.Areas.Pharmacist.Models
{
    public class ProductVM
    {
    }

    public class ProductListVM
    {
        public IEnumerable<Product> Products { get; set; }
    }

    public class AddProductVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string ImageUrl { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string BarcodeUrl { get; set; }
        [DisplayName("Sku Code")]
        public string SkuCode { get; set; }
        [Required]
        [DisplayName("Pack Size")]
        public string PackSize { get; set; }
        [DisplayName("Supplier Price")]
        public float SupplierPrice { get; set; }
        [DisplayName("Selling Price")]
        public float SellingPrice { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Supplier")]
        public int SupplierId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }
    }
}