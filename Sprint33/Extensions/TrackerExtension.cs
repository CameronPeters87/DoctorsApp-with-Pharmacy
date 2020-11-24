using Sprint33.Models.CovidModels;
using System.Linq;
using System.Threading.Tasks;

namespace Sprint33.Extensions
{
    public static class TrackerExtension
    {
        public static void FillTrackerModel(this TrackerViewModel tracker,
            SummaryViewModel summary, NewsViewModel news)
        {
            tracker.Country_SouthAfrica = summary.Countries.Where(s => s.Country == "South Africa")
                .FirstOrDefault();

            tracker.RSA_TotalConfirmed =  Helper.FormatIntToString(
                tracker.Country_SouthAfrica.TotalConfirmed);

            tracker.RSA_TotalRecovered =  Helper.FormatIntToString(
                tracker.Country_SouthAfrica.TotalRecovered);

            tracker.RSA_TotalDeaths =  Helper.FormatIntToString(
                tracker.Country_SouthAfrica.TotalDeaths);

            // Global
            tracker.Global_TotalConfirmed =  Helper.FormatIntToString(
                summary.Global.TotalConfirmed);
            tracker.Global_TotalRecovered =  Helper.FormatIntToString(
                summary.Global.TotalRecovered);
            tracker.Global_TotalDeaths =  Helper.FormatIntToString(
                summary.Global.TotalDeaths);

            // News
            tracker.NewsViewModel = news;
            tracker.NewsViewModel.Articles = tracker.NewsViewModel.Articles.Take(5).ToList();
            foreach (var item in tracker.NewsViewModel.Articles)
            {
                item.PublishedAt = item.PublishedAt.Substring(0, 10);
            }
        }
    }
}