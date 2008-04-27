using System;

namespace BlogEngine.Core.DataStore
{
  /// <summary>
  /// WidgetSettings implementation
  /// </summary>
  public class WidgetSettings : SettingsBase
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public WidgetSettings(string setId)
    {
      SettingID = setId;
      ExType = ExtensionType.Widget;
      SettingsBehavior = new StringDictionaryBehavior();
    }
    ///// <summary>
    ///// Overload to be able to save different objects
    ///// </summary>
    ///// <param name="setId">Setting Id</param>
    ///// <param name="objType">Object Type</param>
    //public WidgetSettings(string setId, Type objType)
    //{
    //  SettingID = setId;
    //  ExType = ExtensionType.Widget;

    //  if (objType.Name == "StringDictionary")
    //  {
    //    SettingsBehavior = new StringDictionaryBehavior();
    //  }
    //  else if (objType.Name == "XmlDocument")
    //  {
    //    SettingsBehavior = new XMLDocumentBehavior();
    //  }
    //  else if (objType.Name == "CustomObjectBase")
    //  {
    //    SettingsBehavior = new CustomObjectBehavior();
    //  }
    //  else
    //  {
    //    throw new ApplicationException(objType.Name + " is not supported by Data Store");
    //  }
    //}
  }
}