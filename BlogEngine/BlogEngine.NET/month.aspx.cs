#region Using

using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetSlave.BlogEngine.BusinessLogic;

#endregion

public partial class month : BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!string.IsNullOrEmpty(Request.QueryString["year"]) && !string.IsNullOrEmpty(Request.QueryString["month"]))
    {
      DateTime dateFrom = DateTime.Parse(Request.QueryString["year"] + "-" + Request.QueryString["month"] + "-01");
      DateTime dateTo = dateFrom.AddMonths(1).AddDays(-1);
      PostList1.Posts = Post.GetPostsByDate(dateFrom, dateTo);
    }
    else if (!string.IsNullOrEmpty(Request.QueryString["date"]))
    {
      DateTime date = DateTime.Parse(Request.QueryString["date"]);
      PostList1.Posts = Post.GetPostsByDate(date, date);
    }
    else
    {
      calendar.Visible = true;
      PostList1.Visible = false;
    }

    base.Title = this.Page.Title;
    base.AddMetaTag("description", BlogSettings.Instance.Description);
  }
}
