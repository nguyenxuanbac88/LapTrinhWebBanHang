using LapTrinhWebBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LapTrinhWebBanHang.Services;
using System.Web.Mvc;
using System.Web.Security;

namespace LapTrinhWebBanHang.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        
        public ActionResult Sign_in()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sign_in(string user, string password)
        {
            WebsiteEntities4 db = new WebsiteEntities4();
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
                            return RedirectToAction("Home_page", "HomePage");
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
        // GET: Register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string user, string password, string confirmpassword)
        {
            WebsiteEntities4 db = new WebsiteEntities4();
            var userInDb = db.Users.FirstOrDefault(u => u.Email.ToLower() == user.ToLower());
            if (userInDb == null)
            {
                if (password == confirmpassword)
                {
                    string md5password = Md5.GetMd5Hash(password);
                    var _user = new User
                    {
                        Email = user,
                        PasswordHash = md5password,
                    };
                    db.Users.Add(_user);
                    db.SaveChanges();
                    ModelState.AddModelError("", "đăng kí thành công");
                }
                else
                {
                    ModelState.AddModelError("", "mật khẩu confirm không đúng với mật khẩu đã nhập");
                }

            }
            else
            {
                ModelState.AddModelError("", "tài khoản đã được sử dụng vui lòng nhập tài khoản khác");
            }
            return View();
        }
        
    }
}