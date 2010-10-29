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
    public class Comments : WebService
    {
        #region Constants and Fields

        /// <summary>
        ///     JSON object that will be return back to client
        /// </summary>
        private readonly JsonResponse response;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref = "Comments" /> class.
        /// </summary>
        static Comments()
        {
            CurrentPage = 1;
            LastPage = 1;
            CommCnt = 1;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Comments" /> class.
        /// </summary>
        public Comments()
        {
            this.response = new JsonResponse();
        }

        #endregion

        /// <summary>
        ///     Gets or sets the comm CNT.
        /// </summary>
        /// <value>The comm CNT.</value>
        protected static int CommCnt { get; set; }

        /// <summary>
        ///     Gets or sets the last page.
        /// </summary>
        /// <value>The last page.</value>
        protected static int LastPage { get; set; }

        /// <summary>
        ///     Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        protected static int CurrentPage { get; set; }
        
        /// <summary>
        /// Reject selected comments
        /// </summary>
        /// <param name="vals">
        /// Array of comments
        /// </param>
        /// <returns>
        /// Json response
        /// </returns>
        [WebMethod]
        public JsonResponse Reject(string[] vals)
        {
            this.response.Success = false;

            if (!Security.IsAuthorizedTo(Rights.ModerateComments))
            {
                this.response.Message = "Not authorized";
                return this.response;
            }

            if (string.IsNullOrEmpty(vals[0]))
            {
                return this.response;
            }

            try
            {
                foreach (var p in Post.Posts.ToArray())
                {
                    foreach (var c in from c in p.Comments.ToArray() from t in vals where c.Id == new Guid(t) select c)
                    {
                        CommentHandlers.AddIpToFilter(c.IP, true);
                        CommentHandlers.ReportMistake(c);

                        c.ModeratedBy = this.User.Identity.Name;
                        p.DisapproveComment(c);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.Log("Api.Comments.Reject", ex);
                this.response.Message = "Error rejecting comment";
                return this.response;
            }

            this.response.Success = true;
            this.response.Message = "Selected comments rejected";
            return this.response;
        }

        /// <summary>
        /// Restore selected comments
        /// </summary>
        /// <param name="vals">
        /// Array of comments
        /// </param>
        /// <returns>
        /// Json response
        /// </returns>
        [WebMethod]
        public JsonResponse Approve(string[] vals)
        {
            this.response.Success = false;

            if (!Security.IsAuthorizedTo(Rights.ModerateComments))
            {
                this.response.Message = "Not authorized";
                return this.response;
            }

            if (string.IsNullOrEmpty(vals[0]))
            {
                return this.response;
            }

            try
            {
                var toapprove = from p in Post.Posts
                                from c in p.Comments
                                join t in vals on c.Id equals new Guid(t)
                                select new { p, c };

                foreach (var t in toapprove)
                {
                    CommentHandlers.AddIpToFilter(t.c.IP, false);
                    CommentHandlers.ReportMistake(t.c);

                    t.c.ModeratedBy = this.User.Identity.Name;
                    t.p.ApproveComment(t.c);
                }
            }
            catch (Exception ex)
            {
                Utils.Log("Api.Comments.Approve", ex);
                this.response.Message = string.Format("Could not restore comment: {0}", vals[0]);
                return this.response;
            }

            this.response.Success = true;
            this.response.Message = "Selected comments restored";
            return this.response;
        }

        /// <summary>
        /// Delete selected comments
        /// </summary>
        /// <param name="vals">
        /// Array of comments
        /// </param>
        /// <returns>
        /// Json response
        /// </returns>
        [WebMethod]
        public JsonResponse Delete(string[] vals)
        {
            this.response.Success = false;

            if (Security.IsAuthorizedTo(Rights.ModerateComments))
            {
                this.response.Message = "Not authorized";
                return this.response;
            }

            if (string.IsNullOrEmpty(vals[0]))
            {
                return this.response;
            }

            try
            {
                var tmp = new List<Comment>();

                foreach (var post in Post.Posts)
                {
                    var post1 = post;
                    tmp.AddRange(
                        vals.Select(t => post1.Comments.Find(c => c.Id == new Guid(t))).Where(
                            comment => comment != null));
                }

                foreach (var c in tmp)
                {
                    this.RemoveComment(c);
                }
            }
            catch (Exception ex)
            {
                Utils.Log("Api.Comments.Delete", ex);
                this.response.Message = string.Format("Could not delete comment: {0}", ex.Message);
                return this.response;
            }

            this.response.Success = true;
            this.response.Message = "Selected comments deleted";
            return this.response;
        }

        /// <summary>
        /// Delete all spam comments
        /// </summary>
        /// <returns>
        /// Json response
        /// </returns>
        [WebMethod]
        public JsonResponse DeleteAll()
        {
            this.response.Success = false;

            if (!Security.IsAuthorizedTo(Rights.ModerateComments))
            {
                this.response.Message = "Not authorized";
                return this.response;
            }

            try
            {
                this.DeleteAllComments();
                this.response.Success = true;
                this.response.Message = "Comments deleted";
                return this.response;
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Api.Comments.DeleteAll: {0}", ex.Message));
                this.response.Message = string.Format("Could not delete all comment: {0}", ex.Message);
                return this.response;
            }

        }

        /// <summary>
        /// Save the comment.
        /// </summary>
        /// <param name="id">
        /// The comment id.
        /// </param>
        /// <param name="author">
        /// The author.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="website">
        /// The website.
        /// </param>
        /// <param name="cont">
        /// The content.
        /// </param>
        /// <returns>
        /// A JSON Comment.
        /// </returns>
        [WebMethod]
        public static JsonComment SaveComment(string id, string author, string email, string website, string cont)
        {

            // There really needs to be validation here so people aren't just posting willy-nilly
            // as anyone they want.

            if (!Security.IsAuthorizedTo(Rights.CreateComments))
            {
                throw new System.Security.SecurityException("Can not create comment");
            }


            var gId = new Guid(id);
            var jc = new JsonComment();

            foreach (var p in Post.Posts.ToArray())
            {
                foreach (var c in p.Comments.Where(c => c.Id == gId).ToArray())
                {
                    c.Author = author;
                    c.Email = email;
                    c.Website = new Uri(website);
                    c.Content = cont;

                    p.Save();
                    return JsonComments.GetComment(gId);
                }
            }

            return jc;
        }

        /// <summary>
        /// Removes the comment.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        protected void RemoveComment(Comment comment)
        {
            // just as in DeleteAll, we can't use foreach/var
            // to avoid "collection modified" error (sorry ReSharper...)
            var found = false;
            for (int i = 0; i < Post.Posts.Count; i++)
            {
                for (int j = 0; j < Post.Posts[i].Comments.Count; j++)
                {
                    if (Post.Posts[i].Comments[j].Id == comment.Id)
                    {
                        Post.Posts[i].RemoveComment(Post.Posts[i].Comments[j]);
                        found = true;
                        break;
                    }
                }
                if (found) { break; }
            }
        }

        /// <summary>
        /// Deletes all comments.
        /// </summary>
        protected void DeleteAllComments()
        {
            if (Post.Posts.Count <= 0)
            {
                return;
            }

            // loop backwards to avoid "collection was modified" error
            for (var i = Post.Posts.Count - 1; i >= 0; i--)
            {
                if (Post.Posts[i].Comments.Count <= 0)
                {
                    continue;
                }

                for (var j = Post.Posts[i].Comments.Count - 1; j >= 0; j--)
                {
                    var comment = Post.Posts[i].Comments[j];

                    // spam comments should never have children but
                    // be on a safe side insure we won't create
                    // orphan comment with deleted parent
                    if (!comment.IsApproved && comment.Comments.Count == 0)
                    {
                        Post.Posts[i].RemoveComment(comment);
                    }
                }
            }
        }
    }
}