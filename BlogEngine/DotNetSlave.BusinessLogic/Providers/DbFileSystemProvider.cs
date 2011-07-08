using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace BlogEngine.Core.Providers
{
    /// <summary>
    /// DbBlogProvider Parial class for manageing all FileSystem methods
    /// </summary>
    public partial class DbBlogProvider : BlogProvider
    {
        #region Contansts & Fields
        /// <summary>
        /// active web.config connection string, defined by the blogProvider sections
        /// </summary>
        private string connectionString { get { return WebConfigurationManager.ConnectionStrings[this.connStringName].ConnectionString; } }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a directory at a specific path
        /// </summary>
        /// <param name="VirtualPath">The virtual path to be created</param>
        /// <returns>the new Directory object created</returns>
        /// <remarks>
        /// Virtual path is the path starting from the /files/ containers
        /// The entity is created against the current blog id
        /// </remarks>
        internal override FileSystem.Directory CreateDirectory(string VirtualPath)
        {
            VirtualPath = VirtualPath.VirtualPathToDbPath();
            if (DirectoryExists(VirtualPath))
                return GetDirectory(VirtualPath);
            var directoryName = "root";
            if (!string.IsNullOrWhiteSpace(VirtualPath))
                directoryName = string.Format("/{0}", VirtualPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last());
            FileSystem.FileStoreDb db = new FileSystem.FileStoreDb(this.connectionString);
            FileSystem.FileStoreDirectory dir = new FileSystem.FileStoreDirectory
            {
                BlogID = Blog.CurrentInstance.Id,
                CreateDate = DateTime.Now,
                FullPath = VirtualPath,
                Id = Guid.NewGuid(),
                LastAccess = DateTime.Now,
                Name = directoryName,
                LastModify = DateTime.Now
            };
            if (!string.IsNullOrWhiteSpace(VirtualPath))
            {
                var parentPath = VirtualPath.Contains("/") ? VirtualPath.Substring(0, VirtualPath.LastIndexOf("/")) : string.Empty;
                dir.ParentID = BlogService.GetDirectory(parentPath).Id;
            }
            db.FileStoreDirectories.InsertOnSubmit(dir);
            db.SubmitChanges();
            db.Dispose();
            return GetDirectory(VirtualPath);
        }

        /// <summary>
        /// Deletes a spefic directory from a virtual path
        /// </summary>
        /// <param name="VirtualPath">The path to delete</param>
        /// <remarks>
        /// Virtual path is the path starting from the /files/ containers
        /// The entity is queried against to current blog id
        /// </remarks>
        public override void DeleteDirectory(string VirtualPath)
        {
            if (!DirectoryExists(VirtualPath.VirtualPathToDbPath()))
                return;
            if (string.IsNullOrWhiteSpace(VirtualPath))
                throw new ArgumentException("Unable to delete root directory.");
            FileSystem.FileStoreDb db = new FileSystem.FileStoreDb(this.connectionString);
            var query = db.FileStoreDirectories.Where(x => x.FullPath.ToLower() == VirtualPath.VirtualPathToDbPath().ToLower() && x.BlogID == Blog.CurrentInstance.Id);
            db.FileStoreDirectories.DeleteAllOnSubmit(query);
            db.SubmitChanges();
            db.Dispose();
        }

        /// <summary>
        /// Returns wether or not the specific directory by virtual path exists
        /// </summary>
        /// <param name="VirtualPath">The virtual path to query</param>
        /// <returns>boolean</returns>
        public override bool DirectoryExists(string VirtualPath)
        {
            VirtualPath = VirtualPath.VirtualPathToDbPath();
            return new FileSystem.FileStoreDb(this.connectionString).FileStoreDirectories.Where(x => x.FullPath.ToLower() == VirtualPath.ToLower() && x.BlogID == Blog.CurrentInstance.Id).Count() > 0;
        }

        /// <summary>
        /// gets a directory by the virtual path
        /// </summary>
        /// <param name="VirtualPath">the virtual path</param>
        /// <returns>the directory object or null for no directory found</returns>
        public override FileSystem.Directory GetDirectory(string VirtualPath)
        {
            VirtualPath = VirtualPath.VirtualPathToDbPath();
            FileSystem.FileStoreDb db = new FileSystem.FileStoreDb(this.connectionString);
            if (string.IsNullOrWhiteSpace(VirtualPath))
            {
                var directory = db.FileStoreDirectories.FirstOrDefault(x => x.BlogID == Blog.CurrentInstance.Id && x.ParentID == null);
                if (directory == null)
                {
                    db.Dispose();
                    return BlogService.CreateDirectory(VirtualPath);
                }
                var obj = directory.CopyToDirectory();
                db.Dispose();
                return obj;
            }
            else
            {
                var dir = db.FileStoreDirectories.FirstOrDefault(x => x.FullPath.ToLower() == VirtualPath.ToLower() && x.BlogID == Blog.CurrentInstance.Id);
                if (dir == null)
                {
                    db.Dispose();
                    return null;
                }
                var obj = dir.CopyToDirectory();
                db.Dispose();
                return obj;
            }
        }

        /// <summary>
        /// gets a directory by a basedirectory and a string array of sub path tree
        /// </summary>
        /// <param name="BaseDirectory">the base directory object</param>
        /// <param name="SubPath">the params of sub path</param>
        /// <returns>the directory found, or null for no directory found</returns>
        public override FileSystem.Directory GetDirectory(FileSystem.Directory BaseDirectory, params string[] SubPath)
        {
            return GetDirectory(string.Concat(BaseDirectory.FullPath, string.Join("/", SubPath)));
        }

        /// <summary>
        /// gets all the directories underneath a base directory. Only searches one level.
        /// </summary>
        /// <param name="BaseDirectory">the base directory</param>
        /// <returns>collection of Directory objects</returns>
        public override IEnumerable<FileSystem.Directory> GetDirectories(FileSystem.Directory BaseDirectory)
        {
            FileSystem.FileStoreDb db = new FileSystem.FileStoreDb(this.connectionString);
            var dirs = db.FileStoreDirectories.Where(x => x.ParentID == BaseDirectory.Id && x.BlogID == Blog.CurrentInstance.Id);
            List<FileSystem.Directory> directories = new List<FileSystem.Directory>();
            foreach (var d in dirs)
                directories.Add(d.CopyToDirectory());
            db.Dispose();
            return directories;
        }

        /// <summary>
        /// gets all the files in a directory, only searches one level
        /// </summary>
        /// <param name="BaseDirectory">the base directory</param>
        /// <returns>collection of File objects</returns>
        public override IEnumerable<FileSystem.File> GetFiles(FileSystem.Directory BaseDirectory)
        {
            var db = new FileSystem.FileStoreDb(this.connectionString);
            var arr = db.FileStoreDirectories.FirstOrDefault(x=>x.Id == BaseDirectory.Id).FileStoreFiles.Select(x=>x.CopyToFile()).ToList();
            db.Dispose();
            return arr;
        }

        /// <summary>
        /// gets a specific file by virtual path
        /// </summary>
        /// <param name="VirtualPath">the virtual path of the file</param>
        /// <returns></returns>
        public override FileSystem.File GetFile(string VirtualPath)
        {
            var db = new FileSystem.FileStoreDb(this.connectionString);
            var file = db.FileStoreFiles.FirstOrDefault(x => x.FullPath == VirtualPath.VirtualPathToDbPath() && x.FileStoreDirectory.BlogID == Blog.CurrentInstance.Id).CopyToFile();
            db.Dispose();
            return file;
        }

        /// <summary>
        /// boolean wether a file exists by its virtual path
        /// </summary>
        /// <param name="VirtualPath">the virtual path</param>
        /// <returns>boolean</returns>
        public override bool FileExists(string VirtualPath)
        {
            var db = new FileSystem.FileStoreDb(this.connectionString);
            var file = db.FileStoreFiles.FirstOrDefault(x => x.FullPath.ToLower() == VirtualPath.VirtualPathToDbPath() && x.FileStoreDirectory.BlogID == Blog.CurrentInstance.Id) == null;
            db.Dispose();
            return file;
        }

        /// <summary>
        /// deletes a file by virtual path
        /// </summary>
        /// <param name="VirtualPath">virtual path</param>
        public override void DeleteFile(string VirtualPath)
        {
            var db = new FileSystem.FileStoreDb(this.connectionString);
            var file = db.FileStoreFiles.Where(x => x.FullPath == VirtualPath.VirtualPathToDbPath() && x.FileStoreDirectory.BlogID == Blog.CurrentInstance.Id);
            db.FileStoreFiles.DeleteAllOnSubmit(file);
            db.SubmitChanges();
            db.Dispose();
        }

        /// <summary>
        /// uploads a file to the provider container
        /// </summary>
        /// <param name="FileBinary">file contents as byte array</param>
        /// <param name="FileName">the file name</param>
        /// <param name="BaseDirectory">directory object that is the owner</param>
        /// <returns>the new file object</returns>
        public override FileSystem.File UploadFile(byte[] FileBinary, string FileName, FileSystem.Directory BaseDirectory)
        {
            return UploadFile(FileBinary, FileName, BaseDirectory, false);
        }

        /// <summary>
        /// uploads a file to the provider container
        /// </summary>
        /// <param name="FileBinary">the contents of the file as a byte array</param>
        /// <param name="FileName">the file name</param>
        /// <param name="BaseDirectory">the directory object that is the owner</param>
        /// <param name="Overwrite">boolean wether to overwrite the file if it exists.</param>
        /// <returns>the new file object</returns>
        public override FileSystem.File UploadFile(byte[] FileBinary, string FileName, FileSystem.Directory BaseDirectory, bool Overwrite)
        {
            var virtualPath = BaseDirectory.FullPath + "/" + FileName;
            var db = new FileSystem.FileStoreDb(this.connectionString);
            var files = db.FileStoreFiles.Where(x => x.FileStoreDirectory.Id == BaseDirectory.Id && x.FullPath.ToLower() == virtualPath);
            if (files.Count() > 0)
            {
                if (!Overwrite)
                {
                    db.Dispose();
                    throw new Exception("File " + FileName + " already exists in this path.");
                }
                db.FileStoreFiles.DeleteAllOnSubmit(files);
                db.SubmitChanges();
            }
            var file = new FileSystem.FileStoreFile()
            {
                Contents = FileBinary,
                CreateDate = DateTime.Now,
                FileID = Guid.NewGuid(),
                FullPath = virtualPath,
                LastAccess = DateTime.Now,
                LastModify = DateTime.Now,
                Name = FileName,
                ParentDirectoryID = BaseDirectory.Id,
                Size = FileBinary.Length
            };
            db.FileStoreFiles.InsertOnSubmit(file);
            db.SubmitChanges();
            db.Dispose();
            return file.CopyToFile();
        }

        /// <summary>
        /// gets the file contents via Lazy load, however in the DbProvider the Contents are loaded when the initial object is created to cut down on DbReads
        /// </summary>
        /// <param name="BaseFile">the baseFile object to fill</param>
        /// <returns>the original file object</returns>
        internal override FileSystem.File GetFileContents(FileSystem.File BaseFile)
        {
            var db = new FileSystem.FileStoreDb(this.connectionString);
            var file = db.FileStoreFiles.FirstOrDefault(x => x.FileID == Guid.Parse(BaseFile.Id));
            if (file == null)
                throw new ArgumentException("File not found in dataset");
            BaseFile.FileContents = file.Contents.ToArray();
            db.Dispose();
            return BaseFile;
        }
        #endregion
    }

    #region Extension Methods
    /// <summary>
    /// static classes for the DbFileSystem
    /// </summary>
    public static class DbFileSystemExtensions
    {
        /// <summary>
        /// copy's the database directory object to a Directory object
        /// </summary>
        /// <param name="inObj">the database directory to copy</param>
        /// <returns>a new Directory object</returns>
        public static FileSystem.Directory CopyToDirectory(this FileSystem.FileStoreDirectory inObj)
        {
            if (inObj == null)
                return null;
            return new FileSystem.Directory()
            {
                DateCreated = inObj.CreateDate,
                DateModified = inObj.LastModify,
                FullPath = inObj.FullPath,
                Id = inObj.Id,
                LastAccessTime = inObj.LastAccess,
                Name = inObj.Name,
                IsRoot = inObj.ParentID == null,
            };
        }

        /// <summary>
        /// copys a database File object to a File object
        /// </summary>
        /// <param name="inObj">the database file object to copy</param>
        /// <returns>a new File object</returns>
        public static FileSystem.File CopyToFile(this FileSystem.FileStoreFile inObj)
        {
            if (inObj == null)
                return null;
            return new FileSystem.File()
            {
                DateCreated = inObj.CreateDate,
                DateModified = inObj.LastModify,
                FilePath = inObj.FullPath,
                FileSize = inObj.Size,
                Id = inObj.FileID.ToString(),
                LastAccessTime = inObj.LastAccess,
                Name = inObj.Name,
                FileContents = inObj.Contents.ToArray(),
                FullPath = inObj.FullPath
            };

        }

        /// <summary>
        /// removes the virtual path of BlogStorageLocation + files from the virtual path. 
        /// </summary>
        /// <param name="VirtualPath">the virtual path to replace against</param>
        /// <returns>the repleaced string</returns>
        public static string VirtualPathToDbPath(this string VirtualPath)
        {
            return VirtualPath.Replace(Blog.CurrentInstance.StorageLocation + "files", "");
        }

    }
    #endregion

}
