namespace Admin.Pages
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using BlogEngine.Core;

    using Resources;

    using Page = System.Web.UI.Page;

    /// <summary>
    /// The AddEntry.
    /// </summary>
    public partial class AddEntry : Page, ICallbackEventHandler
    {
        #region Constants and Fields

        /// <summary>
        /// The raw editor cookie.
        /// </summary>
        private const string RawEditorCookie = "useraweditor";

        /// <summary>
        /// The callback.
        /// </summary>
        private string callback;

        #endregion

        #region Implemented Interfaces

        #region ICallbackEventHandler

        /// <summary>
        /// Returns the results of a callback event that targets a control.
        /// </summary>
        /// <returns>
        /// The result of the callback.
        /// </returns>
        public string GetCallbackResult()
        {
            return this.callback;
        }

        /// <summary>
        /// Processes a callback event that targets a control.
        /// </summary>
        /// <param name="eventArgument">
        /// A string that represents an event argument to pass to the event handler.
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
                this.callback = Utils.RemoveIllegalCharacters(eventArgument.Trim());
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.txtTitle.Focus();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            this.MaintainScrollPositionOnPostBack = true;

            this.BindTags();
            this.BindCategories();
            this.BindUsers();
            this.BindDrafts();

            this.Page.Title = labels.add_Entry;
            this.Page.ClientScript.GetCallbackEventReference(this, "title", "ApplyCallback", "slug");

            if (!String.IsNullOrEmpty(this.Request.QueryString["id"]) && this.Request.QueryString["id"].Length == 36)
            {
                var id = new Guid(this.Request.QueryString["id"]);
                this.Page.Title = string.Format("{0} {1}", labels.edit, labels.post);
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

            if (this.Request.Cookies[RawEditorCookie] != null)
            {
                this.txtRawContent.Visible = true;
                this.txtContent.Visible = false;
                this.cbUseRaw.Checked = true;
            }

            if (!Utils.IsMono && !this.cbUseRaw.Checked)
            {
                this.Page.Form.DefaultButton = this.btnSave.UniqueID;
            }

            this.btnSave.Click += this.BtnSaveClick;
            this.btnCategory.Click += this.BtnCategoryClick;
            this.btnUploadFile.Click += this.BtnUploadFileClick;
            this.btnUploadImage.Click += this.BtnUploadImageClick;
            this.valExist.ServerValidate += this.ValExistServerValidate;
            this.cbUseRaw.CheckedChanged += this.CbUseRawCheckedChanged;

            this.btnSave.Text = labels.savePost; // mono does not interpret the inline code correctly

            base.OnInit(e);
        }

        /// <summary>
        /// The bind bookmarklet.
        /// </summary>
        private void BindBookmarklet()
        {
            if (this.Request.QueryString["title"] == null || this.Request.QueryString["url"] == null)
            {
                return;
            }

            var title = this.Request.QueryString["title"];
            var url = this.Request.QueryString["url"];

            this.txtTitle.Text = title;
            this.txtContent.Text = string.Format("<p><a href=\"{0}\" title=\"{1}\">{1}</a></p>", url, title);
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
                if (post.Published || post.Id == id)
                {
                    continue;
                }

                var li = new HtmlGenericControl("li");
                var a = new HtmlAnchor { HRef = string.Format("?id={0}", post.Id), InnerHtml = post.Title };

                var text =
                    new LiteralControl(
                        string.Format(" by {0} ({1})", post.Author, post.DateCreated.ToString("yyyy-dd-MM HH\\:mm")));

                li.Controls.Add(a);
                li.Controls.Add(text);
                this.ulDrafts.Controls.Add(li);
                counter++;
            }

            if (counter <= 0)
            {
                return;
            }

            this.divDrafts.Visible = true;
            this.aDrafts.InnerHtml = string.Format(labels.thereAreXDrafts, counter);
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

            foreach (var item in
                post.Categories.Select(cat => this.cblCategories.Items.FindByValue(cat.Id.ToString())).Where(
                    item => item != null))
            {
                item.Selected = true;
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
            foreach (var tag in from post in Post.Posts from tag in post.Tags where !col.Contains(tag) select tag)
            {
                col.Add(tag);
            }

            col.Sort(String.Compare);

            foreach (var a in col.Select(tag => new HtmlAnchor { HRef = "javascript:void(0)", InnerText = tag }))
            {
                a.Attributes.Add("onclick", "AddTag(this)");
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
            foreach (ListItem item in
                this.ddlAuthor.Items.Cast<ListItem>().Where(item => item.Text.Equals(author, StringComparison.OrdinalIgnoreCase)))
            {
                item.Selected = true;
                break;
            }
        }

        /// <summary>
        /// Sizes the format.
        /// </summary>
        /// <param name="size">
        /// The string size.
        /// </param>
        /// <param name="formatString">
        /// The format string.
        /// </param>
        /// <returns>
        /// The string.
        /// </returns>
        private static string SizeFormat(float size, string formatString)
        {
            if (size < 1024)
            {
                return string.Format("{0} bytes", size.ToString(formatString));
            }

            if (size < Math.Pow(1024, 2))
            {
                return string.Format("{0} kb", (size / 1024).ToString(formatString));
            }

            if (size < Math.Pow(1024, 3))
            {
                return string.Format("{0} mb", (size / Math.Pow(1024, 2)).ToString(formatString));
            }

            if (size < Math.Pow(1024, 4))
            {
                return string.Format("{0} gb", (size / Math.Pow(1024, 3)).ToString(formatString));
            }

            return size.ToString(formatString);
        }

        /// <summary>
        /// Uploads the specified virtual folder.
        /// </summary>
        /// <param name="virtualFolder">The virtual folder.</param>
        /// <param name="control">The control.</param>
        /// <param name="fileName">Name of the file.</param>
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
        /// Handles the Click event of the btnCategory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnCategoryClick(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            var cat = new Category(this.txtCategory.Text, string.Empty);
            cat.Save();
            var item = new ListItem(this.Server.HtmlEncode(this.txtCategory.Text), cat.Id.ToString())
                {
                    Selected = true
                };
            this.cblCategories.Items.Add(item);
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnSaveClick(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                throw new InvalidOperationException("One or more validators are invalid.");
            }

            var post = this.Request.QueryString["id"] != null ? Post.GetPost(new Guid(this.Request.QueryString["id"])) : new Post();

            if (this.cbUseRaw.Checked)
            {
                this.txtContent.Text = this.txtRawContent.Text;
            }

            if (string.IsNullOrEmpty(this.txtContent.Text))
            {
                this.txtContent.Text = "[No text]";
            }

            post.DateCreated =
                DateTime.ParseExact(this.txtDate.Text, "yyyy-MM-dd HH\\:mm", null).AddHours(
                    -BlogSettings.Instance.Timezone);
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
                foreach (var tag in
                    tags.Where(tag => string.IsNullOrEmpty(post.Tags.Find(t => t.Equals(tag.Trim(), StringComparison.OrdinalIgnoreCase)))))
                {
                    post.Tags.Add(tag.Trim());
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
        /// Handles the Click event of the btnUploadFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnUploadFileClick(object sender, EventArgs e)
        {
            var relativeFolder = DateTime.Now.Year.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Month +
                                 Path.DirectorySeparatorChar;
            var folder = BlogSettings.Instance.StorageLocation + "files" + Path.DirectorySeparatorChar;
            var fileName = this.txtUploadFile.FileName;
            this.Upload(folder + relativeFolder, this.txtUploadFile, fileName);

            const string A = "<p><a href=\"{0}file.axd?file={1}\">{2}</a></p>";
            var text = string.Format("{0} ({1})", this.txtUploadFile.FileName, SizeFormat(this.txtUploadFile.FileBytes.Length, "N"));
            this.txtContent.Text += string.Format(
                A, Utils.RelativeWebRoot, this.Server.UrlEncode(relativeFolder.Replace("\\", "/") + fileName), text);
            this.txtRawContent.Text += string.Format(
                A, Utils.RelativeWebRoot, this.Server.UrlEncode(relativeFolder.Replace("\\", "/") + fileName), text);
        }

        /// <summary>
        /// Handles the Click event of the btnUploadImage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnUploadImageClick(object sender, EventArgs e)
        {
            var relativeFolder = DateTime.Now.Year.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Month +
                                 Path.DirectorySeparatorChar;
            var folder = string.Format("{0}files{1}", BlogSettings.Instance.StorageLocation, Path.DirectorySeparatorChar);
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
        /// Handles the CheckedChanged event of the cbUseRaw control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CbUseRawCheckedChanged(object sender, EventArgs e)
        {
            if (this.cbUseRaw.Checked)
            {
                this.txtRawContent.Text = this.txtContent.Text;
                var cookie = new HttpCookie(RawEditorCookie, "1") { Expires = DateTime.Now.AddYears(3) };
                this.Response.Cookies.Add(cookie);
            }
            else
            {
                this.txtContent.Text = this.txtRawContent.Text;
                if (this.Request.Cookies[RawEditorCookie] != null)
                {
                    var cookie = new HttpCookie(RawEditorCookie) { Expires = DateTime.Now.AddYears(-3) };
                    this.Response.Cookies.Add(cookie);
                }
            }

            this.txtRawContent.Visible = this.cbUseRaw.Checked;
            this.txtContent.Visible = !this.cbUseRaw.Checked;

            // Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Handles the ServerValidate event of the valExist control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        private void ValExistServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid =
                !Category.Categories.Any(
                    cat => cat.Title.Equals(this.txtCategory.Text.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}