using System;
using System.IO;
using System.Globalization;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using System.Configuration.Provider;
using BlogEngine.Core;


namespace BlogEngine.Core.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlMultiBlogProvider : XmlBlogProvider
    {
        //private string _XmlFileDirectory = "";
        private bool _bEnableMultiByHostname = false;
        private bool _bEnableMultiBySubdirectory = false;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string StorageLocation()
        {

            string lsStorageLocation = string.Empty;// = _XmlFileDirectory;
            if (_bEnableMultiByHostname)
            {
                //Add the hostname as a subdirectory of the configured storage location
                string subdomain = Utils.GetSubDomain(HttpContext.Current.Request.Url);
                if (!string.IsNullOrEmpty(subdomain))
                    subdomain = subdomain.Replace("www", string.Empty);
                lsStorageLocation += System.Web.Configuration.WebConfigurationManager.AppSettings["StorageLocation"] + subdomain + HttpContext.Current.Request.Url.DnsSafeHost + HttpContext.Current.Request.Url.Port.ToString() + "/";
            }
            if (_bEnableMultiBySubdirectory)
            {
                string segment = HttpContext.Current.Request.Url.Segments[1].ToLowerInvariant();
                //Add the subdirectory as a subdirectory of the configured storage location
                if (!segment.Contains("."))
                    lsStorageLocation += HttpContext.Current.Request.Url.Segments[1].ToLowerInvariant() + "/";
            }
            return lsStorageLocation;

        }

        void CheckIfInstalIsNecessary()
        {

            string p = StorageLocation();

            //string folder = System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, p);
            string folder = System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, p);
            folder = folder.Replace("~/", "");
            folder = folder.Replace("/", @"\");

            string defaultDataFolder = HttpContext.Current.Server.MapPath("~/setup/DefaultInstall/");

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(p)))
            {
                object _lock = new object();
                lock (_lock)
                {
                    //Create the folder with default files in the storage location
                    Directory.CreateDirectory(folder);
                    Utils.RecursiveDirectoryCopy(defaultDataFolder, folder, true, false);
                }
            }
        }


        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="name">Configuration name</param>
        /// <param name="config">Configuration settings</param>
        public override void Initialize(string name, NameValueCollection config)
        {

            if (config == null)
                throw new ArgumentNullException("config");


            if (String.IsNullOrEmpty(name))
                name = "XmlMultiBlogProvider";

            if (Type.GetType("Mono.Runtime") != null)
            {
                // Mono dies with a "Unrecognized attribute: description" if a description is part of the config.
                if (!string.IsNullOrEmpty(config["description"]))
                {
                    config.Remove("description");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(config["description"]))
                {
                    config.Remove("description");
                    config.Add("description", "XML Multi BlogEngine provider");
                }
            }

            base.Initialize(name, config);

            if (!String.IsNullOrEmpty(config["enableMultiByHostname"]))
            {
                bool.TryParse(config["enableMultiByHostname"], out _bEnableMultiByHostname);
                config.Remove("enableMultiByHostname");
            }

            if (!String.IsNullOrEmpty(config["enableMultiBySubdirectory"]))
            {
                bool.TryParse(config["enableMultiBySubdirectory"], out _bEnableMultiBySubdirectory);
                config.Remove("enableMultiBySubdirectory");
            }

            string path = string.Empty;

            if (String.IsNullOrEmpty(path))
                path = StorageLocation();


            if (!String.IsNullOrEmpty(config["enabled"]))
            {

                if (bool.Parse(config["enabled"]))
                {
                    CheckIfInstalIsNecessary();
                }
                config.Remove("enabled");
            }


            // Make sure we have permission to read the XML data source and
            // throw an exception if we don't
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, HostingEnvironment.MapPath(StorageLocation()));
            permission.Demand();

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException("Unrecognized attribute: " + attr);
            }






            //}



        }


    }
}
