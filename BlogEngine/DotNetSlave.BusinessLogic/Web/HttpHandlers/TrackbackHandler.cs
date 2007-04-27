#region Using

using System;
using System.IO;
using System.Web;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// Summary description for TrackbackHandler
  /// </summary>
  public class TrackbackHandler : IHttpHandler
  {

    #region Private fields

    private static Regex _Regex = new Regex(@"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private string _Title;
    private bool _SourceHasLink;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
      string title = string.Empty;
      string excerpt = string.Empty;
      string url = string.Empty;
      string blog_name = string.Empty;

      title = context.Request.Params["title"];
      excerpt = context.Request.Params["excerpt"];
      blog_name = context.Request.Params["blog_name"];
      if (context.Request.Params["url"] != null)
        url = context.Request.Params["url"].Split(',')[0];

      if (!string.IsNullOrEmpty(title))
      {
        try
        {
          string postId = context.Request.QueryString["id"]; ;
          Post post = Post.GetPost(new Guid(postId));
          ExamineSourcePage(url, post.AbsoluteLink.ToString());

          if (post != null && IsFirstPingBack(post, url) && _SourceHasLink)
          {
            AddComment(url, post, blog_name);

            context.Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>0</error></response>");
            context.Response.End();
          }
        }
        catch (Exception exc)
        {
          context.Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>1</error><message>" + exc.ToString() + "</message></response>");
          context.Response.End();
        }
      }
      else
      {
        string postId = context.Request.QueryString["id"]; ;
        Post post = Post.GetPost(new Guid(postId));
        if (post != null)
          context.Response.Redirect(post.RelativeLink.ToString());
        else
          context.Response.Redirect("~/");
      }
    }

    /// <summary>
    /// Insert the pingback as a comment on the post.
    /// </summary>
    private void AddComment(string sourceUrl, Post post, string blogName)
    {
      Comment comment = new Comment();
      comment.Author = blogName;
      comment.Website = new Uri(sourceUrl);
      comment.Content = "Trackback from " + comment.Author + Environment.NewLine + Environment.NewLine + _Title;
      comment.Email = "trackback";
      comment.DateCreated = DateTime.Now;
      post.Comments.Add(comment);
      post.Save();
    }

    /// <summary>
    /// Parse the HTML of the source page.
    /// </summary>
    private void ExamineSourcePage(string sourceUrl, string targetUrl)
    {
      using (WebClient client = new WebClient())
      {
        string html = client.DownloadString(sourceUrl);
        _Title = _Regex.Match(html).Value.Trim();
        _SourceHasLink = html.ToLowerInvariant().Contains(targetUrl.ToLowerInvariant());
      }
    }

    /// <summary>
    /// Checks to see if the source has already pinged the target.
    /// If it has, there is no reason to add it again.
    /// </summary>
    private bool IsFirstPingBack(Post post, string sourceUrl)
    {
      foreach (Comment comment in post.Comments)
      {
        if (comment.Website.ToString().Equals(sourceUrl, StringComparison.OrdinalIgnoreCase))
          return false;
      }

      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsReusable
    {
      get { return true; }
    }

  }
}