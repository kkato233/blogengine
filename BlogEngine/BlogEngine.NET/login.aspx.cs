using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class login : BlogEngine.Core.Web.Controls.BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (Request.QueryString["signout"] == "true")
    {
      FormsAuthentication.SignOut();
      Response.Redirect(Request.UrlReferrer.ToString(), true);  
    }

    Login1.FindControl("username").Focus();
  }
}
