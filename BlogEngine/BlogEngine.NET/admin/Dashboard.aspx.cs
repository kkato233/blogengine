using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.Security;
using BlogEngine.Core;

public partial class admin_Dashboard : System.Web.UI.Page
{
    public static int PostsPublished { get; set; }
    public static int PagesCount { get; set; }
    public static int CommentsAll { get; set; }
    public static int CategoriesCount { get; set; }
    public static int TagsCount { get; set; }
    public static int UsersCount { get; set; }

    private StringCollection CategoryList = new StringCollection();
    private StringCollection TagList = new StringCollection();

    protected void Page_Load(object sender, EventArgs e)
    {
        var postsLinq = from posts in BlogEngine.Core.Post.Posts where posts.IsPublished = true select posts.Id;
        PostsPublished = postsLinq.Count();

        PagesCount = BlogEngine.Core.Page.Pages.Count();

        CommentsAll = 0;
        foreach (var p in Post.Posts)
        {
            CommentsAll += p.Comments.Count;

            foreach (var c in p.Categories)
            {
                if (!CategoryList.Contains(c.Id.ToString()))
                    CategoryList.Add(c.Id.ToString());
            }
            CategoriesCount = CategoryList.Count;

            foreach (var t in p.Tags)
            {
                if (!TagList.Contains(t))
                    TagList.Add(t);
            }
            TagsCount = TagList.Count;

            int uCount = 0;
            var userCollection = Membership.Provider.GetAllUsers(0, 999, out uCount);
            UsersCount = uCount;
        }
    }
}