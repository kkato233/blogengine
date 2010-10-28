using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Diagnostics;
using System.Security;

namespace BlogEngine.Core
{
    /// <summary>
    /// Class to provide a unified area of authentication/authorization checking.
    /// </summary>
    public static partial class Security
    {

        static Security()
        {
            AnonymousUserRights = (Rights.None);
        }

        #region "Properties"

        /// <summary>
        /// Gets or sets the rights of anonymous, non-authenticated users.
        /// 
        /// This probably would work better as role.
        /// </summary>
        public static Rights AnonymousUserRights
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current user for the current HttpContext.
        /// </summary>
        /// <remarks>
        /// This should always return HttpContext.Current.User. That value and Thread.CurrentPrincipal can't be
        /// guaranteed to always be the same value, as they can be set independently from one another. Looking
        /// through the .Net source, the System.Web.Security.Roles class also returns the HttpContext's User.
        /// </remarks>
        public static System.Security.Principal.IPrincipal CurrentUser
        {
            get
            {
                return HttpContext.Current.User;
            }
        }

        /// <summary>
        /// Gets whether the current user is logged in.
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return Security.CurrentUser.Identity.IsAuthenticated;
            }
        }

        /// <summary>
        /// Gets whether the currently logged in user is in the administrator role.
        /// </summary>
        public static bool IsAdministrator
        {
            get
            {
                return (Security.IsAuthenticated && Security.CurrentUser.IsInRole(BlogSettings.Instance.AdministratorRole));
            }
        }

        /// <summary>
        /// Returns an IEnumerable of Rights that belong to the ecurrent user.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Right> CurrentUserRights()
        {
            return Right.GetRights(Security.GetCurrentUserRoles());
        }

        #endregion

        #region "Public Methods"


        /// <summary>
        /// Throws a SecurityException if the current user is not authorized with the given Rights.
        /// </summary>
        /// <param name="right"></param>
        public static void DemandUserHasRight(Rights right)
        {
            if (!IsAuthorizedTo(right))
            {
                throw new SecurityException("User doesn't have the right to perform this");
            }
        }

        /// <summary>
        /// Returns whether or not the current user has the passed in Right.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool IsAuthorizedTo(Rights right)
        {
            return Right.HasRight(right, Security.GetCurrentUserRoles());
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Helper method that returns the correct roles based on authentication.
        /// </summary>
        /// <returns></returns>
        private static string[] GetCurrentUserRoles()
        {
            if (!IsAuthenticated)
            {
                // This needs to be recreated each time, because it's possible 
                // that the array can fall into the wrong hands and then someone
                // could alter it. 
                return new[] { BlogSettings.Instance.AnonymousRole };
            }
            else
            {
                return Roles.GetRolesForUser();
            }
        }

        #endregion
    }
}
