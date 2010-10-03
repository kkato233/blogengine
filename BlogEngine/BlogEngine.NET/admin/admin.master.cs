namespace Admin
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
    /// The Admin.
    /// </summary>
    public partial class Admin : MasterPage
    {
        #region Constants and Fields

        /// <summary>
        /// The gravatar image.
        /// </summary>
        private const string GravatarImage = "<img class=\"photo\" src=\"{0}\" alt=\"{1}\" />";

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
        /// The add jquery.
        /// </summary>
        protected virtual void AddJquery()
        {
            var s = Path.Combine(this.Server.MapPath("~/"), "Scripts");
            var fileEntries = Directory.GetFiles(s);
            foreach (var fileName in
                fileEntries.Where(fileName => (fileName.EndsWith(".js", StringComparison.OrdinalIgnoreCase) && fileName.Contains("jquery-")) && !fileName.EndsWith("-vsdoc.js", StringComparison.OrdinalIgnoreCase)))
            {
                this.AddJavaScript(string.Format("{0}Scripts/{1}", Utils.RelativeWebRoot, Utils.ExtractFileNameFromPath(fileName)));
            }
        }

        /// <summary>
        /// Gets the admin photo.
        /// </summary>
        /// <returns>
        /// The admin photo.
        /// </returns>
        protected string AdminPhoto()
        {
            var src = string.Format("{0}admin/images/no_avatar.png", Utils.AbsoluteWebRoot);
            var adminName = string.Empty;

            if (this.AdminProfile() != null)
            {
                adminName = this.AdminProfile().DisplayName;
                if (!string.IsNullOrEmpty(this.AdminProfile().PhotoUrl))
                {
                    src = this.AdminProfile().PhotoUrl;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.AdminProfile().EmailAddress) && BlogSettings.Instance.Avatar != "none")
                    {
                        src = this.Avatar(this.AdminProfile().EmailAddress);
                    }
                }
            }

            return string.Format(CultureInfo.InvariantCulture, GravatarImage, src, adminName);
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
        /// Gets the avatar for the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The avatar.</returns>
        protected string Avatar(string email)
        {
            var hash =
                FormsAuthentication.HashPasswordForStoringInConfigFile(email.ToLowerInvariant().Trim(), "MD5");
            if (hash != null)
            {
                hash = hash.ToLowerInvariant();
            }

            var src = string.Format("http://www.gravatar.com/avatar/{0}.jpg?s=28&amp;d=", hash);

            switch (BlogSettings.Instance.Avatar)
            {
                case "identicon":
                    src += "identicon";
                    break;
                case "wavatar":
                    src += "wavatar";
                    break;
                default:
                    src += "monsterid";
                    break;
            }

            return src;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                this.Response.Redirect(Utils.RelativeWebRoot);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.AddJquery();
            this.AddJavaScript(string.Format("{0}admin/admin.js", Utils.RelativeWebRoot));

            base.OnInit(e);
        }

        /// <summary>
        /// The add java script.
        /// </summary>
        /// <param name="src">
        /// The source.
        /// </param>
        private void AddJavaScript(string src)
        {
            if ((from Control ctl in this.Page.Header.Controls
                 where ctl.GetType() == typeof(HtmlGenericControl)
                 select (HtmlGenericControl)ctl
                     into gc
                     where gc.Attributes["src"] != null
                     select gc).Any(gc => gc.Attributes["src"].Contains(src)))
            {
                return;
            }

            var js = new HtmlGenericControl("script");

            js.Attributes["type"] = "text/javascript";
            js.Attributes["src"] = string.Format("{0}js.axd?path={1}", Utils.RelativeWebRoot, this.Server.UrlEncode(src));
            js.Attributes["defer"] = "defer";

            this.Page.Header.Controls.Add(js);
        }

        #endregion
    }
}