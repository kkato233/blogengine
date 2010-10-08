namespace admin.Comments
{
    using System.Collections;
    using System.Web.Services;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    public partial class Spam : System.Web.UI.Page
    {
        [WebMethod]
        public static IEnumerable LoadComments(int PageSize, int Page)
        {
            return JsonComments.GetComments(CommentType.Spam, PageSize, Page);
        }

        [WebMethod]
        public static string LoadPager(int PageSize, int Page)
        {
            return JsonComments.GetPager(PageSize, Page, "Spam.aspx");
        }
    }
}