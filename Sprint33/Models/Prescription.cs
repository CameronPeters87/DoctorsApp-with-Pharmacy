using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sprint33.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        public string Barcode { get; set; }
        public string BarcodeUrl { get; set; }
        public string SignatureUrl { get; set; }
        public DateTime DateIssued { get; set; }

        public DateTime PrescriptionValid { get; set; }

        public byte[] Signature { get; set; }

        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; }
        public virtual ICollection<PrescriptionPharmacyItem> PrescriptionPharmacyItems { get; set; }

    }
}
