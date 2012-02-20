using NUnit.Framework;
using WatiN.Core;
using BlogEngine.Tests.PageTemplates.Admin;

namespace BlogEngine.Tests.Posts
{
    [TestFixture]
    public class Post : BeTest
    {
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

            // this will type test post into TinyMCE editor
            ie.Eval(editPost.JsHack);

            editPost.Save.Click();

            Assert.IsTrue(ie.ContainsText(TheTestPost));

            Assert.IsTrue(ie.Page<PostList>().DeletePostByTitle(TheTestPost, ie), "Could not delete created post by title");

            Wait(5); // wait for ajax to complete. WaitForComplete not always working (((
            ie.WaitForComplete();

            ie.GoTo(Constants.Root);

            ie.WaitForComplete();

            Assert.IsFalse(ie.ContainsText(TheTestPost));
        }

    }
}