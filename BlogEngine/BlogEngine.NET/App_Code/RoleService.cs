namespace App_Code
{
    using System;
    using System.Web.Script.Services;
    using System.Web.Security;
    using System.Web.Services;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    /// <summary>
    /// Membership service to support AJAX calls
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class RoleService : WebService
    {
        #region Constants and Fields

        /// <summary>
        ///     JSON object that will be return back to client
        /// </summary>
        private readonly JsonResponse response;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        public RoleService()
        {
            this.response = new JsonResponse();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a role.
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <returns>
        /// JSON Response
        /// </returns>
        [WebMethod]
        public JsonResponse Add(string roleName)
        {
            if (!this.IsAdmin())
            {
                this.response.Success = false;
                this.response.Message = "Not authorized";
                return this.response;
            }

            if (string.IsNullOrEmpty(roleName))
            {
                this.response.Success = false;
                this.response.Message = "Role name is required field";
                return this.response;
            }

            var roles = Roles.GetAllRoles();
            if (roles.GetUpperBound(0) > 0)
            {
                for (var i = 0; i <= roles.GetUpperBound(0); i++)
                {
                    if (roles[i].ToLowerInvariant() != roleName.ToLowerInvariant())
                    {
                        continue;
                    }

                    this.response.Success = false;
                    this.response.Message = string.Format("Role \"{0}\" already exists", roleName);
                    return this.response;
                }
            }

            try
            {
                Roles.CreateRole(roleName);
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Roles.AddRole: {0}", ex.Message));
                this.response.Success = false;
                this.response.Message = string.Format("Could not create the role: {0}", roleName);
                return this.response;
            }

            this.response.Success = true;
            this.response.Message = string.Format("Role \"{0}\" has been created", roleName);

            // _response.Data = string.Format(row, roleName);
            return this.response;
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The role id.
        /// </param>
        /// <returns>
        /// JSON Response.
        /// </returns>
        [WebMethod]
        public JsonResponse Delete(string id)
        {
            if (!this.IsAdmin())
            {
                this.response.Success = false;
                this.response.Message = "Not authorized";
                return this.response;
            }

            if (string.IsNullOrEmpty(id))
            {
                this.response.Success = false;
                this.response.Message = "Role name is required field";
                return this.response;
            }

            try
            {
                Roles.DeleteRole(id);
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Roles.AddRole: {0}", ex.Message));
                this.response.Success = false;
                this.response.Message = string.Format("Could not delete the role: {0}", id);
                return this.response;
            }

            this.response.Success = true;
            this.response.Message = string.Format("Role \"{0}\" has been deleted", id);
            return this.response;
        }

        /// <summary>
        /// Edits a role.
        /// </summary>
        /// <param name="id">
        /// The row id.
        /// </param>
        /// <param name="bg">
        /// The background.
        /// </param>
        /// <param name="vals">
        /// The values.
        /// </param>
        /// <returns>
        /// JSON Response.
        /// </returns>
        [WebMethod]
        public JsonResponse Edit(string id, string bg, string[] vals)
        {
            var ptrn = "<tr id=\"{0}\" bgcolor=\"#{1}\"><td><input type=\"checkbox\" class\"chk\"/></td>";
            ptrn +=
                "<td class='editable'>{0}</td><td align=\"center\" style=\"vertical-align:middle\"><a href=\"#\" class=\"editButton\">edit</a></td>";
            ptrn +=
                "<td align=\"center\" style=\"vertical-align:middle\"><a href=\"#\" class=\"deleteButton\">delete</a></td></tr>";

            this.response.Success = false;
            this.response.Data = string.Format(ptrn, vals[0], bg);

            if (!this.IsAdmin())
            {
                this.response.Message = "Not authorized";
                return this.response;
            }

            if (string.IsNullOrEmpty(vals[0]))
            {
                this.response.Message = "Role name is required field";
                return this.response;
            }

            try
            {
                Roles.DeleteRole(id);
                Roles.CreateRole(vals[0]);
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Roles.UpdateRole: {0}", ex.Message));
                this.response.Message = string.Format("Could not update the role: {0}", vals[0]);
                return this.response;
            }

            this.response.Success = true;
            this.response.Message = string.Format("Role updated from \"{0}\" to \"{1}\"", id, vals[0]);
            return this.response;
        }

        #endregion

        #region Methods

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

        #endregion
    }
}