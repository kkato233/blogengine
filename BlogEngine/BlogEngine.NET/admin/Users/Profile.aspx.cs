namespace admin.Users
{
    using System;
    using System.Linq;
    using System.Web.Security;
    using System.Web.Services;

    using BlogEngine.Core;

    /// <summary>
    /// The admin_ pages_ profile.
    /// </summary>
    public partial class admin_Pages_Profile : System.Web.UI.Page
    {
        #region Constants and Fields

        /// <summary>
        /// The id string.
        /// </summary>
        private static string theId = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets RolesList.
        /// </summary>
        protected static string RolesList
        {
            get
            {
                var ret = string.Empty;
                const string Ptrn = "<input type=\"checkbox\" id=\"{0}\" class=\"chkRole\" {1} /><span class=\"lbl\">{0}</span>";
                var allRoles = Roles.GetAllRoles();
                return allRoles.Aggregate(ret, (current, r) => current + (Roles.IsUserInRole(theId, r) ? string.Format(Ptrn, r, "checked") : string.Format(Ptrn, r, string.Empty)));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The get profile.
        /// </summary>
        /// <param name="id">
        /// The profile id.
        /// </param>
        /// <returns>
        /// An AuthorProfile.
        /// </returns>
        [WebMethod]
        public static AuthorProfile GetProfile(string id)
        {
            var pf = AuthorProfile.GetProfile(id) ?? new AuthorProfile
                {
                    DisplayName = string.Empty,
                    FirstName = string.Empty,
                    MiddleName = string.Empty,
                    LastName = string.Empty,
                    Birthday = new DateTime(1001, 1, 1),
                    PhotoUrl = string.Empty,
                    EmailAddress = string.Empty,
                    PhoneMobile = string.Empty,
                    PhoneMain = string.Empty,
                    PhoneFax = string.Empty,
                    CityTown = string.Empty,
                    RegionState = string.Empty,
                    Country = string.Empty,
                    AboutMe = string.Empty
                };

            return pf;
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
            if (!string.IsNullOrEmpty(this.Request.QueryString["id"]))
            {
                theId = this.Request.QueryString["id"];
            }
        }

        #endregion
    }
}