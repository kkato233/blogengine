using NUnit.Framework;
using WatiN.Core;
using BlogEngine.Tests.PageTemplates.Admin;

namespace BlogEngine.Tests.Posts
{
    [TestFixture]
    public class Post : BeTest
    {
        string PostId = "";
        string TheTestPost = "The test post";

        [SetUp]
        public void Init()
        {
            Login("admin");
        }

        [TearDown]
        public void Dispose()
        {
            var trash = ie.Page<Trash>();
            ie.GoTo(trash.Url);
            trash.PurgeAll.Click();
        }

        [Test]
        public void CanCreateAndDeletePost()
        {
            var editPost = ie.Page<EditPost>();

            ie.GoTo(editPost.Url);

            TypeQuickly(editPost.PostTitle, TheTestPost);
            
            ie.Eval(editPost.JsHack);

            editPost.Save.Click();

            SetPostId();

            Assert.IsTrue(ie.ContainsText(TheTestPost));

            ie.GoTo(ie.Page<PostList>().Url);
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
