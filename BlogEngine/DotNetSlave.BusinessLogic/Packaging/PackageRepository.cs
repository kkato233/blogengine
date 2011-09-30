using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using BlogEngine.Core.Json;

namespace BlogEngine.Core.Packaging
{
    /// <summary>
    /// Package repository
    /// </summary>
    public class PackageRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkgType"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        public static List<JsonPackage> FromGallery(string pkgType, int page = 0, Gallery.OrderType sortOrder = Gallery.OrderType.Newest, string searchVal = "")
        {
            var packages = new List<JsonPackage>();
            var gallery = CachedPackages.Where(p => p.Location == "G" || p.Location == "I");

            if (pkgType != "all")
                gallery = gallery.Where(p => p.PackageType == pkgType).ToList();

            foreach (var pkg in gallery)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}|{1}|{2}|{3}", 
                    pkg.Id, pkg.OnlineVersion, pkg.PackageType, pkg.Location));

                if (string.IsNullOrEmpty(searchVal))
                {
                    packages.Add(pkg);
                }
                else
                {
                    if (pkg.Title.IndexOf(searchVal, StringComparison.OrdinalIgnoreCase) != -1
                        || pkg.Description.IndexOf(searchVal, StringComparison.OrdinalIgnoreCase) != -1
                        ||
                        (!string.IsNullOrWhiteSpace(pkg.Tags) &&
                         pkg.Tags.IndexOf(searchVal, StringComparison.OrdinalIgnoreCase) != -1))
                    {
                        packages.Add(pkg);
                    }
                }
            }

            Gallery.GalleryPager = new Pager(page, packages.Count, pkgType);

            if (packages.Count > 0)
            {
                switch (sortOrder)
                {
                    case Gallery.OrderType.Downloads:
                        packages = packages.OrderByDescending(p => p.DownloadCount).ThenBy(p => p.Title).ToList();
                        break;
                    case Gallery.OrderType.Rating:
                        packages = packages.OrderByDescending(p => p.Rating).ThenBy(p => p.Title).ToList();
                        break;
                    case Gallery.OrderType.Newest:
                        packages = packages.OrderByDescending(p => Convert.ToDateTime(p.LastUpdated)).ThenBy(p => p.Title).ToList();
                        break;
                    case Gallery.OrderType.Alphanumeric:
                        packages = packages.OrderBy(p => p.Title).ToList();
                        break;
                }
            }
            return packages.Skip(page * Constants.PageSize).Take(Constants.PageSize).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkgType"></param>
        /// <returns></returns>
        public static List<JsonPackage> LocalPackages(string pkgType)
        {
            var packages = new List<JsonPackage>();

            foreach (var pkg in CachedPackages)
            {
                if(pkgType != "all")
                {
                    if (pkg.PackageType != pkgType) continue;
                }

                if(pkg.Location == "G")
                    continue;

                packages.Add(pkg);
            }
            return packages;
        }

        /// <summary>
        /// Package by ID
        /// </summary>
        /// <param name="pkgId"></param>
        /// <returns></returns>
        public static JsonPackage GetPackage(string pkgId)
        {
            return CachedPackages.FirstOrDefault(pkg => pkg.Id == pkgId);
        }

        /// <summary>
        /// Local "packages" - list of extensions,
        /// themes and widgets folders
        /// </summary>
        static IEnumerable<JsonPackage> CachedPackages
        {
            get
            {
                if (Blog.CurrentInstance.Cache[Constants.CacheKey] == null)
                {
                    Blog.CurrentInstance.Cache.Add(
                        Constants.CacheKey,
                        LoadPackages(),
                        null,
                        Cache.NoAbsoluteExpiration,
                        new TimeSpan(0, 15, 0),
                        CacheItemPriority.Low,
                        null);
                }
                return (IEnumerable<JsonPackage>)Blog.CurrentInstance.Cache[Constants.CacheKey];
            }
        }

        static List<JsonPackage> LoadPackages()
        {
            var packages = new List<JsonPackage>();

            Gallery.Load(packages);
            FileSystem.Load(packages);
            Installer.Load(packages);

            return packages;
        }
    }
}
