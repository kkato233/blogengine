using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;

public partial class Account_Account : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AddJavaScript(Utils.RelativeWebRoot + "Account/Account.js");
    }

    public void SetStatus(string status, string msg)
    {
        AdminStatus.Attributes.Clear();
        AdminStatus.Attributes.Add("class", status);
        AdminStatus.InnerHtml = Server.HtmlEncode(msg) + "<a href=\"javascript:HideStatus()\" style=\"width:20px;float:right\">X</a>";
    }

    void AddJavaScript(string src)
    {
        foreach (Control ctl in Page.Header.Controls)
        {
            if (ctl.GetType() == typeof(HtmlGenericControl))
            {
                HtmlGenericControl gc = (HtmlGenericControl)ctl;
                if (gc.Attributes["src"] != null)
                {
                    if (gc.Attributes["src"].Contains(src))
                        return;
                }
            }
        }

        HtmlGenericControl js = new HtmlGenericControl("script");

        js.Attributes["type"] = "text/javascript";
        js.Attributes["src"] = Utils.RelativeWebRoot + "js.axd?path=" + Server.UrlEncode(src);
        js.Attributes["defer"] = "defer";

        Page.Header.Controls.Add(js);
    }
}
