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
  /// Get settings from data store
  /// </summary>
  /// <param name="type">Object type</param>
  /// <param name="obj">Object</param>
  /// <returns>Settings</returns>
  public StringDictionary GetSettings(string id)
  {
    string cacheId = "be_theme_" + id;
    if (Cache[cacheId] == null)
    {
      ThemeSettings ts = new ThemeSettings(id);
      Cache[cacheId] = (StringDictionary)ts.GetSettings();
    }
    return (StringDictionary)Cache[cacheId];
  }

  /// <summary>
  /// Saves settings to data store
  /// </summary>
  /// <param name="settings">Object Settings</param>
  protected virtual void SaveSettings(string id, object settings)
  {
    string cacheId = "be_theme_" + id;
    ThemeSettings ts = new ThemeSettings(id);
    ts.SaveSettings(settings);
    Cache[cacheId] = settings;
  }

  #endregion
}
