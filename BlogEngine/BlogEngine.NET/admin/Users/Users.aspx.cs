namespace admin.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;
    using System.Web.Services;

    using BlogEngine.Core;

    /// <summary>
    /// The admin_newuser.
    /// </summary>
    public partial class admin_newuser : System.Web.UI.Page
    {
        #region Constants and Fields

        /// <summary>
        /// The response.
        /// </summary>
        private JsonResponse response;

        #endregion

        #region Public Methods

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

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.response = new JsonResponse();
        }

        #endregion
    }
}