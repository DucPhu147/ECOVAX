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
    public class DanhMucPhieuSangLocController : Controller
    {
        // GET: PhieuSangLoc
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetPhieuSangLoc()
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tblPhieuSangLoc");
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddPhieuSangLoc(string ten, string id = null)
        {
            int result = DataProvider.ExecuteNonQuery("EXEC INSERT_tblPhieuSangLoc @Id , @Ten",
                new object[] { id, ten });
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemovePhieuSangLoc(string id)
        {
            int result = DataProvider.ExecuteNonQuery("DELETE FROM tblPhieuSangLoc WHERE IdPhieuSangLoc = " + id);
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletePhieuSangLoc(string id, int delFlag)
        {
            int result = DataProvider.ExecuteNonQuery("UPDATE tblPhieuSangLoc SET DeleteFlag = " + delFlag + " WHERE IdPhieuSangLoc = " + id);
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }
    }
}