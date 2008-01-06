#region Using

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.Hosting;
using System.Threading;
using System.Xml;
using BlogEngine.Core;

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
			if (Cache["xml_" + WidgetID] == null)
			{
				XmlDocument xml = new XmlDocument();
				if (File.Exists(FileName))
				{
					xml.Load(FileName);
				}
				Cache.Insert("xml_" + WidgetID, xml, new System.Web.Caching.CacheDependency(FileName));
			}

			return (XmlDocument)Cache["xml_" + WidgetID];
		}
	}

	private string _FileName;
	/// <summary>
	/// Gets the name of the file.
	/// </summary>
	/// <value>The name of the file.</value>
	protected string FileName
	{
		get
		{
			if (string.IsNullOrEmpty(_FileName))
				_FileName = HostingEnvironment.MapPath(Utils.RelativeWebRoot + "App_Data/widgets/" + WidgetID + ".xml");

			return _FileName;
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

		writer.Write("<div class=\"widget\" id=\"widget" + WidgetID + "\">");

		if (Thread.CurrentPrincipal.IsInRole("administrators"))
		{
			writer.Write("<a class=\"delete\" href=\"javascript:void(0)\" onclick=\"removeWidget('" + WidgetID + "');return false\" title=\"" + Resources.labels.delete + " widget\">X</a>");
			writer.Write("<a class=\"edit\" href=\"javascript:void(0)\" onclick=\"editWidget('" + Name + "', '" + WidgetID + "');return false\" title=\"" + Resources.labels.edit + " widget\">" + Resources.labels.edit + "</a>");
		}

		writer.Write("<h4>" + Title + "</h4>");
		writer.Write("<div class=\"content\">");
		base.Render(writer);
		writer.Write("</div>");
		writer.Write("</div>");
	}

}
