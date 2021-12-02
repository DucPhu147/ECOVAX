using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ECOVAX.Models
{
    public class QuanLyGCNViewModel
    {
        public string IdGCN { get; set; }
        public string LoVaccine { get; set; }
        public string TenVaccine { get; set; }
        public string IdDTC { get; set; }
        public string IdGiayDK { get; set; }
        public string IdNguoiLap { get; set; }
        public List<SelectListItem> DdlDTC { get; set; }
        public string ThoiGianTiem { get; set; }
        public string CMND { get; set; }
        public string IdThongTin { get; set; }
        public string TenNguoiDK { get; set; }
        public string SoMui { get; set; }
        public string TenDTC { get; set; }
        public string Mode { get; set; }
        public List<SelectListItem> DdlLoVaccine { get; set; }
        public List<SelectListItem> DdlSoMui { get; set; }
        public string UpdateTime { get; set; }
        public QuanLyGCNViewModel()
        {
            DdlDTC = new List<SelectListItem>();
            DdlLoVaccine = new List<SelectListItem>();
            DdlSoMui = new List<SelectListItem>();
        }
    }
}