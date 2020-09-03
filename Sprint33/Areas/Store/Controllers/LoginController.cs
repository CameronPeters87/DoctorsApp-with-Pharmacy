using Sprint33.Models;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class LoginController : Controller
    {
        // Post: Store/Login
        [HttpPost]
        public string Index(string email, string password)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var item in db.Patients)
                {
                    if (item.Email.Equals(email) && item.Password.Equals(password))
                    {
                        Session["id"] = item.UserID;
                        Session["UserName"] = item.FirstName + " " + item.Surname;
                        Session["IsLoyal"] = item.isLoyal;
                        Session["Title"] = "Patient";
                        return "Success";
                    }
                    else
                    {
                        return "Failed";
                    }
                }

            }

            return "";
        }
    }
}