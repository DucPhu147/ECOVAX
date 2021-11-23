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
    public class ThongTinVaccineController : Controller
    {
        // GET: ThongTinVaccine
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetVaccine()
        {
            DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tblVaccine");
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddVaccine(string ten, string thoiHanTiem, string id = null)
        {
            int result = DataProvider.ExecuteNonQuery("EXEC INSERT_tblVaccine @Id , @Ten , @ThoiHanTiem",
                new object[] { id, ten, thoiHanTiem });
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveVaccine(string id)
        {
            int result = DataProvider.ExecuteNonQuery("DELETE FROM tblVaccine WHERE IdVaccine = " + id);
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteVaccine(string id, int delFlag)
        {
            int result = DataProvider.ExecuteNonQuery("UPDATE tblVaccine SET DeleteFlag = " + delFlag + " WHERE IdVaccine = " + id);
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }
    }
}