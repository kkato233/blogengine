using System;
using System.Collections.Generic;
using System.Linq;
using BlogEngine.Core.GalleryServer;

namespace BlogEngine.Core.Packaging
{
    /// <summary>
    /// Package manager
    /// </summary>
    public class PackageManager
    {
        //private const string _feedUrl = "http://localhost/feed/FeedService.svc";
        private const string _feedUrl = "http://dnbegallery.org/feed/FeedService.svc";

        /// <summary>
        /// Type of sort order
        /// </summary>
        public enum OrderType
        {
            /// <summary>
            /// Most downloaded
            /// </summary>
            Downloads,
            /// <summary>
            /// Heighest rated
            /// </summary>
            Rating,
            /// <summary>
            /// Alphabetical
            /// </summary>
            Alphanumeric
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkgType"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static IQueryable<PublishedPackage> GetPackages(string pkgType, int page, OrderType sortOrder)
        {
            var srs = new PackagingSource {FeedUrl = _feedUrl};

            //TODO: add setting for gallery page size
            var pageSize = BlogSettings.Instance.PostsPerPage;

            try
            {
                var pkgList = GetPackageList(srs,
                    packages =>
                    {
                        packages = packages.Where(p => p.PackageType == pkgType && p.IsLatestVersion);

                        packages = packages.OrderByDescending(p => p.DownloadCount).ThenBy(p => p.Title);

                        return packages;
                    });

                var retPackages = new List<PublishedPackage>();

                switch (sortOrder)
                {
                    case OrderType.Downloads:
                        retPackages = pkgList.Where(pkg => pkg.PackageType == pkgType && pkg.IsLatestVersion)
                            .OrderByDescending(p => p.DownloadCount).ThenBy(p => p.Title)
                            .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    case OrderType.Rating:
                        retPackages = pkgList.Where(pkg => pkg.PackageType == pkgType && pkg.IsLatestVersion)
                            .OrderByDescending(p => p.Rating).ThenBy(p => p.Title)
                            .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    case OrderType.Alphanumeric:
                        retPackages = pkgList.Where(pkg => pkg.PackageType == pkgType && pkg.IsLatestVersion)
                            .OrderBy(p => p.Title)
                            .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                }

                foreach (var pkg in retPackages)
                {
                    if (pkg.PackageType == "Theme" && pkg.Screenshots != null && pkg.Screenshots.Count > 0)
                    {
                        pkg.IconUrl = pkg.Screenshots[0].ScreenshotUri;
                    }
                }

                //retPackages.Sort((p1, p2) => p1.DownloadCount.CompareTo(p2.DownloadCount));

                return retPackages.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        static IEnumerable<PublishedPackage> GetPackageList(PackagingSource packagingSource = null, Func<IQueryable<PublishedPackage>, IQueryable<PublishedPackage>> query = null)
        {
            return (new[] { packagingSource })
            .SelectMany(
                source =>
                {
                    var galleryFeedContext = new GalleryFeedContext(new Uri(_feedUrl)) { IgnoreMissingProperties = true };
                    return galleryFeedContext.Packages.Expand("Screenshots");
                }
            );
        }
    }

    /// <summary>
    /// Package source
    /// </summary>
    public class PackagingSource
    {
        /// <summary>
        /// Feed id
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// Feed title
        /// </summary>
        public virtual string FeedTitle { get; set; }
        /// <summary>
        /// Feed url
        /// </summary>
        public virtual string FeedUrl { get; set; }
    }
}
