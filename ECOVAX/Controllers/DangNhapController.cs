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


        [HttpPost]
        public ActionResult BeginLogin(string tenDN, string mk)
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT * " +
                                                        " FROM tblTaiKhoan " +
                                                        " WHERE TenTK LIKE '" + tenDN + "'" +
                                                        " AND MatKhau LIKE '" + mk + "'");

            if (tb.Rows.Count > 0)
            {
                UserModel user = new UserModel(tb.Rows[0]);
                Session["UserInfo"] = user;
                return View("/Dashboard/");
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }
    }
}