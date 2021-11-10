using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECOVAX.Models
{
    public partial class PhieuSangLocViewModel
    {
        public List<DanhMucSangLocModel> listDanhMucSangLoc { get; set; }

        public PhieuSangLocViewModel()
        {
            listDanhMucSangLoc = new List<DanhMucSangLocModel>();
        }
    }

}