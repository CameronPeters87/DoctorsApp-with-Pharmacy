using Sprint33.Models;
using Sprint33.Models.ViewModel;
using Sprint33.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sprint33.Controllers
{
    public class PayController : Controller
    {   
        //To get OrderID from a loading View
        private static int getOrderIDD = 0;
        
        private IPayment _payment = new Payment();

        // Get Paygate keys from webconfig file 
        readonly string PayGateID = ConfigurationManager.AppSettings["PAYGATEID"];
        readonly string PayGateKey = ConfigurationManager.AppSettings["PAYGATEKEY"];

        // Calling Database context
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? id)
        {
            //Order order = new Order();
            //load created order record on to payment homepage
            var order = db.Orders.Where(o => o.OrderID == id).FirstOrDefault();

            //if (order.OrderID >= 1)
            //{
                //IF 
                //order = db.Orders.Where(o => o.OrderID == (OrdersController.getOrderIDs == 0 ? 1 : OrdersController.getOrderIDs)).SingleOrDefault();
            //}
                
            //OrderId for GetRequest. 
            getOrderIDD = order.OrderID;

            var model = new OrderVM
            {
                OrderID = order.OrderID,
                AppointmentId = order.AppointmentId,
                PatientId = order.PatientId,
                PatientName = order.PatientName,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice
            };

            //view order on payment page
            return View(order);
        }
        public ActionResult Index2(int? id)
        {
            //Order order = new Order();
            //load created order record on to payment homepage
            var order = db.Orders.Where(o => o.OrderID == id).FirstOrDefault();

            //if (order.OrderID >= 1)
            //{
            //IF 
            //order = db.Orders.Where(o => o.OrderID == (OrdersController.getOrderIDs == 0 ? 1 : OrdersController.getOrderIDs)).SingleOrDefault();
            //}

            //OrderId for GetRequest. 
            getOrderIDD = order.OrderID;

            var model = new OrderVM
            {
                OrderID = order.OrderID,
                AppointmentId = order.AppointmentId,
                PatientId = order.PatientId,
                PatientName = order.PatientName,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                LoyaltyPurchase = order.LoyaltyPurchase,
                isApproved = order.isApproved
            };

            //view order on payment page
            return View(model);
        }


        public async Task<JsonResult> GetRequest()
        {
            //Sample Order
            Order orderDb = new Order();
            //Self entered value, hard coded.
            orderDb = db.Orders.Find(getOrderIDD);
            int patientId = Convert.ToInt32(Session["id"]);

            HttpClient http = new HttpClient();
            Dictionary<string, string> request = new Dictionary<string, string>();
            string paymentAmount = (orderDb.TotalPrice * 100).ToString("00"); // amount int cents e.i 50 rands is 5000 cents

            request.Add("PAYGATE_ID", PayGateID);
            request.Add("REFERENCE", orderDb.OrderID.ToString()); // Payment ref e.g ORDER NUMBER
            request.Add("AMOUNT", paymentAmount);
            request.Add("CURRENCY", "ZAR"); // South Africa
            request.Add("RETURN_URL", $"{Request.Url.Scheme}://{Request.Url.Authority}/pay/completepayment");
            request.Add("TRANSACTION_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            request.Add("LOCALE", "en-za");
            request.Add("COUNTRY", "ZAF");

            // get authenticated user's email
            // use a valid email, paygate will send a transaction confirmation to it
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    //request.Add("EMAIL", _payment.GetAuthenticatedUser().Email);
            //    request.Add("EMAIL", "cameronpeters87@gmail.com");
            //} else
            if (Session["Title"] == "Patient")
            {
                //request.Add("EMAIL", _payment.GetAuthenticatedUser().Email);
                Patient patient = db.Patients.Find(patientId);

                request.Add("EMAIL", patient.Email);
            } else
            {
            // put your own email address for the payment confirmation (developer only)
                request.Add("EMAIL", "21825534@dut4life.ac.za");
            }
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

        // This is your return url from Paygate
        // This will run when you complete payment
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
            if(paymentStatus == 1)
            {
                // Yey, payment approved
                // Do something useful
            }
            // Query paygate transaction details
            // And update user transaction on your database
            await VerifyTransaction(responseContent, transaction.REFERENCE);
            return RedirectToAction("Complete", new { id = results["TRANSACTION_STATUS"] });
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
                    status = Approved();
                    break;
                case "2":
                    //status = "Declined";
                    status = Declined();

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

            return View();
        }

        public string Approved()
        {
            var sessionId = Session["id"];
            var appointmentId = Session["AppointmentId"];
            var OrderId = Session["orderId"];
            int appId = Convert.ToInt32(appointmentId);
            int ordId = Convert.ToInt32(OrderId);
            int patientId = Convert.ToInt32(sessionId);

            Appointment appointment = db.Appointments.Where(a => a.AppointmentID.Equals(appId))
                .FirstOrDefault();
            Loyalty loyalty = db.Loyalties.Where(l => l.PatientId.Equals(patientId))
                .FirstOrDefault();
            Order order = db.Orders.Where(l => l.OrderID.Equals(ordId))
                .FirstOrDefault();

            if(loyalty == null)
            {
                loyalty = new Loyalty();
            }
            else
            {
                if (loyalty.Loyalty_Points >= 100)
                {
                    loyalty.Loyalty_Points = 0;
                }
                else
                {
                    loyalty.Loyalty_Points += 10;
                }
                db.Entry(loyalty).State = EntityState.Modified;
            }

            appointment.isPaid = true;
            order.isApproved = true;

            db.Entry(appointment).State = EntityState.Modified;
            db.SaveChanges();

            return "Approved";
        }

        public string Declined()
        {
            var sessionId = Session["id"];
            int patientId = Convert.ToInt32(sessionId);

            Appointment appointment = db.Appointments.Where(a => a.PatientID.Equals(patientId))
                .FirstOrDefault();
            Order order = db.Orders.Where(l => l.PatientId.Equals(patientId))
                .FirstOrDefault();

            appointment.isPaid = false;
            order.isApproved = false;

            db.Entry(appointment).State = EntityState.Modified;
            db.SaveChanges();

            return "Declined";
        }

    }
}
