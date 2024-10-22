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
        public ActionResult Sign_in()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sign_in(string user, string password)
        {
            WebsiteEntities4 db= new WebsiteEntities4();
            string md5password = Md5.GetMd5Hash(password);
            // Tìm người dùng trong cơ sở dữ liệu
            var userInDb = db.Users.FirstOrDefault(u => u.Email.ToLower() == user.ToLower() && u.PasswordHash == md5password);
            if (user.Length >=10 && user.Length < 35) 
            {
                if (password.Length>=6 && password.Length<40)
                {
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
            }
            else
            {
                return RedirectToAction("Sign_in");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("Sign_in");
        }
    }
}