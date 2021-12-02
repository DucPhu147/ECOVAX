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
    public class PrintController : Controller
    {
        private static PrintViewModel model;
        // GET: Print
        public ActionResult Index(string idGiayDK)
        {
            model = new PrintViewModel();
            string query = "SELECT T1.IdGiayDK," +
                    " T1.TenNguoiLH," +
                    " T2.Ten AS 'TenNguoiDK'," +
                    " T2.SDT AS 'SDTNguoiDK'," +
                    " T1.SDTNguoiLH," +
                    " T1.IdDTC," +
                    " T2.CMND," +
                    " T1.ThoiGianDK," +
                    " T2.DiaChi," +
                    " T2.NgaySinh," +
                    " T2.GioiTinh," +
                    " T1.NgheNghiep," +
                    " T1.Email," +
                    " T4.Ten AS 'NhomUuTien'," +
                    " T1.NgayTiem," +
                    " T1.BuoiTiem," +
                    " T3.Ten AS 'QuanHe'," +
                    " T1.TenVaccineMuiMot," +
                    " T1.UpdateTime," +
                    " T1.NgayTiemMuiMot," +
                    " T1.SoMui" +
            " FROM tblGiayDangKy T1 INNER JOIN tblThongTin T2 ON T1.IdThongTin = T2.Id" +
            " LEFT JOIN tblDdlQuanHe T3 ON T3.Id = T1.QuanHe" +
            " LEFT JOIN tblDdlDoiTuongUuTien T4 ON T4.Id = T1.NhomUuTien" +
            " WHERE T1.IdGiayDK LIKE '" + idGiayDK + "'";

            DataTable tb = DataProvider.ExecuteQuery(query);
            model.IdGiayDK = idGiayDK;
            model.VaccineMuiMot = tb.Rows[0]["TenVaccineMuiMot"].ToString();
            model.NgayTiemMuiMot = tb.Rows[0]["NgayTiemMuiMot"].ToString();
            model.NgayTiemMongMuon = tb.Rows[0]["NgayTiem"].ToString().Replace("12:00:00 AM", "");
            model.BuoiTiemMongMuon = tb.Rows[0]["BuoiTiem"].ToString();
            model.SoMui = tb.Rows[0]["SoMui"].ToString();
            model.TenNguoiDK = tb.Rows[0]["TenNguoiDK"].ToString();
            model.NgaySinh = tb.Rows[0]["NgaySinh"].ToString().Replace("12:00:00 AM", "");
            model.DiaChi = tb.Rows[0]["DiaChi"].ToString();
            model.SDTNguoiLH = tb.Rows[0]["SDTNguoiLH"].ToString();
            model.TenNguoiLH = tb.Rows[0]["TenNguoiLH"].ToString();
            model.SDTNguoiDK = tb.Rows[0]["SDTNguoiDK"].ToString();
            model.QuanHe = tb.Rows[0]["QuanHe"].ToString();
            model.Email = tb.Rows[0]["Email"].ToString();
            model.CMND = tb.Rows[0]["CMND"].ToString();
            model.UpdateTime = tb.Rows[0]["UpdateTime"].ToString();
            model.ThoiGianDK = tb.Rows[0]["ThoiGianDK"].ToString();
            model.NhomUuTien = tb.Rows[0]["NhomUuTien"].ToString();
            model.NgheNghiep = tb.Rows[0]["NgheNghiep"].ToString();
            model.IdDTC = tb.Rows[0]["IdDTC"].ToString();

            tb = DataProvider.ExecuteQuery("SELECT * FROM tblDiemTiemChung WHERE IdDTC = " + model.IdDTC);
            model.TenDTC = tb.Rows[0]["TenDTC"].ToString();

            tb = DataProvider.ExecuteQuery("SELECT T1.IdPhieuSangLoc AS 'IdPhieuSangLoc'," +
                                                    "        T1.TrangThai," +
                                                    "        T2.TenDanhMuc" +
                                                    " FROM tblChiTietPhieuSangLoc T1" +
                                                    "   INNER JOIN tblPhieuSangLoc T2 ON T1.IdPhieuSangLoc = T2.IdPhieuSangLoc" +
                                                    " WHERE IdGiayDangKy LIKE '" + idGiayDK + "'");
            DanhMucSangLocModel danhMuc;
            foreach (DataRow row in tb.Rows)
            {
                danhMuc = new DanhMucSangLocModel();
                danhMuc.TenDanhMuc = row["TenDanhMuc"].ToString();
                danhMuc.TrangThai = row["TrangThai"].ToString();
                model.listDanhMucSangLoc.Add(danhMuc);
            }
            return View(model);
        }
    }
}