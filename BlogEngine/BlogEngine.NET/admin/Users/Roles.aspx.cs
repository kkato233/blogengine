﻿namespace Admin.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Services;
    using BlogEngine.Core;
    using BlogEngine.Core.Json;

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
        public static List<JsonRole> GetRoles()
        {
            var roles = new List<JsonRole>();
            roles.AddRange(System.Web.Security.Roles.GetAllRoles().Select(r => new JsonRole { RoleName = r, IsSystemRole = Security.IsSystemRole(r) }));
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