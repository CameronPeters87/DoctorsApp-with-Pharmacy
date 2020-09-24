namespace Sprint33.ApiModels
{
    public class AddressModel
    {
        public int Id { get; set; }
        public string Street_Number { get; set; }
        public string Route { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}