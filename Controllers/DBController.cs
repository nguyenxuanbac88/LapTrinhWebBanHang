using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebBanHang.Controllers
{
    public class DBController : Controller
    {
        // GET: DB
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Test()
        {
            ConnectDB db = new ConnectDB();

            // Kiểm tra kết nối
            bool isConnected = db.TestConnection();

            if (isConnected)
            {
                ViewBag.Message = "Kết nối thành công!";


                List<string> tableNames = db.GetTableNames();
                ViewBag.TableNames = tableNames;


                Dictionary<string, List<string>> columns = new Dictionary<string, List<string>>();

                foreach (var tableName in tableNames)
                {
                    var columnList = db.GetColumnNames(tableName);
                    if (columnList != null && columnList.Count > 0)
                    {
                        columns[tableName] = columnList;
                    }
                }

                ViewBag.Columns = columns;
            }
            else
            {
                ViewBag.Message = "Kết nối thất bại!";
                ViewBag.TableNames = new List<string>();
                ViewBag.Columns = new Dictionary<string, List<string>>();
            }

            return View();
        }
    }
}