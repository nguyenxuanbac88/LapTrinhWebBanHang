using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
            //lấy số dư tháng hiện tại và truyền vào ViewBag
            ViewBag.CurrentMonthBalance = GetCurrentMonthBalance();
            //Lấy số dư năm hiện tại 
            ViewBag.CurrentYearBalance = GetCurrentYearBalance();
            //Lấy số lượng đơn hàng tháng hiện tại
            ViewBag.CurrentMonthOrderCount = GetCurrentMonthOrderCount();
            //Tính tổng số người dùng
            ViewBag.CurrentTotalUserCount = GetTotalUserCount();
            return View();
        }

        // GET: Admin/ManageProducts
        public ActionResult ManageProducts()
        {
            //Lấy tất cả sản phẩm
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
            var categories = db.Categories.ToList();  // Lấy danh sách tất cả loại sản phẩm
            return View(categories); // Trả về view với danh sách loại sản phẩm
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
        //[ValidateAntiForgeryToken]
        public ActionResult CreateProduct()
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
        public ActionResult CreateProducts(ProductViewModel model, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Khai báo allowedExtensions cho các định dạng tệp tin hợp lệ
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                // Xử lý ảnh chính
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(ImageFile.FileName).ToLower();
                    if (allowedExtensions.Contains(extension))
                    {
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + extension;
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

                // Thêm thông tin sản phẩm vào ProductStock cho từng kích thước được chọn
                if (model.SelectedSizeIDs != null && model.SelectedSizeIDs.Count > 0)
                {
                    foreach (var sizeId in model.SelectedSizeIDs)
                    {
                        var productStock = new ProductStock
                        {
                            ProductID = model.Product.ProductID,
                            ColorID = model.SelectedColorID,
                            SizeID = sizeId,
                            Quantity = model.Quantity
                        };
                        db.ProductStocks.Add(productStock);
                    }
                    db.SaveChanges();
                }


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
                                // Tạo tên file duy nhất cho mỗi ảnh phụ
                                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + "_sub" + extension;
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

            // Nếu ModelState không hợp lệ, tải lại danh sách Colors và Sizes
            model.Colors = db.Colors.ToList();
            model.Sizes = db.Sizes.ToList();
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(model);
        }

        public ActionResult EditProducts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy sản phẩm từ cơ sở dữ liệu
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách ảnh phụ từ ImageProducts
            var additionalImagesUrls = db.ImageProducts
                .Where(img => img.ProductsID == product.ProductID)
                .Select(img => img.ImageURL)
                .ToList();

            // Tạo ViewModel và truyền dữ liệu vào ViewModel
            var model = new ProductViewModel
            {
                Product = product,
                SelectedColorID = db.ProductStocks.FirstOrDefault(ps => ps.ProductID == product.ProductID)?.ColorID ?? 0,
                Colors = db.Colors.ToList(),
                AdditionalImagesUrls = additionalImagesUrls // Gán danh sách URL ảnh phụ
            };

            // Truyền danh sách Categories cho ViewBag để hiển thị trong dropdown
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProducts(ProductViewModel model, HttpPostedFileBase ImageFile, IEnumerable<HttpPostedFileBase> AdditionalImages)
        {
            if (ModelState.IsValid)
            {
                // Lấy sản phẩm từ cơ sở dữ liệu
                var product = db.Products.Find(model.Product.ProductID);
                if (product != null)
                {
                    // Cập nhật thông tin sản phẩm
                    product.ProductName = model.Product.ProductName;
                    product.Description = model.Product.Description;
                    product.Price = model.Product.Price;
                    product.CategoryID = model.Product.CategoryID;

                    // Cập nhật màu sắc nếu cần
                    var productStock = db.ProductStocks.FirstOrDefault(ps => ps.ProductID == model.Product.ProductID);
                    if (productStock != null)
                    {
                        productStock.ColorID = model.SelectedColorID;
                        db.Entry(productStock).State = EntityState.Modified;
                    }

                    // Xử lý ảnh chính nếu có file mới
                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(ImageFile.FileName).ToLower();
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                        string path = Path.Combine(Server.MapPath("~/Image/product-image/"), fileName);
                        ImageFile.SaveAs(path);
                        product.ImageURL = fileName; // Lưu tên file vào cơ sở dữ liệu
                    }

                    // Xử lý ảnh phụ nếu có file mới
                    if (AdditionalImages != null)
                    {
                        foreach (var file in AdditionalImages)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file.FileName).ToLower();
                                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid() + extension;
                                string path = Path.Combine(Server.MapPath("~/Image/product-image/"), fileName);
                                file.SaveAs(path);

                                // Lưu ảnh phụ vào bảng ImageProducts
                                db.ImageProducts.Add(new ImageProduct
                                {
                                    ProductsID = model.Product.ProductID,
                                    ImageURL = fileName
                                });
                            }
                        }
                    }

                    // Lưu thay đổi
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ManageProducts");
                }
            }

            // Nếu có lỗi, truyền lại dữ liệu cần thiết cho view
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName", model.Product.CategoryID);
            model.Colors = db.Colors.ToList();
            model.AdditionalImagesUrls = db.ImageProducts
                .Where(img => img.ProductsID == model.Product.ProductID)
                .Select(img => img.ImageURL)
                .ToList(); // Gán lại ảnh phụ khi có lỗi

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProducts(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }

                // Xóa ảnh chính của sản phẩm khỏi thư mục
                var mainImagePath = Path.Combine(Server.MapPath("~/image/product-image/"), product.ImageURL);
                if (System.IO.File.Exists(mainImagePath))
                {
                    System.IO.File.Delete(mainImagePath);
                }

                // Xóa các bản ghi liên quan trong ProductStocks
                var productStocks = db.ProductStocks.Where(ps => ps.ProductID == id).ToList();
                foreach (var stock in productStocks)
                {
                    db.ProductStocks.Remove(stock);
                }

                // Xóa các ảnh phụ trong ImageProducts
                var additionalImages = db.ImageProducts.Where(ip => ip.ProductsID == id).ToList();
                foreach (var image in additionalImages)
                {
                    var additionalImagePath = Path.Combine(Server.MapPath("~/image/product-image/"), image.ImageURL);
                    if (System.IO.File.Exists(additionalImagePath))
                    {
                        System.IO.File.Delete(additionalImagePath);
                    }
                    db.ImageProducts.Remove(image);
                }

                // Xóa sản phẩm
                db.Products.Remove(product);
                db.SaveChanges();

                // Lưu thông báo thành công vào TempData
                TempData["SuccessMessage"] = "Sản phẩm đã được xóa thành công.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa sản phẩm: " + ex.Message);

                TempData["ErrorMessage"] = "Không thể xóa sản phẩm vì có dữ liệu liên quan trong đơn hàng.";
            }

            return RedirectToAction("ManageProducts");
        }



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

            // Kiểm tra nếu là yêu cầu AJAX
            if (Request.IsAjaxRequest())
            {
                // Trả về dữ liệu dưới dạng JSON để hiển thị trong modal
                return Json(new { CategoryID = category.CategoryID, CategoryName = category.CategoryName }, JsonRequestBehavior.AllowGet);
            }

            return View(category); // Trả về view khi truy cập trực tiếp
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

        [HttpGet]
        public ActionResult AddPromotion()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddPromotion(string promotionName, string description, decimal? discountAmount, decimal? discountPercentage, DateTime startDate, DateTime endDate)
        {
            // Kiểm tra tính hợp lệ của dữ liệu
            if (string.IsNullOrWhiteSpace(promotionName) || startDate >= endDate)
            {
                ModelState.AddModelError("", "Invalid promotion data");
                return View();
            }

            // Tạo đối tượng Promotion mới
            var newPromotion = new Promotion
            {
                PromotionName = promotionName,
                Description = description,
                DiscountAmount = discountAmount,
                DiscountPercentage = discountPercentage,
                StartDate = startDate,
                EndDate = endDate
            };

            // Thêm chương trình khuyến mãi vào bảng Promotions
            db.Promotions.Add(newPromotion);
            db.SaveChanges();

            // Chuyển hướng về danh sách hoặc trang khác nếu cần
            return RedirectToAction("PromotionList"); // hoặc bất kỳ action nào bạn muốn chuyển tới
        }

        [HttpGet]
        public ActionResult PromotionList()
        {
            var promotions = db.Promotions.ToList();
            return View(promotions);
        }

        [HttpGet]
        public ActionResult EditPromotion(int id)
        {
            var promotion = db.Promotions
                .Where(p => p.PromotionID == id)
                .Select(p => new PromotionViewModel
                {
                    PromotionID = p.PromotionID,
                    PromotionName = p.PromotionName,
                    Description = p.Description,
                    DiscountAmount = p.DiscountAmount,
                    DiscountPercentage = p.DiscountPercentage,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate
                })
                .FirstOrDefault();

            if (promotion == null)
            {
                return HttpNotFound();
            }

            return View(promotion);

        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult EditPromotion(PromotionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var promotion = db.Promotions.Find(model.PromotionID);
                if (promotion == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật các trường
                promotion.PromotionName = model.PromotionName;
                promotion.Description = model.Description;
                promotion.DiscountAmount = model.DiscountAmount;
                promotion.DiscountPercentage = model.DiscountPercentage;
                promotion.StartDate = model.StartDate;
                promotion.EndDate = model.EndDate;

                db.SaveChanges();

                // Chuyển hướng về danh sách khuyến mãi sau khi lưu
                return RedirectToAction("PromotionList", "Admin");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddPromotion(string promotionName, string description, decimal? discountAmount, decimal? discountPercentage, DateTime startDate, DateTime endDate, int? productId = null)
        {
            // Kiểm tra tính hợp lệ của dữ liệu
            if (string.IsNullOrWhiteSpace(promotionName) || startDate >= endDate)
            {
                ModelState.AddModelError("", "Invalid promotion data");
                return View();
            }

            // Tạo đối tượng Promotion mới
            var newPromotion = new Promotion
            {
                PromotionName = promotionName,
                Description = description,
                DiscountAmount = discountAmount,
                DiscountPercentage = discountPercentage,
                StartDate = startDate,
                EndDate = endDate
            };

            // Thêm chương trình khuyến mãi vào bảng Promotions
            db.Promotions.Add(newPromotion);
            db.SaveChanges();

            // Nếu có productId hợp lệ, thêm vào bảng ProductPromotions để liên kết sản phẩm với chương trình khuyến mãi
            if (productId.HasValue)
            {
                var productPromotion = new ProductPromotion
                {
                    ProductID = productId.Value,
                    PromotionID = newPromotion.PromotionID
                };
                db.ProductPromotions.Add(productPromotion);
                db.SaveChanges();
            }

            // Chuyển hướng về danh sách chương trình khuyến mãi hoặc trang khác nếu cần
            return RedirectToAction("PromotionList");
        }

        public decimal GetCurrentMonthBalance()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1);

            var currentMonthBalance = db.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate)
                .SelectMany(o => o.OrderDetails)
                .Sum(od => (decimal?)od.Quantity * od.UnitPrice) ?? 0;

            return currentMonthBalance;
        }

        public decimal GetCurrentYearBalance()
        {
            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            var startOfNextYear = startOfYear.AddYears(1);

            var currentYearBalance = db.Orders
                .Where(o => o.OrderDate >= startOfYear && o.OrderDate < startOfNextYear)
                .SelectMany(o => o.OrderDetails)
                .Sum(od => (decimal?)od.Quantity * od.UnitPrice) ?? 0;

            return currentYearBalance;
        }
        public int GetCurrentMonthOrderCount()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1);

            // Đếm số lượng đơn hàng trong khoảng thời gian từ ngày đầu tiên đến cuối tháng hiện tại
            int orderCount = db.Orders?
                .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate)
                .Count() ?? 0;

            return orderCount;
        }
        public int GetTotalUserCount()
        {
            // Đếm tổng số người dùng trong bảng Users
            int userCount = db.Users.Count();
            return userCount;
        }
        public ActionResult Orders()
        {
            return View();
        }
        // GET: Inventory
        public ActionResult ManageInventory()
        {
            // Lấy thông tin sản phẩm và tồn kho
            var inventoryData = db.ProductStocks
                .Select(ps => new Models.InventoryAddStockViewModel // Đã sửa lỗi chính tả
                {
                    ProductStockID = ps.ProductStockID,
                    ProductID = (int)ps.ProductID,
                    ProductName = ps.Product.ProductName,
                    MainImageUrl = ps.Product.ImageURL,
                    Size = ps.Size.SizeValue,
                    Color = ps.Color.ColorName,
                    StockQuantity = (int)ps.Quantity
                })
        .ToList();

            return View(inventoryData);
        }
        public ActionResult AddStock(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm sản phẩm theo ProductStockID
            var productStock = db.ProductStocks.FirstOrDefault(ps => ps.ProductStockID == id);

            if (productStock == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm trong kho." });
            }

            var model = new InventoryAddStockViewModel
            {
                ProductStockID = productStock.ProductStockID,
                ProductID = (int)productStock.ProductID,
                ProductName = productStock.Product.ProductName,
                Size = productStock.Size.SizeValue, // Nếu cần hiển thị size
                CurrentStockQuantity = (int)productStock.Quantity
            };

            return PartialView("_AddStock", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStock(InventoryAddStockViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productStock = db.ProductStocks.FirstOrDefault(ps => ps.ProductStockID == model.ProductStockID);

                // Kiểm tra xem productStock có tồn tại hay không
                if (productStock != null)
                {
                    if (model.QuantityAdded <= 0)
                    {
                        return Json(new { success = false, message = "Số lượng nhập kho phải lớn hơn 0." });
                    }

                    // Cập nhật số lượng kho
                    productStock.Quantity += model.QuantityAdded;
                    db.Entry(productStock).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm trong kho." });
                }
            }

            return Json(new { success = false, message = "Không thể cập nhật số lượng kho." });
        }

    }
}
