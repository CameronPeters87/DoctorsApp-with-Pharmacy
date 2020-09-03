using Sprint33.PharmacyEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class StockOrderVM
    {
        public IEnumerable<StockOrderListVM> StockOrderListVM { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
        public IEnumerable<OrderStatus> OrderStatuses { get; set; }
    }

    public class StockOrderListVM
    {
        public int StockOrderId { get; set; }
        public string Supplier { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public string LongOrderDate { get; set; }
        public string PaymentPeriod { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float TotalCost { get; set; }
        public string InvoiceLink { get; set; }
        public int OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public IEnumerable<StockCart> CartItems { get; set; }

    }

    public class NewPurchaseVM
    {
        // Input Form For StockCart
        public IEnumerable<SelectListItem> ProductsDropdown { get; set; }
        public IEnumerable<Product> ProductList { get; set; }
        [Required]
        public int Quantity { get; set; }

        // Input Form To Create Order
        public string PaymentPeriod { get; set; }
        public string Notes { get; set; }

        // General Info
        public float SubTotal { get; set; }
        public float TaxTotal { get; set; }
        public string Supplier { get; set; }
        public IEnumerable<StockCart> StockCart { get; set; }
        public float TotalPrice { get; set; }

        // Ids
        public int SupplierId { get; set; }
        [DisplayName("Select Product")]
        [Required]
        public int ProductId { get; set; }
        public int StockCartId { get; set; }
    }
}