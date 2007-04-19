#region Using

using System;
using DotNetSlave.BlogEngine.BusinessLogic;

#endregion

public partial class _default : BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (Page.IsCallback)
      return;

    if (string.IsNullOrEmpty(Request.QueryString["q"]))
    {
      PostList1.Posts = Post.Posts;      
      Page.Title = BlogSettings.Instance.Name + " - " + BlogSettings.Instance.Description;
    }
    else
    {
      bool includeComments = Request.QueryString["comment"] != null;
      PostList1.Posts = Search.Hits(Post.Posts, Request.QueryString["q"], includeComments);
      Page.Title = "Search result for '" + Server.HtmlEncode(Request.QueryString["q"]) + "'";
    }

    base.AddMetaTag("description", BlogSettings.Instance.Description);
  }
}
