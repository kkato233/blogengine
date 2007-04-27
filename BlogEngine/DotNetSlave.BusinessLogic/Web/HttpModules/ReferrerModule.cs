#region Using

using System;
using System.Xml;
using System.IO;
using System.Web;
using System.Globalization;
using BlogEngine.Core;

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
      if (context.Request.UrlReferrer != null)
      {
        Uri referrer = context.Request.UrlReferrer;
        if (!referrer.Host.Equals(Utils.AbsoluteWebRoot.Host, StringComparison.OrdinalIgnoreCase) && !IsSearchEngine(referrer.ToString()))
        {
          RegisterClick(referrer.ToString());
        }
      }
    }

    private bool IsSearchEngine(string referrer)
    {
      return referrer.ToLowerInvariant().Contains("?q=") || referrer.ToLowerInvariant().Contains("&q=");
    }

    /// <summary>
    /// Used to thread safe the file operations
    /// </summary>
    private static object _SyncRoot = new object();

    /// <summary>
    /// The relative path of the XML file.
    /// </summary>
    private static string _Folder = HttpContext.Current.Server.MapPath("~/app_data/log/");

    private void RegisterClick(string url)
    {
      string fileName = _Folder + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xml";

      lock (_SyncRoot)
      {
        //if (_Document == null)
        XmlDocument doc = CreateDocument(fileName);

        string address = HttpUtility.HtmlEncode(url);
        XmlNode node = doc.SelectSingleNode("urls/url[@address='" + address + "']");
        if (node == null)
        {
          AddNewUrl(doc, address);
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
    private static void AddNewUrl(XmlDocument doc, string address)
    {
      XmlNode newNode = doc.CreateElement("url");

      XmlAttribute attrAddress = doc.CreateAttribute("address");
      attrAddress.Value = address;
      newNode.Attributes.Append(attrAddress);

      newNode.InnerText = "1";
      doc.ChildNodes[1].AppendChild(newNode);
    }

    /// <summary>
    /// Creates the XML file for first time use.
    /// </summary>
    private static XmlDocument CreateDocument(string fileName)
    {
      XmlDocument doc = new XmlDocument();

      if (!Directory.Exists(_Folder))
        Directory.CreateDirectory(_Folder);

      if (!File.Exists(fileName))
      {
        using (XmlWriter writer = XmlWriter.Create(fileName))
        {
          writer.WriteStartDocument(true);
          writer.WriteStartElement("urls");
          writer.WriteEndElement();
        }
      }

      doc.Load(fileName);
      return doc;
    }

  }
}