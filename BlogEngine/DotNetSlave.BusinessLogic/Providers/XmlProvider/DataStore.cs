using System;
using System.IO;
using BlogEngine.Core.DataStore;
using System.Xml.Serialization;

namespace BlogEngine.Core.Providers
{
  /// <summary>
  /// A storage provider for BlogEngine that uses XML files.
  /// <remarks>
  /// To build another provider, you can just copy and modify
  /// this one. Then add it to the web.config's BlogEngine section.
  /// </remarks>
  /// </summary>
  public partial class XmlBlogProvider : BlogProvider
  {
    private static string _storageDirectory = HostingEnvironment.MapPath(BlogSettings.Instance.StorageLocation + "datastore");

    /// <summary>
    /// Loads settings from generic data store
    /// </summary>
    /// <param name="exType">Extension Type</param>
    /// <param name="exId">Extension ID</param>
    /// <returns>Stream Settings</returns>
    public override Stream LoadFromDataStore(ExtensionType exType, string exId)
    {
      string _fileName = FileName(exType, exId);
      StreamReader reader = null;
      Stream str = null;
      try
      {
        reader = new StreamReader(_fileName);
        str = reader.BaseStream;
      }
      catch (Exception)
      {
        throw;
      }
      return str;
    }

    /// <summary>
    /// Save settings to generic data store
    /// </summary>
    /// <param name="exType">Type of extension</param>
    /// <param name="exId">Extension ID</param>
    /// <param name="settings">Stream Settings</param>
    public override void SaveToDataStore(ExtensionType exType, string exId, object settings)
    {
      string _fileName = FileName(exType, exId);
      try
      {
        using (TextWriter writer = new StreamWriter(_fileName))
        {
          XmlSerializer x = new XmlSerializer(settings.GetType());
          x.Serialize(writer, settings);
        }
      }
      catch (Exception e)
      {
        string s = e.Message;
        throw;
      }
    }

    /// <summary>
    /// File name to store settings to
    /// </summary>
    /// <param name="exType">Extension Type</param>
    /// <param name="exId">Extension ID</param>
    /// <returns>String file name</returns>
    private string FileName(ExtensionType exType, string exId)
    {
      string fileName = string.Empty;
      switch (exType)
      {
        case ExtensionType.Extension:
          fileName += @"\extensions\" + exId + ".xml";
          break;
        case ExtensionType.Widget:
          fileName += @"\widgets\" + exId + ".xml";
          break;
        case ExtensionType.Theme:
          fileName += @"\themes\" + exId + ".xml";
          break;
        default:
          break;
      }


      return _storageDirectory + fileName;
    }
  }
}

