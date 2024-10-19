using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace LapTrinhWebBanHang.Controllers
{
    public class ConnectDB
    {
        private string connectionString;

        public ConnectDB()
        {
            // Lấy chuỗi kết nối từ Web.config
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public DataTable ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                return dataTable;
            }
        }
        public bool TestConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Kết nối cơ sở dữ liệu  thành công!");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Không thể kết nối: " + ex.Message);
                    return false;
                }
            }
        }
        public List<string> GetTableNames()
        {
            List<string> tableNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Truy vấn danh sách bảng từ hệ thống thông tin của SQL Server
                    DataTable schema = connection.GetSchema("Tables");

                    foreach (DataRow row in schema.Rows)
                    {
                        tableNames.Add(row[2].ToString()); // Cột thứ 3 (index 2) là tên bảng
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching tables: " + ex.Message);
                }
            }

            return tableNames;
        }
    }
}