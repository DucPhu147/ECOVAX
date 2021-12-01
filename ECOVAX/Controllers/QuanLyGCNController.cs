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
    public class QuanLyGCNController : Controller
    {
        private static QuanLyGCNViewModel model;
        // GET: QuanLyGCN
        public ActionResult Index(string idGCN, string mode)
        {
            model = new QuanLyGCNViewModel();
            DataTable tb;
            if (mode == "2")
            {
                string query = @"SELECT T1.IdGiayCN,
                                   T1.LoVaccine,
                                   T1.ThoiGianTiem,
                                   T1.TenVaccine,
                                   T1.SoMui,
                                   T4.Ten,
                                   T4.CMND,
                                   T1.UpdateTime,
                                   T1.IdDTC,
                                   T1.IdThongTin,
                                   T1.IdTaiKhoan
                             FROM tblGiayChungNhan T1 LEFT JOIN tblThongTin T2 ON T1.IdThongTin = T2.Id
                             LEFT JOIN tblTaiKhoan T3 ON T3.IdTaiKhoan = T1.IdTaiKhoan
                             LEFT JOIN tblThongTin T4 ON T3.IdThongTin = T4.Id
                             WHERE T1.IdGiayCN LIKE '" + idGCN + "'";
                tb = DataProvider.ExecuteQuery(query);

                model.IdGCN = tb.Rows[0]["IdGiayCN"].ToString();
                model.TenVaccine = tb.Rows[0]["TenVaccine"].ToString();
                model.LoVaccine = tb.Rows[0]["LoVaccine"].ToString();
                model.IdNguoiLap = tb.Rows[0]["IdTaiKhoan"].ToString();
                model.UpdateTime = tb.Rows[0]["UpdateTime"].ToString();
                model.ThoiGianTiem = tb.Rows[0]["ThoiGianTiem"].ToString();
                model.IdDTC = tb.Rows[0]["IdDTC"].ToString();
                model.SoMui = tb.Rows[0]["SoMui"].ToString();
                model.CMND = tb.Rows[0]["CMND"].ToString();

                if (model.IdDTC != null)
                {
                    string query2 = @"SELECT T1.LoVaccine,
                        T2.TenVaccine
                        FROM tblChiTietVaccine T1 INNER JOIN tblVaccine T2 ON T1.IdVaccine = T2.IdVaccine
                        WHERE T1.IdDTC = " + model.IdDTC;

                    tb = DataProvider.ExecuteQuery(query2);

                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        string text = tb.Rows[i]["TenVaccine"].ToString();
                        string value = tb.Rows[i]["LoVaccine"].ToString();
                        model.DdlLoVaccine.Add(new SelectListItem() { Text = value + "(" + text + ")", Value = value });
                    }
                }
            }
            tb = DataProvider.ExecuteQuery("SELECT * FROM tblDiemTiemChung T1 WHERE DeleteFlag = 0");

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                string text = tb.Rows[i]["TenDTC"].ToString();
                string value = tb.Rows[i]["IdDTC"].ToString();
                model.DdlDTC.Add(new SelectListItem() { Text = text, Value = value });
            }
            model.Mode = mode;
            model.DdlSoMui = DropDownList.DDL_SOMUI_GCN;

            return View(model);
        }

        [HttpGet]
        public ActionResult GetNguoiLap()
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tblTaiKhoan T1 INNER JOIN tblThongTin T2 ON T1.IdThongTin = T2.Id");

            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLoVaccineByIdDTC(string idDTC)
        {
            string query = @"SELECT T1.LoVaccine,
                        T2.TenVaccine
                        FROM tblChiTietVaccine T1 INNER JOIN tblVaccine T2 ON T1.IdVaccine = T2.IdVaccine
                        WHERE T1.IdDTC = " + idDTC;
            DataTable tb = DataProvider.ExecuteQuery(query);

            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetGCN(string idGCN, string tenDTC, string nguoiLap, string soMui, string loVaccine)
        {
            string query = @"SELECT T1.IdGiayCN,
                                   T1.LoVaccine,
                                   T1.ThoiGianTiem,
                                   T1.TenVaccine,
                                   T1.SoMui,
                                   T4.Ten AS 'TenNguoiLap',
                                   T2.TenDTC,
                                    T5.CMND,
                                    T5.Ten AS 'TenNguoiTiem'
                             FROM tblGiayChungNhan T1 INNER JOIN tblDiemTiemChung T2 ON T1.IdDTC = T2.IdDTC
                             LEFT JOIN tblTaiKhoan T3 ON T3.IdTaiKhoan = T1.IdTaiKhoan
                             LEFT JOIN tblThongTin T4 ON T3.IdThongTin = T4.Id
                             LEFT JOIN tblThongTin T5 ON T1.IdThongTin = T5.Id
                             WHERE T1.IdGiayCN IS NOT NULL";
            if (!string.IsNullOrEmpty(idGCN))
            {
                query += " AND T1.IdGiayCN LIKE '%" + idGCN + "%'";
            }
            if (!string.IsNullOrEmpty(tenDTC))
            {
                query += " AND dbo.removeSign(T2.TenDTC) LIKE N'%" + tenDTC + "%' OR T2.TenDTC LIKE N'%" + tenDTC + "%'";
            }
            if (!string.IsNullOrEmpty(nguoiLap))
            {
                query += " AND dbo.removeSign(T4.Ten) LIKE N'%" + nguoiLap + "%' OR T4.Ten LIKE N'%" + nguoiLap + "%'";
            }
            if (!string.IsNullOrEmpty(soMui))
            {
                query += " AND T1.SoMui LIKE '%" + soMui + "%'";
            }
            if (!string.IsNullOrEmpty(loVaccine))
            {
                query += " AND T1.LoVaccine LIKE '%" + loVaccine + "%'";
            }
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SubmitGCNForm(string cmnd, string idDTC, string loVaccine, string soMui)
        {
            UserModel userModel = (UserModel)Session[Constant.USER_INFO];
            DataTable tb;
            if (model.UpdateTime != null)
            {
                tb = DataProvider.ExecuteQuery("SELECT * FROM tblGiayChungNhan WHERE IdGiayCN LIKE '" + model.IdGCN + "' AND UpdateTime LIKE '" + model.UpdateTime + "'");
                if (tb.Rows.Count == 0)
                {
                    return new HttpStatusCodeResult(500, null);
                }
            }
            //Get Id thông tin
            tb = DataProvider.ExecuteQuery("SELECT * FROM tblThongTIn WHERE CMND LIKE '" + cmnd + "'");
            if (tb.Rows.Count == 0)
            {
                return Json(new
                {
                    status = "error",
                    message = "Không tìm thấy CMND người đăng ký tiêm"
                }, JsonRequestBehavior.AllowGet);
            }
            string idThongTin = tb.Rows[0]["Id"].ToString();

            //Get tên vắc xin
            tb = DataProvider.ExecuteQuery(@"SELECT T1.LoVaccine,
                                                    T2.TenVaccine
                                                    FROM tblChiTietVaccine T1 INNER JOIN tblVaccine T2 ON T1.IdVaccine = T2.IdVaccine
                                                    WHERE T1.LoVaccine LIKE '" + loVaccine + "'");
            if (tb.Rows.Count == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            string tenVaccine = tb.Rows[0]["TenVaccine"].ToString();

            if (model.Mode == "2")
            {
                DataProvider.ExecuteQuery(@"EXEC UPDATE_tblGiayChungNhan @IdGiayCN , 
                                                                               @IdThongTin , 
                                                                               @IdDTC , 
                                                                               @SoMui , 
                                                                               @LoVaccine , 
                                                                               @TenVaccine ",
                     new object[] { model.IdGCN, idThongTin, idDTC, soMui, loVaccine, tenVaccine });
                return Json("{}", JsonRequestBehavior.AllowGet);
            }
            else
            {
                string idGiayCN = DataProvider.GetNewId(Constant.ID_GIAYCN_PREFIX);
                DateTime today = DateTime.Today;
                string date = today.ToString("MM/dd/yyyy");
                DataProvider.ExecuteQuery(@"EXEC INSERT_tblGiayChungNhan @IdGiayCN 
                                                                               , @IdTaiKhoan 
                                                                               , @IdThongTin 
                                                                               , @IdDTC 
                                                                               , @SoMui 
                                                                               , @LoVaccine 
                                                                               , @TenVaccine 
                                                                               , @ThoiGianTiem",
                     new object[] { idGiayCN, userModel.IdTaiKhoan, idThongTin, idDTC, soMui, loVaccine, tenVaccine, date });

                DataProvider.ExecuteQuery(@"UPDATE tblChiTietVaccine SET SoLuong = SoLuong - 1 WHERE LoVaccine LIKE '" + loVaccine + "'");

                return Json(idGiayCN, JsonRequestBehavior.AllowGet);
            }
        }
    }
}