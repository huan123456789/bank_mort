using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Common;
using System.Web.Script.Serialization;
using System.Data;
using System.Reflection;

namespace Business.common
{
    public static class JsonProvider
    {
        #region Json回调拼接
        ///// <summary>
        ///// 接口回调Json拼接方法
        ///// </summary>
        ///// <param name="state">调用状态0:失败 1:成功</param>
        ///// <param name="jsonData">返回数据</param>
        ///// <param name="otherValue">其他信息</param>
        ///// <returns></returns>
        //public static string JsonSplitJoint(int state, string jsonData, string otherValue)
        //{
        //    string result = "{";
        //    result += "\"stateCode\":" + "\"" + state + "\"";
        //    if (!string.IsNullOrEmpty(jsonData))
        //        result += "," + "\"data\":" + jsonData;

        //    result += "," + "\"Message\":\"" + otherValue + "\"";
        //    result += "}";
        //    return result;
        //}

        /// <summary>
        /// 接口回调Json拼接方法
        /// </summary>
        /// <param name="state">调用状态0:失败 1:成功</param>
        /// <param name="jsonData">返回数据</param>
        /// <param name="otherValue">其他信息</param>
        /// <returns></returns>
        public static void JsonSplitJoint(JsonStr model, string zidingyiJosn = null)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = jss.Serialize(model);
            if (!string.IsNullOrEmpty(zidingyiJosn))
            {
                json = json.TrimEnd('}');
                json += zidingyiJosn + "}";
            }
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*"); //允许所有域名访问
            HttpContext.Current.Response.Write(json);
        }
        public static void AdminJsonSplitJoint(AdminJsonStr model)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = jss.Serialize(model);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*"); //允许所有域名访问
            HttpContext.Current.Response.Write(json);
            HttpContext.Current.Response.End();
        }
        /// <summary> 
        /// DataTable转为list 
        /// </summary> 
        /// <param name="dt">DataTable</param> 
        /// <returns>list数据</returns> 
        public static List<Dictionary<string, string>> ToList(DataTable dt)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            return list;
        }
        /// <summary>
        /// 序列化对象为Json字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="recursionLimit">序列化对象的深度，默认为100</param>
        /// <returns>Json字符串</returns>
        private static string SerializeToJson(this object obj, int recursionLimit = 100)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            serialize.RecursionLimit = recursionLimit;
            return serialize.Serialize(obj);
        }
        #endregion

        #region table 转 list
        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {  //You can log something here     
                        //throw;    
                    }
                }
            }

            return obj;
        }

        #endregion
    }
}
