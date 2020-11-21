using System;

namespace Sprint33.ApiModels
{
    public class DeliveryApiModel
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime TimeDelivered { get; set; }
        public string Status { get; set; }
        public byte[] Signature { get; set; }
        public string QRCodeTextConfirmation { get; set; }
        public string QRCodeImagePathConfirmation { get; set; }
        public string ConfirmationType { get; set; }
        public int OrderId { get; set; }
        public int DriverId { get; set; }
    }
}