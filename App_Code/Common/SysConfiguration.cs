using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace Common
{
    public class SysConfiguration
    {
        public static SqlConnection GetErpSqlConn()
        {
            SqlConnection _conn = new SqlConnection(SysConfiguration.GetErpSqlConnStr);
            return _conn;
        }

        public static string GetErpSqlConnStr
        {
            get
            {
                if (SysConfiguration.GetCurCity == "杭州")
                {
                    return ConfigurationManager.AppSettings["ConnectionErpSqlServer"].ToString();
                }
                else if (SysConfiguration.GetCurCity == "金华")
                {
                    return ConfigurationManager.AppSettings["JHConnectionErpSqlServer"].ToString();
                }
                return "";

            }
        }

        public static string GetCurCity
        {
            get
            {
                return ConfigurationManager.AppSettings["CurCity"].ToString();
            }
        }


    }
}
