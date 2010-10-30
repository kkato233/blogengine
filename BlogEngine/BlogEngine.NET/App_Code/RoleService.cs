namespace App_Code
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    public sealed class RoleService : WebService
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        public RoleService()
        {
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
            if (!Security.IsAuthorizedTo(Rights.CreateNewRoles))
            {
                return GetNotAuthorized();
            }
            else if (Utils.StringIsNullOrWhitespace(roleName))
            {
                return new JsonResponse() { Message = "Role name is a required field." };
            }
            else if (Roles.RoleExists(roleName))
            {
                return new JsonResponse() { Message = string.Format("Role \"{0}\" already exists", roleName) };
            }
            else
            {
                var response = new JsonResponse();

                try
                {
                    Roles.CreateRole(roleName);
                    response.Success = true;
                    response.Message = string.Format("Role \"{0}\" has been created", roleName);

                }
                catch (Exception ex)
                {
                    Utils.Log(string.Format("Roles.AddRole: {0}", ex.Message));
                    response.Success = false;
                    response.Message = string.Format("Could not create the role: {0}", roleName);
                }

                return response;
            }
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
            if (!Security.IsAuthorizedTo(Rights.DeleteRoles))
            {
                return GetNotAuthorized();
            }
            else if (Utils.StringIsNullOrWhitespace(id))
            {
                return new JsonResponse() { Message = "Role name is required field" };
            }

            try
            {
                Right.OnRoleDeleting(id);
                Roles.DeleteRole(id);
                return new JsonResponse() { Success = true, Message = string.Format("Role \"{0}\" has been deleted", id) };
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Roles.AddRole: {0}", ex.Message));
                return new JsonResponse() { Message = string.Format("Could not delete the role: {0}", id) };
            }
        }

        /// <summary>
        /// Saves the rights for a specific Role. 
        /// </summary>
        /// <param name="roleName">The name of the role whose rights are being updated.</param>
        /// <param name="rightsCollection">A dictionary of rights that a role is allowed to have.</param>
        /// <returns></returns>
        [WebMethod]
        public JsonResponse SaveRights(string roleName, Dictionary<string, bool> rightsCollection)
        {
            if (!Security.IsAuthorizedTo(Rights.EditRoles))
            {
                return new JsonResponse() { Message = "Not authorized" };
            }
            else if (Utils.StringIsNullOrWhitespace(roleName) || !Roles.RoleExists(roleName))
            {
                return new JsonResponse() { Message = "Invalid role name" };
            }
            else if (rightsCollection == null)
            {
                return new JsonResponse() { Message = "rightsCollection argument can not be null." };
            }
            else
            {

                // The rights collection can be empty, just not null. An empty array would indicate that a role is
                // being updated to include no rights at all.
                rightsCollection = new Dictionary<string, bool>(rightsCollection, StringComparer.OrdinalIgnoreCase);

                // Validate the dictionary before doing any altering to Rights.
                foreach (var right in rightsCollection)
                {
                    if (!Right.RightExists(right.Key))
                    {
                        return new JsonResponse() { Success = true, Message = String.Format("No such Right exists: {0}", right.Key) };
                    }
                    else if (right.Value == false)
                    {
                        return new JsonResponse() { Success = true, Message = "Do not pass back rights that have false values." };
                    }
                }

                foreach (var right in Right.GetAllRights())
                {
                    if (rightsCollection.ContainsKey(right.Name))
                    {
                        right.AddRole(roleName);
                    }
                    else
                    {
                        right.RemoveRole(roleName);
                    }
                }

                BlogEngine.Core.Providers.BlogService.SaveRights();
                return new JsonResponse() { Success = true, Message = "Rights updated for role" };
            }

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
            if (!Security.IsAuthorizedTo(Rights.EditRoles))
            {
                return GetNotAuthorized();
            }
            else if (Utils.StringIsNullOrWhitespace(id))
            {
                return new JsonResponse() { Message = "id argument is null." };
            }
            else if (vals == null)
            {
                return new JsonResponse() { Message = "vals argument is null." };
            }
            else if (vals.Length == 0 || Utils.StringIsNullOrWhitespace(vals[0]))
            {
                return new JsonResponse() { Message = "Role name is required field." };
            }

            var response = new JsonResponse();

            try
            {
                Right.OnRenamingRole(id, vals[0]);
                Roles.DeleteRole(id);
                Roles.CreateRole(vals[0]);
               
                response.Success = true;
                response.Message = string.Format("Role updated from \"{0}\" to \"{1}\"", id, vals[0]);
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Roles.UpdateRole: {0}", ex.Message));
                response.Message = string.Format("Could not update the role: {0}", vals[0]);
            }

            return response;
        }

        #endregion

        #region Methods

        private static JsonResponse GetNotAuthorized()
        {
            return new JsonResponse() { Success = false, Message = "Not authorized" };
        }

        #endregion

    }
}