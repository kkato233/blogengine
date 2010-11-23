using System;
using System.Linq;
using BlogEngine.Core;
using BlogEngine.Core.Json;

namespace App_Code
{
    using System.Web.Services;

    /// <summary>
    /// Summary description for Tags
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class Tags : WebService
    {

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
                return new JsonResponse { Success = false, Message = "Not authorized" };
            }
            if (Utils.StringIsNullOrWhitespace(id))
            {
                return new JsonResponse { Message = "id argument is null." };
            }
            if (vals == null)
            {
                return new JsonResponse { Message = "vals argument is null." };
            }
            if (vals.Length == 0 || Utils.StringIsNullOrWhitespace(vals[0]))
            {
                return new JsonResponse { Message = "Tag is required field." };
            }

            var response = new JsonResponse();
            try
            {
                foreach (var p in Post.Posts.ToArray())
                {
                    var tg = p.Tags.FirstOrDefault(tag => tag == id);
                    if(tg != null)
                    {
                        p.Tags.Remove(tg);
                        p.Tags.Add(vals[0]);
                        p.DateModified = DateTime.Now;
                        p.Save();
                    }
                }
                response.Success = true;
                response.Message = string.Format("Tag updated from \"{0}\" to \"{1}\"", id, vals[0]);
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Tags.Update: {0}", ex.Message));
                response.Message = string.Format("Could not update the tag: {0}", vals[0]);
            }

            return response;
        }

        /// <summary>
        /// Delete tag in all posts
        /// </summary>
        /// <param name="id">Tag</param>
        /// <returns>Response object</returns>
        [WebMethod]
        public JsonResponse Delete(string id)
        {
            if (!Security.IsAuthorizedTo(Rights.DeleteRoles))
            {
                return new JsonResponse { Success = false, Message = "Not authorized" };
            }
            if (Utils.StringIsNullOrWhitespace(id))
            {
                return new JsonResponse { Message = "Role name is required field" };
            }

            try
            {
                foreach (var p in Post.Posts.ToArray())
                {
                    var tg = p.Tags.FirstOrDefault(tag => tag == id);
                    if (tg != null)
                    {
                        p.Tags.Remove(tg);
                        p.DateModified = DateTime.Now;
                        p.Save();
                    }
                }
                return new JsonResponse { Success = true, Message = string.Format("Tag \"{0}\" has been deleted", id) };
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Tags.Delete: {0}", ex.Message));
                return new JsonResponse { Message = string.Format("Could not delete the tag: {0}", id) };
            }
        }

    }
}