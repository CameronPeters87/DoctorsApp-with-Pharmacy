using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Store/Orders
        public ActionResult Index()
        {
            return View();
        }
    }
}