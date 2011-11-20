using NUnit.Framework;

namespace BlogEngine.Tests.Account
{
    [TestFixture]
    public class Login : BeTest
    {
        readonly string LoginFailedMsg = "Login failed";
        readonly string WelcomeToBeMsg = "Welcome to BlogEngine.NET";

        [SetUp]
        public void Init()
        {
            Logout();
        }

        [Test]
        public void InvalidLoginShouldFail()
        {
            Login("foo", "bar");

            Assert.IsTrue(ie.ContainsText(LoginFailedMsg));
        }

        [Test]
        public void ValidLoginShouldPass()
        {
            Login("admin");

            Assert.IsTrue(ie.ContainsText(WelcomeToBeMsg));
        }
    }
}