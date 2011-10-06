﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Web;
using BlogEngine.Core.Json;
using BlogEngine.Core.Providers;

namespace BlogEngine.Core.Packaging
{
    /// <summary>
    /// Class for packaging IO
    /// </summary>
    public class FileSystem
    {
        private static int fileOrder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packages"></param>
        public static void Load(List<JsonPackage> packages)
        {
            try
            {
                var themes = GetThemes();
                var widgets = GetWidgets();
                
                if(themes != null && themes.Count > 0)
                {
                    foreach (var theme in from theme in themes
                        let found = packages.Any(pkg => theme.Id == pkg.Id)
                        where !found select theme)
                    {
                        packages.Add(theme);
                    }
                }

                if (widgets != null && widgets.Count > 0)
                {
                    foreach (var wdg in from wdg in widgets
                        let found = packages.Any(pkg => wdg.Id == pkg.Id)
                        where !found select wdg)
                    {
                        packages.Add(wdg);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.Log(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Copy package files
        /// </summary>
        /// <param name="pkgId">Package Id</param>
        /// <param name="version">Package Version</param>
        public static List<PackageFile> CopyPackageFiles(string pkgId, string version)
        {
            var src = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot +
                string.Format("App_Data/packages/{0}.{1}/content", pkgId, version));

            var tgt = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot);

            var source = new DirectoryInfo(src);
            var target = new DirectoryInfo(tgt);
            var packgeFiles = new List<PackageFile>();

            fileOrder = 0;
            CopyDirectory(source, target, pkgId, packgeFiles);

            return packgeFiles;
        }

        /// <summary>
        /// Remove package files
        /// </summary>
        /// <param name="pkgId">Package Id</param>
        public static void RemovePackageFiles(string pkgId)
        {
            var installedFiles = BlogService.InstalledFromGalleryPackageFiles(pkgId);

            foreach (var file in installedFiles.OrderByDescending(f => f.FileOrder))
            {
                if(file.IsDirectory)
                {
                    var folder = new DirectoryInfo(file.FilePath);
                    if (folder.Exists)
                    {
                        if(folder.GetFileSystemInfos().Length == 0)
                        {
                            ForceDeleteDirectory(file.FilePath);
                        }
                        else
                        {
                            Utils.Log(string.Format("Package Uninstaller: can not remove directory if it is not empty ({0})", file.FilePath));
                        }
                    }

                }else
                {
                    File.Delete(file.FilePath);
                }
            }
        }

        #region Private methods

        static List<JsonPackage> GetThemes()
        {
            var installedThemes = new List<JsonPackage>();
            var path = HttpContext.Current.Server.MapPath(string.Format("{0}themes/", Utils.ApplicationRelativeWebRoot));

            foreach (var p in from d in Directory.GetDirectories(path)
                let index = d.LastIndexOf(Path.DirectorySeparatorChar) + 1
                select d.Substring(index)
                into themeId select GetPackageManifest(themeId, Constants.Theme) ?? 
                new JsonPackage {Id = themeId, PackageType = Constants.Theme, Location = "L"}
                into p where p.Id != "RazorHost" select p)
            {
                if (string.IsNullOrEmpty(p.IconUrl))
                    p.IconUrl = DefaultIconUrl(p);

                installedThemes.Add(p);
            }
            return installedThemes;
        }

        static List<JsonPackage> GetWidgets()
        {
            var installedWidgets = new List<JsonPackage>();
            var path = HttpContext.Current.Server.MapPath(string.Format("{0}widgets/", Utils.ApplicationRelativeWebRoot));

            foreach (var p in from d in Directory.GetDirectories(path)
                let index = d.LastIndexOf(Path.DirectorySeparatorChar) + 1
                select d.Substring(index)
                into widgetId select GetPackageManifest(widgetId, Constants.Widget) ?? 
                new JsonPackage {Id = widgetId, PackageType = Constants.Widget, Location = "L"})
            {
                if (string.IsNullOrEmpty(p.IconUrl))
                    p.IconUrl = DefaultIconUrl(p);

                installedWidgets.Add(p);
            }
            return installedWidgets;
        }

        static JsonPackage GetPackageManifest(string id, string pkgType)
        {
            var jp = new JsonPackage { Id = id, PackageType = pkgType };

            var pkgUrl = pkgType == "Theme" ?
                string.Format("{0}themes/{1}/theme.xml", Utils.ApplicationRelativeWebRoot, id) :
                string.Format("{0}widgets/{1}/widget.xml", Utils.ApplicationRelativeWebRoot, id);

            var pkgPath = HttpContext.Current.Server.MapPath(pkgUrl);
            try
            {
                if (File.Exists(pkgPath))
                {
                    var textReader = new XmlTextReader(pkgPath);
                    textReader.Read();

                    while (textReader.Read())
                    {
                        textReader.MoveToElement();

                        if (textReader.Name == "description")
                            jp.Description = textReader.ReadString();

                        if (textReader.Name == "authors")
                            jp.Authors = textReader.ReadString();

                        if (textReader.Name == "website")
                            jp.Website = textReader.ReadString();

                        if (textReader.Name == "version")
                            jp.LocalVersion = textReader.ReadString();

                        if (textReader.Name == "iconurl")
                            jp.IconUrl = textReader.ReadString();
                    }
                    return jp;
                }
            }
            catch (Exception ex)
            {
                Utils.Log("Packaging.FileSystem.GetPackageManifest", ex);
            }
            return null;
        }

        static void CopyDirectory(DirectoryInfo source, DirectoryInfo target, string pkgId, List<PackageFile> installedFiles)
        {
            foreach (var dir in source.GetDirectories())
            {
                // save directory if it is created by package
                // so we can remove it on package uninstall
                if (!Directory.Exists(Path.Combine(target.FullName, dir.Name)))
                {
                    fileOrder++;
                    var fileToCopy = new PackageFile
                    {
                        FilePath = Path.Combine(target.FullName, dir.Name),
                        PackageId = pkgId,
                        FileOrder = fileOrder,
                        IsDirectory = true
                    };
                    installedFiles.Add(fileToCopy);
                }                   
                CopyDirectory(dir, target.CreateSubdirectory(dir.Name), pkgId, installedFiles);
            }
             
            foreach (var file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name));

                fileOrder++;
                var fileToCopy = new PackageFile
                {
                    FileOrder = fileOrder,
                    IsDirectory = false,
                    FilePath = Path.Combine(target.FullName, file.Name),
                    PackageId = pkgId
                };
                installedFiles.Add(fileToCopy);
            }   
        }

        static void ForceDeleteDirectory(string path)
        {
            DirectoryInfo fol;
            var fols = new Stack<DirectoryInfo>();
            var root = new DirectoryInfo(path);
            fols.Push(root);
            while (fols.Count > 0)
            {
                fol = fols.Pop();
                fol.Attributes = fol.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                foreach (DirectoryInfo d in fol.GetDirectories())
                {
                    fols.Push(d);
                }
                foreach (FileInfo f in fol.GetFiles())
                {
                    f.Attributes = f.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                    f.Delete();
                }
            }
            root.Delete(true);
        }

        static string DefaultIconUrl(JsonPackage pkg)
        {
            var validImages = new List<string> {"screenshot.jpg", "screenshot.png", "theme.jpg", "theme.png"};
            var pkgDir = pkg.PackageType == "Widget" ? "widgets" : "themes";

            foreach (var img in validImages)
            {
                var url = string.Format("{0}{1}/{2}/{3}",
                Utils.ApplicationRelativeWebRoot, pkgDir, pkg.Id, img);

                var path = HttpContext.Current.Server.MapPath(url);

                if (File.Exists(path)) return url;
            }

            if (pkg.PackageType == "Widget") 
                return Utils.ApplicationRelativeWebRoot + "pics/Widget.png";

            return Utils.ApplicationRelativeWebRoot + "pics/Theme.png";
        }

        #endregion
    }
}
