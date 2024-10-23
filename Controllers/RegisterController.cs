using LapTrinhWebBanHang.Models;
using LapTrinhWebBanHang.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string user,string password,string confirmpassword)
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
                        PasswordHash = password,
                    };
                    db.Users.Add(_user);
                    db.SaveChanges();
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