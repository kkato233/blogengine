using System;
using System.Globalization;
using System.Web.Security;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;

public partial class admin_admin : System.Web.UI.MasterPage
{
    private const string GRAVATAR_IMAGE = "<img class=\"photo\" src=\"{0}\" alt=\"{1}\" />";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            Response.Redirect(Utils.RelativeWebRoot);

        AddJquery();
        AddJavaScript(Utils.RelativeWebRoot + "admin/admin.js");
    }

    protected AuthorProfile AdminProfile()
    {
        try
        {
            return AuthorProfile.GetProfile(System.Threading.Thread.CurrentPrincipal.Identity.Name);
        }
        catch (Exception e)
        {
            Utils.Log(e.Message);
            return null;
        }
    }

    protected string AdminPhoto()
    {
        string src = Utils.AbsoluteWebRoot + "admin/images/no_avatar.png";
        string adminName = "";

        if (AdminProfile() != null)
        {
            adminName = AdminProfile().DisplayName;
            if (!string.IsNullOrEmpty(AdminProfile().PhotoUrl))
            {
                src = AdminProfile().PhotoUrl;
            }else
            {
                if(!string.IsNullOrEmpty(AdminProfile().EmailAddress) &&
                    BlogSettings.Instance.Avatar != "none")
                        src = Avatar(AdminProfile().EmailAddress);
            }
        }
        return string.Format(CultureInfo.InvariantCulture, GRAVATAR_IMAGE, src, adminName);
    }

    protected string Avatar(string email)
    {
        string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(email.ToLowerInvariant().Trim(), "MD5").ToLowerInvariant();
        string src = "http://www.gravatar.com/avatar/" + hash + ".jpg?s=28&amp;d=";

        switch (BlogSettings.Instance.Avatar)
        {
            case "identicon":
                src += "identicon";
                break;
            case "wavatar":
                src += "wavatar";
                break;
            default:
                src += "monsterid";
                break;
        }
        return src;
    }

    public void SetStatus(string status, string msg)
    {
        AdminStatus.Attributes.Clear();
        AdminStatus.Attributes.Add("class", status);
        AdminStatus.InnerHtml = Server.HtmlEncode(msg) + "<a href=\"javascript:HideStatus()\" style=\"width:20px;float:right\">X</a>";
        
        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OpenStatus", 
        //    "ShowStatus('" + status + "','" + msg + "');", true);
    }

    protected virtual void AddJquery()
    {
        string s = Path.Combine(Server.MapPath("~/"), "Scripts");
        string[] fileEntries = Directory.GetFiles(s);
        foreach (string fileName in fileEntries)
        {
            if ((fileName.EndsWith(".js", StringComparison.OrdinalIgnoreCase) && fileName.Contains("jquery-")) &&
                !fileName.EndsWith("-vsdoc.js", StringComparison.OrdinalIgnoreCase))
            {
                AddJavaScript(Utils.RelativeWebRoot + "Scripts/" + Utils.ExtractFileNameFromPath(fileName));
            }
        }
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