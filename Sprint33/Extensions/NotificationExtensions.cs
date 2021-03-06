﻿using Sprint33.PharmacyEntities;
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
                BackgroundColorIcon = "bg-info",
                IsPrescriptionNotification = false
            });
        }

        public static void PushPrescriptionNotificaiton(this IDbSet<Notification> model, string link)
        {
            model.Add(new Notification
            {
                CreatedDate = DateTime.Now,
                Message = "Prescription",
                isRead = false,
                Icon = "fa-file-alt",
                BackgroundColorIcon = "bg-info",
                IsPrescriptionNotification = true,
                PrescriptionLink = link
            });
        }
    }
}