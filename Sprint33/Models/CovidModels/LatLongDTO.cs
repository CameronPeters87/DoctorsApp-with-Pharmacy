using System.ComponentModel.DataAnnotations;

namespace Sprint33.Models.CovidModels
{
    public class LatLongDTO
    {
        [Key]
        public int Id { get; set; }

        public string Country { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}