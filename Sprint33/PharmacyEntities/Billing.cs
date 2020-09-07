using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.PharmacyEntities
{
    public class Billing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string Surname { get; set; }

        public string Street_Number { get; set; }
        public string Route { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}