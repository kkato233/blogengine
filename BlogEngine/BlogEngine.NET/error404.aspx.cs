using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core;
using System.Collections.Generic;

public partial class error404 : BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (Request.UrlReferrer == null)
    {
      divDirectHit.Visible = true;
    }
    else if (Request.UrlReferrer.Host == Request.Url.Host)
    {
      divInternalReferrer.Visible = true;
    }
    else if (GetSearchKey() != string.Empty)
    {
      SearchTerm = GetSearchTerm(GetSearchKey());
      BindSearchResult();
      divSearchEngine.Visible = true;
    }
    else if (Request.UrlReferrer != null)
    {
      divExternalReferrer.Visible = true;
    }
  }

  private void BindSearchResult()
  {
    List<Post> posts = Search.Hits(Post.Posts, SearchTerm, false);
    int max = 1;
    foreach (Post post in posts)
    {
      HtmlAnchor link = new HtmlAnchor();
      link.InnerHtml = post.Title;
      link.HRef = post.RelativeLink.ToString();
      divSearchResult.Controls.Add(link);

      if (!string.IsNullOrEmpty(post.Description))
      {
        HtmlGenericControl desc = new HtmlGenericControl("span");
        desc.InnerHtml = post.Description;

        divSearchResult.Controls.Add(new LiteralControl("<br />"));
        divSearchResult.Controls.Add(desc);
      }

      divSearchResult.Controls.Add(new LiteralControl("<br />"));
      max++;
      if (max == 3)
        break;
    }
  }

  protected string SearchTerm = string.Empty;

  private string GetSearchKey()
  {
    string referrer = Request.UrlReferrer.Host.ToLowerInvariant();
    if (referrer.Contains("google.") && referrer.Contains("q="))
      return "q=";

    if (referrer.Contains("yahoo.") && referrer.Contains("p="))
      return "p=";

    if (referrer.Contains("q="))
      return "q=";

    return string.Empty;
  }

  /// <summary>
  /// Retrieves the search term from the specified referrer string.
  /// </summary>
  private string GetSearchTerm(string key)
  {
    string referrer = Request.UrlReferrer.ToString();
    int start = referrer.IndexOf(key) + key.Length;
    int stop = referrer.IndexOf("&", start);
    if (stop == -1)
      stop = referrer.Length;

    string term = referrer.Substring(start, stop - start);
    return term.Replace("+", " ");
  }
}
