using System;
using System.Web;
using System.Web.Security;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        RegisterHyperLink.Text = Resources.labels.createNow;

        if (Request.QueryString.ToString() == "logoff")
        {
            FormsAuthentication.SignOut();
            if (Request.UrlReferrer != null && Request.UrlReferrer != Request.Url)
            {
                Response.Redirect(Request.UrlReferrer.ToString(), true);
            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }

        if(Page.IsPostBack)
        {
            if(!User.Identity.IsAuthenticated)
            {
                ((Account_Account)this.Master).SetStatus("warning", "Login failed");
            }
        }
    }
}
