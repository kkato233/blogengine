using NUnit.Framework;

namespace BlogEngine.Tests.Navigation
{
    [TestFixture]
    public class UrlRewrite : BeTest
    {
        [Test]
        public void CanNavigateToCategoryPage()
        {
            ie.GoTo(Constants.Root + "/category/" + Constants.FirstPostCategory + ".aspx");
            ie.WaitForComplete();
            Assert.IsTrue(ie.ContainsText(Constants.FirstPostTitle));
        }
       
        [Test]
        public void CanNavigateTagPage()
        {
            ie.GoTo(Constants.Root + "/?tag=/welcome");
            ie.WaitForComplete();
            Assert.IsTrue(ie.ContainsText(Constants.FirstPostTitle));
        }

        [Test]
        public void CanNavigatePage()
        {
            ie.GoTo(Constants.Root + "/page/Example-page.aspx");
            ie.WaitForComplete();
            Assert.IsTrue(ie.ContainsText("Example page"));
        }

        [Test]
        public void CanNavigateToCalendar()
        {
            ie.GoTo(Constants.Root + "/calendar/default.aspx");
            ie.WaitForComplete();
            Assert.IsTrue(ie.Html.Contains("widgetContainer"));
        }

        [Test]
        public void CanNavigateToPostsByAuthor()
        {
            ie.GoTo(Constants.Root + "/author/Admin.aspx");
            ie.WaitForComplete();
            Assert.IsTrue(ie.Title.Contains("All posts by admin"));
        }

        [Test]
        public void CanNavigateToPostsByMonth()
        {
            ie.GoTo(Constants.Root + "/2011/06/default.aspx");
            ie.WaitForComplete();
            Assert.IsTrue(ie.Html.Contains(Constants.FirstPostTitle));
        }
    }
}
