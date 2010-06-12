using System;
using System.Web.Security;

public partial class Account_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnPassLength.Value = Membership.MinRequiredPasswordLength.ToString();
    }
    protected void ChangePasswordPushButton_Click(object sender, EventArgs e)
    {
        ((Account_Account)this.Master).SetStatus("warning", "Password was not changed");
    }
}
