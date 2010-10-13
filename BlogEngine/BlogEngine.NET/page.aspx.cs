#region Using

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;

using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;

using Resources;

using Page = BlogEngine.Core.Page;

#endregion

/// <summary>
/// The page.
/// </summary>
public partial class page : BlogBasePage
{
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
        if (this.Request.QueryString["deletepage"] != null && this.Request.QueryString["deletepage"].Length == 36)
        {
            this.DeletePage(new Guid(this.Request.QueryString["deletepage"]));
        }

        if (this.Request.QueryString["id"] != null && this.Request.QueryString["id"].Length == 36)
        {
            this.ServePage(new Guid(this.Request.QueryString["id"]));
            this.AddMetaTags();
        }
        else
        {
            this.Response.Redirect(Utils.RelativeWebRoot);
        }

        base.OnInit(e);
    }

    /// <summary>
    /// Serves the page to the containing DIV tag on the page.
    /// </summary>
    /// <param name="id">
    /// The id of the page to serve.
    /// </param>
    private void ServePage(Guid id)
    {
        this.Page = BlogEngine.Core.Page.GetPage(id);

        if (this.Page == null || (!this.Page.IsVisible))
        {
            this.Response.Redirect(string.Format("{0}error404.aspx", Utils.RelativeWebRoot), true);
            return; // WLF: ReSharper is stupid and doesn't know that redirect returns this method.... or does it not...?
        }

        this.h1Title.InnerHtml = this.Page.Title;

        var arg = new ServingEventArgs(this.Page.Content, ServingLocation.SinglePage);
        BlogEngine.Core.Page.OnServing(this.Page, arg);

        if (arg.Cancel)
        {
            this.Response.Redirect("error404.aspx", true);
        }

        if (arg.Body.ToLowerInvariant().Contains("[usercontrol"))
        {
            this.InjectUserControls(arg.Body);
        }
        else
        {
            this.divText.InnerHtml = arg.Body;
        }
    }

    /// <summary>
    /// Adds the meta tags and title to the HTML header.
    /// </summary>
    private void AddMetaTags()
    {
        if (this.Page == null)
        {
            return;
        }

        this.Title = this.Server.HtmlEncode(this.Page.Title);
        this.AddMetaTag("keywords", this.Server.HtmlEncode(this.Page.Keywords));
        this.AddMetaTag("description", this.Server.HtmlEncode(this.Page.Description));
    }

    /// <summary>
    /// Deletes the page.
    /// </summary>
    /// <param name="id">
    /// The page id.
    /// </param>
    private void DeletePage(Guid id)
    {
        if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
        {
            return;
        }

        var page = BlogEngine.Core.Page.GetPage(id);
        page.Delete();
        page.Save();
        this.Response.Redirect("~/", true);
    }

    /// <summary>
    /// The body regex.
    /// </summary>
    private static readonly Regex BodyRegex = new Regex(
        @"\[UserControl:(.*?)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Injects any user controls if one is referenced in the text.
    /// </summary>
    /// <param name="content">
    /// The content.
    /// </param>
    private void InjectUserControls(string content)
    {
        var currentPosition = 0;
        var thematches = BodyRegex.Matches(content);

        foreach (Match mymatch in thematches)
        {
            if (mymatch.Index > currentPosition)
            {
                this.divText.Controls.Add(
                    new LiteralControl(content.Substring(currentPosition, mymatch.Index - currentPosition)));
            }

            try
            {
                var all = mymatch.Groups[1].Value.Trim();
                Control usercontrol;

                if (!all.EndsWith(".ascx", StringComparison.OrdinalIgnoreCase))
                {
                    var index = all.IndexOf(".ascx", StringComparison.OrdinalIgnoreCase) + 5;
                    usercontrol = this.LoadControl(all.Substring(0, index));

                    var parameters = this.Server.HtmlDecode(all.Substring(index));
                    var type = usercontrol.GetType();
                    var paramCollection = parameters.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var param in paramCollection)
                    {
                        var name = param.Split('=')[0].Trim();
                        var value = param.Split('=')[1].Trim();
                        var property = type.GetProperty(name);
                        property.SetValue(
                            usercontrol,
                            Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture),
                            null);
                    }
                }
                else
                {
                    usercontrol = this.LoadControl(all);
                }

                this.divText.Controls.Add(usercontrol);
            }
            catch (Exception)
            {
                this.divText.Controls.Add(
                    new LiteralControl(string.Format("ERROR - UNABLE TO LOAD CONTROL : {0}", mymatch.Groups[1].Value)));
            }

            currentPosition = mymatch.Index + mymatch.Groups[0].Length;
        }

        // Finally we add any trailing static text.
        this.divText.Controls.Add(
            new LiteralControl(content.Substring(currentPosition, content.Length - currentPosition)));
    }

    /// <summary>
    ///     The Page instance to render on the page.
    /// </summary>
    public new Page Page;

    /// <summary>
    ///     Gets the admin links to edit and delete a page.
    /// </summary>
    /// <value>The admin links.</value>
    public string AdminLinks
    {
        get
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var sb = new StringBuilder();
                sb.AppendLine("<div id=\"admin\">");
                sb.AppendFormat(
                    "<a href=\"{0}admin/Pages/Pages.aspx?id={1}\">{2}</a> | ",
                    Utils.RelativeWebRoot,
                    this.Page.Id,
                    labels.edit);
                sb.AppendFormat(
                    "<a href=\"javascript:void(0);\" onclick=\"if (confirm('Are you sure you want to delete the page?')) location.href='?deletepage={0}'\">{1}</a>",
                    this.Page.Id,
                    labels.delete);
                sb.AppendLine("</div>");
                return sb.ToString();
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Gets PermaLink.
    /// </summary>
    public string PermaLink
    {
        get
        {
            return string.Format("{0}page.aspx?id={1}", Utils.AbsoluteWebRoot, this.Page.Id);
        }
    }
}