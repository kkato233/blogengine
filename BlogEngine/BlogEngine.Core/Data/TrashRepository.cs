using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using BlogEngine.Core.Data.Models;
using BlogEngine.Core.Web.Navigation;
using BlogEngine.Core.Data.Contracts;

namespace BlogEngine.Core.Data
{
    /// <summary>
    /// Trash repository
    /// </summary>
    public class TrashRepository : ITrashRepository
    {
        static int listCount = 0;
        static int currentPage = 1;

        /// <summary>
        /// Get trash list
        /// </summary>
        /// <param name="trashType">Type (post, page, comment)</param>
        /// <param name="take">Take for a page</param>
        /// <param name="skip">Items to sckip</param>
        /// <param name="filter">Filter expression</param>
        /// <param name="order">Sort order</param>
        /// <returns></returns>
        public List<TrashItem> GetTrash(TrashType trashType, int take = 10, int skip = 0, string filter = "1 == 1", string order = "DateCreated descending")
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages))
                throw new System.UnauthorizedAccessException();

            var comments = new List<Comment>();
            var posts = new List<Post>();
            var pages = new List<Page>();
            var trashList = new List<TrashItem>();
            var trashPage = new List<TrashItem>();
            var query = trashList.AsQueryable().Where(filter);

            // comments
            if (trashType == TrashType.All || trashType == TrashType.Comment)
            {
                foreach (var p in Post.Posts)
                {
                    comments.AddRange(p.DeletedComments);
                }
            }

            var tmpl = "<a href='{0}'>{1}: {2}</a>";
            if (comments.Count > 0)
            {
                foreach (var c in comments)
                {
                    TrashItem t1 = new TrashItem
                    {
                        Id = c.Id,
                        Title = c.Author + ": " + c.Teaser,
                        RelativeUrl = c.RelativeLink,
                        ObjectType = "Comment",
                        DateCreated = c.DateCreated.ToString("MM/dd/yyyy HH:mm")
                    };

                    trashList.Add(t1);
                }
            }

            // posts
            if (trashType == TrashType.All || trashType == TrashType.Post)
            {
                posts = (from x in Post.DeletedPosts orderby x.DateCreated descending select x).ToList();
            }

            if (posts.Count > 0)
            {
                foreach (var p in posts)
                {
                    TrashItem t2 = new TrashItem
                    {
                        Id = p.Id,
                        Title = System.Web.HttpContext.Current.Server.HtmlEncode(p.Title),
                        RelativeUrl = p.RelativeLink,
                        ObjectType = "Post",
                        DateCreated = p.DateCreated.ToString("MM/dd/yyyy HH:mm")
                    };

                    trashList.Add(t2);
                }
            }

            // pages
            if (trashType == TrashType.All || trashType == TrashType.Page)
            {
                pages = (from x in Page.DeletedPages orderby x.DateCreated descending select x).ToList();
            }

            if (pages.Count > 0)
            {
                foreach (var p in pages)
                {
                    TrashItem t3 = new TrashItem
                    {
                        Id = p.Id,
                        Title = System.Web.HttpContext.Current.Server.HtmlEncode(p.Title),
                        RelativeUrl = p.RelativeLink,
                        ObjectType = "Page",
                        DateCreated = p.DateCreated.ToString("MM/dd/yyyy HH:mm")
                    };

                    trashList.Add(t3);
                }
            }

            // if take passed in as 0, return all
            if (take == 0) take = trashList.Count;

            foreach (var item in query.OrderBy(order).Skip(skip).Take(take))
                trashPage.Add(item);

            return trashPage;
        }

        /// <summary>
        /// Builds pager control for trash list page
        /// </summary>
        /// <param name="page">Current Page Number</param>
        /// <returns></returns>
        public string GetPager(int page)
        {
            if (listCount == 0)
                return string.Empty;

            IPager pager = new BlogEngine.Core.Web.Navigation.Pager(page, BlogConfig.GenericPageSize, listCount);

            return pager.Render(page, "LoadTrash(null,{1})");
        }

        /// <summary>
        /// If trash is empty.
        /// </summary>
        /// <returns>True if empty.</returns>
        public bool IsTrashEmpty()
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages))
                throw new System.UnauthorizedAccessException();

            foreach (var p in Post.Posts)
            {
                if (p.DeletedComments.Count > 0) return false;
            }
            if (Post.DeletedPosts.Count > 0) return false;
            if (Page.DeletedPages.Count > 0) return false;
            return true;
        }

        /// <summary>
        /// Processes recycle bin actions
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="vals">Values</param>
        /// <returns>Response</returns>
        public JsonResponse Process(string action, string[] vals)
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages))
                throw new System.UnauthorizedAccessException();
            try
            {
                string message = null;

                foreach (var s in vals)
                {
                    var ar = s.Split((":").ToCharArray());

                    if (action == "Purge" && ar[0] == "All" && ar[1] == "All")
                    {
                        PurgeAll();
                        message = "Trash is empty!";
                    }
                    else
                    {
                        if (action == "Purge")
                        {
                            Purge(ar[0], new Guid(ar[1]));
                            message = string.Format("Item{0} purged", (vals.Length > 1) ? "s" : "");
                        }
                        else if (action == "Restore")
                        {
                            Restore(ar[0], new Guid(ar[1]));
                            message = string.Format("Item{0} restored", (vals.Length > 1) ? "s" : "");
                        }
                    }
                }

                if (string.IsNullOrEmpty(message))
                    return new JsonResponse { Success = true, Message = "Nothing to process" };
                else
                    return new JsonResponse { Success = true, Message = message };
            }
            catch (Exception ex)
            {
                return new JsonResponse { Message = "BlogEngine.Core.Data.TrashRepository.Restore: " + ex.Message };
            }
        }

        /// <summary>
        /// Restore
        /// </summary>
        /// <param name="trashType">Trash type</param>
        /// <param name="id">Id</param>
        public void Restore(string trashType, Guid id)
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages))
                throw new System.UnauthorizedAccessException();

            switch (trashType)
            {
                case "Comment":
                    foreach (var p in Post.Posts.ToArray())
                    {
                        var cmnt = p.DeletedComments.FirstOrDefault(c => c.Id == id);
                        if (cmnt != null)
                        {
                            p.RestoreComment(cmnt);
                            break;
                        }
                    }
                    break;
                case "Post":
                    var delPost = Post.DeletedPosts.Where(p => p.Id == id).FirstOrDefault();
                    if (delPost != null) delPost.Restore();
                    break;
                case "Page":
                    var delPage = Page.DeletedPages.Where(pg => pg.Id == id).FirstOrDefault();
                    if (delPage != null) delPage.Restore();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Purge
        /// </summary>
        /// <param name="trashType">Trash type</param>
        /// <param name="id">Id</param>
        public void Purge(string trashType, Guid id)
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages))
                throw new System.UnauthorizedAccessException();

            switch (trashType)
            {
                case "Comment":
                    foreach (var p in Post.Posts.ToArray())
                    {
                        var cmnt = p.DeletedComments.FirstOrDefault(c => c.Id == id);
                        if (cmnt != null)
                        {
                            p.PurgeComment(cmnt);
                            break;
                        }
                    }
                    break;
                case "Post":
                    var delPost = Post.DeletedPosts.Where(p => p.Id == id).FirstOrDefault();
                    if (delPost != null) delPost.Purge();
                    break;
                case "Page":
                    var delPage = Page.DeletedPages.Where(pg => pg.Id == id).FirstOrDefault();
                    if (delPage != null) delPage.Purge();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Purge all
        /// </summary>
        public void PurgeAll()
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages))
                throw new System.UnauthorizedAccessException();

            // remove deleted comments
            foreach (var p in Post.Posts.ToArray())
            {
                foreach (var c in p.DeletedComments.ToArray())
                {
                    p.PurgeComment(c);
                }
            }

            // remove deleted posts
            foreach (var p in Post.DeletedPosts.ToArray())
            {
                p.Purge();
            }

            // remove deleted pages
            foreach (var pg in Page.DeletedPages.ToArray())
            {
                pg.Purge();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResponse PurgeLogfile()
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages))
                throw new System.UnauthorizedAccessException();

            string fileLocation = System.Web.Hosting.HostingEnvironment.MapPath(System.IO.Path.Combine(BlogConfig.StorageLocation, "logger.txt"));
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.StreamWriter sw = System.IO.File.CreateText(fileLocation);

                sw.WriteLine("Purged at " + DateTime.Now);
                sw.Close();
                return new JsonResponse { Success = true, Message = "Log file purged" };
            }
            return new JsonResponse { Success = false, Message = "Log file not found" };
        }
    }
}
