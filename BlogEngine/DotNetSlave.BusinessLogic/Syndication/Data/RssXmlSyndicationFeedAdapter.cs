/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/16/2007	brian.kuhn		Created RssXmlSyndicationFeedAdapter Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

using BlogEngine.Core.Properties;
using DotNetSlave.BlogEngine.BusinessLogic.Syndication.Rss;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Data
{
    /// <summary>
    /// Provides a set of methods and properties used to fill or write <see cref="RssFeed"/> instances from an XML data source.
    /// </summary>
    public class RssXmlSyndicationFeedAdapter : XmlSyndicationFeedAdapter
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the Atom 1.0 XML namespace designator.
        /// </summary>
        private const string RSS_XML_NAMESPACE  = "http://www.rssboard.org/rss-specification";
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssXmlSyndicationFeedAdapter()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssXmlSyndicationFeedAdapter"/> class.
        /// </summary>
        public RssXmlSyndicationFeedAdapter() : base()
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Initialization handled by base class
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

        #region RssXmlSyndicationFeedAdapter(XmlReader reader)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssXmlSyndicationFeedAdapter"/> class using the supplied <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">A reader that provides fast, non-cached, forward-only access to syndication feed XML data.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssXmlSyndicationFeedAdapter(XmlReader reader) : base(reader)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Initialization handled by base class
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

        #region RssXmlSyndicationFeedAdapter(XmlReader reader, SyndicationFeedSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssXmlSyndicationFeedAdapter"/> class using the supplied <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">A reader that provides fast, non-cached, forward-only access to syndication feed XML data.</param>
        /// <param name="settings">The set of features that the XML data adapter supports.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssXmlSyndicationFeedAdapter(XmlReader reader, SyndicationFeedSettings settings) : base(reader, settings)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Initialization handled by base class
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

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region DefaultXmlNamespace
        /// <summary>
        /// Gets the default XML namespace this syndication feed adapter may utilize to parse XML data.
        /// </summary>
        /// <remarks>The RSS specification does not implement a default XML namespace.</remarks>
        public static string DefaultXmlNamespace
        {
            get
            {
                return RSS_XML_NAMESPACE;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region Fill(SyndicationFeed feed)
        /// <summary>
        /// Adds or refreshes items/entries in the <see cref="RssFeed"/> to match those in the XML data source.
        /// </summary>
        /// <param name="feed">A <see cref="RssFeed"/> to fill using the underlying XML data source.</param>
        /// <returns>The number of items/entries successfully added to or refreshed in the <b>RssFeed</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Fill</b> retrieves RSS syndication feed information from the XML data source.
        ///     </para>
        /// 
        ///     <para>
        ///         The <b>Fill</b> operation then sets the <b>RssFeed</b> properties and adds items to the feed, creating the RSS syndication feed entities if they do not already exist.
        ///     </para>
        /// 
        ///     <para>
        ///         If the <b>RssXmlSyndicationFeedAdapter</b> will also add supported extensions to the <b>RssFeed</b> using the supported extensions configured in the <see cref="XmlSyndicationFeedAdapter.Settings"/> property.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <see cref="XmlSyndicationFeedAdapter.Reader"/> property is a null reference (Nothing in Visual Basic) -or- the <paramref name="feed"/> is not a <see cref="RssFeed"/>.</exception>
        /// <exception cref="XmlException">An exception occurred while parsing the syndication feed XML data.</exception>
        public override int Fill(SyndicationFeed feed)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            int modifiedItemsCount  = 0;

            //------------------------------------------------------------
            //	Attempt to fill syndication feed using data source
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (feed == null)
                {
                    throw new ArgumentNullException("feed");
                }
                if(this.Reader == null)
                {
                    throw new ArgumentException(Resources.ExceptionXmlSyndicationFeedAdapterReaderEmpty);
                }
                if(feed.GetType() != typeof(RssFeed))
                {
                    throw new ArgumentException(Resources.ExceptionRssSyndicationFeedAdapterInvalidFeedType);
                }

                //------------------------------------------------------------
                //	Instantiate the syndication feed using XML data source
                //------------------------------------------------------------
                modifiedItemsCount  = RssXmlSyndicationFeedAdapter.FillFeed((RssFeed)feed, new XPathDocument(this.Reader));
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
            return modifiedItemsCount;
        }
        #endregion

        #region Write(SyndicationFeed feed, XmlWriter writer)
        /// <summary>
        /// Writes the <see cref="SyndicationFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="feed">The <see cref="SyndicationFeed"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="feed"/> is not of type <see cref="RssFeed"/>.</exception>
        /// <exception cref="XmlException">An exception occurred while writing the syndication feed XML data.</exception>
        public override void Write(SyndicationFeed feed, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            XmlWriterType writerType    = XmlWriterType.Standard;

            //------------------------------------------------------------
            //	Attempt to write syndication feed to writer
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (feed == null)
                {
                    throw new ArgumentNullException("feed");
                }
                if(feed.GetType() != typeof(RssFeed))
                {
                    throw new ArgumentException(Resources.ExceptionRssSyndicationFeedAdapterInvalidFeedType, "feed");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write the syndication feed to the writer
                //------------------------------------------------------------
                RssXmlSyndicationFeedAdapter.WriteFeed((RssFeed)feed, writer, writerType);
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

        #region Write(SyndicationFeed feed, XmlWriter writer, XmlWriterType writerType)
        /// <summary>
        /// Writes the <see cref="SyndicationFeed"/> to the specified <see cref="XmlWriter"/> using the supplied <see cref="XmlWriterType"/>.
        /// </summary>
        /// <param name="feed">The <see cref="SyndicationFeed"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is written to.</param>
        /// <param name="writerType">A <see cref="XmlWriterType"/> enumeration value indicating the source/type of the <b>XmlWriter</b>.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="feed"/> is not of type <see cref="RssFeed"/> -or- the <paramref name="writerType"/> is <b>XmlWriterType.None</b>.</exception>
        /// <exception cref="XmlException">An exception occurred while writing the syndication feed XML data.</exception>
        public override void Write(SyndicationFeed feed, XmlWriter writer, XmlWriterType writerType)
        {
            //------------------------------------------------------------
            //	Attempt to write syndication feed to writer
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (feed == null)
                {
                    throw new ArgumentNullException("feed");
                }
                if (feed.GetType() != typeof(RssFeed))
                {
                    throw new ArgumentException(Resources.ExceptionRssSyndicationFeedAdapterInvalidFeedType, "feed");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }
                if (writerType == XmlWriterType.None)
                {
                    throw new ArgumentException(Resources.ExceptionSyndicationFeedAdapterInvalidWriterType, "writerType");
                }

                //------------------------------------------------------------
                //	Write the syndication feed to the writer
                //------------------------------------------------------------
                RssXmlSyndicationFeedAdapter.WriteFeed((RssFeed)feed, writer, writerType);
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
        //	PRIVATE FILL ROUTINES
        //============================================================
        #region FillFeed(RssFeed feed, XPathDocument xpathDocument)
        /// <summary>
        /// Instantiates the specified <see cref="RssFeed"/> using the configured settings and XML data source.
        /// </summary>
        /// <param name="feed">The syndication feed to instantiate.</param>
        /// <param name="document">A fast, read-only, in-memory representation of the syndication XML data using the XPath data model.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="document"/> is a null reference (Nothing in Visual Basic).</exception>
        private static int FillFeed(RssFeed feed, XPathDocument document)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            XPathNavigator navigator    = null;

            //------------------------------------------------------------
            //	Attempt to instantiate feed using XML data source
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (feed == null)
                {
                    throw new ArgumentNullException("feed");
                }
                if (document == null)
                {
                    throw new ArgumentNullException("document");
                }

                //------------------------------------------------------------
                //	Create navigator against XML data
                //------------------------------------------------------------
                navigator   = document.CreateNavigator();

                //------------------------------------------------------------
                //	Extract root <rss> node
                //------------------------------------------------------------
                XPathNavigator rssRoot  = navigator.SelectSingleNode("rss");

                //------------------------------------------------------------
                //	Verify root node found
                //------------------------------------------------------------
                if (rssRoot != null)
                {
                    //------------------------------------------------------------
                    //	Get syndication feed version
                    //------------------------------------------------------------
                    string feedVersion      = rssRoot.GetAttribute("version", String.Empty);
                    if (!String.IsNullOrEmpty(feedVersion))
                    {
                        feed.Version        = feedVersion;
                    }
                }

                //------------------------------------------------------------
                //	Extract <channel> node
                //------------------------------------------------------------
                XPathNavigator rssChannel   = navigator.SelectSingleNode("rss/channel");

                //------------------------------------------------------------
                //	Verify channel node found
                //------------------------------------------------------------
                if (rssChannel != null)
                {
                    //------------------------------------------------------------
                    //	Set feed channel properties
                    //------------------------------------------------------------
                    RssXmlSyndicationFeedAdapter.ModifyChannel(feed.Channel, rssChannel);
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }
            finally
            {
                //------------------------------------------------------------
                //	Release resources
                //------------------------------------------------------------
                if (navigator != null)
                {
                    navigator = null;
                }
            }

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return feed.Channel.Items.Count;
        }
        #endregion

        #region ModifyChannel(RssChannel channel, XPathNavigator xmlSource)
        /// <summary>
        /// Updates the <see cref="RssChannel"/> using information extracted from the provided <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="channel">The <see cref="RssChannel"/> to modify.</param>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract RSS channel information from.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="channel"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void ModifyChannel(RssChannel channel, XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Attempt to update channel state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (channel == null)
                {
                    throw new ArgumentNullException("channel");
                }
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract <title> node
                //------------------------------------------------------------
                XPathNavigator channelTitle = navigator.SelectSingleNode("title");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelTitle))
                {
                    channel.Title           = channelTitle.Value;
                }

                //------------------------------------------------------------
                //	Extract <description> node
                //------------------------------------------------------------
                XPathNavigator channelDescription   = navigator.SelectSingleNode("description");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelDescription))
                {
                    channel.Description     = channelDescription.Value;
                }

                //------------------------------------------------------------
                //	Extract <link> node
                //------------------------------------------------------------
                XPathNavigator channelLink  = navigator.SelectSingleNode("link");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelLink))
                {
                    Uri linkUri;
                    if (Uri.TryCreate(channelLink.Value, UriKind.RelativeOrAbsolute, out linkUri))
                    {
                        channel.Link        = linkUri;
                    }
                }

                //------------------------------------------------------------
                //	Extract channel optional elements
                //------------------------------------------------------------
                RssXmlSyndicationFeedAdapter.ModifyChannelOptionals(channel, navigator);

                //------------------------------------------------------------
                //	Extract <category> nodes
                //------------------------------------------------------------
                XPathNodeIterator channelCategories = navigator.Select("category");
                if (RssXmlSyndicationFeedAdapter.IteratorContainsNodes(channelCategories))
                {
                    RssXmlSyndicationFeedAdapter.UpdateCategories(channel.Categories, channelCategories);
                }

                //------------------------------------------------------------
                //	Extract <item> nodes
                //------------------------------------------------------------
                XPathNodeIterator channelItems = navigator.Select("item");
                if (RssXmlSyndicationFeedAdapter.IteratorContainsNodes(channelItems))
                {
                    RssXmlSyndicationFeedAdapter.UpdateItems(channel.Items, channelItems);
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

        #region ModifyChannelOptionals(RssChannel channel, XPathNavigator xmlSource)
        /// <summary>
        /// Updates the <see cref="RssChannel"/> optional elements using information extracted from the provided <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="channel">The <see cref="RssChannel"/> to modify.</param>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract RSS channel information from.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="channel"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void ModifyChannelOptionals(RssChannel channel, XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Attempt to update channel state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (channel == null)
                {
                    throw new ArgumentNullException("channel");
                }
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract <cloud> node
                //------------------------------------------------------------
                XPathNavigator channelCloud = navigator.SelectSingleNode("cloud");
                if (RssXmlSyndicationFeedAdapter.NavigatorHasAttributes(channelCloud))
                {
                    RssCloud cloud          = RssXmlSyndicationFeedAdapter.CreateCloud(channelCloud);
                    if (cloud != null)
                    {
                        channel.Cloud       = cloud;
                    }
                }

                //------------------------------------------------------------
                //	Extract <copyright> node
                //------------------------------------------------------------
                XPathNavigator channelCopyright = navigator.SelectSingleNode("copyright");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelCopyright))
                {
                    channel.Copyright       = channelCopyright.Value;
                }

                //------------------------------------------------------------
                //	Extract <generator> node
                //------------------------------------------------------------
                XPathNavigator channelGenerator = navigator.SelectSingleNode("generator");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelGenerator))
                {
                    channel.Generator       = channelGenerator.Value;
                }

                //------------------------------------------------------------
                //	Extract <image> node
                //------------------------------------------------------------
                XPathNavigator channelImage = navigator.SelectSingleNode("image");
                if (RssXmlSyndicationFeedAdapter.NavigatorContainsChildren(channelImage))
                {
                    RssImage image          = RssXmlSyndicationFeedAdapter.CreateImage(channelImage);
                    if (image != null)
                    {
                        channel.Image       = image;
                    }
                }

                //------------------------------------------------------------
                //	Extract <language> node
                //------------------------------------------------------------
                XPathNavigator channelLanguage = navigator.SelectSingleNode("language");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelLanguage))
                {
                    channel.Language        = channelLanguage.Value;
                }

                //------------------------------------------------------------
                //	Extract <lastBuildDate> node
                //------------------------------------------------------------
                XPathNavigator channelLastBuildDate = navigator.SelectSingleNode("lastBuildDate");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelLastBuildDate))
                {
                    Rfc822DateTime lastBuildDate;
                    if (Rfc822DateTime.TryParse(channelLastBuildDate.Value, out lastBuildDate))
                    {
                        channel.LastBuildDate   = lastBuildDate;
                    }
                }

                //------------------------------------------------------------
                //	Extract <managingEditor> node
                //------------------------------------------------------------
                XPathNavigator channelManagingEditor = navigator.SelectSingleNode("managingEditor");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelManagingEditor))
                {
                    channel.ManagingEditor  = channelManagingEditor.Value;
                }

                //------------------------------------------------------------
                //	Extract <pubDate> node
                //------------------------------------------------------------
                XPathNavigator channelPublicationDate = navigator.SelectSingleNode("pubDate");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelPublicationDate))
                {
                    Rfc822DateTime publicationDate;
                    if (Rfc822DateTime.TryParse(channelPublicationDate.Value, out publicationDate))
                    {
                        channel.PublicationDate = publicationDate;
                    }
                }

                //------------------------------------------------------------
                //	Extract <rating> node
                //------------------------------------------------------------
                XPathNavigator channelRating = navigator.SelectSingleNode("rating");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelRating))
                {
                    channel.Rating          = channelRating.Value;
                }

                //------------------------------------------------------------
                //	Extract <skipDay> node
                //------------------------------------------------------------
                XPathNavigator channelSkipDays      = navigator.SelectSingleNode("skipDays");
                if (RssXmlSyndicationFeedAdapter.NavigatorContainsChildren(channelSkipDays))
                {
                    XPathNodeIterator daysIterator  = channelSkipDays.Select("day");
                    if (RssXmlSyndicationFeedAdapter.IteratorContainsNodes(daysIterator))
                    {
                        RssXmlSyndicationFeedAdapter.UpdateSkipDays(channel.SkipDays, daysIterator);
                    }
                }

                //------------------------------------------------------------
                //	Extract <skipHour> node
                //------------------------------------------------------------
                XPathNavigator channelSkipHours     = navigator.SelectSingleNode("skipHours");
                if (RssXmlSyndicationFeedAdapter.NavigatorContainsChildren(channelSkipHours))
                {
                    XPathNodeIterator hoursIterator = channelSkipHours.Select("hour");
                    if (RssXmlSyndicationFeedAdapter.IteratorContainsNodes(hoursIterator))
                    {
                        RssXmlSyndicationFeedAdapter.UpdateSkipHours(channel.SkipHours, hoursIterator);
                    }
                }

                //------------------------------------------------------------
                //	Extract <textInput> node
                //------------------------------------------------------------
                XPathNavigator channelTextInput = navigator.SelectSingleNode("textInput");
                if (RssXmlSyndicationFeedAdapter.NavigatorContainsChildren(channelTextInput))
                {
                    RssTextInput textInput  = RssXmlSyndicationFeedAdapter.CreateTextInput(channelTextInput);
                    if (textInput != null)
                    {
                        channel.TextInput   = textInput;
                    }
                }

                //------------------------------------------------------------
                //	Extract <ttl> node
                //------------------------------------------------------------
                XPathNavigator channelTimeToLive = navigator.SelectSingleNode("ttl");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelTimeToLive))
                {
                    int timeToLive;
                    if (Int32.TryParse(channelTimeToLive.Value,  NumberStyles.Integer, CultureInfo.InvariantCulture, out timeToLive))
                    {
                        channel.TimeToLive  = timeToLive;
                    }
                }

                //------------------------------------------------------------
                //	Extract <webMaster> node
                //------------------------------------------------------------
                XPathNavigator channelWebMaster = navigator.SelectSingleNode("webMaster");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(channelWebMaster))
                {
                    channel.WebMaster       = channelWebMaster.Value;
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

        #region ModifyItem(RssItem item, XPathNavigator xmlSource)
        /// <summary>
        /// Updates the <see cref="RssItem"/> using information extracted from the provided <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="item">The <see cref="RssItem"/> to modify.</param>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract RSS item information from.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="item"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void ModifyItem(RssItem item, XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Attempt to update item state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract <title> node
                //------------------------------------------------------------
                XPathNavigator itemTitle        = navigator.SelectSingleNode("title");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(itemTitle))
                {
                    item.Title          = itemTitle.Value;
                }

                //------------------------------------------------------------
                //	Extract <description> node
                //------------------------------------------------------------
                XPathNavigator itemDescription  = navigator.SelectSingleNode("description");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(itemDescription))
                {
                    item.Description    = itemDescription.Value;
                }

                //------------------------------------------------------------
                //	Extract <link> node
                //------------------------------------------------------------
                XPathNavigator itemLink         = navigator.SelectSingleNode("link");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(itemLink))
                {
                    Uri linkUri;
                    if (Uri.TryCreate(itemLink.Value, UriKind.RelativeOrAbsolute, out linkUri))
                    {
                        item.Link               = linkUri;
                    }
                }

                //------------------------------------------------------------
                //	Extract <author> node
                //------------------------------------------------------------
                XPathNavigator itemAuthor       = navigator.SelectSingleNode("author");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(itemAuthor))
                {
                    item.Author                 = itemAuthor.Value;
                }

                //------------------------------------------------------------
                //	Extract <comments> node
                //------------------------------------------------------------
                XPathNavigator itemComments     = navigator.SelectSingleNode("comments");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(itemComments))
                {
                    Uri commentsUri;
                    if (Uri.TryCreate(itemComments.Value, UriKind.RelativeOrAbsolute, out commentsUri))
                    {
                        item.Comments           = commentsUri;
                    }
                }

                //------------------------------------------------------------
                //	Extract <pubDate> node
                //------------------------------------------------------------
                XPathNavigator itemPublicationDate  = navigator.SelectSingleNode("pubDate");
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(itemPublicationDate))
                {
                    Rfc822DateTime publicationDate;
                    if (Rfc822DateTime.TryParse(itemPublicationDate.Value, out publicationDate))
                    {
                        item.PublicationDate    = publicationDate;
                    }
                }

                //------------------------------------------------------------
                //	Extract <enclosure> node
                //------------------------------------------------------------
                XPathNavigator itemEnclosure    = navigator.SelectSingleNode("enclosure");
                if (RssXmlSyndicationFeedAdapter.NavigatorHasAttributes(itemEnclosure))
                {
                    RssEnclosure enclosure      = RssXmlSyndicationFeedAdapter.CreateEnclosure(itemEnclosure);
                    if (enclosure != null)
                    {
                        item.Enclosure          = enclosure;
                    }
                }

                //------------------------------------------------------------
                //	Extract <source> node
                //------------------------------------------------------------
                XPathNavigator itemSource       = navigator.SelectSingleNode("source");
                if (RssXmlSyndicationFeedAdapter.NavigatorContainsData(itemSource))
                {
                    RssSource source            = RssXmlSyndicationFeedAdapter.CreateSource(itemSource);
                    if (source != null)
                    {
                        item.Source             = source;
                    }
                }

                //------------------------------------------------------------
                //	Extract <guid> node
                //------------------------------------------------------------
                XPathNavigator itemGuid         = navigator.SelectSingleNode("guid");
                if (RssXmlSyndicationFeedAdapter.NavigatorContainsData(itemGuid))
                {
                    RssGuid guid                = RssXmlSyndicationFeedAdapter.CreateGuid(itemGuid);
                    if (guid != null)
                    {
                        item.Guid               = guid;
                    }
                }

                //------------------------------------------------------------
                //	Extract <category> nodes
                //------------------------------------------------------------
                XPathNodeIterator itemCategories    = navigator.Select("category");
                if (RssXmlSyndicationFeedAdapter.IteratorContainsNodes(itemCategories))
                {
                    RssXmlSyndicationFeedAdapter.UpdateCategories(item.Categories, itemCategories);
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
        //	PRIVATE WRITE ROUTINES
        //============================================================
        #region WriteChannel(RssChannel channel, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="RssChannel"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="channel">The <see cref="RssChannel"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="channel"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteChannel(RssChannel channel, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write channel
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (channel == null)
                {
                    throw new ArgumentNullException("channel");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <channel> element
                //------------------------------------------------------------
                writer.WriteStartElement("channel");

                //------------------------------------------------------------
                //	Write <title> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(channel.Title))
                {
                    writer.WriteElementString("title", channel.Title);
                }

                //------------------------------------------------------------
                //	Write <description> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(channel.Description))
                {
                    writer.WriteElementString("description", channel.Description);
                }

                //------------------------------------------------------------
                //	Write <link> element
                //------------------------------------------------------------
                if (channel.Link != null)
                {
                    writer.WriteElementString("link", channel.Link.ToString());
                }

                //------------------------------------------------------------
                //	Write channel optional elements
                //------------------------------------------------------------
                RssXmlSyndicationFeedAdapter.WriteChannelOptionals(channel, writer);

                //------------------------------------------------------------
                //	Write <category> elements
                //------------------------------------------------------------
                if (channel.Categories != null && channel.Categories.Count > 0)
                {
                    RssXmlSyndicationFeedAdapter.WriteCategories(channel.Categories, writer);
                }

                //------------------------------------------------------------
                //	Write <item> elements
                //------------------------------------------------------------
                if (channel.Items != null && channel.Items.Count > 0)
                {
                    foreach(RssItem item in channel.Items)
                    {
                        RssXmlSyndicationFeedAdapter.WriteItem(item, writer);
                    }
                }

                //------------------------------------------------------------
                //	Write </channel> element
                //------------------------------------------------------------
                writer.WriteFullEndElement();
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

        #region WriteChannelOptionals(RssChannel channel, XmlWriter writer)
        /// <summary>
        /// Writes the optional elements of the specified <see cref="RssChannel"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="channel">The <see cref="RssChannel"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="channel"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteChannelOptionals(RssChannel channel, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write channel optional elements
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (channel == null)
                {
                    throw new ArgumentNullException("channel");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <cloud> element
                //------------------------------------------------------------
                if (channel.Cloud != null)
                {
                    RssXmlSyndicationFeedAdapter.WriteCloud(channel.Cloud, writer);
                }

                //------------------------------------------------------------
                //	Write <copyright> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(channel.Copyright))
                {
                    writer.WriteElementString("copyright", channel.Copyright);
                }

                //------------------------------------------------------------
                //	Write <generator> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(channel.Generator))
                {
                    writer.WriteElementString("generator", channel.Generator);
                }

                //------------------------------------------------------------
                //	Write <image> element
                //------------------------------------------------------------
                if (channel.Image != null)
                {
                    RssXmlSyndicationFeedAdapter.WriteImage(channel.Image, writer);
                }

                //------------------------------------------------------------
                //	Write <language> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(channel.Language))
                {
                    writer.WriteElementString("language", channel.Language);
                }

                //------------------------------------------------------------
                //	Write <lastBuildDate> element
                //------------------------------------------------------------
                if (channel.LastBuildDate != null)
                {
                    writer.WriteElementString("lastBuildDate", channel.LastBuildDate.ToString());
                }

                //------------------------------------------------------------
                //	Write <managingEditor> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(channel.ManagingEditor))
                {
                    writer.WriteElementString("managingEditor", channel.ManagingEditor);
                }

                //------------------------------------------------------------
                //	Write <pubDate> element
                //------------------------------------------------------------
                if (channel.PublicationDate != null)
                {
                    writer.WriteElementString("pubDate", channel.PublicationDate.ToString());
                }

                //------------------------------------------------------------
                //	Write <rating> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(channel.Rating))
                {
                    writer.WriteElementString("rating", channel.Rating);
                }

                //------------------------------------------------------------
                //	Write <skipDay> element
                //------------------------------------------------------------
                if (channel.SkipDays != null && channel.SkipDays.Count > 0)
                {
                    RssXmlSyndicationFeedAdapter.WriteSkipDays(channel.SkipDays, writer);
                }

                //------------------------------------------------------------
                //	Write <skipHour> element
                //------------------------------------------------------------
                if (channel.SkipHours != null && channel.SkipHours.Count > 0)
                {
                    RssXmlSyndicationFeedAdapter.WriteSkipHours(channel.SkipHours, writer);
                }

                //------------------------------------------------------------
                //	Write <textInput> element
                //------------------------------------------------------------
                if (channel.TextInput != null)
                {
                    RssXmlSyndicationFeedAdapter.WriteTextInput(channel.TextInput, writer);
                }

                //------------------------------------------------------------
                //	Write <ttl> element
                //------------------------------------------------------------
                if (channel.TimeToLive != Int32.MinValue)
                {
                    writer.WriteElementString("ttl", channel.TimeToLive.ToString(CultureInfo.InvariantCulture));
                }

                //------------------------------------------------------------
                //	Write <webMaster> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(channel.WebMaster))
                {
                    writer.WriteElementString("webMaster", channel.WebMaster);
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

        #region WriteFeed(RssFeed feed, XmlWriter writer, XmlWriterType writerType)
        /// <summary>
        /// Writes the <see cref="RssFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="feed">The <see cref="RssFeed"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is written to.</param>
        /// <param name="writerType">A <see cref="XmlWriterType"/> enumeration value indicating the source/type of the <b>XmlWriter</b>.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="writerType"/> is <b>XmlWriterType.None</b>.</exception>
        private static void WriteFeed(RssFeed feed, XmlWriter writer, XmlWriterType writerType)
        {
            //------------------------------------------------------------
            //	Attempt to write syndication feed to writer
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (feed == null)
                {
                    throw new ArgumentNullException("feed");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }
                if (writerType == XmlWriterType.None)
                {
                    throw new ArgumentException(Resources.ExceptionSyndicationFeedAdapterInvalidWriterType, "writerType");
                }

                //------------------------------------------------------------
                //	Determine output processing based on writer type
                //------------------------------------------------------------
                switch (writerType)
                {
                    case XmlWriterType.Serialized:

                        //------------------------------------------------------------
                        //	Write syndication feed version attribute
                        //------------------------------------------------------------
                        writer.WriteAttributeString("version", feed.Version);

                        //------------------------------------------------------------
                        //	Write <channel> element
                        //------------------------------------------------------------
                        RssXmlSyndicationFeedAdapter.WriteChannel(feed.Channel, writer);

                        break;

                    case XmlWriterType.Standard:

                        //------------------------------------------------------------
                        //	Write <rss> element
                        //------------------------------------------------------------
                        writer.WriteStartElement("rss");

                        //------------------------------------------------------------
                        //	Write syndication feed version attribute
                        //------------------------------------------------------------
                        writer.WriteAttributeString("version", feed.Version);

                        //------------------------------------------------------------
                        //	Write <channel> element
                        //------------------------------------------------------------
                        RssXmlSyndicationFeedAdapter.WriteChannel(feed.Channel, writer);

                        //------------------------------------------------------------
                        //	Write </rss> element
                        //------------------------------------------------------------
                        writer.WriteFullEndElement();

                        break;

                    default:
                        //------------------------------------------------------------
                        //	Raise exception for unhandled writer types
                        //------------------------------------------------------------
                        throw new ArgumentOutOfRangeException("writerType");
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

        #region WriteItem(RssItem item, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="RssItem"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="item">The <see cref="RssItem"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="item"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteItem(RssItem item, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write channel item element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <item> element
                //------------------------------------------------------------
                writer.WriteStartElement("item");

                //------------------------------------------------------------
                //	Write <title> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(item.Title))
                {
                    writer.WriteElementString("title", item.Title);
                }

                //------------------------------------------------------------
                //	Write <description> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(item.Description))
                {
                    writer.WriteElementString("description", item.Description);
                }

                //------------------------------------------------------------
                //	Write <link> element
                //------------------------------------------------------------
                if (item.Link != null)
                {
                    writer.WriteElementString("link", item.Link.ToString());
                }

                //------------------------------------------------------------
                //	Write <author> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(item.Author))
                {
                    writer.WriteElementString("author", item.Author);
                }

                //------------------------------------------------------------
                //	Write <comments> element
                //------------------------------------------------------------
                if (item.Comments != null)
                {
                    writer.WriteElementString("comments", item.Comments.ToString());
                }

                //------------------------------------------------------------
                //	Write <pubDate> element
                //------------------------------------------------------------
                if (item.PublicationDate != null)
                {
                    writer.WriteElementString("pubDate", item.PublicationDate.ToString());
                }

                //------------------------------------------------------------
                //	Write <enclosure> element
                //------------------------------------------------------------
                if (item.Enclosure != null)
                {
                    RssXmlSyndicationFeedAdapter.WriteEnclosure(item.Enclosure, writer);
                }

                //------------------------------------------------------------
                //	Write <source> element
                //------------------------------------------------------------
                if (item.Source != null)
                {
                    RssXmlSyndicationFeedAdapter.WriteSource(item.Source, writer);
                }

                //------------------------------------------------------------
                //	Write <guid> element
                //------------------------------------------------------------
                if (item.Guid != null)
                {
                    RssXmlSyndicationFeedAdapter.WriteGuid(item.Guid, writer);
                }

                //------------------------------------------------------------
                //	Write <category> elements
                //------------------------------------------------------------
                if (item.Categories != null && item.Categories.Count > 0)
                {
                    RssXmlSyndicationFeedAdapter.WriteCategories(item.Categories, writer);
                }

                //------------------------------------------------------------
                //	Write </item> element
                //------------------------------------------------------------
                writer.WriteFullEndElement();
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
        //	ENTITY SERIALIZATION ROUTINES
        //============================================================
        #region WriteCategories(RssCategoryCollection categories, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="RssCategoryCollection"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="categories">The <see cref="RssCategoryCollection"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="categories"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteCategories(RssCategoryCollection categories, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write category elements
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (categories == null)
                {
                    throw new ArgumentNullException("categories");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Enumerate through categories
                //------------------------------------------------------------
                foreach (RssCategory category in categories)
                {
                    //------------------------------------------------------------
                    //	Write <category> element
                    //------------------------------------------------------------
                    writer.WriteStartElement("category");

                    //------------------------------------------------------------
                    //	Write domain attribute
                    //------------------------------------------------------------
                    if (!String.IsNullOrEmpty(category.Domain))
                    {
                        writer.WriteAttributeString("domain", category.Domain);
                    }

                    //------------------------------------------------------------
                    //	Write category value
                    //------------------------------------------------------------
                    writer.WriteString(category.Value);

                    //------------------------------------------------------------
                    //	Write </category> element
                    //------------------------------------------------------------
                    writer.WriteEndElement();
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

        #region WriteCloud(RssCloud cloud, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="RssCloud"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="cloud">The <see cref="RssCloud"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="cloud"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteCloud(RssCloud cloud, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write cloud element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (cloud == null)
                {
                    throw new ArgumentNullException("cloud");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <cloud> element
                //------------------------------------------------------------
                writer.WriteStartElement("cloud");

                //------------------------------------------------------------
                //	Write cloud attributes
                //------------------------------------------------------------
                writer.WriteAttributeString("domain", cloud.Domain);
                writer.WriteAttributeString("path", cloud.Path);
                writer.WriteAttributeString("port", cloud.Port.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("protocol", RssCloud.ProtocolToString(cloud.Protocol));
                writer.WriteAttributeString("registerProcedure", cloud.RegisterProcedure);

                //------------------------------------------------------------
                //	Write </cloud> element
                //------------------------------------------------------------
                writer.WriteEndElement();
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

        #region WriteEnclosure(RssEnclosure enclosure, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="RssEnclosure"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="enclosure">The <see cref="RssEnclosure"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="enclosure"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteEnclosure(RssEnclosure enclosure, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write emclosure element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (enclosure == null)
                {
                    throw new ArgumentNullException("enclosure");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <enclosure> element
                //------------------------------------------------------------
                writer.WriteStartElement("enclosure");

                //------------------------------------------------------------
                //	Write enclosure attributes
                //------------------------------------------------------------
                writer.WriteAttributeString("url", enclosure.Url != null ? enclosure.Url.ToString() : String.Empty);
                writer.WriteAttributeString("length", enclosure.Length != Int64.MinValue ? enclosure.Length.ToString(CultureInfo.InvariantCulture) : "0");
                writer.WriteAttributeString("type", enclosure.Type);

                //------------------------------------------------------------
                //	Write </enclosure> element
                //------------------------------------------------------------
                writer.WriteEndElement();
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

        #region WriteGuid(RssGuid cloud, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="RssGuid"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="guid">The <see cref="RssGuid"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="guid"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteGuid(RssGuid guid, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write guid element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (guid == null)
                {
                    throw new ArgumentNullException("guid");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <source> element
                //------------------------------------------------------------
                writer.WriteStartElement("guid");

                //------------------------------------------------------------
                //	Write cloud attributes
                //------------------------------------------------------------
                writer.WriteAttributeString("isPermaLink", guid.IsPermaLink.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());

                //------------------------------------------------------------
                //	Write source title
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(guid.Value))
                {
                    writer.WriteString(guid.Value);
                }

                //------------------------------------------------------------
                //	Write </source> element
                //------------------------------------------------------------
                writer.WriteEndElement();
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

        #region WriteImage(RssImage image, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="RssImage"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="image">The <see cref="RssImage"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="image"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteImage(RssImage image, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write image element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (image == null)
                {
                    throw new ArgumentNullException("image");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <image> element
                //------------------------------------------------------------
                writer.WriteStartElement("image");

                //------------------------------------------------------------
                //	Write image sub-elements
                //------------------------------------------------------------
                if (image.Url != null)
                {
                    writer.WriteElementString("url", image.Url.ToString());
                }

                if (!String.IsNullOrEmpty(image.Title))
                {
                    writer.WriteElementString("title", image.Title);
                }

                if (image.Link != null)
                {
                    writer.WriteElementString("link", image.Link.ToString());
                }

                if (!String.IsNullOrEmpty(image.Description))
                {
                    writer.WriteElementString("description", image.Description);
                }

                if (image.Height != Int32.MinValue)
                {
                    writer.WriteElementString("height", image.Height.ToString(CultureInfo.InvariantCulture));
                }

                if (image.Width != Int32.MinValue)
                {
                    writer.WriteElementString("width", image.Width.ToString(CultureInfo.InvariantCulture));
                }

                //------------------------------------------------------------
                //	Write </image> element
                //------------------------------------------------------------
                writer.WriteFullEndElement();
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

        #region WriteSkipDays(Collection<DayOfWeek> days, XmlWriter writer)
        /// <summary>
        /// Writes the specified collection of <see cref="DayOfWeek"/> instances to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="days">The collection of <see cref="DayOfWeek"/> instances to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="days"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteSkipDays(Collection<DayOfWeek> days, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write skip day elements
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (days == null)
                {
                    throw new ArgumentNullException("days");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <skipDays> element
                //------------------------------------------------------------
                writer.WriteStartElement("skipDays");

                //------------------------------------------------------------
                //	Enumerate through collection
                //------------------------------------------------------------
                foreach(DayOfWeek day in days)
                {
                    //------------------------------------------------------------
                    //	Write <day> sub-element based on enum value
                    //------------------------------------------------------------
                    switch (day)
                    {
                        case DayOfWeek.Friday:
                            writer.WriteElementString("day", "Friday");
                            break;

                        case DayOfWeek.Monday:
                            writer.WriteElementString("day", "Monday");
                            break;

                        case DayOfWeek.Saturday:
                            writer.WriteElementString("day", "Saturday");
                            break;

                        case DayOfWeek.Sunday:
                            writer.WriteElementString("day", "Sunday");
                            break;

                        case DayOfWeek.Thursday:
                            writer.WriteElementString("day", "Thursday");
                            break;

                        case DayOfWeek.Tuesday:
                            writer.WriteElementString("day", "Tuesday");
                            break;

                        case DayOfWeek.Wednesday:
                            writer.WriteElementString("day", "Wednesday");
                            break;
                    }
                }

                //------------------------------------------------------------
                //	Write </skipDays> element
                //------------------------------------------------------------
                writer.WriteFullEndElement();
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

        #region WriteSkipHours(Collection<DayOfWeek> hours, XmlWriter writer)
        /// <summary>
        /// Writes the specified collection of <see cref="System.Int32"/> values to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="hours">The collection of <see cref="System.Int32"/> values to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="hours"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteSkipHours(Collection<int> hours, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write skip hour elements
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (hours == null)
                {
                    throw new ArgumentNullException("hours");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <skipHours> element
                //------------------------------------------------------------
                writer.WriteStartElement("skipHours");

                //------------------------------------------------------------
                //	Enumerate through collection
                //------------------------------------------------------------
                foreach (int hour in hours)
                {
                    //------------------------------------------------------------
                    //	Write <hour> sub-element
                    //------------------------------------------------------------
                    writer.WriteElementString("hour", hour.ToString(CultureInfo.InvariantCulture));
                }

                //------------------------------------------------------------
                //	Write </skipHours> element
                //------------------------------------------------------------
                writer.WriteFullEndElement();
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

        #region WriteSource(RssSource cloud, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="RssSource"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="source">The <see cref="RssSource"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteSource(RssSource source, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write source element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (source == null)
                {
                    throw new ArgumentNullException("source");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <source> element
                //------------------------------------------------------------
                writer.WriteStartElement("source");

                //------------------------------------------------------------
                //	Write cloud attributes
                //------------------------------------------------------------
                writer.WriteAttributeString("url", source.Url != null ? source.Url.ToString() : String.Empty);

                //------------------------------------------------------------
                //	Write source title
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(source.Title))
                {
                    writer.WriteString(source.Title);
                }

                //------------------------------------------------------------
                //	Write </source> element
                //------------------------------------------------------------
                writer.WriteEndElement();
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

        #region WriteTextInput(RssTextInput cloud, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="RssTextInput"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="textInput">The <see cref="RssTextInput"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="textInput"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteTextInput(RssTextInput textInput, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write text input element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (textInput == null)
                {
                    throw new ArgumentNullException("textInput");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <textInput> element
                //------------------------------------------------------------
                writer.WriteStartElement("textInput");

                //------------------------------------------------------------
                //	Write text-input sub-elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(textInput.Title))
                {
                    writer.WriteElementString("title", textInput.Title);
                }

                if (!String.IsNullOrEmpty(textInput.Description))
                {
                    writer.WriteElementString("description", textInput.Description);
                }

                if (!String.IsNullOrEmpty(textInput.Name))
                {
                    writer.WriteElementString("name", textInput.Name);
                }

                if (textInput.Link != null)
                {
                    writer.WriteElementString("link", textInput.Link.ToString());
                }

                //------------------------------------------------------------
                //	Write </textInput> element
                //------------------------------------------------------------
                writer.WriteFullEndElement();
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
        //	ENTITY INSTANTIATION ROUTINES
        //============================================================
        #region CreateCloud(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="RssCloud"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="RssCloud"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static RssCloud CreateCloud(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssCloud cloud      = new RssCloud();
            bool propertySet    = false;

            //------------------------------------------------------------
            //	Attempt to create cloud
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract XML attributes for cloud
                //------------------------------------------------------------
                string cloudDomain              = navigator.GetAttribute("domain", String.Empty);
                string cloudPath                = navigator.GetAttribute("path", String.Empty);
                string cloudPort                = navigator.GetAttribute("port", String.Empty);
                string cloudProtocol            = navigator.GetAttribute("protocol", String.Empty);
                string cloudRegisterProcedure   = navigator.GetAttribute("registerProcedure", String.Empty);

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(cloudDomain))
                {
                    cloud.Domain            = cloudDomain;
                    propertySet             = true;
                }

                if (!String.IsNullOrEmpty(cloudPath))
                {
                    cloud.Path              = cloudPath;
                    propertySet             = true;
                }

                if (!String.IsNullOrEmpty(cloudPort))
                {
                    int port;
                    if (Int32.TryParse(cloudPort, NumberStyles.Integer, CultureInfo.InvariantCulture, out port))
                    {
                        cloud.Port  = port;
                        propertySet = true;
                    }
                }

                if (!String.IsNullOrEmpty(cloudProtocol))
                {
                    cloud.Protocol          = RssCloud.ProtocolFromString(cloudProtocol);
                    propertySet             = true;
                }

                if (!String.IsNullOrEmpty(cloudRegisterProcedure))
                {
                    cloud.RegisterProcedure = cloudRegisterProcedure;
                    propertySet             = true;
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Verify at least one property was set
            //------------------------------------------------------------
            if (propertySet)
            {
                return cloud;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreateEnclosure(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="RssEnclosure"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="RssEnclosure"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static RssEnclosure CreateEnclosure(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssEnclosure enclosure  = new RssEnclosure();
            bool propertySet        = false;

            //------------------------------------------------------------
            //	Attempt to create enclosure
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract XML attributes for enclosure
                //------------------------------------------------------------
                string enclosureLength  = navigator.GetAttribute("length", String.Empty);
                string enclosureType    = navigator.GetAttribute("type", String.Empty);
                string enclosureUrl     = navigator.GetAttribute("url", String.Empty);

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(enclosureLength))
                {
                    long length;
                    if (Int64.TryParse(enclosureLength, NumberStyles.Integer, CultureInfo.InvariantCulture, out length))
                    {
                        enclosure.Length    = length;
                        propertySet         = true;
                    }
                }

                if (!String.IsNullOrEmpty(enclosureType))
                {
                    enclosure.Type          = enclosureType;
                    propertySet             = true;
                }

                if (!String.IsNullOrEmpty(enclosureUrl))
                {
                    Uri urlUri;
                    if (Uri.TryCreate(enclosureUrl, UriKind.RelativeOrAbsolute, out urlUri))
                    {
                        enclosure.Url       = urlUri;
                        propertySet         = true;
                    }
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Verify at least one property was set
            //------------------------------------------------------------
            if (propertySet)
            {
                return enclosure;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreateGuid(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="RssGuid"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="RssGuid"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static RssGuid CreateGuid(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssGuid guid        = new RssGuid();
            bool propertySet    = false;

            //------------------------------------------------------------
            //	Attempt to create source
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract information for source
                //------------------------------------------------------------
                string guidValue        = navigator.Value;
                string guidIsPermaLink  = navigator.GetAttribute("isPermaLink", String.Empty);

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(guidValue))
                {
                    guid.Value              = guidValue;
                    propertySet             = true;
                }

                if (!String.IsNullOrEmpty(guidIsPermaLink))
                {
                    bool isPermaLink;
                    if (bool.TryParse(guidIsPermaLink, out isPermaLink))
                    {
                        guid.IsPermaLink    = isPermaLink;
                        propertySet         = true;
                    }
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Verify at least one property was set
            //------------------------------------------------------------
            if (propertySet)
            {
                return guid;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreateImage(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="RssImage"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="RssImage"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static RssImage CreateImage(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssImage image      = new RssImage();
            bool propertySet    = false;

            //------------------------------------------------------------
            //	Attempt to create cloud
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract XML elements for image
                //------------------------------------------------------------
                XPathNavigator imageDescription = navigator.SelectSingleNode("description");
                XPathNavigator imageHeight      = navigator.SelectSingleNode("height");
                XPathNavigator imageLink        = navigator.SelectSingleNode("link");
                XPathNavigator imageTitle       = navigator.SelectSingleNode("title");
                XPathNavigator imageUrl         = navigator.SelectSingleNode("url");
                XPathNavigator imageWidth       = navigator.SelectSingleNode("width");

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(imageDescription))
                {
                    image.Description   = imageDescription.Value;
                    propertySet         = true;
                }

                if (RssXmlSyndicationFeedAdapter.ItemContainsText(imageHeight))
                {
                    int height          = Int32.Parse(imageHeight.Value, CultureInfo.InvariantCulture);
                    if (height > RssImage.MaximumHeight)
                    {
                        image.Height    = RssImage.MaximumHeight;
                    }
                    else
                    {
                        image.Height    = height;
                    }
                    propertySet         = true;
                }

                if (RssXmlSyndicationFeedAdapter.ItemContainsText(imageLink))
                {
                    Uri linkUri;
                    if (Uri.TryCreate(imageLink.Value, UriKind.RelativeOrAbsolute, out linkUri))
                    {
                        image.Link      = linkUri;
                        propertySet     = true;
                    }
                }

                if (RssXmlSyndicationFeedAdapter.ItemContainsText(imageTitle))
                {
                    image.Title         = imageTitle.Value;
                    propertySet         = true;
                }

                if (RssXmlSyndicationFeedAdapter.ItemContainsText(imageUrl))
                {
                    Uri urlUri;
                    if (Uri.TryCreate(imageUrl.Value, UriKind.RelativeOrAbsolute, out urlUri))
                    {
                        image.Url       = urlUri;
                        propertySet     = true;
                    }
                }

                if (RssXmlSyndicationFeedAdapter.ItemContainsText(imageWidth))
                {
                    int width           = Int32.Parse(imageWidth.Value, CultureInfo.InvariantCulture);
                    if (width > RssImage.MaximumWidth)
                    {
                        image.Width     = RssImage.MaximumWidth;
                    }
                    else
                    {
                        image.Width     = width;
                    }
                    propertySet         = true;
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Verify at least one property was set
            //------------------------------------------------------------
            if (propertySet)
            {
                return image;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreateSource(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="RssSource"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="RssSource"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static RssSource CreateSource(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssSource source    = new RssSource();
            bool propertySet    = false;

            //------------------------------------------------------------
            //	Attempt to create source
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract information for source
                //------------------------------------------------------------
                string sourceValue  = navigator.Value;
                string sourceUrl    = navigator.GetAttribute("url", String.Empty);

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(sourceValue))
                {
                    source.Title    = sourceValue;
                    propertySet     = true;
                }

                if (!String.IsNullOrEmpty(sourceUrl))
                {
                    Uri urlUri;
                    if (Uri.TryCreate(sourceUrl, UriKind.RelativeOrAbsolute, out urlUri))
                    {
                        source.Url  = urlUri;
                        propertySet = true;
                    }
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Verify at least one property was set
            //------------------------------------------------------------
            if (propertySet)
            {
                return source;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreateTextInput(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="RssTextInput"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="RssTextInput"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static RssTextInput CreateTextInput(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssTextInput textInput  = new RssTextInput();
            bool propertySet        = false;

            //------------------------------------------------------------
            //	Attempt to create cloud
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract XML elements for image
                //------------------------------------------------------------
                XPathNavigator inputDescription = navigator.SelectSingleNode("description");
                XPathNavigator inputLink        = navigator.SelectSingleNode("link");
                XPathNavigator inputName        = navigator.SelectSingleNode("name");
                XPathNavigator inputTitle       = navigator.SelectSingleNode("title");

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (RssXmlSyndicationFeedAdapter.ItemContainsText(inputDescription))
                {
                    textInput.Description   = inputDescription.Value;
                    propertySet             = true;
                }

                if (RssXmlSyndicationFeedAdapter.ItemContainsText(inputLink))
                {
                    Uri linkUri;
                    if (Uri.TryCreate(inputLink.Value, UriKind.RelativeOrAbsolute, out linkUri))
                    {
                        textInput.Link      = linkUri;
                        propertySet         = true;
                    }
                }

                if (RssXmlSyndicationFeedAdapter.ItemContainsText(inputName))
                {
                    textInput.Name          = inputName.Value;
                    propertySet             = true;
                }

                if (RssXmlSyndicationFeedAdapter.ItemContainsText(inputTitle))
                {
                    textInput.Title         = inputTitle.Value;
                    propertySet             = true;
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Verify at least one property was set
            //------------------------------------------------------------
            if (propertySet)
            {
                return textInput;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region UpdateCategories(RssCategoryCollection categories, XPathNodeIterator iterator)
        /// <summary>
        /// Updates the specified <see cref="RssCategoryCollection"/> instance using the supplied <see cref="XPathNodeIterator"/>.
        /// </summary>
        /// <param name="categories">The <see cref="RssCategoryCollection"/> to update.</param>
        /// <param name="iterator">The <see cref="XPathNodeIterator"/> to extract information from.</param>
        /// <remarks>Calling this method clears the specified collection prior to attempting to add items to collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="categories"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="iterator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void UpdateCategories(RssCategoryCollection categories, XPathNodeIterator iterator)
        {
            //------------------------------------------------------------
            //	Attempt to create collection
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (categories == null)
                {
                    throw new ArgumentNullException("categories");
                }
                if (iterator == null)
                {
                    throw new ArgumentNullException("iterator");
                }

                //------------------------------------------------------------
                //	Clear collection
                //------------------------------------------------------------
                categories.Clear();

                //------------------------------------------------------------
                //	Walk iterator nodes
                //------------------------------------------------------------
                while (iterator.MoveNext())
                {
                    //------------------------------------------------------------
                    //	Verify current node is valid
                    //------------------------------------------------------------
                    if (iterator.Current != null && iterator.Current.NodeType == XPathNodeType.Element)
                    {
                        //------------------------------------------------------------
                        //	Extract category value and (optional) domain identifier
                        //------------------------------------------------------------
                        string categoryName     = iterator.Current.Value;
                        string categoryDomain   = iterator.Current.GetAttribute("domain", String.Empty);

                        //------------------------------------------------------------
                        //	Verify category value found
                        //------------------------------------------------------------
                        if (!String.IsNullOrEmpty(categoryName))
                        {
                            //------------------------------------------------------------
                            //	Create category instance
                            //------------------------------------------------------------
                            RssCategory category    = new RssCategory(categoryName);
                            if (!String.IsNullOrEmpty(categoryDomain))
                            {
                                category.Domain     = categoryDomain;
                            }

                            //------------------------------------------------------------
                            //	Add category to collection
                            //------------------------------------------------------------
                            if (!categories.Contains(category))
                            {
                                categories.Add(category);
                            }
                        }
                    }
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

        #region UpdateItems(RssItemCollection items, XPathNodeIterator iterator)
        /// <summary>
        /// Updates the specified <see cref="RssItemCollection"/> instance using the supplied <see cref="XPathNodeIterator"/>.
        /// </summary>
        /// <param name="items">The <see cref="RssItemCollection"/> to update.</param>
        /// <param name="iterator">The <see cref="XPathNodeIterator"/> to extract information from.</param>
        /// <remarks>Calling this method clears the specified collection prior to attempting to add items to collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="items"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="iterator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void UpdateItems(RssItemCollection items, XPathNodeIterator iterator)
        {
            //------------------------------------------------------------
            //	Attempt to create collection
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (items == null)
                {
                    throw new ArgumentNullException("items");
                }
                if (iterator == null)
                {
                    throw new ArgumentNullException("iterator");
                }

                //------------------------------------------------------------
                //	Clear collection
                //------------------------------------------------------------
                items.Clear();

                //------------------------------------------------------------
                //	Walk iterator nodes
                //------------------------------------------------------------
                while (iterator.MoveNext())
                {
                    //------------------------------------------------------------
                    //	Verify current node is valid
                    //------------------------------------------------------------
                    if (RssXmlSyndicationFeedAdapter.NavigatorContainsChildren(iterator.Current))
                    {
                        //------------------------------------------------------------
                        //	Create channel item instance
                        //------------------------------------------------------------
                        RssItem rssItem                 = new RssItem();

                        //------------------------------------------------------------
                        //	Update channel item using XML data
                        //------------------------------------------------------------
                        RssXmlSyndicationFeedAdapter.ModifyItem(rssItem, iterator.Current);

                        //------------------------------------------------------------
                        //	Add channel item to collection
                        //------------------------------------------------------------
                        items.Add(rssItem);
                    }
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

        #region UpdateSkipDays(Collection<DayOfWeek> skipDays, XPathNodeIterator iterator)
        /// <summary>
        /// Updates the specified collection of <see cref="DayOfWeek"/> instances using the supplied <see cref="XPathNodeIterator"/>.
        /// </summary>
        /// <param name="skipDays">The collection of <see cref="DayOfWeek"/> instances to update.</param>
        /// <param name="iterator">The <see cref="XPathNodeIterator"/> to extract information from.</param>
        /// <remarks>Calling this method clears the specified collection prior to attempting to add items to collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="skipDays"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="iterator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void UpdateSkipDays(Collection<DayOfWeek> skipDays, XPathNodeIterator iterator)
        {
            //------------------------------------------------------------
            //	Attempt to create collection
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (skipDays == null)
                {
                    throw new ArgumentNullException("skipDays");
                }
                if (iterator == null)
                {
                    throw new ArgumentNullException("iterator");
                }

                //------------------------------------------------------------
                //	Clear collection
                //------------------------------------------------------------
                skipDays.Clear();

                //------------------------------------------------------------
                //	Walk iterator nodes
                //------------------------------------------------------------
                while (iterator.MoveNext())
                {
                    //------------------------------------------------------------
                    //	Verify current node is valid
                    //------------------------------------------------------------
                    if (iterator.Current != null && iterator.Current.NodeType == XPathNodeType.Element)
                    {
                        //------------------------------------------------------------
                        //	Add appropriate element to collection
                        //------------------------------------------------------------
                        if (String.Compare(iterator.Current.Value, "Monday", true, CultureInfo.InvariantCulture) == 0)
                        {
                            skipDays.Add(DayOfWeek.Monday);
                        }
                        else if (String.Compare(iterator.Current.Value, "Tuesday", true, CultureInfo.InvariantCulture) == 0)
                        {
                            skipDays.Add(DayOfWeek.Tuesday);
                        }
                        else if (String.Compare(iterator.Current.Value, "Wednesday", true, CultureInfo.InvariantCulture) == 0)
                        {
                            skipDays.Add(DayOfWeek.Wednesday);
                        }
                        else if (String.Compare(iterator.Current.Value, "Thursday", true, CultureInfo.InvariantCulture) == 0)
                        {
                            skipDays.Add(DayOfWeek.Thursday);
                        }
                        else if (String.Compare(iterator.Current.Value, "Friday", true, CultureInfo.InvariantCulture) == 0)
                        {
                            skipDays.Add(DayOfWeek.Friday);
                        }
                        else if (String.Compare(iterator.Current.Value, "Saturday", true, CultureInfo.InvariantCulture) == 0)
                        {
                            skipDays.Add(DayOfWeek.Saturday);
                        }
                        else if (String.Compare(iterator.Current.Value, "Sunday", true, CultureInfo.InvariantCulture) == 0)
                        {
                            skipDays.Add(DayOfWeek.Sunday);
                        }
                    }
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

        #region UpdateSkipHours(Collection<int> skipHours, XPathNodeIterator iterator)
        /// <summary>
        /// Updates the specified collection of skip hour integers using the supplied <see cref="XPathNodeIterator"/>.
        /// </summary>
        /// <param name="skipHours">The collection of skip hours to update.</param>
        /// <param name="iterator">The <see cref="XPathNodeIterator"/> to extract information from.</param>
        /// <remarks>Calling this method clears the specified collection prior to attempting to add items to collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="skipHours"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="iterator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void UpdateSkipHours(Collection<int> skipHours, XPathNodeIterator iterator)
        {
            //------------------------------------------------------------
            //	Attempt to create collection
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (skipHours == null)
                {
                    throw new ArgumentNullException("skipHours");
                }
                if (iterator == null)
                {
                    throw new ArgumentNullException("iterator");
                }

                //------------------------------------------------------------
                //	Clear collection
                //------------------------------------------------------------
                skipHours.Clear();

                //------------------------------------------------------------
                //	Walk iterator nodes
                //------------------------------------------------------------
                while (iterator.MoveNext())
                {
                    //------------------------------------------------------------
                    //	Verify current node is valid
                    //------------------------------------------------------------
                    if (iterator.Current != null && iterator.Current.NodeType == XPathNodeType.Element)
                    {
                        //------------------------------------------------------------
                        //	Add appropriate element to collection
                        //------------------------------------------------------------
                        int skipHour;
                        if (Int32.TryParse(iterator.Current.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out skipHour))
                        {
                            if (skipHour >= 0 && skipHour <= 23)
                            {
                                skipHours.Add(skipHour);
                            }
                        }
                    }
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
    }
}
