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
				SaveToCache(); ;
				break;
			}
		}
	}

	static void LoadExtensions()
	{   // initialize on application load
		if (HttpContext.Current.Cache["Extensions"] == null)
		{
			if (File.Exists(_fileName))
			{
				LoadFromXML();
			}
			else  // very first run
			{
				LoadFromAssembly();
				SaveToXML();
			}
			SaveToCache();
		}
	}

	static void SaveToCache()
	{
		HttpContext.Current.Cache.Remove("Extensions");
		HttpContext.Current.Cache["Extensions"] = _extensions;

		foreach (Extension x in _extensions)
		{
			HttpContext.Current.Cache.Remove("x:" + x.Name);
			if (x.Parameters.Count > 0)
			{
				Dictionary<string, string[]> settings = new Dictionary<string, string[]>();
				foreach (ExtParameter par in x.Parameters)
				{
					settings.Add(par.Name, par.Value.Split(','));
				}
				HttpContext.Current.Cache["x:" + x.Name] = settings;
			}
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

	#region Settings
	public static Dictionary<string, string[]> Settings(string extension)
	{
		Dictionary<string, string[]> settings = new Dictionary<string, string[]>();
		foreach (Extension x in _extensions)
		{
			if (x.Name == extension)
				return x.Settings;
		}
		return settings;
	}

	public static void SaveSettings(string extension, Dictionary<string, string[]> settings)
	{
		foreach (Extension x in _extensions)
		{
			if (x.Name == extension)
				x.SaveSettings(settings);
		}
		SaveToXML();
		SaveToCache();
	}
	#endregion

	#region Serialization
	public static void SaveToXML()
	{
		try
		{
			using (TextWriter writer = new StreamWriter(_fileName))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<Extension>));
				serializer.Serialize(writer, _extensions);
			}
		}
		catch (Exception)
		{
			// No write access to App_Data folder. Do nothing.
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
	#endregion
}
