using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BlogEngine.Core
{

    /// <summary>
    /// Default implementation of BaseSecurityProvider.
    /// </summary>
    public sealed class DefaultSecurityProvider : BaseSecurityProvider
    {

        #region "Properties"

        /// <summary>
        /// Gets the current user for the current HttpContext.
        /// </summary>
        /// <remarks>
        /// This should always return HttpContext.Current.User. That value and Thread.CurrentPrincipal can't be
        /// guaranteed to always be the same value, as they can be set independently from one another. 
        /// </remarks>
        public override System.Security.Principal.IPrincipal CurrentUser
        {
            get
            {
              
                return HttpContext.Current.User;
            }
            set
            {
                throw new NotImplementedException();
            }
        }



        /// <summary>
        /// Gets the current user's rights.
        /// </summary>
        /// <remarks>
        /// 
        /// Ideally, this should enumerate through the current user's roles
        /// and then collect all the rights each of those roles allows. This
        /// implementation doesn't do that.
        /// 
        /// </remarks>
        public override IEnumerable<Right> CurrentUserRights
        {
            get {

                // along with the Administrator role as a role that can't be deleted.

                if (!IsAuthenticated)
                {
                    var anonymous = new List<Right>();
                    anonymous.Add(Right.GetRightByFlag(RightFlags.None));
                    return anonymous;
                }
                else if (IsAdministrator)
                {
                    return this.GetAdministratorRights();
                }
                else
                {

                    // Here there should be a way of grabbing each role a user belongs to 
                    // and then combining the rights of each. 
                    //
                    // There's no way to do this at the moment, because there's no
                    // publicly accessible way to obtain internal Role class instances
                    // from the Xml or Db providers. Unlike the XmlRoleProvider, the DbRoleProvider
                    // doesn't keep a local reference to its roles which would 
                    // probably require some changing to the DbRoleProvider class.

                    var temp = new List<Right>();
                    temp.Add(Right.GetRightByFlag(RightFlags.CreateComments));
                    return temp;

                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets whether the currently authenticated user is in the administrator role.
        /// </summary>
        public override bool IsAdministrator
        {
            get { return (this.IsAuthenticated && this.CurrentUser.IsInRole(BlogSettings.Instance.AdministratorRole)); }
        }


        /// <summary>
        /// Gets whether the current user is authenticated.
        /// </summary>
        public override bool IsAuthenticated
        {
            get { return this.CurrentUser.Identity.IsAuthenticated; }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Gets whether the current user is authorized to perform actions that require a specific right.
        /// </summary>
        /// <param name="rightToCheck">The RightFlags value being checked against the current user's rights.</param>
        /// <returns></returns>
        public override bool IsAuthorizedTo(RightFlags rightToCheck)
        {
            // This should automatically return true if the user is in the Admin role.
            // It just doesn't at the moment for testing purposes.

            foreach (var right in this.CurrentUserRights)
            {
                if (right.Flag == rightToCheck)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion


    }

}
