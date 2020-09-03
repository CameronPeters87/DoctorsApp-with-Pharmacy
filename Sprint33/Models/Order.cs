using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.Models
{
    public class Order
    {
        [Display(Name = "Order ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        [Display(Name = "Patient`s Name")]
        public string PatientName { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Total Price")]
        //[DataType(DataType.Currency)]
        //[DisplayFormat(DataFormatString = "{0:c}")]
        public int TotalPrice { get; set; }
        public bool isApproved { get; set; }
        public bool isRefund { get; set; }
        public bool LoyaltyPurchase { get; set; }
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
    }
}