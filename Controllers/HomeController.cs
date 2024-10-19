using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ConnectDB db = new ConnectDB();

            // Kiểm tra kết nối
            bool isConnected = db.TestConnection();

            if (isConnected)
            {
                ViewBag.Message = "Kết nối thành công!";
            }
            else
            {
                ViewBag.Message = "Kết nối thất bại!";
            }

            return View();
        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult test()
        {
            ConnectDB db = new ConnectDB();

            // Kiểm tra kết nối
            bool isConnected = db.TestConnection();

            if (isConnected)
            {
                ViewBag.Message = "Kết nối thành công từ ConnectDB";
                //Lấy bảng đang có trong DB
                List<string> tableNames = db.GetTableNames();
                ViewBag.TableNames = tableNames;  
            }
            else
            {
                ViewBag.Message = "Kết nối thất bại từ ConnectDB!";
                ViewBag.TableNames = null;
            }

            return View();
        }

    }
}