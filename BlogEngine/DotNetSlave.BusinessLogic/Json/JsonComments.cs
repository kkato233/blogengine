namespace BlogEngine.Core.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Comment type.
    /// </summary>
    public enum CommentType
    {
        /// <summary>
        ///     Pending Comment Type
        /// </summary>
        Pending,

        /// <summary>
        ///     Approved Comment Type
        /// </summary>
        Approved,

        /// <summary>
        ///     Pingbacks and trackbacks Comment Type
        /// </summary>
        Pingback,

        /// <summary>
        ///     Spam Comment Type
        /// </summary>
        Spam
    }

    /// <summary>
    /// List of comments
    /// </summary>
    public static class JsonComments
    {
        /// <summary>
        /// The current page.
        /// </summary>
        private static int currentPage = 1;

        /// <summary>
        /// The comm cnt.
        /// </summary>
        private static int commCnt;

        /// <summary>
        /// List of comments based on type for a single page.
        /// </summary>
        /// <param name="commentType">
        /// The comment type.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="page">
        /// The current page.
        /// </param>
        /// <returns>
        /// A list of JSON comments.
        /// </returns>
        public static List<JsonComment> GetComments(CommentType commentType, int pageSize, int page)
        {
            var cntTo = page * pageSize;
            var cntFrom = cntTo - pageSize;
            var cnt = 0;

            var allComments = new List<Comment>();
            var pageComments = new List<JsonComment>();

            foreach (var p in Post.Posts)
            {
                switch (commentType)
                {
                    case CommentType.Pending:
                        allComments.AddRange(p.NotApprovedComments);
                        break;
                    case CommentType.Pingback:
                        allComments.AddRange(p.Pingbacks);
                        break;
                    case CommentType.Spam:
                        allComments.AddRange(p.SpamComments);
                        break;
                    default:
                        allComments.AddRange(p.ApprovedComments);
                        break;
                }
            }

            allComments.Sort((x, y) => DateTime.Compare(y.DateCreated, x.DateCreated));

            foreach (var c in allComments)
            {
                cnt++;
                if (cnt <= cntFrom || cnt > cntTo)
                {
                    continue;
                }

                pageComments.Add(CreateJsonCommentFromComment(c));
            }

            currentPage = page;
            commCnt = cnt;

            return pageComments;
        }

        /// <summary>
        /// Single commnet by ID
        /// </summary>
        /// <param name="id">
        /// Comment id
        /// </param>
        /// <returns>
        /// A JSON Comment
        /// </returns>
        public static JsonComment GetComment(Guid id)
        {
            return (from p in Post.Posts
                    from c in p.AllComments
                    where c.Id == id
                    select CreateJsonCommentFromComment(c)).FirstOrDefault();
        }

        private static JsonComment CreateJsonCommentFromComment(Comment c)
        {
            JsonComment jc = new JsonComment();
            jc.Id = c.Id;
            jc.Email = c.Email;
            jc.Author = c.Author;
            jc.Title = c.Title;
            jc.Teaser = c.Teaser;
            jc.Website = c.Website == null ? "" : c.Website.ToString();
            jc.AuthorAvatar = c.Avatar;
            jc.Content = c.Content;
            jc.Ip = c.IP;
            jc.Date = c.DateCreated.ToString("dd MMM yyyy");
            jc.Time = c.DateCreated.ToString("t");
            return jc;
        }

        /// <summary>
        /// Builds pager control for comments page
        /// </summary>
        /// <param name="pageSize">
        /// Page size.
        /// </param>
        /// <param name="page">
        /// Current page
        /// </param>
        /// <param name="srvs">
        /// The Srvs..
        /// </param>
        /// <returns>
        /// HTML with next and previous buttons
        /// </returns>
        public static string GetPager(int pageSize, int page, string srvs)
        {
            if (commCnt == 0)
            {
                return string.Empty;
            }

            var prvLnk = string.Empty;
            var nxtLnk = string.Empty;
            var firstLnk = string.Empty;
            var lastLnk = string.Empty;
            const string LinkFormat = "<a href=\"#\" id=\"{0}\" onclick=\"return LoadComments({1}, '{2}');\" class=\"{0}\"></a>";

            var pgs = Convert.ToDecimal(commCnt) / Convert.ToDecimal(pageSize);
            var p = pgs - (int)pgs;
            var lastPage = p > 0 ? (int)pgs + 1 : (int)pgs;

            var currentScope = ((page * pageSize) - (pageSize - 1)).ToString() + " - " + (page * pageSize).ToString();

            var pageLink = string.Format("<span>Showing {0} of {1}</span>", currentScope, commCnt);

            if (currentPage > 1)
            {
                prvLnk = string.Format(LinkFormat, "prevLink", currentPage - 1, srvs);
                firstLnk = string.Format(LinkFormat, "firstLink", 1, srvs);
            }

            if (page < lastPage)
            {
                nxtLnk = string.Format(LinkFormat, "nextLink", currentPage + 1, srvs);
                lastLnk = string.Format(LinkFormat, "lastLink", lastPage, srvs);
            }

            return firstLnk + prvLnk + pageLink + nxtLnk + lastLnk;
        }
    }
}