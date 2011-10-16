using NUnit.Framework;
using WatiN.Core;

namespace BlogEngine.Tests.Account
{
    [TestFixture]
    public class Login : BeTest
    {
        [SetUp]
        public void Init()
        {
            Logout();
        }

        [Test]
        public void InvalidLoginShouldFail()
        {
            ie.GoTo(Constants.AccountLogin);

            Login("foo", "bar");

            Assert.IsTrue(ie.ContainsText(Constants.LoginFailedMsg));
        }

        [Test]
        public void ValidLoginShouldPass()
        {
            ie.GoTo(Constants.AccountLogin);

            Login("admin");

            Assert.IsTrue(ie.ContainsText(Constants.WelcomeToBeMsg));
        }
    }
}