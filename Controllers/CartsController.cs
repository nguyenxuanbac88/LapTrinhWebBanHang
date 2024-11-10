using LapTrinhWebBanHang.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class CartsController : Controller
{
    // Lấy giỏ hàng từ Session
    private Cart GetCartFromSession()
    {
        var cart = Session["Cart"] as Cart;
        if (cart == null)
        {
            cart = new Cart();  // Tạo giỏ hàng mới nếu không có
            Session["Cart"] = cart;
        }

        return cart;
    }

    // Thêm sản phẩm vào giỏ hàng
    public ActionResult AddToCart(int productId, int quantity, string size)
    {
        // Kiểm tra xem người dùng đã đăng nhập chưa
        if (Session["Email"] == null)
        {
            // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
            return RedirectToAction("Sign_in", "Account");
        }

        var cart = GetCartFromSession();
        using (var db = new WebsiteEntities4())
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                // Thêm sản phẩm vào giỏ với kích thước đã chọn
                cart.AddItem(new CartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageURL,
                    Size = size  // Lưu giá trị size vào CartItem
                });
            }
        }

        // Lưu tổng số lượng giỏ hàng vào Session
        Session["CartQuantity"] = cart.GetTotalQuantity();
        return RedirectToAction("Home_page", "HomePage"); // Trở lại trang sản phẩm
    }


    // Hiển thị giỏ hàng
    public ActionResult ViewCart()
    {
        var cart = GetCartFromSession();
        return View(cart);
    }


    // Cập nhật số lượng sản phẩm trong giỏ hàng
    [HttpPost]
    public ActionResult UpdateCartItem(int productId, string size, int quantity)
    {
        var cart = GetCartFromSession();
        cart.UpdateItem(productId, size, quantity);  // Cập nhật số lượng sản phẩm theo size
                                                     // Cập nhật số lượng giỏ hàng vào Session
        Session["CartQuantity"] = cart.GetTotalQuantity();
        return RedirectToAction("ViewCart");
    }


    // Xóa sản phẩm khỏi giỏ hàng
    [HttpPost]
    public ActionResult RemoveCartItem(int productId)
    {
        var cart = GetCartFromSession();
        cart.RemoveItem(productId);
        // Cập nhật số lượng giỏ hàng vào Session
        Session["CartQuantity"] = cart.GetTotalQuantity();
        return RedirectToAction("ViewCart");
    }

    // Thanh toán
    public ActionResult Checkout()
    {
        var cart = GetCartFromSession();
        if (cart.Items.Any())
        {
            // Xử lý thanh toán tại đây (có thể lưu thông tin vào database, tạo đơn hàng...)
            // Sau khi thanh toán, xóa giỏ hàng
            Session["Cart"] = null;
            return RedirectToAction("OrderConfirmation");
        }
        else
        {
            return RedirectToAction("ViewCart");
        }
    }

    // Xác nhận đơn hàng
    public ActionResult OrderConfirmation()
    {
        return View();
    }
}
