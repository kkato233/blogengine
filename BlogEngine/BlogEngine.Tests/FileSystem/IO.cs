using System.IO;
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
        public void CanWriteAndReadAppCodeDirectory()
        {
            Assert.IsTrue(WriteReadDirectory(AppCodeDir));
            Assert.IsTrue(DeleteDirectory(AppCodeDir));

            // writing into code directory will
            // recycle application pool -
            // wait untill application back online
            ie.GoTo(Constants.Root);
            Wait(35);
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

            using (StreamWriter sw = File.CreateText(file))
            {
                sw.WriteLine("The test");
            }

            if (!File.Exists(file))
                return false;

            using (StreamReader sr = File.OpenText(file))
            {
                if (sr.ReadToEnd() != "The test\r\n")
                    return false;
            }

            using (StreamReader sr = File.OpenText(file))
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
