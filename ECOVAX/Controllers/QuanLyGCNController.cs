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
        private static QuanLyGCNViewModel model = new QuanLyGCNViewModel();
        // GET: QuanLyGCN
        public ActionResult Index()
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tblTaiKhoan T1 INNER JOIN tblThongTin T2 ON T1.IdThongTin = T2.Id");

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                string text = tb.Rows[i]["Ten"].ToString();
                string value = tb.Rows[i]["IdTaiKhoan"].ToString();
                model.DdlNguoiLap.Add(new SelectListItem() { Text = text + " (Mã: " + value + ")", Value = value });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult GetGCN()
        {
            string query = @"SELECT T1.IdGiayCN,
                                   T1.LoVaccine,
                                   T1.ThoiGianTiem,
                                   T1.TenVaccine,
                                   T1.SoMui,
                                   T4.Ten,
                                   T2.TenDTC
                             FROM tblGiayChungNhan T1 INNER JOIN tblDiemTiemChung T2 ON T1.IdDTC = T2.IdDTC
                             LEFT JOIN tblTaiKhoan T3 ON T3.IdTaiKhoan = T1.IdTaiKhoan
                             LEFT JOIN tblThongTin T4 ON T3.IdThongTin = T4.Id";
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}