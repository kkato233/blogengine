using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using BlogEngine.Core.DataStore;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Collections;

/// <summary>
/// Base class for theme settings
/// </summary>
public class ThemeSettingsBase : System.Web.UI.Page
{
  public ThemeSettingsBase()
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
    if (HttpContext.Current.Cache[cacheId] == null)
    {
      ThemeSettings ts = new ThemeSettings(id);
      HttpContext.Current.Cache[cacheId] = (StringDictionary)ts.GetSettings();
    }
    return (StringDictionary)HttpContext.Current.Cache[cacheId];
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
    HttpContext.Current.Cache[cacheId] = settings;
  }

  #endregion
}
