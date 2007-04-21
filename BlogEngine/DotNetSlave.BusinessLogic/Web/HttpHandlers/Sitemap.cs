#region Using

using System;
using System.Xml;
using System.Web;
using BlogEngine.Core.Entities;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// A blog sitemap suitable for Google Sitemap.
  /// </summary>
  public class Sitemap : IHttpHandler
  {

    public void ProcessRequest(HttpContext context)
    {
      using (XmlWriter writer = XmlWriter.Create(context.Response.OutputStream))
      {
        writer.WriteStartElement("urlset", "http://www.google.com/schemas/sitemap/0.84");

        foreach (Post post in Post.Posts)
        {
          writer.WriteStartElement("url");
          writer.WriteElementString("loc", post.AbsoluteLink.ToString());
          writer.WriteElementString("lastmod", post.DateModified.ToString("yyyy-MM-dd"));
          writer.WriteElementString("changefreq", "monthly");
          writer.WriteEndElement();
        }

        foreach (Page page in Page.Pages)
        {
          writer.WriteStartElement("url");
          writer.WriteElementString("loc", page.AbsoluteLink.ToString());
          writer.WriteElementString("lastmod", page.DateModified.ToString("yyyy-MM-dd"));
          writer.WriteElementString("changefreq", "monthly");
          writer.WriteEndElement();
        }

        writer.WriteEndElement();
      }

      context.Response.ContentType = "text/xml";
    }

    public bool IsReusable
    {
      get { return false; }
    }

  }
}