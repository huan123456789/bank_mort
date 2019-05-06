using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public enum ElectroniContracType
    {
        工商银行 = 1,
        杭州银行 = 2,
        其他 = 3
    }
    public class JsonStr
    {
        private int _stateCode = 0;
        public int stateCode
        {
            get { return _stateCode; }
            set { _stateCode = value; }
        }

        private object _data = null;
        public object data
        {
            get { return _data; }
            set { _data = value; }
        }

        private int _count = 0;
        public int count
        {
            get { return _count; }
            set { _count = value; }
        }

        private string _errorMsg = "";
        public string errorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
    }

    public class AdminJsonStr
    {
        private int _code = 0;

        public int code
        {
            get { return _code; }
            set { _code = value; }
        }

        private int _count = 0;

        public int count
        {
            get { return _count; }
            set { _count = value; }
        }

        private object _data = null;

        public object data
        {
            get { return _data; }
            set { _data = value; }
        }


        private string _msg;

        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }


        //按揭列表详情特有字段
        public string ReceiveBuyHaveDatas { get; set; }//收到买方资料
        public string ReceiveSellHaveDatas { get; set; }//收到卖方资料
        public string ReceiveBuyNoHaveDatas { get; set; }//缺失买方资料
        public string ReceiveSellNoHaveDatas { get; set; }//缺失卖方资料
        public string Receivebankdatum { get; set; }

        public string dataPlanID { get; set; } //资料进度id emContractSpan
        public string dataPlanContent { get; set; }//资料进度内容 emContractSpan

    }
}
