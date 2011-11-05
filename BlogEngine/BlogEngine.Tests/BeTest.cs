using System;
using NUnit.Framework;
using WatiN.Core;

namespace BlogEngine.Tests
{
    public abstract class BeTest
    {
        protected IE ie = null;

        [TestFixtureSetUp]
        public void SetUp()
        {
            ie = new IE();
            Settings.WaitForCompleteTimeOut = 240;

            // to hide IE window
            // ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
        }

        [STAThread]
        static void Main(string[] args)
        {

        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ie.Close();
        }

        public void Login(string user, string pwd = "")
        {
            ie.GoTo(Constants.AccountLogin);

            if (string.IsNullOrEmpty(pwd)) 
                pwd = user;

            TypeQuickly(ie.TextField(Find.ById(Constants.UserName)), user);
            TypeQuickly(ie.TextField(Find.ById(Constants.Password)), pwd);

            ie.Button(Find.ById(Constants.LoginButton)).Click();
        }

        public void Logout()
        {
            ie.GoTo(Constants.Root);

            var logOffLink = ie.Link(Find.ById(Constants.LoginLinkId));

            if (logOffLink != null && logOffLink.Text == Constants.LogOff)
            {
                logOffLink.Click();
            }
        }

        public static void TypeQuickly(TextField textField, string text)
        {
            textField.SetAttributeValue("value", text);
        }

        public void Wait(int seconds)
        {
            int i;
            int.TryParse(string.Format("{0}000", seconds), out i);
            System.Threading.Thread.Sleep(i);
        }
    }
}
