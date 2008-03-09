using System;
using System.Xml;
using BlogEngine.Core.Providers;
using System.IO;

namespace BlogEngine.Core.DataStore
{
  /// <summary>
  /// Class to encapsulate saving and retreaving 
  /// XML documents to and from data storage
  /// </summary>
  public class XMLDocumentBehavior : ISettingsBehavior   
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public XMLDocumentBehavior()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    /// <summary>
    /// Saves XML document to data storage
    /// </summary>
    /// <param name="exType">Extension Type</param>
    /// <param name="exId">Extension ID</param>
    /// <param name="settings">Settings as XML document</param>
    /// <returns>True if saved</returns>
    public bool SaveSettings(ExtensionType exType, string exId, object settings)
    {
      Stream stm = (Stream)settings;
      BlogService.SaveToDataStore(exType, exId, settings);
      return true;
    }

    /// <summary>
    /// Gets settings from data store
    /// </summary>
    /// <param name="exType">Extension Type</param>
    /// <param name="exId">Extension ID</param>
    /// <returns>Settings as Stream</returns>
    public object GetSettings(ExtensionType exType, string exId)
    {
      return BlogService.LoadFromDataStore(exType, exId);
    }
  }
}
