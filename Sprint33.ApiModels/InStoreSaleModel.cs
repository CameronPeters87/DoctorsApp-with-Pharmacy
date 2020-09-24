using System;

namespace Sprint33.ApiModels
{
    public class InStoreSaleModel
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public float TotalTax { get; set; }
        public float TotalCost { get; set; }
        public float CashTendered { get; set; }
        public float Change { get; set; }
        public string PaymentMethod { get; set; }
    }
}