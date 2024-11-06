using System.IO;
using System.Web.Mvc;
using LapTrinhWebBanHang.Models;
using Newtonsoft.Json;

namespace LapTrinhWebBanHang
{
    public class WebhookController : Controller
    {
        private const string AccessToken = "YOUR_ACCESS_TOKEN"; // Thay YOUR_ACCESS_TOKEN bằng Access Token thực tế

        [HttpPost]
        public ActionResult PaymentNotification()
        {
            // Lấy Bearer Token từ Header
            var authorizationHeader = Request.Headers["Authorization"];
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var bearerToken = authorizationHeader.Substring(7); // Lấy chuỗi token sau "Bearer "

                // Kiểm tra token
                if (bearerToken == AccessToken)
                {
                    // Đọc dữ liệu JSON từ yêu cầu
                    string receivedData;
                    using (var reader = new StreamReader(Request.InputStream))
                    {
                        receivedData = reader.ReadToEnd();
                    }

                    // Chuyển đổi JSON thành đối tượng PaymentWebhookModel
                    var webhookData = JsonConvert.DeserializeObject<PaymentWebhookModel>(receivedData);

                    // Kiểm tra nếu status là true và có dữ liệu giao dịch
                    if (webhookData != null && webhookData.Status && webhookData.Data != null)
                    {
                        foreach (var transaction in webhookData.Data)
                        {
                            // Truy xuất từng thuộc tính trong transaction
                            string id = transaction.Id;
                            string type = transaction.Type;
                            string transactionID = transaction.TransactionID;
                            string amount = transaction.Amount;
                            string description = transaction.Description;
                            string date = transaction.Date;
                            string bank = transaction.Bank;

                            // Thêm logic xử lý từng giao dịch tại đây (VD: lưu vào cơ sở dữ liệu)
                        }
                    }

                    // Tạo phản hồi JSON để xác nhận đã nhận dữ liệu thành công
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
                // Tiêu đề Authorization không tồn tại hoặc không hợp lệ
                Response.StatusCode = 401; // Unauthorized
                return Content("Access Token không được cung cấp hoặc không hợp lệ.");
            }
        }
    }
}
