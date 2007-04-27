/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/01/2007  mads        Created BlogSettings Class
04/27/2007	brian.kuhn	Refactored code, grouped properties, added comments.
04/27/2007  brian.kuhn  Added syndication extension specific properties.
****************************************************************************/
using System;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Xml;

/// <summary>
/// Represents the configured settings for the blog engine.
/// </summary>
public class BlogSettings
{
    //============================================================
    //	PUBLIC/PRIVATE/PROTECTED MEMBERS
    //============================================================
    #region PRIVATE/PROTECTED/PUBLIC MEMBERS
    /// <summary>
    /// Public event used to indicate in settings have been changed.
    /// </summary>
    public static event EventHandler<EventArgs> Changed;
    /// <summary>
    /// Private member to hold singleton instance.
    /// </summary>
    private static BlogSettings blogSettingsSingleton;
    /// <summary>
    /// Private member to hold the title of the blog.
    /// </summary>
    private string blogName;
    /// <summary>
    /// Private member to hold a brief synopsis of the blog.
    /// </summary>
    private string blogDescription;
    /// <summary>
    /// Private member to hold the default number of posts per day to display.
    /// </summary>
    private int postsPerPage;
    /// <summary>
    /// Private member to hold blog storage location.
    /// </summary>
    private string storageLocation;
    /// <summary>
    /// Private member to hold maximum number of characters that are displayed from a blog-roll retrieved post.
    /// </summary>
    private int blogrollMaxLength;
    /// <summary>
    /// Private member to hold the number of minutes blog-roll entries are updated.
    /// </summary>
    private int blogrollUpdateMinutes;
    /// <summary>
    /// Private member to hold the maximum number of blog-roll posts per blog-roll to display.
    /// </summary>
    private int blogrollVisiblePosts;
    /// <summary>
    /// Private member to hold the name of the configured theme.
    /// </summary>
    private string configuredTheme;
    /// <summary>
    /// Private member to hold a value indicating if related posts is enabled.
    /// </summary>
    private bool enableRelatedPosts;
    /// <summary>
    /// Private member to hold the email address notifications are sent to.
    /// </summary>
    private string emailAddress;
    /// <summary>
    /// Private member to hold the SMTP server to contact when sending email.
    /// </summary>
    private string smtpServer;
    /// <summary>
    /// Private member to hold the username used when contacting the SMTP server.
    /// </summary>
    private string smtpUsername;
    /// <summary>
    /// Private member to hold the password used when contacting the SMTP server.
    /// </summary>
    private string smtpPassword;
    /// <summary>
    /// Private member to hold a value indicating if mail is sent when a new comment is posted.
    /// </summary>
    private bool sendMailOnComment;
    /// <summary>
    /// Private member to hold a value indicating if post comments are enabled.
    /// </summary>
    private bool areCommentsEnabled;
    /// <summary>
    /// Private member to hold a value indicating if display of country of commenter is enabled.
    /// </summary>
    private bool enableCountryInComments;
    /// <summary>
    /// Private member to hold a value indicating live preview of posts is enabled.
    /// </summary>
    private bool showLivePreview;
    /// <summary>
    /// Private member to hold a value indicating if CoComment is enabled.
    /// </summary>
    private bool isCoCommentEnabled;
    /// <summary>
    /// Private member to hold number of days before post comments are closed.
    /// </summary>
    private int daysCommentsAreEnabled;
    /// <summary>
    /// Private member to hold default number of recent posts to display.
    /// </summary>
    private int numberOfRecentPosts;
    /// <summary>
    /// Private member to hold the search button text.
    /// </summary>
    private string searchButtonText;
    /// <summary>
    /// Private member to hold the default search text.
    /// </summary>
    private string searchDefaultText;
    /// <summary>
    /// Private member to hold a value indicating 
    /// </summary>
    private bool enableCommentSearch;
    /// <summary>
    /// Private member to hold the search comment label text.
    /// </summary>
    private string searchCommentLabelText;
    /// <summary>
    /// Private member to hold a value indicating if referral tracking is enabled.
    /// </summary>
    private bool enableReferrerTracking;
    /// <summary>
    /// Private member to hold a value indicating if HTTP compression is enabled.
    /// </summary>
    private bool enableHttpCompression;
    /// <summary>
    /// Private member to hold the URI of a web log that the author of this web log is promoting.
    /// </summary>
    private string blogChannelBLink;
    /// <summary>
    /// Private member to hold the name of the author of the blog.
    /// </summary>
    private string dublinCoreCreator;
    /// <summary>
    /// Private member to hold the language the blog is written in.
    /// </summary>
    private string dublinCoreLanguage;
    /// <summary>
    /// Private member to hold the latitude component of the geocoding position for this blog.
    /// </summary>
    private float geocodingLatitude;
    /// <summary>
    /// Private member to hold the longitude component of the geocoding position for this blog.
    /// </summary>
    private float geocodingLongitude;
    #endregion

    //============================================================
    //	CONSTRUCTORS
    //============================================================
    #region BlogSettings()
    /// <summary>
    /// Initializes a new instance of the <see cref="BlogSettings"/> class.
    /// </summary>
    private BlogSettings()
    {
        //------------------------------------------------------------
        //	Attempt to initialize class state
        //------------------------------------------------------------
        try
        {
            //------------------------------------------------------------
            //	Load the currently configured settings
            //------------------------------------------------------------
            Load();
        }
        catch
        {
            //------------------------------------------------------------
            //	Rethrow exception
            //------------------------------------------------------------
            throw;
        }
    }
    #endregion

    //============================================================
    //	STATIC PROPERTIES
    //============================================================
    #region Instance
    /// <summary>
    /// Gets the singleton instance of the <see cref="BlogSettings"/> class.
    /// </summary>
    /// <value>A singleton instance of the <see cref="BlogSettings"/> class.</value>
    /// <remarks></remarks>
    public static BlogSettings Instance
    {
        get
        {
            if (blogSettingsSingleton == null)
            {
                blogSettingsSingleton = new BlogSettings();
            }
            return blogSettingsSingleton;
        }
    }
    #endregion

    //============================================================
    //	GENERAL SETTINGS
    //============================================================
    #region Description
    /// <summary>
    /// Gets or sets the description of the blog.
    /// </summary>
    /// <value>A brief synopsis of the blog content.</value>
    /// <remarks>This value is also used for the description meta tag.</remarks>
    public string Description
    {
        get
        {
            return blogDescription; 
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                blogDescription = String.Empty;
            }
            else
            {
                blogDescription = value;
            }
        }
    }
    #endregion

    #region EnableHttpCompression
    /// <summary>
    /// Gets or sets a value indicating if HTTP compression is enabled.
    /// </summary>
    /// <value><b>true</b> if compression is enabled, otherwise returns <b>false</b>.</value>
    public bool EnableHttpCompression
    {
        get
        {
            return enableHttpCompression;
        }

        set
        {
            enableHttpCompression = value;
        }
    }
    #endregion

    #region EnableReferrerTracking
    /// <summary>
    /// Gets or sets a value indicating if referral tracking is enabled.
    /// </summary>
    /// <value><b>true</b> if referral tracking is enabled, otherwise returns <b>false</b>.</value>
    public bool EnableReferrerTracking
    {
        get
        {
            return enableReferrerTracking;
        }

        set
        {
            enableReferrerTracking = value;
        }
    }
    #endregion

    #region EnableRelatedPosts
    /// <summary>
    /// Gets or sets a value indicating if related posts are displayed.
    /// </summary>
    /// <value><b>true</b> if related posts are displayed, otherwise returns <b>false</b>.</value>
    public bool EnableRelatedPosts
    {
        get
        {
            return enableRelatedPosts;
        }

        set
        {
            enableRelatedPosts = value;
        }
    }
    #endregion

    #region Name
    /// <summary>
    /// Gets or sets the name of the blog.
    /// </summary>
    /// <value>The title of the blog.</value>
    public string Name
    {
        get
        {
            return blogName;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                blogName = String.Empty;
            }
            else
            {
                blogName = value;
            }
        }
    }
    #endregion

    #region NumberOfRecentPosts
    /// <summary>
    /// Gets or sets the default number of recent posts to display.
    /// </summary>
    /// <value>The number of recent posts to display.</value>
    public int NumberOfRecentPosts
    {
        get
        {
            return numberOfRecentPosts;
        }

        set
        {
            numberOfRecentPosts = value;
        }
    }
    #endregion

    #region PostsPerPage
    /// <summary>
    /// Gets or sets the number of posts to show an each page.
    /// </summary>
    /// <value>The number of posts to show an each page.</value>
    public int PostsPerPage
    {
        get
        {
            return postsPerPage;
        }

        set
        {
            postsPerPage = value;
        }
    }
    #endregion

    #region ShowLivePreview
    /// <summary>
    /// Gets or sets a value indicating if live preview of post is displayed.
    /// </summary>
    /// <value><b>true</b> if live previews are displayed, otherwise returns <b>false</b>.</value>
    public bool ShowLivePreview
    {
        get
        {
            return showLivePreview;
        }

        set
        {
            showLivePreview = value;
        }
    }
    #endregion

    #region StorageLocation
    /// <summary>
    /// Gets or sets the default storage location for blog data.
    /// </summary>
    /// <value>The default storage location for blog data.</value>
    public string StorageLocation
    {
        get
        {
            return storageLocation;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                storageLocation = String.Empty;
            }
            else
            {
                storageLocation = value;
            }
        }
    }
    #endregion

    #region Theme
    /// <summary>
    /// Gets or sets the current theme applied to the blog.
    /// </summary>
    /// <value>The configured theme for the blog.</value>
    public string Theme
    {
        get
        {
            return configuredTheme;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                configuredTheme = String.Empty;
            }
            else
            {
                configuredTheme = value;
            }
        }
    }
    #endregion

    //============================================================
    //	EMAIL SETTINGS
    //============================================================
    #region Email
    /// <summary>
    /// Gets or sets the e-mail address notifications are sent to.
    /// </summary>
    /// <value>The e-mail address notifications are sent to.</value>
    public string Email
    {
        get
        {
            return emailAddress;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                emailAddress = String.Empty;
            }
            else
            {
                emailAddress = value;
            }
        }
    }
    #endregion

    #region SendMailOnComment
    /// <summary>
    /// Gets or sets a value indicating if an enail is sent when a comment is added to a post.
    /// </summary>
    /// <value><b>true</b> if email notification of new comments is enabled, otherwise returns <b>false</b>.</value>
    public bool SendMailOnComment
    {
        get
        {
            return sendMailOnComment;
        }

        set
        {
            sendMailOnComment = value;
        }
    }
    #endregion

    #region SmtpPassword
    /// <summary>
    /// Gets or sets the password used to connect to the SMTP server.
    /// </summary>
    /// <value>The password used to connect to the SMTP server.</value>
    public string SmtpPassword
    {
        get
        {
            return smtpPassword;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                smtpPassword = String.Empty;
            }
            else
            {
                smtpPassword = value;
            }
        }
    }
    #endregion

    #region SmtpServer
    /// <summary>
    /// Gets or sets the DNS name or IP address of the SMTP server used to send notification emails.
    /// </summary>
    /// <value>The DNS name or IP address of the SMTP server used to send notification emails.</value>
    public string SmtpServer
    {
        get
        {
            return smtpServer;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                smtpServer = String.Empty;
            }
            else
            {
                smtpServer = value;
            }
        }
    }
    #endregion

    #region SmtpUsername
    /// <summary>
    /// Gets or sets the user name used to connect to the SMTP server.
    /// </summary>
    /// <value>The user name used to connect to the SMTP server.</value>
    public string SmtpUsername
    {
        get
        {
            return smtpUsername;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                smtpUsername = String.Empty;
            }
            else
            {
                smtpUsername = value;
            }
        }
    }
    #endregion

    //============================================================
    //	COMMENT SETTINGS
    //============================================================
    #region DaysCommentsAreEnabled
    /// <summary>
    /// Gets or sets the number of days that a post accepts comments.
    /// </summary>
    /// <value>The number of days that a post accepts comments.</value>
    /// <remarks>After this time period has expired, comments on a post are disabled.</remarks>
    public int DaysCommentsAreEnabled
    {
        get
        {
            return daysCommentsAreEnabled;
        }

        set
        {
            daysCommentsAreEnabled = value;
        }
    }
    #endregion

    #region EnableCountryInComments
    /// <summary>
    /// Gets or sets a value indicating if dispay of the country of commenter is enabled.
    /// </summary>
    /// <value><b>true</b> if commenter country display is enabled, otherwise returns <b>false</b>.</value>
    public bool EnableCountryInComments
    {
        get
        {
            return enableCountryInComments;
        }

        set
        {
            enableCountryInComments = value;
        }
    }
    #endregion

    #region IsCoCommentEnabled
    /// <summary>
    /// Gets or sets a value indicating if CoComment support is enabled.
    /// </summary>
    /// <value><b>true</b> if CoComment support is enabled, otherwise returns <b>false</b>.</value>
    public bool IsCoCommentEnabled
    {
        get
        {
            return isCoCommentEnabled;
        }

        set
        {
            isCoCommentEnabled = value;
        }
    }
    #endregion

    #region IsCommentsEnabled
    /// <summary>
    /// Gets or sets a value indicating if comments are enabled for posts.
    /// </summary>
    /// <value><b>true</b> if comments can be made against a post, otherwise returns <b>false</b>.</value>
    public bool IsCommentsEnabled
    {
        get
        {
            return areCommentsEnabled;
        }

        set
        {
            areCommentsEnabled = value;
        }
    }
    #endregion

    //============================================================
    //	BLOG ROLL SETTINGS
    //============================================================
    #region BlogrollMaxLength
    /// <summary>
    /// Gets or sets the maximum number of characters that are displayed from a blog-roll retrieved post.
    /// </summary>
    /// <value>The maximum number of characters to display.</value>
    public int BlogrollMaxLength
    {
        get
        {
            return blogrollMaxLength;
        }

        set
        {
            blogrollMaxLength = value;
        }
    }
    #endregion

    #region BlogrollUpdateMinutes
    /// <summary>
    /// Gets or sets the number of minutes to wait before polling blog-roll sources for changes.
    /// </summary>
    /// <value>The number of minutes to wait before polling blog-roll sources for changes.</value>
    public int BlogrollUpdateMinutes
    {
        get
        {
            return blogrollUpdateMinutes;
        }

        set
        {
            blogrollUpdateMinutes = value;
        }
    }
    #endregion

    #region BlogrollVisiblePosts
    /// <summary>
    /// Gets or sets the number of posts to display from a blog-roll source.
    /// </summary>
    /// <value>The number of posts to display from a blog-roll source.</value>
    public int BlogrollVisiblePosts
    {
        get
        {
            return blogrollVisiblePosts;
        }

        set
        {
            blogrollVisiblePosts = value;
        }
    }
    #endregion

    //============================================================
    //	SEARCH SETTINGS
    //============================================================
    #region EnableCommentSearch
    /// <summary>
    /// Gets or sets a value indicating if search of post comments is enabled.
    /// </summary>
    /// <value><b>true</b> if post comments can be searched, otherwise returns <b>false</b>.</value>
    public bool EnableCommentSearch
    {
        get
        {
            return enableCommentSearch;
        }

        set
        {
            enableCommentSearch = value;
        }
    }
    #endregion

    #region SearchButtonText
    /// <summary>
    /// Gets or sets the search button text to be displayed.
    /// </summary>
    /// <value>The search button text to be displayed.</value>
    public string SearchButtonText
    {
        get
        {
            return searchButtonText;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                searchButtonText = String.Empty;
            }
            else
            {
                searchButtonText = value;
            }
        }
    }
    #endregion

    #region SearchCommentLabelText
    /// <summary>
    /// Gets or sets the search comment label text to display.
    /// </summary>
    /// <value>The search comment label text to display.</value>
    public string SearchCommentLabelText
    {
        get
        {
            return searchCommentLabelText;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                searchCommentLabelText = String.Empty;
            }
            else
            {
                searchCommentLabelText = value;
            }
        }
    }
    #endregion

    #region SearchDefaultText
    /// <summary>
    /// Gets or sets the default search text to display.
    /// </summary>
    /// <value>The default search text to display.</value>
    public string SearchDefaultText
    {
        get
        {
            return searchDefaultText;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                searchDefaultText = String.Empty;
            }
            else
            {
                searchDefaultText = value;
            }
        }
    }
    #endregion

    //============================================================
    //	BLOG CHANNEL EXTENSION SETTINGS
    //============================================================
    #region Endorsement
    /// <summary>
    /// Gets or sets the URI of a web log that the author of this web log is promoting.
    /// </summary>
    /// <value>The <see cref="Uri"/> of a web log that the author of this web log is promoting.</value>
    public string Endorsement
    {
        get
        {
            return blogChannelBLink;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                blogChannelBLink = String.Empty;
            }
            else
            {
                blogChannelBLink = value;
            }
        }
    }
    #endregion

    //============================================================
    //	DUBLIN CORE EXTENSION SETTINGS
    //============================================================
    #region AuthorName
    /// <summary>
    /// Gets or sets the name of the author of this blog.
    /// </summary>
    /// <value>The name of the author of this blog.</value>
    public string AuthorName
    {
        get
        {
            return dublinCoreCreator;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                dublinCoreCreator = String.Empty;
            }
            else
            {
                dublinCoreCreator = value;
            }
        }
    }
    #endregion

    #region Language
    /// <summary>
    /// Gets or sets the language this blog is written in.
    /// </summary>
    /// <value>The language this blog is written in.</value>
    /// <remarks>
    ///     Recommended best practice for the values of the Language element is defined by RFC 1766 [RFC1766] which includes a two-letter Language Code (taken from the ISO 639 standard [ISO639]), 
    ///     followed optionally, by a two-letter Country Code (taken from the ISO 3166 standard [ISO3166]).
    /// </remarks>
    /// <example>en-US</example>
    public string Language
    {
        get
        {
            return dublinCoreLanguage;
        }

        set
        {
            if (String.IsNullOrEmpty(value))
            {
                dublinCoreLanguage = String.Empty;
            }
            else
            {
                dublinCoreLanguage = value.Trim();
            }
        }
    }
    #endregion

    //============================================================
    //	GEOCODING EXTENSION SETTINGS
    //============================================================
    #region GeocodingLatitude
    /// <summary>
    /// Gets or sets the latitude component of the geocoding position for this blog.
    /// </summary>
    /// <value>The latitude value.</value>
    public float GeocodingLatitude
    {
        get
        {
            return geocodingLatitude;
        }

        set
        {
            geocodingLatitude = value;
        }
    }
    #endregion

    #region GeocodingLongitude
    /// <summary>
    /// Gets or sets the longitude component of the geocoding position for this blog.
    /// </summary>
    /// <value>The longitude value.</value>
    public float GeocodingLongitude
    {
        get
        {
            return geocodingLongitude;
        }

        set
        {
            geocodingLongitude = value;
        }
    }
    #endregion

    //============================================================
    //	PRIVATE ROUTINES
    //============================================================
    #region Load()
    /// <summary>
    /// Initializes the singleton instance of the <see cref="BlogSettings"/> class.
    /// </summary>
    private void Load()
    {
        string fileName = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SettingsFile"]);
        Type type       = this.GetType();
        XmlDocument doc = new XmlDocument();

        doc.Load(fileName);

        foreach (XmlNode node in doc.SelectSingleNode("settings").ChildNodes)
        {
            string name     = node.Name;
            string value    = node.InnerText;

            foreach (PropertyInfo info in type.GetProperties())
            {
                if (info.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    switch (info.PropertyType.ToString())
                    {
                        case "System.String":
                            info.SetValue(this, value, null);
                            break;

                        case "System.Int32":
                            info.SetValue(this, int.Parse(value), null);
                            break;

                        case "System.DateTime":
                            info.SetValue(this, DateTime.Parse(value), null);
                            break;

                        case "System.Double":
                            info.SetValue(this, double.Parse(value), null);
                            break;

                        case "System.Int64":
                            info.SetValue(this, long.Parse(value), null);
                            break;

                        case "System.Boolean":
                            info.SetValue(this, bool.Parse(value), null);
                            break;

                        default:
                            throw new InvalidCastException("The BlogSettings does not allow properties of type '" + info.PropertyType + "'");
                    }
                    break;
                }
            }
        }
    }
    #endregion

    #region OnChanged()
    /// <summary>
    /// Occurs when the settings have been changed.
    /// </summary>
    private static void OnChanged()
    {
        if (BlogSettings.Changed != null)
        {
            BlogSettings.Changed(null, new EventArgs());
        }
    }
    #endregion

    //============================================================
    //	PUBLIC ROUTINES
    //============================================================
    #region Save()
    /// <summary>
    /// Saves the settings to disk.
    /// </summary>
    public void Save()
    {
        string fileName             = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SettingsFile"]);
        Type type                   = this.GetType();

        XmlWriterSettings settings  = new XmlWriterSettings();
        settings.Indent             = true;

        using (XmlWriter writer = XmlWriter.Create(fileName, settings))
        {
            writer.WriteStartElement("settings");

            foreach (PropertyInfo info in type.GetProperties())
            {
                try
                {
                    if (info.Name != "Instance")
                    {
                        writer.WriteElementString(info.Name, info.GetValue(this, null).ToString());
                    }
                }
                catch 
                {
                }
            }

            writer.WriteEndElement();
        }

        OnChanged();
    }
    #endregion
}
