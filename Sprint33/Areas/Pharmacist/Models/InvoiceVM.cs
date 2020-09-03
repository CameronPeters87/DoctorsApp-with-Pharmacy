using Sprint33.PharmacyEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class InvoiceVm
    {
        // Order
        public int StockOrderId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StockOrderDate { get; set; }
        public string PaymentPeriod { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float SubTotal { get; set; }
        public float TotalTax { get; set; }
        public float TotalCost { get; set; }
        public bool IsPaid { get; set; }
        [DataType(DataType.Date)]
        public DateTime PaymentDue { get; set; }
        public string StringPaymentDue { get; set; }
        public string StringOrderDate { get; set; }

        // Supplier
        public virtual Supplier Supplier { get; set; }

        // Medication List
        public IEnumerable<StockCart> StockOrderItems { get; set; }
    }
}