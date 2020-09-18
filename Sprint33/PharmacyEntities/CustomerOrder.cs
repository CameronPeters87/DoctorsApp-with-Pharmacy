using Sprint33.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.PharmacyEntities
{
    public class CustomerOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        //public float SubTotal { get; set; }
        public float TotalTax { get; set; }
        public float TotalCost { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }

        public string FirstName { get; set; }
        public string Surname { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int? CouponId { get; set; }
        public int CustomerId { get; set; }
        public int OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual Patient Customer { get; set; }
    }
}