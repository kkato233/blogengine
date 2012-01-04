namespace BlogEngine.Core.Notes
{
    /// <summary>
    /// Settings
    /// </summary>
    public class QuickSetting
    {
        /// <summary>
        /// Each author can have individual settings
        /// driving quick notes behavior
        /// </summary>
        public string Author { get; set; }

        public string SettingName { get; set; }

        public string SettingValue { get; set; }
    }
}