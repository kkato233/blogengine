#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

#endregion

namespace BlogEngine.Core.Web.Controls
{
  /// <summary>
  /// Summary description for BlogBasePage
  /// </summary>
  public class BlogBasePage : Page
  {
    /// <summary>
    /// Assignes the selected theme to the pages.
    /// </summary>
    protected override void OnPreInit(EventArgs e)
    {
      MasterPageFile = "~/themes/" + BlogSettings.Instance.Theme + "/site.master";
      base.OnPreInit(e);
    }

    private void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsCallback)
      {
        AddMetaContentType();
        SetMicroSummaryLinkInHeader();
      }
    }

    private void AddMetaContentType()
    {
      HtmlMeta meta = new HtmlMeta();
      meta.HttpEquiv = "content-type";
      meta.Content = Response.ContentType + "; charset=" + Response.ContentEncoding.HeaderName;
      Page.Header.Controls.Add(meta);
    }

    /// <summary>
    /// Add a meta tag to the page's header.
    /// </summary>
    public void AddMetaTag(string name, string value)
    {
      if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
        return;

      HtmlMeta meta = new HtmlMeta();
      meta.Name = name;
      meta.Content = value;
      Page.Header.Controls.Add(meta);
    }

    private void SetMicroSummaryLinkInHeader()
    {
      HtmlLink ms = new HtmlLink();
      ms.Attributes["rel"] = "microsummary";
      ms.Attributes["type"] = "application/x.microsummary+xml";
      ms.Href = "microsummary.axd?id=" + Request.PathInfo;
      Page.Header.Controls.Add(ms);
    }

  }
}