using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sprint33.Models
{
    public class ReferralViewModel
    {
        public int refferal_ID { get; set; }
        [Display(Name = "Doctor Full Name")]
        public string referral_Doctors_Name { get; set; }
        [Display(Name = "Doctor Practice Address")]
        public string referral_doctor_Add { get; set; }
        [Display(Name = "Doctor Contact Number")]
        public string referral_doctor_num { get; set; }
        [Display(Name = "Doctor Email Address")]
        public string referral_doctor_Email { get; set; }
        [Display(Name = "Date Issued")]
        public DateTime refferal_Date { get; set; }
        [Display(Name = "Referral Letter Valid Until")]
        // [DataType(DataType.Date)]
        [CurrentDate(ErrorMessage = "Date must be after or equal to current date")]
        public DateTime referral_ValidDate { get; set; }
        [Display(Name = "Hospital")]
        public string refferal_Location { get; set; }
        [Display(Name = "Reasoning")]
        public string referral_Reasoning { get; set; }
        public Patient Patient { get; set; }
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        public int PatientID { get; set; }
    }
}