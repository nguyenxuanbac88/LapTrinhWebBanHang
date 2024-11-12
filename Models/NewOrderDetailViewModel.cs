﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWebBanHang.Models
{
    public class NewOrderDetailViewModel
    {
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }

}