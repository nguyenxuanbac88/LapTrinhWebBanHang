using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LapTrinhWebBanHang.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AddressUser> AddressUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ImageProduct> ImageProducts { get; set; }
        public DbSet<InventoryLog> InventoryLogs { get; set; }
    }
}