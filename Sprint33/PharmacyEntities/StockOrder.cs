using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.PharmacyEntities
{
    public class StockOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime StockOrderDate { get; set; }
        public string PaymentPeriod { get; set; }
        public float SubTotal { get; set; }
        public float TotalTax { get; set; }
        public float TotalCost { get; set; }
        public string Notes { get; set; }
        public int SupplierId { get; set; }
        public int OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}