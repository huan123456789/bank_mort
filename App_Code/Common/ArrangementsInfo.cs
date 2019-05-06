using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///ArrangementsInfo 的摘要说明
/// </summary>
public class ArrangementsInfo
{
    private string buycode;
    /// <summary>
    /// 买方资料
    /// </summary>
    public string Buycode
    {
        get { return buycode; }
        set { buycode = value; }
    }
    private string sellcode;
    /// <summary>
    /// 卖方资料
    /// </summary>
    public string Sellcode
    {
        get { return sellcode; }
        set { sellcode = value; }
    }
}

public class contractinfo
{
    private string contratctnum;
    /// <summary>
    /// 合同编号
    /// </summary>
    public string Contratctnum
    {
        get { return contratctnum; }
        set { contratctnum = value; }
    }
    private string buyname;
    /// <summary>
    /// 买方姓名
    /// </summary>
    public string Buyname
    {
        get { return buyname; }
        set { buyname = value; }
    }

    private string sellname;
    /// <summary>
    /// 卖方姓名
    /// </summary>
    public string Sellname
    {
        get { return sellname; }
        set { sellname = value; }
    }
    private string agent;
    /// <summary>
    /// 经纪人
    /// </summary>
    public string Agent
    {
        get { return agent; }
        set { agent = value; }
    }
    private string loadname;
    /// <summary>
    /// 信贷专员
    /// </summary>
    public string Loadname
    {
        get { return loadname; }
        set { loadname = value; }
    }

    private string area;
    /// <summary>
    /// 面积
    /// </summary>
    public string Area
    {
        get { return area; }
        set { area = value; }
    }
    private string address;
    /// <summary>
    /// 物业地址
    /// </summary>
    public string Address
    {
        get { return address; }
        set { address = value; }
    }
    private string contractcountmoney;
    /// <summary>
    /// 合同总价
    /// </summary>
    public string Contractcountmoney
    {
        get { return contractcountmoney; }
        set { contractcountmoney = value; }
    }

    private string frfistmoney;
    /// <summary>
    /// 首付金额
    /// </summary>
    public string Frfistmoney
    {
        get { return frfistmoney; }
        set { frfistmoney = value; }
    }

    private string loadmoney = "0";
    /// <summary>商业贷款金额
    /// </summary>
    public string Loadmoney
    {
        get { return loadmoney; }
        set { loadmoney = value; }
    }
    private string gjjloadmoney = "0";
    /// <summary> 公积金贷款金额
    /// </summary>
    public string GJJLoadmoney
    {
        get { return gjjloadmoney; }
        set { gjjloadmoney = value; }
    }
    private string bankname;
    /// <summary>
    /// 银行名字
    /// </summary>
    public string Bankname
    {
        get { return bankname; }
        set { bankname = value; }
    }
    private string acceptTime;
    /// <summary>
    /// 接收时间
    /// </summary>
    public string AcceptTime
    {
        get { return acceptTime; }
        set { acceptTime = value; }
    }

    private string mortgageTime;
    /// <summary>
    /// 按揭时间
    /// </summary>
    public string MortgageTime
    {
        get { return mortgageTime; }
        set { mortgageTime = value; }
    }

    private string colletData;
    /// <summary>
    /// 资料收齐时间
    /// </summary>
    public string ColletData
    {
        get { return colletData; }
        set { colletData = value; }
    }

    private string dataPaass;
    /// <summary>
    /// 审核通过时间
    /// </summary>
    public string DataPaass
    {
        get { return dataPaass; }
        set { dataPaass = value; }
    }

    private string contractTurn;
    /// <summary>
    /// 借款合同移交时间
    /// </summary>
    public string ContractTurn
    {
        get { return contractTurn; }
        set { contractTurn = value; }
    }
    private string bounceTime;
    /// <summary>
    /// 退件时间
    /// </summary>
    public string BounceTime
    {
        get { return bounceTime; }
        set { bounceTime = value; }
    }
}