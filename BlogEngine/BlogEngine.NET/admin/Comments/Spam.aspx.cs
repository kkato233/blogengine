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
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        /// <param name="page">
        /// The page number.
        /// </param>
        /// <returns>
        /// An enumerable of comments.
        /// </returns>
        [WebMethod]
        public static IEnumerable LoadComments(int pageSize, int page)
        {
            return JsonComments.GetComments(CommentType.Spam, pageSize, page);
        }

        /// <summary>
        /// Loads the pager.
        /// </summary>
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        /// <param name="page">
        /// The page number.
        /// </param>
        /// <returns>
        /// The pager.
        /// </returns>
        [WebMethod]
        public static string LoadPager(int pageSize, int page)
        {
            return JsonComments.GetPager(pageSize, page, "Spam.aspx");
        }
    }
}