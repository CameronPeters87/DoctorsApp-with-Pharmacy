﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sprint33.Extensions
{
    public static class ICollectionExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>
            (this ICollection<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("itemName"),
                       Value = item.GetPropertyValue("ItemID"),
                       Selected = item.GetPropertyValue("ItemID")
                                    .Equals(selectedValue.ToString())
                   };
        }
    }
}