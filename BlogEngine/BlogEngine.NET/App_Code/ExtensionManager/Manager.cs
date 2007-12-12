using System;
using System.Web;
using System.Web.Hosting;
using System.Web.Caching;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;

/// <summary>
/// Summary description for ExtensionManager
/// </summary>
[XmlRoot]
public class ExtensionManager
{
    public ExtensionManager() { }

    private static string _fileName = HostingEnvironment.MapPath(BlogSettings.Instance.StorageLocation + "extensions.xml");
    private static List<Extension> _extensions = new List<Extension>();

    [XmlElement]
    public static List<Extension> Extensions { get { return _extensions; } }

    public static bool ExtensionEnabled(string extensionName)
    {
        bool val = true;
        LoadExtensions();
        _extensions.Sort(delegate(Extension p1, Extension p2) { return String.Compare(p1.Name, p2.Name); });

        foreach (Extension x in _extensions)
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

    public static void ChangeStatus(string extension, bool enabled)
    {
        foreach (Extension x in _extensions)
        {
            if (x.Name == extension)
            {
                x.Enabled = enabled;
                SaveToXML();
                SaveToCache();
                break;
            }
        }
    }

    public static void Save()
    {
        SaveToXML();
        SaveToCache();
    }

    static void LoadExtensions()
    {   // initialize on application load
        if (HttpContext.Current.Cache["Extensions"] == null)
        {
            if (File.Exists(_fileName))
            {
                LoadFromXML();
                AddNewExtensions();
            }
            else  // very first run
            {
                LoadFromAssembly();
            }
            SaveToXML();
            SaveToCache();
        }
    }

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
                    AddExtension(type, attribute);
            }
        }
    }

    #region Contains and Add
    public static bool Contains(Type type)
    {
        foreach (Extension extension in _extensions)
        {
            if (extension.Name == type.Name)
                return true;
        }

        return false;
    }

    public static void AddExtension(Type type, object attribute)
    {
        ExtensionAttribute xa = (ExtensionAttribute)attribute;
        Extension x = new Extension(type.Name, xa.Version, xa.Description);
        _extensions.Add(x);
    }
    #endregion

    #region Settings
    public static ExtensionSettings GetSettings(string extensionName)
    {
        foreach (Extension x in _extensions)
        {
            if (x.Name == extensionName)
                return x.Settings;
        }

        return null;
    }

    /// <summary>
    /// Do initial import here.
    /// If already imported, let extension manager take care of settings
    /// To reset, blogger has to delete all settings in the manager
    /// </summary>
    public static bool ImportSettings(ExtensionSettings settings)
    {
        foreach (Extension x in _extensions)
        {
            if (x.Name == settings.ExtensionName)
            {
                if (x.Settings == null)
                {
                    x.SaveSettings(settings);
                }
                break;
            }
        }

        SaveToCache();

        return SaveToXML();
    }
    #endregion

    #region Serialization
    public static bool SaveToXML()
    {
        try
        {
            using (TextWriter writer = new StreamWriter(_fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Extension>));
                serializer.Serialize(writer, _extensions);
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static void LoadFromXML()
    {
        TextReader reader = null;
        try
        {
            reader = new StreamReader(_fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Extension>));
            _extensions = (List<Extension>)serializer.Deserialize(reader);

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
            }
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }

    static void SaveToCache()
    {
        HttpContext.Current.Cache.Remove("Extensions");
        HttpContext.Current.Cache["Extensions"] = _extensions;
    }
    #endregion
}
