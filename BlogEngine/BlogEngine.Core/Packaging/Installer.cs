using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using BlogEngine.Core.Providers;
using NuGet;
using BlogEngine.Core.Data.Models;
using BlogEngine.Core.Data.Services;

namespace BlogEngine.Core.Packaging
{
    /// <summary>
    /// Responsible for install/uninstall operations
    /// </summary>
    public static class Installer
    {
        private static IPackageRepository _repository
        {
            get
            {
                return PackageRepositoryFactory.Default.CreateRepository(
                    BlogSettings.Instance.GalleryFeedUrl);
            }
        }

        /// <summary>
        /// Install package
        /// </summary>
        /// <param name="pkgId"></param>
        public static bool InstallPackage(string pkgId)
        {
            try
            {
                if(BlogService.InstalledFromGalleryPackages() != null && 
                    BlogService.InstalledFromGalleryPackages().Find(p => p.PackageId == pkgId) != null)
                    UninstallPackage(pkgId);

                var packageManager = new PackageManager(
                    _repository,
                    new DefaultPackagePathResolver(BlogSettings.Instance.GalleryFeedUrl),
                    new PhysicalFileSystem(HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot + "App_Data/packages"))
                );

                var package = _repository.FindPackage(pkgId);

                packageManager.InstallPackage(package, false, true);

                var iPkg = new InstalledPackage { PackageId = package.Id, Version = package.Version.ToString() };
                BlogService.InsertPackage(iPkg);

                var packageFiles = FileSystem.InstallPackage(package);

                BlogService.InsertPackageFiles(packageFiles);

                Blog.CurrentInstance.Cache.Remove(Constants.CacheKey);

                CustomFieldsParser.ClearCache();

                Utils.Log(string.Format("Installed package {0} by {1}", pkgId, Security.CurrentUser.Identity.Name));
            }
            catch (Exception ex)
            {
                Utils.Log("BlogEngine.Core.Packaging.Installer.InstallPackage(" + pkgId + ")", ex);
                UninstallPackage(pkgId);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Uninstall package
        /// </summary>
        /// <param name="pkgId"></param>
        /// <returns></returns>
        public static bool UninstallPackage(string pkgId)
        {
            try
            {
                var packageManager = new PackageManager(
                    _repository,
                    new DefaultPackagePathResolver(BlogSettings.Instance.GalleryFeedUrl),
                    new PhysicalFileSystem(HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot + "App_Data/packages"))
                );

                var package = _repository.FindPackage(pkgId);

                if (package == null)
                {
                    throw new ApplicationException("Package not found");
                }

                packageManager.UninstallPackage(package, true);

                FileSystem.UninstallPackage(package.Id);

                BlogService.DeletePackage(pkgId);

                // reset cache
                Blog.CurrentInstance.Cache.Remove(Constants.CacheKey);

                Utils.Log(string.Format("Uninstalled package {0} by {1}", pkgId, Security.CurrentUser.Identity.Name));
            }
            catch (Exception ex)
            {
                Utils.Log("BlogEngine.Core.Packaging.Installer.UninstallPackage(" + pkgId + ")", ex);
                return false;
            }

            return true;
        }

    }
}
