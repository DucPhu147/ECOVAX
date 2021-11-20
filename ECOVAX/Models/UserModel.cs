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
        public string Ten { get; set; }
        public string SDT { get; set; }
        public string CMND { get; set; }
        public string IdQuyen { get; set; }
        public string UpdateTime { get; set; }
        public string TenTK { get; set; }
        public string MatKhau { get; set; }

        public UserModel()
        {

        }
        public UserModel (DataRow row)
        {
            IdTaiKhoan = row["IdTaiKhoan"].ToString();
            Ten = row["Ten"].ToString();
            SDT = row["SDT"].ToString();
            CMND = row["CMND"].ToString();
            TenTK = row["TenTK"].ToString();
            MatKhau = row["MatKhau"].ToString();
        }
    }
}