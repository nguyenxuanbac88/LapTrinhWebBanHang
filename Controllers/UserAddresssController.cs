using LapTrinhWebBanHang.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class UserAddresssController : Controller
    {
        // GET: UserAddresss/Index
        public ActionResult Index()
        {
            string email = Session["Email"] as string;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Sign_in", "Account");
            }

            using (WebsiteEntities4 db = new WebsiteEntities4())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var address = db.AddressUsers.FirstOrDefault(a => a.IdUser == user.IdUser);
                var viewModel = new AddressUserViewModel
                {
                    IdUser = user.IdUser,
                    FullName = address?.FullName ?? "",
                    Phone = address?.Phone ?? "",
                    Province = address?.Province ?? "",
                    Town = address?.Town ?? "",
                    Block = address?.Block ?? "",
                    SpecificAddress = address?.SpecificAddress ?? ""
                };
                ViewBag.UserEmail = email;
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Update(AddressUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = Session["Email"] as string;
                if (string.IsNullOrEmpty(email))
                {
                    return RedirectToAction("Sign_in", "Account");
                }

                using (WebsiteEntities4 db = new WebsiteEntities4())
                {
                    var user = db.Users.FirstOrDefault(u => u.Email == email);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }

                    var address = db.AddressUsers.FirstOrDefault(a => a.IdUser == user.IdUser);
                    if (address == null)
                    {
                        address = new AddressUser { IdUser = user.IdUser };
                        db.AddressUsers.Add(address);
                    }

                    address.FullName = model.FullName;
                    address.Phone = model.Phone;
                    address.Province = model.Province;
                    address.Town = model.Town;
                    address.Block = model.Block;
                    address.SpecificAddress = model.SpecificAddress;

                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công";
                }
            }

            return RedirectToAction("Index");
        }

    }
}
