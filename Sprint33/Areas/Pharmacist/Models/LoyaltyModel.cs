using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class LoyaltyModel
    {
        public int Id { get; set; }
        [Required]
        public int PointsLimit { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public int CouponDiscountRate { get; set; }
        [Required]
        public int MonthsToExpiry { get; set; }
        [Required]
        public string Subject { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}