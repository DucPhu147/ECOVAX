using ECOVAX.Models;
using ECOVAX.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOVAX.Controllers
{
    public class DangNhapController : Controller
    {
        // GET: DangNhap
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BeginLogin(string tenDN, string mk)
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT T1.IdTaiKhoan," +
                                                        " T1.TenTK," +
                                                        " T1.MatKhau," +
                                                        " T2.Ten," +
                                                        " T2.SDT," +
                                                        " T2.CMND," +
                                                        " T1.UpdateTime" +
                                                        " FROM tblTaiKhoan T1 LEFT JOIN tblThongTin T2 ON T1.IdThongTin = T2.Id" +
                                                        " WHERE T1.TenTK LIKE '" + tenDN + "'" +
                                                        " AND T1.MatKhau LIKE '" + mk + "'");

            if (tb.Rows.Count > 0)
            {
                UserModel user = new UserModel(tb.Rows[0]);
                Session[Constant.USER_INFO] = user;
                return Json("{}", JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(500, null);
        }
    }
}