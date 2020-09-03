using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sprint33.PharmacyEntities
{
    public class InStoreCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? InStoreSaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float VatAmount { get; set; }
        public float Price { get; set; }
        public virtual Product Product { get; set; }

    }
}