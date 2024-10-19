using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class SigninController : Controller
    {
        // GET: Signin
        public ActionResult Index()
        {
            return View();
        }
        //HTTP get /Home/DangKy
        public ActionResult DangKy()
        {
            return View();
        }
        //HTTP post /Home/DangKy
      
    }
}