using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class CustomerOrdersController : Controller
    {
        // GET: Pharmacist/CustomerOrders
        public ActionResult Index()
        {
            return View();
        }
    }
}