namespace BlogEngine.Core
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Web;

    using BlogEngine.Core.Providers;

    /// <summary>
    /// Represents the configured settings for the blog engine.
    /// </summary>
    public class BlogSettings
    {
        #region Constants and Fields

        /// <summary>
        ///     The blog settings singleton.
        /// </summary>
        private static BlogSettings blogSettingsSingleton;

        /// <summary>
        ///     The version.
        /// </summary>
        private static string version;

        /// <summary>
        ///     The configured theme.
        /// </summary>
        private string configuredTheme = String.Empty;

        /// <summary>
        ///     The enable http compression.
        /// </summary>
        private bool enableHttpCompression;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref = "BlogSettings" /> class from being created. 
        ///     Initializes a new instance of the <see cref = "BlogSettings" /> class.
        /// </summary>
        private BlogSettings()
        {
            this.NumberOfRecentPosts = 10;

            this.Load();
        }

        #endregion

        #region Events

        /// <summary>
        ///     The changed.
        /// </summary>
        public static event EventHandler<EventArgs> Changed;

        #endregion

        #region Enums

        /// <summary>
        /// Type of comment moderation
        /// </summary>
        public enum Moderation
        {
            /// <summary>
            ///     Comments moderated manually
            /// </summary>
            Manual = 0, 

            /// <summary>
            ///     Comments moderated by filters
            /// </summary>
            Auto = 1, 

            /// <summary>
            ///     Moderated by Disqus
            /// </summary>
            Disqus = 2
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the singleton instance of the <see cref = "BlogSettings" /> class.
        /// </summary>
        /// <value>A singleton instance of the <see cref = "BlogSettings" /> class.</value>
        public static BlogSettings Instance
        {
            get
            {
                return blogSettingsSingleton ?? (blogSettingsSingleton = new BlogSettings());
            }
        }

        /// <summary>
        ///     Gets the role that has administrator persmissions
        /// </summary>
        public string AdministratorRole
        {
            get
            {
                return ConfigurationManager.AppSettings["BlogEngine.AdminRole"] ?? "administrators";
            }
        }

        /// <summary>
        ///     Gets or sets the FeedBurner user name.
        /// </summary>
        public string AlternateFeedUrl { get; set; }

        /// <summary>
        ///     Gets or sets the name of the author of this blog.
        /// </summary>
        /// <value>The name of the author of this blog.</value>
        public string AuthorName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating if Gravatars are enabled or not.
        /// </summary>
        /// <value><b>true</b> if Gravatars are enabled, otherwise returns <b>false</b>.</value>
        public string Avatar { get; set; }

        /// <summary>
        ///     Gets or sets the maximum number of characters that are displayed from a blog-roll retrieved post.
        /// </summary>
        /// <value>The maximum number of characters to display.</value>
        public int BlogrollMaxLength { get; set; }

        /// <summary>
        ///     Gets or sets the number of minutes to wait before polling blog-roll sources for changes.
        /// </summary>
        /// <value>The number of minutes to wait before polling blog-roll sources for changes.</value>
        public int BlogrollUpdateMinutes { get; set; }

        /// <summary>
        ///     Gets or sets the number of posts to display from a blog-roll source.
        /// </summary>
        /// <value>The number of posts to display from a blog-roll source.</value>
        public int BlogrollVisiblePosts { get; set; }

        /// <summary>
        ///     Gets or sets the number of comments approved to add user to white list
        /// </summary>
        public int CommentBlackListCount { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to enable reporting mistakes back to service
        /// </summary>
        public bool CommentReportMistakes { get; set; }

        /// <summary>
        ///     Gets or sets the number of comments approved to add user to white list
        /// </summary>
        public int CommentWhiteListCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of comments per page displayed in the comments asmin section
        /// </summary>
        public int CommentsPerPage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to compress WebResource.axd
        /// </summary>
        /// <value><c>true</c> if [compress web resource]; otherwise, <c>false</c>.</value>
        public bool CompressWebResource { get; set; }

        /// <summary>
        ///     Gets or sets the name of the author of this blog.
        /// </summary>
        /// <value>The name of the author of this blog.</value>
        public string ContactFormMessage { get; set; }

        /// <summary>
        ///     Gets or sets the name of the author of this blog.
        /// </summary>
        /// <value>The name of the author of this blog.</value>
        public string ContactThankMessage { get; set; }

        /// <summary>
        ///     Gets or sets the name of the author of this blog.
        /// </summary>
        /// <value>The name of the author of this blog.</value>
        public string Culture { get; set; }

        /// <summary>
        ///     Gets or sets the number of days that a post accepts comments.
        /// </summary>
        /// <value>The number of days that a post accepts comments.</value>
        /// <remarks>
        ///     After this time period has expired, comments on a post are disabled.
        /// </remarks>
        public int DaysCommentsAreEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the description of the blog.
        /// </summary>
        /// <value>A brief synopsis of the blog content.</value>
        /// <remarks>
        ///     This value is also used for the description meta tag.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating how many characters should be shown of the description
        /// </summary>
        public int DescriptionCharacters { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating how many characters should be shown of the description when posts are shown by tag or category.
        /// </summary>
        public int DescriptionCharactersForPostsByTagOrCategory { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether whitespace in stylesheets should be removed
        /// </summary>
        /// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
        public bool DisplayCommentsOnRecentPosts { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether whitespace in stylesheets should be removed
        /// </summary>
        /// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
        public bool DisplayRatingsOnRecentPosts { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to allow also to add comments to the pages
        /// </summary>
        public bool DisqusAddCommentsToPages { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether development mode to test disqus on local host
        /// </summary>
        public bool DisqusDevMode { get; set; }

        /// <summary>
        ///     Gets or sets the short website name that used to identify Disqus account
        /// </summary>
        public string DisqusWebsiteName { get; set; }

        /// <summary>
        ///     Gets or sets the e-mail address notifications are sent to.
        /// </summary>
        /// <value>The e-mail address notifications are sent to.</value>
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the email subject prefix.
        /// </summary>
        /// <value>The email subject prefix.</value>
        public string EmailSubjectPrefix { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether search of post comments is enabled.
        /// </summary>
        /// <value><b>true</b> if post comments can be searched, otherwise returns <b>false</b>.</value>
        public bool EnableCommentSearch { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether comments moderation is used for posts.
        /// </summary>
        /// <value><b>true</b> if comments are moderated for posts, otherwise returns <b>false</b>.</value>
        public bool EnableCommentsModeration { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to allow visitors to send attachments via the contact form.
        /// </summary>
        public bool EnableContactAttachments { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to dispay of the country of commenter is enabled.
        /// </summary>
        /// <value><b>true</b> if commenter country display is enabled, otherwise returns <b>false</b>.</value>
        public bool EnableCountryInComments { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether Enable enclosures for RSS feeds
        /// </summary>
        public bool EnableEnclosures { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether unhandled errors are logged.
        /// </summary>
        /// <value>
        ///     <c>true</c> if unhandled errors are to be logged otherwise, <c>false</c>.
        /// </value>
        public bool EnableErrorLogging { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether HTTP compression is enabled.
        /// </summary>
        /// <value><b>true</b> if compression is enabled, otherwise returns <b>false</b>.</value>
        public bool EnableHttpCompression
        {
            get
            {
                return this.enableHttpCompression && !Utils.IsMono;
            }

            set
            {
                this.enableHttpCompression = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether whitespace in stylesheets should be removed
        /// </summary>
        /// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
        public bool EnableOpenSearch { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [enable ping back receive].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [enable ping back receive]; otherwise, <c>false</c>.
        /// </value>
        public bool EnablePingBackReceive { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [enable ping back send].
        /// </summary>
        /// <value><c>true</c> if [enable ping back send]; otherwise, <c>false</c>.</value>
        public bool EnablePingBackSend { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether live preview of post is displayed.
        /// </summary>
        /// <value><b>true</b> if live previews are displayed, otherwise returns <b>false</b>.</value>
        public bool EnableRating { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether referral tracking is enabled.
        /// </summary>
        /// <value><b>true</b> if referral tracking is enabled, otherwise returns <b>false</b>.</value>
        public bool EnableReferrerTracking { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether related posts are displayed.
        /// </summary>
        /// <value><b>true</b> if related posts are displayed, otherwise returns <b>false</b>.</value>
        public bool EnableRelatedPosts { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to enable self registration.
        /// </summary>
        /// <value><c>true</c> if [enable self registration]; otherwise, <c>false</c>.</value>
        public bool EnableSelfRegistration { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether if SSL is enabled for sending e-mails
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [enable track back receive].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [enable track back receive]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableTrackBackReceive { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [enable track back send].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [enable track back send]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableTrackBackSend { get; set; }

        /// <summary>
        ///     Gets or sets the URI of a web log that the author of this web log is promoting.
        /// </summary>
        /// <value>The <see cref = "Uri" /> of a web log that the author of this web log is promoting.</value>
        public string Endorsement { get; set; }

        /// <summary>
        ///     Gets the file extension used for aspx pages
        /// </summary>
        /// <value>The file extension.</value>
        public string FileExtension
        {
            get
            {
                return ConfigurationManager.AppSettings["BlogEngine.FileExtension"] ?? ".aspx";
            }
        }

        /// <summary>
        ///     Gets or sets the latitude component of the geocoding position for this blog.
        /// </summary>
        /// <value>The latitude value.</value>
        public float GeocodingLatitude { get; set; }

        /// <summary>
        ///     Gets or sets the longitude component of the geocoding position for this blog.
        /// </summary>
        /// <value>The longitude value.</value>
        public float GeocodingLongitude { get; set; }

        /// <summary>
        ///     Gets or sets how to handle the www subdomain of the url (for SEO purposes).
        /// </summary>
        public string HandleWwwSubdomain { get; set; }

        /// <summary>
        ///     Gets or sets the name of the author of this blog.
        /// </summary>
        /// <value>The name of the author of this blog.</value>
        public string HtmlHeader { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether if CoComment support is enabled.
        /// </summary>
        /// <value><b>true</b> if CoComment support is enabled, otherwise returns <b>false</b>.</value>
        public bool IsCoCommentEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether if comments should be displayed as nested.
        /// </summary>
        /// <value><b>true</b> if comments should be displayed as nested, <b>false</b> for flat comments.</value>
        public bool IsCommentNestingEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether if comments are enabled for posts.
        /// </summary>
        /// <value><b>true</b> if comments can be made against a post, otherwise returns <b>false</b>.</value>
        public bool IsCommentsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the language this blog is written in.
        /// </summary>
        /// <value>The language this blog is written in.</value>
        /// <remarks>
        ///     Recommended best practice for the values of the Language element is defined by RFC 1766 [RFC1766] which includes a two-letter Language Code (taken from the ISO 639 standard [ISO639]), 
        ///     followed optionally, by a two-letter Country Code (taken from the ISO 3166 standard [ISO3166]).
        /// </remarks>
        /// <example>
        ///     en-US
        /// </example>
        public string Language { get; set; }

        /// <summary>
        ///     Gets or sets the mobile theme.
        /// </summary>
        /// <value>The mobile theme.</value>
        public string MobileTheme { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating type of moderation
        /// </summary>
        public Moderation ModerationType { get; set; }

        /// <summary>
        ///     Gets or sets the name of the blog.
        /// </summary>
        /// <value>The title of the blog.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the default number of recent posts to display.
        /// </summary>
        /// <value>The number of recent posts to display.</value>
        public int NumberOfRecentComments { get; set; }

        /// <summary>
        ///     Gets or sets the default number of recent posts to display.
        /// </summary>
        /// <value>The number of recent posts to display.</value>
        public int NumberOfRecentPosts { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating the number of days that referrer information should be stored.
        /// </summary>
        public int NumberOfReferrerDays { get; set; }

        /// <summary>
        ///     Gets or sets the maximum number of characters that are displayed from a blog-roll retrieved post.
        /// </summary>
        /// <value>The maximum number of characters to display.</value>
        public int PostsPerFeed { get; set; }

        /// <summary>
        ///     Gets or sets the number of posts to show an each page.
        /// </summary>
        /// <value>The number of posts to show an each page.</value>
        public int PostsPerPage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether if whitespace in stylesheets should be removed
        /// </summary>
        /// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
        public bool RemoveWhitespaceInStyleSheets { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to require user been logged in to post comment.
        /// </summary>
        /// <value><c>true</c> if [require login]; otherwise, <c>false</c>.</value>
        public bool RequireLoginToPostComment { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to require user been logged in to view posts.
        /// </summary>
        /// <value><c>true</c> if [require login]; otherwise, <c>false</c>.</value>
        public bool RequireLoginToViewPosts { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [require SSL for MetaWeblogAPI connections].
        /// </summary>
        public bool RequireSslMetaWeblogApi { get; set; }

        /// <summary>
        ///     Gets or sets the search button text to be displayed.
        /// </summary>
        /// <value>The search button text to be displayed.</value>
        public string SearchButtonText { get; set; }

        /// <summary>
        ///     Gets or sets the search comment label text to display.
        /// </summary>
        /// <value>The search comment label text to display.</value>
        public string SearchCommentLabelText { get; set; }

        /// <summary>
        ///     Gets or sets the default search text to display.
        /// </summary>
        /// <value>The default search text to display.</value>
        public string SearchDefaultText { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether an enail is sent when a comment is added to a post.
        /// </summary>
        /// <value><b>true</b> if email notification of new comments is enabled, otherwise returns <b>false</b>.</value>
        public bool SendMailOnComment { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the full post is displayed in lists or only the description/excerpt.
        /// </summary>
        public bool ShowDescriptionInPostList { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the full post is displayed in lists by tag/category or only the description/excerpt.
        /// </summary>
        public bool ShowDescriptionInPostListForPostsByTagOrCategory { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether live preview of post is displayed.
        /// </summary>
        /// <value><b>true</b> if live previews are displayed, otherwise returns <b>false</b>.</value>
        public bool ShowLivePreview { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether pingbacks and trackbacks shown in the comment list in admin panel
        /// </summary>
        public bool ShowPingBacks { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to show the post navigation.
        /// </summary>
        /// <value><c>true</c> if [show post navigation]; otherwise, <c>false</c>.</value>
        public bool ShowPostNavigation { get; set; }

        /// <summary>
        ///     Gets or sets the password used to connect to the SMTP server.
        /// </summary>
        /// <value>The password used to connect to the SMTP server.</value>
        public string SmtpPassword { get; set; }

        /// <summary>
        ///     Gets or sets the DNS name or IP address of the SMTP server used to send notification emails.
        /// </summary>
        /// <value>The DNS name or IP address of the SMTP server used to send notification emails.</value>
        public string SmtpServer { get; set; }

        /// <summary>
        ///     Gets or sets the DNS name or IP address of the SMTP server used to send notification emails.
        /// </summary>
        /// <value>The DNS name or IP address of the SMTP server used to send notification emails.</value>
        public int SmtpServerPort { get; set; }

        /// <summary>
        ///     Gets or sets the user name used to connect to the SMTP server.
        /// </summary>
        /// <value>The user name used to connect to the SMTP server.</value>
        public string SmtpUserName { get; set; }

        /// <summary>
        ///     Gets or sets the default storage location for blog data.
        /// </summary>
        /// <value>The default storage location for blog data.</value>
        public string StorageLocation { get; set; }

        /// <summary>
        ///     Gets or sets the default syndication format used by the blog.
        /// </summary>
        /// <value>The default syndication format used by the blog.</value>
        /// <remarks>
        ///     If no value is specified, blog defaults to using RSS 2.0 format.
        /// </remarks>
        /// <seealso cref = "BlogEngine.Core.SyndicationFormat" />
        public string SyndicationFormat { get; set; }

        /// <summary>
        ///     Gets or sets the current theme applied to the blog.
        ///     Default theme can be overridden in the query string
        ///     or cookie to let users select different theme
        /// </summary>
        /// <value>The configured theme for the blog.</value>
        public string Theme
        {
            get
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    var request = context.Request;
                    if (request.QueryString["theme"] != null)
                    {
                        return request.QueryString["theme"];
                    }

                    if (request.Cookies["theme"] != null)
                    {
                        var cookies = request.Cookies["theme"];
                        if (cookies != null)
                        {
                            return cookies.Value;
                        }
                    }
                }

                return Utils.IsMobile && !string.IsNullOrEmpty(this.MobileTheme)
                           ? this.MobileTheme
                           : this.configuredTheme;
            }

            set
            {
                this.configuredTheme = String.IsNullOrEmpty(value) ? String.Empty : value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to time stamp post links.
        /// </summary>
        public bool TimeStampPostLinks { get; set; }

        /// <summary>
        ///     Gets or sets the maximum number of characters that are displayed from a blog-roll retrieved post.
        /// </summary>
        /// <value>The maximum number of characters to display.</value>
        public double Timezone { get; set; }

        /// <summary>
        ///     Gets or sets the tracking script used to collect visitor data.
        /// </summary>
        public string TrackingScript { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether if true comments from authenticated users always approved
        /// </summary>
        public bool TrustAuthenticatedUsers { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether if whitespace in stylesheets should be removed
        /// </summary>
        /// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
        public bool UseBlogNameInPageTitles { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves the settings to disk.
        /// </summary>
        public void Save()
        {
            var dic = new StringDictionary();
            var settingsType = this.GetType();

            // ------------------------------------------------------------
            // Enumerate through settings properties
            // ------------------------------------------------------------
            foreach (var propertyInformation in settingsType.GetProperties())
            {
                if (propertyInformation.Name == "Instance")
                {
                    continue;
                }

                // ------------------------------------------------------------
                // Extract property value and its string representation
                // ------------------------------------------------------------
                var propertyValue = propertyInformation.GetValue(this, null);

                string valueAsString;

                // ------------------------------------------------------------
                // Format null/default property values as empty strings
                // ------------------------------------------------------------
                if (propertyValue == null || propertyValue.Equals(Int32.MinValue) ||
                    propertyValue.Equals(Single.MinValue))
                {
                    valueAsString = String.Empty;
                }
                else
                {
                    valueAsString = propertyValue.ToString();
                }

                // ------------------------------------------------------------
                // Write property name/value pair
                // ------------------------------------------------------------
                dic.Add(propertyInformation.Name, valueAsString);
            }

            BlogService.SaveSettings(dic);
            OnChanged();
        }

        /// <summary>
        /// Returns the BlogEngine.NET version information.
        /// </summary>
        /// <value>
        /// The BlogEngine.NET major, minor, revision, and build numbers.
        /// </value>
        /// <remarks>
        /// The current version is determined by extracting the build version of the BlogEngine.Core assembly.
        /// </remarks>
        /// <returns>
        /// The version.
        /// </returns>
        public string Version()
        {
            return version ?? (version = this.GetType().Assembly.GetName().Version.ToString());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when the settings have been changed.
        /// </summary>
        private static void OnChanged()
        {
            // ------------------------------------------------------------
            // Attempt to raise the IsChanged event
            // Execute event handler
            // ------------------------------------------------------------
            if (Changed != null)
            {
                Changed(null, new EventArgs());
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the <see cref="BlogSettings"/> class.
        /// </summary>
        private void Load()
        {
            var settingsType = this.GetType();

            // ------------------------------------------------------------
            // Enumerate through individual settings nodes
            // ------------------------------------------------------------
            var dic = BlogService.LoadSettings();

            foreach (string key in dic.Keys)
            {
                // ------------------------------------------------------------
                // Extract the setting's name/value pair
                // ------------------------------------------------------------
                var name = key;
                var value = dic[key];

                // ------------------------------------------------------------
                // Enumerate through public properties of this instance
                // Determine if configured setting matches current setting based on name
                // ------------------------------------------------------------
                foreach (var propertyInformation in
                    settingsType.GetProperties().Where(
                        propertyInformation => propertyInformation.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    )
                {
                    // ------------------------------------------------------------
                    // Attempt to apply configured setting
                    // ------------------------------------------------------------
                    try
                    {
                        if (propertyInformation.CanWrite)
                        {
                            if (propertyInformation.PropertyType.IsEnum)
                            {
                                propertyInformation.SetValue(
                                    this, Enum.Parse(propertyInformation.PropertyType, value), null);
                            }
                            else
                            {
                                var thevalue = Convert.ChangeType(
                                    value, propertyInformation.PropertyType, CultureInfo.CurrentCulture);

                                propertyInformation.SetValue(this, thevalue, null);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Utils.Log(string.Format("Error loading blog settings: {0}", e.Message));
                    }

                    break;
                }
            }

            this.StorageLocation = BlogService.GetStorageLocation();
        }

        #endregion
    }
}