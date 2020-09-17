using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System.Data.Entity;
using System.Linq;

namespace Sprint33.Extensions
{
    public static class BillingExtensions
    {
        public static Billing GetLastBilling(this IDbSet<Billing> billings)
        {
            return billings.OrderByDescending(b => b.Id).FirstOrDefault();
        }

        public static void CreateBilling(this IDbSet<Billing> billings,
            ApplicationDbContext db, Patient patient)
        {
            db.Billings.Add(new Billing
            {
                FirstName = patient.FirstName,
                Surname = patient.Surname,
                City = patient.Address.City,
                Country = patient.Address.Country,
                Email = patient.Email,
                PhoneNumber = patient.ContactNumber,
                Province = patient.Address.Province,
                Route = patient.Address.Route,
                Street_Number = patient.Address.Street_Number,
                ZipCode = patient.Address.ZipCode
            });

            db.SaveChanges();
        }
    }
}