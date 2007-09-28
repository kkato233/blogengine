/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
08/29/2007	brian.kuhn	Created SyndicationGenerator Class
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace BlogEngine.Core
{
    /// <summary>
    /// Generates syndication feeds for blog entities.
    /// </summary>
    public class SyndicationGenerator
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the name of the syndication generation utility.
        /// </summary>
        private const string GENERATOR_NAME                 = "BlogEngine.Net Syndication Generator";
        /// <summary>
        /// Private member to hold the URI of the syndication generation utility.
        /// </summary>
        private static readonly Uri GENERATOR_URI           = new Uri("http://dotnetblogengine.net/");
        /// <summary>
        /// Private member to hold the version of the syndication generation utility.
        /// </summary>
        private static readonly Version GENERATOR_VERSION   = new Version("1.0.0.0");
        /// <summary>
        /// Private member to hold the <see cref="BlogSettings"/> to use when generating syndication results.
        /// </summary>
        private BlogSettings blogSettings;
        /// <summary>
        /// Private member to hold a collection of <see cref="Category"/> objects used to categorize the web log content.
        /// </summary>
        private List<Category> blogCategories;
        /// <summary>
        /// Private member to hold a collection of the XML namespaces that define supported syndication extensions.
        /// </summary>
        private static Dictionary<string, string> xmlNamespaces;
        #endregion

        //============================================================
		//	CONSTRUCTORS
        //============================================================
        #region SyndicationGenerator(BlogSettings settings, List<Category> categories)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationGenerator"/> class using the supplied <see cref="BlogSettings"/> and collection of <see cref="Category"/> objects.
        /// </summary>
        /// <param name="settings">The <see cref="BlogSettings"/> to use when generating syndication results.</param>
        /// <param name="categories">A collection of <see cref="Category"/> objects used to categorize the web log content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="categories"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationGenerator(BlogSettings settings, List<Category> categories)
        {
            //------------------------------------------------------------
            //	Attempt to handle class initialization
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }
                if (categories == null)
                {
                    throw new ArgumentNullException("categories");
                }

                //------------------------------------------------------------
                //	Initialize class state
                //------------------------------------------------------------
                this.Settings   = settings;

                if (categories.Count > 0)
                {
                    Category[] values = new Category[categories.Count];
                    categories.CopyTo(values);

                    foreach(Category category in values)
                    {
                        this.Categories.Add(category);
                    }
                }
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
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
        /// Gets a collection of <see cref="Category"/> objects used to categorize the web log content.
        /// </summary>
        /// <value>A collection of <see cref="Category"/> objects used to categorize the web log content.</value>
        public List<Category> Categories
        {
            get
            {
                if (blogCategories == null)
                {
                    blogCategories = new List<Category>();
                }

                return blogCategories;
            }
        }
        #endregion

        #region SupportedNamespaces
        /// <summary>
        /// Gets a collection of the XML namespaces used to provide support for syndication extensions.
        /// </summary>
        /// <value>The collection of the XML namespaces, keyed by namespace prefix, that are used to provide support for syndication extensions.</value>
        public static Dictionary<string, string> SupportedNamespaces
        {
            get
            {
                if(xmlNamespaces == null)
                {
                    xmlNamespaces = new Dictionary<string, string>();

                    xmlNamespaces.Add("blogChannel", "http://backend.userland.com/blogChannelModule");
                    xmlNamespaces.Add("dc", "http://purl.org/dc/elements/1.1/");

                    xmlNamespaces.Add("pingback", "http://madskills.com/public/xml/rss/module/pingback/");
                    xmlNamespaces.Add("trackback", "http://madskills.com/public/xml/rss/module/trackback/");
                    xmlNamespaces.Add("wfw", "http://wellformedweb.org/CommentAPI/");
                    xmlNamespaces.Add("slash", "http://purl.org/rss/1.0/modules/slash/");
                    xmlNamespaces.Add("geo", "http://www.w3.org/2003/01/geo/wgs84_pos#");
                }

                return xmlNamespaces;
            }
        }
        #endregion

        #region Settings
        /// <summary>
        /// Gets or sets the <see cref="BlogSettings"/> used when generating syndication results.
        /// </summary>
        /// <value>The <see cref="BlogSettings"/> used when generating syndication results.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public BlogSettings Settings
        {
            get
            {
                return blogSettings;
            }

            protected set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    blogSettings = value;
                }
            }
        }
        #endregion

        //============================================================
        //	STATIC UTILITY METHODS
        //============================================================
        #region FormatW3cOffset(TimeSpan offset, string separator)
        /// <summary>
        /// Converts the value of the specified <see cref="TimeSpan"/> to its equivalent string representation.
        /// </summary>
        /// <param name="offset">The <see cref="TimeSpan"/> to convert.</param>
        /// <param name="separator">Separator used to deliminate hours and minutes.</param>
        /// <returns>A string representation of the TimeSpan.</returns>
        private static string FormatW3cOffset(TimeSpan offset, string separator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string formattedOffset  = String.Empty;

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Generate formatted result
                //------------------------------------------------------------
                if (offset >= TimeSpan.Zero)
                {
                    formattedOffset = "+";
                }

                formattedOffset = String.Concat(formattedOffset, offset.Hours.ToString("00", CultureInfo.InvariantCulture), separator, offset.Minutes.ToString("00", CultureInfo.InvariantCulture));
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return formattedOffset;
        }
        #endregion

        #region GetPermaLink(IPublishable publishable)
        /// <summary>
        /// Creates a <see cref="Uri"/> that represents the peramlink for the supplied <see cref="IPublishable"/>.
        /// </summary>
        /// <param name="publishable">The <see cref="IPublishable"/> used to generate the permalink for.</param>
        /// <returns>A <see cref="Uri"/> that represents the peramlink for the supplied <see cref="IPublishable"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="publishable"/> is a null reference (Nothing in Visual Basic).</exception>
        public static Uri GetPermaLink(IPublishable publishable)
        {
            //------------------------------------------------------------
            //	Attempt to create permalink
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (publishable == null)
                {
                    throw new ArgumentNullException("publishable");
                }

                //------------------------------------------------------------
                //	Return result
                //------------------------------------------------------------
                Post post   = publishable as Post;
                if (post != null)
                {
                    return post.PermaLink;
                }
                else
                {
                    return Utils.ConvertToAbsolute(publishable.RelativeLink);
                }
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
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

        #region ToRfc822DateTime(DateTime dateTime)
        /// <summary>
        /// Converts the supplied <see cref="DateTime"/> to its equivalent <a href="http://asg.web.cmu.edu/rfc/rfc822.html">RFC-822 DateTime</a> string representation.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to convert.</param>
        /// <returns>The equivalent <a href="http://asg.web.cmu.edu/rfc/rfc822.html">RFC-822 DateTime</a> string representation.</returns>
        public static string ToRfc822DateTime(DateTime dateTime)
        {
            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Get offset and time zone
                //------------------------------------------------------------
                int offset      = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
                string timeZone = "+" + offset.ToString(NumberFormatInfo.InvariantInfo).PadLeft(2, '0');

                //------------------------------------------------------------
                //	Adjust time zone based on offset
                //------------------------------------------------------------
                if (offset < 0)
                {
                    int i       = offset * -1;
                    timeZone    = "-" + i.ToString(NumberFormatInfo.InvariantInfo).PadLeft(2, '0');

                }

                //------------------------------------------------------------
                //	Return RFC-822 formatted DateTime
                //------------------------------------------------------------
                return dateTime.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'), DateTimeFormatInfo.InvariantInfo);

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

        #region ToW3cDateTime(DateTime utcDateTime)
        /// <summary>
        /// Converts the supplied <see cref="DateTime"/> to its equivalent <a href="http://www.w3.org/TR/NOTE-datetime">W3C DateTime</a> string representation.
        /// </summary>
        /// <param name="utcDateTime">The Coordinated Universal Time (UTC) <see cref="DateTime"/> to convert.</param>
        /// <returns>The equivalent <a href="http://www.w3.org/TR/NOTE-datetime">W3C DateTime</a> string representation.</returns>
        public static string ToW3cDateTime(DateTime utcDateTime)
        {
            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Return result
                //------------------------------------------------------------
                TimeSpan utcOffset  = TimeSpan.Zero;
                return (utcDateTime + utcOffset).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) + SyndicationGenerator.FormatW3cOffset(utcOffset, ":");
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

        #region ToW3cDateTime(DateTime utcDateTime, TimeSpan utcOffset)
        /// <summary>
        /// Converts the supplied <see cref="DateTime"/> to its equivalent <a href="http://www.w3.org/TR/NOTE-datetime">W3C DateTime</a> string representation.
        /// </summary>
        /// <param name="utcDateTime">The Coordinated Universal Time (UTC) <see cref="DateTime"/> to convert.</param>
        /// <param name="utcOffset">The UTC offest of the <paramref name="utcDateTime"/>.</param>
        /// <returns>The equivalent <a href="http://www.w3.org/TR/NOTE-datetime">W3C DateTime</a> string representation.</returns>
        public static string ToW3cDateTime(DateTime utcDateTime, TimeSpan utcOffset)
        {
            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Return result
                //------------------------------------------------------------
                return (utcDateTime + utcOffset).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) + SyndicationGenerator.FormatW3cOffset(utcOffset, ":");
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
        //	PUBLIC METHODS
        //============================================================
        #region WriteFeed(SyndicationFormat format, Stream stream, List<IPublishable> publishables)
        /// <summary>
        /// Writes a generated syndication feed that conforms to the supplied <see cref="SyndicationFormat"/> using the supplied <see cref="Stream"/> and collection.
        /// </summary>
        /// <param name="format">A <see cref="SyndicationFormat"/> enumeration value indicating the syndication format to generate.</param>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="publishables">The collection of <see cref="IPublishable"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="publishables"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The provided <paramref name="stream"/> does not support writing.</exception>
        /// <exception cref="NotImplementedException">The provided <paramref name="format"/> has not been implemented by the <see cref="SyndicationGenerator"/>.</exception>
        public void WriteFeed(SyndicationFormat format, Stream stream, List<IPublishable> publishables)
        {
            //------------------------------------------------------------
            //	Attempt to generate syndication feed and write to stream
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (stream == null)
                {
                    throw new ArgumentNullException("stream");
                }
                if (publishables == null)
                {
                    throw new ArgumentNullException("publishables");
                }
                if (!stream.CanWrite)
                {
                    throw new ArgumentException(String.Format(null, "Unable to generate {0} syndication feed. The provided stream does not support writing.", format), "stream");
                }

                //------------------------------------------------------------
                //	Write syndication feed based on specified format
                //------------------------------------------------------------
                switch (format)
                {
                    case SyndicationFormat.Atom:
                        this.WriteAtomFeed(stream, publishables);
                        break;

                    case SyndicationFormat.Rss:
                        this.WriteRssFeed(stream, publishables);
                        break;

                    default:
                        throw new NotImplementedException(String.Format(null, "The syndication format '{0}' has not been implemented.", format));
                }
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
            }
            catch (ArgumentException)
            {
                //------------------------------------------------------------
                //	Rethrow argument exception
                //------------------------------------------------------------
                throw;
            }
            catch (XmlException)
            {
                //------------------------------------------------------------
                //	Rethrow XML exception
                //------------------------------------------------------------
                throw;
            }
            catch (NotImplementedException)
            {
                //------------------------------------------------------------
                //	Rethrow not implemented exception
                //------------------------------------------------------------
                throw;
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
        //	SYNDICATION WRITE METHODS
        //============================================================
        #region WriteAtomFeed(Stream stream, List<IPublishable> publishables)
        /// <summary>
        /// Writes a generated Atom syndication feed to the specified <see cref="Stream"/> using the supplied collection.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="publishables">The collection of <see cref="IPublishable"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="publishables"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteAtomFeed(Stream stream, List<IPublishable> publishables)
        {
            //------------------------------------------------------------
            //	Attempt to generate syndication feed and write to stream
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (stream == null)
                {
                    throw new ArgumentNullException("stream");
                }
                if (publishables == null)
                {
                    throw new ArgumentNullException("publishables");
                }

                //------------------------------------------------------------
                //	Define writer settings
                //------------------------------------------------------------
                XmlWriterSettings writerSettings    = new XmlWriterSettings();
                writerSettings.Encoding             = System.Text.Encoding.UTF8;
                writerSettings.Indent               = true;

                //------------------------------------------------------------
                //	Create writer against stream using defined settings
                //------------------------------------------------------------
                using(XmlWriter writer = XmlWriter.Create(stream, writerSettings))
                {
                    //------------------------------------------------------------
                    //	Write root element
                    //------------------------------------------------------------
                    writer.WriteStartElement("feed", "http://www.w3.org/2005/Atom");

                    //------------------------------------------------------------
                    //	Write syndication feed version attribute
                    //------------------------------------------------------------
                    writer.WriteAttributeString("version", "1.0");

                    //------------------------------------------------------------
                    //	Write XML namespaces used to support syndication extensions
                    //------------------------------------------------------------
                    foreach (string prefix in SyndicationGenerator.SupportedNamespaces.Keys)
                    {
                        writer.WriteAttributeString("xmlns", prefix, null, SyndicationGenerator.SupportedNamespaces[prefix]);
                    }

                    //------------------------------------------------------------
                    //	Write feed content
                    //------------------------------------------------------------
                    this.WriteAtomContent(writer, publishables);

                    writer.WriteFullEndElement();
                }
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
            }
            catch (XmlException)
            {
                //------------------------------------------------------------
                //	Rethrow XML exception
                //------------------------------------------------------------
                throw;
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

        #region WriteRssFeed(Stream stream, List<IPublishable> publishables)
        /// <summary>
        /// Writes a generated RSS syndication feed to the specified <see cref="Stream"/> using the supplied collection.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="publishables">The collection of <see cref="IPublishable"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="publishables"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteRssFeed(Stream stream, List<IPublishable> publishables)
        {
            //------------------------------------------------------------
            //	Attempt to generate syndication feed and write to stream
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (stream == null)
                {
                    throw new ArgumentNullException("stream");
                }
                if (publishables == null)
                {
                    throw new ArgumentNullException("publishables");
                }

                //------------------------------------------------------------
                //	Define writer settings
                //------------------------------------------------------------
                XmlWriterSettings writerSettings    = new XmlWriterSettings();
                writerSettings.Encoding             = System.Text.Encoding.UTF8;
                writerSettings.Indent               = true;

                //------------------------------------------------------------
                //	Create writer against stream using defined settings
                //------------------------------------------------------------
                using(XmlWriter writer = XmlWriter.Create(stream, writerSettings))
                {
                    //------------------------------------------------------------
                    //	Write root element
                    //------------------------------------------------------------
                    writer.WriteStartElement("rss");

                    //------------------------------------------------------------
                    //	Write syndication feed version attribute
                    //------------------------------------------------------------
                    writer.WriteAttributeString("version", "2.0");

                    //------------------------------------------------------------
                    //	Write XML namespaces used to support syndication extensions
                    //------------------------------------------------------------
                    foreach (string prefix in SyndicationGenerator.SupportedNamespaces.Keys)
                    {
                        writer.WriteAttributeString("xmlns", prefix, null, SyndicationGenerator.SupportedNamespaces[prefix]);
                    }

                    //------------------------------------------------------------
                    //	Write <channel> element
                    //------------------------------------------------------------
                    this.WriteRssChannel(writer, publishables);

                    writer.WriteFullEndElement();
                }
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
            }
            catch (XmlException)
            {
                //------------------------------------------------------------
                //	Rethrow XML exception
                //------------------------------------------------------------
                throw;
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
        //	PRIVATE RSS METHODS
        //============================================================
        #region WriteRssChannel(XmlWriter writer, List<IPublishable> publishables)
        /// <summary>
        /// Writes the RSS channel element information to the specified <see cref="XmlWriter"/> using the supplied collection.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="publishables">The collection of <see cref="IPublishable"/> objects used to generate syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="publishables"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssChannel(XmlWriter writer, List<IPublishable> publishables)
        {
            //------------------------------------------------------------
            //	Attempt to write channel information
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }
                if (publishables == null)
                {
                    throw new ArgumentNullException("publishables");
                }

                //------------------------------------------------------------
                //	Write <channel> element
                //------------------------------------------------------------
                writer.WriteStartElement("channel");

                //------------------------------------------------------------
                //	Write required channel elements
                //------------------------------------------------------------
                writer.WriteElementString("title", this.Settings.Name);
                writer.WriteElementString("description", this.Settings.Description);
                writer.WriteElementString("link", Utils.AbsoluteWebRoot.ToString());

                //------------------------------------------------------------
                //	Write common/shared channel elements
                //------------------------------------------------------------
                this.WriteRssChannelCommonElements(writer);

                //------------------------------------------------------------
                //	Enumerate through publishable content
                //------------------------------------------------------------
                foreach (IPublishable publishable in publishables)
                {
                    //------------------------------------------------------------
                    //	Skip publishable content if it is not visible
                    //------------------------------------------------------------
                    if (!publishable.IsVisible)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <item> element for publishable content
                    //------------------------------------------------------------
                    this.WriteRssItem(writer, publishable);
                }

                //------------------------------------------------------------
                //	Write </channel> element
                //------------------------------------------------------------
                writer.WriteEndElement();
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
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

        #region WriteRssChannelCommonElements(XmlWriter writer)
        /// <summary>
        /// Writes the common/shared RSS channel element information to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssChannelCommonElements(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write channel information
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write optional channel elements
                //------------------------------------------------------------
                writer.WriteElementString("docs", "http://www.rssboard.org/rss-specification");
                writer.WriteElementString("generator", String.Format(null, "{0} {1} ({2})", GENERATOR_NAME, GENERATOR_VERSION, GENERATOR_URI));
                if (!String.IsNullOrEmpty(this.Settings.Language))
                {
                    writer.WriteElementString("language", this.Settings.Language);
                }

                //------------------------------------------------------------
                //	Write blogChannel syndication extension elements
                //------------------------------------------------------------
                Uri blogRoll;
                if (Uri.TryCreate(String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/opml.axd"), UriKind.RelativeOrAbsolute, out blogRoll))
                {
                    writer.WriteElementString("blogChannel", "blogRoll", "http://backend.userland.com/blogChannelModule", blogRoll.ToString());
                }

                if (!String.IsNullOrEmpty(this.Settings.Endorsement))
                {
                    Uri blink;
                    if (Uri.TryCreate(this.Settings.Endorsement, UriKind.RelativeOrAbsolute, out blink))
                    {
                        writer.WriteElementString("blogChannel", "blink", "http://backend.userland.com/blogChannelModule", blink.ToString());
                    }
                }

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Settings.AuthorName))
                {
                    writer.WriteElementString("dc", "creator", "http://purl.org/dc/elements/1.1/", this.Settings.AuthorName);
                }
                if (!String.IsNullOrEmpty(this.Settings.Description))
                {
                    writer.WriteElementString("dc", "description", "http://purl.org/dc/elements/1.1/", this.Settings.Description);
                }
                if (!String.IsNullOrEmpty(this.Settings.Name))
                {
                    writer.WriteElementString("dc", "title", "http://purl.org/dc/elements/1.1/", this.Settings.Name);
                }

                //------------------------------------------------------------
                //	Write basic geo-coding syndication extension elements
                //------------------------------------------------------------
                NumberFormatInfo decimalFormatInfo      = new NumberFormatInfo();
                decimalFormatInfo.NumberDecimalDigits   = 6;

                if (this.Settings.GeocodingLatitude != Single.MinValue)
                {
                    writer.WriteElementString("geo", "lat", "http://www.w3.org/2003/01/geo/wgs84_pos#", this.Settings.GeocodingLatitude.ToString("N", decimalFormatInfo));
                }
                if (this.Settings.GeocodingLongitude != Single.MinValue)
                {
                    writer.WriteElementString("geo", "long", "http://www.w3.org/2003/01/geo/wgs84_pos#", this.Settings.GeocodingLongitude.ToString("N", decimalFormatInfo));
                }
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
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

        #region WriteRssItem(XmlWriter writer, IPublishable publishable)
        /// <summary>
        /// Writes the RSS channel item element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Page"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel item element information to.</param>
        /// <param name="publishable">The <see cref="IPublishable"/> used to generate channel item content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="publishable"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssItem(XmlWriter writer, IPublishable publishable)
        {
            //------------------------------------------------------------
            //	Attempt to write channel item information
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }
                if (publishable == null)
                {
                    throw new ArgumentNullException("publishable");
                }

                //------------------------------------------------------------
                //	Cast IPublishable as Post to support comments/trackback
                //------------------------------------------------------------
                Post post       = publishable as Post;

                //------------------------------------------------------------
                //	Cast IPublishable as Comment to support custom title
                //------------------------------------------------------------
                Comment comment = publishable as Comment;

                //------------------------------------------------------------
                //	Write <item> element
                //------------------------------------------------------------
                writer.WriteStartElement("item");

                //------------------------------------------------------------
                //	Raise serving event
                //------------------------------------------------------------                
                ServingEventArgs arg = new ServingEventArgs(publishable.Content, ServingLocation.Feed);
                publishable.OnServing(arg);

                //------------------------------------------------------------
                //	Modify post content to make references absolute
                //------------------------------------------------------------    
                string content  = arg.Body;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");
                content         = content.Replace("href=\"/", "href=\"" + Utils.AbsoluteWebRoot);

                //------------------------------------------------------------
                //	Modify publishable title to support comment feeds
                //------------------------------------------------------------
                string title = publishable.Title;
                if (comment != null)
                {
                    title       = String.Format(null, "{0}{1}", comment.DateCreated.ToString("MMMM d. yyyy HH:mm", CultureInfo.InvariantCulture), !String.IsNullOrEmpty(comment.Author) ? String.Concat(" by ", comment.Author) : String.Empty);
                }

                //------------------------------------------------------------
                //	Write required channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("title", title);
                writer.WriteElementString("description", content);
                writer.WriteElementString("link", Utils.ConvertToAbsolute(publishable.RelativeLink).ToString());

                 //------------------------------------------------------------
                //	Write optional channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("author", publishable.Author);
                if (post != null)
                {
                  writer.WriteElementString("comments", String.Concat(Utils.ConvertToAbsolute(publishable.RelativeLink).ToString(), "#comment"));
                }
                writer.WriteElementString("guid", SyndicationGenerator.GetPermaLink(publishable).ToString());
                writer.WriteElementString("pubDate", SyndicationGenerator.ToRfc822DateTime(publishable.DateCreated));

                //------------------------------------------------------------
                //	Write channel item category elements
                //------------------------------------------------------------
                if (publishable.Categories != null)
                {
                    foreach (Category category in publishable.Categories)
                    {
                        writer.WriteElementString("category", category.Title);
                    }
                }

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(publishable.Author))
                {
                    writer.WriteElementString("dc", "publisher", "http://purl.org/dc/elements/1.1/", publishable.Author);
                }
                if (!String.IsNullOrEmpty(publishable.Description))
                {
                    writer.WriteElementString("dc", "description", "http://purl.org/dc/elements/1.1/", publishable.Description);
                }

                //------------------------------------------------------------
                //	Write pingback syndication extension elements
                //------------------------------------------------------------
                Uri pingbackServer;
                if (Uri.TryCreate(String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/pingback.axd"), UriKind.RelativeOrAbsolute, out pingbackServer))
                {
                    writer.WriteElementString("pingback", "server", "http://madskills.com/public/xml/rss/module/pingback/", pingbackServer.ToString());
                    writer.WriteElementString("pingback", "target", "http://madskills.com/public/xml/rss/module/pingback/", SyndicationGenerator.GetPermaLink(publishable).ToString());
                }

                //------------------------------------------------------------
                //	Write slash syndication extension elements
                //------------------------------------------------------------
                if (post != null && post.Comments != null)
                {
                    writer.WriteElementString("slash", "comments", "http://purl.org/rss/1.0/modules/slash/", post.Comments.Count.ToString(CultureInfo.InvariantCulture));
                }
                
                //------------------------------------------------------------
                //	Write trackback syndication extension elements
                //------------------------------------------------------------
                if (post != null && post.TrackbackLink != null)
                {
                    writer.WriteElementString("trackback", "ping", "http://madskills.com/public/xml/rss/module/trackback/", post.TrackbackLink.ToString());
                }

                //------------------------------------------------------------
                //	Write well-formed web syndication extension elements
                //------------------------------------------------------------
                writer.WriteElementString("wfw", "comment", "http://wellformedweb.org/CommentAPI/", String.Concat(Utils.ConvertToAbsolute(publishable.RelativeLink).ToString(), "#comment"));
                writer.WriteElementString("wfw", "commentRss", "http://wellformedweb.org/CommentAPI/", Utils.AbsoluteWebRoot.ToString().TrimEnd('/') + "/commentfeed.axd?id=" + publishable.Id.ToString());

                //------------------------------------------------------------
                //	Write </item> element
                //------------------------------------------------------------
                writer.WriteEndElement();
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
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
        //	PRIVATE ATOM METHODS
        //============================================================
        #region WriteAtomContent(XmlWriter writer, List<IPublishable> publishables)
        /// <summary>
        /// Writes the Atom feed element information to the specified <see cref="XmlWriter"/> using the supplied collection.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="publishables">The collection of <see cref="IPublishable"/> objects used to generate syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="publishables"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomContent(XmlWriter writer, List<IPublishable> publishables)
        {
            //------------------------------------------------------------
            //	Attempt to write channel information
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }
                if (publishables == null)
                {
                    throw new ArgumentNullException("publishables");
                }

                //------------------------------------------------------------
                //	Write required feed elements
                //------------------------------------------------------------
                writer.WriteElementString("id", Utils.AbsoluteWebRoot.ToString());
                writer.WriteElementString("title", this.Settings.Name);
                writer.WriteElementString("updated", (publishables.Count > 0) ? SyndicationGenerator.ToW3cDateTime(publishables[0].DateModified.ToUniversalTime()) : SyndicationGenerator.ToW3cDateTime(DateTime.UtcNow));

                //------------------------------------------------------------
                //	Write recommended feed elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("href", Utils.AbsoluteWebRoot.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", Utils.AbsoluteWebRoot.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "alternate");
                writer.WriteAttributeString("href", Utils.FeedUrl.ToString());
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write optional feed elements
                //------------------------------------------------------------
                writer.WriteElementString("subtitle", this.Settings.Description);

                //------------------------------------------------------------
                //	Write common/shared feed elements
                //------------------------------------------------------------
                this.WriteAtomContentCommonElements(writer);

                //------------------------------------------------------------
                //	Enumerate through publishable content
                //------------------------------------------------------------
                foreach (IPublishable publishable in publishables)
                {
                    //------------------------------------------------------------
                    //	Skip publishable content if it is not visible
                    //------------------------------------------------------------
                    if (!publishable.IsVisible)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <entry> element for publishable content
                    //------------------------------------------------------------
                    this.WriteAtomEntry(writer, publishable);
                }
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
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

        #region WriteAtomContentCommonElements(XmlWriter writer)
        /// <summary>
        /// Writes the common/shared Atom feed element information to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomContentCommonElements(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write channel information
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write optional feed elements
                //------------------------------------------------------------
                writer.WriteStartElement("author");
                writer.WriteElementString("name", this.Settings.AuthorName);
                writer.WriteEndElement();

                writer.WriteStartElement("generator");
                writer.WriteAttributeString("uri", GENERATOR_URI.ToString());
                writer.WriteAttributeString("version", GENERATOR_VERSION.ToString());
                writer.WriteString(GENERATOR_NAME);
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write blogChannel syndication extension elements
                //------------------------------------------------------------
                Uri blogRoll;
                if (Uri.TryCreate(String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/opml.axd"), UriKind.RelativeOrAbsolute, out blogRoll))
                {
                    writer.WriteElementString("blogChannel", "blogRoll", "http://backend.userland.com/blogChannelModule", blogRoll.ToString());
                }

                if (!String.IsNullOrEmpty(this.Settings.Endorsement))
                {
                    Uri blink;
                    if (Uri.TryCreate(this.Settings.Endorsement, UriKind.RelativeOrAbsolute, out blink))
                    {
                        writer.WriteElementString("blogChannel", "blink", "http://backend.userland.com/blogChannelModule", blink.ToString());
                    }
                }

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Settings.AuthorName))
                {
                    writer.WriteElementString("dc", "creator", "http://purl.org/dc/elements/1.1/", this.Settings.AuthorName);
                }
                if (!String.IsNullOrEmpty(this.Settings.Description))
                {
                    writer.WriteElementString("dc", "description", "http://purl.org/dc/elements/1.1/", this.Settings.Description);
                }
                if (!String.IsNullOrEmpty(this.Settings.Language))
                {
                    writer.WriteElementString("dc", "language", "http://purl.org/dc/elements/1.1/", this.Settings.Language);
                }
                if (!String.IsNullOrEmpty(this.Settings.Name))
                {
                    writer.WriteElementString("dc", "title", "http://purl.org/dc/elements/1.1/", this.Settings.Name);
                }

                //------------------------------------------------------------
                //	Write basic geo-coding syndication extension elements
                //------------------------------------------------------------
                NumberFormatInfo decimalFormatInfo      = new NumberFormatInfo();
                decimalFormatInfo.NumberDecimalDigits   = 6;

                if (this.Settings.GeocodingLatitude != Single.MinValue)
                {
                    writer.WriteElementString("geo", "lat", "http://www.w3.org/2003/01/geo/wgs84_pos#", this.Settings.GeocodingLatitude.ToString("N", decimalFormatInfo));
                }
                if (this.Settings.GeocodingLongitude != Single.MinValue)
                {
                    writer.WriteElementString("geo", "long", "http://www.w3.org/2003/01/geo/wgs84_pos#", this.Settings.GeocodingLongitude.ToString("N", decimalFormatInfo));
                }
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
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

        #region WriteAtomEntry(XmlWriter writer, IPublishable publishable)
        /// <summary>
        /// Writes the Atom feed entry element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Page"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write feed entry element information to.</param>
        /// <param name="publishable">The <see cref="IPublishable"/> used to generate feed entry content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="publishable"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomEntry(XmlWriter writer, IPublishable publishable)
        {
            //------------------------------------------------------------
            //	Attempt to write feed entry information
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }
                if (publishable == null)
                {
                    throw new ArgumentNullException("publishable");
                }

                //------------------------------------------------------------
                //	Cast IPublishable as Post to support comments/trackback
                //------------------------------------------------------------
                Post post       = publishable as Post;

                //------------------------------------------------------------
                //	Cast IPublishable as Comment to support custom title
                //------------------------------------------------------------
                Comment comment = publishable as Comment;

                //------------------------------------------------------------
                //	Write <entry> element
                //------------------------------------------------------------
                writer.WriteStartElement("entry");

                //------------------------------------------------------------
                //	Raise serving event
                //------------------------------------------------------------                
                ServingEventArgs arg    = new ServingEventArgs(publishable.Content, ServingLocation.Feed);
                publishable.OnServing(arg);

                //------------------------------------------------------------
                //	Modify publishable content to make references absolute
                //------------------------------------------------------------
                string content  = arg.Body;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");
                content         = content.Replace("href=\"/", "href=\"" + Utils.AbsoluteWebRoot);

                //------------------------------------------------------------
                //	Modify publishable title to support comment feeds
                //------------------------------------------------------------
                string title    = publishable.Title;
                if (comment != null)
                {
                    title = String.Format(null, "{0}{1}", comment.DateCreated.ToString("MMMM d. yyyy HH:mm", CultureInfo.InvariantCulture), !String.IsNullOrEmpty(comment.Author) ? String.Concat(" by ", comment.Author) : String.Empty);
                }

                //------------------------------------------------------------
                //	Write required entry elements
                //------------------------------------------------------------
                writer.WriteElementString("id", Utils.ConvertToAbsolute(publishable.RelativeLink).ToString());
                writer.WriteElementString("title", title);
                writer.WriteElementString("updated", SyndicationGenerator.ToW3cDateTime(publishable.DateCreated.ToUniversalTime()));

                //------------------------------------------------------------
                //	Write recommended entry elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", SyndicationGenerator.GetPermaLink(publishable).ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("href", Utils.ConvertToAbsolute(publishable.RelativeLink).ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("author");
                writer.WriteElementString("name", publishable.Author);
                writer.WriteEndElement();

                writer.WriteStartElement("summary");
                writer.WriteAttributeString("type", "html");
                writer.WriteString(content);
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write optional entry elements
                //------------------------------------------------------------
                writer.WriteElementString("published", SyndicationGenerator.ToW3cDateTime(publishable.DateCreated.ToUniversalTime()));

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "related");
                writer.WriteAttributeString("href", String.Concat(Utils.ConvertToAbsolute(publishable.RelativeLink).ToString(), "#comment"));
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write entry category elements
                //------------------------------------------------------------
                if (publishable.Categories != null)
                {
                    foreach (Category category in publishable.Categories)
                    {
                        writer.WriteStartElement("category");
                        writer.WriteAttributeString("term", category.Title);
                        writer.WriteEndElement();
                    }
                }

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(publishable.Author))
                {
                    writer.WriteElementString("dc", "publisher", "http://purl.org/dc/elements/1.1/", publishable.Author);
                }
                if (!String.IsNullOrEmpty(publishable.Description))
                {
                    writer.WriteElementString("dc", "description", "http://purl.org/dc/elements/1.1/", publishable.Description);
                }

                //------------------------------------------------------------
                //	Write pingback syndication extension elements
                //------------------------------------------------------------
                Uri pingbackServer;
                if (Uri.TryCreate(String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/pingback.axd"), UriKind.RelativeOrAbsolute, out pingbackServer))
                {
                    writer.WriteElementString("pingback", "server", "http://madskills.com/public/xml/rss/module/pingback/", pingbackServer.ToString());
                    writer.WriteElementString("pingback", "target", "http://madskills.com/public/xml/rss/module/pingback/", SyndicationGenerator.GetPermaLink(publishable).ToString());
                }

                //------------------------------------------------------------
                //	Write slash syndication extension elements
                //------------------------------------------------------------
                if (post != null && post.Comments != null)
                {
                    writer.WriteElementString("slash", "comments", "http://purl.org/rss/1.0/modules/slash/", post.Comments.Count.ToString(CultureInfo.InvariantCulture));
                }

                //------------------------------------------------------------
                //	Write trackback syndication extension elements
                //------------------------------------------------------------
                if (post != null && post.TrackbackLink != null)
                {
                    writer.WriteElementString("trackback", "ping", "http://madskills.com/public/xml/rss/module/trackback/", post.TrackbackLink.ToString());
                }

                //------------------------------------------------------------
                //	Write well-formed web syndication extension elements
                //------------------------------------------------------------
                writer.WriteElementString("wfw", "comment", "http://wellformedweb.org/CommentAPI/", String.Concat(Utils.ConvertToAbsolute(publishable.RelativeLink).ToString(), "#comment"));
                writer.WriteElementString("wfw", "commentRss", "http://wellformedweb.org/CommentAPI/", Utils.AbsoluteWebRoot.ToString().TrimEnd('/') + "/commentfeed.axd?id=" + publishable.Id.ToString());

                //------------------------------------------------------------
                //	Write </entry> element
                //------------------------------------------------------------
                writer.WriteEndElement();
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
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
