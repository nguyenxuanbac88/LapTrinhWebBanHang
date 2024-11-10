using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWebBanHang.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; }  // Danh sách các sản phẩm trong giỏ hàng

        public Cart()
        {
            Items = new List<CartItem>();  // Khởi tạo danh sách giỏ hàng trống
        }

        // Thêm một sản phẩm vào giỏ hàng
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductID == item.ProductID);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;  // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng
            }
            else
            {
                Items.Add(item);  // Nếu chưa có trong giỏ hàng, thêm mới
            }
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        public void UpdateItem(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductID == productId);
            if (item != null)
            {
                item.Quantity = quantity;  // Cập nhật số lượng của sản phẩm
            }
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductID == productId);
            if (item != null)
            {
                Items.Remove(item);  // Xóa sản phẩm khỏi giỏ hàng
            }
        }

        // Tính tổng số lượng sản phẩm trong giỏ hàng
        public int GetTotalQuantity()
        {
            return Items.Sum(i => i.Quantity);
        }

        // Tính tổng tiền của giỏ hàng
        public decimal Total
        {
            get { return Items.Sum(i => i.TotalPrice); }
        }
    }
}

