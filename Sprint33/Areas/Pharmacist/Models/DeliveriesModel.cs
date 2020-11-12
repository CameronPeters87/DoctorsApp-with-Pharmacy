using Sprint33.PharmacyEntities;
using System.Collections.Generic;
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
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string ContactNumber { get; set; }

        public IEnumerable<Driver> Drivers { get; set; }
    }
}