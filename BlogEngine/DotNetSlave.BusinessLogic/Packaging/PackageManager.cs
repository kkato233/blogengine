using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using BlogEngine.Core.GalleryServer;
using BlogEngine.Core.Json;
using NuGet;

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
                var pkgList = GetPackageList(srs);

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

        /// <summary>
        /// Installed themes
        /// </summary>
        /// <returns></returns>
        public static List<JsonPackage> InstalledThemes()
        {
            var installedThemes = new List<JsonPackage>();
            var path = HttpContext.Current.Server.MapPath(string.Format("{0}themes/", Blog.CurrentInstance.RelativeWebRoot));

            foreach (var dic in Directory.GetDirectories(path))
            {
                var jp = new JsonPackage();
                var index = dic.LastIndexOf(Path.DirectorySeparatorChar) + 1;
                
                var themeFile = dic + @"\theme.png";
                jp.Id = dic.Substring(index);

                jp.IconUrl = File.Exists(themeFile)
                               ? string.Format("{0}themes/{1}/theme.png", Blog.CurrentInstance.RelativeWebRoot, jp.Id)
                               : Blog.CurrentInstance.RelativeWebRoot + "pics/Theme.png";

                if (jp.Id != BlogSettings.Instance.Theme && jp.Id != BlogSettings.Instance.MobileTheme)
                {
                    installedThemes.Add(jp);
                }
            }

            return installedThemes;
        }

        /// <summary>
        /// Install package
        /// </summary>
        /// <param name="pkgId"></param>
        public static JsonResponse InstallPackage(string pkgId)
        {
            try
            {
                var repo =
                PackageRepositoryFactory.Default.CreateRepository(
                    new PackageSource("http://dnbegallery.org/feed/FeedService.svc", "Default"));

                var packageManager = new NuGet.PackageManager(
                    repo,
                    new DefaultPackagePathResolver("http://dnbegallery.org/feed/FeedService.svc"),
                    new PhysicalFileSystem(HttpContext.Current.Server.MapPath(Utils.RelativeWebRoot + "App_Data/packages"))
                );

                var package = repo.FindPackage(pkgId);

                packageManager.InstallPackage(package, false);

                CopyPackageFiles(package.Id, package.Version.ToString());
            }
            catch (Exception ex)
            {
                Utils.Log("PackageManager.InstallPackage", ex);
                return new JsonResponse() { Success = false, Message = "Error installing package, see logs for details" };
            }

            return new JsonResponse() { Success = true, Message = "Package successfully installed" };
        }

        /// <summary>
        /// Uninstall package
        /// </summary>
        /// <param name="pkgId"></param>
        /// <returns></returns>
        public static JsonResponse UninstallPackage(string pkgId)
        {
            try
            {
                var repo =
                PackageRepositoryFactory.Default.CreateRepository(
                    new PackageSource("http://dnbegallery.org/feed/FeedService.svc", "Default"));

                var packageManager = new NuGet.PackageManager(
                    repo,
                    new DefaultPackagePathResolver("http://dnbegallery.org/feed/FeedService.svc"),
                    new PhysicalFileSystem(HttpContext.Current.Server.MapPath(Utils.RelativeWebRoot + "App_Data/packages"))
                );

                var package = repo.FindPackage(pkgId);

                if(package == null)
                    return new JsonResponse { Success = false, Message = "Package " + pkgId + " not found" };

                packageManager.UninstallPackage(package, true);

                RemovePackageFiles(package.Id, package.Version.ToString());        
            }
            catch (Exception ex)
            {
                Utils.Log("PackageManager.UninstallPackage", ex);
                return new JsonResponse { Success = false, Message = "Error uninstalling package, see logs for details" };
            }

            return new JsonResponse { Success = true, Message = "Package successfully uninstalled" };
        }

        static void CopyPackageFiles(string pkgId, string version)
        {
            //TODO: implement also for extensions and widgets, add "lib" handling
            var src = HttpContext.Current.Server.MapPath(Blog.CurrentInstance.RelativeWebRoot +
                string.Format("App_Data/packages/{0}.{1}/content", pkgId, version));

            var tgt = HttpContext.Current.Server.MapPath(Blog.CurrentInstance.RelativeWebRoot);

            var source = new DirectoryInfo(src);
            var target = new DirectoryInfo(tgt);

            Utils.CopyDirectoryContents(source, target);
        }

        static void RemovePackageFiles(string pkgId, string version)
        {
            //TODO: implement also for extensions and widgets
            var pkg = HttpContext.Current.Server.MapPath(Blog.CurrentInstance.RelativeWebRoot +
                string.Format("themes/{0}", pkgId));

            if (Directory.Exists(pkg))
                ForceDeleteDirectory(pkg); // Directory.Delete(pkg, true);

            // remove package itself
            pkg = HttpContext.Current.Server.MapPath(Blog.CurrentInstance.RelativeWebRoot +
                string.Format("App_Data/packages/{0}.{1}", pkgId, version));

            if (Directory.Exists(pkg))
                ForceDeleteDirectory(pkg); // Directory.Delete(pkg, true);
        }

        static IEnumerable<PublishedPackage> GetPackageList(PackagingSource packagingSource = null)
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

        public static void ForceDeleteDirectory(string path)
        {
            DirectoryInfo root;
            Stack<DirectoryInfo> fols;
            DirectoryInfo fol;
            fols = new Stack<DirectoryInfo>();
            root = new DirectoryInfo(path);
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
