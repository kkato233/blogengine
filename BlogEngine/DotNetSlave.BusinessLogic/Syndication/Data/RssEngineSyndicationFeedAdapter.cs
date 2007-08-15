/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/16/2007	brian.kuhn		Created RssEngineSyndicationFeedAdapter Class
05/03/2007  brian.kuhn      Added Pingback support
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

using BlogEngine.Core.Properties;
using BlogEngine.Core.Syndication.Rss;

using BlogEngine.Core.Syndication.Extensions;
using BlogEngine.Core.Syndication.Extensions.Common;

namespace BlogEngine.Core.Syndication.Data
{
    /// <summary>
    /// Provides a set of methods and properties used to fill or write <see cref="RssFeed"/> instances from blog engine data source(s).
    /// </summary>
    public class RssEngineSyndicationFeedAdapter : EngineSyndicationFeedAdapter
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssEngineSyndicationFeedAdapter()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssEngineSyndicationFeedAdapter"/> class.
        /// </summary>
        public RssEngineSyndicationFeedAdapter() : base()
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

        #region RssEngineSyndicationFeedAdapter(SyndicationFeedSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssEngineSyndicationFeedAdapter"/> class using the supplied <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="settings">The set of features that the XML data adapter supports.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssEngineSyndicationFeedAdapter(SyndicationFeedSettings settings) : base(settings)
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

        #region RssEngineSyndicationFeedAdapter(List<Post> posts, BlogSettings blogSettings, CategoryDictionary categories)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssEngineSyndicationFeedAdapter"/> class using the supplied collection of <see cref="Post"/> instances, <see cref="BlogSettings"/> and <see cref="Category"/>.
        /// </summary>
        /// <param name="posts">The collection of blog posts that the syndication feed adapter uses when filling a <see cref="RssFeed"/>.</param>
        /// <param name="blogSettings">The <see cref="BlogSettings"/> that the syndication feed adapter uses when filling a <see cref="RssFeed"/>.</param>
        /// <param name="categories">The <see cref="Category"/> that the syndication feed adapter uses when filling a <see cref="RssFeed"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="blogSettings"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="categories"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssEngineSyndicationFeedAdapter(List<Post> posts, BlogSettings blogSettings, List<Category> categories) : base(posts, blogSettings, categories)
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
        //	PUBLIC ROUTINES
        //============================================================
        #region Fill(SyndicationFeed feed)
        /// <summary>
        /// Adds or refreshes items/entries in the <see cref="RssFeed"/> to match those in the blog engine data source(s).
        /// </summary>
        /// <param name="feed">A <see cref="RssFeed"/> to fill using the underlying blog engine data source(s).</param>
        /// <returns>The number of items/entries successfully added to or refreshed in the <b>RssFeed</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Fill</b> retrieves RSS syndication feed information from the blog engine data source(s).
        ///     </para>
        /// 
        ///     <para>
        ///         The <b>Fill</b> operation then sets the <b>RssFeed</b> properties and adds items to the feed, creating the RSS syndication feed entities if they do not already exist.
        ///     </para>
        /// 
        ///     <para>
        ///         If the <b>RssEngineSyndicationFeedAdapter</b> will also add supported extensions to the <b>RssFeed</b> using the supported extensions configured in the <see cref="EngineSyndicationFeedAdapter.Settings"/> property.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="feed"/> is not a <see cref="RssFeed"/>.</exception>
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
                if(feed.GetType() != typeof(RssFeed))
                {
                    throw new ArgumentException(Resources.ExceptionRssSyndicationFeedAdapterInvalidFeedType);
                }

                //------------------------------------------------------------
                //	Instantiate the syndication feed using blog engine data source(s)
                //------------------------------------------------------------
                modifiedItemsCount  = this.FillFeed((RssFeed)feed);
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
                RssEngineSyndicationFeedAdapter.WriteFeed((RssFeed)feed, writer);
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
        #region FillFeed(RssFeed feed)
        /// <summary>
        /// Instantiates the specified <see cref="RssFeed"/> using the configured adapter properties.
        /// </summary>
        /// <param name="feed">The syndication feed to instantiate.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic).</exception>
        private int FillFeed(RssFeed feed)
        {
            //------------------------------------------------------------
            //	Attempt to instantiate feed using data sources
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (feed == null)
                {
                    throw new ArgumentNullException("feed");
                }

                //------------------------------------------------------------
                //	Add default supported extensions
                //------------------------------------------------------------
                feed.Settings.SupportedExtensions.Add(new PingbackSyndicationExtension());
                feed.Settings.SupportedExtensions.Add(new TrackbackSyndicationExtension());
                feed.Settings.SupportedExtensions.Add(new SlashSyndicationExtension());
                feed.Settings.SupportedExtensions.Add(new WellFormedWebSyndicationExtension());

                //------------------------------------------------------------
                //	Set standard channel properties
                //------------------------------------------------------------
                feed.Channel.Title          = this.BlogSettings.Name;
                feed.Channel.Language       = this.BlogSettings.Language;
                feed.Channel.Description    = this.BlogSettings.Description;
                feed.Channel.Link           = this.FeedLocation;

                //------------------------------------------------------------
                //	Add configured extensions to channel
                //------------------------------------------------------------
                this.FillExtensions(feed.Channel, feed.Settings.SupportedExtensions);

                //------------------------------------------------------------
                //	Enumerate through available blog posts
                //------------------------------------------------------------
                foreach (Post post in this.Posts)
                {
                    //------------------------------------------------------------
                    //	Skip blog post if it does not have a published status
                    //------------------------------------------------------------
                    if (!post.IsPublished)
                    {
                        continue;
                    }

                    //------------------------------------------------------------
                    //	Create channel item for post and set standard item properties
                    //------------------------------------------------------------
                    RssItem item            = new RssItem();
                    item.Title              = post.Title;
                    item.Link               = post.AbsoluteLink;
                    item.Description        = this.MakeReferencesAbsolute(post.Content);

                    //------------------------------------------------------------
                    //	Set channel item optional properties
                    //------------------------------------------------------------
                    item.Guid               = new RssGuid(post.PermaLink.ToString());
                    item.Comments           = new Uri(String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                    item.PublicationDate    = new Rfc822DateTime(post.DateCreated);

                    //------------------------------------------------------------
                    //	Add channel item categories if available
                    //------------------------------------------------------------
                    if (post.Categories.Count > 0)
                    {
                        foreach (Category cat in post.Categories)
                        {
                            if (this.Categories.Contains(cat))
                            {
                                RssCategory category = new RssCategory(cat.Title);
                                if (!item.Categories.Contains(category))
                                {
                                    item.Categories.Add(category);
                                }
                            }
                            else
                            {
                                RssCategory unknownCategory = new RssCategory(cat.Title);
                                if (!item.Categories.Contains(unknownCategory))
                                {
                                    item.Categories.Add(unknownCategory);
                                }
                            }
                        }
                    }

                    //------------------------------------------------------------
                    //	Add standard extensions to channel item
                    //------------------------------------------------------------
                    PingbackSyndicationExtension pingbackExtension              = new PingbackSyndicationExtension(new Uri(this.WebRoot.ToString().TrimEnd('/') + "/pingback.axd"), post.PermaLink);
                    item.Extensions.Add(pingbackExtension);

                    TrackbackSyndicationExtension trackbackExtension            = new TrackbackSyndicationExtension(post.TrackbackLink);
                    item.Extensions.Add(trackbackExtension);

                    SlashSyndicationExtension slashExtension                    = new SlashSyndicationExtension(post.Comments.Count);
                    item.Extensions.Add(slashExtension);

                    WellFormedWebSyndicationExtension wellFormedWebExtension    = new WellFormedWebSyndicationExtension();
                    wellFormedWebExtension.Comment                              = new Uri(String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                    wellFormedWebExtension.CommentFeed                          = new Uri(this.WebRoot.ToString().TrimEnd('/') + "/commentfeed.axd?id=" + post.Id.ToString());
                    item.Extensions.Add(wellFormedWebExtension);

                    //------------------------------------------------------------
                    //	Add item to channel
                    //------------------------------------------------------------
                    feed.Channel.Items.Add(item);
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
            //	Return result
            //------------------------------------------------------------
            return feed.Channel.Items.Count;
        }
        #endregion

        #region FillExtensions(RssChannel channel, SyndicationExtensionDictionary supportedExtensions)
        /// <summary>
        /// Adds syndication extensions to the specified <see cref="RssChannel"/>.
        /// </summary>
        /// <param name="channel">The <see cref="RssChannel"/> to add extensions to.</param>
        /// <param name="supportedExtensions">The collection of supported extensions to modify if an extension is added to the <see cref="RssChannel"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="channel"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="supportedExtensions"/> is a null reference (Nothing in Visual Basic).</exception>
        private void FillExtensions(RssChannel channel, SyndicationExtensionDictionary supportedExtensions)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool extensionIsSupported   = false;

            //------------------------------------------------------------
            //	Attempt to instantiate feed using data sources
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
                if (supportedExtensions == null)
                {
                    throw new ArgumentNullException("supportedExtensions");
                }

                //------------------------------------------------------------
                //	Add Blog Channel extension information to channel
                //------------------------------------------------------------
                BlogChannelSyndicationExtension blogChannelExtension    = new BlogChannelSyndicationExtension();
                extensionIsSupported                                    = false;
                if(this.Blogroll != null)
                {
                    blogChannelExtension.BlogRoll   = this.Blogroll;
                    extensionIsSupported            = true;
                }
                if (this.Subscriptions != null)
                {
                    blogChannelExtension.Subscriptions  = this.Subscriptions;
                    extensionIsSupported                = true;
                }
                if (!String.IsNullOrEmpty(this.BlogSettings.Endorsement))
                {
                    Uri bLinkUri;
                    if (Uri.TryCreate(this.BlogSettings.Endorsement, UriKind.RelativeOrAbsolute, out bLinkUri))
                    {
                        blogChannelExtension.BLink  = bLinkUri;
                        extensionIsSupported        = true;
                    }
                }
                if(extensionIsSupported)
                {
                    supportedExtensions.Add(new BlogChannelSyndicationExtension());
                    channel.Extensions.Add(blogChannelExtension);
                }

                //------------------------------------------------------------
                //	Add Dublin Core extension information to channel
                //------------------------------------------------------------
                DublinCoreSyndicationExtension dublinCoreExtension  = new DublinCoreSyndicationExtension();
                extensionIsSupported                                = false;
                if (!String.IsNullOrEmpty(this.BlogSettings.AuthorName))
                {
                    dublinCoreExtension.Creator     = this.BlogSettings.AuthorName;
                    extensionIsSupported            = true;
                }
                if (!String.IsNullOrEmpty(this.BlogSettings.Language))
                {
                    dublinCoreExtension.Language    = this.BlogSettings.Language;
                    extensionIsSupported            = true;
                }
                if (extensionIsSupported)
                {
                    supportedExtensions.Add(new DublinCoreSyndicationExtension());
                    channel.Extensions.Add(dublinCoreExtension);
                }

                //------------------------------------------------------------
                //	Add Geocoding extension information to channel
                //------------------------------------------------------------
                GeocodingSyndicationExtension geocodingExtension    = new GeocodingSyndicationExtension();
                extensionIsSupported                                = false;
                if (this.BlogSettings.GeocodingLatitude != Single.MinValue)
                {
                    geocodingExtension.Latitude     = this.BlogSettings.GeocodingLatitude;
                    extensionIsSupported            = true;
                }
                if (this.BlogSettings.GeocodingLongitude != Single.MinValue)
                {
                    geocodingExtension.Longitude    = this.BlogSettings.GeocodingLongitude;
                    extensionIsSupported            = true;
                }
                if (extensionIsSupported)
                {
                    supportedExtensions.Add(new GeocodingSyndicationExtension());
                    channel.Extensions.Add(geocodingExtension);
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
                RssEngineSyndicationFeedAdapter.WriteChannelOptionals(channel, writer);

                //------------------------------------------------------------
                //	Write <category> elements
                //------------------------------------------------------------
                if (channel.Categories != null && channel.Categories.Count > 0)
                {
                    RssEngineSyndicationFeedAdapter.WriteCategories(channel.Categories, writer);
                }

                //------------------------------------------------------------
                //	Write <item> elements
                //------------------------------------------------------------
                if (channel.Items != null && channel.Items.Count > 0)
                {
                    foreach(RssItem item in channel.Items)
                    {
                        RssEngineSyndicationFeedAdapter.WriteItem(item, writer);
                    }
                }

                //------------------------------------------------------------
                //	Write channel extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in channel.Extensions.Values)
                {
                    extension.Inject(writer);
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
                    RssEngineSyndicationFeedAdapter.WriteCloud(channel.Cloud, writer);
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
                    RssEngineSyndicationFeedAdapter.WriteImage(channel.Image, writer);
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
                    RssEngineSyndicationFeedAdapter.WriteSkipDays(channel.SkipDays, writer);
                }

                //------------------------------------------------------------
                //	Write <skipHour> element
                //------------------------------------------------------------
                if (channel.SkipHours != null && channel.SkipHours.Count > 0)
                {
                    RssEngineSyndicationFeedAdapter.WriteSkipHours(channel.SkipHours, writer);
                }

                //------------------------------------------------------------
                //	Write <textInput> element
                //------------------------------------------------------------
                if (channel.TextInput != null)
                {
                    RssEngineSyndicationFeedAdapter.WriteTextInput(channel.TextInput, writer);
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

        #region WriteFeed(RssFeed feed, XmlWriter writer)
        /// <summary>
        /// Writes the <see cref="RssFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="feed">The <see cref="RssFeed"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteFeed(RssFeed feed, XmlWriter writer)
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

                //------------------------------------------------------------
                //	Write <rss> element
                //------------------------------------------------------------
                writer.WriteStartElement("rss");

                //------------------------------------------------------------
                //	Write syndication feed version attribute
                //------------------------------------------------------------
                writer.WriteAttributeString("version", feed.Version);

                //------------------------------------------------------------
                //	Write syndication feed extension namespaces
                //------------------------------------------------------------
                foreach (SyndicationExtension supportedExtension in feed.Settings.SupportedExtensions.Values)
                {
                    writer.WriteAttributeString("xmlns", supportedExtension.NamespacePrefix, null, supportedExtension.Namespace);
                }

                //------------------------------------------------------------
                //	Write <channel> element
                //------------------------------------------------------------
                RssEngineSyndicationFeedAdapter.WriteChannel(feed.Channel, writer);

                //------------------------------------------------------------
                //	Write feed extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in feed.Extensions.Values)
                {
                    extension.Inject(writer);
                }

                //------------------------------------------------------------
                //	Write </rss> element
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
                    RssEngineSyndicationFeedAdapter.WriteEnclosure(item.Enclosure, writer);
                }

                //------------------------------------------------------------
                //	Write <source> element
                //------------------------------------------------------------
                if (item.Source != null)
                {
                    RssEngineSyndicationFeedAdapter.WriteSource(item.Source, writer);
                }

                //------------------------------------------------------------
                //	Write <guid> element
                //------------------------------------------------------------
                if (item.Guid != null)
                {
                    RssEngineSyndicationFeedAdapter.WriteGuid(item.Guid, writer);
                }

                //------------------------------------------------------------
                //	Write <category> elements
                //------------------------------------------------------------
                if (item.Categories != null && item.Categories.Count > 0)
                {
                    RssEngineSyndicationFeedAdapter.WriteCategories(item.Categories, writer);
                }

                //------------------------------------------------------------
                //	Write item extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in item.Extensions.Values)
                {
                    extension.Inject(writer);
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
                    //	Write category extensions
                    //------------------------------------------------------------
                    foreach (SyndicationExtension extension in category.Extensions.Values)
                    {
                        extension.Inject(writer);
                    }

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
                //	Write cloud extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in cloud.Extensions.Values)
                {
                    extension.Inject(writer);
                }

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
                //	Write enclosure extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in enclosure.Extensions.Values)
                {
                    extension.Inject(writer);
                }

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
                //	Write <guid> element
                //------------------------------------------------------------
                writer.WriteStartElement("guid");

                //------------------------------------------------------------
                //	Write guid attributes
                //------------------------------------------------------------
                writer.WriteAttributeString("isPermaLink", guid.IsPermaLink.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());

                //------------------------------------------------------------
                //	Write guid value
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(guid.Value))
                {
                    writer.WriteString(guid.Value);
                }

                //------------------------------------------------------------
                //	Write guidextensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in guid.Extensions.Values)
                {
                    extension.Inject(writer);
                }

                //------------------------------------------------------------
                //	Write </guid> element
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
                //	Write image extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in image.Extensions.Values)
                {
                    extension.Inject(writer);
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
                //	Write source extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in source.Extensions.Values)
                {
                    extension.Inject(writer);
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
                //	Write text input extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in textInput.Extensions.Values)
                {
                    extension.Inject(writer);
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
    }
}
