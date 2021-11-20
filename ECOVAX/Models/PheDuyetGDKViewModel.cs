using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ECOVAX.Models
{
    public class PheDuyetGDKViewModel
    {
        public List<DanhMucSangLocModel> listDanhMucSangLoc { get; set; }
        public string IdGiayGK { get; set; }
        public string BuoiTiemThucTe { get; set; }
        public string BuoiTiemMongMuon { get; set; }
        public string NgayTiemMongMuon { get; set; }
        public string UpdateTime { get; set; }
        public DateTime NgayTiemThucTe { get; set; }
        public string VaccineMuiMot { get; set; }
        public string SoMui { get; set; }
        public string TrangThaiPD { get; set; }
        public string NgayTiemMuiMot { get; set; }
        public string IdDTC { get; set; }
        public string TenNguoiDK { get; set; }
        public string TenNguoiLH { get; set; }
        public string SDTNguoiLH { get; set; }
        public string SDTNguoiDK { get; set; }
        public string QuanHe { get; set; }
        public string NgheNghiep { get; set; }
        public string Email { get; set; }
        public string NhomUuTien { get; set; }
        public string CMND { get; set; }
        public string NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string Vaccine { get; set; }
        public List<SelectListItem> DdlBuoiTiem { get; set; }
        public List<SelectListItem> DdlTrangThaiPD { get; set; }
        public List<SelectListItem> DdlVaccine { get; set; }
        public PheDuyetGDKViewModel()
        {
            listDanhMucSangLoc = new List<DanhMucSangLocModel>();
            DdlBuoiTiem = new List<SelectListItem>();
            DdlTrangThaiPD = new List<SelectListItem>();
            DdlVaccine = new List<SelectListItem>();
        }
    }
}