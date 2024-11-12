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
            if (userAddress == null || string.IsNullOrEmpty(userAddress.FullName) ||
            string.IsNullOrEmpty(userAddress.Phone) ||
            string.IsNullOrEmpty(userAddress.Province) ||
            string.IsNullOrEmpty(userAddress.Town) ||
            string.IsNullOrEmpty(userAddress.Block) ||
            string.IsNullOrEmpty(userAddress.SpecificAddress))
            {
                // Nếu địa chỉ không có hoặc còn thiếu thông tin, chuyển hướng người dùng đến trang cập nhật địa chỉ
                TempData["Message"] = "Vui lòng nhập đầy đủ thông tin địa chỉ trước khi thanh toán.";
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
            // Lấy thông tin giỏ hàng từ session
            var cart = GetCartFromSession();
            var cartItems = cart.Items.ToList();

            // Tính tổng số tiền giỏ hàng
            int totalAmount = cartItems.Sum(item => item.Quantity * item.Price);
            // Tạo đơn hàng mới trong bảng Orders
            var newOrder = new Order
            {
                UserID = userId.Value,
                SpecificAddress = userAddress.SpecificAddress,
                Block = userAddress.Block,
                Town = userAddress.Town,
                Province = userAddress.Province,
                phone = userAddress.Phone,
                OrderDate = DateTime.UtcNow,
                Status = 0, // Trạng thái đơn hàng (ví dụ: đang xử lý)
                price = totalAmount
            };

            db.Orders.Add(newOrder);
            db.SaveChanges();

            // Tạo chi tiết đơn hàng trong bảng OrderDetails

            foreach (var item in cart.Items)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = newOrder.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    size = item.Size,
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
    public ActionResult OrderHistory()
    {
        int? userId = Session["IdUser"] as int?;
        if (!userId.HasValue)
        {
            return RedirectToAction("Sign_in", "Account");
        }

        using (var db = new WebsiteEntities4())
        {
            var orders = db.Orders
                           .Where(o => o.UserID == userId.Value)
                           .Select(o => new OrderHistoryViewModel
                           {
                               OrderID = o.OrderID,
                               OrderDate = o.OrderDate,
                               Status = o.Status,
                               Price = o.price,
                               SpecificAddress = o.SpecificAddress,
                               Block = o.Block,
                               Town = o.Town,
                               Province = o.Province,
                               Phone = o.phone,
                               OrderDetails = db.OrderDetails
                                                .Where(od => od.OrderID == o.OrderID)
                                                .Select(od => new OrderDetailViewModel
                                                {
                                                    ProductID = od.ProductID,
                                                    ProductName = db.Products.FirstOrDefault(p => p.ProductID == od.ProductID).ProductName,
                                                    Quantity = od.Quantity,
                                                    UnitPrice = od.UnitPrice,
                                                    Size = od.size
                                                }).ToList()
                           }).ToList();

            return View(orders);
        }
    }
    public ActionResult OrderDetail(int orderId)
    {
        using (var db = new WebsiteEntities4())
        {
            // Tìm đơn hàng theo orderId
            var order = db.Orders
                .Where(o => o.OrderID == orderId)
                .Select(o => new OrderHistoryViewModel
                {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    Price = o.price,
                    SpecificAddress = o.SpecificAddress,
                    Block = o.Block,
                    Town = o.Town,
                    Province = o.Province,
                    Phone = o.phone,
                    // Lấy danh sách OrderDetails và thêm thông tin sản phẩm từ bảng Product
                    OrderDetails = o.OrderDetails
                        .Select(od => new OrderDetailViewModel
                        {
                            ProductID = od.ProductID,
                            Quantity = od.Quantity,
                            UnitPrice = od.UnitPrice,
                            Size = od.size,
                            ProductName = db.Products
                                            .Where(p => p.ProductID == od.ProductID)
                                            .Select(p => p.ProductName)
                                            .FirstOrDefault()  // Lấy tên sản phẩm
                        }).ToList()
                }).FirstOrDefault();

            // Kiểm tra nếu không tìm thấy đơn hàng
            if (order == null)
            {
                return HttpNotFound();  // Trả về lỗi 404 nếu không tìm thấy đơn hàng
            }

            return View(order);
        }
    }



}
