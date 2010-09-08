#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;

#endregion

/// <summary>
/// The user_controls_ comment view.
/// </summary>
public partial class UserControlsCommentView : UserControl, ICallbackEventHandler
{
    #region Constants and Fields

    /// <summary>
    ///     The any captcha enabled.
    /// </summary>
    protected bool AnyCaptchaEnabled;

    /// <summary>
    ///     The any captcha necessary.
    /// </summary>
    protected bool AnyCaptchaNecessary;

    /// <summary>
    ///     The comment counter.
    /// </summary>
    protected int CommentCounter;

    /// <summary>
    ///     The default name.
    /// </summary>
    protected string DefaultName = string.Empty;

    /// <summary>
    ///     The name input id.
    /// </summary>
    protected string NameInputId = string.Empty;

    /// <summary>
    ///     The re captcha enabled.
    /// </summary>
    protected bool ReCaptchaEnabled;

    /// <summary>
    ///     The simple captcha enabled.
    /// </summary>
    protected bool SimpleCaptchaEnabled;

    /// <summary>
    ///     The callback.
    /// </summary>
    private string callback;

    /// <summary>
    ///     The nesting supported.
    /// </summary>
    private bool? nestingSupported;

    #endregion

    #region Properties

    /// <summary>
    ///     Gets true if on comment save will show "awaiting moderation" message
    ///     otherwise "comment saved, thank you.." will be displayd.
    /// </summary>
    public string ManualModeration
    {
        get
        {
            if (BlogSettings.Instance.EnableCommentsModeration && BlogSettings.Instance.ModerationType == 0)
            {
                return "true";
            }

            return "false";
        }
    }

    /// <summary>
    ///     Gets a value indicating whether NestingSupported.
    /// </summary>
    public bool NestingSupported
    {
        get
        {
            if (!this.nestingSupported.HasValue)
            {
                if (!BlogSettings.Instance.IsCommentNestingEnabled)
                {
                    this.nestingSupported = false;
                }
                else
                {
                    var path = Utils.RelativeWebRoot + "themes/" + BlogSettings.Instance.Theme + "/CommentView.ascx";

                    // test comment control for nesting placeholder (for backwards compatibility with older themes)
                    var commentTester = (CommentViewBase)this.LoadControl(path);
                    var subComments = commentTester.FindControl("phSubComments") as PlaceHolder;
                    this.nestingSupported = subComments != null;
                }
            }

            return this.nestingSupported.Value;
        }
    }

    /// <summary>
    ///     Gets or sets the post from which the comments are parsed.
    /// </summary>
    public Post Post { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Resolves the region based on the browser language.
    /// </summary>
    /// <returns>
    /// The region info.
    /// </returns>
    public static RegionInfo ResolveRegion()
    {
        var languages = HttpContext.Current.Request.UserLanguages;

        if (languages == null || languages.Length == 0)
        {
            return new RegionInfo(CultureInfo.CurrentCulture.LCID);
        }

        try
        {
            var language = languages[0].ToLowerInvariant().Trim();
            var culture = CultureInfo.CreateSpecificCulture(language);
            return new RegionInfo(culture.LCID);
        }
        catch (ArgumentException)
        {
            try
            {
                return new RegionInfo(CultureInfo.CurrentCulture.LCID);
            }
            catch (ArgumentException)
            {
                // the googlebot sometimes gives a culture LCID of 127 which is invalid
                // so assume US english if invalid LCID
                return new RegionInfo(1033);
            }
        }
    }

    /// <summary>
    /// Binds the country dropdown list with countries retrieved
    ///     from the .NET Framework.
    /// </summary>
    public void BindCountries()
    {
        var dic = new StringDictionary();
        var col = new List<string>();

        foreach (var ri in CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(ci => new RegionInfo(ci.Name)))
        {
            if (!dic.ContainsKey(ri.EnglishName))
            {
                dic.Add(ri.EnglishName, ri.TwoLetterISORegionName.ToLowerInvariant());
            }

            if (!col.Contains(ri.EnglishName))
            {
                col.Add(ri.EnglishName);
            }
        }

        // Add custom cultures
        if (!dic.ContainsValue("bd"))
        {
            dic.Add("Bangladesh", "bd");
            col.Add("Bangladesh");
        }

        col.Sort();

        this.ddlCountry.Items.Add(new ListItem("[Not specified]", string.Empty));
        foreach (var key in col)
        {
            this.ddlCountry.Items.Add(new ListItem(key, dic[key]));
        }

        if (this.ddlCountry.SelectedIndex == 0)
        {
            this.ddlCountry.SelectedValue = ResolveRegion().TwoLetterISORegionName.ToLowerInvariant();
            this.SetFlagImageUrl();
        }
    }

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
        if (!BlogSettings.Instance.IsCommentsEnabled)
        {
            return;
        }

        var args = eventArgument.Split(new[] { "-|-" }, StringSplitOptions.None);
        var author = args[0];
        var email = args[1];
        var website = args[2];
        var country = args[3];
        var content = args[4];
        var notify = bool.Parse(args[5]);
        var preview = bool.Parse(args[6]);
        var sentCaptcha = args[7];

        // If there is no "reply to" comment, args[8] is empty
        var replyToCommentId = String.IsNullOrEmpty(args[8]) ? Guid.Empty : new Guid(args[8]);
        var avatar = args[9];

        var recaptchaResponse = args[10];
        var recaptchaChallenge = args[11];

        var simpleCaptchaChallenge = args[12];

        this.recaptcha.UserUniqueIdentifier = this.hfCaptcha.Value;
        if (!preview && this.AnyCaptchaEnabled && this.AnyCaptchaNecessary)
        {
            if (this.ReCaptchaEnabled)
            {
                if (!this.recaptcha.ValidateAsync(recaptchaResponse, recaptchaChallenge))
                {
                    this.callback = "RecaptchaIncorrect";
                    return;
                }
            }
            else
            {
                this.simplecaptcha.Validate(simpleCaptchaChallenge);
                if (!this.simplecaptcha.IsValid)
                {
                    this.callback = "SimpleCaptchaIncorrect";
                    return;
                }
            }
        }

        var storedCaptcha = this.hfCaptcha.Value;

        if (sentCaptcha != storedCaptcha)
        {
            return;
        }

        var comment = new Comment
            {
                Id = Guid.NewGuid(), 
                ParentId = replyToCommentId, 
                Author = this.Server.HtmlEncode(author), 
                Email = email, 
                Content = this.Server.HtmlEncode(content), 
                IP = this.Request.UserHostAddress, 
                Country = country, 
                DateCreated = DateTime.Now, 
                Parent = this.Post, 
                IsApproved = !BlogSettings.Instance.EnableCommentsModeration, 
                Avatar = avatar.Trim()
            };

        if (this.Page.User.Identity.IsAuthenticated)
        {
            comment.IsApproved = true;
        }

        if (website.Trim().Length > 0)
        {
            if (!website.ToLowerInvariant().Contains("://"))
            {
                website = string.Format("http://{0}", website);
            }

            Uri url;
            if (Uri.TryCreate(website, UriKind.Absolute, out url))
            {
                comment.Website = url;
            }
        }

        if (!preview)
        {
            if (notify && !this.Post.NotificationEmails.Contains(email))
            {
                this.Post.NotificationEmails.Add(email);
            }
            else if (!notify && this.Post.NotificationEmails.Contains(email))
            {
                this.Post.NotificationEmails.Remove(email);
            }

            this.Post.AddComment(comment);
            this.SetCookie(author, email, website, country);
            if (this.ReCaptchaEnabled)
            {
                this.recaptcha.UpdateLog(comment);
            }
        }

        var path = string.Format("{0}themes/{1}/CommentView.ascx", Utils.RelativeWebRoot, BlogSettings.Instance.Theme);

        var control = (CommentViewBase)this.LoadControl(path);
        control.Comment = comment;
        control.Post = this.Post;

        using (var sw = new StringWriter())
        {
            control.RenderControl(new HtmlTextWriter(sw));
            this.callback = sw.ToString();
        }
    }

    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// Displays a delete link to visitors that is authenticated
    ///     using the default membership provider.
    /// </summary>
    /// <param name="id">
    /// The id of the comment.
    /// </param>
    /// <returns>
    /// The admin link.
    /// </returns>
    protected string AdminLink(string id)
    {
        if (this.Page.User.Identity.IsAuthenticated)
        {
            var sb = new StringBuilder();
            foreach (var comment in this.Post.Comments.Where(comment => comment.Id.ToString() == id))
            {
                sb.AppendFormat(" | <a href=\"mailto:{0}\">{0}</a>", comment.Email);
            }

            const string ConfirmDelete = "Are you sure you want to delete the comment?";
            sb.AppendFormat(
                " | <a href=\"?deletecomment={0}\" onclick=\"return confirm('{1}?')\">{2}</a>", 
                id, 
                ConfirmDelete, 
                "Delete");
            return sb.ToString();
        }

        return string.Empty;
    }

    /// <summary>
    /// Displays BBCodes dynamically loaded from settings.
    /// </summary>
    /// <returns>
    /// The bb codes.
    /// </returns>
    protected string BBCodes()
    {
        try
        {
            var sb = new StringBuilder();

            var settings = ExtensionManager.GetSettings("BBCode");
            if (settings != null)
            {
                var table = settings.GetDataTable();

                foreach (DataRow row in table.Rows)
                {
                    var code = (string)row["Code"];
                    var title = string.Format("[{0}][/{1}]", code, code);
                    sb.AppendFormat(
                        "<a title=\"{0}\" href=\"javascript:void(BlogEngine.addBbCode('{1}'))\">{2}</a>", 
                        title, 
                        code, 
                        code);
                }
            }

            return sb.ToString();
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Displays the Gravatar image that matches the specified email.
    /// </summary>
    /// <param name="email">
    /// The email address.
    /// </param>
    /// <param name="name">
    /// The name/title.
    /// </param>
    /// <param name="size">
    /// The size int.
    /// </param>
    /// <returns>
    /// The gravatar.
    /// </returns>
    protected string Gravatar(string email, string name, int size)
    {
        if (email.Contains("://"))
        {
            return
                string.Format(
                    "<img class=\"thumb\" src=\"http://images.websnapr.com/?url={0}&amp;size=t\" alt=\"{1}\" />", 
                    name, 
                    email);
        }

        // http://www.artviper.net/screenshots/screener.php?&url={0}&h={1}&w={1}
        MD5 md5 = new MD5CryptoServiceProvider();
        var result = md5.ComputeHash(Encoding.ASCII.GetBytes(email));

        var hash = new StringBuilder();
        for (var i = 0; i < result.Length; i++)
        {
            hash.Append(result[i].ToString("x2"));
        }

        var image = new StringBuilder();
        image.Append("<img src=\"");
        image.Append("http://www.gravatar.com/avatar.php?");
        image.Append("gravatar_id=" + hash);
        image.Append("&amp;rating=G");
        image.Append("&amp;size=" + size);
        image.Append("&amp;default=");
        image.Append(
            this.Server.UrlEncode(Utils.AbsoluteWebRoot + "themes/" + BlogSettings.Instance.Theme + "/noavatar.jpg"));
        image.Append("\" alt=\"\" />");
        return image.ToString();
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">
    /// The <see cref="T:System.EventArgs"/> object that contains the event data.
    /// </param>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        this.NameInputId = string.Format("txtName{0}", DateTime.Now.Ticks);

        this.EnableCaptchas();

        if (this.Post == null)
        {
            this.Response.Redirect(Utils.RelativeWebRoot);
            return;
        }

        if (!this.Page.IsPostBack && !this.Page.IsCallback)
        {
            if (this.Page.User.Identity.IsAuthenticated)
            {
                if (this.Request.QueryString["deletecomment"] != null)
                {
                    this.DeleteComment();
                }

                if (this.Request.QueryString["deletecommentandchildren"] != null)
                {
                    this.DeleteCommentAndChildren();
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["approvecomment"]))
                {
                    this.ApproveComment();
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["approveallcomments"]))
                {
                    this.ApproveAllComments();
                }
            }

            var path = string.Format(
                "{0}themes/{1}/CommentView.ascx", Utils.RelativeWebRoot, BlogSettings.Instance.Theme);

            if (this.NestingSupported)
            {
                // newer, nested comments
                if (this.Post != null)
                {
                    this.AddNestedComments(path, this.Post.NestedComments, this.phComments);
                }
            }
            else
            {
                // old, non nested code
                // Add approved Comments
                if (this.Post != null)
                {
                    foreach (var comment in
                        this.Post.Comments.Where(comment => comment.Email != "pingback" && comment.Email != "trackback")
                        )
                    {
                        if (comment.IsApproved)
                        {
                            this.CommentCounter++;
                        }

                        if (!comment.IsApproved && BlogSettings.Instance.EnableCommentsModeration)
                        {
                            continue;
                        }

                        var control = (CommentViewBase)this.LoadControl(path);
                        control.Comment = comment;
                        control.Post = this.Post;
                        this.phComments.Controls.Add(control);
                    }
                }

                // Add unapproved comments
                if (this.Page.User.Identity.IsAuthenticated)
                {
                    if (this.Post != null)
                    {
                        foreach (var comment in this.Post.Comments)
                        {
                            if (comment.Email == "pingback" || comment.Email == "trackback")
                            {
                                continue;
                            }

                            if (comment.IsApproved)
                            {
                                continue;
                            }

                            var control = (CommentViewBase)this.LoadControl(path);
                            control.Comment = comment;
                            control.Post = this.Post;
                            this.phComments.Controls.Add(control);
                        }
                    }
                }
            }

            var pingbacks = new List<CommentViewBase>();

            if (this.Post != null)
            {
                foreach (var comment in this.Post.Comments)
                {
                    var control = (CommentViewBase)this.LoadControl(path);

                    if (comment.Email != "pingback" && comment.Email != "trackback")
                    {
                        continue;
                    }

                    control.Comment = comment;
                    control.Post = this.Post;
                    pingbacks.Add(control);
                }
            }

            if (pingbacks.Count > 0)
            {
                var litTrackback = new Literal();
                var sb = new StringBuilder();
                sb.AppendFormat("<h3 id=\"trackbackheader\">Pingbacks and trackbacks ({0})", pingbacks.Count);
                sb.Append(
                    "<a id=\"trackbacktoggle\" style=\"float:right;width:20px;height:20px;border:1px solid #ccc;text-decoration:none;text-align:center\"");
                sb.Append(" href=\"javascript:toggle_visibility('trackbacks','trackbacktoggle');\">+</a>");
                sb.Append("</h3><div id=\"trackbacks\" style=\"display:none\">");
                litTrackback.Text = sb.ToString();
                this.phTrckbacks.Controls.Add(litTrackback);

                foreach (var c in pingbacks)
                {
                    this.phTrckbacks.Controls.Add(c);
                }

                var closingDiv = new Literal { Text = @"</div>" };
                this.phTrckbacks.Controls.Add(closingDiv);
            }
            else
            {
                this.phTrckbacks.Visible = false;
            }

            if (BlogSettings.Instance.IsCommentsEnabled)
            {
                if (this.Post != null &&
                    (!this.Post.HasCommentsEnabled ||
                     (BlogSettings.Instance.DaysCommentsAreEnabled > 0 &&
                      this.Post.DateCreated.AddDays(BlogSettings.Instance.DaysCommentsAreEnabled) < DateTime.Now.Date)))
                {
                    this.phAddComment.Visible = false;
                    this.lbCommentsDisabled.Visible = true;
                }

                this.BindCountries();
                this.GetCookie();
                this.recaptcha.UserUniqueIdentifier = this.hfCaptcha.Value = Guid.NewGuid().ToString();
            }
            else
            {
                this.phAddComment.Visible = false;
            }

            // InititializeCaptcha();
        }

        this.Page.ClientScript.GetCallbackEventReference(this, "arg", null, string.Empty);
    }

    /// <summary>
    /// Adds the nested comments.
    /// </summary>
    /// <param name="path">
    /// The path string.
    /// </param>
    /// <param name="nestedComments">
    /// The nested comments.
    /// </param>
    /// <param name="commentsPlaceHolder">
    /// The comments place holder.
    /// </param>
    private void AddNestedComments(string path, IEnumerable<Comment> nestedComments, Control commentsPlaceHolder)
    {
        foreach (var comment in nestedComments)
        {
            var control = (CommentViewBase)this.LoadControl(path);
            if ((!comment.IsApproved && BlogSettings.Instance.EnableCommentsModeration) &&
                (comment.IsApproved || !this.Page.User.Identity.IsAuthenticated))
            {
                continue;
            }

            control.Comment = comment;
            control.Post = this.Post;

            if (comment.IsApproved)
            {
                this.CommentCounter++;
            }

            if (comment.Comments.Count > 0)
            {
                // find the next placeholder and add the subcomments to it
                var subCommentsPlaceHolder = control.FindControl("phSubComments") as PlaceHolder;
                if (subCommentsPlaceHolder != null)
                {
                    this.AddNestedComments(path, comment.Comments, subCommentsPlaceHolder);
                }
            }

            commentsPlaceHolder.Controls.Add(control);
        }
    }

    /// <summary>
    /// The approve all comments.
    /// </summary>
    private void ApproveAllComments()
    {
        this.Post.ApproveAllComments();

        var index = this.Request.RawUrl.IndexOf("?");
        var url = this.Request.RawUrl.Substring(0, index);
        this.Response.Redirect(url, true);
    }

    /// <summary>
    /// The approve comment.
    /// </summary>
    private void ApproveComment()
    {
        foreach (var comment in this.Post.NotApprovedComments)
        {
            if (comment.Id == new Guid(this.Request.QueryString["approvecomment"]))
            {
                this.Post.ApproveComment(comment);

                var index = this.Request.RawUrl.IndexOf("?");
                var url = this.Request.RawUrl.Substring(0, index);
                this.Response.Redirect(url, true);
            }
        }
    }

    /// <summary>
    /// The collect comment to delete.
    /// </summary>
    /// <param name="comment">
    /// The comment.
    /// </param>
    /// <param name="commentsToDelete">
    /// The comments to delete.
    /// </param>
    private void CollectCommentToDelete(Comment comment, List<Comment> commentsToDelete)
    {
        commentsToDelete.Add(comment);

        // recursive collection
        foreach (var subComment in comment.Comments)
        {
            this.CollectCommentToDelete(subComment, commentsToDelete);
        }
    }

    /// <summary>
    /// The delete comment.
    /// </summary>
    private void DeleteComment()
    {
        foreach (var comment in this.Post.Comments)
        {
            if (comment.Id == new Guid(this.Request.QueryString["deletecomment"]))
            {
                this.Post.RemoveComment(comment);

                var index = this.Request.RawUrl.IndexOf("?");
                var url = this.Request.RawUrl.Substring(0, index) + "#comment";
                this.Response.Redirect(url, true);
            }
        }
    }

    /// <summary>
    /// The delete comment and children.
    /// </summary>
    private void DeleteCommentAndChildren()
    {
        foreach (var comment in this.Post.Comments)
        {
            if (comment.Id == new Guid(this.Request.QueryString["deletecommentandchildren"]))
            {
                // collect comments to delete first so the Nesting isn't lost
                var commentsToDelete = new List<Comment>();

                this.CollectCommentToDelete(comment, commentsToDelete);

                foreach (var commentToDelete in commentsToDelete)
                {
                    this.Post.RemoveComment(commentToDelete);
                }

                var index = this.Request.RawUrl.IndexOf("?");
                var url = this.Request.RawUrl.Substring(0, index) + "#comment";
                this.Response.Redirect(url, true);
            }
        }
    }

    /// <summary>
    /// The enable captchas.
    /// </summary>
    private void EnableCaptchas()
    {
        this.ReCaptchaEnabled = ExtensionManager.ExtensionEnabled("Recaptcha");
        this.SimpleCaptchaEnabled = ExtensionManager.ExtensionEnabled("SimpleCaptcha");
        if (this.ReCaptchaEnabled && this.SimpleCaptchaEnabled)
        {
            var simpleCaptchaExtension = ExtensionManager.GetExtension("SimpleCaptcha");
            var recaptchaExtension = ExtensionManager.GetExtension("Recaptcha");
            if (simpleCaptchaExtension.Priority < recaptchaExtension.Priority)
            {
                this.EnableRecaptcha();
            }
            else
            {
                this.EnableSimpleCaptcha();
            }
        }
        else if (this.ReCaptchaEnabled)
        {
            this.EnableRecaptcha();
        }
        else if (this.SimpleCaptchaEnabled)
        {
            this.EnableSimpleCaptcha();
        }
    }

    /// <summary>
    /// The enable recaptcha.
    /// </summary>
    private void EnableRecaptcha()
    {
        this.AnyCaptchaEnabled = true;
        this.AnyCaptchaNecessary = this.recaptcha.RecaptchaNecessary;
        this.recaptcha.Visible = true;
        this.simplecaptcha.Visible = false;
        this.SimpleCaptchaEnabled = false;
    }

    /// <summary>
    /// The enable simple captcha.
    /// </summary>
    private void EnableSimpleCaptcha()
    {
        this.AnyCaptchaEnabled = true;
        this.AnyCaptchaNecessary = this.simplecaptcha.SimpleCaptchaNecessary;
        this.simplecaptcha.Visible = true;
        this.recaptcha.Visible = false;
        this.ReCaptchaEnabled = false;
    }

    /// <summary>
    /// Gets the cookie with visitor information if any is set.
    ///     Then fills the contact information fields in the form.
    /// </summary>
    private void GetCookie()
    {
        var cookie = this.Request.Cookies["comment"];
        try
        {
            if (cookie != null)
            {
                this.DefaultName = this.Server.UrlDecode(cookie.Values["name"]);
                this.txtEmail.Text = cookie.Values["email"];
                this.txtWebsite.Text = cookie.Values["url"];
                this.ddlCountry.SelectedValue = cookie.Values["country"];
                this.SetFlagImageUrl();
            }
            else if (this.Page.User.Identity.IsAuthenticated)
            {
                var user = Membership.GetUser();
                if (user != null)
                {
                    this.DefaultName = user.UserName;
                    this.txtEmail.Text = user.Email;
                }

                this.txtWebsite.Text = this.Request.Url.Host;
            }
        }
        catch (Exception)
        {
            // Couldn't retrieve info on the visitor/user
        }
    }

    /// <summary>
    /// Sets a cookie with the entered visitor information
    ///     so it can be prefilled on next visit.
    /// </summary>
    /// <param name="name">
    /// The name.
    /// </param>
    /// <param name="email">
    /// The email.
    /// </param>
    /// <param name="website">
    /// The website.
    /// </param>
    /// <param name="country">
    /// The country.
    /// </param>
    private void SetCookie(string name, string email, string website, string country)
    {
        var cookie = new HttpCookie("comment");
        cookie.Expires = DateTime.Now.AddMonths(24);
        cookie.Values.Add("name", this.Server.UrlEncode(name.Trim()));
        cookie.Values.Add("email", email.Trim());
        cookie.Values.Add("url", website.Trim());
        cookie.Values.Add("country", country);
        this.Response.Cookies.Add(cookie);
    }

    /// <summary>
    /// The set flag image url.
    /// </summary>
    private void SetFlagImageUrl()
    {
        if (!string.IsNullOrEmpty(this.ddlCountry.SelectedValue))
        {
            this.imgFlag.ImageUrl = Utils.RelativeWebRoot + "pics/flags/" + this.ddlCountry.SelectedValue + ".png";
        }
        else
        {
            this.imgFlag.ImageUrl = Utils.RelativeWebRoot + "pics/pixel.png";
        }
    }

    #endregion
}