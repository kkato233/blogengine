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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;

namespace BlogEngine.Core
{
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
    private string blogName = String.Empty;
    /// <summary>
    /// Private member to hold a brief synopsis of the blog.
    /// </summary>
    private string blogDescription = String.Empty;
    /// <summary>
    /// Private member to hold the default number of posts per day to display.
    /// </summary>
    private int postsPerPage = Int32.MinValue;
    /// <summary>
    /// Private member to hold blog storage location.
    /// </summary>
    private string storageLocation = String.Empty;
    /// <summary>
    /// Private member to hold a value indicating if the search word hightlight feature is enabled.
    /// </summary>
    private bool enableSearchHighlight;
    /// <summary>
    /// Private member to hold maximum number of characters that are displayed from a blog-roll retrieved post.
    /// </summary>
    private int blogrollMaxLength = Int32.MinValue;
    /// <summary>
    /// Private member to hold the number of minutes blog-roll entries are updated.
    /// </summary>
    private int blogrollUpdateMinutes = Int32.MinValue;
    /// <summary>
    /// Private member to hold the maximum number of blog-roll posts per blog-roll to display.
    /// </summary>
    private int blogrollVisiblePosts = Int32.MinValue;
    /// <summary>
    /// Private member to hold the name of the configured theme.
    /// </summary>
    private string configuredTheme = String.Empty;
    /// <summary>
    /// Private member to hold a value indicating if related posts is enabled.
    /// </summary>
    private bool enableRelatedPosts;
    /// <summary>
    /// Private member to hold the email address notifications are sent to.
    /// </summary>
    private string emailAddress = String.Empty;
    /// <summary>
    /// Private member to hold the SMTP server to contact when sending email.
    /// </summary>
    private string smtpServer = String.Empty;
    /// <summary>
    /// Private member to hold the SMTP port number.
    /// </summary>
    private int smtpServerPort = Int32.MinValue;
    /// <summary>
    /// Private member to hold the username used when contacting the SMTP server.
    /// </summary>  
    private string smtpUsername = String.Empty;
    /// <summary>
    /// Private member to hold the password used when contacting the SMTP server.
    /// </summary>
    private string smtpPassword = String.Empty;
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
    private int daysCommentsAreEnabled = Int32.MinValue;
    /// <summary>
    /// Private member to hold default number of recent posts to display.
    /// </summary>
    private int numberOfRecentPosts = Int32.MinValue;
    /// <summary>
    /// Private member to hold the search button text.
    /// </summary>
    private string searchButtonText = String.Empty;
    /// <summary>
    /// Private member to hold the default search text.
    /// </summary>
    private string searchDefaultText = String.Empty;
    /// <summary>
    /// Private member to hold a value indicating 
    /// </summary>
    private bool enableCommentSearch;
    /// <summary>
    /// Private member to hold the search comment label text.
    /// </summary>
    private string searchCommentLabelText = String.Empty;
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
    private string blogChannelBLink = String.Empty;
    /// <summary>
    /// Private member to hold the name of the author of the blog.
    /// </summary>
    private string dublinCoreCreator = String.Empty;
    /// <summary>
    /// Private member to hold the language the blog is written in.
    /// </summary>
    private string dublinCoreLanguage = String.Empty;
    /// <summary>
    /// Private member to hold the latitude component of the geocoding position for this blog.
    /// </summary>
    private float geocodingLatitude = Single.MinValue;
    /// <summary>
    /// Private member to hold the longitude component of the geocoding position for this blog.
    /// </summary>
    private float geocodingLongitude = Single.MinValue;
    /// <summary>
    /// Private member to hold the default syndication format for this blog.
    /// </summary>
    private string defaultSyndicationFormat = String.Empty;
    /// <summary>
    /// Private member to hold a value indicating if the css files should be compressed.
    /// </summary>
    private bool removeWhitespaceInStyleSheets;
    /// <summary>
    /// Private member to hold a value indicating if the open search link header should be added.
    /// </summary>
    private bool enableOpenSearch;
    /// <summary>
    /// Private member to hold a tracking script from e.g. Google Analytics.
    /// </summary>
    private string trackingScript;
    /// <summary>
    /// Connection String for MSSQL Provider
    /// </summary>
    private string mssqlConnectionString;
    /// <summary>
    /// Feedburner user name.
    /// </summary>
    private string feedburnerUserName;

    private string contactFormMessage;
    private string contactThankMessage;
    private string htmlHeader;
    private string culture;
    private int timezone;
    private int postsPerFeed;
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
      //try
      //{
      //------------------------------------------------------------
      //	Load the currently configured settings
      //------------------------------------------------------------
      Load();
      //}
      //catch
      //{
      //    //------------------------------------------------------------
      //    //	Rethrow exception
      //    //------------------------------------------------------------
      //    throw;
      //}
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

    #region FeedburnerUserName
    /// <summary>
    /// Gets or sets the FeedBurner user name.
    /// </summary>
    public string FeedburnerUserName
    {
      get { return feedburnerUserName; }
      set { feedburnerUserName = value; }
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

    #region SyndicationFormat
    /// <summary>
    /// Gets or sets the default syndication format used by the blog.
    /// </summary>
    /// <value>The default syndication format used by the blog.</value>
    /// <remarks>If no value is specified, blog defaults to using RSS 2.0 format.</remarks>
    /// <seealso cref="BlogEngine.Core.Syndication.SyndicationFormat"/>
    public string SyndicationFormat
    {
      get
      {
        return defaultSyndicationFormat;
      }

      set
      {
        if (String.IsNullOrEmpty(value))
        {
          defaultSyndicationFormat = String.Empty;
        }
        else
        {
          defaultSyndicationFormat = value;
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

    #region RemoveWhitespaceInStyleSheets
    /// <summary>
    /// Gets or sets a value indicating if whitespace in stylesheets should be removed
    /// </summary>
    /// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
    public bool RemoveWhitespaceInStyleSheets
    {
      get
      {
        return removeWhitespaceInStyleSheets;
      }

      set
      {
        removeWhitespaceInStyleSheets = value;
      }
    }
    #endregion

    #region EnableOpenSearch
    /// <summary>
    /// Gets or sets a value indicating if whitespace in stylesheets should be removed
    /// </summary>
    /// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
    public bool EnableOpenSearch
    {
      get
      {
        return enableOpenSearch;
      }

      set
      {
        enableOpenSearch = value;
      }
    }
    #endregion

    #region TrackingScript
    /// <summary>
    /// Gets or sets the tracking script used to collect visitor data.
    /// </summary>
    public string TrackingScript
    {
      get
      {
        return trackingScript;
      }

      set
      {
        trackingScript = value;
      }
    }
    #endregion

    //============================================================
    //	DATABASE SETTINGS
    //============================================================
    #region MSSQLConnectionString
    /// <summary>
    /// Gets or sets the connection string used to connect to the SQL database.
    /// </summary>
    public string MSSQLConnectionString
    {
      get { return mssqlConnectionString; }
      set { mssqlConnectionString = value; }
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

    #region SmtpServerPort
    /// <summary>
    /// Gets or sets the DNS name or IP address of the SMTP server used to send notification emails.
    /// </summary>
    /// <value>The DNS name or IP address of the SMTP server used to send notification emails.</value>
    public int SmtpServerPort
    {
      get
      {
        return smtpServerPort;
      }

      set
      {
        smtpServerPort = value;
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

    #region EnableSearchHightlight
    /// <summary>
    /// Gets or sets a value indicating if highlighting of search term matches is enabled.
    /// </summary>
    /// <value><b>true</b> if search terms are highlighted, otherwise returns <b>false</b>.</value>
    public bool EnableSearchHightlight
    {
      get
      {
        return enableSearchHighlight;
      }

      set
      {
        enableSearchHighlight = value;
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

    #region PostsPerFeed
    /// <summary>
    /// Gets or sets the maximum number of characters that are displayed from a blog-roll retrieved post.
    /// </summary>
    /// <value>The maximum number of characters to display.</value>
    public int PostsPerFeed
    {
      get
      {
        return postsPerFeed;
      }
      set
      {
        postsPerFeed= value;
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
    //	CONTACT FORM
    //============================================================
    #region ContactFormMessage;
    /// <summary>
    /// Gets or sets the name of the author of this blog.
    /// </summary>
    /// <value>The name of the author of this blog.</value>
    public string ContactFormMessage
    {
      get { return contactFormMessage; }
      set
      {
        if (String.IsNullOrEmpty(value))
        {
          contactFormMessage = String.Empty;
        }
        else
        {
          contactFormMessage = value;
        }
      }
    }
    #endregion

    #region ContactThankMessage
    /// <summary>
    /// Gets or sets the name of the author of this blog.
    /// </summary>
    /// <value>The name of the author of this blog.</value>
    public string ContactThankMessage
    {
      get { return contactThankMessage; }
      set
      {
        if (String.IsNullOrEmpty(value))
        {
          contactThankMessage = String.Empty;
        }
        else
        {
          contactThankMessage = value;
        }
      }
    }
    #endregion

    #region HtmlHeader
    /// <summary>
    /// Gets or sets the name of the author of this blog.
    /// </summary>
    /// <value>The name of the author of this blog.</value>
    public string HtmlHeader
    {
      get { return htmlHeader; }
      set
      {
        if (String.IsNullOrEmpty(value))
        {
          htmlHeader = String.Empty;
        }
        else
        {
          htmlHeader = value;
        }
      }
    }
    #endregion

    #region Culture
    /// <summary>
    /// Gets or sets the name of the author of this blog.
    /// </summary>
    /// <value>The name of the author of this blog.</value>
    public string Culture
    {
      get { return culture; }
      set
      {
        if (String.IsNullOrEmpty(value))
        {
          culture = String.Empty;
        }
        else
        {
          culture = value;
        }
      }
    }
    #endregion

    #region Timezone
    /// <summary>
    /// Gets or sets the maximum number of characters that are displayed from a blog-roll retrieved post.
    /// </summary>
    /// <value>The maximum number of characters to display.</value>
    public int Timezone
    {
      get { return timezone; }
      set { timezone = value; }
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
      Type settingsType = this.GetType();

      //------------------------------------------------------------
      //	Enumerate through individual settings nodes
      //------------------------------------------------------------
      System.Collections.Specialized.StringDictionary dic = Providers.BlogService.LoadSettings();
      foreach (string key in dic.Keys)
      {
        //------------------------------------------------------------
        //	Extract the setting's name/value pair
        //------------------------------------------------------------
        string name = key;
        string value = dic[key];

        //------------------------------------------------------------
        //	Enumerate through public properties of this instance
        //------------------------------------------------------------
        foreach (PropertyInfo propertyInformation in settingsType.GetProperties())
        {
          //------------------------------------------------------------
          //	Determine if configured setting matches current setting based on name
          //------------------------------------------------------------
          if (propertyInformation.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
          {
            //------------------------------------------------------------
            //	Attempt to apply configured setting
            //------------------------------------------------------------
            try
            {
              propertyInformation.SetValue(this, Convert.ChangeType(value, propertyInformation.PropertyType, CultureInfo.CurrentCulture), null);
            }
            catch
            {
              // TODO: Log exception to a common logging framework?
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
      //------------------------------------------------------------
      //	Attempt to raise the Changed event
      //------------------------------------------------------------
      try
      {
        //------------------------------------------------------------
        //	Execute event handler
        //------------------------------------------------------------
        if (BlogSettings.Changed != null)
        {
          BlogSettings.Changed(null, new EventArgs());
        }
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
    //	PUBLIC ROUTINES
    //============================================================
    #region Save()
    /// <summary>
    /// Saves the settings to disk.
    /// </summary>
    public void Save()
    {
      System.Collections.Specialized.StringDictionary dic = new System.Collections.Specialized.StringDictionary();
      Type settingsType = this.GetType();

      //------------------------------------------------------------
      //	Enumerate through settings properties
      //------------------------------------------------------------
      foreach (PropertyInfo propertyInformation in settingsType.GetProperties())
      {
        try
        {
          if (propertyInformation.Name != "Instance")
          {
            //------------------------------------------------------------
            //	Extract property value and its string representation
            //------------------------------------------------------------
            object propertyValue = propertyInformation.GetValue(this, null);
            string valueAsString = propertyValue.ToString();

            //------------------------------------------------------------
            //	Format null/default property values as empty strings
            //------------------------------------------------------------
            if (propertyValue.Equals(null))
            {
              valueAsString = String.Empty;
            }
            if (propertyValue.Equals(Int32.MinValue))
            {
              valueAsString = String.Empty;
            }
            if (propertyValue.Equals(Single.MinValue))
            {
              valueAsString = String.Empty;
            }

            //------------------------------------------------------------
            //	Write property name/value pair
            //------------------------------------------------------------
            dic.Add(propertyInformation.Name, valueAsString);
          }
        }
        catch
        { }
      }

      Providers.BlogService.SaveSettings(dic);
      OnChanged();
    }
    #endregion

    #region Version()
    /// <summary>
    /// Returns the BlogEngine.NET version information.
    /// </summary>
    /// <value>The BlogEngine.NET major, minor, revision, and build numbers.</value>
    /// <remarks>The current version is determined by extracting the build version of the BlogEngine.Core assembly.</remarks>
    public string Version()
    {
      //------------------------------------------------------------
      //	Attempt to retrieve version information
      //------------------------------------------------------------
      try
      {
        //------------------------------------------------------------
        //	Return assembly version information
        //------------------------------------------------------------
        return GetType().Assembly.GetName().Version.ToString();
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
  }
}