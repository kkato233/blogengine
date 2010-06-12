using System;
using System.Web.Security;

public partial class Account_Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        hdnPassLength.Value = Membership.MinRequiredPasswordLength.ToString();

        // if self registration not allowed and user is trying to directly
        // navigate to register page, redirect to login
        if (!BlogEngine.Core.BlogSettings.Instance.EnableSelfRegistration)
        {
            if(!User.Identity.IsAuthenticated)
                Response.Redirect("~/Account/login.aspx?ReturnUrl=" + Request.QueryString["ReturnUrl"]);
        }
    }

    protected void RegisterUser_CreatedUser(object sender, EventArgs e)
    {
        FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

        string continueUrl = RegisterUser.ContinueDestinationPageUrl;
        if (String.IsNullOrEmpty(continueUrl))
        {
            continueUrl = "~/";
        }
        Response.Redirect(continueUrl);
    }
}
