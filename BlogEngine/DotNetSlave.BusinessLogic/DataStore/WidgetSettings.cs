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
    public WidgetSettings()
    {
      base.ExType = ExtensionType.Widget;
      SettingsBehavior = new XMLDocumentBehavior();
    }
  }
}
