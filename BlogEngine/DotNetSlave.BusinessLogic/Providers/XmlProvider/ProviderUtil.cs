using System;
using System.Configuration;
using System.IO;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace BlogEngine.Core.Providers
{

    /// <summary>
    /// Summary description for ProviderUtil
    /// </summary>
    internal static class ProviderUtil
    {

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        public static void EnsureDataFoler()
        {

            if (HttpContext.Current != null)
            {
                string folder = HttpContext.Current.Server.MapPath("~/App_Data/");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
        }

        /// <summary>
        /// Gets the config value.
        /// </summary>
        /// <param name="configValue">The config value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetConfigValue(string configValue, string defaultValue)
        {
            return (String.IsNullOrEmpty(configValue))
                ? defaultValue : configValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string MapPath(string virtualPath)
        {

            string path = VirtualPathUtility.ToAbsolute(virtualPath);
            if (path.Length > 0) path = path.Substring(1);
            return string.Concat(HttpRuntime.AppDomainAppPath, path.Replace('/', '\\'));
        }
        #endregion
    }
}