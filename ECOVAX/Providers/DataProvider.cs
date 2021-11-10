using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace ECOVAX.Providers
{
    public class DataProvider
    {
        public static readonly string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ECOVAX"].ConnectionString;
        // Hàm chuyên chạy các câu như select, trả về 1 table
        public static DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable tb;
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    if (parameter != null)
                    {
                        string[] listPara = query.Split(' ');
                        int j = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains('@'))
                            {
                                cmd.Parameters.AddWithValue(item, parameter[j] ?? DBNull.Value);
                                j++;
                            }
                        }
                    }
                    using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                    {
                        tb = new DataTable();
                        adap.Fill(tb);
                    }
                    cnn.Close();
                }
            }
            return tb;
        }
        //Hàm chạy thủ tục insert, update, delete có tham số hoặc không có tham số, trả về số dòng bị ảnh hưởng
        public static int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int i;
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    cnn.Open();
                    if (parameter != null)
                    {
                        string[] listPara = query.Split(' ');
                        int j = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains('@'))
                            {
                                cmd.Parameters.AddWithValue(item, parameter[j] ?? DBNull.Value);
                                j++;
                            }
                        }
                    }
                    i = cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
            return i;
        }

        public static string GetNewId(string prefix = "")
        {
            return prefix + Guid.NewGuid().ToString().Replace("-","");
        }

        public static string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
