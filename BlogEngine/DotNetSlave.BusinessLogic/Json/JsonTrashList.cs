namespace BlogEngine.Core.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Type of deleted object
    /// </summary>
    public enum TrashType
    {
        /// <summary>
        ///     Any deleted object
        /// </summary>
        All,

        /// <summary>
        ///     Deleted comment
        /// </summary>
        Comment,

        /// <summary>
        ///     Deleted post
        /// </summary>
        Post,

        /// <summary>
        ///     Deleted page
        /// </summary>
        Page
    }

    /// <summary>
    /// List of deleted objects
    /// </summary>
    public class JsonTrashList
    {
        /// <summary>
        /// Paged list of deleted objects
        /// </summary>
        /// <param name="trashType">Type of delted object</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="page">Page number</param>
        /// <returns></returns>
        public static List<JsonTrash> GetTrash(TrashType trashType, int pageSize, int page)
        {
            var cntTo = page * pageSize;
            var cntFrom = cntTo - pageSize;
            var cnt = 0;

            var comments = new List<Comment>();
            var posts = new List<Post>();
            var pages = new List<Page>();

            var trashList = new List<JsonTrash>();

            foreach (var p in Post.Posts)
            {
                switch (trashType)
                {
                    case TrashType.Comment:
                        comments.AddRange(p.DeletedComments);
                        break;
                    case TrashType.Post:
                        
                        break;
                    case TrashType.Page:
                        
                        break;
                    default:
                        comments.AddRange(p.DeletedComments);
                        break;
                }
            }

            if (comments.Count > 0)
            {
                foreach (var c in comments)
                {
                    JsonTrash t1 = new JsonTrash { 
                        Id = c.Id, 
                        Title = c.Title, 
                        ObjectType = "Comment",
                        Date = c.DateCreated.ToString("dd MMM yyyy"), 
                        Time = c.DateCreated.ToString("t") };

                    trashList.Add(t1);
                }
            }

            return trashList;
        }

        public static JsonResponse Process(string action, string[] vals)
        {
            try
            {
                foreach (var s in vals)
                {
                    var ar = s.Split((":").ToCharArray());

                    if (action == "Purge" && ar[0] == "All" && ar[1] == "All")
                    {
                        PurgeAll();
                        return new JsonResponse { Success = true, Message = "Trash is empty!" };
                    }
                    else
                    {
                        if (action == "Purge")
                        {
                            Purge(ar[0], new Guid(ar[1]));
                            return new JsonResponse { Success = true, Message = "Item(s) purged" };
                        }
                        else if (action == "Restore")
                        {
                            Restore(ar[0], new Guid(ar[1]));
                            return new JsonResponse { Success = true, Message = "Item(s) restored" };
                        }
                    }
                }
                return new JsonResponse { Success = true, Message = "Nothing to process" };
            }
            catch (Exception ex)
            {
                return new JsonResponse { Message = "BlogEngine.Core.Json.JsonTrashList.Restore: " + ex.Message };
            }
        }

        static void Restore(string trashType, Guid id)
        {
            switch (trashType)
            {
                case "Comment":
                    foreach (var p in Post.Posts.ToArray())
                    {
                        var cmnt = p.Comments.FirstOrDefault(c => c.Id == id);
                        if (cmnt != null)
                        {
                            p.RestoreComment(cmnt);
                            break;
                        }
                    }
                    break;
                case "Post":
                    break;
                case "Page":
                    break;
                default:
                    break;
            }
        }

        static void Purge (string trashType, Guid id)
        {
            switch (trashType)
            {
                case "Comment":
                    foreach (var p in Post.Posts.ToArray())
                    {
                        var cmnt = p.Comments.FirstOrDefault(c => c.Id == id);
                        if (cmnt != null)
                        {
                            p.PurgeComment(cmnt);
                            break;
                        }
                    }
                    break;
                case "Post":
                    break;
                case "Page":
                    break;
                default:
                    break;
            }
        }

        static void PurgeAll()
        {
            // remove deleted comments
            foreach (var p in Post.Posts.ToArray())
            {
                foreach (var c in p.DeletedComments.ToArray())
                {
                    p.PurgeComment(c);
                }
            }

            // remove deleted posts

            // remove deleted pages
        }

    }
}
