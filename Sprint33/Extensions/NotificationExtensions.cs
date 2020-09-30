using Sprint33.PharmacyEntities;
using System;
using System.Data.Entity;

namespace Sprint33.Extensions
{
    public static class NotificationExtensions
    {
        public static void PushNotificaiton(this IDbSet<Notification> model, string message)
        {
            model.Add(new Notification
            {
                CreatedDate = DateTime.Now,
                Message = message,
                isRead = false,
                Icon = "fa-file-alt",
                BackgroundColorIcon = "bg-info"
            });
        }
    }
}