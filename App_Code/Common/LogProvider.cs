using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    public class LogProvider
    {
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="infos"></param>
        public static void SaveLog(string infos)
        {
            try
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "log";
                //string strPath = "C:\\web\\" + "log";
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                System.IO.FileStream fs = new System.IO.FileStream(strPath + "\\log"
                    + DateTime.Now.ToString("yyyyMMdd") + ".txt", System.IO.FileMode.Append, System.IO.FileAccess.Write);
                byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + infos + "\r\n");
                fs.Write(buff, 0, buff.Length);
                fs.Flush();
                fs.Close();
            }
            catch { }
        }
    }
}
