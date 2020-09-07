using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Sprint33.PharmacyEntities;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sprint33.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Inventory> Inventorys { get; set; }
        public object Items { get; internal set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Consultation_v2> Consultation_V2s { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public System.Data.Entity.DbSet<Sprint33.Models.Order> Orders { get; set; }
        public DbSet<Referral> referrals { get; set; }
        public DbSet<Referral_v2> Referral_V2s { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Loyalty> Loyalties { get; set; }

        // Pharmacy Entities
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockOrder> StockOrders { get; set; }
        public DbSet<StockCart> StockCarts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PharmacySetting> PharmacySettings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<InStoreSale> InStoreSales { get; set; }
        public DbSet<InStoreCart> InStoreCarts { get; set; }
        public DbSet<CustomerCart> CustomerCarts { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Billing> Billings { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}