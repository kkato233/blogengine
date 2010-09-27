namespace admin.Comments
{
    using System.Collections;
    using System.Web.Services;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    public partial class Pingbacks : System.Web.UI.Page
    {
        [WebMethod]
        public static IEnumerable LoadComments(int PageSize, int Page)
        {
            return JsonComments.GetComments(CommentType.Pingback, PageSize, Page);
        }

        [WebMethod]
        public static string LoadPager(int PageSize, int Page)
        {
            return JsonComments.GetPager(PageSize, Page, "Pingbacks.aspx");
        }
    }
}