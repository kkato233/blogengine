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

    #region Properties

    private static string _Html;
    private string Html
    {
      get
      {
        if (_Html == null)
        {
          Post.Saved += delegate { _Html = null; };
          HtmlGenericControl ul = BindMonths();
          System.IO.StringWriter sw = new System.IO.StringWriter();
          ul.RenderControl(new HtmlTextWriter(sw));
          _Html = sw.ToString();
        }

        return _Html;
      }
    }

    #endregion

    private HtmlGenericControl BindMonths()
    {
      HtmlGenericControl ul = new HtmlGenericControl("ul");
      

      DateTime first = DateTime.Parse( Post.Posts[Post.Posts.Count -1].DateCreated.Date.ToString("yyyy-MM-") + "01");
      int year = first.Year;
      int month = first.Month;      

      while (first <= DateTime.Now)
      {
        List<Post> list = Post.GetPostsByDate(first, DateTime.Parse(first.Year + "-" + first.Month + "-01").AddMonths(1).AddDays(-1));
        if (list.Count > 0)
        {
          HtmlGenericControl li = new HtmlGenericControl("li");

          HtmlAnchor anc = new HtmlAnchor();
          anc.HRef = VirtualPathUtility.ToAbsolute("~/") + "?year=" + first.Year + "&amp;month=" + first.Month;
          anc.InnerHtml = DateTime.Parse(first.Year + "-" + first.Month + "-01").ToString("MMMM") + " " + first.Year + " (" + list.Count + ")";

          li.Controls.Add(anc);
          ul.Controls.AddAt(0, li);          
        }

        first = first.AddMonths(1);
      }

      return ul;
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
      writer.Write(Html);
      writer.Write(Environment.NewLine);
    }
  }
}