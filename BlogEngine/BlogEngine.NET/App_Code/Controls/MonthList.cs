#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using BlogEngine.Core;

#endregion

namespace Controls
{
  /// <summary>
  /// Builds a category list.
  /// </summary>
  public class MonthList : Control
  {

    static MonthList()
    {
      BindMonths();
      Post.Saved += new EventHandler<SavedEventArgs>(Post_Saved);
    }

    static void Post_Saved(object sender, SavedEventArgs e)
    {
      if (e.Action != SaveAction.Update)
        BindMonths();
    }

    #region Private fields

    private static object _SyncRoot = new object();
    private static Dictionary<DateTime, int> _Months = new Dictionary<DateTime, int>();
   
    #endregion

    private static void BindMonths()
    {
      lock (_SyncRoot)
      {
        _Months.Clear();
        DateTime first = DateTime.Parse(Post.Posts[Post.Posts.Count - 1].DateCreated.Date.ToString("yyyy-MM-") + "01");
        int year = first.Year;
        int month = first.Month;

        while (first <= DateTime.Now)
        {
          List<Post> list = Post.GetPostsByDate(first, DateTime.Parse(first.Year + "-" + first.Month + "-01").AddMonths(1).AddDays(-1));
          if (list.Count > 0)
          {
            DateTime date = DateTime.Parse(first.Year + "-" + first.Month + "-01");
            int posts = list.Count;
            _Months.Add(date, posts);
          }

          first = first.AddMonths(1);
        }
      }
    }

    private string RenderMonths()
    {
      HtmlGenericControl ul = new HtmlGenericControl("ul");
      ul.Attributes.Add("id", "monthList");
      HtmlGenericControl year = null;
      HtmlGenericControl list = null;
      foreach (DateTime date in _Months.Keys)
      {
        if (date.Month == 1 || ul.Controls.Count == 0)
        {
          list = new HtmlGenericControl("ul");
          list.ID = "year" + date.Year.ToString();

          year = new HtmlGenericControl("li");
          year.Attributes.Add("class", "year");
          year.Attributes.Add("onclick", "ToggleMonth('year" + date.Year + "')");
          year.InnerHtml = date.Year.ToString();
          year.Controls.Add(list);

          if (date.Year == DateTime.Now.Year)
            list.Attributes.Add("class", "open");
         
          ul.Controls.AddAt(0, year);
        }

        HtmlGenericControl li = new HtmlGenericControl("li");

        HtmlAnchor anc = new HtmlAnchor();
        anc.HRef = VirtualPathUtility.ToAbsolute("~/") + "?year=" + date.Year + "&amp;month=" + date.Month;
        anc.InnerHtml = DateTime.Parse(date.Year + "-" + date.Month + "-01").ToString("MMMM") + " (" + _Months[date] + ")";

        li.Controls.Add(anc);
        list.Controls.AddAt(0, li);
      }

      System.IO.StringWriter sw = new System.IO.StringWriter();
      ul.RenderControl(new HtmlTextWriter(sw));
      return sw.ToString();
    }

    private SortedDictionary<string, Guid> SortGategories(Dictionary<Guid, string> categories)
    {
      SortedDictionary<string, Guid> dic = new SortedDictionary<string, Guid>();
      foreach (Guid key in categories.Keys)
      {
        dic.Add(categories[key], key);
      }

      return dic;
    }

    /// <summary>
    /// Renders the control.
    /// </summary>
    public override void RenderControl(HtmlTextWriter writer)
    {
      if (Post.Posts.Count > 0)
      {
        string html = RenderMonths();
        writer.Write(html);
        writer.Write(Environment.NewLine);
      }
    }
  }
}