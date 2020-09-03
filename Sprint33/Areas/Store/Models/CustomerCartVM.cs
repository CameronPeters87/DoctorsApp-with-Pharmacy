using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sprint33.Models;
using Sprint33.PharmacyEntities;

namespace Sprint33.Areas.Store.Models
{
    public class CustomerCartVM
    {
        public int Id { get; set; }
        public int? CustomerOrderId { get; set; }
        public int? CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float VatAmount { get; set; }
        public float Price { get; set; }
        public virtual Product Product { get; set; }
        public virtual Patient Patient { get; set; }

    }
}