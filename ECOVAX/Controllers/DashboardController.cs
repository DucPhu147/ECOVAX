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
            if (Session[Constant.USER_INFO] == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }
            dashboardModel = new DashboardModel();
            dashboardModel.MenuId = System.Reflection.MethodBase.GetCurrentMethod().Name;
            dashboardModel.MenuName = "Tổng quan";
            return View("Index", dashboardModel);
        }

        public ActionResult PheDuyetGDK()
        {
            if (Session[Constant.USER_INFO] == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }
            dashboardModel = new DashboardModel();
            dashboardModel.MenuId = System.Reflection.MethodBase.GetCurrentMethod().Name;
            dashboardModel.MenuName = "Phê duyệt giấy đăng ký";
            return View("PheDuyetGDK", dashboardModel);
        }
        public ActionResult QuanLyDTC()
        {
            if (Session[Constant.USER_INFO] == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }
            dashboardModel = new DashboardModel();
            dashboardModel.MenuId = System.Reflection.MethodBase.GetCurrentMethod().Name;
            dashboardModel.SubMenuName = "Điểm tiêm chủng";
            dashboardModel.MenuName = "Quản lý điểm tiêm chủng";
            return View("QuanLyDTC", dashboardModel);
        }
        public ActionResult ThemLoVaccine()
        {
            if (Session[Constant.USER_INFO] == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }
            dashboardModel = new DashboardModel();
            dashboardModel.MenuId = System.Reflection.MethodBase.GetCurrentMethod().Name;
            dashboardModel.SubMenuName = "Điểm tiêm chủng";
            dashboardModel.MenuName = "Thêm lô vắc xin";
            return View("ThemLoVaccine", dashboardModel);
        }
    }
}