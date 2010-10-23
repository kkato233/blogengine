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
    }

}