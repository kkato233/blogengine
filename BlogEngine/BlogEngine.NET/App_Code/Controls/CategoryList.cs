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
  public class CategoryList : Control
  {

    #region Properties

    private bool _ShowRssIcon = true;
    /// <summary>
    /// Gets or sets whether or not to show feed icons next to the category links.
    /// </summary>
    public bool ShowRssIcon
    {
      get { return _ShowRssIcon; }
      set { _ShowRssIcon = value; }
    }

    private static string _Html;
    private string Html
    {
      get
      {
        if (_Html == null)
        {
          Post.Saved += delegate { _Html = null; };
          CategoryDictionary.Saved += delegate { _Html = null; };
          HtmlGenericControl ul = BindCategories();
          System.IO.StringWriter sw = new System.IO.StringWriter();
          ul.RenderControl(new HtmlTextWriter(sw));
          _Html = sw.ToString();
        }

        return _Html;
      }
    }

    #endregion

    private HtmlGenericControl BindCategories()
    {
      HtmlGenericControl ul = new HtmlGenericControl("ul");
      SortedDictionary<string, Guid> dic = SortGategories(CategoryDictionary.Instance);
      foreach (string key in dic.Keys)
      {
        HtmlGenericControl li = new HtmlGenericControl("li");

        if (ShowRssIcon)
        {
          HtmlImage img = new HtmlImage();
          img.Src = Utils.RelativeWebRoot + "pics/rssButton.gif";
          img.Alt = "RSS feed for " + key;
          img.Attributes["class"] = "rssButton";

          HtmlAnchor feedAnchor = new HtmlAnchor();
          feedAnchor.HRef = VirtualPathUtility.ToAbsolute("~/") + "category/rss.axd?category=" + dic[key].ToString();
          feedAnchor.Attributes["rel"] = "nofollow";
          feedAnchor.Controls.Add(img);

          li.Controls.Add(feedAnchor);
        }

        HtmlAnchor anc = new HtmlAnchor();
        anc.HRef = VirtualPathUtility.ToAbsolute("~/") + "category/" + Utils.RemoveIlegalCharacters(key) + ".aspx";
        anc.InnerHtml = key + " (" + Post.GetPostsByCategory(dic[key]).Count + ")";
        anc.Attributes["rel"] = "tag";
        
        li.Controls.Add(anc);
        ul.Controls.Add(li);
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