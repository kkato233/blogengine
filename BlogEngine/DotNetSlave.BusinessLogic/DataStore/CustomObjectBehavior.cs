using System;
using BlogEngine.Core.Providers;
using System.IO;

namespace BlogEngine.Core.DataStore
{
  class CustomObjectBehavior : ISettingsBehavior
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public CustomObjectBehavior() { }

    /// <summary>
    /// Saves extension to database or file system
    /// </summary>
    /// <param name="exType">Extension Type</param>
    /// <param name="exId">Extension ID</param>
    /// <param name="settings">Extension object</param>
    /// <returns>True if saved</returns>
    public bool SaveSettings(ExtensionType exType, string exId, object settings)
    {
      try
      {
        BlogService.SaveToDataStore(exType, exId, settings);
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Retreaves extension object from database or file system
    /// </summary>
    /// <param name="exType">Extension Type</param>
    /// <param name="exId">Extension ID</param>
    /// <returns>Extension object as Stream</returns>
    public object GetSettings(ExtensionType exType, string exId)
    {
      return BlogService.LoadFromDataStore(exType, exId);
    }
  }
}

