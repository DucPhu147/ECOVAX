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
    public class PheDuyetGDKController : Controller
    {
        private static PheDuyetGDKViewModel model;
        // GET: PheDuyetGDK
        public ActionResult Index(string idGiayDK)
        {
            if (Session[Constant.USER_INFO] == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }
            model = new PheDuyetGDKViewModel();
            model.IdGiayGK = idGiayDK;
            string query = "SELECT T1.IdGiayDK," +
                                " T1.TenNguoiLH," +
                                " T2.Ten AS 'TenNguoiDK'," +
                                " T2.SDT AS 'SDTNguoiDK'," +
                                " T1.SDTNguoiLH," +
                                " T2.CMND," +
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
                        " WHERE T1.DeleteFlag = 0 AND T1.IdGiayDK LIKE '" + idGiayDK + "'";

            DataTable tb = DataProvider.ExecuteQuery(query);
            model.VaccineMuiMot = tb.Rows[0]["TenVaccineMuiMot"].ToString();
            model.NgayTiemMuiMot = tb.Rows[0]["NgayTiemMuiMot"].ToString();
            model.NgayTiemMongMuon = tb.Rows[0]["NgayTiem"].ToString();
            model.BuoiTiemMongMuon = tb.Rows[0]["BuoiTiem"].ToString();
            model.SoMui = tb.Rows[0]["SoMui"].ToString();
            model.TenNguoiDK = tb.Rows[0]["TenNguoiDK"].ToString();
            model.NgaySinh = tb.Rows[0]["NgaySinh"].ToString();
            model.DiaChi = tb.Rows[0]["DiaChi"].ToString();
            model.SDTNguoiLH = tb.Rows[0]["SDTNguoiLH"].ToString();
            model.TenNguoiLH = tb.Rows[0]["TenNguoiLH"].ToString();
            model.SDTNguoiDK = tb.Rows[0]["SDTNguoiDK"].ToString();
            model.QuanHe = tb.Rows[0]["QuanHe"].ToString();
            model.Email = tb.Rows[0]["Email"].ToString();
            model.CMND = tb.Rows[0]["CMND"].ToString();
            model.UpdateTime = tb.Rows[0]["UpdateTime"].ToString();
            model.NhomUuTien = tb.Rows[0]["NhomUuTien"].ToString();
            model.NgheNghiep = tb.Rows[0]["NgheNghiep"].ToString();

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

            tb = DataProvider.ExecuteQuery("SELECT * FROM tblVaccine");
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                string text = tb.Rows[i]["TenVaccine"].ToString();
                string value = tb.Rows[i]["IdVaccine"].ToString();
                model.DdlVaccine.Add(new SelectListItem() { Text = text, Value = value });
            }

            model.DdlBuoiTiem = DropDownList.DDL_BUOITIEM;
            model.DdlTrangThaiPD = DropDownList.DDL_TRANGTHAIPD;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetDTC(string idVaccine)
        {
            string query = @"SELECT  T1.IdDTC,
                                      T1.TenDTC,
                                      T1.ThoiGianLamViec,
                                      T1.DiaChi,
                                      T5.Ten,
                                      T5.SDT,
                                      SUM(T2.SoLuong) AS SoLuong
                            FROM tblDiemTiemChung T1 
                                 INNER JOIN tblChiTietVaccine T2 ON T1.IdDTC = T2.IdDTC
                                 INNER JOIN tblVaccine T3 ON T3.IdVaccine = T2.IdVaccine
                                 LEFT JOIN tblTaiKhoan T4 ON T1.IdTaiKhoan = T4.IdTaiKhoan 
                                 LEFT JOIN tblThongTin T5 ON T4.IdThongTin = T5.Id
                            WHERE T1.DeleteFlag = 0 AND T2.IdVaccine = " + idVaccine + @"
                            GROUP BY T1.TenDTC,
                                      T1.ThoiGianLamViec,
                                      T1.DiaChi,
                                      T1.IdDTC,
                                      T5.Ten,
                                      T5.SDT";
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetGDKPheDuyet(string dateFrom, string dateTo, string tinhThanh, string quanHuyen, string phuongXa, string nhomUuTien)
        {
            string query = "SELECT T1.IdGiayDK," +
                                " T1.TenNguoiLH," +
                                " T2.Ten AS 'TenNguoiDK'," +
                                " T2.SDT AS 'SDTNguoiDK'," +
                                " T1.SDTNguoiLH," +
                                " T2.CMND," +
                                " T2.DiaChi," +
                                " T2.NgaySinh," +
                                " T2.GioiTinh," +
                                " T1.NgheNghiep," +
                                " T1.Email," +
                                " T4.Ten AS 'NhomUuTien'," +
                                " T1.NgayTiem," +
                                " T1.BuoiTiem," +
                                " T3.Ten AS 'QuanHe'," +
                                " T1.TrangThaiPD," +
                                " T1.ThoiGianDK," +
                                " T1.UpdateTime," +
                                " T1.TenVaccineMuiMot," +
                                " T1.NgayTiemMuiMot," +
                                " T1.SoMui" +
                        " FROM tblGiayDangKy T1 INNER JOIN tblThongTin T2 ON T1.IdThongTin = T2.Id" +
                        " INNER JOIN tblDdlQuanHe T3 ON T3.Id = T1.QuanHe" +
                        " INNER JOIN tblDdlDoiTuongUuTien T4 ON T4.Id = T1.NhomUuTien" +
                        " WHERE T1.DeleteFlag = 0 AND TrangThaiPD LIKE N'Chờ xác nhận'";

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query += " AND ThoiGianDK >= '" + dateFrom + "'";
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query += " AND ThoiGianDK <= '" + dateTo + "'";
            }
            if (!string.IsNullOrEmpty(tinhThanh))
            {
                query += " AND DiaChi LIKE N'%" + tinhThanh + "%'";
            }
            if (!string.IsNullOrEmpty(quanHuyen))
            {
                query += " AND DiaChi LIKE N'%" + quanHuyen + "%'";
            }
            if (!string.IsNullOrEmpty(phuongXa))
            {
                query += " AND DiaChi LIKE N'%" + phuongXa + "%'";
            }
            if (!string.IsNullOrEmpty(nhomUuTien))
            {
                query += " AND T4.Id LIKE '" + nhomUuTien + "'";
            }

            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet); ;
        }

        [HttpGet]
        public ActionResult GetNhomUuTien()
        {
            string query = "SELECT * FROM tblDdlDoiTuongUuTien WHERE DeleteFlag = 0";
            DataTable tb = DataProvider.ExecuteQuery(query);
            string json = JsonConvert.SerializeObject(tb);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PheDuyetGDK(string trangThaiPD, string vaccine, string buoiTiem = "", string idDTC = "", string ngayTiem = "")
        {
            UserModel userModel = (UserModel)Session[Constant.USER_INFO];
            DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tblGiayDangKy WHERE IdGiayDK LIKE '" + model.IdGiayGK + "' AND UpdateTime LIKE '" + model.UpdateTime + "'");
            if (tb.Rows.Count == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }

            if (idDTC != "")
            {
                string query = "SELECT * FROM tblDiemTiemChung WHERE DeleteFlag = 0 AND IdDTC = " + idDTC;
                tb = DataProvider.ExecuteQuery(query);
                if (tb.Rows.Count == 0)
                {
                    return new HttpStatusCodeResult(500, null);
                }
            }
            string updateQuery;
            object[] updateObject;
            if (idDTC == "")
            {
                updateQuery = "EXEC PHEDUYET_tblGiayDangKy @idGiayDK , @trangThaiPD , @ngayTiem , @buoiTiem , @idTK , @tenVaccine";
                updateObject = new object[]{
                                            model.IdGiayGK,
                                            trangThaiPD,
                                            ngayTiem,
                                            buoiTiem,
                                            userModel.IdTaiKhoan,
                                            vaccine};
            }
            else
            {
                updateQuery = "EXEC PHEDUYET_tblGiayDangKy @idGiayDK , @trangThaiPD , @ngayTiem , @buoiTiem , @idTK , @tenVaccine , @idDTC ";
                updateObject = new object[]{
                                            model.IdGiayGK,
                                            trangThaiPD,
                                            ngayTiem,
                                            buoiTiem,
                                            userModel.IdTaiKhoan,
                                            vaccine,
                                            idDTC};
            }
            int result = DataProvider.ExecuteNonQuery(updateQuery, updateObject);
            if (result == 0)
            {
                return new HttpStatusCodeResult(500, null);
            }
            return Json("{}", JsonRequestBehavior.AllowGet); ;
        }
    }
}