using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

/// <summary>
///UserPrincipal 的摘要说明
/// </summary>
public class UserPrincipal : IPrincipal
{
    public static UserPrincipal current
    {
        get
        {
            if (HttpContext.Current.User is UserPrincipal)
            {
                return HttpContext.Current.User as UserPrincipal;
            }
            else
            {
                return null;
            }
        }
    }

    private string _subbranchID;
    public string SubbranchID
    {
        get { return _subbranchID; }
        set { _subbranchID = value; }
    }

    private string _subbranchName;
    public string SubbranchName
    {
        get { return _subbranchName; }
        set { _subbranchName = value; }
    }

    private string _password;
    public string Password
    {
        get { return _password; }
        set { _password = value; }
    }

    private string _userName;
    public string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }

   



    #region IPrincipal 成员

    public IIdentity Identity
    {
        get
        {
            IdentityUser1 IUser = new IdentityUser1();
            IUser.Name = _subbranchID + "|" + _userName;
            return IUser;
        }
    }

    public bool IsInRole(string role)
    {
        return true;
    }

    #endregion
}

public class IdentityUser1 : IIdentity
{
    #region IIdentity 成员

    public string AuthenticationType
    {
        get { return "from"; }
    }

    public bool IsAuthenticated
    {
        get { return true; }
    }

    private string _userName;
    public string Name
    {
        get { return _userName; }
        set
        {
            _userName = value;
        }
    }

    #endregion

}