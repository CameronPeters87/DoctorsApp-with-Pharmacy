using System.ComponentModel.DataAnnotations;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class CustomerVM
    {
        [Display(Name = "Name")]
        public string FirstName { get; set; }
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public bool IsLoyal { get; set; }
        public int NumberOfPurchases { get; set; }
    }
}