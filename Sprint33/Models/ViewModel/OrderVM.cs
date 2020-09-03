using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sprint33.Models.ViewModel
{
    public class OrderVM
    {
        [Display(Name = "Order ID")]
        public int OrderID { get; set; }
        [Display(Name = "Patient`s Name")]
        public string PatientName { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Price")]
        //[DataType(DataType.Currency)]
        //[DisplayFormat(DataFormatString = "{0:c}")]
        public int TotalPrice { get; set; }
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public bool LoyaltyPurchase { get; set; }
        public bool isApproved { get; set; }
        public bool isRefund { get; set; }
    }
}