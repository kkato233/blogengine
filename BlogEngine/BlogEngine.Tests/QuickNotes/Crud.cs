using NUnit.Framework;
using WatiN.Core;
using BlogEngine.Tests.PageTemplates.Admin;

namespace BlogEngine.Tests.QuickNotes
{
    [TestFixture]
    public class Crud : BeTest
    {
        [Test]
        public void ShouldBeAbleToCreateUpdateAndDeleteNote()
        {
            Login("admin");
            ie.GoTo(Constants.Root);
            ie.Link(Find.ById("open")).Click();

            // create
            TypeQuickly(ie.TextField("q-txtarea"), "Note one. This is a note one.");
            ie.Button("q-save").Click();
            ie.Link("q-alist").Click();
            Assert.IsTrue(ie.Html.Contains("Note one"));
            ie.SelectList("q-listbox").Options[0].Select();
            Assert.IsTrue(ie.Html.Contains("q-txtarea"), "Text area not found on the page after selecting new note");

            // updated
            TypeQuickly(ie.TextField("q-txtarea"), "Note one. This is a note one. Updated.");
            ie.Button("q-save").Click();
            ie.Link("q-alist").Click();
            Assert.IsTrue(ie.Html.Contains("This is a note one. Updated."));

            // delete
            ie.SelectList("q-listbox").Options[0].Select();
            ie.WaitForComplete();
            ie.Link("q-delete").Click();
            ie.Link("q-alist").Click();
            Assert.IsTrue(ie.Html.Contains("You do not have any notes yet"));
        }
    }
}
