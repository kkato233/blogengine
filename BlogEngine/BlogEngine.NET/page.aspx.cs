#region Using

using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;

#endregion

public partial class page : BlogEngine.Core.Web.Controls.BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (Request.QueryString["deletepage"] != null && Request.QueryString["deletepage"].Length == 36)
    {
      if (System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
      {
        Guid id = new Guid(Request.QueryString["deletepage"]);
        Page page = Page.GetPage(id);
        page.Delete();
        page.Save();
        Response.Redirect("~/", true);
      }
    }
    else if (Request.QueryString["id"] != null && Request.QueryString["id"].Length == 36)
    {
      Guid id = new Guid(Request.QueryString["id"]);
      this.Page = BlogEngine.Core.Page.GetPage(id);
      if (this.Page != null)
      {
        if (!this.Page.IsPublished && !this.User.Identity.IsAuthenticated)
          Response.Redirect(Utils.RelativeWebRoot + "error404.aspx", true);

        h1Title.InnerHtml = this.Page.Title;

        ServingEventArgs arg = new ServingEventArgs(this.Page.Content, ServingLocation.SinglePage);
        Page.OnServing(this.Page, arg);
        divText.InnerHtml = arg.Body;

        base.Title = this.Page.Title;
        base.AddMetaTag("keywords", this.Page.Keywords);
        base.AddMetaTag("description", this.Page.Description);
      }
    }
    else
    {
      Response.Redirect("~/");
    }
  }


  /// <summary>
  /// 
  /// </summary>
  public new BlogEngine.Core.Page Page;

  /// <summary>
  /// Gets the admin links to edit and delete a page.
  /// </summary>
  /// <value>The admin links.</value>
  public string AdminLinks
  {
    get 
    {
      if (System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<div id=\"admin\">");
        sb.AppendFormat("<a href=\"{0}admin/pages/pages.aspx?id={1}\">{2}</a> | ", Utils.RelativeWebRoot, this.Page.Id.ToString(), Resources.labels.edit);
        sb.AppendFormat("<a href=\"?deletepage={0}\" onclick=\"return confirm('Are you sure you want to delete the page?')\">{1}</a>", this.Page.Id.ToString(), Resources.labels.delete);
        sb.AppendLine("</div>");
        return sb.ToString();
      }

      return string.Empty;
    }
  }


}
