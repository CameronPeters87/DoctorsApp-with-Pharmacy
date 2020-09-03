using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Sprint33.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Required]
        [Display(Name = "Prescribed Medication:")]
        public int InventoryId { get; set; }

        [Required]
        [Display(Name = "Date Issued")]
        //[DataType(DataType.Date)]
        public DateTime DateIssued { get; set; }

        [Required]
        [Display(Name = "Prescription Valid Until")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        //[DataType(DataType.DateTime)]
        [CurrentDate(ErrorMessage = "Date must be after or equal to current date")]
        public DateTime PrescriptionValid { get; set; }
        [Required]
        [Display(Name = "Notes:")]
        [DataType(DataType.MultilineText)]
        public string PrescriptionDetails { get; set; }
        [Display(Name = "Quantity")]
        public int itemQuantity { get; set; }

        public Doctor Doctor { get; set; }

        [Display(Name = "Doctor Name")]
        public string DoctorName = "Dr B Naidoo";

        public Patient Patient { get; set; }

        [Required]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        public int PatientID { get; set; }
        public int DoctorId { get; set; }
    }
}
