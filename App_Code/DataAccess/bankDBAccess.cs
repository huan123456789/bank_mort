using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Common;
using Common.bankMort;
using Mana.DBProxy;
using System.Data;

namespace Business.bankMort
{
    public class bankDBAccess
    {
        /// <summary>
        /// 获取银行支行登陆信息
        /// </summary>
        /// <param name="subbranchid"></param>
        /// <returns></returns>
        public UserPrincipal getLoginInfoByBankName(string bankname, string username)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect("F_0120_01,F_0119_02,F_0120_03,F_0120_11,F_0120_10");
            sa.addCondition("F_0119_02", bankname, CompareType.Equal, LogicType.none, "");
            sa.addCondition("F_0120_11", "%," + username + ",%", CompareType.Like, LogicType.And, "");
            DataTable bankDT = sa.select("T_0120 inner join T_0119 on F_0119_01=F_0120_02");
            if (bankDT.Rows.Count > 0)
            {
                DataRow bankDR = bankDT.Rows[0];
                UserPrincipal bankinfo = new UserPrincipal();
                bankinfo.SubbranchID = bankDR["F_0120_01"].ToString();
                bankinfo.SubbranchName = bankDR["F_0119_02"].ToString() + bankDR["F_0120_03"].ToString();
                //bankinfo.UserName = bankDR["F_0120_11"].ToString();
                bankinfo.UserName = username;
                bankinfo.Password = bankDR["F_0120_10"].ToString();
                return bankinfo;
            }
            return null;
        }
        public List<keyValuePair> getbanklist()
        {
            //select * from T_0119 where F_0119_03=2 order by F_0119_04 
            List<keyValuePair> listKv = new List<keyValuePair>();
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect("F_0119_01,F_0119_02");
            sa.addCondition("F_0119_03", 2, CompareType.Equal, LogicType.none, "");
            sa.addOrder("F_0119_04", true);
            DataTable bankDT = sa.select("T_0119");
            if (bankDT != null && bankDT.Rows.Count > 0)
            {
                foreach (DataRow dr in bankDT.Rows)
                {
                    keyValuePair kv = new keyValuePair();
                    kv.key = dr["F_0119_01"].ToString();
                    kv.value = dr["F_0119_02"].ToString();
                    if (!listKv.Contains(kv))
                        listKv.Add(kv);
                }
            }
            return listKv;
        }

        /// <summary>
        /// 获取银行支行登陆信息
        /// </summary>
        /// <param name="subbranchid"></param>
        /// <returns></returns>
        public UserPrincipal getSubbranchLoginInfo(string subbranchid)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect("F_0119_02,F_0120_03,F_0120_11,F_0120_10");
            sa.addCondition("F_0120_01", subbranchid.Split('|')[0], CompareType.Equal, LogicType.none, "");
            DataTable bankDT = sa.select("T_0120 inner join T_0119 on F_0119_01=F_0120_02");
            if (bankDT.Rows.Count > 0)
            {
                DataRow bankDR = bankDT.Rows[0];
                UserPrincipal bankinfo = new UserPrincipal();
                bankinfo.SubbranchID = subbranchid;
                bankinfo.SubbranchName = bankDR["F_0119_02"].ToString() + bankDR["F_0120_03"].ToString();
                //bankinfo.UserName = bankDR["F_0120_11"].ToString();
                bankinfo.UserName = subbranchid.Split('|')[1];
                bankinfo.Password = bankDR["F_0120_10"].ToString();
                return bankinfo;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractNo">合约编号</param>
        /// <param name="stimeR">合同签署日期</param>
        /// <param name="etimeR">合同签署日期</param>
        /// <param name="userName">经纪人姓名</param>
        /// <param name="clientName">借款人姓名</param>
        /// <param name="owenName">卖家姓名</param>
        /// <param name="neiqin">内勤</param>
        /// <param name="receiveName">银行接收人</param>
        /// <param name="selecttype">合同状态</param>
        /// <param name="paytype">付款方式</param>
        /// <param name="less">是否缺失资料</param>
        /// <param name="loginName"></param>
        /// <param name="attr">房屋地址</param>
        /// <param name="subbranchID">支行ID</param>
        /// <param name="contracttypes">贷款进度</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableTotalCount"></param>
        /// <returns></returns>
        public List<bankMortData> getbankMortList(string contractNo, string stimeR, string etimeR, string userName,
            string clientName, string owenName, string neiqin, string receiveName,
            string selecttype, string paytype, bool less, string loginName, string attr, string subbranchID, string contracttypes, int pageIndex, int pageSize, ref int tableTotalCount)
        {
            List<bankMortData> list_bankMort = new List<bankMortData>();
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect("F_8057_01,F_8057_02,F_8057_03,F_8057_13,F_8057_36,F_8057_44,F_8057_60,F_8057_61,F_8057_63");
            sa.addSelect("F_8057_68,F_8057_78,F_8057_71,F_8057_79,F_8057_80,F_8057_81,F_8057_85,F_8057_88,F_8057_94,F_8057_93,F_8057_87");
            sa.addCondition("F_8057_73", subbranchID.Split('|')[0], CompareType.Equal, LogicType.none, "");
            sa.addCondition("F_8057_78", ElectroniContracType.其他.GetHashCode(), CompareType.Equal, LogicType.And, "");

            #region 查询条件
            if (!string.IsNullOrEmpty(contractNo))
                sa.addCondition("F_8057_03", contractNo, CompareType.Equal, LogicType.And, "");
            if (!string.IsNullOrEmpty(stimeR))
                sa.addCondition("F_8057_12", stimeR.Replace("-", ""), CompareType.BigAndEqual, LogicType.And, "");//DateTime.Parse(stimeR).ToString("yyyyMMdd")
            if (!string.IsNullOrEmpty(etimeR))
                sa.addCondition("F_8057_12", etimeR.Replace("-", ""), CompareType.SmallAndEqual, LogicType.And, "");//DateTime.Parse(etimeR).AddDays(1).ToString("yyyyMMdd")
            if (!string.IsNullOrEmpty(userName))
                sa.addCondition("F_8057_60", "%" + userName + "%", CompareType.Like, LogicType.And, "");
            if (!string.IsNullOrEmpty(clientName))
                sa.addCondition("F_8057_13", "%" + clientName + "%", CompareType.Like, LogicType.And, "");
            if (!string.IsNullOrEmpty(owenName))
                sa.addCondition("F_8057_44", "%" + owenName + "%", CompareType.Like, LogicType.And, "");
            if (!string.IsNullOrEmpty(attr))
                sa.addCondition("F_8057_36", "%" + attr + "%", CompareType.Like, LogicType.And, "");
            if (!string.IsNullOrEmpty(neiqin))
                sa.addCondition("F_8057_93", "%" + neiqin + "%", CompareType.Like, LogicType.And, "");
            if (!string.IsNullOrEmpty(receiveName))
                sa.addCondition("F_8057_87", "%" + receiveName + "%", CompareType.Like, LogicType.And, "");
            if (!string.IsNullOrEmpty(selecttype))
                sa.addCondition("F_8057_88", selecttype, CompareType.Equal, LogicType.And, "");
            if (string.IsNullOrEmpty(selecttype))
                sa.addCondition("F_8057_88", DatacontarcStateEnum.已退件, CompareType.NotEqual, LogicType.And, "");
            if (!string.IsNullOrEmpty(paytype))
                sa.addCondition("F_8057_79", paytype, CompareType.Equal, LogicType.And, "");
            if (!string.IsNullOrEmpty(contracttypes))
            {
                sa.addCondition("F_8057_71", contracttypes, CompareType.Equal, LogicType.And, "");
            }
            if (string.IsNullOrEmpty(contracttypes))
            {
                sa.addCondition("F_8057_71", emContractSpan.流程已结束, CompareType.NotEqual, LogicType.And, "");
            }
            if (less)
                sa.whereAppend = "(F_8057_63 is not null or F_8057_81 is not null)";


            #endregion 查询条件


            sa.addOrder("F_8057_03", false);
            sa.pageNow = pageIndex - 1;
            sa.pageSize = pageSize;
            DataTable dt = sa.selectPage("T_8057");
            tableTotalCount = sa.recordCount;
            sa.whereAppend = "";
            if (dt == null || dt.Rows.Count == 0)
                return list_bankMort;
            foreach (DataRow dr in dt.Rows)
            {
                bankMortData b = new bankMortData();
                b.id = dr["F_8057_01"].ToString();
                b.contractID = dr["F_8057_02"].ToString();
                b.contractNo = dr["F_8057_03"].ToString();
                b.clientname = dr["F_8057_13"].ToString();
                b.address = dr["F_8057_36"].ToString();
                b.owenname = dr["F_8057_44"].ToString();
                b.username = dr["F_8057_60"].ToString();
                b.usertel = dr["F_8057_61"].ToString();
                b.owenInfoLack = dr["F_8057_68"].ToString();
                b.clientInfoReceive = dr["F_8057_80"].ToString();
                b.clientInfoLack = dr["F_8057_63"].ToString(); //工商
                b.Userphone = dr["F_8057_94"].ToString();//信贷专员电话
                b.XDname = dr["F_8057_93"].ToString();//信贷专员名字
                b.receivename = dr["F_8057_87"].ToString();
                emContractSpan SccetptypeS = (emContractSpan)System.Enum.Parse(typeof(emContractSpan), Convert.ToInt32(dr["F_8057_71"]).ToString());
                b.Sccetptype = SccetptypeS.ToString();//资料接收状态
                b.receivetime = dr["F_8057_85"].ToString();//接收时间
                DatacontarcStateEnum displayType = (DatacontarcStateEnum)System.Enum.Parse(typeof(DatacontarcStateEnum), Convert.ToInt32(dr["F_8057_88"]).ToString());
                b.contractstatu = displayType.ToString(); //合同状态
                if (dr["F_8057_79"].ToString() != "")
                {
                    empayType payType = (empayType)System.Enum.Parse(typeof(empayType), Convert.ToInt32(dr["F_8057_79"]).ToString());
                    b.paytype = payType.ToString(); //付款方式
                }

                b.clientInfoLack = dr["F_8057_81"].ToString();
                //if (Convert.ToInt32(dr["F_8057_78"].ToString()) != 2)
                //{
                //    b.clientInfoLack = dr["F_8057_81"].ToString();
                //}
                //else
                //{
                //    if (!string.IsNullOrEmpty(dr["F_8057_81"].ToString()))
                //    {
                //        JavaScriptSerializer js = new JavaScriptSerializer();
                //        List<Items> browerItemsList = js.Deserialize<List<Items>>(dr["F_8057_81"].ToString());
                //        string borrowNoHaveFiles = "";
                //        if (browerItemsList != null && browerItemsList.Count > 0)
                //        {
                //            foreach (Items items in browerItemsList)
                //            {
                //                borrowNoHaveFiles += items.typeName + "|"; //借款人缺失资料
                //            }
                //            b.clientInfoLack += string.Format("借款人缺失资料:{0}", borrowNoHaveFiles.TrimEnd('|')); //杭州
                //        }
                //    }
                //}
                b.paytype = string.IsNullOrEmpty(dr["F_8057_79"].ToString())
                    ? ""
                    : ((empayType)Convert.ToInt32(dr["F_8057_79"].ToString())).ToString();
                list_bankMort.Add(b);
            }

            return list_bankMort;
        }


        /// <summary>银行按揭信息详情收到按揭资料分页查询
        /// </summary>
        /// <param name="isHaveData"></param>
        /// <returns></returns>
        public List<receiveData> GetReceiveDataList(string contractid, bool isReceive, int pageIndex, int pageSize, ref int tableTotalCount)
        {

            List<receiveData> list_receiveData = new List<receiveData>();
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect(
                "F_8100_01,F_8100_02,F_8100_03,F_8100_04,F_8100_05,F_8100_06,F_8100_07,F_8100_08,F_8100_09,F_992");
            sa.addCondition("F_8100_02", contractid, CompareType.Equal, LogicType.none, "");
            if (isReceive)
            {
                sa.addCondition("F_8100_05", 1, CompareType.Equal, LogicType.And, "");//已收到
            }
            else
            {
                sa.addCondition("F_8100_05", 0, CompareType.Equal, LogicType.And, "");//缺失
            }
            sa.addOrder("F_992", false);
            sa.pageNow = pageIndex - 1;
            sa.pageSize = pageSize;
            DataTable dt = sa.selectPage("T_8100");
            tableTotalCount = sa.recordCount;
            sa.whereAppend = "";
            if (dt == null || dt.Rows.Count == 0)
                return list_receiveData;


            foreach (DataRow dr in dt.Rows)
            {
                receiveData b = new receiveData();
                b.id = dr["F_8100_01"].ToString();
                b.cid = dr["F_8100_02"].ToString();
                b.receiveBuyData = dr["F_8100_03"].ToString();
                b.receiveSellData = dr["F_8100_04"].ToString();
                b.DataIsReceiveOK = bool.Parse(dr["F_8100_05"].ToString());
                b.remark = dr["F_8100_07"].ToString();
                b.arriveTime = dr["F_8100_08"].ToString();
                b.operatorName = dr["F_8100_09"].ToString();
                b.inputTime = dr["F_992"].ToString();
                getData(b);
                if (!string.IsNullOrEmpty(b.receiveBuyHaveData))
                    b.receiveBuyHaveData = b.receiveBuyHaveData.TrimEnd(',');
                if (!string.IsNullOrEmpty(b.receiveSellHaveData))
                    b.receiveSellHaveData = b.receiveSellHaveData.TrimEnd(',');
                if (!string.IsNullOrEmpty(b.receiveBuyNoHaveData))
                    b.receiveBuyNoHaveData = b.receiveBuyNoHaveData.TrimEnd(',');
                if (!string.IsNullOrEmpty(b.receiveSellNoHaveData))
                    b.receiveSellNoHaveData = b.receiveSellNoHaveData.TrimEnd(',');
                list_receiveData.Add(b);
            }
            return list_receiveData;
        }


        /// <summary> 获取全数据  资料拼接展示
        /// </summary>
        public void getFullData(string contractid, ref string ReceiveBuyHaveDatas, ref string ReceiveSellHaveDatas, ref string ReceiveBuyNoHaveDatas,
            ref string ReceiveSellNoHaveDatas)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect(
                "F_8100_01,F_8100_02,F_8100_03,F_8100_04,F_8100_05,F_8100_06,F_8100_07,F_8100_08,F_8100_09,F_992");
            sa.addCondition("F_8100_02", contractid, CompareType.Equal, LogicType.none, "");
            sa.addOrder("F_992", false);
            DataTable dt = sa.select("T_8100");
            List<receiveData> list_receiveData = new List<receiveData>();

            foreach (DataRow dr in dt.Rows)
            {
                receiveData b = new receiveData();
                b.id = dr["F_8100_01"].ToString();
                b.cid = dr["F_8100_02"].ToString();
                b.receiveBuyData = dr["F_8100_03"].ToString();
                b.receiveSellData = dr["F_8100_04"].ToString();
                b.DataIsReceiveOK = bool.Parse(dr["F_8100_05"].ToString());
                b.remark = dr["F_8100_07"].ToString();
                b.arriveTime = dr["F_8100_08"].ToString();
                b.operatorName = dr["F_8100_09"].ToString();
                b.inputTime = dr["F_992"].ToString();
                getData(b);
                list_receiveData.Add(b);
            }
            foreach (receiveData item in list_receiveData)
            {
                ReceiveBuyHaveDatas += string.IsNullOrEmpty(item.receiveBuyHaveData) ? "" : item.receiveBuyHaveData + ",";
                ReceiveSellHaveDatas += string.IsNullOrEmpty(item.receiveSellHaveData) ? "" : item.receiveSellHaveData + ",";
                ReceiveBuyNoHaveDatas += string.IsNullOrEmpty(item.receiveBuyNoHaveData) ? "" : item.receiveBuyNoHaveData + ",";
                ReceiveSellNoHaveDatas += string.IsNullOrEmpty(item.receiveSellNoHaveData) ? "" : item.receiveSellNoHaveData + ",";
            }
            if (!string.IsNullOrEmpty(ReceiveBuyHaveDatas))
                ReceiveBuyHaveDatas = ReceiveBuyHaveDatas.TrimEnd(',');
            if (!string.IsNullOrEmpty(ReceiveSellHaveDatas))
                ReceiveSellHaveDatas = ReceiveSellHaveDatas.TrimEnd(',');
            if (!string.IsNullOrEmpty(ReceiveBuyNoHaveDatas))
                ReceiveBuyNoHaveDatas = ReceiveBuyNoHaveDatas.TrimEnd(',');
            if (!string.IsNullOrEmpty(ReceiveSellNoHaveDatas))
                ReceiveSellNoHaveDatas = ReceiveSellNoHaveDatas.TrimEnd(',');
        }


        /// <summary> 删除收到/缺少资料
        /// </summary>
        /// <param name="delids"></param>
        /// <returns></returns>
        public string deldata(string delids)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addDBCondition("F_8100_01", delids, CompareType.In, "");
            return sa.delete("T_8100");
        }


        /// <summary> 获取收到/缺少资料  分开枚举b 和 其他资料文本(ref)
        /// </summary>
        /// <returns></returns>
        public receiveData getData(receiveData b)
        {
            //已收到
            if (b.DataIsReceiveOK)
            {
                if (!string.IsNullOrEmpty(b.receiveBuyData))
                {
                    //多个资料存在判断
                    if (b.receiveBuyData.Contains(":"))
                    {
                        b.receiveBuyData = b.receiveBuyData.Replace("买方其他资料:", "").Replace("卖方其他资料:", "");
                    }
                    string[] datas = b.receiveBuyData.Split(',');
                    for (int i = 0; i < datas.Length; i++)
                    {
                        b.receiveBuyHaveData += datas[i] + ",";
                    }
                    b.receiveBuyHaveData = !string.IsNullOrEmpty(b.receiveBuyHaveData)
                        ? b.receiveBuyHaveData.TrimEnd(',')
                        : b.receiveBuyHaveData;
                }
                if (!string.IsNullOrEmpty(b.receiveSellData))
                {
                    //多个资料存在判断
                    if (b.receiveSellData.Contains(":"))
                    {
                        b.receiveSellData = b.receiveSellData.Replace("买方其他资料:", "").Replace("卖方其他资料:", "");
                    }
                    string[] datas = b.receiveSellData.Split(',');
                    for (int i = 0; i < datas.Length; i++)
                    {
                        b.receiveSellHaveData += datas[i] + ",";
                    }
                    b.receiveSellHaveData = !string.IsNullOrEmpty(b.receiveSellHaveData)
                        ? b.receiveSellHaveData.TrimEnd(',')
                        : b.receiveSellHaveData;
                }
            }
            else
            {
                //缺失
                if (!string.IsNullOrEmpty(b.receiveBuyData))
                {
                    //多个资料存在判断
                    if (b.receiveBuyData.Contains(":"))
                    {
                        b.receiveBuyData = b.receiveBuyData.Replace("买方其他资料:", "").Replace("卖方其他资料:", "");
                    }
                    string[] datas = b.receiveBuyData.Split(',');
                    for (int i = 0; i < datas.Length; i++)
                    {
                        b.receiveBuyNoHaveData += datas[i] + ",";
                    }
                    b.receiveBuyNoHaveData = !string.IsNullOrEmpty(b.receiveBuyNoHaveData)
                        ? b.receiveBuyNoHaveData.TrimEnd(',')
                        : b.receiveBuyNoHaveData;
                }
                if (!string.IsNullOrEmpty(b.receiveSellData))
                {
                    //多个资料存在判断
                    if (b.receiveSellData.Contains(":"))
                    {
                        b.receiveSellData = b.receiveSellData.Replace("买方其他资料:", "").Replace("卖方其他资料:", "");
                    }
                    string[] datas = b.receiveSellData.Split(',');
                    for (int i = 0; i < datas.Length; i++)
                    {
                        b.receiveSellNoHaveData += datas[i] + ",";
                    }
                    b.receiveSellNoHaveData = !string.IsNullOrEmpty(b.receiveSellNoHaveData)
                        ? b.receiveSellNoHaveData.TrimEnd(',')
                        : b.receiveSellNoHaveData;
                }
            }


            return b;
        }
        /// <summary>
        /// 按揭资料数据添加
        /// </summary>
        /// <param name="buycode">买方资料</param>
        /// <param name="sellcode">卖方资料</param>
        /// <param name="contrsctid">合同ID</param>
        /// <param name="codetype">缺失/收到</param>
        public void insertcode(string buycode, string sellcode, string contrsctid, string codetype)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            Guid id = Guid.NewGuid();
            sa.addParameterString("F_8100_01", id.ToString(), 50, false, "");
            sa.addParameterString("F_8100_02", contrsctid, 50, false, "");
            sa.addParameterString("F_8100_03", buycode, 2000, false, "");
            sa.addParameterString("F_8100_04", sellcode, 2000, false, "");
            if (codetype != "")
            {
                sa.addParameterDB("F_8100_05", codetype, "");
            }
            sa.addParameterString("F_8100_08", DateTime.Now.ToString(), 100, false, "");
            sa.addParameterString("F_992", DateTime.Now.ToString(), 100, false, "");
            sa.addParameterString("F_993", "0", 10, false, "");
            sa.insert("T_8100");
        }
        /// <summary>
        /// 按揭资料修改
        /// </summary>
        /// <param name="bankdataid">按揭资料ID</param>
        /// <param name="buycount">买方资料</param>
        /// <param name="sellcount">卖方资料</param>
        /// <param name="contrsctid">合同ID</param>
        /// <param name="codetype">缺失/收到</param>
        public void updatecode(string bankdataid, string buycount, string sellcount)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addParameterString("F_8100_03", buycount, 2000, false, "");
            sa.addParameterString("F_8100_04", sellcount, 2000, false, "");
            sa.addParameterString("F_998", DateTime.Now.ToString(), 100, false, "");
            sa.addCondition("F_8100_01", bankdataid, CompareType.Equal, LogicType.none, "");
            sa.update("T_8100");
        }
        /// <summary>
        /// 根据按揭资料ID得到相应数据
        /// </summary>
        /// <param name="bankcodeid">按揭资料ID</param>
        /// <returns></returns>
        public DataRow selectbankcode(string bankcodeid)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect("F_8100_01,F_8100_02,F_8100_03,F_8100_04,F_8100_05,F_8100_07,F_8100_08,F_8100_09,F_991,F_992,F_993");
            sa.addCondition("F_8100_01", bankcodeid, CompareType.Equal, LogicType.none, "");
            sa.addCondition("F_993", 0, CompareType.Equal, LogicType.And, "");
            DataTable dt = sa.select("T_8100");
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            return null;
        }

        /// <summary> 修改登陆密码
        /// </summary>
        /// <param name="subbranchID">支行id</param>
        /// <param name="newpwd">密码</param>
        public string updatePwd(string subbranchID, string newpwd, string oldpwd)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addCondition("F_0120_01", subbranchID, CompareType.Equal, LogicType.none, "");
            sa.addCondition("F_0120_10", oldpwd, CompareType.Equal, LogicType.And, "");
            if (!sa.selectOne("T_0120"))
            {
                return "输入密码错误!";
            }
            sa.addParameterString("F_0120_10", newpwd, 200, false, "");
            sa.update("T_0120");
            return "";
        }


        /// <summary>
        /// 合同ID
        /// </summary>
        /// <param name="contactid">合同ID</param>
        /// <returns></returns>
        public DataRow selbankcode(string contactid)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect("F_8057_02");
            sa.addCondition("F_8057_02", contactid, CompareType.Equal, LogicType.none, "");
            DataTable dt = sa.select("T_8057");
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            return null;
        }

        /// <summary>
        /// 添加按揭数据到 T_8057
        /// </summary>
        /// <param name="contactid">合同ID</param>
        /// <param name="inbuycode">收到的买方资料</param>
        /// <param name="insellcode">收到的卖方资料</param>
        /// <param name="upbuycode">缺失的买方资料</param>
        /// <param name="upsellcode">缺失的卖方资料</param>
        public void insertbankcode(string contactid, string inbuycode, string insellcode, string upbuycode, string upsellcode)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addParameterString("F_8057_80", inbuycode, 8000, false, "");
            sa.addParameterString("F_8057_90", insellcode, 8000, false, "");
            sa.addParameterString("F_8057_81", upbuycode, 8000, false, "");
            sa.addParameterString("F_8057_91", upsellcode, 8000, false, "");
            sa.addCondition("F_8057_02", contactid, CompareType.Equal, LogicType.none, "");
            sa.insert("T_8057");

        }
        /// <summary>
        /// 添加按揭数据到 T_8057
        /// </summary>
        /// <param name="contactid">合同ID</param>
        /// <param name="inbuycode">收到的买方资料</param>
        /// <param name="insellcode">收到的卖方资料</param>
        /// <param name="upbuycode">缺失的买方资料</param>
        /// <param name="upsellcode">缺失的卖方资料</param>
        public void updatebankdata(string contactid, string inbuycode, string insellcode, string upbuycode, string upsellcode)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addParameterString("F_8057_80", inbuycode, 8000, false, "");
            sa.addParameterString("F_8057_90", insellcode, 8000, false, "");
            sa.addParameterString("F_8057_81", upbuycode, 8000, false, "");
            sa.addParameterString("F_8057_91", upsellcode, 8000, false, "");
            sa.addCondition("F_8057_02", contactid, CompareType.Equal, LogicType.none, "");
            sa.addCondition("F_8057_78", ElectroniContracType.其他.GetHashCode(), CompareType.Equal, LogicType.And, "");
            sa.update("T_8057");

        }

        public DataRow selectcontractinfo(string contractid)
        {
            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect("F_8057_03,F_8057_13,F_8057_44,F_8057_60,F_8057_86,F_8057_40,F_8057_36,F_8057_09,F_8057_10,F_8057_11");
            sa.addSelect("F_8057_74,F_8057_84,F_8057_85,F_8057_95,F_8057_96,F_8057_97,F_8057_98,F_8057_99");
            sa.addCondition("F_8057_02", contractid, CompareType.Equal, LogicType.none, "");
            sa.addCondition("F_8057_78", ElectroniContracType.其他.GetHashCode(), CompareType.Equal, LogicType.And, "");
            DataTable dt = sa.select("T_8057");
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            return null;
        }




        /// <summary> 修改合同进度
        /// </summary>
        /// <param name="contactid"></param>
        /// <param name="plan"></param>
        public string updatePlan(string contactid, string plan, string username)
        {
            try
            {
                SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
                sa.addParameterNumber("F_8057_71", int.Parse(plan), "");
                if (int.Parse(plan) == emContractSpan.已接收.GetHashCode())
                {
                    sa.addParameterString("F_8057_87", username, 100, false, "");
                    sa.addParameterString("F_8057_85", DateTime.Now.ToString(), 100, false, "");
                }
                if (int.Parse(plan) == emContractSpan.按揭办理.GetHashCode())
                {
                    sa.addParameterString("F_8057_95", DateTime.Now.ToString(), 100, false, "");
                }
                if (int.Parse(plan) == emContractSpan.资料收齐.GetHashCode())
                {
                    sa.addParameterString("F_8057_96", DateTime.Now.ToString(), 100, false, "");
                }
                if (int.Parse(plan) == emContractSpan.审批通过.GetHashCode())
                {
                    sa.addParameterString("F_8057_97", DateTime.Now.ToString(), 100, false, "");
                }
                if (int.Parse(plan) == emContractSpan.借款合同已移交.GetHashCode())
                {
                    sa.addParameterString("F_8057_98", DateTime.Now.ToString(), 100, false, "");
                }
                if (int.Parse(plan) == emContractSpan.已退件.GetHashCode())
                {
                    sa.addParameterString("F_8057_99", DateTime.Now.ToString(), 100, false, "");
                }
                sa.addCondition("F_8057_02", contactid, CompareType.Equal, LogicType.none, "");
                sa.addCondition("F_8057_78", ElectroniContracType.其他.GetHashCode(), CompareType.Equal, LogicType.And, "");
                return sa.update("T_8057");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary> 获取资料是否齐全
        /// </summary>
        /// <param name="contactid"></param>
        /// <param name="plan"></param>
        public string getDataFull(string contactid)
        {

            SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
            sa.addSelect("F_8057_71");
            sa.addCondition("F_8057_02", contactid, CompareType.Equal, LogicType.none, "");
            sa.addCondition("F_8057_78", ElectroniContracType.其他.GetHashCode(), CompareType.Equal, LogicType.And, "");
            DataTable dt = sa.select("T_8057");
            if (dt == null || dt.Rows.Count < 1)
            {
                return "";
            }
            return dt.Rows[0]["F_8057_71"].ToString();
        }



        /// <summary> 更改流程内容
        /// </summary>
        /// <param name="cid">合同id</param>
        /// <param name="plan">进度</param>
        public string updateFlow(string cid, string plan)
        {
            try
            {
                SqlAgent sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
                sa.addParameterDB("F_3517_09", "getdate()", "");
                sa.addParameterString("F_3517_22", "系统自动完成", 100, false, "");
                sa.addParameterNumber("F_3517_18", 1, "");
                sa.addCondition("F_3517_03", cid, CompareType.Equal, LogicType.none, "");
                sa.addCondition("F_3517_18", 1, CompareType.NotEqual, LogicType.And, "");
                if (plan == emContractSpan.资料收齐.GetHashCode().ToString())
                {
                    sa.addCondition("F_3517_06", "备齐按揭资料", CompareType.Equal, LogicType.And, "");
                }
                if (plan == emContractSpan.审批通过.GetHashCode().ToString())
                {
                    //sa.addDBCondition("F_3517_06", "'按揭审批通过领取按揭合同','商业按揭审批通过'", CompareType.In, "");
                    //sa.addCondition("F_3517_06", "商业按揭审批通过", CompareType.Equal, LogicType.And, "");
                    sa.whereAppend = " (F_3517_06 = '商业按揭审批通过' or F_3517_06 = '按揭审批通过')";
                }
                sa.update("T_3517");


                sa.addSelect("F_3517_04");
                DataTable dt = sa.@select("T_3517");
                sa.whereAppend = "";
                if (dt == null || dt.Rows.Count < 1)
                {
                    return "";
                }
                sa = new SqlAgent(SysConfiguration.GetErpSqlConn());
                sa.addParameterDB("F_3517_08", "DATEADD(day,1,getdate())", "");
                sa.addCondition("F_3517_03", cid, CompareType.Equal, LogicType.none, "");
                sa.addCondition("F_3515_07", dt.Rows[0]["F_3517_04"].ToString(), CompareType.Equal, LogicType.And, "");
                return sa.update("T_3517");
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

    }
}
