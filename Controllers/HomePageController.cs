using LapTrinhWebBanHang.Models;
using LapTrinhWebBanHang.Services;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace LapTrinhWebBanHang.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult Home_page()
        {
            return View();
        }
       

    }
}