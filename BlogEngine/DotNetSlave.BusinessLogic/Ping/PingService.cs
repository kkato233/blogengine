#region Using

using System;
using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using DotNetSlave.BlogEngine.BusinessLogic;

#endregion

namespace BlogEngine.Core.Ping
{
  /// <summary>
  /// Pings various blog ping services.
  /// <remarks>
  /// Whenever a post is created or updated, it is important
  /// to notify the ping services so that they have the latest
  /// updated posts from the blog.
  /// </remarks>
  /// </summary>
  public class PingService
  {
    /// <summary>
    /// Sends a ping to various ping services asynchronously.
    /// </summary>
    public void Send()
    {
      Execute("http://blogsearch.google.com/ping/RPC2");
      Execute("http://rpc.technorati.com/rpc/ping");
      Execute("http://rpc.pingomatic.com/RPC2");
      Execute("http://ping.feedburner.com");
      Execute("http://api.my.yahoo.com/RPC2");
      Execute("http://api.feedster.com/ping");
      Execute("http://www.bloglines.com/ping");
      Execute("http://rpc.icerocket.com:10080/");
      Execute("http://services.newsgator.com/ngws/xmlrpcping.aspx");
      Execute("http://ping.weblogalot.com/rpc.php");
    }

    /// <summary>
    /// Creates a web request and invokes the response asynchronous.
    /// </summary>
    private void Execute(string url)
    {
      try
      {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "text/xml";
        request.Timeout = 3000;

        AddXmlToRequest(request);
        request.BeginGetResponse(EndGetResponse, request);
      }
      catch (Exception)
      {
        // Log the error.
      }
    }

    /// <summary>
    /// Receives the response.
    /// </summary>
    private void EndGetResponse(IAsyncResult result)
    {
      HttpWebRequest request = (HttpWebRequest)result.AsyncState;
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();

      //string filename = System.Web.HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation) + "ping.txt";

      //StreamReader reade = new StreamReader(response.GetResponseStream());
      //string content = reade.ReadToEnd();

      //StreamWriter writer = new StreamWriter(filename, true);
      //writer.WriteLine(request.RequestUri.ToString() + Environment.NewLine);
      //writer.WriteLine(content);
      //writer.WriteLine("--------------------------------------------------------------------");
      //writer.Flush();
      //writer.Close();
    }

    /// <summary>
    /// Adds the XML to web request. The XML is the standard
    /// XML used by RPC-XML requests.
    /// </summary>
    private static void AddXmlToRequest(HttpWebRequest webreqPing)
    {
      Stream stream = (Stream)webreqPing.GetRequestStream();
      using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
      {
        writer.WriteStartDocument();
        writer.WriteStartElement("methodCall");
        writer.WriteElementString("methodName", "weblogUpdates.ping");
        writer.WriteStartElement("params");
        writer.WriteStartElement("param");
        writer.WriteElementString("value", BlogSettings.Instance.Name);
        writer.WriteEndElement();
        writer.WriteStartElement("param");
        writer.WriteElementString("value", Utils.AbsoluteWebRoot.ToString());
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
    }
  }
}