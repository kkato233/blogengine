using System.Collections.Generic;
using System.IO;
using System.Web;

namespace BlogEngine.Core.Packaging
{
    /// <summary>
    /// Class for packaging IO
    /// </summary>
    public class FileSystem
    {
        /// <summary>
        /// Copy package files
        /// </summary>
        /// <param name="pkgId">Package Id</param>
        /// <param name="version">Package Version</param>
        public static void CopyPackageFiles(string pkgId, string version)
        {
            //TODO: implement also for extensions and widgets, add "lib" handling
            var src = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot +
                string.Format("App_Data/packages/{0}.{1}/content", pkgId, version));

            var tgt = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot);

            var source = new DirectoryInfo(src);
            var target = new DirectoryInfo(tgt);

            Utils.CopyDirectoryContents(source, target);
        }

        /// <summary>
        /// Remove package files
        /// </summary>
        /// <param name="pkgId">Package Id</param>
        /// <param name="version">Package Version</param>
        public static void RemovePackageFiles(string pkgId, string version)
        {
            //TODO: implement also for extensions and widgets
            var pkg = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot +
                string.Format("themes/{0}", pkgId));

            if (Directory.Exists(pkg))
                ForceDeleteDirectory(pkg); // Directory.Delete(pkg, true);

            // remove package itself
            pkg = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot +
                string.Format("App_Data/packages/{0}.{1}", pkgId, version));

            if (Directory.Exists(pkg))
                ForceDeleteDirectory(pkg); // Directory.Delete(pkg, true);
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
    }
}
