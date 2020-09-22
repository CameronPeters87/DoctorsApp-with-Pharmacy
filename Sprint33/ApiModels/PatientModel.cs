using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sprint33.PharmacyEntities;

namespace Sprint33.ApiModels
{
    public class PatientModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isActive { get; set; }
        public bool isLoyal { get; set; }
        public int AddressId { get; set; }
    }
}