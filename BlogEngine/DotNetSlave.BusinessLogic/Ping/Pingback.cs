#region Using

using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using DotNetSlave.BlogEngine.BusinessLogic;

#endregion

namespace BlogEngine.Core.Ping
{
  /// <summary>
  /// Sends pingbacks to website that the blog links to.
  /// </summary>
  public static class Pingback
  {
    /// <summary>
    /// Sends pingbacks to all external links in the specified post.
    /// </summary>
    public static bool Send(Post post)
    {
      if (post == null)
        throw new ArgumentNullException("post");

      Regex regex = new Regex("href\\s*=\\s*(?:(?:\\\"(?<url>[^\\\"]*)\\\")|(?<url>[^\\s]* ))"); 
      foreach (Match match in regex.Matches(post.Content))
      {
        Send(post.AbsoluteLink.ToString(), match.Groups[1].Value);
      }

      return true;
    }

    private static bool Send(string sourceUrl, string targetUrl)
    {
      if (string.IsNullOrEmpty(sourceUrl) || string.IsNullOrEmpty(targetUrl))
        return false;

      try
      {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUrl);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string pingUrl = response.Headers["x-pingback"];
        
        if (string.IsNullOrEmpty(pingUrl))
        {
          request = (HttpWebRequest)HttpWebRequest.Create(pingUrl);
          request.Method = "POST";
          request.Timeout = 3000;
          AddXmlToRequest(sourceUrl, targetUrl, request);
          request.GetResponse();
          return true;
        }

        return false;
      }
      catch (Exception)
      {
        return false;
      }
    }

    /// <summary>
    /// Adds the XML to web request. The XML is the standard
    /// XML used by RPC-XML requests.
    /// </summary>
    private static void AddXmlToRequest(string sourceUrl, string targetUrl, HttpWebRequest webreqPing)
    {
      Stream stream = (Stream)webreqPing.GetRequestStream();
      using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
      {
        writer.WriteStartDocument();
        writer.WriteStartElement("methodCall");
        writer.WriteElementString("methodName", "pingback.ping");
        writer.WriteStartElement("params");
        
        writer.WriteStartElement("param");        
        writer.WriteStartElement("value");
        writer.WriteElementString("string", sourceUrl);
        writer.WriteEndElement();        
        writer.WriteEndElement();

        writer.WriteStartElement("param");
        writer.WriteStartElement("value");
        writer.WriteElementString("string", targetUrl);
        writer.WriteEndElement();
        writer.WriteEndElement();

        writer.WriteEndElement();
        writer.WriteEndElement();
      }
    }
  }
}
