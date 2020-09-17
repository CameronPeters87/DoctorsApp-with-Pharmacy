using Sprint33.Models;
using Sprint33.PharmacyEntities;

namespace Sprint33.Extensions
{
    public static class OrderStatusExtensions
    {
        public static OrderStatus CreateOrderStatus(ApplicationDbContext db,
            string name, string description, string color, string icon,
            bool isPaid)
        {
            var status = db.OrderStatuses.Add(new OrderStatus
            {
                Name = name,
                Description = description,
                Color = color,
                Icon = icon,
                isPaid = isPaid
            });

            db.SaveChanges();

            return status;
        }
    }
}