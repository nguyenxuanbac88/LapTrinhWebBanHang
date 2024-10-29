using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LapTrinhWebBanHang.Models;
using LapTrinhWebBanHang.Services;

namespace LapTrinhWebBanHang.Controllers
{
    public class AdminController : Controller
    {
        private WebsiteEntities4 db = new WebsiteEntities4(); // Sử dụng DbContext đã được tạo từ Entity Framework
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var email = Session["Email"] as string;

            if (string.IsNullOrEmpty(email))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                { "controller", "Account" }, // Tên controller cho trang đăng nhập
                { "action", "Sign_in" }
                    });
                return;
            }

            // Kiểm tra quyền admin
            if (!UserServices.CheckAdmin(email))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                    { "controller", "Error" },
                    { "action", "Error" },
                    { "statusCode", 403 },
                    { "message", "Bạn không có quyền truy cập trang này." }
                            });
            }


            base.OnActionExecuting(filterContext);
        }



        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        // GET: Admin/ManageProducts
        public ActionResult ManageProducts()
        {
            var products = db.Products.ToList();  // Lấy danh sách tất cả sản phẩm
            return View(products); // Trả về view với danh sách sản phẩm
        }

        // GET: Admin/ManageProducts
        public ActionResult ManageCategory()
        {
            var ManageCategory = db.Categories.ToList();  // Lấy danh sách tất cả sản phẩm
            return View(ManageCategory); // Trả về view với danh sách sản phẩm
        }

        // GET: Admin/CreateProducts
        public ActionResult CreateProducts()
        {
            // Đưa danh mục (Categories) vào ViewBag nếu cần dùng
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/CreateProducts
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateProducts(Product product, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu có file ảnh được upload
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // Validate file extension to ensure it is an image
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    string extension = Path.GetExtension(ImageFile.FileName).ToLower();

                    if (allowedExtensions.Contains(extension))
                    {
                        // Generate a unique file name with timestamp
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

                        // Construct the file path for saving
                        string path = Path.Combine(Server.MapPath("~/image/product-image/"), fileName);

                        try
                        {
                            // Save the image file to the specified directory
                            ImageFile.SaveAs(path);

                            // Set the file name to the product's ImageURL property
                            product.ImageURL = fileName;
                        }
                        catch (Exception ex)
                        {
                            // Log the exception and handle it as needed
                            // Example: Log.Error("Failed to save image", ex);
                            ViewBag.ErrorMessage = "There was an error saving the image. Please try again.";
                        }
                    }
                    else
                    {
                        // Handle invalid file type
                        ViewBag.ErrorMessage = "Invalid file type. Please upload a JPG, JPEG, PNG, or GIF image.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Please select an image to upload.";
                }


                // Thêm sản phẩm vào cơ sở dữ liệu
                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("ManageProducts");
            }

            // Nếu có lỗi, trả lại view với dữ liệu đã nhập
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(product);
        }


        // GET: Admin/EditProducts/5
        public ActionResult EditProducts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
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



        // POST: Admin/EditProducts/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProducts(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;  // Đánh dấu sản phẩm để cập nhật
                db.SaveChanges();  // Lưu 
                return RedirectToAction("ManageProducts");
            }
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/DeleteProducts/5
        public ActionResult DeleteProducts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/DeleteProducts/5
        [HttpPost, ActionName("DeleteProducts")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);  // Xóa sản phẩm
            db.SaveChanges();  // Lưu
            return RedirectToAction("ManageProducts");
        }


        // POST: Admin/CreateProducts
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)  // Kiểm tra xem dữ liệu trong model có hợp lệ hay không
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("ManageCategory");
            }

            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(category);
        }


        public ActionResult CreateCategory()
        {
            return View();
        }
        // GET: Admin/EditCategory/5
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category); // Pass the single category to the view
        }
    }
}
