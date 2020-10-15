using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.PharmacyEntities
{
    public class LoyaltyPreference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PointsLimit { get; set; }
        public string CouponCode { get; set; }
        public int CouponDiscountRate { get; set; }
        public int MonthsToExpiry { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}