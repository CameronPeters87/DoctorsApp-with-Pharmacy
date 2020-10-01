using Sprint33.PharmacyEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.Models
{
    public class PrescriptionPharmacyItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? PrescriptionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float VatAmount { get; set; }
        public float Price { get; set; }
        public virtual Product Product { get; set; }
    }
}