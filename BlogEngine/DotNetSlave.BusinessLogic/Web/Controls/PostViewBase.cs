namespace BlogEngine.Core.Web.Controls
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The PostView.ascx that is located in the themes folder
    ///     has to inherit from this class.
    ///     <remarks>
    /// It provides the basic functionaly needed to display a post.
    ///     </remarks>
    /// </summary>
    public class PostViewBase : UserControl
    {
        #region Constants and Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="PostViewBase"/> class.
        /// </summary>
        public PostViewBase()
        {
            this.Location = ServingLocation.None;
            this.ContentBy = ServingContentBy.Unspecified;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the body of the post. Important: use this instead of Post.Content.
        /// </summary>
        public string Body
        {
            get
            {
                var body = this.Post.Content;

                if (this.ShowExcerpt)
                {
                    var link = string.Format(" <a href=\"{0}\">[{1}]</a>", this.Post.RelativeLink, Utils.Translate("more"));

                    if (!string.IsNullOrEmpty(this.Post.Description))
                    {
                        body = this.Post.Description.Replace(Environment.NewLine, "<br />") + link;
                    }
                    else
                    {
                        body = Utils.StripHtml(this.Post.Content);
                        if (body.Length > this.DescriptionCharacters)
                        {
                            body = string.Format("{0}...{1}", body.Substring(0, this.DescriptionCharacters), link);
                        }
                    }
                }

                var arg = new ServingEventArgs(body, this.Location, this.ContentBy);
                Post.OnServing(this.Post, arg);

                if (arg.Cancel)
                {
                    if (arg.Location == ServingLocation.SinglePost)
                    {
                        this.Response.Redirect("~/error404.aspx", true);
                    }
                    else
                    {
                        this.Visible = false;
                    }
                }

                return arg.Body ?? string.Empty;
            }
        }

        /// <summary>
        ///     Gets the comment feed link.
        /// </summary>
        /// <value>The comment feed.</value>
        public string CommentFeed
        {
            get
            {
                return this.Post.RelativeLink.Replace("/post/", "/post/feed/");
            }
        }

        /// <summary>
        ///     Gets or sets the criteria by which the content is being served (by tag, category, author, etc).
        /// </summary>
        public ServingContentBy ContentBy { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating how many characters should be shown of the description.
        /// </summary>
        public int DescriptionCharacters { get; set; }

        /// <summary>
        ///     Gets or sets the index of the post in a list of posts displayed
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Gets or sets the location where the serving takes place.
        /// </summary>
        public ServingLocation Location { get; set; }

        /// <summary>
        ///     Gets or sets the Post object that is displayed through the PostView.ascx control.
        /// </summary>
        /// <value>The Post object that has to be displayed.</value>
        public virtual Post Post
        {
            get
            {
                return (Post)this.ViewState["Post"];
            }

            set
            {
                this.ViewState["Post"] = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to show the entire post or just the excerpt/description.
        /// </summary>
        public bool ShowExcerpt { get; set; }

        /// <summary>
        ///     Gets an Edit and Delete link to any authenticated user.
        /// </summary>
        protected virtual string AdminLinks
        {
            get
            {
                if (this.Page.User.IsInRole(BlogSettings.Instance.AdministratorRole) ||
                    this.Page.User.Identity.Name.Equals(this.Post.Author, StringComparison.OrdinalIgnoreCase))
                {
                    var confirmDelete = string.Format(
                        CultureInfo.InvariantCulture,
                        Utils.Translate("areYouSure"),
                        Utils.Translate("delete").ToLowerInvariant(),
                        Utils.Translate("thePost"));
                    var sb = new StringBuilder();

                    if (this.Post.NotApprovedComments.Count > 0 &&
                        BlogSettings.Instance.ModerationType != BlogSettings.Moderation.Disqus)
                    {
                        sb.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "<a href=\"{0}\">{1} ({2})</a> | ",
                            this.Post.RelativeLink,
                            Utils.Translate("unapprovedcomments"),
                            this.Post.NotApprovedComments.Count);
                        sb.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "<a href=\"{0}\">{1}</a> | ",
                            this.Post.RelativeLink + "?approveallcomments=true",
                            Utils.Translate("approveallcomments"));
                    }

                    sb.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "<a href=\"{0}\">{1}</a> | ",
                        Utils.AbsoluteWebRoot + "admin/Posts/Add_entry.aspx?id=" + this.Post.Id,
                        Utils.Translate("edit"));
                    sb.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "<a href=\"javascript:void(0);\" onclick=\"if (confirm('{2}')) location.href='{0}?deletepost={1}'\">{3}</a> | ",
                        this.Post.RelativeLink,
                        this.Post.Id,
                        confirmDelete,
                        Utils.Translate("delete"));
                    return sb.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///     Gets the rating.
        /// Enable visitors to rate the post.
        /// </summary>
        protected virtual string Rating
        {
            get
            {
                if (!BlogSettings.Instance.EnableRating)
                {
                    return string.Empty;
                }

                // string script = "<div id=\"rating_{0}\"></div><script type=\"text/javascript\">BlogEngine.showRating('{0}',{1},{2});</script>";
                const string Script = "<div class=\"ratingcontainer\" style=\"visibility:hidden\">{0}|{1}|{2}</div>";
                return string.Format(
                    Script,
                    this.Post.Id,
                    this.Post.Raters,
                    this.Post.Rating.ToString("#.0", CultureInfo.InvariantCulture));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Displays the Post's categories seperated by the specified string.
        /// </summary>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The category links.
        /// </returns>
        protected virtual string CategoryLinks(string separator)
        {
            var keywords = new string[this.Post.Categories.Count];
            const string Link = "<a href=\"{0}\">{1}</a>";
            for (var i = 0; i < this.Post.Categories.Count; i++)
            {
                var c = Category.GetCategory(this.Post.Categories[i].Id);
                if (c != null)
                {
                    keywords[i] = string.Format(CultureInfo.InvariantCulture, Link, c.RelativeLink, c.Title);
                }
            }

            return string.Join(separator, keywords);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.Post.IsVisible && !this.Page.User.Identity.IsAuthenticated)
            {
                this.Visible = false;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        /// <remarks>
        /// Lets process our .Body content and build up our controls collection
        /// inside the 'BodyContent' placeholder.
        /// User controls are insterted into the blog in the following format..
        /// [UserControl:~/path/usercontrol.ascx]
        /// TODO : Expose user control parameters.
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

         
            var bodyContent = (PlaceHolder)this.FindControl("BodyContent");
            if (bodyContent == null)
            {
                // We have no placeholder so we assume this is an old style <% =Body %> theme and do nothing.
            }
            else
            {
                Utils.InjectUserControls(bodyContent, this.Body);
            }
        }

        /// <summary>
        /// Displays the Post's tags seperated by the specified string.
        /// </summary>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The tag links.
        /// </returns>
        protected virtual string TagLinks(string separator)
        {
            var tags = this.Post.Tags;
            if (tags.Count == 0)
            {
                return null;
            }

            var tagStrings = new string[tags.Count];
            const string Link = "<a href=\"{0}/{1}\" rel=\"tag\">{2}</a>";
            var path = Utils.RelativeWebRoot + "?tag=";
            for (var i = 0; i < tags.Count; i++)
            {
                var tag = tags[i];
                tagStrings[i] = string.Format(
                    CultureInfo.InvariantCulture, Link, path, HttpUtility.UrlEncode(tag), HttpUtility.HtmlEncode(tag));
            }

            return string.Join(separator, tagStrings);
        }

        #endregion
    }
}