/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/18/2007	brian.kuhn		Created AtomEngineSyndicationFeedAdapter Class
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

using BlogEngine.Core.Properties;
using BlogEngine.Core.Syndication.Atom;

using BlogEngine.Core.Syndication.Extensions;
using BlogEngine.Core.Syndication.Extensions.Common;

namespace BlogEngine.Core.Syndication.Data
{
    /// <summary>
    /// Provides a set of methods and properties used to fill or write <see cref="AtomFeed"/> instances from blog engine data source(s).
    /// </summary>
    public class AtomEngineSyndicationFeedAdapter : EngineSyndicationFeedAdapter
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the Atom 1.0 XML namespace designator.
        /// </summary>
        private const string ATOM_XML_NAMESPACE = "http://www.w3.org/2005/Atom";
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomEngineSyndicationFeedAdapter()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomEngineSyndicationFeedAdapter"/> class.
        /// </summary>
        public AtomEngineSyndicationFeedAdapter() : base()
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

        #region AtomEngineSyndicationFeedAdapter(SyndicationFeedSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomEngineSyndicationFeedAdapter"/> class using the supplied <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="settings">The set of features that the XML data adapter supports.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomEngineSyndicationFeedAdapter(SyndicationFeedSettings settings) : base(settings)
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

        #region AtomEngineSyndicationFeedAdapter(List<Post> posts, BlogSettings blogSettings, CategoryDictionary categories)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomEngineSyndicationFeedAdapter"/> class using the supplied collection of <see cref="Post"/> instances, <see cref="BlogSettings"/> and <see cref="CategoryDictionary"/>.
        /// </summary>
        /// <param name="posts">The collection of blog posts that the syndication feed adapter uses when filling a <see cref="AtomFeed"/>.</param>
        /// <param name="blogSettings">The <see cref="BlogSettings"/> that the syndication feed adapter uses when filling a <see cref="AtomFeed"/>.</param>
        /// <param name="categories">The <see cref="CategoryDictionary"/> that the syndication feed adapter uses when filling a <see cref="AtomFeed"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="blogSettings"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="categories"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomEngineSyndicationFeedAdapter(List<Post> posts, BlogSettings blogSettings, CategoryDictionary categories) : base(posts, blogSettings, categories)
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
        /// Adds or refreshes items/entries in the <see cref="AtomFeed"/> to match those in the blog engine data source(s).
        /// </summary>
        /// <param name="feed">A <see cref="AtomFeed"/> to fill using the underlying blog engine data source(s).</param>
        /// <returns>The number of items/entries successfully added to or refreshed in the <b>AtomFeed</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Fill</b> retrieves Atom syndication feed information from the blog engine data source(s).
        ///     </para>
        /// 
        ///     <para>
        ///         The <b>Fill</b> operation then sets the <b>AtomFeed</b> properties and adds items to the feed, creating the Atom syndication feed entities if they do not already exist.
        ///     </para>
        /// 
        ///     <para>
        ///         If the <b>AtomEngineSyndicationFeedAdapter</b> will also add supported extensions to the <b>AtomFeed</b> using the supported extensions configured in the <see cref="EngineSyndicationFeedAdapter.Settings"/> property.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="feed"/> is not a <see cref="AtomFeed"/>.</exception>
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
                if(feed.GetType() != typeof(AtomFeed))
                {
                    throw new ArgumentException(Resources.ExceptionAtomSyndicationFeedAdapterInvalidFeedType);
                }

                //------------------------------------------------------------
                //	Instantiate the syndication feed using blog engine data source(s)
                //------------------------------------------------------------
                modifiedItemsCount  = this.FillFeed((AtomFeed)feed);
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
        /// <exception cref="ArgumentException">The <paramref name="feed"/> is not of type <see cref="AtomFeed"/>.</exception>
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
                if(feed.GetType() != typeof(AtomFeed))
                {
                    throw new ArgumentException(Resources.ExceptionAtomSyndicationFeedAdapterInvalidFeedType, "feed");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }
                
                //------------------------------------------------------------
                //	Write the syndication feed to the writer
                //------------------------------------------------------------
                AtomEngineSyndicationFeedAdapter.WriteFeed((AtomFeed)feed, writer);
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
        #region FillFeed(AtomFeed feed)
        /// <summary>
        /// Instantiates the specified <see cref="AtomFeed"/> using the configured adapter properties.
        /// </summary>
        /// <param name="feed">The syndication feed to instantiate.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic).</exception>
        private int FillFeed(AtomFeed feed)
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
                feed.Settings.SupportedExtensions.Add(new SlashSyndicationExtension());
                feed.Settings.SupportedExtensions.Add(new TrackbackSyndicationExtension());
                feed.Settings.SupportedExtensions.Add(new WellFormedWebSyndicationExtension());

                //------------------------------------------------------------
                //	Set standard channel properties
                //------------------------------------------------------------
                feed.Title          = new AtomText(this.BlogSettings.Name);
                feed.Description    = new AtomText(this.BlogSettings.Description);
                feed.Id             = this.FeedLocation;
                if (this.Posts.Count > 0)
                {
                    feed.UpdatedOn  = new W3CDateTime(this.Posts[0].DateModified);
                }
                else
                {
                    feed.UpdatedOn  = new W3CDateTime(DateTime.Now);
                }

                //------------------------------------------------------------
                //	Add feed link(s)
                //------------------------------------------------------------
                AtomLink feedLink           = new AtomLink(new Uri(String.Concat(this.FeedLocation.ToString().TrimEnd('/'), "/syndication.axd?format=atom")));
                feed.Links.Add(feedLink);

                AtomLink feedWebSiteLink    = new AtomLink(this.FeedLocation);
                feedWebSiteLink.Relation    = LinkRelation.Related;
                feed.Links.Add(feedWebSiteLink);

                //------------------------------------------------------------
                //	Add configured extensions to feed
                //------------------------------------------------------------
                this.FillExtensions(feed, feed.Settings.SupportedExtensions);

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
                    //	Create feed entry for post and set standard entry properties
                    //------------------------------------------------------------
                    AtomEntry entry             = new AtomEntry();
                    entry.Title                 = new AtomText(post.Title);
                    entry.Id                    = post.PermaLink;
                    entry.UpdatedOn             = new W3CDateTime(post.DateCreated);

                    //------------------------------------------------------------
                    //	Add entry link(s)
                    //------------------------------------------------------------
                    AtomLink entryLink          = new AtomLink(post.AbsoluteLink);
                    entry.Links.Add(entryLink);

                    //------------------------------------------------------------
                    //	Set feed entry summary (Html type needed to display correctly)
                    //------------------------------------------------------------
                    AtomText entrySummary       = new AtomText();
                    entrySummary.Type           = TextType.Html;
                    entrySummary.Value          = this.MakeReferencesAbsolute(post.Content);
                    entry.Summary               = entrySummary;

                    //------------------------------------------------------------
                    //	Add feed entry categories if available
                    //------------------------------------------------------------
                    if (post.Categories.Count > 0)
                    {
                        foreach (Guid categoryId in post.Categories)
                        {
                            if (this.Categories.ContainsKey(categoryId))
                            {
                                AtomCategory category = new AtomCategory(this.Categories[categoryId]);
                                if (!entry.Categories.Contains(category))
                                {
                                    entry.Categories.Add(category);
                                }
                            }
                            else
                            {
                                AtomCategory unknownCategory    = new AtomCategory(categoryId.ToString());
                                if (!entry.Categories.Contains(unknownCategory))
                                {
                                    entry.Categories.Add(unknownCategory);
                                }
                            }
                        }
                    }

                    //------------------------------------------------------------
                    //	Add standard extensions to feed entry
                    //------------------------------------------------------------
                    SlashSyndicationExtension slashExtension                    = new SlashSyndicationExtension(post.Comments.Count);
                    entry.Extensions.Add(slashExtension);

                    TrackbackSyndicationExtension trackbackExtension = new TrackbackSyndicationExtension(post.TrackbackLink);
                    entry.Extensions.Add(trackbackExtension);

                    WellFormedWebSyndicationExtension wellFormedWebExtension    = new WellFormedWebSyndicationExtension();
                    wellFormedWebExtension.Comment                              = new Uri(String.Concat(post.AbsoluteLink.ToString(), "#comments"));
                    entry.Extensions.Add(wellFormedWebExtension);

                    //------------------------------------------------------------
                    //	Add entry to feed
                    //------------------------------------------------------------
                    feed.Entries.Add(entry);
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
            return feed.Entries.Count;
        }
        #endregion

        #region FillExtensions(AtomFeed feed, SyndicationExtensionDictionary supportedExtensions)
        /// <summary>
        /// Adds syndication extensions to the specified <see cref="AtomFeed"/>.
        /// </summary>
        /// <param name="feed">The <see cref="AtomFeed"/> to add extensions to.</param>
        /// <param name="supportedExtensions">The collection of supported extensions to modify if an extension is added to the <see cref="AtomFeed"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="supportedExtensions"/> is a null reference (Nothing in Visual Basic).</exception>
        private void FillExtensions(AtomFeed feed, SyndicationExtensionDictionary supportedExtensions)
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
                if (feed == null)
                {
                    throw new ArgumentNullException("feed");
                }
                if (supportedExtensions == null)
                {
                    throw new ArgumentNullException("supportedExtensions");
                }

                //------------------------------------------------------------
                //	Add Blog Channel extension information to feed
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
                    feed.Extensions.Add(blogChannelExtension);
                }

                //------------------------------------------------------------
                //	Add Dublin Core extension information to feed
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
                    feed.Extensions.Add(dublinCoreExtension);
                }

                //------------------------------------------------------------
                //	Add Geocoding extension information to feed
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
                    feed.Extensions.Add(geocodingExtension);
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
        #region WriteEntries(AtomEntryCollection entries, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomEntryCollection"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="entries">The <see cref="AtomEntryCollection"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="entries"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteEntries(AtomEntryCollection entries, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write entry elements
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (entries == null)
                {
                    throw new ArgumentNullException("entries");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Enumerate through entries
                //------------------------------------------------------------
                foreach (AtomEntry entry in entries)
                {
                    //------------------------------------------------------------
                    //	Write entry
                    //------------------------------------------------------------
                    AtomEngineSyndicationFeedAdapter.WriteEntry(entry, writer);
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

        #region WriteEntry(AtomEntry entry, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomEntry"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="entry">The <see cref="AtomEntry"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="entry"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteEntry(AtomEntry entry, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write feed entry element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (entry == null)
                {
                    throw new ArgumentNullException("entry");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <entry> element
                //------------------------------------------------------------
                writer.WriteStartElement("entry");

                //------------------------------------------------------------
                //	Write <id> element
                //------------------------------------------------------------
                if (entry.Id != null)
                {
                    writer.WriteElementString("id", entry.Id.ToString());
                }

                //------------------------------------------------------------
                //	Write <title> element
                //------------------------------------------------------------
                if (entry.Title != null)
                {
                    AtomEngineSyndicationFeedAdapter.WriteTextConstruct(entry.Title, "title", writer);
                }

                //------------------------------------------------------------
                //	Write <updated> element
                //------------------------------------------------------------
                if (entry.UpdatedOn != null)
                {
                    writer.WriteElementString("updated", entry.UpdatedOn.ToString());
                }

                //------------------------------------------------------------
                //	Write <content> element
                //------------------------------------------------------------
                if (entry.Content != null)
                {
                    AtomEngineSyndicationFeedAdapter.WriteContentConstruct(entry.Content, writer);
                }

                //------------------------------------------------------------
                //	Write <published> element
                //------------------------------------------------------------
                if (entry.PublishedOn != null)
                {
                    writer.WriteElementString("published", entry.PublishedOn.ToString());
                }

                //------------------------------------------------------------
                //	Write <rights> element
                //------------------------------------------------------------
                if (entry.Rights != null)
                {
                    AtomEngineSyndicationFeedAdapter.WriteTextConstruct(entry.Rights, "rights", writer);
                }

                //------------------------------------------------------------
                //	Write <summary> element
                //------------------------------------------------------------
                if (entry.Summary != null)
                {
                    AtomEngineSyndicationFeedAdapter.WriteTextConstruct(entry.Summary, "summary", writer);
                }

                //------------------------------------------------------------
                //	Write <author> elements
                //------------------------------------------------------------
                if (entry.Authors != null && entry.Authors.Count > 0)
                {
                    AtomEngineSyndicationFeedAdapter.WritePersons(entry.Authors, "author", writer);
                }

                //------------------------------------------------------------
                //	Write <category> elements
                //------------------------------------------------------------
                if (entry.Categories != null && entry.Categories.Count > 0)
                {
                    AtomEngineSyndicationFeedAdapter.WriteCategories(entry.Categories, writer);
                }

                //------------------------------------------------------------
                //	Write <contributor> elements
                //------------------------------------------------------------
                if (entry.Contributors != null && entry.Contributors.Count > 0)
                {
                    AtomEngineSyndicationFeedAdapter.WritePersons(entry.Contributors, "contributor", writer);
                }

                //------------------------------------------------------------
                //	Write <link> elements
                //------------------------------------------------------------
                if (entry.Links != null && entry.Links.Count > 0)
                {
                    AtomEngineSyndicationFeedAdapter.WriteLinks(entry.Links, writer);
                }

                //------------------------------------------------------------
                //	Write <source> element
                //------------------------------------------------------------
                if (entry.Source != null)
                {
                    AtomEngineSyndicationFeedAdapter.WriteFeedContent(entry.Source, writer);
                }

                //------------------------------------------------------------
                //	Write entry extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in entry.Extensions.Values)
                {
                    extension.Inject(writer);
                }

                //------------------------------------------------------------
                //	Write </entry> element
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

        #region WriteFeed(AtomFeed feed, XmlWriter writer)
        /// <summary>
        /// Writes the <see cref="AtomFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="feed">The <see cref="AtomFeed"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteFeed(AtomFeed feed, XmlWriter writer)
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
                //	Write <feed> element with default XML namespace
                //------------------------------------------------------------
                writer.WriteStartElement("feed", ATOM_XML_NAMESPACE);

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
                //	Write feed content
                //------------------------------------------------------------
                AtomEngineSyndicationFeedAdapter.WriteFeedContent(feed, writer);

                //------------------------------------------------------------
                //	Write feed extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in feed.Extensions.Values)
                {
                    extension.Inject(writer);
                }

                //------------------------------------------------------------
                //	Write </feed> element
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

        #region WriteFeedContent(AtomFeed feed, XmlWriter writer)
        /// <summary>
        /// Writes the <see cref="AtomFeed"/> content to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="feed">The <see cref="AtomFeed"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteFeedContent(AtomFeed feed, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write syndication feed content to writer
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
                //	Write <id> element
                //------------------------------------------------------------
                if (feed.Id != null)
                {
                    writer.WriteElementString("id", feed.Id.ToString());
                }

                //------------------------------------------------------------
                //	Write <title> element
                //------------------------------------------------------------
                if (feed.Title != null)
                {
                    AtomEngineSyndicationFeedAdapter.WriteTextConstruct(feed.Title, "title", writer);
                }

                //------------------------------------------------------------
                //	Write <updated> element
                //------------------------------------------------------------
                if (feed.UpdatedOn != null)
                {
                    writer.WriteElementString("updated", feed.UpdatedOn.ToString());
                }

                //------------------------------------------------------------
                //	Write feed optional elements
                //------------------------------------------------------------
                AtomEngineSyndicationFeedAdapter.WriteFeedOptionals(feed, writer);

                //------------------------------------------------------------
                //	Write <author> elements
                //------------------------------------------------------------
                if (feed.Authors != null && feed.Authors.Count > 0)
                {
                    AtomEngineSyndicationFeedAdapter.WritePersons(feed.Authors, "author", writer);
                }

                //------------------------------------------------------------
                //	Write <category> elements
                //------------------------------------------------------------
                if (feed.Categories != null && feed.Categories.Count > 0)
                {
                    AtomEngineSyndicationFeedAdapter.WriteCategories(feed.Categories, writer);
                }

                //------------------------------------------------------------
                //	Write <contributor> elements
                //------------------------------------------------------------
                if (feed.Contributors != null && feed.Contributors.Count > 0)
                {
                    AtomEngineSyndicationFeedAdapter.WritePersons(feed.Contributors, "contributor", writer);
                }

                //------------------------------------------------------------
                //	Write <link> elements
                //------------------------------------------------------------
                if (feed.Links != null && feed.Links.Count > 0)
                {
                    AtomEngineSyndicationFeedAdapter.WriteLinks(feed.Links, writer);
                }

                //------------------------------------------------------------
                //	Write <entry> elements
                //------------------------------------------------------------
                if (feed.Entries != null && feed.Entries.Count > 0)
                {
                    AtomEngineSyndicationFeedAdapter.WriteEntries(feed.Entries, writer);
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

        #region WriteFeedOptionals(AtomFeed feed, XmlWriter writer)
        /// <summary>
        /// Writes the <see cref="AtomFeed"/> optional elements to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="feed">The <see cref="AtomFeed"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteFeedOptionals(AtomFeed feed, XmlWriter writer)
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
                //	Write <generator> element
                //------------------------------------------------------------
                if (feed.Generator != null)
                {
                    AtomEngineSyndicationFeedAdapter.WriteGenerator(feed.Generator, writer);
                }

                //------------------------------------------------------------
                //	Write <icon> element
                //------------------------------------------------------------
                if (feed.Icon != null)
                {
                    writer.WriteElementString("icon", feed.Icon.ToString());
                }

                //------------------------------------------------------------
                //	Write <logo> element
                //------------------------------------------------------------
                if (feed.Logo != null)
                {
                    writer.WriteElementString("logo", feed.Logo.ToString());
                }

                //------------------------------------------------------------
                //	Write <rights> element
                //------------------------------------------------------------
                if (feed.Rights != null)
                {
                    AtomEngineSyndicationFeedAdapter.WriteTextConstruct(feed.Rights, "rights", writer);
                }

                //------------------------------------------------------------
                //	Write <subtitle> element
                //------------------------------------------------------------
                if (feed.Description != null)
                {
                    AtomEngineSyndicationFeedAdapter.WriteTextConstruct(feed.Description, "subtitle", writer);
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
        //	ENTITY SERIALIZATION ROUTINES
        //============================================================
        #region WriteCategories(AtomCategoryCollection categories, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomCategoryCollection"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="categories">The <see cref="AtomCategoryCollection"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="categories"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteCategories(AtomCategoryCollection categories, XmlWriter writer)
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
                foreach (AtomCategory category in categories)
                {
                    //------------------------------------------------------------
                    //	Write category
                    //------------------------------------------------------------
                    AtomEngineSyndicationFeedAdapter.WriteCategory(category, writer);
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

        #region WriteCategory(AtomCategory category, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomCategory"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="category">The <see cref="AtomCategory"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="category"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteCategory(AtomCategory category, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write category element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (category == null)
                {
                    throw new ArgumentNullException("category");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <category> element
                //------------------------------------------------------------
                writer.WriteStartElement("category");

                //------------------------------------------------------------
                //	Write category attributes
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(category.Term))
                {
                    writer.WriteAttributeString("term", category.Term);
                }

                if (category.Scheme != null)
                {
                    writer.WriteAttributeString("scheme", category.Scheme.ToString());
                }

                if (!String.IsNullOrEmpty(category.Label))
                {
                    writer.WriteAttributeString("label", category.Label);
                }

                //------------------------------------------------------------
                //	Write category content
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(category.Content))
                {
                    writer.WriteString(category.Content);
                }

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
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }
        }
        #endregion

        #region WriteContentConstruct(AtomContent construct, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomContent"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="construct">The <see cref="AtomContent"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="construct"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteContentConstruct(AtomContent construct, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write text construct element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (construct == null)
                {
                    throw new ArgumentNullException("construct");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <content> element
                //------------------------------------------------------------
                writer.WriteStartElement("content");

                //------------------------------------------------------------
                //	Write text construct attributes
                //------------------------------------------------------------
                if (construct.Type != TextType.None)
                {
                    writer.WriteAttributeString("type", AtomText.TextTypeToString(construct.Type));
                }
                else if (!String.IsNullOrEmpty(construct.MediaType))
                {
                    writer.WriteAttributeString("type", construct.MediaType);
                }

                if (construct.Source != null)
                {
                    writer.WriteAttributeString("src", construct.Source.ToString());
                }

                //------------------------------------------------------------
                //	Write text content
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(construct.Value))
                {
                    writer.WriteString(construct.Value);
                }

                //------------------------------------------------------------
                //	Write construct extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in construct.Extensions.Values)
                {
                    extension.Inject(writer);
                }

                //------------------------------------------------------------
                //	Write </content> element
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

        #region WriteGenerator(AtomGenerator generator, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomGenerator"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="generator">The <see cref="AtomGenerator"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="generator"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteGenerator(AtomGenerator generator, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write generator element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (generator == null)
                {
                    throw new ArgumentNullException("generator");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <generator> element
                //------------------------------------------------------------
                writer.WriteStartElement("generator");

                //------------------------------------------------------------
                //	Write generator attributes
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(generator.Version))
                {
                    writer.WriteAttributeString("version", generator.Version);
                }

                if (generator.Uri != null)
                {
                    writer.WriteAttributeString("uri", generator.Uri.ToString());
                }

                //------------------------------------------------------------
                //	Write generator name
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(generator.Name))
                {
                    writer.WriteString(generator.Name);
                }

                //------------------------------------------------------------
                //	Write generator extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in generator.Extensions.Values)
                {
                    extension.Inject(writer);
                }

                //------------------------------------------------------------
                //	Write </generator> element
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

        #region WriteLink(AtomLink category, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomLink"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="link">The <see cref="AtomLink"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="link"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteLink(AtomLink link, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write link element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (link == null)
                {
                    throw new ArgumentNullException("link");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write <link> element
                //------------------------------------------------------------
                writer.WriteStartElement("link");

                //------------------------------------------------------------
                //	Write link attributes
                //------------------------------------------------------------
                if (link.Link != null)
                {
                    writer.WriteAttributeString("href", link.Link.ToString());
                }

                if(link.Relation != LinkRelation.None)
                {
                    writer.WriteAttributeString("rel", AtomLink.RelationToString(link.Relation));
                }

                if (!String.IsNullOrEmpty(link.Type))
                {
                    writer.WriteAttributeString("type", link.Type);
                }

                if (!String.IsNullOrEmpty(link.Language))
                {
                    writer.WriteAttributeString("hreflang", link.Language);
                }

                if (!String.IsNullOrEmpty(link.Title))
                {
                    writer.WriteAttributeString("title", link.Title);
                }

                if (link.Length != Int64.MinValue)
                {
                    writer.WriteAttributeString("length", link.Length.ToString(CultureInfo.InvariantCulture));
                }

                //------------------------------------------------------------
                //	Write link content
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(link.Content))
                {
                    writer.WriteString(link.Content);
                }

                //------------------------------------------------------------
                //	Write link extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in link.Extensions.Values)
                {
                    extension.Inject(writer);
                }

                //------------------------------------------------------------
                //	Write </link> element
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

        #region WriteLinks(AtomLinkCollection links, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomLinkCollection"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="links">The <see cref="AtomLinkCollection"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="links"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void WriteLinks(AtomLinkCollection links, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write category elements
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (links == null)
                {
                    throw new ArgumentNullException("links");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Enumerate through links
                //------------------------------------------------------------
                foreach (AtomLink link in links)
                {
                    //------------------------------------------------------------
                    //	Write link
                    //------------------------------------------------------------
                    AtomEngineSyndicationFeedAdapter.WriteLink(link, writer);
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

        #region WritePerson(AtomPerson person, string elementName, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomPerson"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="person">The <see cref="AtomPerson"/> to be written.</param>
        /// <param name="elementName">The local name of the element to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="person"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="elementName"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="elementName"/> is an empty string.</exception>
        private static void WritePerson(AtomPerson person, string elementName, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write person element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (person == null)
                {
                    throw new ArgumentNullException("person");
                }
                if (elementName == null)
                {
                    throw new ArgumentNullException("elementName");
                }
                if (String.IsNullOrEmpty(elementName.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "elementName");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write element
                //------------------------------------------------------------
                writer.WriteStartElement(elementName);

                //------------------------------------------------------------
                //	Write person sub-elements
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(person.Name))
                {
                    writer.WriteElementString("name", person.Name);
                }

                if (!String.IsNullOrEmpty(person.EmailAddress))
                {
                    writer.WriteElementString("email", person.EmailAddress);
                }

                if (person.Uri != null)
                {
                    writer.WriteElementString("uri", person.Uri.ToString());
                }

                //------------------------------------------------------------
                //	Write person extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in person.Extensions.Values)
                {
                    extension.Inject(writer);
                }

                //------------------------------------------------------------
                //	Write end element
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

        #region WritePersons(AtomPersonCollection persons, string elementName, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomPersonCollection"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="persons">The <see cref="AtomPersonCollection"/> to be written.</param>
        /// <param name="elementName">The local name of the element to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="persons"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="elementName"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="elementName"/> is an empty string.</exception>
        private static void WritePersons(AtomPersonCollection persons, string elementName, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write person elements
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (persons == null)
                {
                    throw new ArgumentNullException("persons");
                }
                if (elementName == null)
                {
                    throw new ArgumentNullException("elementName");
                }
                if (String.IsNullOrEmpty(elementName.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "elementName");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Enumerate through persons
                //------------------------------------------------------------
                foreach (AtomPerson person in persons)
                {
                    //------------------------------------------------------------
                    //	Write person element using specified element name
                    //------------------------------------------------------------
                    AtomEngineSyndicationFeedAdapter.WritePerson(person, elementName, writer);
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

        #region WriteTextConstruct(AtomText construct, string elementName, XmlWriter writer)
        /// <summary>
        /// Writes the specified <see cref="AtomText"/> to the supplied <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="construct">The <see cref="AtomText"/> to be written.</param>
        /// <param name="elementName">The local name of the element to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which information is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="construct"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="elementName"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="elementName"/> is an empty string.</exception>
        private static void WriteTextConstruct(AtomText construct, string elementName, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to write text construct element
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (construct == null)
                {
                    throw new ArgumentNullException("construct");
                }
                if (elementName == null)
                {
                    throw new ArgumentNullException("elementName");
                }
                if (String.IsNullOrEmpty(elementName.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "elementName");
                }
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write element
                //------------------------------------------------------------
                writer.WriteStartElement(elementName);

                //------------------------------------------------------------
                //	Write text construct attributes
                //------------------------------------------------------------
                if (construct.Type != TextType.None)
                {
                    writer.WriteAttributeString("type", AtomText.TextTypeToString(construct.Type));
                }

                //------------------------------------------------------------
                //	Write text content
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(construct.Value))
                {
                    writer.WriteString(construct.Value);
                }

                //------------------------------------------------------------
                //	Write construct extensions
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in construct.Extensions.Values)
                {
                    extension.Inject(writer);
                }

                //------------------------------------------------------------
                //	Write end element
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
