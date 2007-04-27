using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;

public partial class page : BlogEngine.Core.Web.Controls.BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (Request.QueryString["delete"] != null && Request.QueryString["delete"].Length == 36)
    {
      if (System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
      {
        Guid id = new Guid(Request.QueryString["delete"]);
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
        h1Title.InnerHtml = this.Page.Title;
        divText.InnerHtml = this.Page.Content;

        base.Title = this.Page.Title;
        base.AddMetaTag("keywords", this.Page.Keywords);
        base.AddMetaTag("description", this.Page.Description);
      }      
    }    
  }

  /// <summary>
  /// 
  /// </summary>
  public new BlogEngine.Core.Page Page;

  public string AdminLinks
  {
    get 
    {
      if (System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<div id=\"admin\">");
        sb.AppendFormat("<a href=\"{0}admin/pages/pages.aspx?id={1}\">Edit</a> | ", Utils.RelativeWebRoot, this.Page.Id.ToString());
        sb.AppendFormat("<a href=\"?delete={0}\" onclick=\"return confirm('Are you sure you want to delete the page?')\">Delete</a>", this.Page.Id.ToString());
        sb.AppendLine("</div>");
        return sb.ToString();
      }

      return string.Empty;
    }
  }


}
