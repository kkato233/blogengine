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
        #region WriteFeed(SyndicationFormat format, Stream stream, List<Comment> comments)
        /// <summary>
        /// Writes a generated syndication feed that conforms to the supplied <see cref="SyndicationFormat"/> using the supplied <see cref="Stream"/> and collection.
        /// </summary>
        /// <param name="format">A <see cref="SyndicationFormat"/> enumeration value indicating the syndication format to generate.</param>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="comments">The collection of <see cref="Comment"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="comments"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The provided <paramref name="stream"/> does not support writing.</exception>
        /// <exception cref="NotImplementedException">The provided <paramref name="format"/> has not been implemented by the <see cref="SyndicationGenerator"/>.</exception>
        public void WriteFeed(SyndicationFormat format, Stream stream, List<Comment> comments)
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
                if (comments == null)
                {
                    throw new ArgumentNullException("comments");
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
                        this.WriteAtomFeed(stream, comments);
                        break;

                    case SyndicationFormat.Rss:
                        this.WriteRssFeed(stream, comments);
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

        #region WriteFeed(SyndicationFormat format, Stream stream, List<Page> pages)
        /// <summary>
        /// Writes a generated syndication feed that conforms to the supplied <see cref="SyndicationFormat"/> using the supplied <see cref="Stream"/> and collection.
        /// </summary>
        /// <param name="format">A <see cref="SyndicationFormat"/> enumeration value indicating the syndication format to generate.</param>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="pages">The collection of <see cref="Page"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="pages"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The provided <paramref name="stream"/> does not support writing.</exception>
        /// <exception cref="NotImplementedException">The provided <paramref name="format"/> has not been implemented by the <see cref="SyndicationGenerator"/>.</exception>
        public void WriteFeed(SyndicationFormat format, Stream stream, List<Page> pages)
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
                if (pages == null)
                {
                    throw new ArgumentNullException("pages");
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
                        this.WriteAtomFeed(stream, pages);
                        break;

                    case SyndicationFormat.Rss:
                        this.WriteRssFeed(stream, pages);
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

        #region WriteFeed(SyndicationFormat format, Stream stream, List<Post> posts)
        /// <summary>
        /// Writes a generated syndication feed that conforms to the supplied <see cref="SyndicationFormat"/> using the supplied <see cref="Stream"/> and collection.
        /// </summary>
        /// <param name="format">A <see cref="SyndicationFormat"/> enumeration value indicating the syndication format to generate.</param>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="posts">The collection of <see cref="Post"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The provided <paramref name="stream"/> does not support writing.</exception>
        /// <exception cref="NotImplementedException">The provided <paramref name="format"/> has not been implemented by the <see cref="SyndicationGenerator"/>.</exception>
        public void WriteFeed(SyndicationFormat format, Stream stream, List<Post> posts)
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
                if (posts == null)
                {
                    throw new ArgumentNullException("posts");
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
                        this.WriteAtomFeed(stream, posts);
                        break;

                    case SyndicationFormat.Rss:
                        this.WriteRssFeed(stream, posts);
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

        #region WritePostCommentsFeed(SyndicationFormat format, Stream stream, Post post)
        /// <summary>
        /// Writes a generated syndication feed that conforms to the supplied <see cref="SyndicationFormat"/> using the supplied <see cref="Stream"/> and <see cref="Post"/>.
        /// </summary>
        /// <param name="format">A <see cref="SyndicationFormat"/> enumeration value indicating the syndication format to generate.</param>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="post">The <see cref="Post"/> used to generate the comments syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="post"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The provided <paramref name="stream"/> does not support writing.</exception>
        /// <exception cref="NotImplementedException">The provided <paramref name="format"/> has not been implemented by the <see cref="SyndicationGenerator"/>.</exception>
        public void WritePostCommentsFeed(SyndicationFormat format, Stream stream, Post post)
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
                if (post == null)
                {
                    throw new ArgumentNullException("post");
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
                        this.WriteAtomFeedForPostComments(stream, post);
                        break;

                    case SyndicationFormat.Rss:
                        this.WriteRssFeedForPostComments(stream, post);
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
        #region WriteAtomFeed(Stream stream, List<Comment> comments)
        /// <summary>
        /// Writes a generated Atom syndication feed to the specified <see cref="Stream"/> using the supplied collection.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="comments">The collection of <see cref="Comment"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="comments"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteAtomFeed(Stream stream, List<Comment> comments)
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
                if (comments == null)
                {
                    throw new ArgumentNullException("comments");
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
                    this.WriteAtomContent(writer, comments);

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

        #region WriteAtomFeed(Stream stream, List<Page> pages)
        /// <summary>
        /// Writes a generated Atom syndication feed to the specified <see cref="Stream"/> using the supplied collection.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="pages">The collection of <see cref="Page"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="pages"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteAtomFeed(Stream stream, List<Page> pages)
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
                if (pages == null)
                {
                    throw new ArgumentNullException("pages");
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
                    this.WriteAtomContent(writer, pages);

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

        #region WriteAtomFeed(Stream stream, List<Post> posts)
        /// <summary>
        /// Writes a generated Atom syndication feed to the specified <see cref="Stream"/> using the supplied collection.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="posts">The collection of <see cref="Post"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteAtomFeed(Stream stream, List<Post> posts)
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
                if (posts == null)
                {
                    throw new ArgumentNullException("posts");
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
                    this.WriteAtomContent(writer, posts);

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
            catch(XmlException)
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

        #region WriteAtomFeedForPostComments(Stream stream, Post post)
        /// <summary>
        /// Writes a generated Atom syndication feed to the specified <see cref="Stream"/>and <see cref="Post"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="post">The <see cref="Post"/> used to generate the comments syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="post"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteAtomFeedForPostComments(Stream stream, Post post)
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
                if (post == null)
                {
                    throw new ArgumentNullException("post");
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
                    this.WriteAtomContentForPostComments(writer, post);

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

        #region WriteRssFeed(Stream stream, List<Comment> comments)
        /// <summary>
        /// Writes a generated RSS syndication feed to the specified <see cref="Stream"/> using the supplied collection.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="comments">The collection of <see cref="Comment"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="comments"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteRssFeed(Stream stream, List<Comment> comments)
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
                if (comments == null)
                {
                    throw new ArgumentNullException("comments");
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
                    this.WriteRssChannel(writer, comments);

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

        #region WriteRssFeed(Stream stream, List<Page> pages)
        /// <summary>
        /// Writes a generated RSS syndication feed to the specified <see cref="Stream"/> using the supplied collection.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="pages">The collection of <see cref="Page"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="pages"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteRssFeed(Stream stream, List<Page> pages)
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
                if (pages == null)
                {
                    throw new ArgumentNullException("pages");
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
                    this.WriteRssChannel(writer, pages);

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

        #region WriteRssFeed(Stream stream, List<Post> posts)
        /// <summary>
        /// Writes a generated RSS syndication feed to the specified <see cref="Stream"/> using the supplied collection.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="posts">The collection of <see cref="Post"/> objects used to generate the syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteRssFeed(Stream stream, List<Post> posts)
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
                if (posts == null)
                {
                    throw new ArgumentNullException("posts");
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
                    this.WriteRssChannel(writer, posts);

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

        #region WriteRssFeedForPostComments(Stream stream, Post post)
        /// <summary>
        /// Writes a generated RSS syndication feed to the specified <see cref="Stream"/>and <see cref="Post"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which you want to write the syndication feed.</param>
        /// <param name="post">The <see cref="Post"/> used to generate the comments syndication feed content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="post"/> is a null reference (Nothing in Visual Basic).</exception>
        protected void WriteRssFeedForPostComments(Stream stream, Post post)
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
                if (post == null)
                {
                    throw new ArgumentNullException("post");
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
                    this.WriteRssChannelForPostComments(writer, post);

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
        #region WriteRssChannel(XmlWriter writer, List<Comment> comments)
        /// <summary>
        /// Writes the RSS channel element information to the specified <see cref="XmlWriter"/> using the supplied collection.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="comments">The collection of <see cref="Comment"/> objects used to generate syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="comments"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssChannel(XmlWriter writer, List<Comment> comments)
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
                if (comments == null)
                {
                    throw new ArgumentNullException("comments");
                }

                //------------------------------------------------------------
                //	Write <channel> element
                //------------------------------------------------------------
                writer.WriteStartElement("channel");

                //------------------------------------------------------------
                //	Write required channel elements
                //------------------------------------------------------------
                writer.WriteElementString("title", String.Concat(this.Settings.Name, " (Comments)"));
                writer.WriteElementString("description", this.Settings.Description);
                writer.WriteElementString("link", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=rss"));

                //------------------------------------------------------------
                //	Write common/shared channel elements
                //------------------------------------------------------------
                this.WriteRssChannelCommonElements(writer);

                //------------------------------------------------------------
                //	Enumerate through blog comments
                //------------------------------------------------------------
                foreach (Comment comment in comments)
                {
                    //------------------------------------------------------------
                    //	Skip blog comment if it has not been approved
                    //------------------------------------------------------------
                    if (!comment.Approved)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <item> element for blog comment
                    //------------------------------------------------------------
                    this.WriteRssItem(writer, comment);
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

        #region WriteRssChannel(XmlWriter writer, List<Page> pages)
        /// <summary>
        /// Writes the RSS channel element information to the specified <see cref="XmlWriter"/> using the supplied collection.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="pages">The collection of <see cref="Page"/> objects used to generate syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="pages"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssChannel(XmlWriter writer, List<Page> pages)
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
                if (pages == null)
                {
                    throw new ArgumentNullException("pages");
                }

                //------------------------------------------------------------
                //	Write <channel> element
                //------------------------------------------------------------
                writer.WriteStartElement("channel");

                //------------------------------------------------------------
                //	Write required channel elements
                //------------------------------------------------------------
                writer.WriteElementString("title", String.Concat(this.Settings.Name, " (Pages)"));
                writer.WriteElementString("description", this.Settings.Description);
                writer.WriteElementString("link", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=rss"));

                //------------------------------------------------------------
                //	Write common/shared channel elements
                //------------------------------------------------------------
                this.WriteRssChannelCommonElements(writer);

                //------------------------------------------------------------
                //	Enumerate through blog pages
                //------------------------------------------------------------
                foreach (Page page in pages)
                {
                    //------------------------------------------------------------
                    //	Skip blog page if it is not shown in the sitemap
                    //------------------------------------------------------------
                    if (!page.ShowInList)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <item> element for blog page
                    //------------------------------------------------------------
                    this.WriteRssItem(writer, page);
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

        #region WriteRssChannel(XmlWriter writer, List<Post> posts)
        /// <summary>
        /// Writes the RSS channel element information to the specified <see cref="XmlWriter"/> using the supplied collection.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="posts">The collection of <see cref="Post"/> objects used to generate syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssChannel(XmlWriter writer, List<Post> posts)
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
                if (posts == null)
                {
                    throw new ArgumentNullException("posts");
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
                writer.WriteElementString("link", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=rss"));

                //------------------------------------------------------------
                //	Write common/shared channel elements
                //------------------------------------------------------------
                this.WriteRssChannelCommonElements(writer);
                
                //------------------------------------------------------------
                //	Enumerate through blog posts
                //------------------------------------------------------------
                foreach (Post post in posts)
                {
                    //------------------------------------------------------------
                    //	Skip blog post if it does not have a published status
                    //------------------------------------------------------------
                    if (!post.IsPublished)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <item> element for blog post
                    //------------------------------------------------------------
                    this.WriteRssItem(writer, post);
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
                writer.WriteElementString("language", this.Settings.Language);

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

        #region WriteRssChannelForPostComments(XmlWriter writer, Post post)
        /// <summary>
        /// Writes the RSS channel element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Post"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="post">The <see cref="Post"/> object used to generate comments syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="post"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssChannelForPostComments(XmlWriter writer, Post post)
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
                if (post == null)
                {
                    throw new ArgumentNullException("post");
                }

                //------------------------------------------------------------
                //	Write <channel> element
                //------------------------------------------------------------
                writer.WriteStartElement("channel");

                //------------------------------------------------------------
                //	Write required channel elements
                //------------------------------------------------------------
                writer.WriteElementString("title", String.Concat(post.Title, " (Comments)"));
                writer.WriteElementString("description", String.Concat("Comments regarding: ", post.Description));
                writer.WriteElementString("link", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/commentfeed.axd"));

                //------------------------------------------------------------
                //	Write common/shared channel elements
                //------------------------------------------------------------
                this.WriteRssChannelCommonElements(writer);

                //------------------------------------------------------------
                //	Enumerate through blog posts
                //------------------------------------------------------------
                foreach (Comment comment in post.Comments)
                {
                    //------------------------------------------------------------
                    //	Skip post comment if it has not been approved
                    //------------------------------------------------------------
                    if (!comment.Approved)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <item> element for post comment
                    //------------------------------------------------------------
                    this.WriteRssItemForPostComment(writer, post, comment);
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

        #region WriteRssItem(XmlWriter writer, Comment comment)
        /// <summary>
        /// Writes the RSS channel item element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Comment"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel item element information to.</param>
        /// <param name="comment">The <see cref="Comment"/> used to generate channel item content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="comment"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssItem(XmlWriter writer, Comment comment)
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
                if (comment == null)
                {
                    throw new ArgumentNullException("comment");
                }

                //------------------------------------------------------------
                //	Write <item> element
                //------------------------------------------------------------
                writer.WriteStartElement("item");

                //------------------------------------------------------------
                //	Modify comment content to make references absolute
                //------------------------------------------------------------
                string content  = comment.Content;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");

                //------------------------------------------------------------
                //	Write required channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("title", String.Format(null, "{0}{1}", comment.DateCreated.ToString("MMMM d. yyyy HH:mm", CultureInfo.InvariantCulture), !String.IsNullOrEmpty(comment.Author) ? String.Concat(" by ", comment.Author) : String.Empty));
                writer.WriteElementString("description", content);
                writer.WriteElementString("link", String.Concat(comment.Post.AbsoluteLink.ToString(), "#comments"));

                //------------------------------------------------------------
                //	Write optional channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("author", comment.Author);
                writer.WriteElementString("guid", comment.Post.PermaLink.ToString());
                writer.WriteElementString("pubDate", SyndicationGenerator.ToRfc822DateTime(comment.DateCreated));

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(comment.Author))
                {
                    writer.WriteElementString("dc", "creator", "http://purl.org/dc/elements/1.1/", comment.Author);
                }

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

        #region WriteRssItem(XmlWriter writer, Page page)
        /// <summary>
        /// Writes the RSS channel item element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Page"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel item element information to.</param>
        /// <param name="page">The <see cref="Page"/> used to generate channel item content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="page"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssItem(XmlWriter writer, Page page)
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
                if (page == null)
                {
                    throw new ArgumentNullException("page");
                }

                //------------------------------------------------------------
                //	Write <item> element
                //------------------------------------------------------------
                writer.WriteStartElement("item");

                //------------------------------------------------------------
                //	Modify post content to make references absolute
                //------------------------------------------------------------                
                ServingEventArgs arg = new ServingEventArgs(page.Content);
                Page.OnServing(page, arg);
                string content = arg.Body;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");
                content         = content.Replace("href=\"/", "href=\"" + Utils.AbsoluteWebRoot);

                //------------------------------------------------------------
                //	Write required channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("title", page.Title);
                writer.WriteElementString("description", content);
                writer.WriteElementString("link", page.AbsoluteLink.ToString());

                //------------------------------------------------------------
                //	Write optional channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("pubDate", SyndicationGenerator.ToRfc822DateTime(page.DateCreated));

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(page.Description))
                {
                    writer.WriteElementString("dc", "description", "http://purl.org/dc/elements/1.1/", page.Description);
                }

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

        #region WriteRssItem(XmlWriter writer, Post post)
        /// <summary>
        /// Writes the RSS channel item element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Post"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel item element information to.</param>
        /// <param name="post">The <see cref="Post"/> used to generate channel item content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="post"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssItem(XmlWriter writer, Post post)
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
                if (post == null)
                {
                    throw new ArgumentNullException("post");
                }

                //------------------------------------------------------------
                //	Write <item> element
                //------------------------------------------------------------
                writer.WriteStartElement("item");

                //------------------------------------------------------------
                //	Modify post content to make references absolute
                //------------------------------------------------------------
                ServingEventArgs arg = new ServingEventArgs(post.Content);
                Post.OnServing(post, arg);
                string content = arg.Body;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");
                content         = content.Replace("href=\"/", "href=\"" + Utils.AbsoluteWebRoot);

                //------------------------------------------------------------
                //	Write required channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("title", post.Title);
                writer.WriteElementString("description", content);
                writer.WriteElementString("link", post.AbsoluteLink.ToString());

                //------------------------------------------------------------
                //	Write optional channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("author", post.Author);
                writer.WriteElementString("comments", String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                writer.WriteElementString("guid", post.PermaLink.ToString());
                writer.WriteElementString("pubDate", SyndicationGenerator.ToRfc822DateTime(post.DateCreated));

                //------------------------------------------------------------
                //	Write channel item category elements
                //------------------------------------------------------------
                foreach (Category category in post.Categories)
                {
                    writer.WriteElementString("category", category.Title);
                }

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(post.Author))
                {
                    writer.WriteElementString("dc", "creator", "http://purl.org/dc/elements/1.1/", post.Author);
                }
                if (!String.IsNullOrEmpty(post.Description))
                {
                    writer.WriteElementString("dc", "description", "http://purl.org/dc/elements/1.1/", post.Description);
                }

                //------------------------------------------------------------
                //	Write pingback syndication extension elements
                //------------------------------------------------------------
                Uri pingbackServer;
                if (Uri.TryCreate(String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/pingback.axd"), UriKind.RelativeOrAbsolute, out pingbackServer))
                {
                    writer.WriteElementString("pingback", "server", "http://madskills.com/public/xml/rss/module/pingback/", pingbackServer.ToString());
                    writer.WriteElementString("pingback", "target", "http://madskills.com/public/xml/rss/module/pingback/", post.PermaLink.ToString());
                }

                //------------------------------------------------------------
                //	Write slash syndication extension elements
                //------------------------------------------------------------
                if(post.Comments != null)
                {
                    writer.WriteElementString("slash", "comments", "http://purl.org/rss/1.0/modules/slash/", post.Comments.Count.ToString(CultureInfo.InvariantCulture));
                }

                //------------------------------------------------------------
                //	Write trackback syndication extension elements
                //------------------------------------------------------------
                if (post.TrackbackLink != null)
                {
                    writer.WriteElementString("trackback", "ping", "http://madskills.com/public/xml/rss/module/trackback/", post.TrackbackLink.ToString());
                }

                //------------------------------------------------------------
                //	Write well-formed web syndication extension elements
                //------------------------------------------------------------
                writer.WriteElementString("wfw", "comment", "http://wellformedweb.org/CommentAPI/", String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                writer.WriteElementString("wfw", "commentRss", "http://wellformedweb.org/CommentAPI/", Utils.AbsoluteWebRoot.ToString().TrimEnd('/') + "/commentfeed.axd?id=" + post.Id.ToString());

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

        #region WriteRssItemForPostComment(XmlWriter writer, Post post, Comment comment)
        /// <summary>
        /// Writes the RSS channel item element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Post"/> and <see cref="Comment"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel item element information to.</param>
        /// <param name="post">The <see cref="Post"/> used to generate channel item content.</param>
        /// <param name="comment">The <see cref="Comment"/> used to generate channel item content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="post"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteRssItemForPostComment(XmlWriter writer, Post post, Comment comment)
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
                if (post == null)
                {
                    throw new ArgumentNullException("post");
                }
                if (comment == null)
                {
                    throw new ArgumentNullException("comment");
                }

                //------------------------------------------------------------
                //	Write <item> element
                //------------------------------------------------------------
                writer.WriteStartElement("item");

                //------------------------------------------------------------
                //	Modify comment content to make references absolute
                //------------------------------------------------------------
                string content  = comment.Content;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");

                //------------------------------------------------------------
                //	Write required channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("title", String.Format(null, "{0}{1}", comment.DateCreated.ToString("MMMM d. yyyy HH:mm", CultureInfo.InvariantCulture), !String.IsNullOrEmpty(comment.Author) ? String.Concat(" by ", comment.Author) : String.Empty));
                writer.WriteElementString("description", content);
                writer.WriteElementString("link", String.Concat(post.AbsoluteLink.ToString(), "#comments"));

                //------------------------------------------------------------
                //	Write optional channel item elements
                //------------------------------------------------------------
                writer.WriteElementString("author", comment.Author);
                writer.WriteElementString("guid", String.Concat(post.PermaLink.ToString(), "#comments"));
                writer.WriteElementString("pubDate", SyndicationGenerator.ToRfc822DateTime(comment.DateCreated));

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
        #region WriteAtomContent(XmlWriter writer, List<Comment> comments)
        /// <summary>
        /// Writes the Atom feed element information to the specified <see cref="XmlWriter"/> using the supplied collection.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="comments">The collection of <see cref="Comment"/> objects used to generate syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="comments"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomContent(XmlWriter writer, List<Comment> comments)
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
                if (comments == null)
                {
                    throw new ArgumentNullException("comments");
                }

                //------------------------------------------------------------
                //	Write required feed elements
                //------------------------------------------------------------
                writer.WriteElementString("id", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=atom"));
                writer.WriteElementString("title", String.Concat(this.Settings.Name, " (Comments)"));
                writer.WriteElementString("updated", (comments.Count > 0) ? SyndicationGenerator.ToW3cDateTime(comments[0].Post.DateModified.ToUniversalTime()) : SyndicationGenerator.ToW3cDateTime(DateTime.UtcNow));

                //------------------------------------------------------------
                //	Write recommended feed elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=atom"));
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "related");
                writer.WriteAttributeString("href", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=atom"));
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
                //	Enumerate through blog comments
                //------------------------------------------------------------
                foreach (Comment comment in comments)
                {
                    //------------------------------------------------------------
                    //	Skip blog comment if it is not approved
                    //------------------------------------------------------------
                    if (!comment.Approved)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <entry> element for blog comment
                    //------------------------------------------------------------
                    this.WriteAtomEntry(writer, comment);
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

        #region WriteAtomContent(XmlWriter writer, List<Page> pages)
        /// <summary>
        /// Writes the Atom feed element information to the specified <see cref="XmlWriter"/> using the supplied collection.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="pages">The collection of <see cref="Page"/> objects used to generate syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="pages"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomContent(XmlWriter writer, List<Page> pages)
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
                if (pages == null)
                {
                    throw new ArgumentNullException("pages");
                }

                //------------------------------------------------------------
                //	Write required feed elements
                //------------------------------------------------------------
                writer.WriteElementString("id", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=atom"));
                writer.WriteElementString("title", String.Concat(this.Settings.Name, " (Pages)"));
                writer.WriteElementString("updated", (pages.Count > 0) ? SyndicationGenerator.ToW3cDateTime(pages[0].DateModified.ToUniversalTime()) : SyndicationGenerator.ToW3cDateTime(DateTime.UtcNow));

                //------------------------------------------------------------
                //	Write recommended feed elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=atom"));
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "related");
                writer.WriteAttributeString("href", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=atom"));
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
                //	Enumerate through blog pages
                //------------------------------------------------------------
                foreach (Page page in pages)
                {
                    //------------------------------------------------------------
                    //	Skip blog comment if it is not approved
                    //------------------------------------------------------------
                    if (!page.ShowInList)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <entry> element for blog page
                    //------------------------------------------------------------
                    this.WriteAtomEntry(writer, page);
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

        #region WriteAtomContent(XmlWriter writer, List<Post> posts)
        /// <summary>
        /// Writes the Atom feed element information to the specified <see cref="XmlWriter"/> using the supplied collection.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="posts">The collection of <see cref="Post"/> objects used to generate syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomContent(XmlWriter writer, List<Post> posts)
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
                if (posts == null)
                {
                    throw new ArgumentNullException("posts");
                }

                //------------------------------------------------------------
                //	Write required feed elements
                //------------------------------------------------------------
                writer.WriteElementString("id", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=atom"));
                writer.WriteElementString("title", this.Settings.Name);
                writer.WriteElementString("updated", (posts.Count > 0) ? SyndicationGenerator.ToW3cDateTime(posts[0].DateModified.ToUniversalTime()) : SyndicationGenerator.ToW3cDateTime(DateTime.UtcNow));

                //------------------------------------------------------------
                //	Write recommended feed elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=atom"));
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "related");
                writer.WriteAttributeString("href", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/syndication.axd?format=atom"));
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
                //	Enumerate through blog posts
                //------------------------------------------------------------
                foreach (Post post in posts)
                {
                    //------------------------------------------------------------
                    //	Skip blog post if it does not have a published status
                    //------------------------------------------------------------
                    if (!post.IsPublished)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <entry> element for blog post
                    //------------------------------------------------------------
                    this.WriteAtomEntry(writer, post);
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

        #region WriteAtomContentForPostComments(XmlWriter writer, Post post)
        /// <summary>
        /// Writes the Atom feed information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Post"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write channel element information to.</param>
        /// <param name="post">The <see cref="Post"/> object used to generate comments syndication content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="post"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomContentForPostComments(XmlWriter writer, Post post)
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
                if (post == null)
                {
                    throw new ArgumentNullException("post");
                }

                //------------------------------------------------------------
                //	Write required feed elements
                //------------------------------------------------------------
                writer.WriteElementString("id", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/commentfeed.axd"));
                writer.WriteElementString("title", String.Concat(post.Title, " (Comments)"));
                writer.WriteElementString("updated", SyndicationGenerator.ToW3cDateTime(post.DateModified.ToUniversalTime()));

                //------------------------------------------------------------
                //	Write recommended feed elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/commentfeed.axd"));
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "related");
                writer.WriteAttributeString("href", String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/commentfeed.axd"));
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write optional feed elements
                //------------------------------------------------------------
                writer.WriteElementString("subtitle", String.Concat("Comments regarding: ", post.Description));

                //------------------------------------------------------------
                //	Write common/shared feed elements
                //------------------------------------------------------------
                this.WriteAtomContentCommonElements(writer);

                //------------------------------------------------------------
                //	Enumerate through blog posts
                //------------------------------------------------------------
                foreach (Comment comment in post.Comments)
                {
                    //------------------------------------------------------------
                    //	Skip post comment if it has not been approved
                    //------------------------------------------------------------
                    if (!comment.Approved)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Write <entry> element for post comment
                    //------------------------------------------------------------
                    this.WriteAtomEntryForPostComment(writer, post, comment);
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

        #region WriteAtomEntry(XmlWriter writer, Comment comment)
        /// <summary>
        /// Writes the Atom feed entry element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Comment"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write feed entry element information to.</param>
        /// <param name="comment">The <see cref="Comment"/> used to generate feed entry content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="comment"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomEntry(XmlWriter writer, Comment comment)
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
                if (comment == null)
                {
                    throw new ArgumentNullException("comment");
                }

                //------------------------------------------------------------
                //	Write <entry> element
                //------------------------------------------------------------
                writer.WriteStartElement("entry");

                //------------------------------------------------------------
                //	Modify post content to make references absolute
                //------------------------------------------------------------
                string content  = comment.Content;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");

                //------------------------------------------------------------
                //	Write required entry elements
                //------------------------------------------------------------
                writer.WriteElementString("id", String.Concat(comment.Post.AbsoluteLink.ToString(), "#comments"));
                writer.WriteElementString("title", String.Format(null, "{0}{1}", comment.DateCreated.ToString("MMMM d. yyyy HH:mm", CultureInfo.InvariantCulture), !String.IsNullOrEmpty(comment.Author) ? String.Concat(" by ", comment.Author) : String.Empty));
                writer.WriteElementString("updated", SyndicationGenerator.ToW3cDateTime(comment.DateCreated.ToUniversalTime()));

                //------------------------------------------------------------
                //	Write recommended entry elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", comment.Post.PermaLink.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("href", comment.Post.AbsoluteLink.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("author");
                writer.WriteElementString("name", comment.Author);
                writer.WriteElementString("uri", comment.Website.ToString());
                writer.WriteElementString("email", comment.Email);
                writer.WriteEndElement();

                writer.WriteStartElement("summary");
                writer.WriteAttributeString("type", "html");
                writer.WriteString(content);
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write optional entry elements
                //------------------------------------------------------------
                writer.WriteElementString("published", SyndicationGenerator.ToW3cDateTime(comment.DateCreated.ToUniversalTime()));

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "related");
                writer.WriteAttributeString("href", String.Concat(comment.Post.AbsoluteLink.ToString(), "#comments"));
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(comment.Author))
                {
                    writer.WriteElementString("dc", "creator", "http://purl.org/dc/elements/1.1/", comment.Author);
                }

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

        #region WriteAtomEntry(XmlWriter writer, Page page)
        /// <summary>
        /// Writes the Atom feed entry element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Page"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write feed entry element information to.</param>
        /// <param name="page">The <see cref="Page"/> used to generate feed entry content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="page"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomEntry(XmlWriter writer, Page page)
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
                if (page == null)
                {
                    throw new ArgumentNullException("page");
                }

                //------------------------------------------------------------
                //	Write <entry> element
                //------------------------------------------------------------
                writer.WriteStartElement("entry");

                //------------------------------------------------------------
                //	Modify post content to make references absolute
                //------------------------------------------------------------
                string content  = page.Content;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");

                //------------------------------------------------------------
                //	Write required entry elements
                //------------------------------------------------------------
                writer.WriteElementString("id", page.AbsoluteLink.ToString());
                writer.WriteElementString("title", page.Title);
                writer.WriteElementString("updated", SyndicationGenerator.ToW3cDateTime(page.DateCreated.ToUniversalTime()));

                //------------------------------------------------------------
                //	Write recommended entry elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", page.AbsoluteLink.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("href", page.AbsoluteLink.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("summary");
                writer.WriteAttributeString("type", "html");
                writer.WriteString(content);
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write optional entry elements
                //------------------------------------------------------------
                writer.WriteElementString("published", SyndicationGenerator.ToW3cDateTime(page.DateCreated.ToUniversalTime()));

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(page.Description))
                {
                    writer.WriteElementString("dc", "description", "http://purl.org/dc/elements/1.1/", page.Description);
                }

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

        #region WriteAtomEntry(XmlWriter writer, Post post)
        /// <summary>
        /// Writes the Atom feed entry element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Post"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write feed entry element information to.</param>
        /// <param name="post">The <see cref="Post"/> used to generate feed entry content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="post"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomEntry(XmlWriter writer, Post post)
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
                if (post == null)
                {
                    throw new ArgumentNullException("post");
                }

                //------------------------------------------------------------
                //	Write <entry> element
                //------------------------------------------------------------
                writer.WriteStartElement("entry");

                //------------------------------------------------------------
                //	Modify post content to make references absolute
                //------------------------------------------------------------
                string content  = post.Content;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");

                //------------------------------------------------------------
                //	Write required entry elements
                //------------------------------------------------------------
                writer.WriteElementString("id", post.PermaLink.ToString());
                writer.WriteElementString("title", post.Title);
                writer.WriteElementString("updated", SyndicationGenerator.ToW3cDateTime(post.DateCreated.ToUniversalTime()));

                //------------------------------------------------------------
                //	Write recommended entry elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", post.PermaLink.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("href", post.AbsoluteLink.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("author");
                writer.WriteElementString("name", post.Author);
                writer.WriteEndElement();

                writer.WriteStartElement("summary");
                writer.WriteAttributeString("type", "html");
                writer.WriteString(content);
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write optional entry elements
                //------------------------------------------------------------
                writer.WriteElementString("published", SyndicationGenerator.ToW3cDateTime(post.DateCreated.ToUniversalTime()));

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "related");
                writer.WriteAttributeString("href", String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write entry category elements
                //------------------------------------------------------------
                foreach (Category category in post.Categories)
                {
                    writer.WriteStartElement("category");
                    writer.WriteAttributeString("term", category.Title);
                    writer.WriteEndElement();
                }

                //------------------------------------------------------------
                //	Write Dublin Core syndication extension elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(post.Author))
                {
                    writer.WriteElementString("dc", "creator", "http://purl.org/dc/elements/1.1/", post.Author);
                }
                if (!String.IsNullOrEmpty(post.Description))
                {
                    writer.WriteElementString("dc", "description", "http://purl.org/dc/elements/1.1/", post.Description);
                }

                //------------------------------------------------------------
                //	Write pingback syndication extension elements
                //------------------------------------------------------------
                Uri pingbackServer;
                if (Uri.TryCreate(String.Concat(Utils.AbsoluteWebRoot.ToString().TrimEnd('/'), "/pingback.axd"), UriKind.RelativeOrAbsolute, out pingbackServer))
                {
                    writer.WriteElementString("pingback", "server", "http://madskills.com/public/xml/rss/module/pingback/", pingbackServer.ToString());
                    writer.WriteElementString("pingback", "target", "http://madskills.com/public/xml/rss/module/pingback/", post.PermaLink.ToString());
                }

                //------------------------------------------------------------
                //	Write slash syndication extension elements
                //------------------------------------------------------------
                if(post.Comments != null)
                {
                    writer.WriteElementString("slash", "comments", "http://purl.org/rss/1.0/modules/slash/", post.Comments.Count.ToString(CultureInfo.InvariantCulture));
                }

                //------------------------------------------------------------
                //	Write trackback syndication extension elements
                //------------------------------------------------------------
                if (post.TrackbackLink != null)
                {
                    writer.WriteElementString("trackback", "ping", "http://madskills.com/public/xml/rss/module/trackback/", post.TrackbackLink.ToString());
                }

                //------------------------------------------------------------
                //	Write well-formed web syndication extension elements
                //------------------------------------------------------------
                writer.WriteElementString("wfw", "comment", "http://wellformedweb.org/CommentAPI/", String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                writer.WriteElementString("wfw", "commentRss", "http://wellformedweb.org/CommentAPI/", Utils.AbsoluteWebRoot.ToString().TrimEnd('/') + "/commentfeed.axd?id=" + post.Id.ToString());

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

        #region WriteAtomEntryForPostComment(XmlWriter writer, Post post, Comment comment)
        /// <summary>
        /// Writes the Atom entry element information to the specified <see cref="XmlWriter"/> using the supplied <see cref="Post"/> and <see cref="Comment"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write entry element information to.</param>
        /// <param name="post">The <see cref="Post"/> used to generate entry content.</param>
        /// <param name="comment">The <see cref="Comment"/> used to generate entry content.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="post"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteAtomEntryForPostComment(XmlWriter writer, Post post, Comment comment)
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
                if (post == null)
                {
                    throw new ArgumentNullException("post");
                }
                if (comment == null)
                {
                    throw new ArgumentNullException("comment");
                }

                //------------------------------------------------------------
                //	Write <entry> element
                //------------------------------------------------------------
                writer.WriteStartElement("entry");

                //------------------------------------------------------------
                //	Modify comment content to make references absolute
                //------------------------------------------------------------
                string content  = comment.Content;
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "image.axd", "\"" + Utils.AbsoluteWebRoot + "image.axd");
                content         = content.Replace("\"" + Utils.AbsoluteWebRoot.AbsolutePath + "file.axd", "\"" + Utils.AbsoluteWebRoot + "file.axd");

                //------------------------------------------------------------
                //	Write required entry elements
                //------------------------------------------------------------
                writer.WriteElementString("id", String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                writer.WriteElementString("title", String.Format(null, "{0}{1}", comment.DateCreated.ToString("MMMM d. yyyy HH:mm", CultureInfo.InvariantCulture), !String.IsNullOrEmpty(comment.Author) ? String.Concat(" by ", comment.Author) : String.Empty));
                writer.WriteElementString("updated", SyndicationGenerator.ToW3cDateTime(comment.DateCreated.ToUniversalTime()));

                //------------------------------------------------------------
                //	Write recommended entry elements
                //------------------------------------------------------------
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("href", String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                writer.WriteEndElement();

                writer.WriteStartElement("author");
                writer.WriteElementString("name", comment.Author);
                writer.WriteEndElement();

                writer.WriteStartElement("summary");
                writer.WriteAttributeString("type", "html");
                writer.WriteString(content);
                writer.WriteEndElement();

                //------------------------------------------------------------
                //	Write optional entry elements
                //------------------------------------------------------------
                writer.WriteElementString("published", SyndicationGenerator.ToW3cDateTime(comment.DateCreated.ToUniversalTime()));

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "related");
                writer.WriteAttributeString("href", post.AbsoluteLink.ToString());
                writer.WriteEndElement();

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
