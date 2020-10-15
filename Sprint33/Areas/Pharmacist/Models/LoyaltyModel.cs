namespace Sprint33.Areas.Pharmacist.Models
{
    public class LoyaltyModel
    {
        public int Id { get; set; }
        public int PointsLimit { get; set; }
        public string CouponCode { get; set; }
        public int CouponDiscountRate { get; set; }
        public int MonthsToExpiry { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}