namespace Sprint33.Models.CovidModels
{
    public class TrackerViewModel
    {
        public CountryViewModel Country_SouthAfrica { get; set; }

        //South Africa Stats
        public string RSA_TotalConfirmed { get; set; }
        public string RSA_TotalRecovered { get; set; }
        public string RSA_TotalDeaths { get; set; }

        // Global
        public string Global_TotalConfirmed { get; set; }
        public string Global_TotalRecovered { get; set; }
        public string Global_TotalDeaths { get; set; }

        // News
        public NewsViewModel NewsViewModel { get; set; }
    }
}