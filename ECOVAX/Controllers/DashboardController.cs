using ECOVAX.Models;
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
    public class DashboardController : Controller
    {
        private static DashboardModel dashboardModel = new DashboardModel();
        // GET: Dashboard
        public ActionResult Index()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }
            dashboardModel.MenuId = System.Reflection.MethodBase.GetCurrentMethod().Name;
            dashboardModel.MenuName = "Tổng quan";
            return View("Index", dashboardModel);
        }

        public ActionResult PheDuyetGDK()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }
            dashboardModel.MenuId = System.Reflection.MethodBase.GetCurrentMethod().Name;
            dashboardModel.MenuName = "Phê duyệt giấy đăng ký";
            return View("PheDuyetGDK", dashboardModel);
        }

        [HttpGet]
        public ActionResult GetGDKPheDuyet(string dateFrom, string dateTo, string tinhThanh, string quanHuyen, string phuongXa, string nhomUuTien)
        {
            string query = "SELECT T1.IdGiayDK," +
                                " T1.TenNguoiLH," +
                                " T2.Ten AS 'TenNguoiDK'," +
                                " T2.SDT AS 'SDTNguoiDK'," +
                                " T1.SDTNguoiLH," +
                                " T2.CMND," +
                                " T2.DiaChi," +
                                " T2.NgaySinh," +
                                " T2.GioiTinh," +
                                " T1.NgheNghiep," +
                                " T1.Email," +
                                " T4.Ten AS 'NhomUuTien'," +
                                " T1.NgayTiem," +
                                " T1.BuoiTiem," +
                                " T3.Ten AS 'QuanHe'," +
                                " T1.TrangThaiPD," +
                                " T1.ThoiGianDK," +
                                " T1.UpdateTime," +
                                " T1.TenVaccineMuiMot," +
                                " T1.NgayTiemMuiMot," +
                                " T1.SoMui" +
                        " FROM tblGiayDangKy T1 INNER JOIN tblThongTin T2 ON T1.IdThongTin = T2.Id" +
                        " INNER JOIN tblDdlQuanHe T3 ON T3.Id = T1.QuanHe" +
                        " INNER JOIN tblDdlDoiTuongUuTien T4 ON T4.Id = T1.NhomUuTien" +
                        " WHERE T1.DeleteFlag = 0 AND TrangThaiPD LIKE N'Chờ xác nhận'";

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query += " AND ThoiGianDK >= '" + dateFrom + "'";
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query += " AND ThoiGianDK <= '" + dateTo + "'";
            }
            if (!string.IsNullOrEmpty(tinhThanh))
            {
                query += " AND DiaChi LIKE N'%" + tinhThanh + "%'";
            }
            if (!string.IsNullOrEmpty(quanHuyen))
            {
                query += " AND DiaChi LIKE N'%" + quanHuyen + "%'";
            }
            if (!string.IsNullOrEmpty(phuongXa))
            {
                query += " AND DiaChi LIKE N'%" + phuongXa + "%'";
            }
            if (!string.IsNullOrEmpty(nhomUuTien))
            {
                query += " AND T4.Id LIKE '" + nhomUuTien + "'";
            }

            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet); ;
        }

        [HttpGet]
        public ActionResult GetNhomUuTien()
        {
            string query = "SELECT * FROM tblDdlDoiTuongUuTien WHERE DeleteFlag = 0";
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}