using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.PharmacyEntities
{
    public class InStoreSale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public float TotalTax { get; set; }
        public float TotalCost { get; set; }
        public float CashTendered { get; set; }
        public float Change { get; set; }
        public string PaymentMethod { get; set; }
    }
}