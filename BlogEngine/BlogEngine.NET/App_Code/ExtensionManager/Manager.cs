﻿using System;
using System.Web;
using System.Web.Hosting;
using System.Web.Caching;
using System.Reflection;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BlogEngine.Core;
using BlogEngine.Core.Providers;
using BlogEngine.Core.Web.Controls;

/// <summary>
/// Extension Manager - top level object in the hierarchy
/// Holds collection of extensions and methods to manipulate
/// extensions
/// </summary>
[XmlRoot]
public class ExtensionManager
{
  #region Constructor
  /// <summary>
  /// Default constructor, requred for serialization to work
  /// </summary>
  public ExtensionManager() { }
  #endregion

  #region Private members
  private static string _fileName = HostingEnvironment.MapPath(BlogSettings.Instance.StorageLocation + "extensions.xml");
  private static List<ManagedExtension> _extensions = new List<ManagedExtension>();
  private static BlogProviderSection _section = (BlogProviderSection)ConfigurationManager.GetSection("BlogEngine/blogProvider");
  private static StringCollection _newExtensions = new StringCollection();
  #endregion

  #region Public members
  /// <summary>
  /// Used to hold exeption thrown when extension can not be serialized because of
  /// file access permission. Not serializable, used by UI to show error message.
  /// </summary>
  [XmlIgnore]
  public static Exception FileAccessException = null;
  /// <summary>
  /// Collection of extensions
  /// </summary>
  [XmlElement]
  public static List<ManagedExtension> Extensions { get { return _extensions; } }
  /// <summary>
  /// Enabled / Disabled
  /// </summary>
  /// <param name="extensionName"></param>
  /// <returns>True if enabled</returns>
  public static bool ExtensionEnabled(string extensionName)
  {
    bool val = true;
    LoadExtensions();
    _extensions.Sort(delegate(ManagedExtension p1, ManagedExtension p2) { return String.Compare(p1.Name, p2.Name); });

    foreach (ManagedExtension x in _extensions)
    {
      if (x.Name == extensionName)
      {
        if (x.Enabled == false)
        {
          val = false;
        }
        break;
      }
    }
    return val;
  }
  /// <summary>
  /// Only change status on first load;
  /// This allows to enable/disable extension on
  /// initial load and then be able to override it with
  /// change status from admin interface
  /// </summary>
  /// <param name="extension">Extension Name</param>
  /// <param name="enabled">Enable/disable extension on initial load</param>
  public static void SetStatus(string extension, bool enabled)
  {
    if (IsNewExtension(extension))
    {
      ChangeStatus(extension, enabled);
    }
  }
  /// <summary>
  /// Method to change extension status
  /// </summary>
  /// <param name="extension">Extensio Name</param>
  /// <param name="enabled">If true, enables extension</param>
  public static void ChangeStatus(string extension, bool enabled)
  {
    foreach (ManagedExtension x in _extensions)
    {
      if (x.Name == extension)
      {
        x.Enabled = enabled;
        SaveToStorage();
        SaveToCache();

        string ConfigPath = HttpContext.Current.Request.PhysicalApplicationPath + "web.config";
        System.IO.File.SetLastWriteTimeUtc(ConfigPath, DateTime.UtcNow);

        break;
      }
    }
  }
  /// <summary>
  /// A way to let extension author to use custom
  /// admin page. Will show up as link on extensions page
  /// </summary>
  /// <param name="extension">Extension Name</param>
  /// <param name="url">Path to custom admin page</param>
  public static void SetAdminPage(string extension, string url)
  {
    foreach (ManagedExtension x in _extensions)
    {
      if (x.Name == extension)
      {
        x.AdminPage = url;
        SaveToStorage();
        SaveToCache();
        break;
      }
    }
  }
  /// <summary>
  /// Tell if manager already has this extension
  /// </summary>
  /// <param name="type">Extension Type</param>
  /// <returns>True if already has</returns>
  public static bool Contains(Type type)
  {
    foreach (ManagedExtension extension in _extensions)
    {
      if (extension.Name == type.Name)
        return true;
    }

    return false;
  }
  /// <summary>
  /// Adds extension to ext. collection in the manager
  /// </summary>
  /// <param name="type">Extension type</param>
  /// <param name="attribute">Extension attribute</param>
  public static void AddExtension(Type type, object attribute)
  {
    ExtensionAttribute xa = (ExtensionAttribute)attribute;
    ManagedExtension x = new ManagedExtension(type.Name, xa.Version, xa.Description, xa.Author);
    _extensions.Add(x);
  }
  #endregion

  #region Private methods
  /// <summary>
  /// If extensions not in the cache will load
  /// from the XML file. If file not exists
  /// will load from assembly using reflection
  /// </summary>
  static void LoadExtensions()
  {   // initialize on application load
    if (HttpContext.Current.Cache["Extensions"] == null)
    {
      if (_section.DefaultProvider == "XmlBlogProvider")
      {
        if (File.Exists(_fileName))
        {
          LoadFromStorage();
          AddNewExtensions();
        }
        else  // very first run
        {
          LoadFromAssembly();
        }
      }
      else
      {
        Stream settings = BlogService.LoadExtensionSettings();
        if (!((settings == null) || (settings.Length < 3)))
        {
          LoadFromStorage();
          AddNewExtensions();
        }
        else  // very first run
        {
          LoadFromAssembly();
        }
      }
      Save();
    }
  }
  /// <summary>
  /// Populates extensions collection with
  /// information loaded from assembly
  /// </summary>
  static void LoadFromAssembly()
  {
    string assemblyName = "__code";

    if (Utils.IsMono)
      assemblyName = "App_Code";

    Assembly a = Assembly.Load(assemblyName);
    Type[] types = a.GetTypes();

    foreach (Type type in types)
    {
      object[] attributes = type.GetCustomAttributes(typeof(ExtensionAttribute), false);

      foreach (object attribute in attributes)
      {
        AddExtension(type, attribute);
      }
    }
  }
  /// <summary>
  /// After loading from XML file, checks if
  /// there were new extensions added to application
  /// </summary>
  static void AddNewExtensions()
  {
    string assemblyName = "__code";

    if (Utils.IsMono)
      assemblyName = "App_Code";

    Assembly a = Assembly.Load(assemblyName);
    Type[] types = a.GetTypes();

    foreach (Type type in types)
    {
      object[] attributes = type.GetCustomAttributes(typeof(ExtensionAttribute), false);

      foreach (object attribute in attributes)
      {
        if (!Contains(type))
        {
          _newExtensions.Add(type.Name);
          AddExtension(type, attribute);
        }
      }
    }
  }
  #endregion

  #region Settings
  /// <summary>
  /// Method to get settings collection
  /// </summary>
  /// <param name="extensionName">Extension Name</param>
  /// <returns>Collection of settings</returns>
  public static ExtensionSettings GetSettings(string extensionName)
  {
    return GetSettings(extensionName, extensionName);
  }
  public static ExtensionSettings GetSettings(string extensionName, string settingName)
  {
    foreach (ManagedExtension x in _extensions)
    {
      if (x.Name == extensionName)
      {
        foreach (ExtensionSettings setting in x.Settings)
        {
          if (setting != null)
          {
            if (setting.Name == settingName)
            {
              return setting;
            }
          }
        }
      }
    }
    return null;
  }
  /// <summary>
  /// Will save settings (add to extension object, then
  /// cache and serialize all object hierarhy to XML)
  /// </summary>
  /// <param name="extensionName">Extension Name</param>
  /// <param name="settings">Settings object</param>
  public static void SaveSettings(ExtensionSettings settings)
  {
    SaveSettings(settings.Name, settings);
  }
  public static void SaveSettings(string extensionName, ExtensionSettings settings)
  {
    foreach (ManagedExtension x in _extensions)
    {
      if (x.Name == extensionName)
      {
        x.SaveSettings(settings);
        break;
      }
    }
    Save();
  }
  /// <summary>
  /// Do initial import here.
  /// If already imported, let extension manager take care of settings
  /// To reset, blogger has to delete all settings in the manager
  /// </summary>
  public static bool ImportSettings(ExtensionSettings settings)
  {
    return ImportSettings(settings.Name, settings);
  }
  public static bool ImportSettings(string extensionName, ExtensionSettings settings)
  {
    foreach (ManagedExtension x in _extensions)
    {
      if (x.Name == extensionName)
      {
        if (!x.Initialized(settings.Name))
        {
          x.SaveSettings(settings);
        }
        break;
      }
    }
    return Save();
  }
  #endregion

  #region Serialization
  /// <summary>
  /// Will serialize and cache ext. mgr. object
  /// </summary>
  public static bool Save()
  {
    SaveToCache();
    return SaveToStorage();
  }
  /// <summary>
  /// Saves ext. manager object to XML file
  /// or database table using provider model
  /// </summary>
  /// <returns>True if successful</returns>
  public static bool SaveToStorage()
  {
    if (_section.DefaultProvider == "XmlBlogProvider")
    {
      try
      {
        using (TextWriter writer = new StreamWriter(_fileName))
        {
          XmlSerializer serializer = new XmlSerializer(typeof(List<ManagedExtension>));
          serializer.Serialize(writer, _extensions);
          return true;
        }
      }
      catch (Exception e)
      {
        FileAccessException = e;
        HttpContext.Current.Cache.Remove("Extensions");
        throw;
      }
    }
    else
    {
      MemoryStream settings = new MemoryStream();
      XmlSerializer serializer = new XmlSerializer(typeof(List<ManagedExtension>));
      XmlTextWriter xmlTextWriter = new XmlTextWriter(settings, System.Text.Encoding.UTF8);
      serializer.Serialize(xmlTextWriter, _extensions);

      settings = (MemoryStream)xmlTextWriter.BaseStream;

      BlogService.SaveExtensionSettings(settings);
      return true;
    }
  }
  /// <summary>
  /// Deserializes extension manager from storage
  /// (XML file or database) back to ext. manager object
  /// </summary>
  private static void LoadFromStorage()
  {
    Stream settings = null;
    try
    {
      settings = BlogService.LoadExtensionSettings();
      XmlSerializer serializer = new XmlSerializer(typeof(List<ManagedExtension>));
      _extensions = (List<ManagedExtension>)serializer.Deserialize(settings);

      string assemblyName = "__code";
      if (Utils.IsMono)
        assemblyName = "App_Code";

      Assembly a = Assembly.Load(assemblyName);

      for (int i = _extensions.Count - 1; i >= 0; i--)
      {
        if (a.GetType(_extensions[i].Name, false) == null)
        {
          _extensions.Remove(_extensions[i]);
        }
        else
        {
          // fix setting name for older extensions 
          // set it to extension name
          foreach (ExtensionSettings xset in _extensions[i].Settings)
          {
            if (xset != null)
            {
              if (string.IsNullOrEmpty(xset.Name))
              {
                xset.Name = _extensions[i].Name;
              }
            }
          }
        }
      }
    }
    catch (Exception)
    {
      HttpContext.Current.Cache.Remove("Extensions");
      throw;
    }
    finally
    {
      if (settings != null)
        settings.Close();
    }
  }
  /// <summary>
  /// Caches for performance. If manager cached
  /// and not updates done, chached copy always 
  /// returned
  /// </summary>
  static void SaveToCache()
  {
    HttpContext.Current.Cache.Remove("Extensions");
    HttpContext.Current.Cache["Extensions"] = _extensions;
  }
  #endregion
  
  /// <summary>
  /// Extension is "new" if it is loaded from assembly
  /// but not yet saved to the disk. This state is needed
  /// so that we can initialize extension and its settings
  /// on the first load and then override it from admin
  /// </summary>
  /// <param name="name">Extension name</param>
  /// <returns>True if new</returns>
  private static bool IsNewExtension(string name)
  {
    if (_newExtensions.Contains(name))
      return true;
    else
      return false;
  }
}
