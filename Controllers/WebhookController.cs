using System.IO;
using System.Web.Mvc;
using LapTrinhWebBanHang.Models;
using Newtonsoft.Json;

namespace LapTrinhWebBanHang
{
    public class WebhookController : Controller
    {
        private const string AccessToken = "917A6151-E32C-6308-4DE5-DE503713CBCC";

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PaymentNotification()
        {
            // Lấy Bearer Token từ Header
            var authorizationHeader = Request.Headers["Authorization"];
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var bearerToken = authorizationHeader.Substring(7); 
                if (bearerToken == AccessToken)
                {
                    string receivedData;
                    using (var reader = new StreamReader(Request.InputStream))
                    {
                        receivedData = reader.ReadToEnd();
                    }
                    var webhookData = JsonConvert.DeserializeObject<PaymentWebhookModel>(receivedData);

                    if (webhookData != null && webhookData.Status && webhookData.Data != null)
                    {
                        foreach (var transaction in webhookData.Data)
                        {
                            string id = transaction.Id;
                            string type = transaction.Type;
                            string transactionID = transaction.TransactionID;
                            string amount = transaction.Amount;
                            string description = transaction.Description;
                            string date = transaction.Date;
                            string bank = transaction.Bank;

                        }
                    }

                    var response = new
                    {
                        status = true,
                        msg = "OK"
                    };

                    return Json(response);
                }
                else
                {
                    // Chữ ký không khớp, từ chối yêu cầu
                    Response.StatusCode = 401; // Unauthorized
                    return Content("Chữ ký không hợp lệ.");
                }
            }
            else
            {
                Response.StatusCode = 401; 
                return Content("Access Token không được cung cấp hoặc không hợp lệ.");
            }
        }
    }
}
