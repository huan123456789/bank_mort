using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class bankMort_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string  txt_type.Value;
    }
    protected void bnt_login_Click(object sender, EventArgs e)
    {
        AuthProvider authprovider = new AuthProvider();
        string bankname = hf_bankname.Value;
        string username = txt_username.Value;
        string pwd = txt_pwd.Value;
        if (authprovider.ValidateUser(bankname, username, pwd))
        {
            FormsAuthentication.RedirectFromLoginPage(HttpContext.Current.User.Identity.Name, true, FormsAuthentication.FormsCookieName);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('登录失败！');", true);
            return;
        }
    }
}