using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.PharmacyEntities
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Status { get; set; }
        public byte[] Signature { get; set; }
        public string QRCodeTextConfirmation { get; set; }
        public string QRCodeImagePathConfirmation { get; set; }
        public int OrderId { get; set; }
        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual CustomerOrder CustomerOrder { get; set; }
    }
}