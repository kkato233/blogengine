#region Using

using System;
using System.Xml;
using System.IO;
using System.Web;
using System.Collections;
using System.Globalization;
using BlogEngine.Core;
using System.Net;

#endregion

namespace BlogEngine.Core.Web.HttpModules
{
  /// <summary>
  /// Summary description for ReferrerModule
  /// </summary>
  public class ReferrerModule : IHttpModule
  {

    #region IHttpModule Members

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
      // Nothing to dispose.
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void Init(HttpApplication context)
    {

      if (BlogSettings.Instance.EnableReferrerTracking)
        context.BeginRequest += new EventHandler(context_BeginRequest);
    }

    #endregion

    private void context_BeginRequest(object sender, EventArgs e)
    {
      HttpContext context = ((HttpApplication)sender).Context;
      if (!context.Request.PhysicalPath.ToLowerInvariant().Contains(".aspx"))
        return;

      if (context.Request.UrlReferrer != null)
      {
        Uri referrer = context.Request.UrlReferrer;
        if (!referrer.Host.Equals(Utils.AbsoluteWebRoot.Host, StringComparison.OrdinalIgnoreCase) && !IsSearchEngine(referrer.ToString()))
        {
          //RegisterClick(referrer.ToString());
          System.Threading.ThreadPool.QueueUserWorkItem(BeginRegisterClick, new DictionaryEntry(referrer.ToString(), context.Request.Url));
        }
      }
    }

    #region Private fields

    /// <summary>
    /// Used to thread safe the file operations
    /// </summary>
    private static object _SyncRoot = new object();

    /// <summary>
    /// The relative path of the XML file.
    /// </summary>
    private static string _Folder = HttpContext.Current.Server.MapPath("~/app_data/log/");

    #endregion

    private bool IsSearchEngine(string referrer)
    {
      return referrer.ToLowerInvariant().Contains("?q=") || referrer.ToLowerInvariant().Contains("&q=");
    }

    private bool IsSpam(string referrer, Uri url)
    {
      try
      {
        using (WebClient client = new WebClient())
        {
          string html = client.DownloadString(referrer).ToUpperInvariant();
          string subdomain = GetSubDomain(url);
          string host = url.Host.ToUpperInvariant();
          
          if (subdomain != null)
            host.Replace(subdomain + ".", string.Empty);

          return !html.Contains(host);
        }
      }
      catch
      {
        return true;
      }
    }

    /// Retrieves the subdomain from the specified URL.
    /// </summary>
    /// <param name="url">The URL from which to retrieve the subdomain.</param>
    /// <returns>The subdomain if it exist, otherwise null.</returns>
    private static string GetSubDomain(Uri url)
    {
      if (url.HostNameType == UriHostNameType.Dns)
      {
        string host = url.Host;
        if (host.Split('.').Length > 2)
        {
          int lastIndex = host.LastIndexOf(".");
          int index = host.LastIndexOf(".", lastIndex - 1);
          return host.Substring(0, index);
        }
      }

      return null;
    }



    private void BeginRegisterClick(object stateInfo)
    {
      DictionaryEntry entry = (DictionaryEntry)stateInfo;
      string referrer = (string)entry.Key;
      Uri url = (Uri)entry.Value;
      bool isSpam = IsSpam(referrer, url);

      RegisterClick(referrer, isSpam);
    }

    private void RegisterClick(string url, bool isSpam)
    {
      string fileName = _Folder + DateTime.Now.Date.ToString("dddd", CultureInfo.InvariantCulture) + ".xml";

      lock (_SyncRoot)
      {
        //if (_Document == null)
        XmlDocument doc = CreateDocument(fileName);

        string address = HttpUtility.HtmlEncode(url);
        XmlNode node = doc.SelectSingleNode("urls/url[@address='" + address + "']");
        if (node == null)
        {
          AddNewUrl(doc, address, isSpam);
        }
        else
        {
          int count = int.Parse(node.InnerText, CultureInfo.InvariantCulture);
          node.InnerText = (count + 1).ToString();
        }

        doc.Save(fileName);
      }
    }

    /// <summary>
    /// Adds a new Url to the XmlDocument.
    /// </summary>
    private static void AddNewUrl(XmlDocument doc, string address, bool isSpam)
    {
      XmlNode newNode = doc.CreateElement("url");

      XmlAttribute attrAddress = doc.CreateAttribute("address");
      attrAddress.Value = address;
      newNode.Attributes.Append(attrAddress);

      XmlAttribute attrSpam = doc.CreateAttribute("isSpam");
      attrSpam.Value = isSpam.ToString();
      newNode.Attributes.Append(attrSpam);

      newNode.InnerText = "1";
      doc.ChildNodes[1].AppendChild(newNode);
    }

    private static DateTime _Date = DateTime.Now;

    /// <summary>
    /// Creates the XML file for first time use.
    /// </summary>
    private static XmlDocument CreateDocument(string fileName)
    {
      XmlDocument doc = new XmlDocument();

      if (!Directory.Exists(_Folder))
        Directory.CreateDirectory(_Folder);

      if (DateTime.Now.Day != _Date.Day || !File.Exists(fileName))
      {
        using (XmlWriter writer = XmlWriter.Create(fileName))
        {
          writer.WriteStartDocument(true);
          writer.WriteStartElement("urls");
          writer.WriteEndElement();
        }
      }

      _Date = DateTime.Now;
      doc.Load(fileName);
      return doc;
    }
  }

}