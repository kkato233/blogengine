#region Using

using System;
using BlogEngine.Core;

#endregion

public partial class _default : BlogEngine.Core.Web.Controls.BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (Page.IsCallback)
      return;
    
    if (Request.RawUrl.ToLowerInvariant().Contains("/category/"))
    {
      DisplayCategories();
    }
    else if (Request.RawUrl.ToLowerInvariant().Contains("/author/"))
    {
      DisplayAuthors();
    }
    else if (Request.RawUrl.ToLowerInvariant().Contains("?tag="))
    {
      DisplayTags();
    }
    else if (Request.QueryString.Count == 0 || !string.IsNullOrEmpty(Request.QueryString["page"]))
    {
      PostList1.Posts = Post.Posts;
      Page.Title = BlogSettings.Instance.Name + " - " + BlogSettings.Instance.Description;
    }
    else if (!string.IsNullOrEmpty(Request.QueryString["q"]))
    {
      DisplaySearch();
    }
    else
    {
      DisplayDateRange();
    }

    base.AddMetaTag("description", BlogSettings.Instance.Description);
    base.AddMetaTag("author", BlogSettings.Instance.AuthorName);
    AddMetaKeywords();
    base.AddMicroSummary(string.Empty);
  }

  /// <summary>
  /// Adds the post's tags as meta keywords.
  /// </summary>
  private void AddMetaKeywords()
  {
    if (CategoryDictionary.Instance.Count > 0)
    {
      string[] categories = new string[CategoryDictionary.Instance.Count];
      CategoryDictionary.Instance.Values.CopyTo(categories, 0);      
      base.AddMetaTag("keywords", string.Join(",", categories));
    }
  }

  private void DisplaySearch()
  {
    bool includeComments = Request.QueryString["comment"] != null;
    PostList1.Posts = Search.Hits(Post.Posts, Request.QueryString["q"], includeComments);
    Page.Title = "Search result for '" + Server.HtmlEncode(Request.QueryString["q"]) + "'";
    AddMetaTag("description", BlogSettings.Instance.Description);
  }

  private void DisplayCategories()
  {
    if (!String.IsNullOrEmpty(Request.QueryString["id"]))
    {
      Guid categoryId = new Guid(Request.QueryString["id"]);
      PostList1.Posts = Post.GetPostsByCategory(categoryId);
      Page.Title = BlogSettings.Instance.Name + " - " + CategoryDictionary.Instance[categoryId];
    }
  }

  private void DisplayAuthors()
  {
    if (!string.IsNullOrEmpty(Request.QueryString["name"]))
    {
      PostList1.Posts = Post.GetPostsByAuthor(Request.QueryString["name"]); ;
      Title = BlogSettings.Instance.Name + " - All posts by " + Request.QueryString["name"];      
    }
  }

  private void  DisplayTags()
  {
    if (!string.IsNullOrEmpty(Request.QueryString["tag"]))
    {
      PostList1.Posts = Post.GetPostsByTag(Request.QueryString["tag"].Substring(1)); ;
      base.Title = BlogSettings.Instance.Name + " - All posts tagged '" + Request.QueryString["tag"].Substring(1) + "'";
      base.AddMetaTag("description", BlogSettings.Instance.Description);
    }
  }

  private void DisplayDateRange()
  {
    if (!string.IsNullOrEmpty(Request.QueryString["year"]) && !string.IsNullOrEmpty(Request.QueryString["month"]))
    {
      DateTime dateFrom = DateTime.Parse(Request.QueryString["year"] + "-" + Request.QueryString["month"] + "-01");
      DateTime dateTo = dateFrom.AddMonths(1).AddDays(-1);
      PostList1.Posts = Post.GetPostsByDate(dateFrom, dateTo);
      Title = BlogSettings.Instance.Name + " - " + dateFrom.ToString("MMMM yyyy");
    }
    else if (!string.IsNullOrEmpty(Request.QueryString["date"]))
    {
      DateTime date = DateTime.Parse(Request.QueryString["date"]);
      PostList1.Posts = Post.GetPostsByDate(date, date);
      Title = BlogSettings.Instance.Name + " - " + date.ToString("MMMM d. yyyy");
    }
    else if (!string.IsNullOrEmpty(Request.QueryString["calendar"]))
    {
      calendar.Visible = true;
      PostList1.Visible = false;
    }
  }

}
