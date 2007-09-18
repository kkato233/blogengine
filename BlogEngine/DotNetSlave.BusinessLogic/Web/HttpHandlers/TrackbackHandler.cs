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

    //private static Regex _Regex = new Regex(@"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    //private string _Title;
    private bool _SourceHasLink;

    #endregion

    /// <summary>
    /// Enables processing of HTTP Web requests by a custom HttpHandler that 
    /// implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
    /// </summary>
    /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> 
    /// object that provides references to the intrinsic server objects 
    /// (for example, Request, Response, Session, and Server) used to service HTTP requests.
    /// </param>
    public void ProcessRequest(HttpContext context)
    {
      OnReceived();
      string postId = context.Request.Params["id"]; ;
      string title = context.Request.Params["title"];
      string excerpt = context.Request.Params["excerpt"];
      string blog_name = context.Request.Params["blog_name"];
      string url = string.Empty;

      if (context.Request.Params["url"] != null)
        url = context.Request.Params["url"].Split(',')[0];

      if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(postId))
      {
        Post post = Post.GetPost(new Guid(postId));
        ExamineSourcePage(url, post.AbsoluteLink.ToString());

        if (post != null && IsFirstPingBack(post, url) && _SourceHasLink)
        {
          AddComment(url, post, blog_name, title);
          OnAccepted(url);
          context.Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>0</error></response>");
          context.Response.End();
        }
        else if (!IsFirstPingBack(post, url))
        {
          OnRejected(url);
          context.Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>Trackback already registered</error></response>");
          context.Response.End();
        }
        else if (!_SourceHasLink)
        {
          OnSpammed(url);
          context.Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>The source page does not link</error></response>");
          context.Response.End();
        }
      }
      else
      {
        context.Response.Redirect("~/");
      }
    }

    /// <summary>
    /// Insert the pingback as a comment on the post.
    /// </summary>
    private static void AddComment(string sourceUrl, Post post, string blogName, string excerpt)
    {
      Comment comment = new Comment();
      comment.Id = Guid.NewGuid();
      comment.Author = blogName;
      comment.Website = new Uri(sourceUrl);
      comment.Content = "Trackback from " + comment.Author + Environment.NewLine + Environment.NewLine + excerpt;
      comment.Email = "trackback";
      comment.Post = post;
      comment.DateCreated = DateTime.Now;
      comment.IP = HttpContext.Current.Request.UserHostAddress;
      comment.IsApproved = true; //NOTE: Trackback comments are approved by default 
      post.AddComment(comment);
    }

    /// <summary>
    /// Parse the HTML of the source page.
    /// </summary>
    private void ExamineSourcePage(string sourceUrl, string targetUrl)
    {
      try
      {
        using (WebClient client = new WebClient())
        {
          client.Credentials = CredentialCache.DefaultNetworkCredentials;
          string html = client.DownloadString(sourceUrl);
          _SourceHasLink = html.ToLowerInvariant().Contains(targetUrl.ToLowerInvariant());
        }
      }
      catch (WebException ex)
      {
        _SourceHasLink = false;
        throw new ArgumentException("Trackback sender does not exists: " + sourceUrl, ex);
      }
    }

    /// <summary>
    /// Checks to see if the source has already pinged the target.
    /// If it has, there is no reason to add it again.
    /// </summary>
    private static bool IsFirstPingBack(Post post, string sourceUrl)
    {
      foreach (Comment comment in post.Comments)
      {
        if (comment.Website != null && comment.Website.ToString().Equals(sourceUrl, StringComparison.OrdinalIgnoreCase))          
            return false;

          if (comment.IP != null && comment.IP == HttpContext.Current.Request.UserHostAddress)
            return false;
      }

      return true;
    }

    /// <summary>
    /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
    /// </summary>
    /// <value></value>
    /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
    public bool IsReusable
    {
      get { return true; }
    }

    #region Events

    /// <summary>
    /// Occurs when a hit is made to the trackback.axd handler.
    /// </summary>
    public static event EventHandler<EventArgs> Received;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    protected virtual void OnReceived()
    {
      if (Received != null)
      {
        Received(null, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Occurs when a trackback is accepted as valid and added as a comment.
    /// </summary>
    public static event EventHandler<EventArgs> Accepted;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    protected virtual void OnAccepted(string url)
    {
      if (Accepted != null)
      {
        Accepted(url, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Occurs when a trackback request is rejected because the sending
    /// website already made a trackback or pingback to the specific page.
    /// </summary>
    public static event EventHandler<EventArgs> Rejected;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    protected virtual void OnRejected(string url)
    {
      if (Accepted != null)
      {
        Accepted(url, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Occurs when the request comes from a spammer.
    /// </summary>
    public static event EventHandler<EventArgs> Spammed;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    protected virtual void OnSpammed(string url)
    {
      if (Spammed != null)
      {
        Spammed(url, EventArgs.Empty);
      }
    }

    #endregion

  }
}