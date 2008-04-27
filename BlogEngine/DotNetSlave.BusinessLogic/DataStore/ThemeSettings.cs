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
  }
}
