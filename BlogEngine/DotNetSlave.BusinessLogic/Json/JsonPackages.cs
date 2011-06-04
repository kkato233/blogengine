using System.Collections.Generic;
using System.Linq;
using BlogEngine.Core.Packaging;

namespace BlogEngine.Core.Json
{
    /// <summary>
    /// Json Packages
    /// </summary>
    public class JsonPackages
    {
        /// <summary>
        /// Package count
        /// </summary>
        public static int Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkgType"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        public static List<JsonPackage> GetPage(string pkgType, int page, PackageManager.OrderType sortOrder, string searchVal)
        {
            var retPkgs = new List<JsonPackage>();

            var packages = PackageManager.GetPackages(pkgType, page, sortOrder, searchVal);

            if(packages == null)
            {
                return null;
            }

            Count = packages.Count();

            foreach (var p in packages)
            {
                var jp = new JsonPackage
                {
                    Id = p.Id,
                    PackageType = pkgType,
                    Authors = string.IsNullOrEmpty(p.Authors) ? "unknown" : p.Authors,
                    Description = p.Description,
                    DownloadCount = p.DownloadCount,
                    LastUpdated = p.LastUpdated.ToString("dd MMM yyyy"),
                    Title = p.Title,
                    Version = p.Version,
                    Website = p.ProjectUrl,
                    Tags = p.Tags,
                    IconUrl = string.IsNullOrWhiteSpace(p.IconUrl)
                        ? Utils.ApplicationRelativeWebRoot + "Pics/imagePlaceholder.png"
                        : "http://dnbegallery.org" + p.IconUrl
                };

                if (!string.IsNullOrWhiteSpace(p.GalleryDetailsUrl))
                    jp.PackageUrl = "http://dnbegallery.org/cms/List/Themes/" + p.Id;

                retPkgs.Add(jp);
            }
            return retPkgs;
        }
    }
}
