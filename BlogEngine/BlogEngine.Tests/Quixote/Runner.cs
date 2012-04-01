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
    }
}
