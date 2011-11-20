using WatiN.Core;

namespace BlogEngine.Tests.PageTemplates.Admin
{
    public class PostList : Page
    {
        public string Url
        {
            get { return Constants.Root + "/admin/Posts/Posts.aspx"; }
        }
    }
}
