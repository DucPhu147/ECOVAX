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
                string value = tb.Rows[i]["TenVaccine"].ToString();
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
            DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tblPhieuSangLoc WHERE DeleteFlag = 0");
            DanhMucSangLocModel danhMuc;
            foreach (DataRow row in tb.Rows)
            {
                danhMuc = new DanhMucSangLocModel();
                danhMuc.IdDanhMuc = (int)row["IdPhieuSangLoc"];
                danhMuc.TenDanhMuc = row["TenDanhMuc"].ToString();
                phieuSangLocViewModel.listDanhMucSangLoc.Add(danhMuc);
            }
        }

        private void InsertGiayDangKy()
        {
            if (giayDangKyViewModel != null && phieuSangLocViewModel != null)
            {
                string idGiayDK = DataProvider.GetNewId(Constant.ID_GIAYDK_PREFIX);
                giayDangKyViewModel.DiaChi += " " + giayDangKyViewModel.DiaChiHanhChinh;

                string idThongTin;
                //Check Exist Thong tin
                DataTable tb = DataProvider.ExecuteQuery("SELECT Id FROM tblThongTin WHERE CMND LIKE '" + giayDangKyViewModel.CMND + "'");
                if (tb.Rows.Count > 0)
                {
                    idThongTin = tb.Rows[0]["Id"].ToString();
                }
                else
                {
                    tb = DataProvider.ExecuteQuery("EXEC INSERT_tblThongTin @Ten , @SDT , @CMND , @DiaChi , @NgaySinh , @GioiTinh",
                        new object[] {giayDangKyViewModel.TenNguoiDK, giayDangKyViewModel.SDTNguoiDK, giayDangKyViewModel.CMND,
                        giayDangKyViewModel.DiaChi, giayDangKyViewModel.NgaySinh, giayDangKyViewModel.GioiTinh});

                    idThongTin = tb.Rows[0]["Id"].ToString();
                }

                if (giayDangKyViewModel.SoMui == "1")
                {
                    giayDangKyViewModel.TenVaccineMuiMot = null;
                    giayDangKyViewModel.NgayTiemMuiMot = null;
                }
                object[] giayDKObject = new object[]
                {
                    idGiayDK,
                    idThongTin,
                    giayDangKyViewModel.Email,
                    giayDangKyViewModel.NhomUuTien,
                    giayDangKyViewModel.NgheNghiep,
                    giayDangKyViewModel.TenNguoiLH,
                    giayDangKyViewModel.SDTNguoiLH,
                    giayDangKyViewModel.QuanHe,
                    giayDangKyViewModel.SoMui,
                    Constant.TRANG_THAI_CHO_XN,
                    giayDangKyViewModel.TenVaccineMuiMot,
                    giayDangKyViewModel.NgayTiemMuiMot,
                    giayDangKyViewModel.NgayTiem,
                    giayDangKyViewModel.BuoiTiem
                };

                DataProvider.ExecuteNonQuery("EXEC INSERT_tblGiayDangKy @IdGiayDK ," +
                                                                       " @IdThongTin , " +
                                                                       " @Email , " +
                                                                       " @NhomUuTien , " +
                                                                       " @NgheNghiep , " +
                                                                       " @TenNguoiLH , " +
                                                                       " @SDTNguoiLH , " +
                                                                       " @QuanHe , " +
                                                                       " @SoMui , " +
                                                                       " @TrangThaiPD , " +
                                                                       " @TenVaccineMuiMot , " +
                                                                       " @NgayTiemMuiMot , " +
                                                                       " @NgayTiem ," +
                                                                       " @BuoiTiem ", giayDKObject);

                foreach (DanhMucSangLocModel item in phieuSangLocViewModel.listDanhMucSangLoc)
                {
                    DataProvider.ExecuteNonQuery("EXEC INSERT_tblChiTietPhieuSangLoc @IdGiayDK , @IdPhieuSangLoc , @TrangThai", new object[] {
                    idGiayDK, item.IdDanhMuc, item.TrangThai });
                }

                giayDangKyViewModel = null;
            }
        }

        [HttpGet]
        public ActionResult CheckNgayTiemMuiMot(string idVaccine, string ngayTiem, string ngayTiemMongMuon)
        {
            if (!string.IsNullOrEmpty(idVaccine))
            {
                DataTable tb = DataProvider.ExecuteQuery("SELECT * FROM tblVaccine WHERE TenVaccine LIKE '" + idVaccine + "'");
                int thoiHanTiem = (int)tb.Rows[0]["ThoiHanTiem"];
                DateTime dateCheck = DateTime.Now;
                if (!string.IsNullOrEmpty(ngayTiemMongMuon))
                {
                    dateCheck = DateTime.Parse(ngayTiemMongMuon);
                }
                TimeSpan ts = dateCheck - DateTime.Parse(ngayTiem);
                int days = (int)ts.TotalDays;
                if (days <= thoiHanTiem)
                {
                    string message = "Ngày tiêm mũi một phải nhỏ hơn ngày hiện tại ít nhất " + thoiHanTiem + " ngày";
                    if (!string.IsNullOrEmpty(ngayTiemMongMuon))
                    {
                        message = "Ngày tiêm mong muốn phải lớn hơn ngày tiêm mũi một ít nhất " + thoiHanTiem + " ngày";
                    }
                    return Json(new
                    {
                        status = "error",
                        message = message
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("{}", JsonRequestBehavior.AllowGet);
        }
    }
}