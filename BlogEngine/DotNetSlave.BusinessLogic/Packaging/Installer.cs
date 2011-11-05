using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using BlogEngine.Core.Json;
using BlogEngine.Core.Providers;
using NuGet;

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
                    new PackageSource(BlogSettings.Instance.GalleryFeedUrl, "Default"));
            }
        }

        /// <summary>
        /// Install package
        /// </summary>
        /// <param name="pkgId"></param>
        public static JsonResponse InstallPackage(string pkgId)
        {
            try
            {
                UninstallPackage(pkgId);

                var packageManager = new PackageManager(
                    _repository,
                    new DefaultPackagePathResolver(BlogSettings.Instance.GalleryFeedUrl),
                    new PhysicalFileSystem(HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot + "App_Data/packages"))
                );

                var package = _repository.FindPackage(pkgId);

                packageManager.InstallPackage(package, false);

                var iPkg = new InstalledPackage { PackageId = package.Id, Version = package.Version.ToString() };
                BlogService.InsertPackage(iPkg);

                var packageFiles = FileSystem.CopyPackageFiles(package.Id, package.Version.ToString());
                BlogService.InsertPackageFiles(packageFiles);

                Blog.CurrentInstance.Cache.Remove(Constants.CacheKey);
            }
            catch (Exception ex)
            {
                Utils.Log("PackageManager.InstallPackage", ex);
                return new JsonResponse { Success = false, Message = "Error installing package, see logs for details" };
            }

            return new JsonResponse { Success = true, Message = "Package successfully installed" };
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
                var packageManager = new PackageManager(
                    _repository,
                    new DefaultPackagePathResolver(BlogSettings.Instance.GalleryFeedUrl),
                    new PhysicalFileSystem(HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot + "App_Data/packages"))
                );

                var package = _repository.FindPackage(pkgId);

                if (package == null)
                    return new JsonResponse { Success = false, Message = "Package " + pkgId + " not found" };

                packageManager.UninstallPackage(package, true);

                FileSystem.RemovePackageFiles(package.Id);

                BlogService.DeletePackage(pkgId);

                // reset cache
                Blog.CurrentInstance.Cache.Remove(Constants.CacheKey);
            }
            catch (Exception ex)
            {
                Utils.Log("PackageManager.UninstallPackage", ex);
                return new JsonResponse { Success = false, Message = "Error uninstalling package, see logs for details" };
            }

            return new JsonResponse { Success = true, Message = "Package successfully uninstalled" };
        }

        /// <summary>
        /// Load installed packages
        /// </summary>
        /// <param name="packages"></param>
        public static void Load(List<JsonPackage> packages)
        {
            var installed = BlogService.InstalledFromGalleryPackages();

            foreach (var pkg in packages)
            {
                if (pkg == null) continue;
                var p = pkg;
                if(installed != null && installed.Count > 0)
                {
                    foreach (var inst in installed.Where(inst => p.Id == inst.PackageId))
                    {
                        pkg.Location = "I";
                        pkg.LocalVersion = inst.Version;
                    }
                }
            }
        }
    }
}
