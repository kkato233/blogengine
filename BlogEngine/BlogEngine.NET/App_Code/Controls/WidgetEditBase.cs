#region Using

using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using BlogEngine.Core;
using BlogEngine.Core.DataStore;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;

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
	//public XmlDocument Xml
	//{
	//  get 
	//  {
	//    string cacheId = "be_widget_" + WidgetID;
	//    XmlDocument xml = new XmlDocument();
	//    if (Cache[cacheId] == null)
	//    {
	//      WidgetSettings ws = new WidgetSettings(WidgetID.ToString());
	//      xml = (XmlDocument)ws.GetSettings();

	//      HttpContext.Current.Cache[cacheId] = xml;
	//    }
	//    return (XmlDocument)Cache[cacheId];
	//  }
	//}

	#endregion

	/// <summary>
	/// Saves this the basic widget settings such as the Title.
	/// </summary>
	public abstract void Save();

	//protected virtual void SaveXml()
	//{
	//  WidgetSettings ws = new WidgetSettings(WidgetID.ToString());
	//  ws.SaveSettings(Xml);
	//}

  #region Settings

  /// <summary>
  /// Object types supported by data store
  /// </summary>
  public enum ObjectType
  {
    XmlDocument,
    StringDictionary,
    CustomObject
  }

  /// <summary>
  /// Get settings from data store
  /// </summary>
  /// <param name="type">Object type</param>
  /// <returns>Settings</returns>
  public object GetSettings(ObjectType type)
  {
    return GetSettings(type, null);
  }

  /// <summary>
  /// Get settings from data store
  /// </summary>
  /// <param name="type">Object type</param>
  /// <param name="obj">Object</param>
  /// <returns>Settings</returns>
  public object GetSettings(ObjectType type, object obj)
  {
    string cacheId = "be_widget_" + WidgetID;
    if (Cache[cacheId] == null)
    {
      if (type == ObjectType.XmlDocument)
      {
        WidgetSettings ws = new WidgetSettings(WidgetID.ToString(), typeof(XmlDocument));
        Cache[cacheId] = (XmlDocument)ws.GetSettings();
      }
      else if (type == ObjectType.StringDictionary)
      {
        WidgetSettings ws = new WidgetSettings(WidgetID.ToString(), typeof(StringDictionary));
        Cache[cacheId] = (StringDictionary)ws.GetSettings();
      }
      else if (type == ObjectType.CustomObject)
      {
        object widgetData = CustomObject.GetObject(WidgetID.ToString(), obj.GetType());
        Cache[cacheId] = widgetData;
      }
    }
    return Cache[cacheId];
  }

  /// <summary>
  /// Saves settings to data store
  /// </summary>
  /// <param name="settings">Object Settings</param>
  protected virtual void SaveSettings(object settings)
  {
    string objType = settings.GetType().Name;
    string cacheId = "be_widget_" + WidgetID;

    if (objType == "XmlDocument")
    { 
      WidgetSettings ws = new WidgetSettings(WidgetID.ToString(), typeof(XmlDocument));
      ws.SaveSettings(settings);
    }
    else if (objType == "SerializableStringDictionary")
    {
      WidgetSettings ws = new WidgetSettings(WidgetID.ToString(), typeof(StringDictionary));
      ws.SaveSettings(settings);
    }
    else if (settings.GetType().BaseType.Name == "CustomObjectBase")
    {
      WidgetSettings ws = new WidgetSettings(WidgetID.ToString(), typeof(CustomObjectBase));
      ws.SaveSettings(settings);
    }
    else
    {
      throw new ApplicationException(objType + " is not supported by Data Store");
    }
    Cache[cacheId] = settings;
  }

  #endregion

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
