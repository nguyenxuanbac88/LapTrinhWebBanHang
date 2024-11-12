using LapTrinhWebBanHang.Models;
using System.Collections.Generic;
using System;

public class OrderHistoryViewModel
{
    public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public int? Status { get; set; }
    public decimal Price { get; set; }
    public string SpecificAddress { get; set; }
    public string Block { get; set; }
    public string Town { get; set; }
    public string Province { get; set; }
    public string Phone { get; set; }
    public List<OrderDetailViewModel> OrderDetails { get; set; }
}