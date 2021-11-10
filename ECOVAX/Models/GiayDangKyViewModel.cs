using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOVAX.Models
{
    public class GiayDangKyViewModel
    {
        public string TenNguoiLH { get; set; }

        public string SDTNguoiLH { get; set; }

        public string QuanHe { get; set; }

        public string SoMui { get; set; }

        public string TenNguoiDK { get; set; }

        public DateTime NgaySinh { get; set; }

        public string GioiTinh { get; set; }

        public string SDTNguoiDK { get; set; }

        public string DiaChi { get; set; }

        public string Email { get; set; }

        public string CMND { get; set; }

        public string NhomUuTien { get; set; }

        public string NgheNghiep { get; set; }

        public DateTime? NgayTiem { get; set; }

        public string BuoiTiem { get; set; }

        public string DiemTiemChung { get; set; }

        public string TenVaccineMuiMot { get; set; }

        public DateTime? NgayTiemMuiMot { get; set; }

        public List<SelectListItem> DdlQuanHe { get; set; }

        public List<SelectListItem> DdlDoiTuongUuTien { get; set; }

        public List<SelectListItem> DdlSoMui { get; set; }

        public List<SelectListItem> DdlGioiTinh { get; set; }

        public List<SelectListItem> DdlBuoiTiem { get; set; }

        public List<SelectListItem> DdlDiemTiemChung { get; set; }

        public List<SelectListItem> DdlVaccine { get; set; }

        public string DiaChiHanhChinh { get; set; }

        public GiayDangKyViewModel()
        {
            DdlQuanHe = new List<SelectListItem>();
            DdlDoiTuongUuTien = new List<SelectListItem>();
            DdlGioiTinh = new List<SelectListItem>();
            DdlSoMui = new List<SelectListItem>();
            DdlBuoiTiem = new List<SelectListItem>();
            DdlDiemTiemChung = new List<SelectListItem>();
            DdlVaccine = new List<SelectListItem>();
        }
    }
}