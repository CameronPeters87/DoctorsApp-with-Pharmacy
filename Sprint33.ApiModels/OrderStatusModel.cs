namespace Sprint33.ApiModels
{
    public class OrderStatusModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public bool isPaid { get; set; }
    }
}