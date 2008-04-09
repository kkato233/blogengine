using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Serialization;
using BlogEngine.Core;
using BlogEngine.Core.DataStore;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;

/// <summary>
/// Summary description for MasterPageBase
/// </summary>
public class MasterPageBase : System.Web.UI.MasterPage
{
  public MasterPageBase()
  {
    //
    // TODO: Add constructor logic here
    //
  }

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
  public object GetSettings(string id)
  {
    return GetSettings(ObjectType.StringDictionary, id, null);
  }
  public object GetSettings(string id, ObjectType type)
  {
    return GetSettings(type, id, null);
  }

  /// <summary>
  /// Get settings from data store
  /// </summary>
  /// <param name="type">Object type</param>
  /// <param name="obj">Object</param>
  /// <returns>Settings</returns>
  public object GetSettings(ObjectType type, string id, object obj)
  {
    string cacheId = "be_theme_" + id;
    if (Cache[cacheId] == null)
    {
      if (type == ObjectType.XmlDocument)
      {
        ThemeSettings ts = new ThemeSettings(id, typeof(XmlDocument));
        Cache[cacheId] = (XmlDocument)ts.GetSettings();
      }
      else if (type == ObjectType.StringDictionary)
      {
        ThemeSettings ts = new ThemeSettings(id, typeof(StringDictionary));
        Cache[cacheId] = (StringDictionary)ts.GetSettings();
      }
      else if (type == ObjectType.CustomObject)
      {
        object custData = CustomObject.GetObject(id, obj.GetType());
        Cache[cacheId] = custData;
      }
    }
    return Cache[cacheId];
  }

  /// <summary>
  /// Saves settings to data store
  /// </summary>
  /// <param name="settings">Object Settings</param>
  protected virtual void SaveSettings(string id, object settings)
  {
    string objType = settings.GetType().Name;
    string cacheId = "be_theme_" + id;

    if (objType == "XmlDocument")
    {
      ThemeSettings ts = new ThemeSettings(id, typeof(XmlDocument));
      ts.SaveSettings(settings);
    }
    else if (objType.Contains("StringDictionary")) // Use contains because objType can be both StringDictionary and SerializableStringDictionary
    {
      ThemeSettings ts = new ThemeSettings(id, typeof(StringDictionary));
      ts.SaveSettings(settings);
    }
    else if (settings.GetType().BaseType.Name == "CustomObjectBase")
    {
      ThemeSettings ts = new ThemeSettings(id, typeof(CustomObjectBase));
      ts.SaveSettings(settings);
    }
    else
    {
      throw new ApplicationException(objType + " is not supported by Data Store");
    }
    Cache[cacheId] = settings;
  }

  #endregion
}
