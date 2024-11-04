using LapTrinhWebBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class ProductPageController : Controller
    {
        private WebsiteEntities4 db = new WebsiteEntities4(); // Sử dụng DbContext đã được tạo từ Entity Framework
        // GET: ProductPage
        public ActionResult ProductPage()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProduct(int id)
        {
            var product = db.Products
                .Where(p => p.ProductID == id)
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Price,
                    p.Description,
                    p.ImageURL,
                    p.CategoryID
                })
                .FirstOrDefault();

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, data = product }, JsonRequestBehavior.AllowGet);
        }

    }
}