namespace BlogEngine.Core.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Post type
    /// </summary>
    public enum PostType
    {
        /// <summary>
        /// All posts
        /// </summary>
        All,
        /// <summary>
        /// Drafts
        /// </summary>
        Draft,
        /// <summary>
        /// Published posts
        /// </summary>
        Published
    }

    /// <summary>
    /// List of posts
    /// </summary>
    public static class JsonPosts
    {
        /// <summary>
        /// The current page.
        /// </summary>
        private static int currentPage = 1;

        /// <summary>
        /// The comm cnt.
        /// </summary>
        private static int postCnt;

        /// <summary>
        /// List of drafts or published posts for a single page
        /// </summary>
        /// <param name="postType">Post type</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="page">Page number</param>
        /// <returns>Json-friendly list of posts</returns>
        public static List<JsonPost> GetPosts(int pageSize, int page, string postType)
        {
            var cntTo = page * pageSize;
            var cntFrom = cntTo - pageSize;
            var cnt = 0;

            var allPosts = new List<Post>();
            var pagePosts = new List<JsonPost>();

            foreach (var p in Post.Posts)
            {
                switch (postType)
                {
                    case "Published":
                        if(p.IsPublished)
                            allPosts.Add(p);
                        break;
                    case "Draft":
                        if (!p.IsPublished)
                            allPosts.Add(p);
                        break;
                    default:
                        allPosts.Add(p);
                        break;
                }
            }

            allPosts.Sort((x, y) => DateTime.Compare(y.DateCreated, x.DateCreated));

            foreach (var x in allPosts)
            {
                cnt++;
                if (cnt <= cntFrom || cnt > cntTo)
                {
                    continue;
                }

                string categories = x.Categories.Aggregate("", (current, c) => current + (c.Title + ","));
                string tags = x.Tags.Aggregate("", (current, tag) => current + (tag + ","));

                var jp = new JsonPost
                {
                    Id = x.Id,
                    Author = x.Author,
                    Title = x.Title,
                    Date = x.DateCreated.ToString("dd MMM yyyy"),
                    Time = x.DateCreated.ToString("t"),
                    Categories = RemoveTrailingComma(categories),
                    Tags = RemoveTrailingComma(tags),
                    Comments = x.Comments.Count
                };
                pagePosts.Add(jp);
            }

            currentPage = page;
            postCnt = cnt;

            return pagePosts;
        }

        /// <summary>
        /// Builds pager control for posts page
        /// </summary>
        /// <param name="pageSize">Page size</param>
        /// <param name="page">Page number</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPager(int pageSize, int page, string type)
        {
            if (postCnt == 0)
            {
                return string.Empty;
            }

            var prvLnk = string.Empty;
            var nxtLnk = string.Empty;
            const string linkFormat = "<a href=\"#\" id=\"{0}\" onclick=\"return LoadPosts({1}, '{3}');\" alt=\"{2}\">{2}</a>";

            var pgs = Convert.ToDecimal(postCnt) / Convert.ToDecimal(pageSize);
            var p = pgs - (int)pgs;
            var lastPage = p > 0 ? (int)pgs + 1 : (int)pgs;

            var pageLink = string.Format("Page <span id=\"PagerCurrentPage\">{0}</span> of {1}", currentPage, lastPage);

            if (currentPage > 1)
            {
                prvLnk = string.Format(linkFormat, "prevLink", currentPage - 1, "&lt;&lt; ", type);
            }

            if (page < lastPage)
            {
                nxtLnk = string.Format(linkFormat, "nextLink", currentPage + 1, " &gt;&gt;", type);
            }

            return "<div id=\"ListPager\">" + prvLnk + pageLink + nxtLnk + "</div>";
        }

        static string RemoveTrailingComma(string s)
        {
            if (s.Trim().Length == 0) 
                return string.Empty;
            
            return s.Trim().Substring(0, s.Length - 1);
        }
    }
}
