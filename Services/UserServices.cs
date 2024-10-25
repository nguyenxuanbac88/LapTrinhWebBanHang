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
            WebsiteEntities4 db = new WebsiteEntities4();
            var isAdmin = db.Users
                .Where(u => u.Email.ToLower() == Email.ToLower())
                .Select(u => u.IsAdmin)
                .FirstOrDefault();
            if (isAdmin == 1)
            {
                return true;
            }
            return false;
        }
    }
}