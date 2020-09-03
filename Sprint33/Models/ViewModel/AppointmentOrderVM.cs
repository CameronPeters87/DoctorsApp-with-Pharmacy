using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sprint33.Models.ViewModel
{
    public class AppointmentOrderVM
    {
        [Key]
        public int AppointmentID { get; set; }
        public Patient Patient { get; set; }
        public int PatientID { get; set; }
        public Doctor Doctor { get; set; }
        public int DoctorID { get; set; }
        [Display(Name = "Patient`s Name")]
        public string PatientName { get; set; }
        [Display(Name = "Doctor`s Name")]
        public string DoctorName { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Day and Time of Appointment")]
        public DateTime AppointmentTime { get; set; }
        public bool Confirmed { get; set; }
        [Display(Name = "Checked In")]
        public bool CheckedIn { get; set; }
        [Display(Name = "Patient`s Symtoms")]
        public string symtoms { get; set; }
        [Display(Name = "Doctor`s Diagnosis")]
        public string diagnosis { get; set; }
        [Display(Name = "Exspration Date")]
        public DateTime Expire { get; set; }

        // Order
        [Display(Name = "Order ID")]
        public int OrderID { get; set; }
        [Display(Name = "User Name")]
        public string UserID { get; set; }
        [Display(Name = "Postal Code")]
        public string DeliveryName { get; set; }
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }
        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public int TotalPrice { get; set; }
    }
}