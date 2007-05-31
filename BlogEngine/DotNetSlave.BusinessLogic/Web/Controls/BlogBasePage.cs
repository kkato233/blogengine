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
  /// All pages in the custom themes as well as pre-defined pages in the root
  /// must inherit from this class.
  /// </summary>
  /// <remarks>
  /// The class is responsible for assigning the theme to all
  /// derived pages as well as adding RSS, RSD, MicroSummary, tracking script
  /// and a whole lot more.
  /// </remarks>
  public abstract class BlogBasePage : System.Web.UI.Page
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
    /// Adds links and javascript to the HTML header tag.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (!Page.IsCallback && !Page.IsPostBack)
      {
        // Links
        AddMetaContentType();
        AddMicroSummary();        
        AddRsdLinkHeader();
        AddSyndicationLink();

        if (BlogSettings.Instance.EnableOpenSearch)
          AddOpenSearchLinkInHeader();

        if (BlogSettings.Instance.RemoveWhitespaceInStyleSheets)
          CompressCSS();

        // JavaScripts
        AddEmbeddedJavaScript("BlogEngine.Core.Web.Scripts.blog.js");

        if (BlogSettings.Instance.EnableSearchHightlight)
          AddEmbeddedJavaScript("BlogEngine.Core.Web.Scripts.se_hilite.js");

        if (!string.IsNullOrEmpty(BlogSettings.Instance.HtmlHeader))
          AddCustomCodeToHead();

        if (!string.IsNullOrEmpty(BlogSettings.Instance.TrackingScript))
          AddTrackingScript();
      }
    }

    private void AddSyndicationLink()
    {
      HtmlLink link = new HtmlLink();
      link.Attributes["rel"] = "alternate";
      link.Attributes["type"] = "application/rss+xml";
      link.Attributes["title"] = BlogSettings.Instance.Name;
      
      if (!string.IsNullOrEmpty(BlogSettings.Instance.FeedburnerUserName))
        link.Attributes["href"] = "http://feeds.feedburner.com/" + BlogSettings.Instance.FeedburnerUserName;
      else
        link.Attributes["href"] = Utils.AbsoluteWebRoot + "syndication.axd";

      Page.Header.Controls.Add(link);
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

    private void AddRsdLinkHeader()
    {
      HtmlLink link = new HtmlLink();
      link.Attributes["rel"] = "edituri";
      link.Attributes["type"] = "application/rsd+xml";
      link.Attributes["title"] = "RSD";
      link.Attributes["href"] = Utils.AbsoluteWebRoot + "rsd.axd";
      Page.Header.Controls.Add(link);
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

    private void AddMicroSummary()
    {
      HtmlLink ms = new HtmlLink();
      ms.Attributes["rel"] = "microsummary";
      ms.Attributes["type"] = "application/x.microsummary+xml";
      ms.Href = "microsummary.axd?id=" + Request.PathInfo;
      Page.Header.Controls.Add(ms);
    }

    /// <summary>
    /// Adds a JavaScript reference to the HTML head tag.
    /// </summary>
    private void AddEmbeddedJavaScript(string name)
    {
      HtmlGenericControl script = new HtmlGenericControl("script");
      script.Attributes["type"] = "text/javascript";
      script.Attributes["src"] = Page.ClientScript.GetWebResourceUrl(typeof(Post), name);
      Page.Header.Controls.Add(script);
    }

    /// <summary>
    /// Adds a JavaScript to the bottom of the page at runtime.    
    /// </summary>
    /// <remarks>
    /// You must add the script tags to the BlogSettings.Instance.TrackingScript.
    /// </remarks>
    private void AddTrackingScript()
    {
      ClientScript.RegisterStartupScript(this.GetType(), "tracking", "\n" + BlogSettings.Instance.TrackingScript, false);
    }

    /// <summary>
    /// Adds code to the HTML head section.
    /// </summary>
    private void AddCustomCodeToHead()
    {
      string code = string.Format("{0}<!-- Start custom code -->{0}{1}{0}<!-- End custom code -->{0}", Environment.NewLine, BlogSettings.Instance.HtmlHeader);
      LiteralControl control = new LiteralControl(code);
      Page.Header.Controls.Add(control);
    }

    /// <summary>
    /// Translates the specified string using the resource files
    /// </summary>
    public string Translate(string text)
    {
      try
      {
        return this.GetGlobalResourceObject("labels", text).ToString();
      }
      catch (NullReferenceException)
      {
        return text;
      }
    }

  }
}