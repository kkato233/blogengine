#region Using

using System;
using System.Globalization;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;

#endregion

namespace BlogEngine.Core.Web.Controls
{
	/// <summary>
	/// All pages in the custom themes as well as pre-defined pages in the root
	/// must inherit from this class.
	/// </summary>
	/// <remarks>
	/// The class is responsible for assigning the theme to all
	/// derived pages as well as adding RSS, RSD, tracking script
	/// and a whole lot more.
	/// </remarks>
	public abstract class BlogBasePage : System.Web.UI.Page
	{

		private string _Theme = BlogSettings.Instance.Theme;
		/// <summary>
		/// Assignes the selected theme to the pages.
		/// </summary>
		protected override void OnPreInit(EventArgs e)
		{
			Page.Title = Server.HtmlEncode(BlogSettings.Instance.Name);

			if (Request.QueryString["theme"] != null)
				_Theme = Request.QueryString["theme"];

			MasterPageFile = Utils.RelativeWebRoot + "themes/" + _Theme + "/site.master";

			base.OnPreInit(e);

			if (!Page.IsPostBack && !string.IsNullOrEmpty(Request.QueryString["deletepost"]))
			{
				if (Page.User.Identity.IsAuthenticated)
				{
					Post post = BlogEngine.Core.Post.GetPost(new Guid(Request.QueryString["deletepost"]));
					if (Page.User.IsInRole(BlogSettings.Instance.AdministratorRole) || Page.User.Identity.Name == post.Author)
					{
						post.Delete();
						post.Save();
						Response.Redirect(Utils.RelativeWebRoot);
					}
				}
			}
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
				AddGenericLink("contents", "Archive", Utils.RelativeWebRoot + "archive.aspx");
				AddGenericLink("start", BlogSettings.Instance.Name, Utils.RelativeWebRoot);
				AddGenericLink("application/rdf+xml", "meta", "SIOC", Utils.AbsoluteWebRoot + "sioc.axd");
				AddGenericLink("application/apml+xml", "meta", "APML", Utils.AbsoluteWebRoot + "apml.axd");
				AddGenericLink("application/rdf+xml", "meta", "FOAF", Utils.AbsoluteWebRoot + "foaf.axd");
				AddGenericLink("application/rss+xml", "alternate", BlogSettings.Instance.Name, Utils.FeedUrl);
				AddGenericLink("application/rsd+xml", "edituri", "RSD", Utils.AbsoluteWebRoot + "rsd.axd");

				AddMetaContentType();
				AddLocalizationKeys();

				if (BlogSettings.Instance.EnableOpenSearch)
					AddGenericLink("application/opensearchdescription+xml", "search", BlogSettings.Instance.Name, Utils.AbsoluteWebRoot + "opensearch.axd");

				if (!string.IsNullOrEmpty(BlogSettings.Instance.HtmlHeader))
					AddCustomCodeToHead();

				if (!string.IsNullOrEmpty(BlogSettings.Instance.TrackingScript))
					AddTrackingScript();
			}

			AddJavaScriptInclude(Utils.RelativeWebRoot + "blog.js");

			if (User.IsInRole(BlogSettings.Instance.AdministratorRole))
			{
				AddJavaScriptInclude(Utils.RelativeWebRoot + "admin/widget.js");
				AddStylesheetInclude(Utils.RelativeWebRoot + "admin/widget.css");				
			}

			if (BlogSettings.Instance.RemoveWhitespaceInStyleSheets)
				CompressCss();
		}

		/// <summary>
		/// Adds the localization keys to JavaScript for use globally.
		/// </summary>
		protected virtual void AddLocalizationKeys()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("KEYhasRated='{0}';", Translate("youAlreadyRated"));
			sb.AppendFormat("KEYwebRoot='{0}';", Utils.RelativeWebRoot);

			HtmlGenericControl script = new HtmlGenericControl("script");
			script.Attributes.Add("type", "text/javascript");
			script.InnerText = sb.ToString();
			Page.Header.Controls.Add(script);
		}

		/// <summary>
		/// Finds all stylesheets in the header and changes the 
		/// path so it points to css.axd which removes the whitespace.
		/// </summary>
		protected virtual void CompressCss()
		{
			if (Request.QueryString["theme"] != null)
				return;

			foreach (Control control in Page.Header.Controls)
			{
				HtmlControl c = control as HtmlControl;
				if (c != null && c.Attributes["type"] != null && c.Attributes["type"].Equals("text/css", StringComparison.OrdinalIgnoreCase))
				{
					if (!c.Attributes["href"].StartsWith("http://"))
					{
						c.Attributes["href"] = Utils.RelativeWebRoot + "themes/" + BlogSettings.Instance.Theme + "/css.axd?name=" + c.Attributes["href"];
						c.EnableViewState = false;
					}
				}
			}
		}

		/// <summary>
		/// Adds the content-type meta tag to the header.
		/// </summary>
		protected virtual void AddMetaContentType()
		{
			HtmlMeta meta = new HtmlMeta();
			meta.HttpEquiv = "content-type";
			meta.Content = Response.ContentType + "; charset=" + Response.ContentEncoding.HeaderName;
			Page.Header.Controls.Add(meta);
		}

		/// <summary>
		/// Add a meta tag to the page's header.
		/// </summary>
		protected virtual void AddMetaTag(string name, string value)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
				return;

			HtmlMeta meta = new HtmlMeta();
			meta.Name = name;
			meta.Content = value;
			Page.Header.Controls.Add(meta);
		}

		/// <summary>
		/// Adds the generic link to the header.
		/// </summary>
		public virtual void AddGenericLink(string relation, string title, string href)
		{
			HtmlLink link = new HtmlLink();
			link.Attributes["rel"] = relation;
			link.Attributes["title"] = title;
			link.Attributes["href"] = href;
			Page.Header.Controls.Add(link);
		}

		/// <summary>
		/// Adds the generic link to the header.
		/// </summary>
		public virtual void AddGenericLink(string type, string relation, string title, string href)
		{
			HtmlLink link = new HtmlLink();
			link.Attributes["type"] = type;
			link.Attributes["rel"] = relation;
			link.Attributes["title"] = title;
			link.Attributes["href"] = href;
			Page.Header.Controls.Add(link);
		}

		/// <summary>
		/// Adds a JavaScript reference to the HTML head tag.
		/// </summary>
		public virtual void AddJavaScriptInclude(string url)
		{
			HtmlGenericControl script = new HtmlGenericControl("script");
			script.Attributes["type"] = "text/javascript";
			script.Attributes["src"] = ResolveScriptUrl(url);
			Page.Header.Controls.Add(script);
		}

		/// <summary>
		/// Adds a Stylesheet reference to the HTML head tag.
		/// </summary>
		/// <param name="url">The relative URL.</param>
		public virtual void AddStylesheetInclude(string url)
		{
			HtmlLink link = new HtmlLink();
			link.Attributes["type"] = "text/css";
			link.Attributes["href"] = url;
			link.Attributes["rel"] = "stylesheet";
			Page.Header.Controls.Add(link);
		}

		/// <summary>
		/// Resolves the script URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		public virtual string ResolveScriptUrl(string url)
		{
			return Utils.RelativeWebRoot + "js.axd?path=" + HttpUtility.UrlEncode(url);
		}

		/// <summary>
		/// Adds a JavaScript to the bottom of the page at runtime.    
		/// </summary>
		/// <remarks>
		/// You must add the script tags to the BlogSettings.Instance.TrackingScript.
		/// </remarks>
		protected virtual void AddTrackingScript()
		{
			ClientScript.RegisterStartupScript(this.GetType(), "tracking", "\n" + BlogSettings.Instance.TrackingScript, false);
		}

		/// <summary>
		/// Adds code to the HTML head section.
		/// </summary>
		protected virtual void AddCustomCodeToHead()
		{
			string code = string.Format(CultureInfo.InvariantCulture, "{0}<!-- Start custom code -->{0}{1}{0}<!-- End custom code -->{0}", Environment.NewLine, BlogSettings.Instance.HtmlHeader);
			LiteralControl control = new LiteralControl(code);
			Page.Header.Controls.Add(control);
		}

		/// <summary>
		/// Translates the specified string using the resource files
		/// </summary>
		public virtual string Translate(string text)
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

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.TemplateControl.Error"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnError(EventArgs e)
		{
			HttpContext ctx = HttpContext.Current;
			Exception exception = ctx.Server.GetLastError();

			if (exception != null && exception.Message.Contains("callback"))
			{
				// This is a robot spam attack so we send it a 404 status to make it go away.
				ctx.Response.StatusCode = 404;
				ctx.Server.ClearError();
				Comment.OnSpamAttack();
			}

			base.OnError(e);
		}

		/// <summary>
		/// Initializes the <see cref="T:System.Web.UI.HtmlTextWriter"></see> object and calls on 
		/// the child controls of the <see cref="T:System.Web.UI.Page"></see> to render.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> that receives the page content.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			// Overwrite the default HtmlTextWriter in order to rewrite the form tag's action attribute
			// due to mono rewrite path issues.
			base.Render(new RewriteFormHtmlTextWriter(writer));
		}

	}
}