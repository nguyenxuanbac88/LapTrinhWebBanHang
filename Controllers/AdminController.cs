using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LapTrinhWebBanHang.Models;

namespace LapTrinhWebBanHang.Controllers
{
    public class AdminController : Controller
    {
        //Thiếu check login, vì đang test nên chưa có thêm vô
        private WebsiteEntities4 db = new WebsiteEntities4(); // Sử dụng DbContext đã được tạo từ Entity Framework

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/ManageProducts
        public ActionResult ManageProducts()
        {
            var products = db.Products.ToList();  // Lấy danh sách tất cả sản phẩm
            return View(products); // Trả về view với danh sách sản phẩm
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
                    // Tạo tên file duy nhất cho ảnh
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName =DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

                    // Đường dẫn lưu ảnh
                    string path = Path.Combine(Server.MapPath("~/image/product-image/"), fileName);

                    // Lưu ảnh vào thư mục đã chỉ định
                    ImageFile.SaveAs(path);

                    // Lưu tên file vào thuộc tính ImageURL của sản phẩm
                    product.ImageURL =fileName;
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

    }
}
