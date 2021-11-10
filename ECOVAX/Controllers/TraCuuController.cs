using ECOVAX.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOVAX.Controllers
{
    public class TraCuuController : Controller
    {
        // GET: TraCuu
        public ActionResult Index()
        {
            return RedirectToAction("GiayDangKy");
        }

        public ActionResult GiayDangKy()
        {
            return View("GiayDangKy");
        }
        public ActionResult GiayChungNhan()
        {
            return View("GiayChungNhan");
        }
        public ActionResult DiemTiemChung()
        {
            return View("DiemTiemChung");
        }

        [HttpGet]
        public ActionResult GetGiayDangKy(string cmnd, string sdt)
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT  IdGiayDK," +
                                                            " TenNguoiDK," +
                                                            " TenNguoiLH," +
                                                            " SDTNguoiDK," +
                                                            " SDTNguoiLH," +
                                                            " CMND," +
                                                            " TrangThaiPD," +
                                                            " ThoiGianDK" +
                                                      " FROM tblGiayDangKy " +
                                                      " WHERE CMND LIKE '" + cmnd + "'" +
                                                      " AND SDTNguoiDK LIKE '" + sdt + "'" +
                                                      " OR SDTNguoiLH LIKE '" + sdt + "'");
            string json = DataProvider.DataTableToJsonObj(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGiayChungNhan(string cmnd, string ten)
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT  T1.IdGiayCN," +
                                                            " T1.TenCongDan," +
                                                            " T1.CMND," +
                                                            " T1.ThoiGianCap," +
                                                            " T1.NoiDung," +
                                                            " T3.TenDTC," +
                                                            " T2.TenVaccine," +
                                                            " T1.ThoiGianTiem" +
                                                      " FROM tblGiayChungNhan T1" +
                                                      " LEFT JOIN tblVaccine T2 ON T1.IdVaccine = T2.IdVaccine" +
                                                      " LEFT JOIN tblDiemTiemChung T3 ON T1.IdDTC = T3.IdDTC" +
                                                      " WHERE CMND LIKE '" + cmnd + "'" +
                                                      "     AND dbo.removeSign(T1.TenCongDan) LIKE N'%" + ten + "%'" +
                                                      "     OR T1.TenCongDan LIKE N'%" + ten + "%'");
            string json = DataProvider.DataTableToJsonObj(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDiemTiemChung(string tinhThanh = "", string quanHuyen = "", string phuongXa = "")
        {
            string query = "SELECT TOP 20 T1.IdDTC," +
                                    " T1.DiaChi," +
                                    " T1.TenDTC," +
                                    " T2.TenCanBo," +
                                    " T2.SDT" +
                           " FROM tblDiemTiemChung T1" +
                           " LEFT JOIN tblTaiKhoan T2 ON T1.IdTaiKhoan = T2.IdTaiKhoan";

            if (tinhThanh != "")
            {
                query += " WHERE T1.DiaChi LIKE N'%" + tinhThanh + "%'";
            }
            if (quanHuyen != "")
            {
                query += " AND T1.DiaChi LIKE N'%" + quanHuyen + "%'";
            }
            if (phuongXa != "")
            {
                query += " AND T1.DiaChi LIKE N'%" + phuongXa + "%'";
            }
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = DataProvider.DataTableToJsonObj(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}