using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGet;

namespace BlogEngine.Core.Packaging
{
    /// <summary>
    /// Package manager
    /// </summary>
    public class PackageManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkgType"></param>
        /// <returns></returns>
        public static IQueryable<IPackage> GetPackages(string pkgType)
        {
            var repo = PackageRepositoryFactory.Default.CreateRepository(
                new PackageSource("http://localhost/feed/FeedService.svc", "Default"));

            try
            {
                return repo.GetPackages();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
