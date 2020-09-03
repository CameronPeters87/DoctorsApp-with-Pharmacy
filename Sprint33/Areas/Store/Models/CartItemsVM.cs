using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sprint33.Areas.Store.Models
{
    public class CartItemsVM
    {
        public int NumberOfItemsInCart { get; set; }
        public string LinkToSummary { get; set; }
    }
}