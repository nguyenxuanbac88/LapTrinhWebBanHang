using LapTrinhWebBanHang.Models;
using LapTrinhWebBanHang.Services;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace LapTrinhWebBanHang.Controllers
{

    public class AdminController : Controller
    {
        WebsiteEntities4 db = new WebsiteEntities4();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManageProducts()
        {
            using (var db = new WebsiteEntities4())
            {
                if (Session["IdUser"] == null)
                {
                    return RedirectToAction("Login", "Sign_in");
                }
                var userId = (int)Session["IdUser"];
                var user = db.Users.SingleOrDefault(u => u.IdUser == userId);

                if (user == null || user.IsAdmin != 1)
                {
                    return RedirectToAction("Index", "Home");
                }

                var products = db.Products.ToList();
                return View(products);
            }
        }
    }
}