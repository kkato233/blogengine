using System;
using NUnit.Framework;
using WatiN.Core;
using BlogEngine.Tests.PageTemplates.Account;

namespace BlogEngine.Tests
{
    public abstract class BeTest
    {
        protected IE ie = null;

        [TestFixtureSetUp]
        public void SetUp()
        {
            ie = new IE();

            ie.Refresh();
            ie.ClearCache();

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
            if (string.IsNullOrEmpty(pwd)) 
                pwd = user;

            var login = ie.Page<Login>();

            ie.GoTo(login.Url);

            TypeQuickly(login.UserName, user);
            TypeQuickly(login.Password, pwd);

            login.LoginButton.Click();
        }

        public void Logout()
        {
            ie.GoTo(Constants.Root);

            var login = ie.Page<Login>();
            var logOffLink = login.LogoffLink;

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
