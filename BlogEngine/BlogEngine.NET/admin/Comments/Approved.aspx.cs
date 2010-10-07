namespace admin.Comments
{
    using System;
    using System.Collections;
    using System.Web.Services;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    public partial class Approved : System.Web.UI.Page
    {
        [WebMethod]
        public static IEnumerable LoadComments(int PageSize, int Page)
        {
            return JsonComments.GetComments(CommentType.Approved, PageSize, Page);
        }

        [WebMethod]
        public static string LoadPager(int PageSize, int Page)
        {
            return JsonComments.GetPager(PageSize, Page, "Approved.aspx");
        }

        [WebMethod]
        public static JsonComment GetComment(string id)
        {
            return JsonComments.GetComment(new System.Guid(id));
        }

        [WebMethod]
        public static JsonComment SaveComment(string[] vals)
        {
            Guid gId = new Guid(vals[0]);         
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
    }
}