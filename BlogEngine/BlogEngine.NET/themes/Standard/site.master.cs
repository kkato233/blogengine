using System;
using System.Web.UI;
using BlogEngine.Core;
using System.Text.RegularExpressions;

public partial class StandardSite : System.Web.UI.MasterPage
{
    private static Regex reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}");

    protected static string ShRoot = Utils.ApplicationRelativeWebRoot + "editors/tiny_mce_3_5_8/plugins/syntaxhighlighter/";

    protected void Page_Load(object sender, EventArgs e)
    {
        // needed to make <%# %> binding work in the page header
        Page.Header.DataBind();

        if (!Utils.IsMono)
        {
            var lc = new LiteralControl("\n<!--[if lt IE 9]>" +
	            "\n<script type=\"text/javascript\" src=\"/themes/standard/include/html5.js\"></script>" +
                "\n<![endif]-->\n");
            HeadContent.Controls.Add(lc);
        }
        if (Security.IsAuthenticated)
        {
            aUser.InnerText = "Welcome" + Page.User.Identity.Name + "!";
            aLogin.InnerText = Resources.labels.logoff;
            aLogin.HRef = Utils.RelativeWebRoot + "Account/login.aspx?logoff";
        }
        else
        {
            aLogin.HRef = Utils.RelativeWebRoot + "Account/login.aspx";
            aLogin.InnerText = Resources.labels.login;
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
        {
            base.Render(htmlwriter);
            writer.Write(htmlwriter.InnerWriter.ToString());
        }
    }

}
