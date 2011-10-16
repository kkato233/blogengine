using NUnit.Framework;
using WatiN.Core;

namespace BlogEngine.Tests.Packaging
{
    [TestFixture]
    public class Installer : BeTest
    {
        [SetUp]
        public void Init()
        {
            Login("admin");
        }

        [Test]
        public void InstallTheme()
        {
            ie.GoTo(Constants.GalleryThemes);

            TypeQuickly(ie.TextField(Find.ById("searchGallery")), "Boldy");

            ie.Button(Find.ById("btnGalSearch")).Click();

            ie.Link(Find.ById("a-Boldy")).Click();

            Wait(10);

            ie.GoTo(Constants.LocalThemes);

            ie.WaitUntilContainsText("Boldy");

            Assert.IsTrue(ie.ContainsText("Boldy"));
        }

        [Test]
        public void UninstallTheme()
        {
            ie.GoTo(Constants.LocalThemes);

            ie.Link(Find.ById("a-Boldy")).Click();

            Wait(10);

            Assert.IsFalse(ie.ContainsText("Boldy"));
        }

    }
}