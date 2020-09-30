using System;

namespace Sprint33.ApiModels
{
    public class CustomerOrderModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public float SubTotal { get; set; }
        public float TotalTax { get; set; }
        public float TotalCost { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }

        public string FirstName { get; set; }
        public string Surname { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // What the customer will scan on the delivery drivers phone
        public string ConfirmationQR_Url { get; set; }
        // What the delivery person will scan on the customer order or email
        public string DelivererConfirmationQR_Url { get; set; }

        public int? CouponId { get; set; }
        public int CustomerId { get; set; }
        public int OrderStatusId { get; set; }
        //public OrderStatusModel OrderStatusModel { get; set; }
        //public PatientModel PatientModel { get; set; }
    }
}