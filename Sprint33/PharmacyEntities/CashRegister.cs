using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.PharmacyEntities
{
    public class CashRegister
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public double CashFloat { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}