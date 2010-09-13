namespace Account
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    using BlogEngine.Core;

    /// <summary>
    /// The account_ account.
    /// </summary>
    public partial class Account : MasterPage
    {
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
            this.AddJavaScript(string.Format("{0}Account/Account.js", Utils.RelativeWebRoot));
        }

        /// <summary>
        /// Adds the java script.
        /// </summary>
        /// <param name="src">
        /// The SRC.
        /// </param>
        private void AddJavaScript(string src)
        {
            if (this.Page.Header.Controls.Cast<Control>()
                .Where(ctl => ctl.GetType() == typeof(HtmlGenericControl))
                    .Select(ctl => (HtmlGenericControl)ctl)
                        .Where(gc => gc.Attributes["src"] != null)
                        .Any(gc => gc.Attributes["src"].Contains(src)))
            {
                return;
            }

            using (var js = new HtmlGenericControl("script"))
            {
                js.Attributes["type"] = "text/javascript";
                js.Attributes["src"] = string.Format(
                    "{0}js.axd?path={1}", Utils.RelativeWebRoot, this.Server.UrlEncode(src));
                js.Attributes["defer"] = "defer";

                this.Page.Header.Controls.Add(js);
            }
        }

        #endregion
    }
}