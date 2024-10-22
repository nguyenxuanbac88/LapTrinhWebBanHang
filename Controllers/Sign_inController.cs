using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class Sign_inController : Controller
    {
        // GET: Sign_in
        public ActionResult Index()
        {
            return View();
        }
        //http /home/get/đăng kí
        public ActionResult Sign_in()
        {
            return View();
        }
        //http /home/post/đăng kí
       
    }
}