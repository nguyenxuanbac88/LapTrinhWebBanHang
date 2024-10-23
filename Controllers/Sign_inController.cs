using LapTrinhWebBanHang.Models;
using LapTrinhWebBanHang.Services;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

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

        [HttpPost]
        public ActionResult Sign_in(string user, string password)
        {
            WebsiteEntities4 db= new WebsiteEntities4();
            {
                string md5password = Md5.GetMd5Hash(password);
                // Tìm người dùng trong cơ sở dữ liệu
                var userInDb = db.Users.FirstOrDefault(u => u.Email.ToLower() == user.ToLower() && u.PasswordHash == md5password);
                if (user.Length >= 10 && user.Length < 35)
                {
                    if (password.Length >= 6 && password.Length < 40)
                    {
                        if (userInDb != null)
                        {
                           
                            Session["Email"] = userInDb;
                            return RedirectToAction("Home_page","HomePage");
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
        }

        public ActionResult Logout()
        {
            Session.Remove("Email");
            FormsAuthentication.SignOut();
            return RedirectToAction("Sign_in");
        }
    }
}