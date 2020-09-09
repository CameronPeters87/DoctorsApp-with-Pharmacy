using Sprint33.Areas.Store.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using Sprint33.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class CheckoutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private IPayment _payment = new Payment();

        // Get Paygate keys from webconfig file 
        readonly string PayGateID = ConfigurationManager.AppSettings["PAYGATEID"];
        readonly string PayGateKey = ConfigurationManager.AppSettings["PAYGATEKEY"];


        // GET: Store/Checkout
        public async Task<ActionResult> Index(int orderId, int customerId)
        {
            var model = new Checkout();

            model.CustomerOrder = await db.CustomerOrders.Where(o => o.Id == orderId &&
            o.CustomerId == customerId).FirstOrDefaultAsync();

            var currentCart = db.CustomerCarts.GetCartItemsFromOrder(customerId, orderId);

            // Product list
            foreach (var item in currentCart)
            {
                model.CartItems.Add(new CartItemsSummary
                {
                    Name = item.Product.Name,
                    Id = item.Product.Id,
                    Price = item.Product.IsOnSale ? item.Product.DiscountedPrice : item.Product.SellingPrice,
                    ImageUrl = item.Product.ImageUrl,
                    Quantity = item.Quantity
                });
            }

            Coupon coupon = new Coupon();

            if (!model.CustomerOrder.IsCouponNull())
            {
                model.CouponId = model.CustomerOrder.CouponId;
                coupon = db.Coupons.Find(model.CouponId);
                model.CouponCode = coupon.Code;
                model.CouponDiscount = coupon.DiscountRate;
            }

            return View(model);
        }

        // Post: Store/Checkout/ApplyCoupon
        [HttpPost]
        public async Task<string> ApplyCoupon(string code, int orderId)
        {
            if (code == string.Empty)
            {
                return "CodeNull";
            }

            var valid = db.Coupons.IsCouponCodeEnteredValid(code);
            var order = await db.CustomerOrders.FindAsync(orderId);
            if (order.CouponId != null)
            {
                return "CouponInUse";
            }

            var coupon = new Coupon();

            if (valid)
            {
                coupon = db.Coupons.GetCouponByCode(code);

                order.CouponId = coupon.Id;
                order.TotalCost -= (order.TotalCost * coupon.DiscountRate);


                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return "Success";
            }

            return "Failed";
        }

        [HttpPost]
        public string Confirm(Confirm model)
        {
            if (!ModelState.IsValid)
            {
                return "Failed";
            }



            return "Success";
        }

        public async Task<JsonResult> GetRequest(int orderId)
        {
            //Self entered value, hard coded.
            var orderDb = db.CustomerOrders.Find(orderId);

            int patientId = Convert.ToInt32(Session["id"]);
            Patient patient = db.Patients.Find(patientId);

            HttpClient http = new HttpClient();
            Dictionary<string, string> request = new Dictionary<string, string>();
            string paymentAmount = (orderDb.TotalCost * 100).ToString("00"); // amount int cents e.i 50 rands is 5000 cents

            request.Add("PAYGATE_ID", PayGateID);
            request.Add("REFERENCE", orderDb.Id.ToString()); // Payment ref e.g ORDER NUMBER
            request.Add("AMOUNT", paymentAmount);
            request.Add("CURRENCY", "ZAR"); // South Africa
            request.Add("RETURN_URL", $"{Request.Url.Scheme}://{Request.Url.Authority}/pay/completepayment");
            request.Add("TRANSACTION_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            request.Add("LOCALE", "en-za");
            request.Add("COUNTRY", "ZAF");
            request.Add("EMAIL", patient.Email);
            request.Add("CHECKSUM", _payment.GetMd5Hash(request, PayGateKey));

            //This place all request key values above to a string encoded in HTTP protocol
            string requestString = _payment.ToUrlEncodedString(request);
            StringContent content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");

            //HTTP Response from PAYGATE based on the Request sent to its website from GetRequest actionMethod from Pay Controller.
            HttpResponseMessage response = await http.PostAsync("https://secure.paygate.co.za/payweb3/initiate.trans", content);

            // if the request did not succeed, this line will make the program crash
            response.EnsureSuccessStatusCode();

            //Get the Content information from HTTP Responce and convert it a string
            string responseContent = await response.Content.ReadAsStringAsync();
            //Top 4 imperative information from Response (PAYGATE SERVER) to return to user website
            Dictionary<string, string> results = _payment.ToDictionary(responseContent);

            if (results.Keys.Contains("ERROR"))
            {
                return Json(new
                {
                    success = false,
                    message = "An error occured while initiating your request"
                }, JsonRequestBehavior.AllowGet);
            }
            //if MD5 is not true or equal produce the error
            if (!_payment.VerifyMd5Hash(results, PayGateKey, results["CHECKSUM"]))
            {
                return Json(new
                {
                    success = false,
                    message = "MD5 verification failed"
                }, JsonRequestBehavior.AllowGet);
            }

            bool IsRecorded = _payment.AddTransaction(request, results["PAY_REQUEST_ID"]);
            if (IsRecorded)
            {
                return Json(new
                {
                    success = true,
                    message = "Request completed successfully",
                    results
                }, JsonRequestBehavior.AllowGet);

            }
            return Json(new
            {
                success = false,
                message = "Failed to record a transaction"
            }, JsonRequestBehavior.AllowGet);
        }

    }
}