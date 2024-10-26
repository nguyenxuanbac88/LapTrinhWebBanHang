using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error(int statusCode, string message)
        {
            ViewBag.StatusCode = statusCode;
            ViewBag.Message = message;
            return View("Error");
        }
    }
}