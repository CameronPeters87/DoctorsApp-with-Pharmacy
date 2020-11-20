using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class DeliveriesModel
    {
        public virtual Delivery Delivery { get; set; }
        public double Distance { get; set; }
    }

    public class SetDeliveryModel
    {
        public virtual CustomerOrder CustomerOrder { get; set; }

        public IEnumerable<SelectListItem> Drivers { get; set; }
        public int OrderId { get; set; }
        [Required]
        public int DriverId { get; set; }
    }

    public class DirectionsModel
    {
        public virtual Delivery Delivery { get; set; }
        public int DeliveryId { get; set; }
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