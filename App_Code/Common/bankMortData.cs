using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.bankMort
{
    public class bankMortData
    {
        //, { field: 'contractstatu', title: '合同状态', width: 80, sort: true }
        //, { fixed: 'right', title: '操作', width: 160, align: 'center', toolbar: '#barDemo' }

        private string _id;//按揭主键
        /// <summary> 按揭主键 F_8057_01
        /// </summary>
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }


        private string _contractID;//合同id
        /// <summary> 合同id
        /// </summary>
        public string contractID
        {
            get { return _contractID; }
            set { _contractID = value; }
        }

        private string _contractNo;//合同号
        /// <summary> 合同号
        /// </summary>
        public string contractNo
        {
            get { return _contractNo; }
            set { _contractNo = value; }
        }
        private string _paytype;//付款方式
        /// <summary> 付款方式
        /// </summary>
        public string paytype
        {
            get { return _paytype; }
            set { _paytype = value; }
        }
        private string _clientname;//客户
        /// <summary> 客户
        /// </summary>
        public string clientname
        {
            get { return _clientname; }
            set { _clientname = value; }
        }
        private string _owenname;//业主名字
        /// <summary> 业主名字
        /// </summary>
        public string owenname
        {
            get { return _owenname; }
            set { _owenname = value; }
        }
        private string _address;//物业地址
        /// <summary> 物业地址
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _username;//经纪人
        /// <summary> 经纪人
        /// </summary>
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _usertel;//经纪人电话
        /// <summary> 经纪人电话
        /// </summary>
        public string usertel
        {
            get { return _usertel; }
            set { _usertel = value; }
        }
        private string _neiqin;//内勤人员
        /// <summary> 内勤人员
        /// </summary>
        public string neiqin
        {
            get { return _neiqin; }
            set { _neiqin = value; }
        }

        private string userphone;
        /// <summary>
        /// 信贷专员电话
        /// </summary>
        public string Userphone
        {
            get { return userphone; }
            set { userphone = value; }
        }

        private string xDname;
        /// <summary>
        /// 信贷专员名字
        /// </summary>
        public string XDname
        {
            get { return xDname; }
            set { xDname = value; }
        }
        private string sccetptype;
        /// <summary>
        /// 接收状态
        /// </summary>
        public string Sccetptype
        {
            get { return sccetptype; }
            set { sccetptype = value; }
        }
        private string _receivename;//接收人
        /// <summary> 接收人
        /// </summary>
        public string receivename
        {
            get { return _receivename; }
            set { _receivename = value; }
        }
        private string _receivetime;//接收时间
        /// <summary> 接收时间
        /// </summary>
        public string receivetime
        {
            get { return _receivetime; }
            set { _receivetime = value; }
        }
        private string _contractstatu;//合同状态
        /// <summary> 合同状态
        /// </summary>
        public string contractstatu
        {
            get { return _contractstatu; }
            set { _contractstatu = value; }
        }
        private string _clientInfoLack;//缺少的客户资料
        /// <summary> 缺少的客户资料
        /// </summary>
        public string clientInfoLack
        {
            get { return _clientInfoLack; }
            set { _clientInfoLack = value; }
        }
        private string _clientInfoReceive;//接收到的客户资料
        /// <summary> 接收到的客户资料
        /// </summary>
        public string clientInfoReceive
        {
            get { return _clientInfoReceive; }
            set { _clientInfoReceive = value; }
        }
        private string _owenInfoLack;//缺少的卖方资料
        /// <summary> 缺少的卖方资料
        /// </summary>
        public string owenInfoLack
        {
            get { return _owenInfoLack; }
            set { _owenInfoLack = value; }
        }
        private string _owenInfoReceive;//接收到的卖方资料
        /// <summary> 接收到的卖方资料
        /// </summary>
        public string owenInfoReceive
        {
            get { return _owenInfoReceive; }
            set { _owenInfoReceive = value; }
        }
    }

    public class keyValuePair
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    /// <summary> 支付方式
    /// </summary>
    public class payWay
    {
        public string payWayID { get; set; }
        public string payWayName { get; set; }
        /// <summary> 根据枚举添加支付方式内容
        /// </summary>
        /// <returns></returns>
        public List<payWay> GetListPayWays()
        {
            List<payWay> list = new List<payWay>();
            foreach (empayType item in Enum.GetValues(typeof(empayType)))
            {
                payWay way = new payWay();
                way.payWayID = item.GetHashCode().ToString();
                way.payWayName = item.ToString();
                list.Add(way);
            }
            return list;
        }
    } 
    /// <summary> 合同进度
    /// </summary>
    public class contractType
    {
        public string contractID { get; set; }
        public string contractName { get; set; }
        /// <summary> 根据枚举添加合同进度
        /// </summary>
        /// <returns></returns>
        public List<contractType> GetListcontracts()
        {
            List<contractType> list = new List<contractType>();
            foreach (emContractSpan item in Enum.GetValues(typeof(emContractSpan)))
            {
                contractType way = new contractType();
                way.contractID = item.GetHashCode().ToString();
                way.contractName = item.ToString();
                list.Add(way);
            }
            return list;
        }
    }
    /// <summary>
    /// 合同状态
    /// </summary>
    public enum DatacontarcStateEnum
    {
        未移交 = 1,
        已移交 = 2,
        已退件 = 3
    }

  
    /// <summary>
    /// 付款方式
    /// </summary>
    public enum empayType
    {
        一次性付款 = 1,
        分期付款 = 2,
        商业贷款 = 3,
        纯市公贷款 = 4,
        市公组合贷款 = 5,
        纯省公贷款 = 6,
        省公组合贷款 = 7,
        纯市公转商贷款 = 8,
        市组合公转商贷款 = 9,
        纯省公转商贷款 = 10,
        省组合公转商贷款 = 11,
        纯铁路公积金贷款 = 12,
        铁路公积金组合贷款 = 13,
        部队公积金 = 14
    }


    /// <summary> 合同进度
    /// </summary>
    public enum emContractSpan
    {
        未接收 = 1,
        已接收 = 2,
        按揭办理 = 3,
        资料收齐 = 4,
        审批通过 = 5,
        借款合同已移交 = 6,
        已退件 = 7,
        流程已结束 = 8
    }


    /// <summary> 按揭资料接收记录表
    /// </summary>
    public class receiveData
    {
        //T_9100字段
        public string id { get; set; }
        public string cid { get; set; }
        public string receiveBuyData { get; set; }
        public string receiveSellData { get; set; }
        public bool DataIsReceiveOK { get; set; }
        public string remark { get; set; }
        public string arriveTime { get; set; }
        public string operatorName { get; set; }
        public string inputTime { get; set; }

        //bankMortDataDetail业务字段
        public string receiveBuyHaveData { get; set; }
        public string receiveSellHaveData { get; set; }
        public string receiveBuyNoHaveData { get; set; }
        public string receiveSellNoHaveData { get; set; }
    }

    /// <summary> 买方按揭资料
    /// </summary>
    public enum ReceiveBuyDataEnum
    {
        转让合同 = 1,
        资金监管协议 = 2,
        经纪服务合同 = 3,
        身份证 = 4,
        户口本 = 5,

        户口本首页 = 6,
        结婚证 = 7,
        离婚证明 = 8,
        离婚协议 = 9,
        单身具结书 = 10,

        夫妻关系具结书 = 11,
        资产证明 = 12,
        首付 = 13,
        收入证明 = 14,
        流水 = 15,

        大杭州查档 = 16,
        户口所在地查档 = 17,
        公积金缴交所在地查档 = 18,
        公积金缴存明细 = 19,
        公积金缴存证明 = 20,

        公积金代缴证明 = 21,
        公积金补缴情况说明 = 22,
        存贷证明 = 23,
        不缴公积金证明 = 24,
        原借款合同 = 25,

        原贷款结清证明 = 26,
        原三证复印件 = 27,
        非恶意拖欠证明及情况说明 = 28,
        社保 = 29,
        劳动合同 = 30,

        兼职收入 = 31,
        兼职劳动合同 = 32,
        房款 = 33,
        还款卡号 = 34,
        抵押物状况表 = 35,

        实勘报告 = 36,
        委托公证书 = 37,
        受托人身份证 = 38,
        小孩出生证明 = 39,
        抵押评估报告 = 40
    }

    /// <summary> 卖方按揭资料
    /// </summary>
    public enum ReceiveSellDataEnum
    {
        配偶同意出售承诺书 = 1,
        卖方未出租承诺书 = 2,
        门牌证 = 3,
        房屋照片 = 4,
        营业执照 = 5,

        税务登记证明 = 6,
        公司章程 = 7,
        组织机构代码 = 8,
        租赁合同 = 9,
        承租人身份证 = 10,

        放弃优先购买承诺书 = 11,
        房东账号 = 12,
        抵押注销=13,
        年代证明 = 14,
        身份证 = 15,

        户口本 = 16,
        户口本首页 = 17,
        结婚证 = 18,
        离婚证明= 19,
        离婚协议 = 20,

        单身具结书 = 21,
        夫妻关系具结书 = 22,
        产权证复印件 = 23,
        契证复印件 = 24,
        土地证复印件 = 25,

        委托公证书 = 26,
        受托人身份证 = 27
    }




    /// <summary> 借款人/中介提供资料清单
    /// </summary>
    public class Items
    {
        /// <summary> 资料类型
        /// </summary>
        public string type = "";

        /// <summary> 资料名称
        /// </summary>
        public string typeName = "";

        /// <summary> 原件份数
        /// </summary>
        public string oriCopyNum = "";

        /// <summary> 原件审核份数
        /// </summary>
        public string oriCopyChkNum = "";

        /// <summary> 复印件份数
        /// </summary>
        public string copiesNum = "";

        /// <summary> 复印件审核份数
        /// </summary>
        public string copiesChkNum = "";

        /// <summary> 审核结果
        /// </summary>
        public string chkResult = "";

        /// <summary> 结果描述
        /// </summary>
        public string desc = "";
    }
}
