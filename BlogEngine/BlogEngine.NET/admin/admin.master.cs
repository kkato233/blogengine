﻿// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The AdminMasterPage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Admin
{
    using System;
    using System.Threading;
    using System.Web.UI;

    using BlogEngine.Core;

    /// <summary>
    /// The AdminMasterPage.
    /// </summary>
    public partial class AdminMasterPage : MasterPage
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
            var ap = this.AdminProfile();

            if (ap != null)
            {
                adminName = ap.DisplayName;
                if (string.IsNullOrEmpty(ap.PhotoUrl))
                {
                    if (!string.IsNullOrEmpty(ap.EmailAddress) && BlogSettings.Instance.Avatar != "none")
                    {
                        email = ap.EmailAddress;
                        src = null;
                    }
                }
                else
                {
                    src = ap.PhotoUrl;
                }
            }

            return Avatar.GetAvatarImageTag(28, email, null, src, adminName);
        }

        /// <summary>
        /// Gets the admin profile.
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
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                this.Response.Redirect(Utils.RelativeWebRoot);
            }
            System.Diagnostics.Debug.Print(this.Request.IsAuthenticated.ToString());

            Utils.AddFolderJavaScripts(this.Page, "Scripts", false);
            Utils.AddJavaScriptInclude(this.Page, string.Format("{0}admin/admin.js", Utils.RelativeWebRoot), false, false);

            base.OnInit(e);
        }

        #endregion
    }
}