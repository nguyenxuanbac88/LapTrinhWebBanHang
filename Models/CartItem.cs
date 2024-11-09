using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWebBanHang.Models
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } // Thêm tên sản phẩm
        public decimal Price { get; set; } // Thêm giá sản phẩm
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } // Tuỳ chọn: Thêm URL hình ảnh
    }
}