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
        public ActionResult GetAllProducts()
        {
            var products = db.Products
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Price,
                    p.ImageURL,
                    CategoryName = db.Categories
                        .Where(c => c.CategoryID == p.CategoryID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault()
                })
                .ToList();

            return View(products); // Trả về View với model là danh sách sản phẩm
        }

        [HttpGet]
        public JsonResult GetAllProductsJson()
        {
            var products = db.Products
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Price,
                    p.ImageURL,
                    // Truy vấn lấy tên danh mục từ bảng Categories
                    CategoryName = db.Categories
                        .Where(c => c.CategoryID == p.CategoryID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault()
                })
                .ToList();

            return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
        }

    }
}