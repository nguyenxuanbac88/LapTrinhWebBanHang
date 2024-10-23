using LapTrinhWebBanHang.Models;
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
                return View();
            }
            return View(userInDb);
        }
    }
}