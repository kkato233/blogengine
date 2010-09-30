namespace admin.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Services;

    using BlogEngine.Core;

    using Page = System.Web.UI.Page;

    /// <summary>
    /// The admin account roles.
    /// </summary>
    public partial class Roles : Page
    {
        #region Constants and Fields

        /// <summary>
        /// The response.
        /// </summary>
        private JsonResponse response;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <returns>The roles.</returns>
        [WebMethod]
        public static List<Role> GetRoles()
        {
            var roles = new List<Role>();
            roles.AddRange(System.Web.Security.Roles.GetAllRoles().Select(r => new Role { RoleName = r }));
            roles.Sort((r1, r2) => string.Compare(r1.RoleName, r2.RoleName));

            return roles;
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
            this.response = new JsonResponse();
        }

/*
        /// <summary>
        /// Determines whether this instance is admin.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is admin; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAdmin()
        {
            return this.User.IsInRole(BlogSettings.Instance.AdministratorRole);
        }
*/

        #endregion
    }

    /// <summary>
    /// The user role.
    /// </summary>
    public class Role
    {
        #region Constants and Fields

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }

        #endregion
    }
}