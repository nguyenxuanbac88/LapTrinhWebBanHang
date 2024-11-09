using LapTrinhWebBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class ProductPageController : Controller
    {
        private WebsiteEntities4 db = new WebsiteEntities4(); // Sử dụng DbContext đã được tạo từ Entity Framework
                                                              // GET: ProductPage
        public ActionResult ProductPage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.Products.Find(id); // db là context của Entity Framework

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product); // Truyền sản phẩm vào View
        }


        /*Note dành riêng cho dev frontEnd sử dụng
         {
              "success": true,
              "data": {
                "ProductID": 1,
                "ProductName": "Product A",
                "Price": 100.00,
                "Description": "Description of Product A",
                "ImageURL": "/images/productA.jpg",
                "CategoryID": 2,
                "IsDiscounted": true,
                "DiscountAmount": 10.00,
                "DiscountPercentage": null,
                "SupplementaryImages": [
                  "/images/productA_1.jpg",
                  "/images/productA_2.jpg"
                ]
              }
            }
         */


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
                    p.CategoryID,
                    // Kiểm tra nếu sản phẩm có khuyến mãi đang hoạt động
                    IsDiscounted = db.ProductPromotions
                        .Any(pp => pp.ProductID == p.ProductID &&
                                   db.Promotions.Any(pr => pr.PromotionID == pp.PromotionID &&
                                                           pr.StartDate <= DateTime.Now &&
                                                           pr.EndDate >= DateTime.Now)),
                    // Lấy chi tiết khuyến mãi nếu có
                    DiscountAmount = db.ProductPromotions
                        .Where(pp => pp.ProductID == p.ProductID)
                        .Select(pp => db.Promotions
                            .Where(pr => pr.PromotionID == pp.PromotionID &&
                                         pr.StartDate <= DateTime.Now &&
                                         pr.EndDate >= DateTime.Now)
                            .Select(pr => pr.DiscountAmount)
                            .FirstOrDefault())
                        .FirstOrDefault(),
                    DiscountPercentage = db.ProductPromotions
                        .Where(pp => pp.ProductID == p.ProductID)
                        .Select(pp => db.Promotions
                            .Where(pr => pr.PromotionID == pp.PromotionID &&
                                         pr.StartDate <= DateTime.Now &&
                                         pr.EndDate >= DateTime.Now)
                            .Select(pr => pr.DiscountPercentage)
                            .FirstOrDefault())
                        .FirstOrDefault(),
                    // Lấy danh sách ảnh phụ
                    SupplementaryImages = db.ImageProducts
                        .Where(img => img.ProductsID == p.ProductID)
                        .Select(img => img.ImageURL)
                        .ToList()
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