using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sprint33.Models.ViewModel
{
    public class LoyaltyVM
    {
    }

    public class LoyaltyListVM
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string ContactNumber { get; set; }
        [DisplayName("Loyalty Points")]
        public int Loyalty_Points { get; set; }
        [DataType(DataType.Date)]
        public DateTime Registered { get; set; }
        public int PatientId { get; set; }
    }

    public class LoyaltySubscribeVM
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public int Loyalty_Points { get; set; }
        public DateTime Registered { get; set; }
        public int PatientId { get; set; }
    }
}