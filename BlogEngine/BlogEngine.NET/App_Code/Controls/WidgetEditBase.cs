#region Using

using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Threading;
using System.Xml;
using BlogEngine.Core;

#endregion

/// <summary>
/// Summary description for WidgetBase
/// </summary>
public abstract class WidgetEditBase : UserControl
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
	/// Saves this the basic widget settings such as the Title.
	/// </summary>
	public abstract void Save();

	protected virtual void SaveXml()
	{
		Xml.Save(FileName);
	}

	public static event EventHandler<EventArgs> Saved;
	/// <summary>
	/// Occurs when the class is Saved
	/// </summary>
	public static void OnSaved()
	{
		if (Saved != null)
		{
			Saved(null, new EventArgs());
		}
	}
        

}
