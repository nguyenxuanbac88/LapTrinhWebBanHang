using LapTrinhWebBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class CategoryController : Controller
    {
        private WebsiteEntities4 db = new WebsiteEntities4(); // Sử dụng DbContext đã được tạo từ Entity Framework
                                                              // GET: Category
        public ActionResult CategoryPage(int? id)
        {
            // Kiểm tra nếu id là null, lấy tất cả danh mục
            var category = id.HasValue ? db.Categories.Find(id.Value) : null;

            if (!id.HasValue || category == null) 
            {
                // Lấy tất cả sản phẩm và sắp xếp theo ID từ lớn đến bé
                var products = db.Products
                    .OrderByDescending(p => p.ProductID) // Sắp xếp theo ID từ lớn đến bé
                    .ToList();

                ViewBag.CategoryName = "Tất cả sản phẩm";
                return View(products); // Trả về tất cả sản phẩm
            }

            // Lấy tất cả danh mục từ database
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;

            // Lọc sản phẩm theo CategoryID được truyền vào và sắp xếp theo ID từ lớn đến bé
            var productsByCategory = db.Products
                .Where(p => p.CategoryID == id.Value)
                .OrderByDescending(p => p.ProductID) // Sắp xếp theo ID từ lớn đến bé
                .ToList();

            ViewBag.CategoryName = category.CategoryName;
            ViewBag.CategoryID = category.CategoryID;
            return View(productsByCategory);

        }


        public ActionResult GetAllProducts()
        {
            var products = db.Products
                .OrderByDescending(p => p.ProductID) // Sắp xếp theo ID từ lớn đến bé
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
        public ActionResult SortProductsName(string data)
        {
            var products = db.Products
                .Where(p => p.ProductName.Contains(data)) // Chỉ lấy sản phẩm có tên chứa "data"
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
        public ActionResult SortProductsPrice(int price)
        {
            var products = db.Products
                .Where(p => p.Price > price) // Chỉ lấy sản phẩm có giá trên 1000
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
        public ActionResult GetAllProducts(int? IdCategory)
        {
            // Lấy danh sách sản phẩm từ cơ sở dữ liệu, có kiểm tra điều kiện categoryId
            var products = db.Products
                .Where(p => !IdCategory.HasValue || p.CategoryID == IdCategory) // Nếu có categoryId, lọc theo ID, nếu không lấy tất cả
                .OrderByDescending(p => p.ProductID) // Sắp xếp theo ID từ lớn đến bé
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Price,
                    p.ImageURL,
                    p.Description,
                    // Truy vấn lấy tên danh mục từ bảng Categories
                    CategoryName = db.Categories
                        .Where(c => c.CategoryID == p.CategoryID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault()
                })
                .ToList();

            return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllProductsJson(string sortBy)
        {
            var products = db.Products.AsQueryable();

            // Sắp xếp sản phẩm dựa trên giá trị của `sortBy`
            switch (sortBy)
            {
                case "Newest":
                    products = products.OrderByDescending(p => p.ProductID); // Sắp xếp sản phẩm mới nhất
                    break;
                case "Oldest":
                    products = products.OrderBy(p => p.ProductID); // Sắp xếp cũ nhất
                    break;
                case "PriceDesc":
                    products = products.OrderByDescending(p => p.Price); // Sắp xếp giá từ cao tới thấp
                    break;
                case "PriceAsc":
                    products = products.OrderBy(p => p.Price); // Sắp xếp giá từ thấp tới cao
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName); // Sắp xếp mặc định theo tên sản phẩm
                    break;
            }

            // Lấy danh sách sản phẩm và trả về dưới dạng JSON
            var productList = products.Select(p => new
            {
                p.ProductID,
                p.ProductName,
                p.Price,
                p.ImageURL,
                p.Description
            }).ToList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }

    }
}