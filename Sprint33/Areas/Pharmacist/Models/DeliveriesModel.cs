using Sprint33.PharmacyEntities;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class DeliveriesModel
    {
        public virtual CustomerOrder CustomerOrder { get; set; }
        public double Distance { get; set; }
    }
}