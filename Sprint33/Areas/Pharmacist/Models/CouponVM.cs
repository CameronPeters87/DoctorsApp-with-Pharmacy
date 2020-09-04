using Sprint33.PharmacyEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class CouponVM
    {
        public IEnumerable<CouponItem> Coupons { get; set; }

        // Adding a new Coupon (Form fields)
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Range(1, 100)]
        public int DiscountRate { get; set; }
        public int? CategoryId { get; set; }
        public IEnumerable<SelectListItem> CategoryDropdown { get; set; }
    }

    public class CouponItem
    {
        public CouponItem() { }
        public CouponItem(Coupon dto)
        {
            Code = dto.Code;
            Description = dto.Description;
            QRcodeURL = dto.QRcodeURL;
            StartDateString = dto.StartDate.ToLongDateString();
            EndDateString = dto.EndDate.ToLongDateString();
            DiscountRate = dto.DiscountRate;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string QRcodeURL { get; set; }
        public string Description { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public int DiscountRate { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}