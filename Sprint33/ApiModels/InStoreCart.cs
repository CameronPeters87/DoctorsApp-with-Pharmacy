using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sprint33.ApiModels
{
    public class InStoreCart
    {
        public int Id { get; set; }
        public int? InStoreSaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float VatAmount { get; set; }
        public float Price { get; set; }
    }
}