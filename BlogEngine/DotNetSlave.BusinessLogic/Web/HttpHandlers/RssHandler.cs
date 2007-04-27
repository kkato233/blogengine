#region Using

using System;
using System.Xml;
using System.Web;
using System.Collections.Generic;
using System.Text;
using BlogEngine.Core.Entities;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{

  /// <summary>
  /// 
  /// </summary>
  public class RssHandler : IHttpHandler
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
      List<Post> posts;
      if (!string.IsNullOrEmpty(context.Request.QueryString["category"]) && context.Request.QueryString["category"].Length == 36)
      {
        posts = Post.GetPostsByCategory(new Guid(context.Request.QueryString["category"]));
      }
      else if (!string.IsNullOrEmpty(context.Request.QueryString["author"]))
      {
        posts = Post.GetPostsByAuthor(context.Request.QueryString["author"]);
      }
      //else if (!string.IsNullOrEmpty(context.Request.QueryString["q"]))
      //{
      //  posts = Search.Hits(Post.Posts, context.Request.QueryString["q"], false);
      //}
      else
      {
        posts = Post.Posts;
      }

      int count = BlogSettings.Instance.PostsPerPage;
      if (count > posts.Count)
        count = posts.Count;

      posts = posts.GetRange(0, count);


      if (posts != null)
      {
        SetHeaders(context, posts);
        CreateRSS(context, posts);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="posts"></param>
    private void CreateRSS(HttpContext context, List<Post> posts)
    {
      string path = context.Request.Url.ToString().Substring(0, context.Request.Url.ToString().IndexOf("rss.axd"));
      string title = BlogSettings.Instance.Name;

      using (XmlTextWriter rss = new XmlTextWriter(context.Response.OutputStream, Encoding.UTF8))
      {
        rss.Formatting = Formatting.Indented;
        rss.WriteStartDocument();

        // The mandatory rss tag
        rss.WriteStartElement("rss");
        rss.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
        rss.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        rss.WriteAttributeString("xmlns:wfw", "http://wellformedweb.org/CommentAPI/");
        rss.WriteAttributeString("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");
        rss.WriteAttributeString("version", "2.0");

        // The channel tag contains RSS feed details
        rss.WriteStartElement("channel");
        rss.WriteElementString("title", title);
        rss.WriteElementString("language", "en-gb");
        rss.WriteElementString("description", BlogSettings.Instance.Description);
        rss.WriteElementString("link", path);

        // Loop through the posts and add them to the RSS feed
        foreach (Post item in posts)
        {
          if (!item.IsPublished)
            continue;

          //string link = item.AbsoluteLink.ToString(); //path + "post/" + Utils.RemoveIlegalCharacters(item.Title) + ".aspx";
          //string perma = path + "post.aspx?id=" + item.Id.ToString();

          rss.WriteStartElement("item");
          rss.WriteElementString("wfw:comment", item.AbsoluteLink.ToString() + "#comments");
          //rss.WriteElementString("wfw:commentRss", path + "commentrss.ashx?post=" + item.Id.ToString());
          rss.WriteElementString("slash:comments", item.Comments.Count.ToString());

          rss.WriteElementString("guid", item.PermaLink.ToString());
          rss.WriteElementString("title", item.Title);
          rss.WriteElementString("description", MakeReferencesAbsolute(item.Content));
          if (item.Categories.Count > 0)
            rss.WriteElementString("category", CategoryDictionary.Instance[item.Categories[0]]);
          rss.WriteElementString("link", item.AbsoluteLink.ToString());
          rss.WriteElementString("comments", item.AbsoluteLink.ToString() + "#comments");
          rss.WriteElementString("pubDate", GetRFC822Date(item.DateCreated));
          rss.WriteEndElement();
        }

        // Close all tags
        rss.WriteEndElement();
        rss.WriteEndElement();
        rss.WriteEndDocument();
      }
    }

    private string MakeReferencesAbsolute(string content)
    {
      content = content.Replace("\"/image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
      content = content.Replace("\"/file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");
      return content;
    }

    /// <summary>
    /// Converts a regular DateTime to a RFC822 date string.
    /// </summary>
    /// <returns>The specified date formatted as a RFC822 date string.</returns>
    private static string GetRFC822Date(DateTime date)
    {
      int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours - 1;
      string timeZone = "+" + offset.ToString().PadLeft(2, '0');

      if (offset < 0)
      {
        int i = offset * -1;
        timeZone = "-" + i.ToString().PadLeft(2, '0');
      }

      return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'), System.Globalization.CultureInfo.GetCultureInfo("en-us"));
    }

    private void SetHeaders(HttpContext context, List<Post> posts)
    {
      DateTime lastModified = posts[0].DateModified;
      bool notModified = false;

      // check the etag value first. if it matches then 
      // we send a 304. If not, check the if-modified-since
      string etag = context.Request.Headers["If-None-Match"];
      if (etag != null)
      {
        notModified = (etag.Equals(lastModified.Ticks.ToString()));
      }
      else
      {
        string ifModifiedSince = context.Request.Headers["if-modified-since"];

        if (ifModifiedSince != null)
        {
          // ifModifiedSince can have a legnth param in there
          // If-Modified-Since: Wed, 29 Dec 2004 18:34:27 GMT; length=126275
          if (ifModifiedSince.IndexOf(";") > -1)
          {
            ifModifiedSince = ifModifiedSince.Split(';').GetValue(0).ToString();
          }

          DateTime ifModDate = DateTime.Parse(ifModifiedSince);

          notModified = (lastModified <= ifModDate);
        }
      }

      if (notModified)
      {
        context.Response.StatusCode = 304;
        context.Response.SuppressContent = true;
        context.Response.End();
      }
      else
      {
        context.Response.Cache.SetCacheability(HttpCacheability.Public);
        context.Response.Cache.SetLastModified(lastModified);
        context.Response.Cache.SetETag(lastModified.Ticks.ToString());
        context.Response.ContentType = "application/rss+xml";
        context.Response.AppendHeader("Content-Disposition", "inline; filename=rss.xml");
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsReusable
    {
      get
      {
        return false;
      }
    }

  }
}