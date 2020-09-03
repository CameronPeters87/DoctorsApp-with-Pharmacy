﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sprint33.Models
{
    public class PatientPrescriptionViewModel
    {

        public string PatientName { get; set; }
        public int Age { get; set; }
        public string DoctorName { get; set; }
        [Required]
        [Display(Name = "Prescribed Medication:")]
        public int InventoryId { get; set; }
        [Display(Name = "Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        [Required]
        public int itemQuantity { get; set; }
        public DateTime DateIssued { get; set; }
        [Required]
        [Display(Name = "Prescription Valid Until")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [CurrentDate(ErrorMessage = "Date must be after or equal to current date")]
        public DateTime PrescriptionValid { get; set; }
        [Required]
        [Display(Name = "Notes:")]
        [DataType(DataType.MultilineText)]
        public string PrescriptionDetails { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }


        public string InventoryName
        {
            get
            {
                // Return string title
                // If theres no product type, String is empty
                // Else if ProdType.Id == ProductTypeId in this model, display the title
                return Inventories == null || Inventories.Count.Equals(0) ? String.Empty :
                    Inventories.First(i => i.ItemID.Equals(InventoryId)).itemName;
            }
        }
    }

    public class PrescriptionListViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        [Required]
        [Display(Name = "Date Issued")]
        public DateTime DateIssued { get; set; }
        public int InventoryId { get; set; }
        [Display(Name = "Quantity")]
        public int itemQuantity { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
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
        [Display(Name = "Prescription Details")]
        public string InventoryName
        {
            get
            {
                // Return string title
                // If theres no product type, String is empty
                // Else if ProdType.Id == ProductTypeId in this model, display the title
                return Inventories == null || Inventories.Count.Equals(0) ? String.Empty :
                    Inventories.First(i => i.ItemID.Equals(InventoryId)).itemName;
            }
        }
    }

    public class CurrentDateAttribute : ValidationAttribute
    {
        public CurrentDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var dt = (DateTime)value;
            if (dt >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}