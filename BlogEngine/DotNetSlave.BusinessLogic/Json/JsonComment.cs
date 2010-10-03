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
                BlogEngine.Core.Avatar avatar = BlogEngine.Core.Avatar.GetAvatar(28, Email, null, null, Author);
                
                if (avatar.HasNoImage || string.IsNullOrEmpty(Email) || !Email.Contains("@"))
                {
                    // <img> tag pointing to noavatar.jpg, or no image if Avatar setting is "none".
                    return avatar.ImageTag ?? string.Empty;
                }

                string linkWithImage = "<a href=\"mailto:{2}\" alt=\"{0}\" title=\"{0}\">{1}</a>";
                return string.Format(CultureInfo.InvariantCulture, linkWithImage, Author, avatar.ImageTag, Email);
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
        /// Static avatar image
        /// </summary>
        public string AuthorAvatar { get; set; }
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
        /// If comment has nested comments
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
