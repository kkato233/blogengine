namespace Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using BlogEngine.Core;
    using Resources;

    using Page = System.Web.UI.Page;
    using System.Web.UI.HtmlControls;

    public partial class CreateBlog : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CreateUserButton_Click(object sender, EventArgs e)
        {
            string blogName = this.UserName.Text.Trim().ToLower();

            string msg = Create(blogName);

            if (string.IsNullOrEmpty(msg))
            {
                this.Response.Redirect(Utils.ApplicationRelativeWebRoot + blogName);
            }
            else
            {
                this.Master.SetStatus("warning", msg);
            }
        }

        string Create(string blogName)
        {
            Guid existingId = Guid.Empty;
            string message = string.Empty;
            Blog blog = null;
            Blog template = Blog.Blogs.Where(b => b.Name == "Template").FirstOrDefault();

            if (template == null)
                template = Blog.Blogs[0];

            var copyFromExistingBlogId = template.Id.ToString();

            if (!Blog.ValidateProperties(blog == null, blog, blogName, "", true, blogName.ToLower(), "~/" + blogName.ToLower(), false, out message))
            {
                if (string.IsNullOrWhiteSpace(message)) { message = "Validation for new blog failed."; }
                return message;
            }

            if (blog == null)
            {
                blog = Blog.CreateNewBlog(copyFromExistingBlogId, blogName, "", true, blogName.ToLower(), "~/" + blogName.ToLower(), true, false, out message);

                if (blog == null || !string.IsNullOrWhiteSpace(message))
                {
                    return message ?? "Failed to create the new blog.";
                }
            }
            return message;
        }
    }
}