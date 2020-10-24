using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.PharmacyEntities
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isRead { get; set; }
        public string Icon { get; set; }
        public string BackgroundColorIcon { get; set; }
        public bool IsPrescriptionNotification { get; set; }
        public string PrescriptionLink { get; set; }
    }
}