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
            if (cmnd == "" || sdt == "")
            {
                return Json("{ }", JsonRequestBehavior.AllowGet); ;
            }
            string query = "SELECT T1.IdGiayDK AS 'IdGiayDK'," +
                                                            " T2.Ten AS 'TenNguoiDK'," +
                                                            " T1.TenNguoiLH," +
                                                            " T2.SDT AS 'SDTNguoiDK'," +
                                                            " T1.SDTNguoiLH," +
                                                            " T3.TenDTC," +
                                                            " T2.CMND AS 'CMND'," +
                                                            " T1.NgayTiem," +
                                                            " T1.BuoiTiem," +
                                                            " T1.TrangThaiPD," +
                                                            " T1.ThoiGianDK," +
                                                            " T1.UpdateTime," +
                                                            " T1.DeleteFlag" +
                                                      " FROM tblGiayDangKy T1 INNER JOIN tblThongTin T2 ON T1.IdThongTin = T2.Id" +
                                                      " LEFT JOIN tblDiemTiemChung T3 ON T1.IdDTC = T3.IdDTC " +
                                                      " WHERE T1.DeleteFlag = 0 AND T2.CMND LIKE '" + cmnd + "'" +
                                                      " AND (T2.SDT LIKE '" + sdt + "'" +
                                                      " OR T1.SDTNguoiLH LIKE '" + sdt + "')";

            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGiayChungNhan(string cmnd, string ten)
        {
            if (cmnd == "" || ten == "")
            {
                return Json("{ }", JsonRequestBehavior.AllowGet); ;
            }
            string query = "SELECT  T1.IdGiayCN," +
                                                            " T1.SoMui," +
                                                            " T1.LoVaccine," +
                                                            " T1.TenVaccine," +
                                                            " T1.ThoiGianTiem," +
                                                            " T2.TenDTC," +
                                                            " T4.Ten," +
                                                            " T4.CMND" +
                                                      " FROM tblGiayChungNhan T1" +
                                                      " LEFT JOIN tblDiemTiemChung T2 ON T1.IdDTC = T2.IdDTC" +
                                                      " LEFT JOIN tblGiayDangKy T3 ON T1.IdGiayDK = T3.IdGiayDK" +
                                                      " INNER JOIN tblThongTin T4 ON T4.Id = T3.IdThongTin" +
                                                      " WHERE T4.CMND LIKE '" + cmnd + "'" +
                                                      "     AND (dbo.removeSign(T4.Ten) LIKE N'%" + ten + "%'" +
                                                      "     OR T4.Ten LIKE N'%" + ten + "%')";
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDiemTiemChung(string tinhThanh = "", string quanHuyen = "", string phuongXa = "")
        {
            string query = "SELECT TOP 20 T1.IdDTC," +
                                    " T1.DiaChi," +
                                    " T1.TenDTC," +
                                    " T3.Ten AS TenCanBo," +
                                    " T3.SDT" +
                           " FROM tblDiemTiemChung T1" +
                           " LEFT JOIN tblTaiKhoan T2 ON T2.IdTaiKhoan = T1.IdTaiKhoan" +
                           " LEFT JOIN tblThongTin T3 ON T3.Id = T2.IdThongTin WHERE T1.DeleteFlag = 0";

            if (tinhThanh != "")
            {
                query += " AND T1.DiaChi LIKE N'%" + tinhThanh + "%'";
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
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult XoaGiayDangKy(string idGDK, string updateTime)
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT UpdateTime FROM tblGiayDangKy" +
                                                        " WHERE IdGiayDK LIKE '" + idGDK + "' AND UpdateTime LIKE '" + updateTime + "'");

            if (tb.Rows.Count == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }

            int i = DataProvider.ExecuteNonQuery("exec DELETE_LOGIC_tblGiayDangKy @idGiayDK", new object[] { idGDK });
            if (i > 0)
            {
                return Json("{}", JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(500, null);
        }
    }
}