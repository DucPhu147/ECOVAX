using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOVAX.Models
{
    public class UserModel
    {
        public string IdTaiKhoan { get; set; }
        public string TenCanBo { get; set; }
        public string SDT { get; set; }
        public string CMND { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string IdQuyen { get; set; }
        public string UpdateTime { get; set; }
        public bool DeleteFlag { get; set; }
        public string TenTK { get; set; }
        public string MatKhau { get; set; }

        public UserModel()
        {

        }
        public UserModel (DataRow row)
        {
            IdTaiKhoan = row["IdTaiKhoan"].ToString();
            TenCanBo = row["TenCanBo"].ToString();
            SDT = row["SDT"].ToString();
            CMND = row["CMND"].ToString();
            DiaChi = row["DiaChi"].ToString();
            NgaySinh = DateTime.Parse(row["NgaySinh"].ToString());
             GioiTinh = row["GioiTinh"].ToString();
            IdQuyen = row["IdQuyen"].ToString();
            DiaChi = row["DiaChi"].ToString();
            UpdateTime = row["UpdateTime"].ToString();
            DeleteFlag = (bool)row["DeleteFlag"];
            TenTK = row["TenTK"].ToString();
            MatKhau = row["MatKhau"].ToString();
        }
    }
}