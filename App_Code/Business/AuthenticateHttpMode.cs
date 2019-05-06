using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Caching;
using Business.bankMort;

/// <summary>
///AuthenticateHttpMode 的摘要说明
/// </summary>
public class AuthenticateHttpMode : IHttpModule
{
    #region IHttpModule 成员

    public void Dispose()
    {

    }

    public void Init(HttpApplication context)
    {
        context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
    }

    #endregion


    void context_AuthenticateRequest(object sender, EventArgs e)
    {
        HttpApplication application = (HttpApplication)sender;
        HttpContext context = application.Context;
        HttpCookie authCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
        if (authCookie == null)
        {
            //如没有提取到身份验证信息
            return;
        }
        string loginName = getGuidFromCookie(authCookie);
        if (loginName == "")
        {
            //如没有提取到身份验证信息
            return;
        }
        UserPrincipal curUser = getUserFromCacheByLoginName(loginName);
        if (curUser == null)
        {
            bankDBAccess bankDBprovider = new bankDBAccess();
            curUser = bankDBprovider.getSubbranchLoginInfo(loginName);
            if (curUser != null)
            {
                saveDataToCache(curUser.SubbranchID, curUser);
            }
        }
        if (curUser == null)
        {
            return;

        }
        HttpContext.Current.User = curUser;
        //HttpContext.Current.User = new UserPrincipal();
    }

    private static UserPrincipal getUserFromCacheByLoginName(string loginName)
    {
        if (HttpContext.Current != null)
        {
            return HttpContext.Current.Cache[loginName] as UserPrincipal;
        }
        return null;
    }

    /// <summary>
    /// 保存数据到Cache，默认自动设为30分钟后过期
    /// </summary>
    public void saveDataToCache(string key, object value)
    {
        HttpContext.Current.Cache.Remove(key);
        HttpContext.Current.Cache.Add(key, value, null, Cache.NoAbsoluteExpiration,
                        new TimeSpan(0, 30, 0), CacheItemPriority.High, null);

    }

    private static string getGuidFromCookie(HttpCookie cookie1)
    {
        //解压身份票
        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie1.Value);

        if (authTicket == null)
        {
            return "";   //如无法解压
        }
        else
        {
            return authTicket.Name;
        }
    }
}