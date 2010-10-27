namespace Admin.Tracking
{
    using System.Collections;
    using System.Web.Services;
    using BlogEngine.Core.Json;

    public partial class Pingbacks : System.Web.UI.Page
    {
        /// <summary>
        /// Number of comments in the list
        /// </summary>
        protected static int CommentCounter { get; set; }

        [WebMethod]
        public static IEnumerable LoadComments(int pageSize, int page)
        {
            var commentList = JsonComments.GetComments(CommentType.Pingback, pageSize, page);
            CommentCounter = commentList.Count;
            return commentList;
        }

        [WebMethod]
        public static string LoadPager(int pageSize, int page)
        {
            return JsonComments.GetPager(pageSize, page, "Pingbacks.aspx");
        }
    }
}