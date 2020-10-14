using Sprint33.PharmacyEntities;
using System.ComponentModel.DataAnnotations;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class DeliveriesModel
    {
        public virtual CustomerOrder CustomerOrder { get; set; }
        public double Distance { get; set; }
    }

    public class DirectionsModel
    {
        public virtual CustomerOrder CustomerOrder { get; set; }
        [UIHint("SignaturePad")]
        public byte[] Signature { get; set; }
    }
}