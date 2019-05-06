using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Business.bankMort;

/// <summary>
///AuthProvider 的摘要说明
/// </summary>
public class AuthProvider
{
    private string _errInfo;
    public string ErrInfo
    {
        get
        {
            return _errInfo;
        }
    }

    public bool ValidateUser(string bankname,string username, string password)
    {
        bankDBAccess bankDBProvider = new bankDBAccess();
        UserPrincipal _userprincipal = bankDBProvider.getLoginInfoByBankName(bankname,username);
        if (_userprincipal == null)
        {
            _errInfo = "当前用户名不存在！";
            return false;
        }
        string pwd = _userprincipal.Password;
        if (password==pwd)
        {
            setFormsAuthenticationTicket(_userprincipal.SubbranchID + "|" + username, true);
            HttpContext.Current.User = _userprincipal;
            return true;
        }
        else
        {
            _errInfo = "密码错误！";
            return false;
        }
    }

    private void setFormsAuthenticationTicket(string userkey, bool persistent)
    {
        //创建票据
        //读取票据有效时间,默认是30分
        int nTicketTime = 30;
        persistent = false;
        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
            1,
            userkey,
            DateTime.Now,
            DateTime.Now.AddMinutes(nTicketTime),
            persistent,
            userkey);
        ///票据加密
        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
        ///存为Cookie
        HttpCookie authCookie = new HttpCookie(
            FormsAuthentication.FormsCookieName, encryptedTicket);
        authCookie.Path = FormsAuthentication.FormsCookiePath;
        HttpContext.Current.Response.Cookies.Add(authCookie);
    }

    private void RemoveFormsAuthenticationTicket(string userkey, bool persistent)
    {
        HttpContext.Current.Response.Cookies.Remove(userkey);
    }
}