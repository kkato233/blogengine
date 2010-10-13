namespace Admin.Comments
{
    using System.Collections;
    using System.Web.Services;
    using System.Web.UI;

    using BlogEngine.Core.Json;

    /// <summary>
    /// The spam settings.
    /// </summary>
    public partial class Spam : Page
    {
        /// <summary>
        /// Loads the comments.
        /// </summary>
        /// <param name=PpageSize">
        /// Size of the page.
        /// </param>
        /// <param name="Page">
        /// The page number.
        /// </param>
        /// <returns>
        /// An enumerable of comments.
        /// </returns>
        [WebMethod]
        public static IEnumerable LoadComments(int PageSize, int Page)
        {
            return JsonComments.GetComments(CommentType.Spam, PageSize, Page);
        }

        /// <summary>
        /// Loads the pager.
        /// </summary>
        /// <param name="PageSize">
        /// Size of the page.
        /// </param>
        /// <param name="Page">
        /// The page number.
        /// </param>
        /// <returns>
        /// The pager.
        /// </returns>
        [WebMethod]
        public static string LoadPager(int PageSize, int Page)
        {
            return JsonComments.GetPager(PageSize, Page, "Spam.aspx");
        }
    }
}