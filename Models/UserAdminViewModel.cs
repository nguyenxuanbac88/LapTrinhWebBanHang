﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWebBanHang.Models
{
    public class UserAdminViewModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IPAddress { get; set; }
    }
}