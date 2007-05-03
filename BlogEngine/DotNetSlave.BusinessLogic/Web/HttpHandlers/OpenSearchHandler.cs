#region Using

using System;
using System.Xml;
using System.Web;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// Displays the open search XML provider as
  /// descriped at http://opensearch.a9.com/
  /// </summary>
  public class OpenSearchHandler : IHttpHandler
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
      using (XmlWriter writer = XmlWriter.Create(context.Response.OutputStream))
      {
        writer.WriteStartElement("OpenSearchDescription", "http://a9.com/-/spec/opensearch/1.1/");

        writer.WriteElementString("ShortName", BlogSettings.Instance.Name);
        writer.WriteElementString("Description", BlogSettings.Instance.Description);

        writer.WriteRaw("<Image height=\"16\" width=\"16\" type=\"image/vnd.microsoft.icon\">" + Utils.AbsoluteWebRoot.ToString() + "pics/favicon.ico</Image>");

        writer.WriteStartElement("Url");
        writer.WriteAttributeString("type", "text/html");
        writer.WriteAttributeString("template", Utils.AbsoluteWebRoot.ToString() + "?q={searchTerms}");

        writer.WriteStartElement("Url");
        writer.WriteAttributeString("type", "application/rss+xml");
        writer.WriteAttributeString("template", Utils.AbsoluteWebRoot.ToString() + "syndication.axd?q={searchTerms}");

        writer.WriteEndElement();
      }

      context.Response.ContentType = "text/xml";
      context.Response.Cache.SetValidUntilExpires(true);
      context.Response.Cache.SetExpires(DateTime.Now.AddDays(30));
      context.Response.Cache.SetCacheability(HttpCacheability.Public);
      context.Response.Cache.SetLastModified(DateTime.Now);
      context.Response.Cache.SetETag(Guid.NewGuid().ToString());

      BlogSettings.Changed += delegate { context.Response.Cache.SetExpires(DateTime.Now.AddDays(30)); };
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsReusable
    {
      get { return false; }
    }

  }
}