using ECOVAX.Models;
using ECOVAX.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ECOVAX.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    public class DangKyTiemController : Controller
    {
        private static GiayDangKyViewModel giayDangKyViewModel;
        private static PhieuSangLocViewModel phieuSangLocViewModel;
        public ActionResult Index()
        {
            giayDangKyViewModel = new GiayDangKyViewModel();

            DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tbl" +
                "DdlQuanHe");
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                string text = tb.Rows[i]["Ten"].ToString();
                string value = tb.Rows[i]["Id"].ToString();
                giayDangKyViewModel.DdlQuanHe.Add(new SelectListItem() { Text = i + 1 + ". " + text, Value = value });
            }

            tb = DataProvider.ExecuteQuery("SELECT * FROM tblDiemTiemChung");
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                string text = tb.Rows[i]["TenDTC"].ToString();
                string value = tb.Rows[i]["IdDTC"].ToString();
                giayDangKyViewModel.DdlDiemTiemChung.Add(new SelectListItem() { Text = text, Value = value });
            }

            tb = DataProvider.ExecuteQuery("SELECT * FROM tblDdlDoiTuongUuTien");
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                string text = tb.Rows[i]["Ten"].ToString();
                string value = tb.Rows[i]["Id"].ToString();
                giayDangKyViewModel.DdlDoiTuongUuTien.Add(new SelectListItem() { Text = i + 1 + ". " + text, Value = value });
            }

            tb = DataProvider.ExecuteQuery("SELECT * FROM tblVaccine");
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                string text = tb.Rows[i]["TenVaccine"].ToString();
                string value = tb.Rows[i]["IdVaccine"].ToString();
                giayDangKyViewModel.DdlVaccine.Add(new SelectListItem() { Text = text, Value = value });
            }

            giayDangKyViewModel.DdlGioiTinh = DropDownList.DDL_GIOITINH;

            giayDangKyViewModel.DdlSoMui = DropDownList.DDL_SOMUI;

            giayDangKyViewModel.DdlBuoiTiem = DropDownList.DDL_BUOITIEM;

            return View("Index", giayDangKyViewModel);
        }

        [HttpPost]
        public ActionResult SubmitPhieuDangKy(GiayDangKyViewModel formItem)
        {
            giayDangKyViewModel = formItem;
            GetDanhMucSangLoc();
            return RedirectToAction("SangLoc");
        }

        public ActionResult SangLoc()
        {
            return View("PhieuSangLoc", phieuSangLocViewModel);
        }

        [HttpPost]
        public ActionResult SubmitPhieuSangLoc(PhieuSangLocViewModel formItem)
        {
            phieuSangLocViewModel = formItem;
            return RedirectToAction("XacNhanTiem");
        }

        public ActionResult XacNhanTiem()
        {
            return View("XacNhanTiem");
        }

        [HttpPost]
        public ActionResult SubmitXacNhanTiem()
        {
            return RedirectToAction("HoanTat");
        }
        public ActionResult HoanTat()
        {
            if (giayDangKyViewModel.TenNguoiLH != null && giayDangKyViewModel.TenNguoiLH.Trim() != "")
            {
                InsertGiayDangKy();
            }
            return View("HoanTat");
        }

        private void GetDanhMucSangLoc()
        {
            phieuSangLocViewModel = new PhieuSangLocViewModel();
            DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tblDanhMucSangLoc");
            DanhMucSangLocModel danhMuc;
            foreach (DataRow row in tb.Rows)
            {
                danhMuc = new DanhMucSangLocModel();
                danhMuc.IdDanhMuc = (int)row["IdDanhMuc"];
                danhMuc.TenDanhMuc = row["TenDanhMuc"].ToString();
                phieuSangLocViewModel.listDanhMucSangLoc.Add(danhMuc);
            }
        }

        private void InsertGiayDangKy()
        {
            if (giayDangKyViewModel != null)
            {
                string idGiayDK = DataProvider.GetNewId(Constant.ID_GIAYDK_PREFIX);
                giayDangKyViewModel.DiaChi += " " + giayDangKyViewModel.DiaChiHanhChinh;

                if (giayDangKyViewModel.SoMui == "1")
                {
                    giayDangKyViewModel.TenVaccineMuiMot = null;
                    giayDangKyViewModel.NgayTiemMuiMot = null;
                }
                object[] giayDKObject = new object[]
                {
                idGiayDK,
                giayDangKyViewModel.DiemTiemChung,
                giayDangKyViewModel.TenNguoiDK,
                giayDangKyViewModel.SDTNguoiDK,
                giayDangKyViewModel.CMND,
                giayDangKyViewModel.DiaChi,
                giayDangKyViewModel.Email,
                giayDangKyViewModel.NhomUuTien,
                giayDangKyViewModel.GioiTinh,
                giayDangKyViewModel.NgaySinh,
                giayDangKyViewModel.NgheNghiep,
                giayDangKyViewModel.TenNguoiLH,
                giayDangKyViewModel.SDTNguoiLH,
                giayDangKyViewModel.QuanHe,
                giayDangKyViewModel.NgayTiem,
                giayDangKyViewModel.BuoiTiem,
                giayDangKyViewModel.SoMui,
                Constant.TRANG_THAI_CHO_XN,
                giayDangKyViewModel.TenVaccineMuiMot,
                giayDangKyViewModel.NgayTiemMuiMot
                };

                DataProvider.ExecuteNonQuery("EXEC INSERT_tblGiayDangKy @IdGiayDK ," +
                                                                       " @IdDTC , " +
                                                                       " @TenNguoiDK , " +
                                                                       " @SDTNguoiDK , " +
                                                                       " @CMND , " +
                                                                       " @DiaChi , " +
                                                                       " @Email , " +
                                                                       " @NhomUuTien , " +
                                                                       " @GioiTinh , " +
                                                                       " @NgaySinh , " +
                                                                       " @NgheNghiep , " +
                                                                       " @TenNguoiLH , " +
                                                                       " @SDTNguoiLH , " +
                                                                       " @QuanHe , " +
                                                                       " @NgayTiem , " +
                                                                       " @BuoiTiem , " +
                                                                       " @SoMui , " +
                                                                       " @TrangThaiPD , " +
                                                                       " @TenVaccineMuiMot , " +
                                                                       " @NgayTiemMuiMot ", giayDKObject);

                string idPhieuSL = DataProvider.GetNewId(Constant.ID_PHIEUSL_PREFIX);

                DataProvider.ExecuteNonQuery("EXEC INSERT_tblPhieuSangLoc @IdPhieuSangLoc , @IdGiayDK", new object[] { idPhieuSL, idGiayDK });

                foreach (DanhMucSangLocModel item in phieuSangLocViewModel.listDanhMucSangLoc)
                {
                    DataProvider.ExecuteNonQuery("EXEC INSRERT_tblChiTietPhieuSangLoc @IdDanhMuc , @IdPhieuSangLoc , @TrangThai", new object[] {
                    item.IdDanhMuc, idPhieuSL, item.TrangThai });
                }

                giayDangKyViewModel = null;
            }
        }
    }
}