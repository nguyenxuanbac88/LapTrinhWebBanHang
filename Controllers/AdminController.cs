using System;
using System.Linq;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProducts(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);  // Thêm sản phẩm mới vào cơ sở dữ liệu
                db.SaveChanges();  // Lưu các thay đổi
                return RedirectToAction("ManageProducts");  // Chuyển hướng về trang quản lý sản phẩm
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
