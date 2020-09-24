namespace Sprint33.ApiModels
{
    public class CustomerCartModel
    {
        public int Id { get; set; }
        public int? CustomerOrderId { get; set; }
        public int? CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float VatAmount { get; set; }
        public float Price { get; set; }
        public ProductModel Product { get; set; }
        public PatientModel Patient { get; set; }
    }
}