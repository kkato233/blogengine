namespace Admin.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Services;

    using Page = System.Web.UI.Page;

    /// <summary>
    /// The admin account roles.
    /// </summary>
    public partial class Roles : Page
    {
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
}