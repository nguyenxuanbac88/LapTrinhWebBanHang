using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWebBanHang.Models
{
    public class CartItem
    {
        public int ProductID { get; set; }        // ID của sản phẩm
        public string ProductName { get; set; }   // Tên sản phẩm
        public decimal Price { get; set; }        // Giá sản phẩm
        public int Quantity { get; set; }         // Số lượng sản phẩm trong giỏ hàng
    }
}