using LapTrinhWebBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LapTrinhWebBanHang.Services
{
    public class UserServices
    {
        public string GetUserIP()
        {
            string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ipAddress;
        }
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())  
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);

                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static bool CheckAdmin(string Email)
        {
            using (WebsiteEntities4 db = new WebsiteEntities4()) // Sử dụng 'using' để tự động giải phóng tài nguyên
            {
                var isAdmin = db.Users
                    .Where(u => u.Email.ToLower() == Email.ToLower())
                    .Select(u => u.IsAdmin)
                    .FirstOrDefault();
                // Kiểm tra nếu không tìm thấy người dùng hoặc IsAdmin khác 1
                if (isAdmin == 1)
                {
                    return true;
                }
                return false;
            }
        }
        public static string GenerateRandomCode(int length)
        {
            Random random = new Random();
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += random.Next(0, 10).ToString(); // Tạo số ngẫu nhiên từ 0 đến 9
            }
            return result; // Trả về chuỗi ngẫu nhiên
        }
    }
}