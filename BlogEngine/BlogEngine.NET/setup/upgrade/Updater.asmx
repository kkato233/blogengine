<%@ WebService Language="C#" Class="Updater" %>
using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Hosting;
using System.Linq;
using System.Web;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Specialized;
using System.Net;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class Updater  : WebService {

    private StringCollection _ignoreDirs;
    private string _root;
    private string _latestZip;
    private string _backupZip;
    private string _downloadUrl = "http://dnbegallery.org/beupgrade/file.axd?file=/2931.zip";
    private string _versionsTxt = "http://dnbegallery.org/beupgrade/file.axd?file=/versions.txt";
    private bool _test = true;
    
    public Updater()
    {
        _root = System.Web.Hosting.HostingEnvironment.MapPath("~/");
        if (_root.EndsWith("\\")) _root = _root.Substring(0, _root.Length - 1);

        _latestZip = _root + "\\setup\\upgrade\\backup\\latest.zip";
        _backupZip = _root + "\\setup\\upgrade\\backup\\backup.zip";
        
        _ignoreDirs = new StringCollection();
        _ignoreDirs.Add(_root + "\\Custom");
        _ignoreDirs.Add(_root + "\\setup\\upgrade");
    }
    
    [WebMethod]
    public string Check(string version)
    {
        try
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(_versionsTxt);
            StreamReader reader = new StreamReader(stream);
            string line = "";
            while (reader.Peek() >= 0)
            {
                line = reader.ReadLine();
                if (!string.IsNullOrEmpty(version) && line.StartsWith(version) && line.Contains("|"))
                {
                    return line.Substring(line.LastIndexOf("|") + 1);
                }
            }
            return "";
        }
        catch (Exception)
        {
            return "";
        }
    }
    
    [WebMethod]
    public string Download(string version)
    {
        if (_test)
        {
            System.Threading.Thread.Sleep(2000);
            return "";
        }
        try
        {
            if (!Directory.Exists(_root + "\\setup\\upgrade\\backup"))
                Directory.CreateDirectory(_root + "\\setup\\upgrade\\backup");

            if (File.Exists(_latestZip))
                File.Delete(_latestZip);
            
            DateTime startTime = DateTime.UtcNow;
            WebRequest request = System.Net.WebRequest.Create(_downloadUrl);
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                using (Stream fileStream = File.OpenWrite(_latestZip))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = responseStream.Read(buffer, 0, 4096);
                    while (bytesRead > 0)
                    {
                        fileStream.Write(buffer, 0, bytesRead);
                        DateTime nowTime = DateTime.UtcNow;
                        if ((nowTime - startTime).TotalMinutes > 5)
                        {
                            throw new ApplicationException(
                                "Download timed out");
                        }
                        bytesRead = responseStream.Read(buffer, 0, 4096);
                    }
                }
            }
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod]
    public string Extract()
    {
        if (_test)
        {
            System.Threading.Thread.Sleep(2000);
            return "";
        }
        
        ZipFile zf = null;
        string outFolder = _root + "\\setup\\upgrade\\backup\\be";
        try
        {
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);
                     
            FileStream fs = File.OpenRead(_latestZip);
            zf = new ZipFile(fs);

            foreach (ZipEntry zipEntry in zf)
            {
                if (!zipEntry.IsFile)
                {
                    continue;
                }
                String entryFileName = zipEntry.Name;

                byte[] buffer = new byte[4096];
                Stream zipStream = zf.GetInputStream(zipEntry);

                String fullZipToPath = Path.Combine(outFolder, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
                }
            }
            return "";
        }
        catch(Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (zf != null)
            {
                zf.IsStreamOwner = true;
                zf.Close();
            }
        }
    }

    [WebMethod]
    public string Backup()
    {
        if (_test)
        {
            System.Threading.Thread.Sleep(2000);
            return "";
        }
        
        try
        {
            var backupDir = HostingEnvironment.MapPath("~/setup/upgrade/backup");
            var blogDir = HostingEnvironment.MapPath("~/");

            if (!System.IO.Directory.Exists(backupDir))
                System.IO.Directory.CreateDirectory(backupDir);

            var fsOut = File.Create(_backupZip);
            var zipStream = new ZipOutputStream(fsOut);

            zipStream.SetLevel(3);
            int folderOffset = blogDir.Length + (blogDir.EndsWith("\\") ? 0 : 1);

            CompressFolder(blogDir, zipStream, folderOffset);

            zipStream.IsStreamOwner = true;
            zipStream.Close();
            
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod]
    public string Delete()
    {
        if (_test)
        {
            System.Threading.Thread.Sleep(2000);
            return "";
        }
        
        try
        {
            DeleteDir("\\Account");
            DeleteDir("\\admin");
            DeleteDir("\\api");          
            DeleteDir("\\editors");
            DeleteDir("\\fonts");
            DeleteDir("\\Modules");
            DeleteDir("\\pics");
            DeleteDir("\\setup\\Mono");
            DeleteDir("\\setup\\MySQL");
            DeleteDir("\\setup\\SQL_CE");
            DeleteDir("\\setup\\SQLite");
            DeleteDir("\\setup\\SQLServer");
            DeleteDir("\\setup\\VistaDB");
            
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod]
    public string Install()
    {
        if (_test)
        {
            System.Threading.Thread.Sleep(2000);
            return "";
        }
        
        try
        {
            CopyDir("Account");
            CopyDir("admin");
            CopyDir("api");
            CopyDir("editors");
            CopyDir("fonts");
            CopyDir("Modules");
            CopyDir("pics");
            
            CopyDir("setup\\Mono");
            CopyDir("setup\\MySQL");
            CopyDir("setup\\SQL_CE");
            CopyDir("setup\\SQLite");
            CopyDir("setup\\SQLServer");
            CopyDir("setup\\VistaDB");

            ReplaceDir("App_GlobalResources");
            ReplaceDir("Scripts");
            ReplaceDir("Content");
            ReplaceDir("App_Code");

            ReplaceFile("archive.aspx");
            ReplaceFile("contact.aspx");
            ReplaceFile("default.aspx");
            ReplaceFile("error.aspx");
            ReplaceFile("error404.aspx");
            ReplaceFile("page.aspx");
            ReplaceFile("post.aspx");
            ReplaceFile("search.aspx");
            ReplaceFile("web.sitemap");
            ReplaceFile("wlwmanifest.xml");

            ReplaceFilesInDir("bin");
            
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod]
    public string Rollback()
    {
        System.Threading.Thread.Sleep(2000);
        return "";
    }

    //----------------------------------------------

    void CopyDir(string dir)
    {
        var source = new DirectoryInfo(_root + "\\setup\\upgrade\\backup\\be\\" + dir);
        var target = new DirectoryInfo(_root + "\\" + dir);

        CopyDirectory(source, target);
    }

    void ReplaceDir(string dir)
    {
        Directory.Delete(_root + "\\" + dir, true);
        CopyDir(dir);
    }

    void DeleteDir(string dir)
    {
        Directory.Delete(_root + dir, true);
    }

    void DeleteFile(string file)
    {
        File.Delete(_root + file);
    }

    void ReplaceFile(string file)
    {
        string sourceFile = _root + "\\setup\\upgrade\\backup\\be\\" + file;
        string targetFile = _root + "\\" + file;

        BlogEngine.Core.Utils.Log(string.Format("Replace: {0} from: {1}", sourceFile, targetFile));

        if (File.Exists(targetFile))
            File.Delete(targetFile);

        File.Copy(sourceFile, targetFile);
    }

    void ReplaceFilesInDir(string dir)
    {
        string sourceDir = _root + "\\setup\\upgrade\\backup\\be\\" + dir;
        string[] files = Directory.GetFiles(sourceDir);

        foreach (string sourceFile in files)
        {
            string fileName = sourceFile.Substring(sourceFile.LastIndexOf(@"\") + 1);
            ReplaceFile(dir + "\\" + fileName);
        }
    }

    void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
    {
        if (IgnoredDirectory(path))
            return;
        
        string[] files = Directory.GetFiles(path);

        foreach (string filename in files)
        {
            FileInfo fi = new FileInfo(filename);
            string entryName = filename.Substring(folderOffset);
            entryName = ZipEntry.CleanName(entryName);
            ZipEntry newEntry = new ZipEntry(entryName);
            newEntry.DateTime = fi.LastWriteTime;
            newEntry.Size = fi.Length;
            zipStream.PutNextEntry(newEntry);

            byte[] buffer = new byte[4096];
            using (FileStream streamReader = File.OpenRead(filename))
            {
                StreamUtils.Copy(streamReader, zipStream, buffer);
            }
            zipStream.CloseEntry();
        }
        string[] folders = Directory.GetDirectories(path);
        foreach (string folder in folders)
        {
            CompressFolder(folder, zipStream, folderOffset);
        }
    }

    void ExtractZipFile(string archiveFilenameIn, string outFolder)
    {
        ZipFile zf = null;
        try
        {
            FileStream fs = File.OpenRead(archiveFilenameIn);
            zf = new ZipFile(fs);

            foreach (ZipEntry zipEntry in zf)
            {
                if (!zipEntry.IsFile)
                    continue;

                String entryFileName = zipEntry.Name;

                byte[] buffer = new byte[4096];
                Stream zipStream = zf.GetInputStream(zipEntry);

                String fullZipToPath = Path.Combine(outFolder, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
                }
            }
        }
        finally
        {
            if (zf != null)
            {
                zf.IsStreamOwner = true;
                zf.Close();
            }
        }
    }

    static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
    {
        var rootPath = HttpContext.Current.Server.MapPath(BlogEngine.Core.Utils.RelativeWebRoot);

        if (!Directory.Exists(target.FullName))
            Directory.CreateDirectory(target.FullName);

        foreach (var dir in source.GetDirectories())
        {
            var dirPath = Path.Combine(target.FullName, dir.Name);

            // Files moved to Custom folder
            //dirPath = dirPath.Replace("App_Code", "Custom");
            //dirPath = dirPath.Replace("\\themes", "\\Custom\\Themes");

            var relPath = dirPath.Replace(rootPath, "");

            CopyDirectory(dir, Directory.CreateDirectory(dirPath));
        }

        foreach (var file in source.GetFiles())
        {
            var filePath = Path.Combine(target.FullName, file.Name);

            var relPath = filePath.Replace(rootPath, "");

            file.CopyTo(filePath);
        }
    }

    bool IgnoredDirectory(string item)
    {
        return _ignoreDirs.Contains(item) ? true : false;
    }
}