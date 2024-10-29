using LapTrinhWebBanHang.Models;
using LapTrinhWebBanHang.Services;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LapTrinhWebBanHang.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult Home_page()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetNoStore();

            return View();
        }
       

    }
}