using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWebBanHang.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();  // Danh sách các sản phẩm trong giỏ hàng
        
        // Tính tổng giá trị giỏ hàng
        public decimal Total
        {
            get { return Items.Sum(item => item.Price * item.Quantity); }  // Tổng giá trị giỏ hàng
        }

        // Thêm một sản phẩm vào giỏ hàng
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductID == item.ProductID);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;  // Nếu sản phẩm đã có trong giỏ, tăng số lượng
            }
            else
            {
                Items.Add(item);  // Nếu chưa có, thêm sản phẩm mới vào giỏ
            }
        }

        // Xóa một sản phẩm khỏi giỏ hàng
        public void RemoveItem(int productID)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.ProductID == productID);
            if (itemToRemove != null)
            {
                Items.Remove(itemToRemove);  // Xóa sản phẩm khỏi giỏ hàng
            }
        }
    }
}