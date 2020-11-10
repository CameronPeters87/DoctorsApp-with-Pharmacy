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

    public class DriverModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        //public TYPE Type { get; set; }
    }
}