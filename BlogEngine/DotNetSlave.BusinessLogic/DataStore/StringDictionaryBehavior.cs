using System;
using BlogEngine.Core.Providers;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BlogEngine.Core.DataStore
{
  class StringDictionaryBehavior : ISettingsBehavior
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public StringDictionaryBehavior() { }

    private static BlogProviderSection _section = (BlogProviderSection)ConfigurationManager.GetSection("BlogEngine/blogProvider");
    /// <summary>
    /// Saves String Dictionary to Data Store
    /// </summary>
    /// <param name="exType">Extension Type</param>
    /// <param name="exId">Extension Id</param>
    /// <param name="settings">StringDictionary settings</param>
    /// <returns></returns>
    public bool SaveSettings(ExtensionType exType, string exId, object settings)
    {
      try
      {
        StringDictionary sd = (StringDictionary)settings;
        SerializableStringDictionary ssd = new SerializableStringDictionary();

        foreach (DictionaryEntry de in sd)
        {
          ssd.Add(de.Key.ToString(), de.Value.ToString());
        }

        BlogService.SaveToDataStore(exType, exId, ssd);
        return true;
      }
      catch (Exception e)
      {
        string s = e.Message;
        throw;
      }
    }

    /// <summary>
    /// Retreaves StringDictionary object from database or file system
    /// </summary>
    /// <param name="exType">Extension Type</param>
    /// <param name="exId">Extension Id</param>
    /// <returns>StringDictionary object as Stream</returns>
    public object GetSettings(ExtensionType exType, string exId)
    {
      string s = (string)BlogService.LoadFromDataStore(exType, exId);
      SerializableStringDictionary ssd;
      StringDictionary sd = new StringDictionary();

      if (!string.IsNullOrEmpty(s))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(SerializableStringDictionary));
        using (StringReader reader = new StringReader(s))
        {
          ssd = (SerializableStringDictionary)serializer.Deserialize(reader);
        }
        sd = (StringDictionary)ssd;
      }
      return sd;
    }
  }
}
