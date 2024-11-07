using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LapTrinhWebBanHang
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Cấu hình route cho webhook
            routes.MapRoute(
                    name: "PaymentWebhook",
                    url: "api/webhook/payment",
                    defaults: new { controller = "Webhook", action = "PaymentNotification" }
                );


            // Route cho trang chủ
            routes.MapRoute(
                name: "HomePage",
                url: "",
                defaults: new { controller = "HomePage", action = "Home_page" }
            );

            // Route cho xác thực mã
            routes.MapRoute(
                name: "Verify",
                url: "verify/{code}",
                defaults: new { controller = "Account", action = "Verify" }
            );

            // Route mặc định
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }


    }
}
