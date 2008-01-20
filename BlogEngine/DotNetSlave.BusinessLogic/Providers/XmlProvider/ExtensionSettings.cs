#region Using

using System;
using System.IO;
using System.Web.Hosting;
using BlogEngine.Core;

#endregion

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
        private static string _fileName = HostingEnvironment.MapPath(BlogSettings.Instance.StorageLocation + "extensions.xml");

        /// <summary>
        /// Loads the extension settings from XML file
        /// </summary>
        /// <returns></returns>
        public override Stream LoadExtensionSettings()
        {
            StreamReader reader = null;
            Stream str = null;
            try
            {
                reader = new StreamReader(_fileName);
                str = reader.BaseStream;
            }
            catch (Exception)
            {
                // to avoid runtime error. In the
                // worse case defaults will be loaded
            }
            return str;
        }

        /// <summary>
        /// Saves the extension settings to the provider.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public override void SaveExtensionSettings(Stream settings)
        {
            // saved to XML in the manager itself
        }
    }
}
