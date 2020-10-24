using System;
using System.Collections.Generic;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class NotificationVM
    {
        public IEnumerable<SingleNotificationVM> SingleNotification { get; set; }
        public int NumberOfUnreadNotifications { get; set; }
    }

    public class SingleNotificationVM
    {
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