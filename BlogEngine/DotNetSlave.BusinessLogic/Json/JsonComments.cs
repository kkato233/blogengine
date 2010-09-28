using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogEngine.Core.Json
{
    /// <summary>
    /// Comment type.
    /// </summary>
    public enum CommentType 
    { 
        /// <summary>
        /// Penging
        /// </summary>
        Pending, 
        /// <summary>
        /// Approved
        /// </summary>
        Approved, 
        /// <summary>
        /// Pingbacks and trackbacks
        /// </summary>
        Pingback, 
        /// <summary>
        /// Spam
        /// </summary>
        Spam 
    }

    /// <summary>
    /// List of comments
    /// </summary>
    public static class JsonComments
    {
        static int currentPage = 1;
        static int commCnt = 0;

        /// <summary>
        /// List of comments based on type for a single page
        /// </summary>
        /// <param name="cType">Comment type</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="Page">Current page</param>
        /// <returns></returns>
        public static List<JsonComment> GetComments(CommentType cType, int PageSize, int Page)
        {
            int cntTo = Page * PageSize;
            int cntFrom = cntTo - PageSize;
            int cnt = 0;

            List<Comment> allComments = new List<Comment>();
            List<JsonComment> pageComments = new List<JsonComment>();

            foreach (Post p in Post.Posts)
            {
                switch (cType)
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
                foreach (Comment c in allComments)
                {
                    cnt++;
                    if (cnt > cntFrom && cnt <= cntTo)
                    {
                        JsonComment jc = new JsonComment();
                        jc.Id = c.Id;
                        jc.Email = c.Email;
                        jc.Author = c.Author;
                        jc.Title = c.Title;
                        jc.Teaser = c.Teaser;
                        jc.Ip = c.IP;
                        jc.Date = c.DateCreated.ToString("dd MMM yyyy");
                        jc.Time = c.DateCreated.ToString("t");
                        pageComments.Add(jc);
                    }
                }
            }
            currentPage = Page;
            commCnt = cnt;

            return pageComments;
        }

        /// <summary>
        /// Single commnet by ID
        /// </summary>
        /// <param name="id">Comment id</param>
        /// <returns>Comment</returns>
        public static JsonComment GetComment(Guid id)
        {
            foreach (Post p in Post.Posts)
            {
                foreach (Comment c in p.AllComments)
                {
                    if (c.Id == id)
                    {
                        JsonComment jc = new JsonComment();
                        jc.Id = c.Id;
                        jc.Email = c.Email;
                        jc.Author = c.Author;
                        jc.Title = c.Title;
                        jc.Teaser = c.Teaser;
                        jc.Website = c.Website == null ? "" : c.Website.ToString();
                        jc.Content = c.Content;
                        jc.Ip = c.IP;
                        jc.Date = c.DateCreated.ToString("dd MMM yyyy");
                        jc.Time = c.DateCreated.ToString("t");
                        return jc;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Builds pager control for comments page
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Page">Current page</param>
        /// <returns>HTML with next and previous buttons</returns>
        public static string GetPager(int PageSize, int Page, string Srvs)
        {
            if (commCnt == 0) return "";

            string prvLnk = "";
            string nxtLnk = "";
            string lnk = "<a href=\"#\" id=\"{0}\" onclick=\"return LoadComments({1}, '" + Srvs + "');\" alt=\"{2}\">{2}</a>";

            decimal pgs = Convert.ToDecimal(commCnt) / Convert.ToDecimal(PageSize);
            decimal p = pgs - (int)pgs;
            int lastPage = p > 0 ? (int)pgs + 1 : (int)pgs;

            string pgLink = string.Format("<span>Page {0} of {1}</span>", currentPage, lastPage);

            if (currentPage > 1)
            {
                prvLnk = string.Format(lnk, "prevLink", (currentPage - 1).ToString(), "&lt;&lt; ");
            }

            if (Page < lastPage)
            {
                nxtLnk = string.Format(lnk, "nextLink", (currentPage + 1).ToString(), " &gt;&gt;");
            }

            return prvLnk + pgLink + nxtLnk;
        }
    }
}
