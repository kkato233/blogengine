#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BlogEngine.Core;

using Resources;

using Page = System.Web.UI.Page;

#endregion

/// <summary>
/// The admin_entry.
/// </summary>
public partial class admin_entry : Page, ICallbackEventHandler
{
    #region Constants and Fields

    /// <summary>
    /// The ra w_ edito r_ cookie.
    /// </summary>
    private const string RAW_EDITOR_COOKIE = "useraweditor";

    /// <summary>
    /// The _ callback.
    /// </summary>
    private string _Callback;

    #endregion

    #region Implemented Interfaces

    #region ICallbackEventHandler

    /// <summary>
    /// The get callback result.
    /// </summary>
    /// <returns>
    /// The get callback result.
    /// </returns>
    public string GetCallbackResult()
    {
        return this._Callback;
    }

    /// <summary>
    /// The raise callback event.
    /// </summary>
    /// <param name="eventArgument">
    /// The event argument.
    /// </param>
    public void RaiseCallbackEvent(string eventArgument)
    {
        if (eventArgument.StartsWith("_autosave"))
        {
            var fields = eventArgument.Replace("_autosave", string.Empty).Split(
                new[] { ";|;" }, StringSplitOptions.None);
            this.Session["content"] = fields[0];
            this.Session["title"] = fields[1];
            this.Session["description"] = fields[2];
            this.Session["slug"] = fields[3];
            this.Session["tags"] = fields[4];
        }
        else
        {
            this._Callback = Utils.RemoveIllegalCharacters(eventArgument.Trim());
        }
    }

    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// The page_ load.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.MaintainScrollPositionOnPostBack = true;
        this.txtTitle.Focus();
        this.BindTags();

        if (!this.Page.IsPostBack && !this.Page.IsCallback)
        {
            this.BindCategories();
            this.BindUsers();
            this.BindDrafts();

            this.Page.Title = labels.add_Entry;
            this.Page.ClientScript.GetCallbackEventReference(this, "title", "ApplyCallback", "slug");

            if (!String.IsNullOrEmpty(this.Request.QueryString["id"]) && this.Request.QueryString["id"].Length == 36)
            {
                var id = new Guid(this.Request.QueryString["id"]);
                this.Page.Title = labels.edit + " " + labels.post;
                this.BindPost(id);
            }
            else
            {
                this.PreSelectAuthor(this.Page.User.Identity.Name);
                this.txtDate.Text = DateTime.Now.AddHours(BlogSettings.Instance.Timezone).ToString("yyyy-MM-dd HH\\:mm");
                this.cbEnableComments.Checked = BlogSettings.Instance.IsCommentsEnabled;
                if (this.Session["content"] != null)
                {
                    this.txtContent.Text = this.Session["content"].ToString();
                    this.txtRawContent.Text = this.txtContent.Text;
                    this.txtTitle.Text = this.Session["title"].ToString();
                    this.txtDescription.Text = this.Session["description"].ToString();
                    this.txtSlug.Text = this.Session["slug"].ToString();
                    this.txtTags.Text = this.Session["tags"].ToString();
                }

                this.BindBookmarklet();
            }

            if (!this.Page.User.IsInRole(BlogSettings.Instance.AdministratorRole))
            {
                this.ddlAuthor.Enabled = false;
            }

            this.cbEnableComments.Enabled = BlogSettings.Instance.IsCommentsEnabled;

            if (this.Request.Cookies[RAW_EDITOR_COOKIE] != null)
            {
                this.txtRawContent.Visible = true;
                this.txtContent.Visible = false;
                this.cbUseRaw.Checked = true;
            }

            if (!Utils.IsMono && !this.cbUseRaw.Checked)
            {
                this.Page.Form.DefaultButton = this.btnSave.UniqueID;
            }
        }

        this.btnSave.Text = labels.savePost; // mono does not interpret the inline code correctly
        this.btnSave.Click += this.btnSave_Click;
        this.btnCategory.Click += this.btnCategory_Click;
        this.btnUploadFile.Click += this.btnUploadFile_Click;
        this.btnUploadImage.Click += this.btnUploadImage_Click;
        this.valExist.ServerValidate += this.valExist_ServerValidate;
        this.cbUseRaw.CheckedChanged += this.cbUseRaw_CheckedChanged;
    }

    /// <summary>
    /// The bind bookmarklet.
    /// </summary>
    private void BindBookmarklet()
    {
        if (this.Request.QueryString["title"] != null && this.Request.QueryString["url"] != null)
        {
            var title = this.Request.QueryString["title"];
            var url = this.Request.QueryString["url"];

            this.txtTitle.Text = title;
            this.txtContent.Text = string.Format("<p><a href=\"{0}\" title=\"{1}\">{1}</a></p>", url, title);
        }
    }

    /// <summary>
    /// The bind categories.
    /// </summary>
    private void BindCategories()
    {
        foreach (var cat in Category.Categories)
        {
            this.cblCategories.Items.Add(new ListItem(this.Server.HtmlEncode(cat.Title), cat.Id.ToString()));
        }
    }

    /// <summary>
    /// The bind drafts.
    /// </summary>
    private void BindDrafts()
    {
        var id = Guid.Empty;
        if (!String.IsNullOrEmpty(this.Request.QueryString["id"]) && this.Request.QueryString["id"].Length == 36)
        {
            id = new Guid(this.Request.QueryString["id"]);
        }

        var counter = 0;

        foreach (var post in Post.Posts)
        {
            if (!post.Published && post.Id != id)
            {
                var li = new HtmlGenericControl("li");
                var a = new HtmlAnchor();
                a.HRef = "?id=" + post.Id;
                a.InnerHtml = post.Title;

                var text =
                    new LiteralControl(
                        " by " + post.Author + " (" + post.DateCreated.ToString("yyyy-dd-MM HH\\:mm") + ")");

                li.Controls.Add(a);
                li.Controls.Add(text);
                this.ulDrafts.Controls.Add(li);
                counter++;
            }
        }

        if (counter > 0)
        {
            this.divDrafts.Visible = true;
            this.aDrafts.InnerHtml = string.Format(labels.thereAreXDrafts, counter);
        }
    }

    /// <summary>
    /// The bind post.
    /// </summary>
    /// <param name="postId">
    /// The post id.
    /// </param>
    private void BindPost(Guid postId)
    {
        var post = Post.GetPost(postId);

        // verifies if the current user is the author of the post and not and admin
        // it will redirect the user to the root of the blog.
        if (post.Author != Thread.CurrentPrincipal.Identity.Name &&
            !this.Page.User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            this.Response.Redirect(Utils.RelativeWebRoot);
        }

        this.txtTitle.Text = post.Title;
        this.txtContent.Text = post.Content;
        this.txtRawContent.Text = post.Content;
        this.txtDescription.Text = post.Description;
        this.txtDate.Text = post.DateCreated.ToString("yyyy-MM-dd HH\\:mm");
        this.cbEnableComments.Checked = post.HasCommentsEnabled;
        this.cbPublish.Checked = post.Published;
        this.txtSlug.Text = Utils.RemoveIllegalCharacters(post.Slug);

        this.PreSelectAuthor(post.Author);

        foreach (var cat in post.Categories)
        {
            var item = this.cblCategories.Items.FindByValue(cat.Id.ToString());
            if (item != null)
            {
                item.Selected = true;
            }
        }

        var tags = new string[post.Tags.Count];
        for (var i = 0; i < post.Tags.Count; i++)
        {
            tags[i] = post.Tags[i];
        }

        this.txtTags.Text = string.Join(",", tags);
    }

    /// <summary>
    /// The bind tags.
    /// </summary>
    private void BindTags()
    {
        var col = new List<string>();
        foreach (var post in Post.Posts)
        {
            foreach (var tag in post.Tags)
            {
                if (!col.Contains(tag))
                {
                    col.Add(tag);
                }
            }
        }

        col.Sort(delegate(string s1, string s2) { return String.Compare(s1, s2); });

        foreach (var tag in col)
        {
            var a = new HtmlAnchor();
            a.HRef = "javascript:void(0)";
            a.Attributes.Add("onclick", "AddTag(this)");
            a.InnerText = tag;
            this.phTags.Controls.Add(a);
        }
    }

    /// <summary>
    /// The bind users.
    /// </summary>
    private void BindUsers()
    {
        foreach (MembershipUser user in Membership.GetAllUsers())
        {
            this.ddlAuthor.Items.Add(user.UserName);
        }
    }

    /// <summary>
    /// The pre select author.
    /// </summary>
    /// <param name="author">
    /// The author.
    /// </param>
    private void PreSelectAuthor(string author)
    {
        this.ddlAuthor.ClearSelection();
        foreach (ListItem item in this.ddlAuthor.Items)
        {
            if (item.Text.Equals(author, StringComparison.OrdinalIgnoreCase))
            {
                item.Selected = true;
                break;
            }
        }
    }

    /// <summary>
    /// The size format.
    /// </summary>
    /// <param name="size">
    /// The size.
    /// </param>
    /// <param name="formatString">
    /// The format string.
    /// </param>
    /// <returns>
    /// The size format.
    /// </returns>
    private string SizeFormat(float size, string formatString)
    {
        if (size < 1024)
        {
            return size.ToString(formatString) + " bytes";
        }

        if (size < Math.Pow(1024, 2))
        {
            return (size / 1024).ToString(formatString) + " kb";
        }

        if (size < Math.Pow(1024, 3))
        {
            return (size / Math.Pow(1024, 2)).ToString(formatString) + " mb";
        }

        if (size < Math.Pow(1024, 4))
        {
            return (size / Math.Pow(1024, 3)).ToString(formatString) + " gb";
        }

        return size.ToString(formatString);
    }

    /// <summary>
    /// The upload.
    /// </summary>
    /// <param name="virtualFolder">
    /// The virtual folder.
    /// </param>
    /// <param name="control">
    /// The control.
    /// </param>
    /// <param name="fileName">
    /// The file name.
    /// </param>
    private void Upload(string virtualFolder, FileUpload control, string fileName)
    {
        var folder = this.Server.MapPath(virtualFolder);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        control.PostedFile.SaveAs(folder + fileName);
    }

    /// <summary>
    /// Creates and saves a new category
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void btnCategory_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid)
        {
            var cat = new Category(this.txtCategory.Text, string.Empty);
            cat.Save();
            var item = new ListItem(this.Server.HtmlEncode(this.txtCategory.Text), cat.Id.ToString());
            item.Selected = true;
            this.cblCategories.Items.Add(item);
        }
    }

    /// <summary>
    /// Saves the post
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.Page.IsValid)
        {
            throw new InvalidOperationException("One or more validators are invalid.");
        }

        Post post;
        if (this.Request.QueryString["id"] != null)
        {
            post = Post.GetPost(new Guid(this.Request.QueryString["id"]));
        }
        else
        {
            post = new Post();
        }

        if (this.cbUseRaw.Checked)
        {
            this.txtContent.Text = this.txtRawContent.Text;
        }

        if (string.IsNullOrEmpty(this.txtContent.Text))
        {
            this.txtContent.Text = "[No text]";
        }

        post.DateCreated =
            DateTime.ParseExact(this.txtDate.Text, "yyyy-MM-dd HH\\:mm", null).AddHours(-BlogSettings.Instance.Timezone);
        post.Author = this.ddlAuthor.SelectedValue;
        post.Title = this.txtTitle.Text.Trim();
        post.Content = this.txtContent.Text;
        post.Description = this.txtDescription.Text.Trim();
        post.Published = this.cbPublish.Checked;
        post.HasCommentsEnabled = this.cbEnableComments.Checked;

        if (!string.IsNullOrEmpty(this.txtSlug.Text))
        {
            post.Slug = Utils.RemoveIllegalCharacters(this.txtSlug.Text.Trim());
        }

        post.Categories.Clear();

        foreach (ListItem item in this.cblCategories.Items)
        {
            if (item.Selected)
            {
                post.Categories.Add(Category.GetCategory(new Guid(item.Value)));
            }
        }

        post.Tags.Clear();
        if (this.txtTags.Text.Trim().Length > 0)
        {
            var tags = this.txtTags.Text.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var tag in tags)
            {
                if (
                    string.IsNullOrEmpty(
                        post.Tags.Find(
                            delegate(string t) { return t.Equals(tag.Trim(), StringComparison.OrdinalIgnoreCase); })))
                {
                    post.Tags.Add(tag.Trim());
                }
            }
        }

        post.Save();

        this.Session.Remove("content");
        this.Session.Remove("title");
        this.Session.Remove("description");
        this.Session.Remove("slug");
        this.Session.Remove("tags");
        this.Response.Redirect(post.RelativeLink);
    }

    /// <summary>
    /// The btn upload file_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void btnUploadFile_Click(object sender, EventArgs e)
    {
        var relativeFolder = DateTime.Now.Year.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Month +
                             Path.DirectorySeparatorChar;
        var folder = BlogSettings.Instance.StorageLocation + "files" + Path.DirectorySeparatorChar;
        var fileName = this.txtUploadFile.FileName;
        this.Upload(folder + relativeFolder, this.txtUploadFile, fileName);

        var a = "<p><a href=\"{0}file.axd?file={1}\">{2}</a></p>";
        var text = this.txtUploadFile.FileName + " (" + this.SizeFormat(this.txtUploadFile.FileBytes.Length, "N") + ")";
        this.txtContent.Text += string.Format(
            a, Utils.RelativeWebRoot, this.Server.UrlEncode(relativeFolder.Replace("\\", "/") + fileName), text);
        this.txtRawContent.Text += string.Format(
            a, Utils.RelativeWebRoot, this.Server.UrlEncode(relativeFolder.Replace("\\", "/") + fileName), text);
    }

    /// <summary>
    /// The btn upload image_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void btnUploadImage_Click(object sender, EventArgs e)
    {
        var relativeFolder = DateTime.Now.Year.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Month +
                             Path.DirectorySeparatorChar;
        var folder = BlogSettings.Instance.StorageLocation + "files" + Path.DirectorySeparatorChar;
        var fileName = this.txtUploadImage.FileName;
        this.Upload(folder + relativeFolder, this.txtUploadImage, fileName);

        var path = Utils.RelativeWebRoot;
        var img = string.Format(
            "<img src=\"{0}image.axd?picture={1}\" alt=\"\" />", 
            path, 
            this.Server.UrlEncode(relativeFolder.Replace("\\", "/") + fileName));
        this.txtContent.Text += img;
        this.txtRawContent.Text += img;
    }

    /// <summary>
    /// The cb use raw_ checked changed.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void cbUseRaw_CheckedChanged(object sender, EventArgs e)
    {
        if (this.cbUseRaw.Checked)
        {
            this.txtRawContent.Text = this.txtContent.Text;
            var cookie = new HttpCookie(RAW_EDITOR_COOKIE, "1");
            cookie.Expires = DateTime.Now.AddYears(3);
            this.Response.Cookies.Add(cookie);
        }
        else
        {
            this.txtContent.Text = this.txtRawContent.Text;
            if (this.Request.Cookies[RAW_EDITOR_COOKIE] != null)
            {
                var cookie = new HttpCookie(RAW_EDITOR_COOKIE);
                cookie.Expires = DateTime.Now.AddYears(-3);
                this.Response.Cookies.Add(cookie);
            }
        }

        this.txtRawContent.Visible = this.cbUseRaw.Checked;
        this.txtContent.Visible = !this.cbUseRaw.Checked;

        // Response.Redirect(Request.RawUrl);
    }

    /// <summary>
    /// The val exist_ server validate.
    /// </summary>
    /// <param name="source">
    /// The source.
    /// </param>
    /// <param name="args">
    /// The args.
    /// </param>
    private void valExist_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;

        foreach (var cat in Category.Categories)
        {
            if (cat.Title.Equals(this.txtCategory.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                args.IsValid = false;
            }
        }
    }

    #endregion
}