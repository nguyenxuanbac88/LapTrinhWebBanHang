using LapTrinhWebBanHang.Models;
using LapTrinhWebBanHang.Services;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LapTrinhWebBanHang.Controllers
{
    public class HomePageController : Controller
    {
        private WebsiteEntities4 db = new WebsiteEntities4(); // Sử dụng DbContext đã được tạo từ Entity Framework
        // GET: HomePage
        public ActionResult Home_page()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetNoStore();

            return View();
        }
        [HttpGet]
        public JsonResult GetAllProducts()
        {
            var products = db.Products
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Price,
                    p.ImageURL,
                    p.CategoryID

                })
                .ToList();

            return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
        }


    }
}