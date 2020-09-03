using Sprint33.PharmacyEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class InStoreSaleVM
    {
        public int InStoreSaleId { get; set; }
        [DataType(DataType.Date)]
        public DateTime SaleDate { get; set; }
        public string LongSaleDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float TotalCost { get; set; }
        public string ReceiptLink { get; set; }
        public IEnumerable<InStoreCart> CartItems { get; set; }
    }

    public class InStoreSalesReportVM
    {
        public IEnumerable<InStoreSaleVM> InStoreSales { get; set; }
    }

    public class NewInStoreSaleVM
    {
        // Input Form 
        public IEnumerable<SelectListItem> ProductsDropdown { get; set; }
        public IEnumerable<Product> ProductList { get; set; }
        public int Quantity { get; set; }
        [DisplayName("Payment Method")]
        public string PaymentMethod { get; set; }
        public float CashTendered { get; set; }

        // General Info
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float SubTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float TaxTotal { get; set; }
        public IEnumerable<InStoreCart> CurrentCartItems { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float TotalPrice { get; set; }
        public float Change { get; set; }

        // Ids
        [DisplayName("Select Product")]
        public int ProductId { get; set; }
        public int InStoreCartId { get; set; }
    }

    public class ReceiptVm
    {
        // Order
        public int InStoreSaleId { get; set; }
        [DataType(DataType.Date)]
        public DateTime InStoreSaleDateTime { get; set; }
        public string PaymentMethod { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float SubTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float TotalTax { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float TotalCost { get; set; }
        public float CashTendered { get; set; }
        public float Change { get; set; }
        public string StringSaleDateLong { get; set; }

        // Medication List
        public IEnumerable<InStoreCart> InStoreSaleItems { get; set; }
    }
}