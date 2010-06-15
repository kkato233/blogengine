using System;

public partial class Account_Account : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void SetStatus(string status, string msg)
    {
        AdminStatus.Attributes.Clear();
        AdminStatus.Attributes.Add("class", status);
        AdminStatus.InnerHtml = Server.HtmlEncode(msg) + "<a href=\"javascript:HideStatus()\" style=\"width:20px;float:right\">X</a>";
    }
}
