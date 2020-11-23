using Sprint33.PharmacyEntities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sprint33.Models
{
    public class Patient
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use letters only")]
        [Display(Name = "Name")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use letters only")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Age")]
        public int Age { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please use numbers only")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Patient`s Active Status")]
        public bool isActive { get; set; }
        public bool isLoyal { get; set; }
        public DateTime DateRegistered { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}