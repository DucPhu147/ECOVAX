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
    public class QuanLyDTCController : Controller
    {
        private static QuanLyDTCViewModel dtcModel;
        // GET: QuanLyDTC
        public ActionResult Index(string idDTC, string mode)
        {
            if (Session[Constant.USER_INFO] == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }
            dtcModel = new QuanLyDTCViewModel();
            dtcModel.Mode = mode;
            DataTable tb;
            if (mode == "2")
            {
                tb = DataProvider.ExecuteQuery("SELECT T1.IdDTC," +
                                                            " T1.TenDTC," +
                                                            " T1.ThoiGianLamViec," +
                                                            " T1.DiaChi," +
                                                            " T1.IdTaiKhoan," +
                                                            " T1.UpdateTime," +
                                                            " T1.DeleteFlag" +
                                                            " FROM tblDiemTiemChung T1 LEFT JOIN tblTaiKhoan T2 ON T1.IdTaiKhoan = T2.IdTaiKhoan" +
                                                            " WHERE IdDTC LIKE '" + idDTC + "'");
                dtcModel.IdDTC = tb.Rows[0]["IdDTC"].ToString();
                dtcModel.TenDTC = tb.Rows[0]["TenDTC"].ToString();
                dtcModel.ThoiGianLamViec = tb.Rows[0]["ThoiGianLamViec"].ToString();
                dtcModel.IdNguoiPT = tb.Rows[0]["IdTaiKhoan"].ToString();
                dtcModel.DiaChi = tb.Rows[0]["DiaChi"].ToString();
                dtcModel.UpdateTime = tb.Rows[0]["UpdateTime"].ToString();
                dtcModel.DeleteFlag = (bool)tb.Rows[0]["DeleteFlag"];

            }
            tb = DataProvider.ExecuteQuery("SELECT * FROM tblTaiKhoan T1 INNER JOIN tblThongTin T2 ON T1.IdThongTin = T2.Id");

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                string text = tb.Rows[i]["Ten"].ToString();
                string value = tb.Rows[i]["IdTaiKhoan"].ToString();
                dtcModel.DdlNguoiPT.Add(new SelectListItem() { Text = text + " (Mã tài khoản: " + value + ")", Value = value });
            }
            return View(dtcModel);
        }

        [HttpGet]
        public ActionResult GetDTC(string idDTC, string tenDTC, string tinhThanh, string quanHuyen, string phuongXa, bool delFlag)
        {
            int deleteFlag = delFlag ? 1 : 0;
            string query = "SELECT T1.IdDTC," +
                                   "T1.TenDTC," +
                                   "T1.DiaChi," +
                                   "T1.ThoiGianLamViec," +
                                   "T3.Ten," +
                                   "T3.SDT" +
                           " FROM tblDiemTiemChung T1 " +
                           " LEFT JOIN tblTaiKhoan T2 ON T1.IdTaiKhoan = T2.IdTaiKhoan" +
                           " LEFT JOIN tblThongTin T3 ON T2.IdThongTin = T3.Id" +
                           " WHERE T1.DeleteFlag = " + deleteFlag;

            if (!string.IsNullOrEmpty(idDTC))
            {
                query += " AND T1.IdDTC LIKE '%" + idDTC + "%'";
            }
            if (!string.IsNullOrEmpty(tenDTC))
            {
                query += " AND dbo.removeSign(T1.TenDTC) LIKE N'%" + tenDTC + "%' OR T1.TenDTC LIKE N'%" + tenDTC + "%'";
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

            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SubmitDTCForm(string diaChiHanhChinh, string diaChi, string tenDTC, string idNguoiPT, string thoiGianLamViec)
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tblDiemTiemChung WHERE IdDTC LIKE '" + dtcModel.IdDTC + "' AND UpdateTime LIKE '" + dtcModel.UpdateTime + "'");
            if (tb.Rows.Count == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }

            diaChiHanhChinh += " " + diaChi;
            int result;
            if (dtcModel.Mode == "2")
            {
                result = DataProvider.ExecuteNonQuery("EXEC UPDATE_tblDiemTiemChung @IdDTC , @DiaChi , @TenDTC , @ThoiGianLamViec , @IdTaiKhoan",
                     new object[] { dtcModel.IdDTC, diaChiHanhChinh, tenDTC, thoiGianLamViec, idNguoiPT });
            }
            else
            {
                result = DataProvider.ExecuteNonQuery("EXEC INSERT_tblDiemTiemChung @DiaChi , @TenDTC , @ThoiGianLamViec , @IdTaiKhoan",
                     new object[] { diaChiHanhChinh, tenDTC, thoiGianLamViec, idNguoiPT });
            }
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteDTCByDelFlag(string idDTC, string delFlag)
        {
            int result = DataProvider.ExecuteNonQuery("UPDATE tblDiemTiemChung SET DeleteFlag = " + delFlag + " WHERE IdDTC =" + idDTC);
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteDTC(string idDTC)
        {
            int result = DataProvider.ExecuteNonQuery("DELETE FROM tblDiemTiemChung WHERE IdDTC = " + idDTC);
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetVaccine()
        {
            string query = "SELECT * FROM tblVaccine WHERE DeleteFlag = 0";
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLoVaccine(string idDTC)
        {
            string query = "SELECT T1.LoVaccine," +
                            "       T1.SoLuong," +
                            "       T1.Id," +
                            "       T2.TenVaccine" +
                            " FROM tblChiTietVaccine T1 INNER JOIN tblVaccine T2 ON T1.IdVaccine = T2.IdVaccine" +
                            " WHERE T1.IdDTC = " + idDTC;
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddLoVaccine(string idDTC, string soLuong, string loVaccine, string vaccine)
        {
            int result = DataProvider.ExecuteNonQuery("EXEC INSERT_tblChiTietVaccine @IdDTC , @IdVaccine , @SoLuong , @LoVaccine ",
                new object[] { idDTC, vaccine, soLuong, loVaccine });
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteLoVaccine(string id)
        {
            int result = DataProvider.ExecuteNonQuery("EXEC DELETE_tblChiTietVaccine @Id",
                new object[] { id });
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }
    }
}