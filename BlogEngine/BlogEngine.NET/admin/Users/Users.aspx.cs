namespace Admin.Users
{
    using System;
    using BlogEngine.Core;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;
    using System.Web.Services;

    /// <summary>
    /// The Users.
    /// </summary>
    public partial class Users : System.Web.UI.Page
    {
        #region Public Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Security.DemandUserHasRight(BlogEngine.Core.Rights.AccessAdminPages, true);
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns>The users.</returns>
        [WebMethod]
        public static List<MembershipUser> GetUsers()
        {
            int count;
            var userCollection = Membership.Provider.GetAllUsers(0, 999, out count);
            var users = userCollection.Cast<MembershipUser>().ToList();

            users.Sort((u1, u2) => string.Compare(u1.UserName, u2.UserName));

            return users;
        }

        #endregion
    }
}