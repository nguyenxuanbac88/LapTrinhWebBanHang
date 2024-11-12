public class OrderDetailViewModel
{
    public int? ProductID { get; set; }
    public string ProductName { get; set; } // Thêm tên sản phẩm
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Size { get; set; }
}