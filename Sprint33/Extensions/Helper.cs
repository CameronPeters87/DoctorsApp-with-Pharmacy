using Geocoding;
using Geocoding.Google;
using Sprint33.Models.CovidModels;
using System.Linq;

namespace Sprint33.Extensions
{
    public static class Helper
    {
        public static string FormatIntToString(int integerValue)
        {
            return integerValue.ToString("N0");
        }

        public static double GetLongFromCountryName(string country)
        {
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyCPWzQ0h1vedStiQWFQ5Ez1Jf2f1rj209Q" };
            var addresses = geocoder.Geocode(country);

            return addresses.First().Coordinates.Longitude;
        }

        public static double GetLongFromCountry(this CountryViewModel country,
            IGeocoder geocoder)
        {
            var addresses = geocoder.Geocode(country.Country);

            return addresses.First().Coordinates.Longitude;
        }

        public static double GetLatFromCountry(this CountryViewModel country,
            IGeocoder geocoder)
        {
            var addresses = geocoder.Geocode(country.Country);

            return addresses.First().Coordinates.Latitude;
        }

        //public static async Task<bool> CheckDBForLatLongExists(this SummaryViewModel summary,
        //    ApplicationDbContext db, IGeocoder geocoder)
        //{
        //    var dto = await db.LatLongDtos.ToListAsync();

        //    if (!dto.Any())
        //    {
        //        double position_Long;
        //        double position_Lat;
        //        string country;
        //        foreach (var item in summary.Countries)
        //        {
        //            position_Long = item.GetLongFromCountry(geocoder);
        //            position_Lat = item.GetLatFromCountry(geocoder);
        //            country = item.Country;

        //            db.LatLongDtos.Add(new LatLongDTO
        //            {
        //                Country = country,
        //                Latitude = position_Lat,
        //                Longitude = position_Long
        //            });

        //            await db.SaveChangesAsync();
        //        }

        //        return true;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}