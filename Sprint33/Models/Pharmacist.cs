using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.Models
{
    public class Pharmacist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}