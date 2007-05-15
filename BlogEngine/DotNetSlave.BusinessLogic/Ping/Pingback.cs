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
    public static void Send(string sourceUrl, string targetUrl)
    {
      if (string.IsNullOrEmpty(sourceUrl) || string.IsNullOrEmpty(targetUrl))
        return;

      try
      {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUrl);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string pingUrl = response.Headers["x-pingback"];
        
        if (!string.IsNullOrEmpty(pingUrl))
        {
          request = (HttpWebRequest)HttpWebRequest.Create(pingUrl);
          request.Method = "POST";
          request.Timeout = 10000;
          request.ContentType = "text/xml";
          request.ProtocolVersion = HttpVersion.Version11;
          AddXmlToRequest(sourceUrl, targetUrl, request);
          request.BeginGetResponse(EndGetResponse, request);
          //request.GetResponse();
        }
      }
      catch
      {
      }
    }

    private static void EndGetResponse(IAsyncResult result)
    {
      HttpWebRequest request = (HttpWebRequest)result.AsyncState;
      request.EndGetResponse(result);
    }

    /// <summary>
    /// Adds the XML to web request. The XML is the standard
    /// XML used by RPC-XML requests.
    /// </summary>
    private static void AddXmlToRequest(string sourceUrl, string targetUrl, HttpWebRequest webreqPing)
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
