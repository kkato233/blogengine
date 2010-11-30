namespace Account
{
    using System;
    using System.Web.UI;

    using BlogEngine.Core;
    using System.Web.UI.HtmlControls;
    using System.Text;
    using System.Collections;

    /// <summary>
    /// The account_ account.
    /// </summary>
    public partial class AccountMasterPage : MasterPage
    {
        /// <summary>
        /// Fetches the localized strings from resource files and generates
        /// the javascript helper string for utilization in .js files.
        /// </summary>
        /// <param name="resourceKeys">Array of resource keys</param>
        public void AddLocalizedStringsToJavaScript(ArrayList resourceKeys)
        {
            var script = new HtmlGenericControl("script");
            script.Attributes["type"] = "text/javascript";

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("//<![CDATA[");
            builder.AppendLine("var accountResources={};");

            foreach (string resourceKey in resourceKeys)
            {
                builder.AppendLine(string.Format("{0}=\"{1}\"", resourceKey.ToUpper(), GetGlobalResourceObject("labels", resourceKey)));
            }

            builder.AppendLine("//]]>");

            script.InnerText = builder.ToString();

            //add at top in header to prevent any javascript issue.
            // ToDo: Insert this script exactly before Account.js
            this.Page.Header.Controls.AddAt(0, script);
        }

        #region Public Methods

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <param name="msg">
        /// The message.
        /// </param>
        public void SetStatus(string status, string msg)
        {
            this.AdminStatus.Attributes.Clear();
            this.AdminStatus.Attributes.Add("class", status);
            this.AdminStatus.InnerHtml =
                string.Format(
                    "{0}<a href=\"javascript:HideStatus()\" style=\"width:20px;float:right\">X</a>", 
                    this.Server.HtmlEncode(msg));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // fix for issue 12004
            //create array list for the resource string
            ArrayList resources = new ArrayList() { 
                "passwordIsRequried",
                "emailIsRequired",
                "emailIsInvalid",
                "userNameIsRequired",
                "newAndConfirmPasswordMismatch",
                "confirmPasswordIsRequired",
                "oldPasswordIsRequired",
                "newPasswordIsRequired"
            };

            Utils.AddFolderJavaScripts(this.Page, "Scripts", true);

            // fix: issue 12004
            this.AddLocalizedStringsToJavaScript(resources);

            Utils.AddJavaScriptInclude(this.Page, string.Format("{0}Account/Account.js", Utils.RelativeWebRoot), false, false);
        }

        #endregion
    }
}