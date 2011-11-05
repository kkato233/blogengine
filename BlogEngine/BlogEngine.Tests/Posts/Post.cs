using NUnit.Framework;
using WatiN.Core;

namespace BlogEngine.Tests.Posts
{
    [TestFixture]
    public class Post : BeTest
    {
        string PostId = "";
        string TheTestPost = "The test post";
        string TxtTitle = "ctl00_cphAdmin_txtTitle";

        [SetUp]
        public void Init()
        {
            Login("admin");
        }

        [TearDown]
        public void Dispose()
        {
            ie.GoTo(Constants.UrlAdminTrash);
            ie.Button(Find.ById("btnPurgeAll")).Click();
        }

        [Test]
        public void CanCreateAndDeletePost()
        {
            ie.GoTo(Constants.UrlAdminAddNewPost);
            TypeQuickly(ie.TextField(Find.ById(TxtTitle)), TheTestPost);

            // tinyMCE uses frames to simulate text area, need javascript hack as workaround
            string js = "document.getElementById('ctl00_cphAdmin_txtContent_TinyMCE1_txtContent_ifr').contentWindow.document.body.innerHTML = 'This is WATIN test post.';";
            ie.Eval(js);

            ie.Button(Find.ById("btnSave")).Click();

            SetPostId();

            Assert.IsTrue(ie.ContainsText(TheTestPost));

            ie.GoTo(Constants.UrlAdminPosts);
            ie.Link(Find.ById("a-" + PostId)).Click();

            // give 5 seconds for ajax method to execute
            // and romove element from the page
            Wait(5);

            Assert.IsFalse(ie.ContainsText(TheTestPost));
        }

        void SetPostId()
        {
            var pos = ie.Html.IndexOf("deletepost=");
            PostId = ie.Html.Substring(pos + 11, 36);
            System.Console.WriteLine("Post ID is: " + PostId);
        }
    }
}
