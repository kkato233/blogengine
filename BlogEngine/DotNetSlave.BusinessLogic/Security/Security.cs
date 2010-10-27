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
    public static class Security
    {

        private static BaseSecurityProvider provider;

        static Security()
        {
            // This should be retrieved from the web.config if possible.
            provider = new DefaultSecurityProvider();

            AnonymousUserRights = (RightFlags.None);

        }

        #region "Properties"

        /// <summary>
        /// Gets or sets the rights of anonymous, non-authenticated users.
        /// 
        /// This probably would work better as role.
        /// </summary>
        public static RightFlags AnonymousUserRights
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current user for the current HttpContext.
        /// </summary>
        public static System.Security.Principal.IPrincipal CurrentUser
        {
            get
            {
                return provider.CurrentUser;
            }
        }

        /// <summary>
        /// Gets whether the current user is logged in.
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return provider.IsAuthenticated;
            }
        }

        /// <summary>
        /// Gets whether the currently logged in user is in the administrator role.
        /// </summary>
        public static bool IsAdministrator
        {
            get
            {
                return provider.IsAdministrator;
            }
        }

        /// <summary>
        /// Returns an IEnumerable of Rights that belong to the ecurrent user.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Right> CurrentUserRights()
        {
            return provider.CurrentUserRights;
        }

        #endregion

        #region "Public Methods"


        /// <summary>
        /// Throws a SecurityException if the current user is not authorized with the given Rights.
        /// </summary>
        /// <param name="right"></param>
        public static void DemandUserHasRight(RightFlags right)
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
        public static bool IsAuthorizedTo(RightFlags right)
        {
            return provider.IsAuthorizedTo(right);
        }

        #endregion
    }
}
