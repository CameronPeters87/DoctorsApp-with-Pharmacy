using Geocoding;
using Geocoding.Google;
using Newtonsoft.Json;
using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.Models.CovidModels;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Controllers
{
    public class CovidController : Controller
    {
        private string baseUrl = "https://api.covid19api.com/";
        private string baseUrlNews = "http://newsapi.org/";
        private IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyCPWzQ0h1vedStiQWFQ5Ez1Jf2f1rj209Q" };
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Covid
        public async Task<ActionResult> Index()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            SummaryViewModel summary = new SummaryViewModel();
            NewsAPI newsAPI = new NewsAPI();
            TrackerViewModel tracker = new TrackerViewModel();

            #region HttpClient Config
            // Covid Stats
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                HttpResponseMessage responseMessage = await client.GetAsync("summary");

                if (responseMessage.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var covidResponse = responseMessage.Content
                        .ReadAsStringAsync().Result;

                    summary = JsonConvert.DeserializeObject<SummaryViewModel>(covidResponse);
                }
            }
            // South Africa News
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlNews);

                HttpResponseMessage responseMessage = await client.GetAsync("v2/top-headlines?country=za&category=health&apiKey=8066df69f5c2435c9a6b0510ea7b16d5");

                if (responseMessage.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var newsResponse = responseMessage.Content
                        .ReadAsStringAsync().Result;

                    newsAPI = JsonConvert.DeserializeObject<NewsAPI>(newsResponse);
                }
            }
            #endregion

            NewsViewModel news = new NewsViewModel
            {
                Articles = newsAPI.Articles.ToList()
            };

            tracker.FillTrackerModel(summary, news);

            //var latLongTableExists = await summary.CheckDBForLatLongExists(db, geocoder);

            //if (latLongTableExists)
            //{
            //    ViewBag.DataPoints = db.LatLongDtos.ToList();
            //}

            watch.Stop();

            return View(tracker);
        }
    }
}