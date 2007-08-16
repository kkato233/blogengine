#region Using

using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Ping
{
  /// <summary>
  /// Sends pingbacks to website that the blog links to.
  /// </summary>
  public static class Pingback
  {

    /// <summary>
    /// Sends pingbacks to the targetUrl.
    /// </summary>
    public static void Send(Uri sourceUrl, Uri targetUrl)
    {
      if (sourceUrl == null || targetUrl == null)
        return;

      try
      {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUrl);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string pingUrl = response.Headers["x-pingback"];
        Uri url;
        if (!string.IsNullOrEmpty(pingUrl) && Uri.TryCreate(pingUrl, UriKind.Absolute, out url))
        {
          request = (HttpWebRequest)HttpWebRequest.Create(url);
          request.Method = "POST";
          request.Timeout = 10000;
          request.ContentType = "text/xml";
          request.ProtocolVersion = HttpVersion.Version11;
          AddXmlToRequest(sourceUrl, targetUrl, request);
          request.GetResponse();
        }
      }
      catch (Exception)
      {
        // Stops unhandled exceptions that can cause the app pool to recycle
      }
    }

    /// <summary>
    /// Adds the XML to web request. The XML is the standard
    /// XML used by RPC-XML requests.
    /// </summary>
    private static void AddXmlToRequest(Uri sourceUrl, Uri targetUrl, HttpWebRequest webreqPing)
    {
      Stream stream = (Stream)webreqPing.GetRequestStream();
      using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.ASCII))
      {
        writer.WriteStartDocument(true);
        writer.WriteStartElement("methodCall");
        writer.WriteElementString("methodName", "pingback.ping");
        writer.WriteStartElement("params");
        
        writer.WriteStartElement("param");        
        writer.WriteStartElement("value");
        writer.WriteElementString("string", sourceUrl.ToString());
        writer.WriteEndElement();        
        writer.WriteEndElement();

        writer.WriteStartElement("param");
        writer.WriteStartElement("value");
        writer.WriteElementString("string", targetUrl.ToString());
        writer.WriteEndElement();
        writer.WriteEndElement();

        writer.WriteEndElement();
        writer.WriteEndElement();
      }
    }
  }
}
