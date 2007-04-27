/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/12/2007	brian.kuhn		Created RssChannel Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Rss
{
    /// <summary>
    /// Represents a feed channel, which contains information about the channel (metadata) and its contents.
    /// </summary>
    [Serializable()]
    public class RssChannel : SyndicationFeedEntityBase
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the name of the channel.
        /// </summary>
        private string channelTitle                             = String.Empty;
        /// <summary>
        /// Private member to hold the URL to the HTML website corresponding to the channel.
        /// </summary>
        private Uri channelLink;
        /// <summary>
        /// Private member to hold a phrase or sentence describing the channel.
        /// </summary>
        private string channelDescription                       = String.Empty;
        /// <summary>
        /// Private member to hold the language the channel is written in.
        /// </summary>
        private string channelLanguage                          = String.Empty;
        /// <summary>
        /// Private member to hold the copyright notice for content in this channel.
        /// </summary>
        private string channelCopyright                         = String.Empty;
        /// <summary>
        /// Private member to hold the email address and name for the person responsible for editorial content.
        /// </summary>
        private string channelManagingEditor                    = String.Empty;
        /// <summary>
        /// Private member to hold the email address and name for the person responsible for technical issues relating to channel.
        /// </summary>
        private string channelWebMaster                         = String.Empty;
        /// <summary>
        /// Private member to hold the publication date for the content in the channel.
        /// </summary>
        private Rfc822DateTime channelPublicationDate;
        /// <summary>
        /// Private member to hold the last time the content of the channel changed.
        /// </summary>
        private Rfc822DateTime channelLastBuildDate;
        /// <summary>
        /// Private member to hold the information that indicates the program used to generate the channel.
        /// </summary>
        private string channelGenerator                         = "Argotic Syndication Library, http://argotic.oppositionallydefiant.com/";
        /// <summary>
        /// Private member to hold a URL that points to the documentation for the RSS syndication format.
        /// </summary>
        private const string SPECIFICATION_DOCUMENTATION_URL    = "http://www.rssboard.org/rss-specification";
        /// <summary>
        /// Private member to hold the number of minutes that indicates how long a channel can be cached before refreshing from the source.
        /// </summary>
        private int channelTimeToLive                           = Int32.MinValue;
        /// <summary>
        /// Private member to hold the PICS rating for the channel.
        /// </summary>
        private string channelRating                            = String.Empty;
        /// <summary>
        /// Private member to hold a GIF, JPEG or PNG image that can be displayed with the channel.
        /// </summary>
        private RssImage channelImage;
        /// <summary>
        /// Private member to hold collection of associated categories for the channel.
        /// </summary>
        private RssCategoryCollection channelCategories;
        /// <summary>
        /// Private member to hold the cloud to be notified of updates to the channel.
        /// </summary>
        private RssCloud channelCloud;
        /// <summary>
        /// Private member to hold a text-input that can be used to specify a search engine box or to allow a reader to provide feedback.
        /// </summary>
        private RssTextInput channelTextInput;
        /// <summary>
        /// Private member to hold hours aggregators may not read the channel.
        /// </summary>
        private Collection<int> channelSkipHours;
        /// <summary>
        /// Private member to hold days aggregators may not read the channel.
        /// </summary>
        private Collection<DayOfWeek> channelSkipDays;
        /// <summary>
        /// Private member to hold collection of associated items for the channel.
        /// </summary>
        private RssItemCollection channelItems;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssChannel()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssChannel"/> class.
        /// </summary>
        public RssChannel() : base()
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	
                //------------------------------------------------------------
                
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

        #region RssChannel(string title, string description, Uri link)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssChannel"/> class using the supplied title, description, and link.
        /// </summary>
        /// <param name="title">The name of the channel.</param>
        /// <param name="description">A phrase or sentence describing the channel.</param>
        /// <param name="link">The URL to the HTML website corresponding to the channel.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="title"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="description"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="link"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="title"/> is an empty string -or- the <paramref name="description"/> is an empty string.</exception>
        public RssChannel(string title, string description, Uri link)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Description    = description;
                this.Link           = link;
                this.Title          = title;
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
        //	PUBLIC PROPERTIES
        //============================================================
        #region Categories
        /// <summary>
        /// Gets or sets the collection of categories associated to this channel.
        /// </summary>
        /// <value>A collection of <see cref="RssCategory"/> instances associated to this channel.</value>
        [XmlElement(ElementName = "category", Type = typeof(RssCategory))]
        public RssCategoryCollection Categories
        {
            get
            {
                if (channelCategories == null)
                {
                    channelCategories   = new RssCategoryCollection();
                }
                return channelCategories;
            }
        }
        #endregion

        #region Cloud
        /// <summary>
        /// Gets or sets the cloud to be notified of updates to the channel.
        /// </summary>
        /// <value>The cloud to be notified of updates to the channel.</value>
        /// <remarks>This is an optional property. Specifies a web service that supports the rssCloud interface which allows processes to register with a cloud to be notified of updates to the channel.</remarks>
        [XmlElement(ElementName = "cloud", Type = typeof(RssCloud))]
        public RssCloud Cloud
        {
            get
            {
                return channelCloud;
            }

            set
            {
                if (value == null)
                {
                    channelCloud = null;
                }
                else
                {
                    channelCloud = value;
                }
            }
        }
        #endregion

        #region Copyright
        /// <summary>
        /// Gets or sets the copyright notice for content in this channel.
        /// </summary>
        /// <value>The copyright notice for content in this channel.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "copyright", Type = typeof(System.String))]
        public string Copyright
        {
            get
            {
                return channelCopyright;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    channelCopyright = String.Empty;
                }
                else
                {
                    channelCopyright = value.Trim();
                }
            }
        }
        #endregion

        #region Description
        /// <summary>
        /// Gets or sets a phrase or sentence describing the channel.
        /// </summary>
        /// <value>A phrase or sentence describing the channel.</value>
        /// <remarks>This is an required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlElement(ElementName = "description", Type = typeof(System.String))]
        public string Description
        {
            get
            {
                return channelDescription;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else if (String.IsNullOrEmpty(value.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "value");
                }
                else
                {
                    channelDescription = value.Trim();
                }
            }
        }
        #endregion

        #region Documentation
        /// <summary>
        /// Gets a URL that points to the documentation for the RSS syndication format.
        /// </summary>
        /// <value>A URL that points to the documentation for the RSS syndication format.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "docs", Type = typeof(System.String))]
        public string Documentation
        {
            get
            {
                return SPECIFICATION_DOCUMENTATION_URL;
            }
        }
        #endregion

        #region Generator
        /// <summary>
        /// Gets or sets the information that indicates the program used to generate the channel.
        /// </summary>
        /// <value>The information that indicates the program used to generate the channel.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "generator", Type = typeof(System.String))]
        public string Generator
        {
            get
            {
                return channelGenerator;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    channelGenerator = String.Empty;
                }
                else
                {
                    channelGenerator = value.Trim();
                }
            }
        }
        #endregion

        #region Image
        /// <summary>
        /// Gets or sets a GIF, JPEG or PNG image that can be displayed with the channel.
        /// </summary>
        /// <value>A GIF, JPEG or PNG image that can be displayed with the channel.</value>
        /// <remarks>This is an optional property. In practice image title and link should be the same value as channel title and link.</remarks>
        [XmlElement(ElementName = "image", Type = typeof(RssImage))]
        public RssImage Image
        {
            get
            {
                return channelImage;
            }

            set
            {
                if(value == null)
                {
                    channelImage = null;
                }
                else
                {
                    channelImage = value;
                }
            }
        }
        #endregion

        #region Items
        /// <summary>
        /// Gets or sets the collection of items associated to this channel.
        /// </summary>
        /// <value>A collection of <see cref="RssItem"/> instances associated to this channel.</value>
        [XmlElement(ElementName = "item", Type = typeof(RssItem))]
        public RssItemCollection Items
        {
            get
            {
                if (channelItems == null)
                {
                    channelItems = new RssItemCollection();
                }
                return channelItems;
            }
        }
        #endregion

        #region Language
        /// <summary>
        /// Gets or sets the language the channel is written in.
        /// </summary>
        /// <value>The language the channel is written in.</value>
        /// <remarks>This is an optional property. See http://www.w3.org/TR/REC-html40/struct/dirlang.html#langcodes for a list of W3C defined language codes.</remarks>
        /// <example>en-us</example>
        [XmlElement(ElementName = "language", Type = typeof(System.String))]
        public string Language
        {
            get
            {
                return channelLanguage;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    channelLanguage = String.Empty;
                }
                else
                {
                    channelLanguage = value.Trim();
                }
            }
        }
        #endregion

        #region LastBuildDate
        /// <summary>
        /// Gets or sets the last time the content of the channel changed.
        /// </summary>
        /// <value>The last time the content of the channel changed.</value>
        /// <remarks>
        ///     This is an optional property. All date-times in RSS conform to the Date and Time Specification of <a href="http://asg.web.cmu.edu/rfc/rfc822.html">RFC 822</a>, with the exception that the year may be expressed with two characters or four characters (four preferred).
        /// </remarks>
        /// <example>Sat, 07 Sep 2002 09:42:31 GMT</example>
        [XmlElement(ElementName = "lastBuildDate", Type = typeof(Rfc822DateTime))]
        public Rfc822DateTime LastBuildDate
        {
            get
            {
                return channelLastBuildDate;
            }

            set
            {
                if (value == null)
                {
                    channelLastBuildDate = null;
                }
                else
                {
                    channelLastBuildDate = value;
                }
            }
        }
        #endregion

        #region Link
        /// <summary>
        /// Gets or sets the URL to the HTML website corresponding to the channel.
        /// </summary>
        /// <value>The URL to the HTML website corresponding to the channel.</value>
        /// <remarks>This is a required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "link", Type = typeof(System.Uri))]
        public Uri Link
        {
            get
            {
                return channelLink;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    channelLink = value;
                }
            }
        }
        #endregion

        #region ManagingEditor
        /// <summary>
        /// Gets or sets the email address and name for the person responsible for editorial content.
        /// </summary>
        /// <value>The email address and name for person responsible for editorial content.</value>
        /// <remarks>This is an optional property.</remarks>
        /// <example>john.doe@domain.com (John Doe)</example>
        [XmlElement(ElementName = "managingEditor", Type = typeof(System.String))]
        public string ManagingEditor
        {
            get
            {
                return channelManagingEditor;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    channelManagingEditor = String.Empty;
                }
                else
                {
                    channelManagingEditor = value.Trim();
                }
            }
        }
        #endregion

        #region PublicationDate
        /// <summary>
        /// Gets or sets the publication date for the content in the channel.
        /// </summary>
        /// <value>The publication date for the content in the channel.</value>
        /// <remarks>
        ///     This is an optional property. All date-times in RSS conform to the Date and Time Specification of <a href="http://asg.web.cmu.edu/rfc/rfc822.html">RFC 822</a>, with the exception that the year may be expressed with two characters or four characters (four preferred).
        /// </remarks>
        /// <example>Sat, 07 Sep 2002 00:00:01 GMT</example>
        [XmlElement(ElementName = "pubDate", Type = typeof(Rfc822DateTime))]
        public Rfc822DateTime PublicationDate
        {
            get
            {
                return channelPublicationDate;
            }

            set
            {
                if (value == null)
                {
                    channelPublicationDate = null;
                }
                else
                {
                    channelPublicationDate = value;
                }
            }
        }
        #endregion

        #region Rating
        /// <summary>
        /// Gets or sets the <a href="http://www.w3.org/PICS/">PICS</a> rating for the channel.
        /// </summary>
        /// <value>The PICS rating for the channel.</value>
        /// <remarks>This is an optional property. See http://www.w3.org/PICS/ for more information.</remarks>
        [XmlElement(ElementName = "rating", Type = typeof(System.String))]
        public string Rating
        {
            get
            {
                return channelRating;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    channelRating = String.Empty;
                }
                else
                {
                    channelRating = value.Trim();
                }
            }
        }
        #endregion

        #region SkipDays
        /// <summary>
        /// Gets a collection of <see cref="DayOfWeek"/> enumeration values that represent days aggregators may not read this channel.
        /// </summary>
        /// <remarks>Collection should contain up to seven distinct <see cref="DayOfWeek"/> enumeration values.</remarks>
        [XmlArray(ElementName = "skipDays")]
        [XmlArrayItem(ElementName = "day", Type = typeof(DayOfWeek))]
        public Collection<DayOfWeek> SkipDays
        {
            get
            {
                if (channelSkipDays == null)
                {
                    channelSkipDays = new Collection<DayOfWeek>();
                }
                return channelSkipDays;
            }
        }
        #endregion

        #region SkipHours
        /// <summary>
        /// Gets a collection of <see cref="System.Int32"/> values that represent hours aggregators may not read this channel.
        /// </summary>
        /// <remarks>Collection element values are a number between 0 and 23, representing a time in GMT.</remarks>
        [XmlArray(ElementName = "skipHours")]
        [XmlArrayItem(ElementName = "hour", Type = typeof(System.Int32))]
        public Collection<int> SkipHours
        {
            get
            {
                if (channelSkipHours == null)
                {
                    channelSkipHours = new Collection<int>();
                }
                return channelSkipHours;
            }
        }
        #endregion

        #region TextInput
        /// <summary>
        /// Gets or sets a text-input that can be used to specify a search engine box or to allow a reader to provide feedback.
        /// </summary>
        /// <value>The text-input associated to the channel.</value>
        /// <remarks>This is an optional property. Sometimes used to specify a search engine box or to allow a reader to provide feedback. Most feed aggregators ignore this entity.</remarks>
        [XmlElement(ElementName = "textInput", Type = typeof(RssTextInput))]
        public RssTextInput TextInput
        {
            get
            {
                return channelTextInput;
            }

            set
            {
                if (value == null)
                {
                    channelTextInput = null;
                }
                else
                {
                    channelTextInput = value;
                }
            }
        }
        #endregion

        #region TimeToLive
        /// <summary>
        /// Gets or sets the number of minutes that indicates how long a channel can be cached before refreshing from the source.
        /// </summary>
        /// <value>The number of minutes that indicates how long a channel can be cached before refreshing from the source.</value>
        /// <remarks>This is an optional property. The recommended default value is 60 minutes.</remarks>
        [XmlElement(ElementName = "ttl", Type = typeof(System.Int32))]
        public int TimeToLive
        {
            get
            {
                return channelTimeToLive;
            }

            set
            {
                channelTimeToLive = value;
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets the name of the channel.
        /// </summary>
        /// <value>The name of the channel.</value>
        /// <remarks>This is an required property. If an HTML website that contains the same information as your RSS feed exists, the title of the channel should be the same as the title of the website.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlElement(ElementName = "title", Type = typeof(System.String))]
        public string Title
        {
            get
            {
                return channelTitle;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else if (String.IsNullOrEmpty(value.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "value");
                }
                else
                {
                    channelTitle = value.Trim();
                }
            }
        }
        #endregion

        #region WebMaster
        /// <summary>
        /// Gets or sets the email address and name for the person responsible for technical issues relating to this channel.
        /// </summary>
        /// <value>The email address and name for the person responsible for technical issues relating to this channel.</value>
        /// <remarks>This is an optional property.</remarks>
        /// <example>web.master@domain.com (Jane Doe)</example>
        [XmlElement(ElementName = "webMaster", Type = typeof(System.String))]
        public string WebMaster
        {
            get
            {
                return channelWebMaster;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    channelWebMaster = String.Empty;
                }
                else
                {
                    channelWebMaster = value.Trim();
                }
            }
        }
        #endregion
    }
}
