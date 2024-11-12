using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using LapTrinhWebBanHang.Models;
using Newtonsoft.Json;

namespace LapTrinhWebBanHang
{
    public class WebhookController : Controller
    {
        private const string AccessToken = "0c9d6d847f794e94d5eb8fe3c2fb929d979f0a44";
        private readonly WebsiteEntities4 db = new WebsiteEntities4();

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PaymentNotification()
        {
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
                            string type = transaction.Type;
                            string description = transaction.Description;
                            string amount = transaction.Amount;
                            string transactionID = transaction.TransactionID;
                            int paymentMethod = 1; // Giả định phương thức thanh toán là 1, có thể thay đổi tùy vào yêu cầu
                            if (type == "IN")
                            {
                                // Sử dụng Regex để tìm "musports" và lấy ID sau đó
                                var match = Regex.Match(description, @"musports\s*(\d+)");
                                if (match.Success && int.TryParse(match.Groups[1].Value, out int idOrder) && decimal.TryParse(amount, out decimal amountBank))
                                {
                                    var order = db.Orders.FirstOrDefault(o => o.OrderID == idOrder);

                                    if (order != null)
                                    {
                                        if (amountBank == order.price)
                                        {
                                            // Cập nhật trạng thái đơn hàng
                                            order.Status = 1;

                                            // Ghi log vào bảng Payments
                                            var payment = new Payment
                                            {
                                                OrderID = idOrder,
                                                PaymentAmount = amountBank,
                                                PaymentDate = DateTime.Now,
                                                PaymentMethod = paymentMethod,
                                                PaymentStatus = 1, // 1 biểu thị thanh toán thành công
                                                TransactionID = transactionID
                                            };

                                            db.Payments.Add(payment);

                                            try
                                            {
                                                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                                            }
                                            catch (Exception ex)
                                            {
                                                // Ghi log lỗi nếu xảy ra lỗi khi lưu dữ liệu
                                                Console.WriteLine("Error saving to database: " + ex.Message);
                                                return Json(new { status = false, msg = "Error saving to database: " + ex.Message });
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Skipped transaction with Type: " + type);
                                }
                            }

                        }
                    }


                    var response = new
                    {
                        status = true,
                        msg = "Ok"
                    };

                    return Json(response);
                }
                else
                {
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
