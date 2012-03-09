using System.IO;
using System.Security.AccessControl;
using NUnit.Framework;

namespace BlogEngine.Tests.FileSystem
{
    [TestFixture]
    public class IO : BeTest
    {
        readonly string AppDataDir = Path.Combine(RootPath(), "App_Data");
        readonly string AppCodeDir = Path.Combine(RootPath(), "App_Code");

        [Test]
        public void CanWriteAndReadAppDataDirectory()
        {
            Assert.IsTrue(WriteReadDirectory(AppDataDir));
            Assert.IsTrue(DeleteDirectory(AppDataDir));
        }

        [Test]
        [Category("Reload")]
        public void CanWriteAndReadAppCodeDirectory()
        {
            Assert.IsTrue(WriteReadDirectory(AppCodeDir));
            Assert.IsTrue(DeleteDirectory(AppCodeDir));

            // writing into code directory will
            // recycle application pool -
            // wait untill application back online
            ie.GoTo(Constants.Root);
            ie.WaitForComplete();
            //Wait(35);
        }

        [TearDown]
        public void Dispose()
        {

        }

        static bool WriteReadDirectory(string dir)
        {
            var testDir = Path.Combine(dir, "foo");
            var file = Path.Combine(testDir, "bar.txt");

            Directory.CreateDirectory(testDir);

            using (var sw = File.CreateText(file))
            {
                sw.WriteLine("The test");
            }

            if (!File.Exists(file))
                return false;

            using (var sr = File.OpenText(file))
            {
                if (sr.ReadToEnd() != "The test\r\n")
                    return false;
            }

            using (var sr = File.OpenText(file))
            {
                return (sr.ReadToEnd() == "The test\r\n");
            }
        }

        static bool DeleteDirectory(string dir)
        {
            File.Delete(Path.Combine(dir, "foo/bar.txt"));
            Directory.Delete(Path.Combine(dir, "foo"));

            return !Directory.Exists(Path.Combine(dir, "foo"));
        }

        public static void MkDir(string dir)
        {
            string path = Path.Combine(RootPath(), dir);

            if (!Directory.Exists((path)))
                Directory.CreateDirectory(path);
        }

        public static void DelDir(string dir)
        {
            string path = Path.Combine(RootPath(), dir);

            if (Directory.Exists((path)))
                Directory.Delete(path);
        }

        public static void MkFile(string file, string text)
        {
            var path = Path.Combine(RootPath(), file);

            if(!File.Exists(path))
            {
                using (var sw = File.CreateText(path))
                {
                    sw.Write(text);
                }
            }
        }

        public static void DelFile(string file)
        {
            var path = Path.Combine(RootPath(), file);

            if (File.Exists((path)))
                File.Delete(path);
        }

        static string RootPath()
        {
            string s = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            s = s.Substring(0, s.IndexOf("BlogEngine.Tests"));
            s = s.Replace(@"file:///", "");
            s = s.Replace("/", @"\");
            s += @"BlogEngine.NET\";

            return s;
        }
    }
}
