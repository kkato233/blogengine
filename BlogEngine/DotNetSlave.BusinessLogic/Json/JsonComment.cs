namespace BlogEngine.Core.Json
{
    using System;
    using System.Globalization;
    using System.Web.Security;

    /// <summary>
    /// Wrapper for Json serializible comment list
    /// </summary>
    public class JsonComment
    {
        #region Constants and Fields

        /// <summary>
        /// Comment Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Author
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Avatar image
        /// </summary>
        public string Avatar
        {
            get
            {
                string GravatarImage = "<a href=\"mailto:{2}\" alt=\"{2}\" title=\"{2}\"><img class=\"gravatar\" src=\"{0}\" alt=\"{1}\" /></a>";

                if (BlogSettings.Instance.Avatar == "none")
                    return null;

                if (String.IsNullOrEmpty(Email) || !Email.Contains("@"))
                {
                    return "<img class=\"gravatar\" src=\"" + Utils.AbsoluteWebRoot + "themes/" + BlogSettings.Instance.Theme + "/noavatar.jpg\" alt=\"" + Author + "\" />";
                }

                string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(Email.ToLowerInvariant().Trim(), "MD5").ToLowerInvariant();
                string gravatar = "http://www.gravatar.com/avatar/" + hash + ".jpg?s=28&amp;d=";

                string link;
                switch (BlogSettings.Instance.Avatar)
                {
                    case "identicon":
                        link = gravatar + "identicon";
                        break;

                    case "wavatar":
                        link = gravatar + "wavatar";
                        break;

                    default:
                        link = gravatar + "monsterid";
                        break;
                }

                return string.Format(CultureInfo.InvariantCulture, GravatarImage, link, Author, Email);
            }
        }
        /// <summary>
        /// If comment has nested comments
        /// </summary>
        public bool HasChildred
        {
            get
            {
                return GetChildren(Id);
            }
        }
        /// <summary>
        /// Comment title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Shorten comment content
        /// </summary>
        public string Teaser { get; set; }
        /// <summary>
        /// Author's website
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Comment body
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// Date portion of published date
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Time portion of published date
        /// </summary>
        public string Time { get; set; }

        #endregion

        /// <summary>
        /// If connent has nested comments
        /// </summary>
        /// <param name="comId">Comment Id</param>
        /// <returns>True if has child records</returns>
        static bool GetChildren(Guid comId)
        {
            foreach (Post p in Post.Posts)
            {
                foreach (Comment c in p.Comments)
                {
                    if (c.ParentId == comId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
