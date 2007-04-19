using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetSlave.BlogEngine.BusinessLogic;

public partial class author : BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!string.IsNullOrEmpty(Request.QueryString["name"]))
    {
      PostList1.Posts = Post.GetPostsByAuthor(Request.QueryString["name"]);;
      
      base.Title = BlogSettings.Instance.Name + " - All posts by " +  Request.QueryString["name"];
      base.AddMetaTag("description", BlogSettings.Instance.Description);
    }
  }
}
