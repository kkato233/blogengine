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

            var pageComments = new List<JsonComment>();

            foreach (var p in Post.Posts)
            {
                List<Comment> allComments;
                switch (commentType)
                {
                    case CommentType.Pending:
                        allComments = p.NotApprovedComments;
                        break;
                    case CommentType.Pingback:
                        allComments = p.Pingbacks;
                        break;
                    case CommentType.Spam:
                        allComments = p.SpamComments;
                        break;
                    default:
                        allComments = p.ApprovedComments;
                        break;
                }

                allComments.Sort((x, y) => DateTime.Compare(y.DateCreated, x.DateCreated));

                foreach (var c in allComments)
                {
                    cnt++;
                    if (cnt <= cntFrom || cnt > cntTo)
                    {
                        continue;
                    }

                    var jc = new JsonComment
                        {
                            Id = c.Id,
                            Email = c.Email,
                            Author = c.Author,
                            Title = c.Title,
                            Teaser = c.Teaser,
                            Ip = c.IP,
                            Date = c.DateCreated.ToString("dd MMM yyyy"),
                            Time = c.DateCreated.ToString("t")
                        };
                    pageComments.Add(jc);
                }
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
                    select new JsonComment
                        {
                            Id = c.Id,
                            Email = c.Email,
                            Author = c.Author,
                            Title = c.Title,
                            Teaser = c.Teaser,
                            Website = c.Website == null ? string.Empty : c.Website.ToString(),
                            Content = c.Content,
                            Ip = c.IP,
                            Date = c.DateCreated.ToString("dd MMM yyyy"),
                            Time = c.DateCreated.ToString("t")
                        }).FirstOrDefault();
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
            const string LinkFormat = "<a href=\"#\" id=\"{0}\" onclick=\"return LoadComments({1}, '{3}');\" alt=\"{2}\">{2}</a>";

            var pgs = Convert.ToDecimal(commCnt) / Convert.ToDecimal(pageSize);
            var p = pgs - (int)pgs;
            var lastPage = p > 0 ? (int)pgs + 1 : (int)pgs;

            var pageLink = string.Format("<span>Page {0} of {1}</span>", currentPage, lastPage);

            if (currentPage > 1)
            {
                prvLnk = string.Format(LinkFormat, "prevLink", currentPage - 1, "&lt;&lt; ", srvs);
            }

            if (page < lastPage)
            {
                nxtLnk = string.Format(LinkFormat, "nextLink", currentPage + 1, " &gt;&gt;", srvs);
            }

            return prvLnk + pageLink + nxtLnk;
        }
    }
}