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
        // "http://localhost/feed/FeedService.svc";
        // "http://dnbegallery.org/feed/FeedService.svc";
        private static readonly string _feedUrl = BlogSettings.Instance.GalleryFeedUrl; 

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
            /// Newest
            /// </summary>
            Newest,
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
        /// Gallery pager
        /// </summary>
        public static Pager GalleryPager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkgType"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        public static IQueryable<PublishedPackage> GetPackages(string pkgType, int page = 1, OrderType sortOrder = OrderType.Newest, string searchVal = "")
        {
            var srs = new PackagingSource {FeedUrl = _feedUrl};

            //TODO: add setting for gallery page size
            var pageSize = BlogSettings.Instance.PostsPerPage;
            var cnt = 0;

            try
            {
                var retPackages = new List<PublishedPackage>();
                var pkgList = GetPackageList(srs,
                    packages => packages.Where(p => p.PackageType == pkgType && p.IsLatestVersion));

                foreach (var pkg in pkgList.ToList())
                {
                    if (pkg.PackageType != pkgType || !pkg.IsLatestVersion) continue;

                    if (pkg.PackageType == "Theme" && pkg.Screenshots != null && pkg.Screenshots.Count > 0)
                        pkg.IconUrl = pkg.Screenshots[0].ScreenshotUri;

                    if(string.IsNullOrEmpty(searchVal))
                    {
                        retPackages.Add(pkg);
                        cnt++;
                    }
                    else
                    {
                        if(pkg.Title.Contains(searchVal) 
                           || pkg.Description.Contains(searchVal) 
                           || (!string.IsNullOrWhiteSpace(pkg.Tags) && pkg.Tags.Contains(searchVal)))
                        {
                            retPackages.Add(pkg);
                            cnt++;
                        }
                    }
                }
                
                GalleryPager = new Pager(page, cnt);

                if (cnt > 0)
                {
                    switch (sortOrder)
                    {
                        case OrderType.Downloads:
                            retPackages = retPackages
                                .OrderByDescending(p => p.DownloadCount).ThenBy(p => p.Title)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                            break;
                        case OrderType.Rating:
                            retPackages = retPackages
                                .OrderByDescending(p => p.Rating).ThenBy(p => p.Title)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                            break;
                        case OrderType.Newest:
                            retPackages = retPackages
                                .OrderByDescending(p => p.LastUpdated).ThenBy(p => p.Title)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                            break;
                        case OrderType.Alphanumeric:
                            retPackages = retPackages.OrderBy(p => p.Title)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                            break;
                    }
                }
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
