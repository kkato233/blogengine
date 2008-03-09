using System;
namespace BlogEngine.Core.DataStore
{
  /// <summary>
  /// Extension settings implementation
  /// </summary>
  public class ExtensionSettings : SettingsBase
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public ExtensionSettings()
    {
      base.ExType = ExtensionType.Extension;
      SettingsBehavior = new ExtensionSettingsBehavior();
    }
  }
}