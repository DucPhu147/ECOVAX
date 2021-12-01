using ECOVAX.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOVAX.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: ThongKe
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TKMuiDaTiem(string fromDate, string toDate)
        {
            string query = "SELECT TenVaccine, SoMui, Count(LoVaccine) AS 'SoLuong' FROM tblGiayChungNhan WHERE IdGiayCN IS NOT NULL";

            if (!string.IsNullOrEmpty(fromDate))
            {
                query += " AND CAST(ThoiGianTiem as datetime) >= '" + fromDate + "'";
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                query += " AND CAST(ThoiGianTiem as datetime) <= '" + toDate + "'";
            }
            query += " GROUP BY TenVaccine, SoMui ORDER BY TenVaccine";
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TKMuiDaTiem2(string fromDate, string toDate, string tinhThanh, string quanHuyen, string phuongXa)
        {
            string query = @" SELECT T1.TenDTC, T1.DiaChi, COUNT (DISTINCT T2.IdGiayCN) AS SoLuong, SUM(T3.SoLuong) AS SoLuongConLai
                                FROM tblDiemTiemChung T1 
                                LEFT JOIN tblChiTietVaccine T3 ON T1.IdDTC = T3.IdDTC
                                LEFT JOIN tblGiayChungNhan T2 ON T1.IdDTC = T2.IdDTC
                                WHERE T1.IdDTC IS NOT NULL";

            if (!string.IsNullOrEmpty(fromDate))
            {
                query += " AND CAST(T2.ThoiGianTiem as datetime) >= '" + fromDate + "'";
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                query += " AND CAST(T2.ThoiGianTiem as datetime) <= '" + toDate + "'";
            }
            if (!string.IsNullOrEmpty(tinhThanh))
            {
                query += " AND T1.DiaChi LIKE N'%" + tinhThanh + "%'";
            }
            if (!string.IsNullOrEmpty(quanHuyen))
            {
                query += " AND T1.DiaChi LIKE N'%" + quanHuyen + "%'";
            }
            if (!string.IsNullOrEmpty(phuongXa))
            {
                query += " AND T1.DiaChi LIKE N'%" + phuongXa + "%'";
            }
            query += "  GROUP BY T1.TenDTC, T1.DiaChi ORDER BY SoLuong DESC";
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}