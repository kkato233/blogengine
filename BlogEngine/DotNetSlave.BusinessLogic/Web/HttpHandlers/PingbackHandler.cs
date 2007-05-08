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
  /// Recieves pingbacks from other blogs and websites, and 
  /// registers them as a comment.
  /// </summary>
  public class PingbackHandler : IHttpHandler
  {

    #region Private fields

    private static Regex _Regex = new Regex(@"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private string _Title;
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
      string html = string.Empty;
      try
      {
        string xml = ParseRequest(context);
        html = xml;
        if (!xml.Contains("<methodName>pingback.ping</methodName>"))
          return;

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        XmlNodeList list = doc.SelectNodes("methodCall/params/param/value/string");
        string sourceUrl = list[0].InnerText.Trim();
        string targetUrl = list[1].InnerText.Trim();

        ExamineSourcePage(sourceUrl, targetUrl);

        Post post = GetPostByUrl(targetUrl);
        if (post != null && IsFirstPingBack(post, sourceUrl) && _SourceHasLink)
        {
          AddComment(sourceUrl, post);
          context.Response.Write("OK");
        }
      }
      catch
      {
        //StreamWriter writer = new StreamWriter(context.Server.MapPath("~/app_data/error.txt"), true);
        //writer.WriteLine(ex.Message);
        //writer.WriteLine(ex.StackTrace);
        //writer.WriteLine();
        //writer.WriteLine(html);
        //writer.Flush();
        //writer.Dispose();
        context.Response.Write("ERROR");
      }
    }

    /// <summary>
    /// Insert the pingback as a comment on the post.
    /// </summary>
    private void AddComment(string sourceUrl, Post post)
    {
      Comment comment = new Comment();
      comment.Author = GetDomain(sourceUrl);
      comment.Website = new Uri(sourceUrl);
      comment.Content = "Pingback from " + comment.Author + Environment.NewLine + Environment.NewLine + _Title;
      comment.DateCreated = DateTime.Now;
      comment.Email = "pingback";
      comment.IP = "n/a";

      post.Comments.Add(comment);
      post.Save();
    }

    /// <summary>
    /// Retrieves the content of the input stream
    /// and return it as plain text.
    /// </summary>
    private static string ParseRequest(HttpContext context)
    {
      byte[] buffer = new byte[context.Request.InputStream.Length];
      context.Request.InputStream.Read(buffer, 0, buffer.Length);

      return System.Text.Encoding.Default.GetString(buffer);
    }

    /// <summary>
    /// Parse the source URL to get the domain.
    /// It is used to fill the Author property of the comment.
    /// </summary>
    private static string GetDomain(string sourceUrl)
    {
      int start = sourceUrl.IndexOf("://") + 3;
      int stop = sourceUrl.IndexOf("/", start);
      return sourceUrl.Substring(start, stop - start).Replace("www.", string.Empty);
    }

    /// <summary>
    /// Checks to see if the source has already pinged the target.
    /// If it has, there is no reason to add it again.
    /// </summary>
    private bool IsFirstPingBack(Post post, string sourceUrl)
    {
      foreach (Comment comment in post.Comments)
      {
        if (comment.Website != null && comment.Website.ToString().Equals(sourceUrl, StringComparison.OrdinalIgnoreCase))
          return false;
      }

      return true;
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
    /// Retrieve the post that belongs to the target URL.
    /// </summary>
    private static Post GetPostByUrl(string url)
    {
      int start = url.LastIndexOf("/") + 1;
      int stop = url.ToLowerInvariant().IndexOf(".aspx");
      string name = url.Substring(start, stop - start).ToLowerInvariant();

      foreach (Post post in Post.Posts)
      {
        string legalTitle = Utils.RemoveIlegalCharacters(post.Title).ToLowerInvariant();
        if (name == legalTitle)
        {
          return post;
        }
      }

      return null;
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

  }
}