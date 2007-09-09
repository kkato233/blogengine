#region using

using System;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core;

#endregion

/// <summary>
/// Automatically turns certain names into XFN enabled hyperlinks
/// </summary>
[Extension("Automatically turns certain names into XFN enabled hyperlinks", "1.0", "BlogEngine.NET")]
public class AutoLink
{

  /// <summary>
  /// Hooks up an event handler to the Post.Saved event.
  /// </summary>
  public AutoLink()
  {
    BuildLinkCollection();
    Post.Serving += new EventHandler<ServingEventArgs>(Post_Serving);
  }

  private static readonly string _Anchor = "<a href=\"{0}\"{1}>{2}</a>";
  private static readonly Collection<Link> _Links = new Collection<Link>();

  private void Post_Serving(object sender, ServingEventArgs e)
  {
    foreach (Link link in _Links)
    {
      string rel = string.Format(" rel=\"{0}\"", link.Xfn);
      if (string.IsNullOrEmpty(link.Xfn))
        rel = null;

      string url = string.Format(_Anchor, link.Url, rel, link.Name);
      e.Body = e.Body.Replace(link.Shortcut, url);
    }
  }

  private void BuildLinkCollection()
  {
    string fileName = HttpContext.Current.Server.MapPath("~/app_data/autolink.xml");
    XmlDocument doc = new XmlDocument();
    doc.Load(fileName);
    XmlNodeList list = doc.SelectNodes("//link");

    foreach (XmlNode node in list)
    {
      Link link = new Link();
      link.Name = node.InnerText;
      link.Shortcut = node.Attributes["shortcut"].InnerText;

      if (node.Attributes["url"] != null)
        link.Url = node.Attributes["url"].InnerText;

      if (node.Attributes["xfn"] != null)
        link.Xfn = node.Attributes["xfn"].InnerText;

      _Links.Add(link);
    }
  }

  private struct Link
  {
    public string Shortcut;
    public string Url;
    public string Name;
    public string Xfn;
  }

}
