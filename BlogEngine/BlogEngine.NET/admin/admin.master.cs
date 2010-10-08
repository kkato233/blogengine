namespace admin
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using BlogEngine.Core;

    /// <summary>
    /// The admin_admin.
    /// </summary>
    public partial class admin_admin : MasterPage
    {
        #region Constants and Fields

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="msg">The message.</param>
        public void SetStatus(string status, string msg)
        {
            this.AdminStatus.Attributes.Clear();
            this.AdminStatus.Attributes.Add("class", status);
            this.AdminStatus.InnerHtml = string.Format("{0}<a href=\"javascript:HideStatus()\" style=\"width:20px;float:right\">X</a>", this.Server.HtmlEncode(msg));

            // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OpenStatus", 
            // "ShowStatus('" + status + "','" + msg + "');", true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the admin photo.
        /// </summary>
        /// <returns>
        /// The admin photo.
        /// </returns>
        protected string AdminPhoto()
        {
            var src = string.Format("{0}admin/images/no_avatar.png", Utils.AbsoluteWebRoot);
            var email = (string)null;
            var adminName = string.Empty;
            AuthorProfile ap = this.AdminProfile();

            if (ap != null)
            {
                adminName = ap.DisplayName;
                if (!string.IsNullOrEmpty(ap.PhotoUrl))
                {
                    src = ap.PhotoUrl;
                }
                else
                {
                    if (!string.IsNullOrEmpty(ap.EmailAddress) && BlogSettings.Instance.Avatar != "none")
                    {
                        email = ap.EmailAddress;
                        src = null;
                    }
                }
            }

            return Avatar.GetAvatarImageTag(28, email, null, src, adminName);
        }

        /// <summary>
        /// The admin profile.
        /// </summary>
        /// <returns>
        /// An Author Profile.
        /// </returns>
        protected AuthorProfile AdminProfile()
        {
            try
            {
                return AuthorProfile.GetProfile(Thread.CurrentPrincipal.Identity.Name);
            }
            catch (Exception e)
            {
                Utils.Log(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                this.Response.Redirect(Utils.RelativeWebRoot);
            }

            Utils.AddFolderJavaScripts(this.Page, "Scripts", false);
            Utils.AddJavaScriptInclude(this.Page, Utils.RelativeWebRoot + "admin/admin.js", false, false);
        }

        #endregion
    }
}