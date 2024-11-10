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
    public ActionResult AddToCart(int productId, int quantity)
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
                cart.AddItem(new CartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageURL
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
    public ActionResult UpdateCartItem(int productId, int quantity)
    {
        var cart = GetCartFromSession();
        cart.UpdateItem(productId, quantity);
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
        // Kiểm tra IdUser trong session
        int? userId = Session["IdUser"] as int?;
        if (!userId.HasValue)
        {
            // Nếu không có IdUser, chuyển hướng đến trang đăng nhập
            return RedirectToAction("Sign_in", "Account");
        }

        using (WebsiteEntities4 db = new WebsiteEntities4())
        {
            // Lấy thông tin địa chỉ của người dùng từ AddressUser
            var userAddress = db.AddressUsers.FirstOrDefault(a => a.IdUser == userId.Value);
            if (userAddress == null)
            {
                // Nếu không tìm thấy địa chỉ người dùng, yêu cầu người dùng cập nhật
                return RedirectToAction("Index", "UserAddresss");
            }

            // Lấy thông tin giỏ hàng từ session
            var cart = GetCartFromSession();
            var cartItems = cart.Items.ToList();

            // Tính tổng số tiền giỏ hàng
            decimal totalAmount = cartItems.Sum(item => item.Quantity * item.Price);

            // Chuyển thông tin cho View
            var checkoutViewModel = new CheckoutViewModel
            {
                Address = userAddress,
                CartItems = cartItems,
                TotalAmount = totalAmount
            };

            return View(checkoutViewModel);
        }
    }

    [HttpPost]
    public ActionResult Checkout(CheckoutViewModel model)
    {
        int? userId = Session["IdUser"] as int?;
        if (!userId.HasValue)
        {
            return RedirectToAction("Sign_in", "Account");
        }

        using (WebsiteEntities4 db = new WebsiteEntities4())
        {
            // Cập nhật lại địa chỉ người dùng trong AddressUser nếu có thay đổi
            var userAddress = db.AddressUsers.FirstOrDefault(a => a.IdUser == userId.Value);
            if (userAddress == null)
            {
                return RedirectToAction("Index", "UserAddresss");
            }

            userAddress.FullName = model.Address.FullName;
            userAddress.Phone = model.Address.Phone;
            userAddress.Province = model.Address.Province;
            userAddress.Town = model.Address.Town;
            userAddress.Block = model.Address.Block;
            userAddress.SpecificAddress = model.Address.SpecificAddress;

            db.SaveChanges();

            // Tạo đơn hàng mới trong bảng Orders
            var newOrder = new Order
            {
                UserID = userId.Value,
                AddressID = userAddress.IdAddress,
                OrderDate = DateTime.UtcNow,
                Status = 1 // Trạng thái đơn hàng (ví dụ: đang xử lý)
            };

            db.Orders.Add(newOrder);
            db.SaveChanges();

            // Tạo chi tiết đơn hàng trong bảng OrderDetails
            var cart = GetCartFromSession();
            foreach (var item in cart.Items)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = newOrder.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                };

                db.OrderDetails.Add(orderDetail);
            }

            db.SaveChanges();

            // Xóa giỏ hàng sau khi thanh toán thành công
            ClearCart(userId.Value);

            return RedirectToAction("OrderConfirmation", "Carts", new { orderId = newOrder.OrderID });
        }
    }
    // Xóa giỏ hàng
    private void ClearCart(int userId)
    {
        Session["Cart"] = null;
        Session["CartQuantity"] = 0;
    }



    // Xác nhận đơn hàng
    public ActionResult OrderConfirmation()
    {
        return View();
    }
}
