using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Diagnostics;
using System.Security;
using System.Threading;

namespace BlogEngine.Core
{

    /// <summary>
    /// Base class for creating security providers used by BlogEngine.
    /// </summary>
    /// <remarks>
    /// 
    /// The purpose of this class is to create an obvious distinction between authentication and authorization of users.
    /// 
    /// The class provides a uniform way of getting the current user, their rights, checking for authentication, and 
    /// checking for authorization based on rights/rules. 
    /// 
    /// The static class <seealso cref="BlogEngine.Core.Security"/> wraps a BaseSecurityProvider derived instance to 
    /// provide information to external users. This allows other classes to use the various methods/properties without
    /// having to worry about implementation details, and while allowing the BaseSecurityProvider to be switched out.
    /// 
    /// </remarks>
    public abstract class BaseSecurityProvider
    {

        #region "Constructors"

        static BaseSecurityProvider()
        {

            var adminRights = new List<Right>();

            foreach (var r in Right.GetAllRights())
            {
                if (r.Flag != RightFlags.None)
                {
                    adminRights.Add(r);
                }
            }

            administratorRights = adminRights;
        }

        #endregion

        #region "Constants and Fields"


        private static readonly IEnumerable<Right> administratorRights;

        #endregion


        #region "Abstract Properties"

        /// <summary>
        /// Gets or sets the current user for the current context request.
        /// </summary>
        /// <remarks>
        /// 
        /// When overriding this method, users should make note that the setter should change all areas.
        /// 
        /// </remarks>
        public abstract System.Security.Principal.IPrincipal CurrentUser { get; set; }

        /// <summary>
        /// Gets whether the currently authenticated user is in the administrator role.
        /// </summary>
        /// <remarks>
        /// This is merely a shortcut method to replace all the checks for whether the current user is in the admin role.
        /// </remarks>
        public abstract bool IsAdministrator { get; }

        /// <summary>
        /// Gets whether the current user is authenticated. 
        /// </summary>
        /// <remarks>
        /// This will return true for any type of user, including those that are only allowed to post comments or who otherwise
        /// have limited usage of the site.
        /// 
        /// This method should not be used for checking if a user is authorized to do anything. Use the IsAuthorizedTo method for that.
        /// </remarks>
        public abstract bool IsAuthenticated { get; }

        /// <summary>
        /// Returns the current user's rights.
        /// </summary>
        public abstract IEnumerable<Right> CurrentUserRights { get; }

        #endregion

        #region "Abstract Methods"

        /// <summary>
        /// Gets whether the current user is authorized to perform actions that require a specific right.
        /// </summary>
        /// <param name="right">The RightFlags value being checked against the current user's rights.</param>
        /// <returns></returns>
        public abstract bool IsAuthorizedTo(RightFlags right);

        /// <summary>
        /// Returns the RightFlags that represents an administrator. 
        /// </summary>
        /// <returns>
        /// 
        /// Ideally, this should return a RightFlags value that contains
        /// all of the RightFlag enum values except for RightFlags.None.
        /// 
        /// </returns>
        protected virtual IEnumerable<Right> GetAdministratorRights()
        {
            return administratorRights;
        }

        #endregion

    }

}
