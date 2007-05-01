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
  public class BlogBasePage : System.Web.UI.Page
  {
    /// <summary>
    /// Assignes the selected theme to the pages.
    /// </summary>
    protected override void OnPreInit(EventArgs e)
    {
      MasterPageFile = "~/themes/" + BlogSettings.Instance.Theme + "/site.master";
      base.OnPreInit(e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (!Page.IsCallback && !Page.IsPostBack)
      {
        AddMetaContentType();
        SetMicroSummaryLinkInHeader();
        AddScriptFile(Utils.RelativeWebRoot + "scripts/blog.js");

        if (BlogSettings.Instance.EnableOpenSearch)
          AddOpenSearchLinkInHeader();
        
        if (BlogSettings.Instance.EnableSearchHightlight)
          AddScriptFile(Utils.RelativeWebRoot + "script/se_hilite.js");

        if (BlogSettings.Instance.MarkExternalLinks)
          MarkExternalLinks();
      }

      if (BlogSettings.Instance.RemoveWhitespaceInStyleSheets)
        CompressCSS();
    }

    /// <summary>
    /// Finds all stylesheets in the header and changes the 
    /// path so it points to css.axd which removes the whitespace.
    /// </summary>
    private void CompressCSS()
    {
      foreach (Control control in Page.Header.Controls)
      {
        HtmlControl c = control as HtmlControl;
        if (c != null && c.Attributes["type"] != null && c.Attributes["type"].Equals("text/css", StringComparison.OrdinalIgnoreCase))
        {
          c.Attributes["href"] = Utils.RelativeWebRoot + "themes/" + BlogSettings.Instance.Theme + "/css.axd?name=" + c.Attributes["href"];
        }
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

    private void AddOpenSearchLinkInHeader()
    {
      HtmlLink link = new HtmlLink();
      link.Attributes["rel"] = "search";
      link.Attributes["href"] = VirtualPathUtility.ToAbsolute("~/") + "opensearch.axd";
      link.Attributes["type"] = "application/opensearchdescription+xml";
      link.Attributes["title"] = BlogSettings.Instance.Name;
      Page.Header.Controls.Add(link);
    }

    private void SetMicroSummaryLinkInHeader()
    {
      HtmlLink ms = new HtmlLink();
      ms.Attributes["rel"] = "microsummary";
      ms.Attributes["type"] = "application/x.microsummary+xml";
      ms.Href = "microsummary.axd?id=" + Request.PathInfo;
      Page.Header.Controls.Add(ms);
    }

    private void AddScriptFile(string location)
    {
      HtmlGenericControl script = new HtmlGenericControl("script");
      script.Attributes["type"] = "text/javascript";
      script.Attributes["src"] = location;
      Page.Header.Controls.Add(script);
    }

    private void MarkExternalLinks()
    {
      Page.ClientScript.RegisterStartupScript(this.GetType(), "external", "StyleExternalLinks();", true);
    }

  }
}