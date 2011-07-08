using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;

namespace BlogEngine.Core.FileSystem
{
    public class ZipCompressor
    {
        private byte[] buffer = new byte[4096];
        private ZipOutputStream zStream;

        public void CompressDirectory(string ArchiveOutputLocation, Directory ArchiveDirectory)
        {
            using (ZipOutputStream zStream = new ZipOutputStream(System.IO.File.Create(ArchiveOutputLocation)))
            {
                foreach (var file in ArchiveDirectory.Files)
                {
                    ZipEntry fileEntry = new ZipEntry(file.FullPath);
                    zStream.PutNextEntry(fileEntry);
                    zStream.Write(file.FileContents, 0, file.FileContents.Length);
                }
                foreach (var directory in ArchiveDirectory.Directories)
                    ZipFolder(ArchiveDirectory, directory, zStream);
                zStream.Finish();
                zStream.Close();
            }
        }


        public void ZipFolder(Directory RootDirectory, Directory CurrentDirectory, ZipOutputStream zStream)
        {
            foreach (var file in CurrentDirectory.Files)
            {
                ZipEntry fileEntry = new ZipEntry(file.FullPath);
                zStream.PutNextEntry(fileEntry);
                zStream.Write(file.FileContents, 0, file.FileContents.Length);
            }

            foreach (var subDirectory in CurrentDirectory.Directories)
            {
                ZipFolder(RootDirectory, subDirectory, zStream);
            }

            
        }
    }        
}
