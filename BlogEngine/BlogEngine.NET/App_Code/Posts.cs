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
        [Obsolete]
        [WebMethod]
        public JsonResponse DeletePost(string id)
        {
            if (Utils.StringIsNullOrWhitespace(id))
            {
                return new JsonResponse() { Message = "Invalid post id" };
            }

            var post = Post.GetPost(new Guid(id));
            if (post == null)
            {
                return new JsonResponse() { Message = "Invalid post id" };
            }

            if (!post.CanUserDelete)
            {
                return new JsonResponse() { Message = "Not authorized." };
            }
            else
            {
                try
                {
                    post.Delete();
                    post.Save();
                    return new JsonResponse() { Success = true, Message = "Post deleted" };

                }
                catch (Exception ex)
                {
                    Utils.Log(string.Format("Api.Posts.DeletePost: {0}", ex.Message));
                    return new JsonResponse() { Message = string.Format("Could not delete post: {0}", ex.Message) };
                }

            }
        }

        [WebMethod]
        public JsonResponse DeletePage(string id)
        {
            JsonResponse response = new JsonResponse();
            response.Success = false;

            if (string.IsNullOrEmpty(id))
            {
                response.Message = "Page id is required";
                return response;
            }

            var page = Page.GetPage(new Guid(id));
            if (page == null)
            {
                return new JsonResponse() { Message = "Invalid page id" };
            }

            if (!page.CanUserDelete)
            {
                return new JsonResponse() { Message = "Not authorized." };
            }

            try
            {
                page.Delete();
                page.Save();
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Api.Posts.DeletePage: {0}", ex.Message));
                response.Message = string.Format("Could not delete page: {0}", ex.Message);
                return response;
            }

            response.Success = true;
            response.Message = "Page deleted";
            return response;
        }


    }

}
