namespace BlogEngine.Core.Json
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Security;

    /// <summary>
    /// Wrapper for Json serializible comment list
    /// </summary>
    public class JsonComment
    {
        #region Constants and Fields

        /// <summary>
        ///     Gets or sets the Comment Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the Author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///     Gets the avatar image
        /// </summary>
        public string Avatar
        {
            get
            {
                const string GravatarImage = "<a href=\"mailto:{2}\" alt=\"{2}\" title=\"{2}\"><img class=\"gravatar\" src=\"{0}\" alt=\"{1}\" /></a>";

                if (BlogSettings.Instance.Avatar == "none")
                {
                    return null;
                }

                if (String.IsNullOrEmpty(this.Email) || !this.Email.Contains("@"))
                {
                    return string.Format("<img class=\"gravatar\" src=\"{0}themes/{1}/noavatar.jpg\" alt=\"{2}\" />", Utils.AbsoluteWebRoot, BlogSettings.Instance.Theme, this.Author);
                }

                var hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(
                    this.Email.ToLowerInvariant().Trim(), "MD5");
                if (hashedPassword != null)
                {
                    var hash = hashedPassword.ToLowerInvariant();
                    var gravatar = string.Format("http://www.gravatar.com/avatar/{0}.jpg?s=28&amp;d=", hash);

                    string link;
                    switch (BlogSettings.Instance.Avatar)
                    {
                        case "identicon":
                            link = string.Format("{0}identicon", gravatar);
                            break;

                        case "wavatar":
                            link = string.Format("{0}wavatar", gravatar);
                            break;

                        default:
                            link = string.Format("{0}monsterid", gravatar);
                            break;
                    }

                    return string.Format(CultureInfo.InvariantCulture, GravatarImage, link, this.Author, this.Email);
                }

                return null;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this comment has nested comments
        /// </summary>
        public bool HasChildred
        {
            get
            {
                return GetChildren(this.Id);
            }
        }

        /// <summary>
        ///     Gets or sets the comment title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the shorten comment content
        /// </summary>
        public string Teaser { get; set; }

        /// <summary>
        ///     Gets or sets the author's website
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        ///     Gets or sets the comment body
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     Gets or sets the ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        ///     Gets or sets the date portion of published date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        ///     Gets or sets the time portion of published date
        /// </summary>
        public string Time { get; set; }

        #endregion

        /// <summary>
        /// If connent has nested comments
        /// </summary>
        /// <param name="comId">
        /// Comment Id
        /// </param>
        /// <returns>
        /// True if has child records
        /// </returns>
        private static bool GetChildren(Guid comId)
        {
            return Post.Posts.SelectMany(p => p.Comments).Any(c => c.ParentId == comId);
        }
    }
}