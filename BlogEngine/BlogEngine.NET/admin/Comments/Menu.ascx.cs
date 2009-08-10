using System;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;

public partial class admin_Comments_Menu : System.Web.UI.UserControl
{
    private static bool _isAutoModerated;

    public enum Moderation
    {
        Off,
        Manual,
        Auto
    }

    public static Moderation ModerationMode
    {
        get
        {
            if (BlogSettings.Instance.EnableCommentsModeration)
            {
                if (_isAutoModerated)
                    return Moderation.Auto;
                return Moderation.Manual;
            }     
            return Moderation.Off;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //TODO: get value checking if anti-spam service present
        _isAutoModerated = false;

        BuildMenuList();

        if(!Page.IsPostBack)
        {
            
        }
    }

    protected void BuildMenuList()
    {
        string cssClass = "";
        string tmpl = "<a href=\"{0}.aspx\" class=\"{1}\"><span>{2}</span></a>";

        HtmlGenericControl inbx = new HtmlGenericControl("li");
        cssClass = Request.Path.ToLower().Contains("default.aspx") ? "current" : "";
        inbx.InnerHtml = string.Format(tmpl, "Default", cssClass, "Inbox");
        UlMenu.Controls.Add(inbx);

        HtmlGenericControl appr = new HtmlGenericControl("li");
        cssClass = Request.Path.ToLower().Contains("approved.aspx") ? "current" : "";
        appr.InnerHtml = string.Format(tmpl, "Approved", cssClass, "Approved");

        HtmlGenericControl spm = new HtmlGenericControl("li");
        cssClass = Request.Path.ToLower().Contains("spam.aspx") ? "current" : "";
        spm.InnerHtml = string.Format(tmpl, "Spam", cssClass, "Spam");

        switch (ModerationMode)
        {
            case Moderation.Manual:
                hdr.InnerHtml = "Comments: Manual Moderation";
                UlMenu.Controls.Add(appr);
                break;
            case Moderation.Auto:
                hdr.InnerHtml = "Comments: Auto-Moderated";
                UlMenu.Controls.Add(spm);
                break;
            default:
                hdr.InnerHtml = "Comments: Unmoderated";
                break;
        }

        HtmlGenericControl stn = new HtmlGenericControl("li");
        cssClass = Request.Path.ToLower().Contains("settings.aspx") ? "current" : "";
        stn.InnerHtml = string.Format(tmpl, "Settings", cssClass, "Configuration");
        UlMenu.Controls.Add(stn);
    }
}