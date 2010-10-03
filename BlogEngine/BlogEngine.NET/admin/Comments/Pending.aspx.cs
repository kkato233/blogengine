namespace Admin.Comments
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;  
    using System.Web.Services;
    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    public partial class Pending : System.Web.UI.Page
    {
        [WebMethod]
        public static IEnumerable LoadComments(int PageSize, int Page)
        {
            return JsonComments.GetComments(CommentType.Pending, PageSize, Page);
        }

        [WebMethod]
        public static string LoadPager(int PageSize, int Page)
        {
            return JsonComments.GetPager(PageSize, Page, "Pending.aspx");
        }

        //protected static int currentPage = 1;
        //protected static int lastPage = 1;
        //protected static int commCnt = 1;

        //[WebMethod]
        //public static IEnumerable LoadComments(int PageSize, int Page)
        //{
        //    int cntTo = Page * PageSize;
        //    int cntFrom = cntTo - PageSize + 1;
        //    int cnt = 1;           
        //    var comments = new List<JsonComment>();

        //    foreach (Post p in Post.Posts)
        //    {
        //        foreach (Comment c in p.NotApprovedComments)
        //        {
        //            if (cnt >= cntFrom && cnt <= cntTo)
        //            {
        //                JsonComment jc = new JsonComment();
        //                jc.Id = c.Id;
        //                jc.Email = c.Email;
        //                jc.Author = c.Author;
        //                jc.Title = c.Title;
        //                jc.Teaser = c.Teaser;
        //                jc.Ip = c.IP;
        //                jc.Date = c.DateCreated.ToString("dd MMM yyyy");
        //                jc.Time = c.DateCreated.ToString("t");
        //                comments.Add(jc);
        //            }
        //            cnt++;
        //        }
        //    }
        //    currentPage = Page;
        //    commCnt = cnt;

        //    return comments;
        //}

        //[WebMethod]
        //public static string LoadPager(int PageSize, int Page)
        //{
        //    if (commCnt == 0) return "";

        //    string prvLnk = "";
        //    string nxtLnk = "";
        //    string lnk = "<a href=\"#\" id=\"{0}\" onclick=\"return LoadComments({1}, 'Pending.aspx');\" alt=\"{2}\">{2}</a>";

        //    decimal pgs = Convert.ToDecimal(commCnt) / Convert.ToDecimal(PageSize);
        //    decimal p = pgs - (int)pgs;
        //    int lastPage = p > 0 ? (int)pgs + 1 : (int)pgs;

        //    string pgLink = string.Format("<span>Page {0} of {1}</span>", currentPage, lastPage);

        //    if (currentPage > 1)
        //    {
        //        prvLnk = string.Format(lnk, "prevLink", (currentPage - 1).ToString(), "&lt;&lt; ");
        //    }

        //    if (PageSize * Page < commCnt)
        //    {
        //        nxtLnk = string.Format(lnk, "nextLink", (currentPage + 1).ToString(), " &gt;&gt;");
        //    }

        //    return prvLnk + pgLink + nxtLnk;
        //}
    }

}