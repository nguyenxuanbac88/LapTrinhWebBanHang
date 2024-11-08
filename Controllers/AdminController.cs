using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var email = Session["Email"] as string;

        //    if (string.IsNullOrEmpty(email))
        //    {
        //        filterContext.Result = new RedirectToRouteResult(
        //            new System.Web.Routing.RouteValueDictionary
        //            {
        //        { "controller", "Account" }, // Tên controller cho trang đăng nhập
        //        { "action", "Sign_in" }
        //            });
        //        return;
        //    }

        //    // Kiểm tra quyền admin
        //    if (!UserServices.CheckAdmin(email))
        //    {
        //        filterContext.Result = new RedirectToRouteResult(
        //            new System.Web.Routing.RouteValueDictionary
        //            {
        //            { "controller", "Error" },
        //            { "action", "Error" },
        //            { "statusCode", 403 },
        //            { "message", "Bạn không có quyền truy cập trang này." }
        //                    });
        //    }


        //    base.OnActionExecuting(filterContext);
        //}



        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        // GET: Admin/ManageProducts
        public ActionResult ManageProducts()
        {
            // Bước 1: Lấy tất cả sản phẩm
            var products = db.Products.ToList();

            // Bước 2: Tạo danh sách ProductViewModel cho mỗi sản phẩm
            var productViewModels = new List<ProductViewModel>();

            foreach (var product in products)
            {
                var productViewModel = new ProductViewModel
                {
                    Product = product,
                    // Lấy tên danh mục
                    CategoryName = db.Categories
                        .Where(c => c.CategoryID == product.CategoryID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault(),
                    // Lấy danh sách ProductStocks và bao gồm SizeValue
                    ProductStocks = db.ProductStocks
                        .Where(ps => ps.ProductID == product.ProductID)
                        .Select(ps => new ProductStockViewModel
                        {
                            ProductStockID = ps.ProductStockID,
                            ProductID = (int)ps.ProductID,
                            ColorID = (int)ps.ColorID,
                            // Lấy SizeValue từ bảng Sizes dựa trên SizeID
                            SizeValue = db.Sizes
                                .Where(s => s.SizeID == ps.SizeID)
                                .Select(s => s.SizeValue)
                                .FirstOrDefault(),
                            Quantity = (int)ps.Quantity
                        })
                        .ToList(),
                    // Lấy danh sách ảnh phụ
                    ExistingImages = db.ImageProducts
                        .Where(img => img.ProductsID == product.ProductID)
                        .ToList()
                };

                // Thêm ProductViewModel vào danh sách
                productViewModels.Add(productViewModel);
            }

            // Bước 3: Trả về danh sách ProductViewModel vào view
            return View(productViewModels);
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
            var model = new ProductViewModel
            {
                Colors = db.Colors.ToList(),
                Sizes = db.Sizes.ToList()
            };
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(model);
        }

        // POST: Admin/CreateProducts
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateProducts(ProductViewModel model, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Khai báo allowedExtensions ở đầu
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                // Xử lý ảnh chính
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(ImageFile.FileName).ToLower();
                    if (allowedExtensions.Contains(extension))
                    {
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                        string path = Path.Combine(Server.MapPath("~/image/product-image/"), fileName);
                        ImageFile.SaveAs(path);
                        model.Product.ImageURL = fileName;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid file type. Please upload a JPG, JPEG, PNG, or GIF image.";
                        model.Colors = db.Colors.ToList();
                        model.Sizes = db.Sizes.ToList();
                        return View(model);
                    }
                }

                // Thêm sản phẩm vào bảng Products
                db.Products.Add(model.Product);
                db.SaveChanges();

                // Thêm thông tin sản phẩm vào bảng ProductStock
                var productStock = new ProductStock
                {
                    ProductID = model.Product.ProductID,
                    ColorID = model.SelectedColorID,
                    SizeID = model.SelectedSizeID,
                    Quantity = model.Quantity
                };
                db.ProductStocks.Add(productStock);
                db.SaveChanges();

                // Xử lý và lưu ảnh phụ
                if (model.AdditionalImages != null && model.AdditionalImages.Count > 0)
                {
                    foreach (var additionalImage in model.AdditionalImages)
                    {
                        if (additionalImage != null && additionalImage.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(additionalImage.FileName).ToLower();
                            if (allowedExtensions.Contains(extension))
                            {
                                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_sub" + extension;
                                string path = Path.Combine(Server.MapPath("~/image/product-image/"), fileName);
                                additionalImage.SaveAs(path);

                                var imageProduct = new ImageProduct
                                {
                                    ProductsID = model.Product.ProductID,
                                    ImageURL = fileName
                                };
                                db.ImageProducts.Add(imageProduct);
                            }
                        }
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("ManageProducts");
            }

            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName");
            model.Colors = db.Colors.ToList();
            model.Sizes = db.Sizes.ToList();
            return View(model);
        }


        public ActionResult EditProducts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Lấy sản phẩm từ cơ sở dữ liệu
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Tạo ViewModel và truyền dữ liệu vào ViewModel
            var model = new ProductViewModel
            {
                Product = product,
                SelectedColorID = (int)(db.ProductStocks.FirstOrDefault(ps => ps.ProductID == product.ProductID)?.ColorID),
                SelectedSizeID = (int)(db.ProductStocks.FirstOrDefault(ps => ps.ProductID == product.ProductID)?.SizeID),
                Quantity = db.ProductStocks.FirstOrDefault(ps => ps.ProductID == product.ProductID)?.Quantity ?? 0,
                Colors = db.Colors.ToList(),
                Sizes = db.Sizes.ToList()
            };

            // Truyền danh sách Categories cho ViewBag để hiển thị trong dropdown
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProducts(ProductViewModel model, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Xử lý ảnh chính nếu có file mới
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(ImageFile.FileName).ToLower();
                    if (allowedExtensions.Contains(extension))
                    {
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                        string path = Path.Combine(Server.MapPath("~/image/product-image/"), fileName);
                        ImageFile.SaveAs(path);
                        model.Product.ImageURL = fileName;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid file type. Please upload a JPG, JPEG, PNG, or GIF image.";
                        model.Colors = db.Colors.ToList();
                        model.Sizes = db.Sizes.ToList();
                        return View(model);
                    }
                }

                // Cập nhật thông tin sản phẩm
                db.Entry(model.Product).State = EntityState.Modified;

                // Cập nhật ProductStock
                var productStock = db.ProductStocks.FirstOrDefault(ps => ps.ProductID == model.Product.ProductID);
                if (productStock != null)
                {
                    productStock.ColorID = model.SelectedColorID;
                    productStock.SizeID = model.SelectedSizeID;
                    productStock.Quantity = model.Quantity;
                    db.Entry(productStock).State = EntityState.Modified;
                }
                else
                {
                    // Nếu ProductStock chưa tồn tại, tạo mới
                    db.ProductStocks.Add(new ProductStock
                    {
                        ProductID = model.Product.ProductID,
                        ColorID = model.SelectedColorID,
                        SizeID = model.SelectedSizeID,
                        Quantity = model.Quantity
                    });
                }

                db.SaveChanges();
                return RedirectToAction("ManageProducts");
            }

            // Truyền lại dữ liệu cho View nếu có lỗi
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName", model.Product.CategoryID);
            model.Colors = db.Colors.ToList();
            model.Sizes = db.Sizes.ToList();
            return View(model);
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
            // Tìm sản phẩm
            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            // Xóa các bản ghi liên quan trong ProductStock
            var productStocks = db.ProductStocks.Where(ps => ps.ProductID == id).ToList();
            foreach (var stock in productStocks)
            {
                db.ProductStocks.Remove(stock);
            }

            // Xóa các bản ghi liên quan trong ImageProducts
            var imageProducts = db.ImageProducts.Where(img => img.ProductsID == id).ToList();
            foreach (var image in imageProducts)
            {
                db.ImageProducts.Remove(image);
            }

            // Sau khi xóa các bản ghi liên quan, xóa sản phẩm chính
            db.Products.Remove(product);

            // Lưu thay đổi
            db.SaveChanges();

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
