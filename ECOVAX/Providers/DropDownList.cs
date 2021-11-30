using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOVAX.Providers
{
    public class DropDownList
    {
        public static readonly List<SelectListItem> DDL_GIOITINH = new List<SelectListItem> {
                                                                            new SelectListItem() { Text = "Nam", Value = "Nam" },
                                                                            new SelectListItem() { Text = "Nữ", Value = "Nữ" }
                                                                        };

        public static readonly List<SelectListItem> DDL_BUOITIEM = new List<SelectListItem> {
                                                                            new SelectListItem() { Text = "Sáng", Value = "Sáng" },
                                                                            new SelectListItem() { Text = "Chiều", Value = "Chiều" },
                                                                            new SelectListItem() { Text = "Cả ngày", Value = "Cả ngày" }
                                                                        };

        public static readonly List<SelectListItem> DDL_SOMUI = new List<SelectListItem> {
                                                                            new SelectListItem() { Text = "Mũi thứ nhất", Value = "1" },
                                                                            new SelectListItem() { Text = "Mũi tiếp theo", Value = "2" }
                                                                        };

        public static readonly List<SelectListItem> DDL_TRANGTHAIPD = new List<SelectListItem> {
                                                                            new SelectListItem() { Text = "", Value = null },
                                                                            new SelectListItem() { Text = "Xác nhận", Value = "Xác nhận" },
                                                                            new SelectListItem() { Text = "Từ chối", Value = "Từ chối" }
                                                                        };

        public static readonly List<SelectListItem> DDL_SOMUI_GCN = new List<SelectListItem> {
                                                                            new SelectListItem() { Text = "", Value = null },
                                                                            new SelectListItem() { Text = "1", Value = "1" },
                                                                            new SelectListItem() { Text = "2", Value = "2" }
                                                                        };
    }
}