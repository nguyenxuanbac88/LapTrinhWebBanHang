using LapTrinhWebBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class CartsController : Controller
    {
        // GET: Carts
        public ActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["Email"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("Sign_in", "Account");
            }

            // Nếu đã đăng nhập, lấy giỏ hàng của người dùng
            var cart = GetCartFromSession(); // Hàm này lấy giỏ hàng từ session hoặc từ cơ sở dữ liệu

            return View(cart); // Trả về giỏ hàng cho người dùng
        }

        // Thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int productId, int quantity)
        {
            if (Session["Email"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("Sign_in", "Account");
            }

            // Thêm sản phẩm vào giỏ hàng
            var cart = GetCartFromSession();
            cart.AddItem(new CartItem
            {
                ProductID = productId,
                Quantity = quantity
            });

            // Cập nhật giỏ hàng vào session
            SaveCartToSession(cart);

            return RedirectToAction("Index"); // Quay lại giỏ hàng
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int productId)
        {
            if (Session["Email"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("Sign_in", "Account");
            }

            var cart = GetCartFromSession();
            cart.RemoveItem(productId);

            // Cập nhật lại giỏ hàng trong session
            SaveCartToSession(cart);

            return RedirectToAction("Index"); // Quay lại giỏ hàng
        }

        // GET: Checkout (Thanh toán)
        public ActionResult Checkout()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["Email"] == null)
            {
                return RedirectToAction("Sign_in", "Account");
            }

            // Lấy giỏ hàng từ session
            var cart = GetCartFromSession();

            // Kiểm tra giỏ hàng có rỗng không
            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index"); // Nếu giỏ hàng trống, quay lại giỏ hàng
            }

            return View(cart); // Trả về giỏ hàng cho người dùng để xác nhận thanh toán
        }

        // POST: Thanh toán
        [HttpPost]
        public ActionResult Checkout(FormCollection form)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["Email"] == null)
            {
                return RedirectToAction("Sign_in", "Account");
            }

            // Lấy giỏ hàng từ session
            var cart = GetCartFromSession();

            // Kiểm tra giỏ hàng có rỗng không
            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index"); // Nếu giỏ hàng trống, quay lại giỏ hàng
            }

            // Giả sử bạn thực hiện thanh toán ở đây (thực tế sẽ có thể tích hợp với một hệ thống thanh toán bên ngoài)
            // Sau khi thanh toán thành công, bạn sẽ làm sạch giỏ hàng

            // Xử lý thanh toán (ở đây bạn có thể thêm mã xử lý thanh toán thực tế)
            bool paymentSuccess = ProcessPayment(cart); // Giả sử bạn có một phương thức thanh toán

            if (paymentSuccess)
            {
                // Nếu thanh toán thành công, xóa giỏ hàng khỏi session
                ClearCart();

                // Chuyển hướng đến trang xác nhận thanh toán thành công
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                // Nếu thanh toán thất bại, quay lại trang giỏ hàng
                ModelState.AddModelError("", "Thanh toán không thành công. Vui lòng thử lại.");
                return View(cart);
            }
        }

        // Thao tác xử lý thanh toán
        private bool ProcessPayment(Cart cart)
        {
            // Logic thanh toán giả sử thành công, bạn có thể tích hợp với hệ thống thanh toán thực tế ở đây
            return true;
        }

        // Lấy giỏ hàng từ session
        private Cart GetCartFromSession()
        {
            if (Session["Cart"] == null)
            {
                // Nếu chưa có giỏ hàng trong session, tạo mới
                Session["Cart"] = new Cart();
            }

            return (Cart)Session["Cart"];
        }

        // Lưu giỏ hàng vào session
        private void SaveCartToSession(Cart cart)
        {
            Session["Cart"] = cart;
        }

        // Xóa giỏ hàng trong session sau khi thanh toán
        private void ClearCart()
        {
            Session["Cart"] = null;
        }

        // GET: Thanh toán thành công
        public ActionResult PaymentSuccess()
        {
            return View(); // Hiển thị thông báo thanh toán thành công
        }
    }

}
