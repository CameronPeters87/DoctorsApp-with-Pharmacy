﻿using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.Services;
using Sprint33.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class PurchaseController : Controller
    {
        private IPayment _payment = new Payment();
        private ICustomerOrderRepository customerOrderRepository = new CustomerOrderRepository();
        private ApplicationDbContext db = new ApplicationDbContext();

        // Get Paygate keys from webconfig file 
        readonly string PayGateID = ConfigurationManager.AppSettings["PAYGATEID"];
        readonly string PayGateKey = ConfigurationManager.AppSettings["PAYGATEKEY"];

        // GET: Pay
        public ActionResult Index()
        {
            return View();
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
            request.Add("RETURN_URL", $"{Request.Url.Scheme}://{Request.Url.Authority}/store/purchase/completepayment");
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

        [HttpPost]
        public async Task<ActionResult> CompletePayment()
        {
            string responseContent = Request.Params.ToString();
            Dictionary<string, string> results = _payment.ToDictionary(responseContent);

            Transaction transaction = _payment.GetTransaction(results["PAY_REQUEST_ID"]);

            if (transaction == null)
            {
                // Unable to reconsile transaction
                return RedirectToAction("Failed");
            }

            // Reorder attributes for MD5 check
            Dictionary<string, string> validationSet = new Dictionary<string, string>();
            validationSet.Add("PAYGATE_ID", PayGateID);
            validationSet.Add("PAY_REQUEST_ID", results["PAY_REQUEST_ID"]);
            validationSet.Add("TRANSACTION_STATUS", results["TRANSACTION_STATUS"]);
            validationSet.Add("REFERENCE", transaction.REFERENCE);

            if (!_payment.VerifyMd5Hash(validationSet, PayGateKey, results["CHECKSUM"]))
            {
                // checksum error
                return RedirectToAction("Failed");
            }
            /** Payment Status 
             * -2 = Unable to reconsile transaction
             * -1 = Checksum Error
             * 0 = Pending
             * 1 = Approved
             * 2 = Declined
             * 3 = Cancelled
             * 4 = User Cancelled
             */
            int paymentStatus = int.Parse(results["TRANSACTION_STATUS"]);
            if (paymentStatus == 1)
            {
                // Yey, payment approved
                // Do something useful
            }
            // Query paygate transaction details
            // And update user transaction on your database
            await VerifyTransaction(responseContent, transaction.REFERENCE);
            return RedirectToAction("Complete", new { id = results["TRANSACTION_STATUS"], area = "store" });
        }

        private async Task VerifyTransaction(string responseContent, string Referrence)
        {
            HttpClient client = new HttpClient();
            Dictionary<string, string> response = _payment.ToDictionary(responseContent);
            Dictionary<string, string> request = new Dictionary<string, string>();

            request.Add("PAYGATE_ID", PayGateID);
            request.Add("PAY_REQUEST_ID", response["PAY_REQUEST_ID"]);
            request.Add("REFERENCE", Referrence);
            request.Add("CHECKSUM", _payment.GetMd5Hash(request, PayGateKey));

            string requestString = _payment.ToUrlEncodedString(request);

            StringContent content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");

            // ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpResponseMessage res = await client.PostAsync("https://secure.paygate.co.za/payweb3/query.trans", content);
            res.EnsureSuccessStatusCode();

            string _responseContent = await res.Content.ReadAsStringAsync();

            Dictionary<string, string> results = _payment.ToDictionary(_responseContent);
            if (!results.Keys.Contains("ERROR"))
            {
                _payment.UpdateTransaction(results, results["PAY_REQUEST_ID"]);
            }
        }

        public ActionResult Complete(int? id)
        {
            var orderId = Convert.ToInt32(Session["CustomerOrderId"]);
            var orderId1 = customerOrderRepository.GetOrderId(this.HttpContext);
            var patientId = Convert.ToInt32(Session["id"]);

            var order = db.CustomerOrders.Find(orderId1);
            var patient = db.Patients.Find(patientId);

            string status = "Unknown";
            switch (id.ToString())
            {
                case "-2":
                    status = "Unable to reconsile transaction";
                    break;
                case "-1":
                    status = "Checksum Error. The values have been altered";
                    break;
                case "0":
                    status = "Not Done";
                    break;
                case "1":
                    //status = "Approved";
                    status = customerOrderRepository.PaymentApproved(this.HttpContext);
                    break;
                case "2":
                    //status = "Declined";
                    status = customerOrderRepository.PaymentDeclined(this.HttpContext);

                    break;
                case "3":
                    status = "Cancelled";
                    break;
                case "4":
                    status = "User Cancelled";
                    break;
                default:
                    status = $"Unknown Status({ id })";
                    break;
            }
            TempData["Status"] = status;

            if (status == "Approved")
            {
                var callbackUrl = Url.Action("view-order", "order", new { area = "store", id = order.Id }, protocol: Request.Url.Scheme);
                var loyaltyLink = Url.Action("Subscribe", "Loyalties", new { patientId = order.Customer.UserID });

                //EmailExtensions.SendMail(order.Email, "Doctor J Govender Pharmacy: Thank you for you Purchase!",
                //    string.Format("<h1><strong>Hello {0}</strong></h1> <br><br>" +
                //                  "Thank you for your recent transaction on our Pharmacy." +
                //                  "If you are new to our store and not a loyalty member, you can sign up for free for rewards.<br><br>" +
                //    "Your order has being successfully approved and is being processed.<br>" +
                //    "<a href=\"" + callbackUrl + "\">View Order Here</a></strong>", order.Customer.FirstName));

                //EmailExtensions.SendSms(order.Email, "Doctor J Govender Pharmacy: Thank you for you Purchase! Your order has being successfully approved and is being processed." + callbackUrl);
                EmailExtensions.SendSms(order.PhoneNumber, "Doctor J Govender Pharmacy: Thank you for your purchase! Total R" + order.TotalCost);

                //var client = new Client(creds: new Nexmo.Api.Request.Credentials
                //{
                //    ApiKey = "0f48d10b",
                //    ApiSecret = "R0hfbIdGjgNcG9Aa"
                //});

                //var results = client.SMS.Send(request: new SMS.SMSRequest
                //{
                //    from = "Dr J Govender",
                //    to = order.PhoneNumber,
                //    text = "Doctor J Govender Pharmacy: Thank you for you Purchase!"
                //});

                db.Notifications.PushNotificaiton(string.Format("A Customer placed an order: #{0}", order.Id));

                if (patient.isLoyal)
                {
                    var loyalty = db.Loyalties.Where(l => l.PatientId == patientId).FirstOrDefault();
                    var prefs = db.LoyaltyPreferences.FirstOrDefault();

                    loyalty.Loyalty_Points += 25;

                    if (loyalty.Loyalty_Points >= prefs.PointsLimit)
                    {
                        db.Coupons.Add(new PharmacyEntities.Coupon
                        {
                            isLoyaltyCoupon = true,
                            Code = string.Format("{0}-{1}-{2}", prefs.CouponCode, patient.UserID, order.Id),
                            Description = "Loyalty Coupon",
                            EndDate = DateTime.Now.AddMonths(prefs.MonthsToExpiry),
                            StartDate = DateTime.Today,
                            DiscountRate = prefs.CouponDiscountRate,
                            Active = true,
                            Display = false
                        });

                        db.SaveChanges();

                        var coup = db.Coupons.OrderByDescending(c => c.Id).FirstOrDefault();

                        //EmailExtensions.SendMail(patient.Email, prefs.Subject,
                        //    string.Format("<h1 class='text-center'>Code: {0}-{1}-{2}</h1> <br><br>" +
                        //                  "Valid until {3}<br>" +
                        //                  "Discount Rate: {4}%" +
                        //                  "{5}", prefs.CouponCode, patient.UserID, order.Id, coup.EndDate.ToLongDateString(), coup.DiscountRate, prefs.Body));


                        EmailExtensions.SendSms(patient.ContactNumber, string.Format(prefs.Subject + "Code: {0}-{1}-{2} . Valid until {3}. Discount Rate: {4}%", prefs.CouponCode, patient.UserID, order.Id, coup.EndDate.ToLongDateString(), coup.DiscountRate));
                        loyalty.Loyalty_Points = 0;
                    }
                    db.Entry(loyalty).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }

            return View();
        }

    }
}