namespace App_Code
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Services;
    using System.Web.Services;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    /// <summary>
    /// The comments.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class Posts : WebService
    {
        [WebMethod]
        public JsonResponse DeletePost(string id)
        {
            JsonResponse response = new JsonResponse();
            response.Success = false;

            if (!this.User.IsInRole(BlogSettings.Instance.AdministratorRole))
            {
                response.Message = "Not authorized";
                return response;
            }

            if (string.IsNullOrEmpty(id))
            {
                return response;
            }

            try
            {
                var tmp = new Post();

                foreach (var post in Post.Posts)
                {
                    if (post.Id == new Guid(id))
                    {
                        tmp = post;
                        break;
                    }
                }
                Post.Posts.Remove(tmp);
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Api.Posts.Delete: {0}", ex.Message));
                response.Message = string.Format("Could not delete post: {0}", ex.Message);
                return response;
            }

            response.Success = true;
            response.Message = "Post deleted";
            return response;
        }
    }

}
