#region Using

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.Hosting;
using System.Threading;
using System.Xml;
using System.Text;
using BlogEngine.Core;
using BlogEngine.Core.DataStore;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

#endregion

/// <summary>
/// Summary description for WidgetBase
/// </summary>
public class WidgetBase : UserControl
{

	#region Properties

	private string _Title;
	/// <summary>
	/// Gets or sets the title of the widget. It is mandatory for all widgets to set the Title.
	/// </summary>
	/// <value>The title of the widget.</value>
	public string Title
	{
		get { return _Title; }
		set { _Title = value; }
	}

	private bool _ShowTitle;
	/// <summary>
	/// Gets or sets a value indicating whether [show title].
	/// </summary>
	/// <value><c>true</c> if [show title]; otherwise, <c>false</c>.</value>
	public bool ShowTitle
	{
		get { return _ShowTitle; }
		set { _ShowTitle = value; }
	}

	private string _Name;
	/// <summary>
	/// Gets or sets the name. It must be exactly the same as the folder that contains the widget.
	/// </summary>
	/// <value>The name.</value>
	public string Name
	{
		get { return _Name; }
		set { _Name = value; }
	}

	private Guid _WidgetID;
	/// <summary>
	/// Gets the widget ID.
	/// </summary>
	/// <value>The widget ID.</value>
	public Guid WidgetID
	{
		get { return _WidgetID; }
		set { _WidgetID = value; }
	}

	/// <summary>
	/// Gets the XML used for storing settings for the individual widgets.
	/// </summary>
	/// <value>The XML document.</value>
	public XmlDocument Xml
	{
		get
		{
      string cacheId = "be_widget_" + WidgetID;
      XmlDocument xml = new XmlDocument();
      if (Cache[cacheId] == null)
			{
        WidgetSettings ws = new WidgetSettings(WidgetID.ToString());
        xml = (XmlDocument)ws.GetSettings();

        HttpContext.Current.Cache[cacheId] = xml;
			}
      return (XmlDocument)Cache[cacheId];
		}
	}

	#endregion

	/// <summary>
	/// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
	/// </summary>
	/// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
	protected override void Render(HtmlTextWriter writer)
	{
		if (string.IsNullOrEmpty(Name))
			throw new NullReferenceException("Name must be set on a widget");

			StringBuilder sb = new StringBuilder();

			sb.Append("<div class=\"widget " + this.Name.Replace(" ", string.Empty).ToLowerInvariant() + "\" id=\"widget" + WidgetID + "\">");

			if (Thread.CurrentPrincipal.IsInRole(BlogSettings.Instance.AdministratorRole))
			{
				sb.Append("<a class=\"delete\" href=\"javascript:void(0)\" onclick=\"removeWidget('" + WidgetID + "');return false\" title=\"" + Resources.labels.delete + " widget\">X</a>");
					sb.Append("<a class=\"edit\" href=\"javascript:void(0)\" onclick=\"editWidget('" + Name + "', '" + WidgetID + "');return false\" title=\"" + Resources.labels.edit + " widget\">" + Resources.labels.edit + "</a>");
			}

			if (ShowTitle)
				sb.Append("<h4>" + Title + "</h4>");
			else
				sb.Append("<br />");

			sb.Append("<div class=\"content\">");

		writer.Write(sb.ToString());
		base.Render(writer);
		writer.Write("</div>");
		writer.Write("</div>");
	}

}
