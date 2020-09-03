using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sprint33.Areas.Pharmacist.Models
{
    public class CategoryVM
    {
        // For the table
        public IEnumerable<Category> Categories { get; set; }

        // For Creating a category
        [Required]
        public string Name { get; set; }

        public int Id { get; set; }
    }
}