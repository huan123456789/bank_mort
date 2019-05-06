<%@ WebHandler Language="C#" Class="bankMortgage" %>

using System;
using Common;
using System.Web;
using Business.common;
using Business.bankMort;

public class bankMortgage : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        AdminJsonStr model = new AdminJsonStr();
        try
        {
            if (context.Request["t"] == "10")
            {
            }
            model = new bankBusiness().dealbank(context);
        }
        catch (Exception exp)
        {
            model.code = 100;
            model.msg = "银行按揭：bankMortgage.ashx" + exp.Message;
            LogProvider.SaveLog("银行按揭：bankMortgage.ashx" + exp.Message);
        }
        JsonProvider.AdminJsonSplitJoint(model);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}