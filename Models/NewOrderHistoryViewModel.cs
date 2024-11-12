﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWebBanHang.Models
{
    public class NewOrderHistoryViewModel
    {
        public int UserID { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int? Status { get; set; }
        public decimal Price { get; set; }
        public string SpecificAddress { get; set; }
        public string Block { get; set; }
        public string Town { get; set; }
        public string Province { get; set; }
        public string Phone { get; set; }
        public List<NewOrderDetailViewModel> OrderDetails { get; set; }

        // Constructor để tránh OrderDetails bị null
        public NewOrderHistoryViewModel()
        {
            OrderDetails = new List<NewOrderDetailViewModel>();
        }
    }

}