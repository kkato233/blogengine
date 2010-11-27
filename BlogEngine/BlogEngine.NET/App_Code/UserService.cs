namespace App_Code
{
    using System;
    using System.Linq;
    using System.Web.Script.Services;
    using System.Web.Security;
    using System.Web.Services;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    /// <summary>
    /// The user service.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class UserService : WebService
    {
        #region Constants and Fields

        /// <summary>
        /// The response.
        /// </summary>
        private readonly JsonResponse response;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService()
        {
            this.response = new JsonResponse();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="pwd">The password to add.</param>
        /// <param name="email">The email to add.</param>
        /// <param name="roles">Roles for new user</param>
        /// <returns>JSON Response.</returns>
        [WebMethod]
        public JsonResponse Add(string user, string pwd, string email, string[] roles)
        {
            if (!Security.IsAuthorizedTo(Rights.CreateNewUsers))
            {
                return new JsonResponse() { Message = "Not authorized." };
            }
            else if (Utils.StringIsNullOrWhitespace(user))
            {
                return new JsonResponse() { Message = "User argument is invalid." };
            }
            else if (Utils.StringIsNullOrWhitespace(pwd))
            {
                return new JsonResponse() { Message = "Password argument is invalid." };
            }
            else if (Utils.StringIsNullOrWhitespace(email) || !Utils.IsEmailValid(email))
            {
                return new JsonResponse() { Message = "Email argument is invalid." };
            }

            user = user.Trim();
            email = email.Trim();
            pwd = pwd.Trim();

            if (Membership.GetUser(user) != null)
            {
                return new JsonResponse() { Message = string.Format("User \"{0}\" already exists", user) };
            }

            try
            {
                Membership.CreateUser(user, pwd, email);

                if (Security.IsAuthorizedTo(Rights.EditOtherUsersRoles))
                {
                    // remove all user roles and add only checked
                    Roles.RemoveUserFromRoles(user, Roles.GetAllRoles());
                    if (roles.GetLength(0) > 0)
                    {
                        Roles.AddUsersToRoles(new string[] { user }, roles);
                    }
                }

                return new JsonResponse() { Success = true, Message = string.Format("User \"{0}\" has been created", user) };
            }
            catch (Exception ex)
            {
                Utils.Log("UserService.Add", ex);
                return new JsonResponse() { Message = string.Format("Could not create user: {0} : {1}", user, ex.Message) };
            }

        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The username.</param>
        /// <returns>JSON Response</returns>
        [WebMethod]
        public JsonResponse Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                this.response.Success = false;
                this.response.Message = "User name is required field";
                return this.response;
            }

            bool isSelf = id.Equals(Security.CurrentUser.Identity.Name, StringComparison.OrdinalIgnoreCase);

            if (isSelf && !Security.IsAuthorizedTo(Rights.DeleteUserSelf))
            {
                return new JsonResponse() { Message = "Not authorized" };
            }
            else if (!isSelf && !Security.IsAuthorizedTo(Rights.DeleteUsersOtherThanSelf))
            {
                return new JsonResponse() { Message = "Not authorized" };
            }

            try
            {
                Membership.DeleteUser(id);
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Users.Delete : {0}", ex.Message));
                this.response.Success = false;
                this.response.Message = string.Format("Could not delete user : {0}", id);
                return this.response;
            }

            this.response.Success = true;
            this.response.Message = string.Format("User \"{0}\" has been deleted", id);
            return this.response;
        }

        /// <summary>
        /// Edits the specified id.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="bg">The background.</param>
        /// <param name="vals">The values.</param>
        /// <returns>JSON Response</returns>
        [WebMethod]
        public JsonResponse Edit(string id, string bg, string[] vals)
        {
            try
            {
                this.response.Success = false;

                bool isSelf = id.Equals(Security.CurrentUser.Identity.Name, StringComparison.OrdinalIgnoreCase);

                if (string.IsNullOrEmpty(vals[0]))
                {
                    this.response.Message = "Email is required field";
                    return this.response;
                }

                if (
                    Membership.GetAllUsers().Cast<MembershipUser>().Any(
                        u => u.Email.ToLowerInvariant() == vals[0].ToLowerInvariant()))
                {
                    this.response.Message = "User with this email already exists";
                    return this.response;
                }

                if (isSelf && !Security.IsAuthorizedTo(Rights.EditOwnUser))
                {
                    this.response.Message = "Not authorized";
                    return this.response;
                }
                else if (!isSelf && !Security.IsAuthorizedTo(Rights.EditOtherUsers))
                {
                    this.response.Message = "Not authorized";
                    return this.response;
                }

                var usr = Membership.GetUser(id);
                if (usr != null)
                {
                    usr.Email = vals[0];
                    Membership.UpdateUser(usr);
                }

                this.response.Success = true;
                this.response.Message = string.Format("User \"{0}\" updated", id);
                return this.response;
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("UserService.Update: {0}", ex.Message));
                this.response.Message = string.Format("Could not update user: {0}", id);
                return this.response;
            }
        }

        #endregion
    }
}