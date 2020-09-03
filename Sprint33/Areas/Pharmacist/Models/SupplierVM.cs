using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class SupplierVM
    {
    }

    public class SupplierListVM
    {
        public IEnumerable<Supplier> Suppliers { get; set; }
    }

    public class AddSupplierVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Notes { get; set; }
        [Required]
        [Display(Name = "Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

    }
}