namespace Admin
{
    using System;
    using System.Collections;
    using System.Web.Services;
    using System.Web;
    using System.Collections.Generic;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    public partial class AjaxHelper : System.Web.UI.Page
    {
        

        [WebMethod]
        public static JsonComment GetComment(string id)
        {
            if (!HttpContext.Current.User.IsInRole(BlogSettings.Instance.AdministratorRole))
            {
                return null;
            }
            return JsonComments.GetComment(new Guid(id));
        }

        [WebMethod]
        public static JsonComment SaveComment(string[] vals)
        {
            if (!HttpContext.Current.User.IsInRole(BlogSettings.Instance.AdministratorRole))
            {
                return null;
            }
            var gId = new Guid(vals[0]);
            string author = vals[1];
            string email = vals[2];
            string website = vals[3];
            string cont = vals[4];

            foreach (Post p in Post.Posts.ToArray())
            {
                foreach (Comment c in p.Comments.ToArray())
                {
                    if (c.Id == gId)
                    {
                        c.Author = author;
                        c.Email = email;
                        c.Website = string.IsNullOrEmpty(website) ? null : new Uri(website);
                        c.Content = cont;

                        p.Save();
                        return JsonComments.GetComment(gId);
                    }
                }
            }

            return new JsonComment();
        }

        [WebMethod]
        public static IEnumerable LoadPosts(int pageSize, int page, string  type)
        {
            return JsonPosts.GetPosts(pageSize, page, type);
        }

        [WebMethod]
        public static string LoadPostPager(int pageSize, int page, string type)
        {
            return JsonPosts.GetPager(pageSize, page, type);
        }

    }
}