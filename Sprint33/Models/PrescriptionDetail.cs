using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.Models
{
    public class PrescriptionDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string MedicineName { get; set; }
        public string PackSize { get; set; }
        public string Instructions { get; set; }
        public int? PrescriptionId { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }
}