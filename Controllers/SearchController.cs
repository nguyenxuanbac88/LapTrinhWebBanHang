﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LapTrinhWebBanHang.Models;

namespace LapTrinhWebBanHang.Controllers
{
    public class SearchController : Controller
    {
        private WebsiteEntities4 db = new WebsiteEntities4(); // DbContext của bạn

        // Trang tìm kiếm
        public ActionResult Index()
        {
            return View();
        }

        // Tìm kiếm sản phẩm và trả về view với kết quả
        [HttpGet]
        public ActionResult SearchProducts(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return View(new List<Product>());
            }

            var products = db.Products
                .Where(p => p.ProductName.Contains(keyword) || p.Description.Contains(keyword))
                .ToList();

            return View(products);
        }

        // Tìm kiếm sản phẩm và trả về kết quả dưới dạng JSON
        [HttpGet]
        public JsonResult SearchProductsJson(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return Json(new { success = true, data = new List<Product>() }, JsonRequestBehavior.AllowGet);
            }

            var products = db.Products
                .Where(p => p.ProductName.Contains(keyword) || p.Description.Contains(keyword))
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Price,
                    p.ImageURL,
                    p.Description,
                    CategoryName = db.Categories
                        .Where(c => c.CategoryID == p.CategoryID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault()
                })
                .ToList();

            return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
        }
    }
}
