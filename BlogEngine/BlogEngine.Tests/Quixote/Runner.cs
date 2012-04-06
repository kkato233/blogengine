using NUnit.Framework;
using WatiN.Core;

namespace BlogEngine.Tests.Quixote
{
    [TestFixture]
    public class Runner : BeTest
    {
        [SetUp]
        public void Init()
        {
            Login("admin");
        }

        [Test]
        public void RunPagerTests()
        {
            ie.GoTo(Constants.Root + "/tests/pager.cshtml");
            Assert.IsTrue(ie.Html.Contains("class=\"pass\""));
            Assert.IsFalse(ie.Html.Contains("class=\"fail\""));
        }

        [Test]
        public void RunAvatarTests()
        {
            ie.GoTo(Constants.Root + "/tests/avatar.cshtml");
            Assert.IsTrue(ie.Html.Contains("class=\"pass\""));
            Assert.IsFalse(ie.Html.Contains("class=\"fail\""));
        }

        [Test]
        [Category("online")]
        public void RunPackagingTests()
        {
            ie.GoTo(Constants.Root + "/tests/packaging.cshtml");
            Assert.IsTrue(ie.Html.Contains("class=\"pass\""));
            Assert.IsFalse(ie.Html.Contains("class=\"fail\""));
        }
    }
}
