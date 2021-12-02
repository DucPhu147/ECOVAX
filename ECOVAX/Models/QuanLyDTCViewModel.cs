using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ECOVAX.Models
{
    public class QuanLyDTCViewModel
    {
        public string IdDTC { get; set; }
        public string TenDTC { get; set; }
        public string DiaChi { get; set; }
        public string ThoiGianLamViec { get; set; }
        public string IdNguoiPT { get; set; }
        public string UpdateTime { get; set; }
        public string Mode { get; set; }
        public bool DeleteFlag { get; set; }
        public List<SelectListItem> DdlNguoiPT { get; set; }
        public QuanLyDTCViewModel()
        {
            DdlNguoiPT = new List<SelectListItem>();
        }
    }
}