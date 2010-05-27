using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);

        if (Request.QueryString.ToString() == "logoff")
        {
            System.Web.Security.FormsAuthentication.SignOut();
            if (Request.UrlReferrer != null && Request.UrlReferrer != Request.Url)
            {
                Response.Redirect(Request.UrlReferrer.ToString(), true);
            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }
    }
}
