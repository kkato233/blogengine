using BlogEngine.Core.Data.Models;
using Newtonsoft.Json;
using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BlogEngine.Core.Packaging
{
    /// <summary>
    /// Online gallery
    /// </summary>
    public static class Gallery
    {
        /// <summary>
        /// Load gallery packages
        /// </summary>
        /// <param name="packages">Packages to load</param>
        public static void Load(List<Package> packages)
        {
            try
            {
                var packs = GetNugetPackages().ToList();
                var extras = GetPackageExtras();

                foreach (var pkg in packs)
                {
                    if (pkg.IsLatestVersion)
                    {
                        var jp = new Package
                        {
                            Id = pkg.Id,
                            Authors = pkg.Authors == null ? "unknown" : string.Join(", ", pkg.Authors),
                            Description = pkg.Description.Length > 140 ? string.Format("{0}...", pkg.Description.Substring(0, 140)) : pkg.Description,
                            DownloadCount = pkg.DownloadCount,
                            LastUpdated = pkg.Published != null ? pkg.Published.Value.ToString("yyyy-MM-dd HH:mm") : "", // format for sort order to work with strings
                            Title = pkg.Title,
                            OnlineVersion = pkg.Version.ToString(),
                            Website = pkg.ProjectUrl == null ? null : pkg.ProjectUrl.ToString(),
                            Tags = pkg.Tags,
                            IconUrl = pkg.IconUrl == null ? null : pkg.IconUrl.ToString()
                        };

                        if (!string.IsNullOrEmpty(jp.IconUrl) && !jp.IconUrl.StartsWith("http:"))
                            jp.IconUrl = Constants.GalleryUrl + jp.IconUrl;

                        if (string.IsNullOrEmpty(jp.IconUrl))
                            jp.IconUrl = DefaultThumbnail("");

                        if (extras != null && extras.Count() > 0)
                        {
                            var dnbePkg = extras.Where(e => e.Id.ToLower() == pkg.Id.ToLower() + "." + pkg.Version).FirstOrDefault();

                            if (dnbePkg != null)
                            {
                                jp.DownloadCount = dnbePkg.DownloadCount;
                                jp.Rating = dnbePkg.Rating;
                            }
                        }
                        packages.Add(jp);
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.Log("BlogEngine.Core.Packaging.Load", ex);
            }   
        }

        /// <summary>
        /// Convert version from string to int for comparison
        /// </summary>
        /// <param name="version">string version</param>
        /// <returns>int version</returns>
        public static int ConvertVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
                return 0;

            int numVersion;
            Int32.TryParse(version.Replace(".", ""), out numVersion);
            return numVersion;
        }

        /// <summary>
        /// Package URL
        /// </summary>
        /// <param name="pkgType">Package Type</param>
        /// <param name="pkgId">Package ID</param>
        /// <returns></returns>
        public static string PackageUrl(string pkgType, string pkgId)
        {
            switch (pkgType)
            {
                case "Theme":
                    return string.Format("{0}/List/Themes/{1}", Constants.GalleryAppUrl, pkgId);
                case "Extension":
                    return string.Format("{0}/List/Extensions/{1}", Constants.GalleryAppUrl, pkgId);
                case "Widget":
                    return string.Format("{0}/List/Widgets/{1}", Constants.GalleryAppUrl, pkgId);
            }
            return string.Empty;
        }

        #region Package extras

        /// <summary>
        /// Gets extra filds from remote gallery if gallery supports it
        /// </summary>
        /// <param name="id">Package ID</param>
        /// <returns>Object with extra package fields</returns>
        public static PackageExtra GetPackageExtra(string id)
        {
            try
            {
                var url = BlogSettings.Instance.GalleryFeedUrl.Replace("/nuget", "/api/extras/" + id);
                WebClient wc = new WebClient();
                string json = wc.DownloadString(url);
                return JsonConvert.DeserializeObject<PackageExtra>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// BlogEngine.Gallery implements Nuget.Server
        /// and adds fields like download counts, reviews, ratings etc.
        /// </summary>
        /// <returns>List of extra fields if exist</returns>
        public static IEnumerable<PackageExtra> GetPackageExtras()
        {
            try
            {
                var url = BlogSettings.Instance.GalleryFeedUrl.Replace("/nuget", "/api/extras");
                WebClient wc = new WebClient();
                string json = wc.DownloadString(url);
                return JsonConvert.DeserializeObject<List<PackageExtra>>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Send rating/review if gallery supports it
        /// </summary>
        /// <param name="id">Package ID</param>
        /// <param name="review">Review</param>
        /// <returns>True on success</returns>
        public static bool RatePackage(string id, Review review)
        {
            try
            {
                var url = BlogSettings.Instance.GalleryFeedUrl.Replace("/nuget", "/api/review?pkgId=" + id);
                WebClient wc = new WebClient();
                var data = JsonConvert.SerializeObject(review);
                wc.Headers.Add("content-type", "application/json");
                wc.UploadString(url, "PUT", data);
                return true;
            }
            catch (Exception ex)
            {
                Utils.Log("Error rating package", ex);
                return false;
            }
        }

        #endregion

        #region Private methods

        static IEnumerable<IPackage> GetNugetPackages()
        {
            var rep = PackageRepositoryFactory.Default.CreateRepository(BlogSettings.Instance.GalleryFeedUrl);
            return rep.GetPackages();
        }

        static string DefaultThumbnail(string packageType)
        {
            switch (packageType)
            {
                case "Theme":
                    return string.Format("{0}pics/Theme.png", Utils.ApplicationRelativeWebRoot);
                case "Extension":
                    return string.Format("{0}pics/ext.png", Utils.ApplicationRelativeWebRoot);
                case "Widget":
                    return string.Format("{0}pics/Widget.png", Utils.ApplicationRelativeWebRoot);
            }
            return string.Format("{0}pics/pkg.png", Utils.ApplicationRelativeWebRoot);
        }

        #endregion
    }
}