using LapTrinhWebBanHang.Models;
using System;
using System.Collections.Generic;
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
            return View();
        }
        public ActionResult Sign_in()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sign_in(string user, string password)
        {
            WebsiteEntities4 db= new WebsiteEntities4();

            // Tìm người dùng trong cơ sở dữ liệu
            var userInDb = db.Users.FirstOrDefault(u => u.Email.ToLower() == user.ToLower() && u.PasswordHash == password);
            if (userInDb != null)
            {
                Session["user"] = userInDb;
                return RedirectToAction("Home_page");
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("Sign_in");
        }
    }
}