using LapTrinhWebBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class UserAddresssController : Controller
    {
        private WebsiteEntities4 db = new WebsiteEntities4();

        // Hiển thị thông tin người dùng và địa chỉ
        public ActionResult Index()
        {
            // Lấy thông tin người dùng hiện tại từ session hoặc cookie
            string email = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return HttpNotFound();
            }

            var address = db.AddressUsers.FirstOrDefault(a => a.IdUser == user.IdUser);
            if (address == null)
            {
                return HttpNotFound();
            }

            // Trả về view với thông tin người dùng và địa chỉ
            var viewModel = new AddressUserViewModel
            {
                FullName = address.FullName,
                Phone = address.Phone,
                Province = address.Province,
                Town = address.Town,
                Block = address.Block,
                SpecificAddress = address.SpecificAddress
            };
            return View(viewModel);
        }

        // Cập nhật thông tin địa chỉ người dùng
        [HttpPost]
        public ActionResult Update(AddressUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng hiện tại từ session hoặc cookie
                string email = User.Identity.Name;
                var user = db.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var address = db.AddressUsers.FirstOrDefault(a => a.IdUser == user.IdUser);
                if (address == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin địa chỉ
                address.FullName = model.FullName;
                address.Phone = model.Phone;
                address.Province = model.Province;
                address.Town = model.Town;
                address.Block = model.Block;
                address.SpecificAddress = model.SpecificAddress;

                db.SaveChanges();
                ModelState.AddModelError("", "Cập nhật địa chỉ thành công");
            }

            return View("Index", model);
        }
    }
}