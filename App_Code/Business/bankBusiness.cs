using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Common;
using System.Web;
using Common.bankMort;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;

namespace Business.bankMort
{
    public class bankBusiness : Page
    {
        string loginName = "";
        int pageIndex = 0;
        int pageSize = 0;
        string cid = "";
        AdminJsonStr model = new AdminJsonStr();
        public AdminJsonStr dealbank(HttpContext content)
        {
            string type = string.IsNullOrEmpty(content.Request["t"])
                ? "0"
                : content.Request["t"].ToString();

            switch (type)
            {
                case "1": //银行按揭信息列表查询
                    model = getbankList(content);
                    break;
                case "2": //银行按揭信息列表获取付款方式
                    model = getpayway();
                    break;
                case "3": //银行按揭信息列表获取合同进度
                    model = getcontractType();
                    break;
                case "10": //银行按揭信息详情收到按揭资料查询
                    model = getReceiveDataList(content, true);
                    break;
                case "11": //银行按揭信息详情缺少按揭资料查询
                    model = getReceiveDataList(content, false);
                    break;
                case "12": //删除已接受/缺失数据
                    model = delReceiveDataList(content);
                    break;
                case "13": //插入按揭资料
                    model = insertbankcode(content);
                    break;
                case "14": //修改合同进度
                    model = updatePlan(content);
                    break;
                case "15": //获取资料是否齐全
                    model = getDataFull(content);
                    break;
                case "16": //查询合同详情
                    model = selectcontractinfo(content);
                    break;
                case "20"://加入 / 修改  按揭数据
                    model = insertinformation(content);
                    break;
                case "21":
                    model = selectbankcode(content);
                    break;
                case "101": //银行列表
                    model = getbankList();
                    break;
                case "102": //银行登录信息
                    model = getloginUser();
                    break;
                case "103": //银行登录信息
                    model = removeCookie(content);
                    break;
                case "104": //修改银行登录密码
                    model = updatePwd(content);
                    break;
                //case "22":
                //    model = selectcodeinfo(content);
                //    break;

            }

            return model;
        }

        private AdminJsonStr removeCookie(HttpContext content)
        {
            AdminJsonStr model = new AdminJsonStr();
            string zid = content.Request["zid"];
            //HttpCookie hc = content.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
            //hc.Expires = DateTime.Today.AddDays(-1);
            //content.Response.Cookies.Add(hc);
            //content.Request.Cookies.Remove(System.Web.Security.FormsAuthentication.FormsCookieName);
            FormsAuthentication.SignOut();
            HttpContext.Current.Cache.Remove(zid);
            model.data = "已退出登录。";
            return model;
        }



        private AdminJsonStr getloginUser()
        {
            AdminJsonStr model = new AdminJsonStr();
            keyValuePair kv = new keyValuePair();
            if (UserPrincipal.current != null)
            {
                kv.key = UserPrincipal.current.SubbranchID;
                kv.value = UserPrincipal.current.UserName;
                model.data = kv;
                model.count = 1;
            }
            else
            {
                model.code = 1;
                model.msg = "你还没有登录，请重新登录。";
            }
            return model;
        }

        /// <summary> 插入按揭资料
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public AdminJsonStr insertbankcode(HttpContext content)
        {
            string contactid = content.Request["contact"];
            string inbuycode = content.Request["inbuycode"];
            string insellcode = content.Request["insellcode"];
            string upbuycode = content.Request["upbuycode"];
            string upsellcode = content.Request["upsellcode"];
            bankDBAccess bank = new bankDBAccess();
            //查询数据是否存在
            //DataRow dr_code = bank.selbankcode(contactid);
            //if (dr_code != null)
            //{
            bank.updatebankdata(contactid, inbuycode, insellcode, upbuycode, upsellcode);
            //}
            //else
            //{
            //   bank.insertbankcode(contactid, inbuycode, insellcode, upbuycode, upsellcode);
            //   model.msg = "";
            //}

            return model;
        }


        /// <summary> 修改合同进度
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public AdminJsonStr updatePlan(HttpContext content)
        {
            string cid = content.Request["cid"];
            string plan = content.Request["plan"];
            string username = content.Request["username"];
            bankDBAccess bank = new bankDBAccess();
            string msg = bank.updatePlan(cid, plan, username);
            if (plan == emContractSpan.资料收齐.GetHashCode().ToString() || plan == emContractSpan.审批通过.GetHashCode().ToString())
            {
                msg = bank.updateFlow(cid, plan);
            }
            if (!string.IsNullOrEmpty(msg))
            {
                model.code = 1;
                model.msg = msg;
            }


            return model;
        }

        /// <summary> 获取资料是否齐全
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public AdminJsonStr getDataFull(HttpContext content)
        {
            string cid = content.Request["cid"];
            bankDBAccess bank = new bankDBAccess();
            string id = bank.getDataFull(cid);
            if (string.IsNullOrEmpty(id))
            {
                model.code = 1;
                model.msg = "无该合同详情!";
                return model;
            }
            model.dataPlanID = id;
            model.dataPlanContent = ((emContractSpan)int.Parse(id)).ToString();
            return model;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="bankcodeid">按揭资料ID</param>
        /// <returns></returns>
        public AdminJsonStr selectbankcode(HttpContext content)
        {
            bankDBAccess bank = new bankDBAccess();
            string bankcodeid = content.Request["bankcodeid"];
            DataRow dr = bank.selectbankcode(bankcodeid);
            ArrangementsInfo arrangem = new ArrangementsInfo();
            if (dr != null)
            {
                arrangem.Buycode = dr["F_8100_03"].ToString();//买
                arrangem.Sellcode = dr["F_8100_04"].ToString();//卖
                model.data = arrangem;
            }
            return model;
        }
        /// <summary>
        /// 获取合同信息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public AdminJsonStr selectcontractinfo(HttpContext content)
        {
            bankDBAccess bank = new bankDBAccess();
            string contractid = content.Request["contactid"];
            contractinfo cont = new contractinfo();
            DataRow drbank = bank.selectcontractinfo(contractid);
            if (drbank != null)
            {
                cont.Contratctnum = drbank["F_8057_03"].ToString();//合同编号
                cont.Buyname = drbank["F_8057_13"].ToString(); //买方姓名
                cont.Sellname = drbank["F_8057_44"].ToString(); //卖方姓名
                cont.Agent = drbank["F_8057_60"].ToString(); //经纪人
                cont.Loadname = drbank["F_8057_86"].ToString();//信贷专员
                cont.Area = drbank["F_8057_40"].ToString();//面积
                cont.Address = drbank["F_8057_36"].ToString();//物业地址
                cont.Contractcountmoney = drbank["F_8057_09"].ToString();//合同总价
                cont.Frfistmoney = drbank["F_8057_10"].ToString() == "" ? "0" : ((decimal)drbank["F_8057_10"] / 10000).ToString();//首付金额
                cont.Loadmoney = drbank["F_8057_11"].ToString();//商业贷款金额
                cont.GJJLoadmoney = drbank["F_8057_84"].ToString();//公积金贷款金额
                cont.Bankname = drbank["F_8057_74"].ToString();//银行
                cont.AcceptTime = drbank["F_8057_85"].ToString() == "" ? "" : Convert.ToDateTime(drbank["F_8057_85"]).ToString("yyyy-MM-dd");//接受时间
                cont.MortgageTime = drbank["F_8057_95"].ToString() == "" ? "" : Convert.ToDateTime(drbank["F_8057_95"]).ToString("yyyy-MM-dd");//按揭办理时间
                cont.ColletData = drbank["F_8057_96"].ToString() == "" ? "" : Convert.ToDateTime(drbank["F_8057_96"]).ToString("yyyy-MM-dd");//资料已齐时间
                cont.DataPaass = drbank["F_8057_97"].ToString() == "" ? "" : Convert.ToDateTime(drbank["F_8057_97"]).ToString("yyyy-MM-dd");//审批通过时间
                cont.ContractTurn = drbank["F_8057_98"].ToString() == "" ? "" : Convert.ToDateTime(drbank["F_8057_98"]).ToString("yyyy-MM-dd");//合同移交时间
                cont.BounceTime = drbank["F_8057_99"].ToString() == "" ? "" : Convert.ToDateTime(drbank["F_8057_99"]).ToString("yyyy-MM-dd");//退件时间
                model.data = cont;
            }
            return model;
        }




        /// <summary>
        /// 加入按揭资料数据
        /// </summary>
        /// <param name="buycount">买方资料</param>
        /// <param name="sellcount">卖方资料</param>
        /// <param name="contrsctid">合同ID</param>
        /// <param name="codetype">收到/缺失</param>
        /// <param name="buytext">买方其他资料</param>
        /// <param name="selltext">卖方其他资料</param>
        /// <returns></returns>
        public AdminJsonStr insertinformation(HttpContext content)
        {
            AdminJsonStr model = new AdminJsonStr();
            string buycount = content.Request["buycount"]; //买方资料
            string sellcount = content.Request["sellcount"];//卖方资料
            string contrsctid = content.Request["contractid"];//合同ID
            string buytext = content.Request["buytext"];//买方其他资料
            string selltext = content.Request["selltext"];//卖方其他资料
            string bankdataid = content.Request["bankcodeid"];//按揭资料id
            string codetype = content.Request["typecode"];//缺失/收到
            if (buycount == "" && sellcount == "" && buytext == "" && selltext == "")
            {
                model.code = 1;
            }
            else
            {
                bankDBAccess bank = new bankDBAccess();
                //buycount = buycount.Trim(',');
                //sellcount = sellcount.Trim(',');
                if (buytext != "")
                {
                    buytext = buytext.Replace("，", ",");
                    buycount += "买方其他资料:" + buytext;
                }
                if (selltext != "")
                {
                    selltext = selltext.Replace("，", ",");
                    sellcount += "卖方其他资料:" + selltext;
                }
                if (bankdataid != "")//修改
                {
                    bank.updatecode(bankdataid, buycount, sellcount);
                }
                else
                {
                    bank.insertcode(buycount, sellcount, contrsctid, codetype);
                }

            }
            return model;
        }

        /// <summary> 银行按揭信息列表查询
        /// </summary>
        /// <returns></returns>
        public AdminJsonStr getbankList(HttpContext content)
        {
            AdminJsonStr model = new AdminJsonStr();
            bankDBAccess bank = new bankDBAccess();
            int tableTotalCount = 0;
            pageIndex = string.IsNullOrEmpty(content.Request["page"]) //页数
                        ? 1
                        : int.Parse(content.Request["page"].ToString());
            pageSize = string.IsNullOrEmpty(content.Request["limit"]) //页大小
                ? 10
                : int.Parse(content.Request["limit"].ToString());
            string contractNo = string.IsNullOrEmpty(content.Request["contractNo"])
                ? ""
                : content.Request["contractNo"].ToString();
            string stimeR = string.IsNullOrEmpty(content.Request["stimeR"])
                ? ""
                : content.Request["stimeR"].ToString();
            string etimeR = string.IsNullOrEmpty(content.Request["etimeR"])
                ? ""
                : content.Request["etimeR"].ToString();
            string userName = string.IsNullOrEmpty(content.Request["userName"])
                ? ""
                : content.Request["userName"].ToString();
            string clientName = string.IsNullOrEmpty(content.Request["clientName"])
                ? ""
                : content.Request["clientName"].ToString();
            string owenName = string.IsNullOrEmpty(content.Request["owenName"])
                ? ""
                : content.Request["owenName"].ToString();
            string attr = string.IsNullOrEmpty(content.Request["attr"])
                ? ""
                : content.Request["attr"].ToString();
            //string stimeC = string.IsNullOrEmpty(content.Request["stimeC"])
            //    ? ""
            //    : content.Request["stimeC"].ToString();
            //string etimeC = string.IsNullOrEmpty(content.Request["etimeC"])
            //    ? ""
            //    : content.Request["etimeC"].ToString();
            string neiqin = string.IsNullOrEmpty(content.Request["neiqin"])
                ? ""
                : content.Request["neiqin"].ToString();
            string receiveName = string.IsNullOrEmpty(content.Request["receiveName"])
                ? ""
                : content.Request["receiveName"].ToString();
            string selecttype = string.IsNullOrEmpty(content.Request["selecttype"])
                ? ""
                : content.Request["selecttype"].ToString();
            string paytype = string.IsNullOrEmpty(content.Request["paytype"])
                ? ""
                : content.Request["paytype"].ToString();
            string contracttypes = string.IsNullOrEmpty(content.Request["contracttypes"])
                ? ""
                : content.Request["contracttypes"].ToString();
            //str myreceived = string.IsNullOrEmpty(content.Request["myreceived"])
            //    ? false
            //    : Convert.ToBoolean(content.Request["myreceived"].ToString());
            bool less = string.IsNullOrEmpty(content.Request["less"])
                ? false
                : Convert.ToBoolean(content.Request["less"].ToString());
            string subbranchID = string.IsNullOrEmpty(content.Request["subbranchID"])
                ? ""
                : content.Request["subbranchID"].ToString();
            List<bankMortData> list_bank = bank.getbankMortList(contractNo, stimeR, etimeR, userName, clientName,
                owenName, neiqin, receiveName, selecttype, paytype, less, loginName, attr,
                subbranchID, contracttypes, pageIndex, pageSize, ref tableTotalCount);
            model.data = list_bank;
            model.count = tableTotalCount; //所有数据数量
            return model;
        }

        /// <summary> 银行按揭信息列表获取付款方式
        /// </summary>
        /// <returns></returns>
        public AdminJsonStr getpayway()
        {
            AdminJsonStr model = new AdminJsonStr();
            List<payWay> list_payway = new payWay().GetListPayWays();
            model.data = list_payway;
            model.count = list_payway.Count;
            return model;
        }
        /// <summary> 银行按揭信息列表获取合同进度
        /// </summary>
        /// <returns></returns>
        public AdminJsonStr getcontractType()
        {
            AdminJsonStr model = new AdminJsonStr();
            List<contractType> list_payway = new contractType().GetListcontracts();
            model.data = list_payway;
            model.count = list_payway.Count;
            return model;
        }
        public AdminJsonStr getbankList()
        {
            AdminJsonStr model = new AdminJsonStr();
            List<keyValuePair> list_payway = new bankDBAccess().getbanklist();
            model.data = list_payway;
            model.count = list_payway.Count;
            return model;
        }


        /// <summary>银行按揭信息详情收到/缺失按揭资料查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isReceive">true 已收到; false  缺失</param>
        /// <returns></returns>
        public AdminJsonStr getReceiveDataList(HttpContext content, bool isReceive)
        {
            //表格分页数据
            AdminJsonStr model = new AdminJsonStr();
            bankDBAccess bank = new bankDBAccess();
            pageIndex = string.IsNullOrEmpty(content.Request["page"]) //页数
                       ? 1
                       : int.Parse(content.Request["page"].ToString());
            pageSize = string.IsNullOrEmpty(content.Request["limit"]) //页大小
                ? 2
                : int.Parse(content.Request["limit"].ToString());
            cid = string.IsNullOrEmpty(content.Request["cid"])
                ? ""
                : content.Request["cid"].ToString();
            if (string.IsNullOrEmpty(cid))
            {
                model.code = 1;
                model.msg = "无该合同的按揭信息!";
                return model;
            }
            int tableTotalCount = 0;
            List<receiveData> list_Data = bank.GetReceiveDataList(cid, isReceive, pageIndex, pageSize, ref tableTotalCount);
            model.data = list_Data;
            model.count = tableTotalCount;
            if (isReceive)
            {
                //获取拼接数据
                string ReceiveBuyHaveDatas = "";
                string ReceiveSellHaveDatas = "";
                string ReceiveBuyNoHaveDatas = "";
                string ReceiveSellNoHaveDatas = "";
                bank.getFullData(cid, ref ReceiveBuyHaveDatas, ref ReceiveSellHaveDatas, ref ReceiveBuyNoHaveDatas,
                    ref ReceiveSellNoHaveDatas);
                model.ReceiveBuyHaveDatas = ReceiveBuyHaveDatas;
                model.ReceiveSellHaveDatas = ReceiveSellHaveDatas;
                model.ReceiveBuyNoHaveDatas = ReceiveBuyNoHaveDatas;
                model.ReceiveSellNoHaveDatas = ReceiveSellNoHaveDatas;
            }
            return model;
        }


        /// <summary> 删除数据
        /// </summary>
        /// <returns></returns>
        public AdminJsonStr delReceiveDataList(HttpContext content)
        {
            AdminJsonStr model = new AdminJsonStr();
            bankDBAccess bank = new bankDBAccess();
            string delidlist = content.Request["delidlist"].ToString();
            if (string.IsNullOrEmpty(delidlist))
            {
                model.code = 1;
                model.msg = "请选择删除数据";
                return model;
            }
            delidlist = "" + delidlist.TrimEnd(',') + "";
            try
            {
                bank.deldata(delidlist);
            }
            catch (Exception ex)
            {
                model.code = 1;
                model.msg = ex.Message;
            }
            return model;
        }


        public AdminJsonStr updatePwd(HttpContext content)
        {
            AdminJsonStr model = new AdminJsonStr();
            bankDBAccess bank = new bankDBAccess();
            string subbranchID = content.Request["subbranchID"].ToString().Split('|')[0];
            string oldpwd = content.Request["oldpwd"].ToString();
            string newpwd = content.Request["newpwd"].ToString();
            try
            {
                string msg = bank.updatePwd(subbranchID, newpwd, oldpwd);
                if (!string.IsNullOrEmpty(msg))
                {
                    model.code = 1;
                    model.msg = msg;
                    return model;
                }
            }
            catch (Exception ex)
            {
                model.code = 1;
                model.msg = ex.Message;
                return model;
            }
            return model;
        }




    }
}
