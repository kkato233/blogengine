using System;

namespace BlogEngine.Core.DataStore
{
  /// <summary>
  /// ThemeSettings implementation
  /// </summary>
  public class ThemeSettings : SettingsBase
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public ThemeSettings(string setId)
    {
      SettingID = setId;
      ExType = ExtensionType.Theme;
      SettingsBehavior = new StringDictionaryBehavior();
    }

    /// <summary>
    /// Overload to be able to save different objects
    /// </summary>
    /// <param name="setId">Setting Id</param>
    /// <param name="objType">Object Type</param>
    public ThemeSettings(string setId, Type objType)
    {
      SettingID = setId;
      ExType = ExtensionType.Theme;

      if (objType.Name.Contains("StringDictionary"))
      {
        SettingsBehavior = new StringDictionaryBehavior();
      }
      else if (objType.Name == "XmlDocument")
      {
        SettingsBehavior = new XMLDocumentBehavior();
      }
      else if (objType.Name == "CustomObjectBase")
      {
        SettingsBehavior = new CustomObjectBehavior();
      }
      else
      {
        throw new ApplicationException(objType.Name + " is not supported by Data Store");
      }
    }
  }
}
