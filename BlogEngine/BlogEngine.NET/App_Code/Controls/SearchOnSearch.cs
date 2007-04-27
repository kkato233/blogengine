#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Collections.Generic;
using BlogEngine.Core;

#endregion

namespace Controls
{
  /// <summary>
  /// If the visitor arrives through a search engine, this control
  /// will display an in-site search result based on the same search term.
  /// </summary>
  public class SearchOnSearch : Control
  {

    #region Using

    private int _MaxResults;
    /// <summary>
    /// Gets or sets the maximum numbers of results to display.
    /// </summary>
    public int MaxResults
    {
      get { return _MaxResults; }
      set { _MaxResults = value; }
    }

    private string _Headline;
    /// <summary>
    /// Gets or sets the text of the headline.
    /// </summary>
    public string Headline
    {
      get { return _Headline; }
      set { _Headline = value; }
    }

    private string _Text;
    /// <summary>
    /// Gets or sets the text displayed below the headline.
    /// </summary>
    public string Text
    {
      get { return _Text; }
      set { _Text = value; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Checks the referrer to see if it qualifies as a search.
    /// </summary>
    private string Html()
    {
      if (Context.Request.UrlReferrer != null && !Context.Request.UrlReferrer.ToString().Contains(Utils.AbsoluteWebRoot.ToString()) && IsSearch)
      {
        string referrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLowerInvariant();
        string searchTerm = GetSearchTerm(referrer);
        List<Post> posts = Search.Hits(Post.Posts, searchTerm, false);
        if (posts.Count == 0)
          return null;

        return WriteHtml(posts, searchTerm);
      }

      return null;
    }

    /// <summary>
    /// Writes the search results as HTML.
    /// </summary>
    private string WriteHtml(List<Post> posts, string searchTerm)
    {
      int results = MaxResults < posts.Count ? MaxResults : posts.Count;
      StringBuilder sb = new StringBuilder();
      sb.Append("<div id=\"searchonsearch\">");
      sb.AppendFormat("<h3>{0} '{1}'</h3>", Headline, searchTerm);
      sb.AppendFormat("<p>{0}</p>", Text);
      sb.Append("<ol>");

      for (int i = 0; i < results; i++)
      {
        sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", posts[i].RelativeLink, posts[i].Title);
      }

      sb.Append("</ol>");
      sb.Append("</div>");

      return sb.ToString();
    }

    /// <summary>
    /// Retrieves the search term from the specified referrer string.
    /// </summary>
    private string GetSearchTerm(string referrer)
    {
      int start = referrer.IndexOf("q=") + 2;
      int stop = referrer.IndexOf("&", start);
      if (stop == -1)
        stop = referrer.Length;

      string term = referrer.Substring(start, stop - start);
      return term.Replace("+", " ");
    }

    /// <summary>
    /// Checks the referrer to see if it is from a search engine.
    /// </summary>
    private bool IsSearch
    {
      get
      {
        string referrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLowerInvariant();
        return referrer.Contains("q=");
      }
    }

    #endregion

    /// <summary>
    /// Renders the control as a script tag.
    /// </summary>
    public override void RenderControl(HtmlTextWriter writer)
    {
      string html = Html();
      if (html != null)
        writer.Write(html);
    }
  }
}