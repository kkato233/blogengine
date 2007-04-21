/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/18/2007	brian.kuhn		Created AtomXmlSyndicationFeedAdapter Class
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

using BlogEngine.Core.Properties;
using DotNetSlave.BlogEngine.BusinessLogic.Syndication.Atom;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Data
{
    /// <summary>
    /// Provides a set of methods and properties used to fill or write <see cref="AtomFeed"/> instances from an XML data source.
    /// </summary>
    public class AtomXmlSyndicationFeedAdapter : XmlSyndicationFeedAdapter
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold XML namespace manager used to query syndication feed data.
        /// </summary>
        private XmlNamespaceManager xmlNamespaceManager;
        /// <summary>
        /// Private member to hold the Atom 1.0 XML namespace designator.
        /// </summary>
        private const string ATOM_XML_NAMESPACE = "http://www.w3.org/2005/Atom";
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomXmlSyndicationFeedAdapter()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomXmlSyndicationFeedAdapter"/> class.
        /// </summary>
        public AtomXmlSyndicationFeedAdapter() : base()
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

        #region AtomXmlSyndicationFeedAdapter(XmlReader reader)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomXmlSyndicationFeedAdapter"/> class using the supplied <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">A reader that provides fast, non-cached, forward-only access to syndication feed XML data.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomXmlSyndicationFeedAdapter(XmlReader reader) : base(reader)
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

        #region AtomXmlSyndicationFeedAdapter(XmlReader reader, SyndicationFeedSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomXmlSyndicationFeedAdapter"/> class using the supplied <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">A reader that provides fast, non-cached, forward-only access to syndication feed XML data.</param>
        /// <param name="settings">The set of features that the XML data adapter supports.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomXmlSyndicationFeedAdapter(XmlReader reader, SyndicationFeedSettings settings) : base(reader, settings)
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
        /// Gets the default XML namespace this syndication feed adapter expects to parse.
        /// </summary>
        /// <remarks></remarks>
        public static string DefaultXmlNamespace
        {
            get
            {
                return ATOM_XML_NAMESPACE;
            }
        }
        #endregion

        #region NamespaceManager
        /// <summary>
        /// Gets or sets the <see cref="XmlNamespaceManager"/> used to query syndication feed data.
        /// </summary>
        /// <value>The <see cref="XmlNamespaceManager"/> used to query syndication feed data.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public XmlNamespaceManager NamespaceManager
        {
            get
            {
                return xmlNamespaceManager;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    xmlNamespaceManager = value;
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region Fill(SyndicationFeed feed)
        /// <summary>
        /// Adds or refreshes items/entries in the <see cref="AtomFeed"/> to match those in the XML data source.
        /// </summary>
        /// <param name="feed">A <see cref="AtomFeed"/> to fill using the underlying XML data source.</param>
        /// <returns>The number of items/entries successfully added to or refreshed in the <b>AtomFeed</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Fill</b> retrieves Atom syndication feed information from the XML data source.
        ///     </para>
        /// 
        ///     <para>
        ///         The <b>Fill</b> operation then sets the <b>AtomFeed</b> properties and adds items to the feed, creating the Atom syndication feed entities if they do not already exist.
        ///     </para>
        /// 
        ///     <para>
        ///         If the <b>AtomXmlSyndicationFeedAdapter</b> will also add supported extensions to the <b>AtomFeed</b> using the supported extensions configured in the <see cref="XmlSyndicationFeedAdapter.Settings"/> property.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <see cref="XmlSyndicationFeedAdapter.Reader"/> property is a null reference (Nothing in Visual Basic) -or- the <paramref name="feed"/> is not a <see cref="AtomFeed"/>.</exception>
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
                if(feed.GetType() != typeof(AtomFeed))
                {
                    throw new ArgumentException(Resources.ExceptionAtomSyndicationFeedAdapterInvalidFeedType);
                }

                //------------------------------------------------------------
                //	Instantiate the syndication feed using XML data source
                //------------------------------------------------------------
                modifiedItemsCount  = this.FillFeed((AtomFeed)feed, new XPathDocument(this.Reader));
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
                AtomXmlSyndicationFeedAdapter.WriteFeed((AtomFeed)feed, writer, writerType);
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
        /// <exception cref="ArgumentException">The <paramref name="feed"/> is not of type <see cref="AtomFeed"/> -or- the <paramref name="writerType"/> is <b>XmlWriterType.None</b>.</exception>
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
                if (feed.GetType() != typeof(AtomFeed))
                {
                    throw new ArgumentException(Resources.ExceptionAtomSyndicationFeedAdapterInvalidFeedType, "feed");
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
                AtomXmlSyndicationFeedAdapter.WriteFeed((AtomFeed)feed, writer, writerType);
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
        #region FillFeed(AtomFeed feed, XPathDocument xpathDocument)
        /// <summary>
        /// Instantiates the specified <see cref="AtomFeed"/> using the configured settings and XML data source.
        /// </summary>
        /// <param name="feed">The syndication feed to instantiate.</param>
        /// <param name="document">A fast, read-only, in-memory representation of the syndication XML data using the XPath data model.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="document"/> is a null reference (Nothing in Visual Basic).</exception>
        private int FillFeed(AtomFeed feed, XPathDocument document)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            XPathNavigator navigator    = null;
            string defaultFeedNamespace = String.Empty;

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
                //	Move to the <feed>
                //------------------------------------------------------------
                if (navigator.MoveToChild(XPathNodeType.Element))
                {
                    //------------------------------------------------------------
                    //	Verify parsing <feed> node
                    //------------------------------------------------------------
                    if (String.Compare(navigator.LocalName, "feed", true, CultureInfo.InvariantCulture) == 0)
                    {
                        //------------------------------------------------------------
                        //	Initialize XML namespace manager
                        //------------------------------------------------------------
                        this.NamespaceManager   = new XmlNamespaceManager(navigator.NameTable);
                        defaultFeedNamespace    = navigator.LookupNamespace(String.Empty);
                        this.NamespaceManager.AddNamespace("atom", !String.IsNullOrEmpty(defaultFeedNamespace) ? defaultFeedNamespace : ATOM_XML_NAMESPACE);

                        //------------------------------------------------------------
                        //	Get syndication feed version (if explicitly set)
                        //------------------------------------------------------------
                        string feedVersion  = navigator.GetAttribute("version", String.Empty);
                        if (!String.IsNullOrEmpty(feedVersion))
                        {
                            feed.Version    = feedVersion;
                        }

                        //------------------------------------------------------------
                        //	Set feed properties
                        //------------------------------------------------------------
                        this.ModifyFeed(feed, navigator);
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
            return feed.Entries.Count;
        }
        #endregion

        #region ModifyEntry(AtomEntry entry, XPathNavigator xmlSource)
        /// <summary>
        /// Updates the <see cref="AtomEntry"/> using information extracted from the provided <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="entry">The <see cref="AtomEntry"/> to modify.</param>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract Atom entry information from.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="entry"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private void ModifyEntry(AtomEntry entry, XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Attempt to update item state
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
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract <id> node
                //------------------------------------------------------------
                XPathNavigator entryId  = navigator.SelectSingleNode("atom:id", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(entryId))
                {
                    Uri idUri;
                    if (Uri.TryCreate(entryId.Value, UriKind.RelativeOrAbsolute, out idUri))
                    {
                        entry.Id        = idUri;
                    }
                }

                //------------------------------------------------------------
                //	Extract <title> node
                //------------------------------------------------------------
                XPathNavigator entryTitle   = navigator.SelectSingleNode("atom:title", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(entryTitle))
                {
                    AtomText title          = AtomXmlSyndicationFeedAdapter.CreateTextConstruct(entryTitle);
                    if (title != null)
                    {
                        entry.Title         = title;
                    }
                }

                //------------------------------------------------------------
                //	Extract <updated> node
                //------------------------------------------------------------
                XPathNavigator entryUpdated = navigator.SelectSingleNode("atom:updated", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(entryUpdated))
                {
                    W3CDateTime updatedOn;
                    if (W3CDateTime.TryParse(entryUpdated.Value, out updatedOn))
                    {
                        entry.UpdatedOn     = updatedOn;
                    }
                }

                //------------------------------------------------------------
                //	Extract <content> node
                //------------------------------------------------------------
                XPathNavigator entryContent = navigator.SelectSingleNode("atom:content", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.NavigatorContainsData(entryContent))
                {
                    AtomContent content     = AtomXmlSyndicationFeedAdapter.CreateContentConstruct(entryContent);
                    if (content != null)
                    {
                        entry.Content       = content;
                    }
                }

                //------------------------------------------------------------
                //	Extract <published> node
                //------------------------------------------------------------
                XPathNavigator entryPublished   = navigator.SelectSingleNode("atom:published", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(entryPublished))
                {
                    W3CDateTime publishedOn;
                    if (W3CDateTime.TryParse(entryPublished.Value, out publishedOn))
                    {
                        entry.PublishedOn       = publishedOn;
                    }
                }

                //------------------------------------------------------------
                //	Extract <rights> node
                //------------------------------------------------------------
                XPathNavigator entryRights  = navigator.SelectSingleNode("atom:rights", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(entryRights))
                {
                    AtomText rights         = AtomXmlSyndicationFeedAdapter.CreateTextConstruct(entryRights);
                    if (rights != null)
                    {
                        entry.Rights        = rights;
                    }
                }

                //------------------------------------------------------------
                //	Extract <summary> node
                //------------------------------------------------------------
                XPathNavigator entrySummary = navigator.SelectSingleNode("atom:summary", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(entrySummary))
                {
                    AtomText summary        = AtomXmlSyndicationFeedAdapter.CreateTextConstruct(entrySummary);
                    if (summary != null)
                    {
                        entry.Summary       = summary;
                    }
                }

                //------------------------------------------------------------
                //	Extract <author> nodes
                //------------------------------------------------------------
                XPathNodeIterator entryAuthors      = navigator.Select("atom:author", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.IteratorContainsNodes(entryAuthors))
                {
                    this.UpdatePersons(entry.Authors, entryAuthors);
                }

                //------------------------------------------------------------
                //	Extract <category> nodes
                //------------------------------------------------------------
                XPathNodeIterator entryCategories   = navigator.Select("atom:category", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.IteratorContainsNodes(entryCategories))
                {
                    AtomXmlSyndicationFeedAdapter.UpdateCategories(entry.Categories, entryCategories);
                }

                //------------------------------------------------------------
                //	Extract <link> nodes
                //------------------------------------------------------------
                XPathNodeIterator entryLinks        = navigator.Select("atom:link", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.IteratorContainsNodes(entryLinks))
                {
                    AtomXmlSyndicationFeedAdapter.UpdateLinks(entry.Links, entryLinks);
                }

                //------------------------------------------------------------
                //	Extract <contributor> nodes
                //------------------------------------------------------------
                XPathNodeIterator entryContributors = navigator.Select("atom:contributor", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.IteratorContainsNodes(entryContributors))
                {
                    this.UpdatePersons(entry.Contributors, entryContributors);
                }

                //------------------------------------------------------------
                //	Extract <source> node
                //------------------------------------------------------------
                XPathNavigator entrySource = navigator.SelectSingleNode("atom:source", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.NavigatorContainsChildren(entrySource))
                {
                    AtomFeed source = new AtomFeed();
                    this.ModifyFeed(source, entrySummary);
                    if (source != null)
                    {
                        entry.Source    = source;
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

        #region ModifyFeed(AtomFeed feed, XPathNavigator xmlSource)
        /// <summary>
        /// Updates the <see cref="AtomFeed"/> using information extracted from the provided <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="feed">The <see cref="AtomFeed"/> to modify.</param>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract Atom feed information from.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private void ModifyFeed(AtomFeed feed, XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Attempt to update feed state
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
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract <id> node
                //------------------------------------------------------------
                XPathNavigator feedId               = navigator.SelectSingleNode("atom:id", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(feedId))
                {
                    Uri idUri;
                    if (Uri.TryCreate(feedId.Value, UriKind.RelativeOrAbsolute, out idUri))
                    {
                        feed.Id = idUri;
                    }
                }

                //------------------------------------------------------------
                //	Extract <title> node
                //------------------------------------------------------------
                XPathNavigator feedTitle            = navigator.SelectSingleNode("atom:title", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(feedTitle))
                {
                    AtomText title  = AtomXmlSyndicationFeedAdapter.CreateTextConstruct(feedTitle);
                    if (title != null)
                    {
                        feed.Title  = title;
                    }
                }

                //------------------------------------------------------------
                //	Extract <updated> node
                //------------------------------------------------------------
                XPathNavigator feedUpdated          = navigator.SelectSingleNode("atom:updated", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(feedUpdated))
                {
                    W3CDateTime updatedOn;
                    if (W3CDateTime.TryParse(feedUpdated.Value, out updatedOn))
                    {
                        feed.UpdatedOn  = updatedOn;
                    }
                }

                //------------------------------------------------------------
                //	Extract feed optional elements
                //------------------------------------------------------------
                this.ModifyFeedOptionals(feed, navigator);

                //------------------------------------------------------------
                //	Extract <author> nodes
                //------------------------------------------------------------
                XPathNodeIterator feedAuthors       = navigator.Select("atom:author", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.IteratorContainsNodes(feedAuthors))
                {
                    this.UpdatePersons(feed.Authors, feedAuthors);
                }

                //------------------------------------------------------------
                //	Extract <category> nodes
                //------------------------------------------------------------
                XPathNodeIterator feedCategories    = navigator.Select("atom:category", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.IteratorContainsNodes(feedCategories))
                {
                    AtomXmlSyndicationFeedAdapter.UpdateCategories(feed.Categories, feedCategories);
                }

                //------------------------------------------------------------
                //	Extract <contributor> nodes
                //------------------------------------------------------------
                XPathNodeIterator feedContributors  = navigator.Select("atom:contributor", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.IteratorContainsNodes(feedContributors))
                {
                    this.UpdatePersons(feed.Contributors, feedContributors);
                }

                //------------------------------------------------------------
                //	Extract <link> nodes
                //------------------------------------------------------------
                XPathNodeIterator feedLinks = navigator.Select("atom:link", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.IteratorContainsNodes(feedLinks))
                {
                    AtomXmlSyndicationFeedAdapter.UpdateLinks(feed.Links, feedLinks);
                }

                //------------------------------------------------------------
                //	Extract <entry> nodes
                //------------------------------------------------------------
                XPathNodeIterator feedEntries       = navigator.Select("atom:entry", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.IteratorContainsNodes(feedEntries))
                {
                    this.UpdateEntries(feed.Entries, feedEntries);
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

        #region ModifyFeedOptionals(AtomFeed feed, XPathNavigator xmlSource)
        /// <summary>
        /// Updates the <see cref="AtomFeed"/> optional elements using information extracted from the provided <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="feed">The <see cref="AtomFeed"/> to modify.</param>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract Atom feed information from.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private void ModifyFeedOptionals(AtomFeed feed, XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Attempt to update feed state
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
                if (navigator == null)
                {
                    throw new ArgumentNullException("navigator");
                }

                //------------------------------------------------------------
                //	Extract <generator> node
                //------------------------------------------------------------
                XPathNavigator feedGenerator    = navigator.SelectSingleNode("atom:generator", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(feedGenerator))
                {
                    AtomGenerator generator     = AtomXmlSyndicationFeedAdapter.CreateGenerator(feedGenerator);
                    if (generator != null)
                    {
                        feed.Generator  = generator;
                    }
                }

                //------------------------------------------------------------
                //	Extract <icon> node
                //------------------------------------------------------------
                XPathNavigator feedIcon = navigator.SelectSingleNode("atom:icon", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(feedIcon))
                {
                    Uri iconUri;
                    if (Uri.TryCreate(feedIcon.Value, UriKind.RelativeOrAbsolute, out iconUri))
                    {
                        feed.Icon   = iconUri;
                    }
                }

                //------------------------------------------------------------
                //	Extract <logo> node
                //------------------------------------------------------------
                XPathNavigator feedLogo = navigator.SelectSingleNode("atom:logo", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(feedLogo))
                {
                    Uri logoUri;
                    if (Uri.TryCreate(feedLogo.Value, UriKind.RelativeOrAbsolute, out logoUri))
                    {
                        feed.Logo   = logoUri;
                    }
                }

                //------------------------------------------------------------
                //	Extract <rights> node
                //------------------------------------------------------------
                XPathNavigator feedRights   = navigator.SelectSingleNode("atom:rights", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(feedRights))
                {
                    AtomText rights         = AtomXmlSyndicationFeedAdapter.CreateTextConstruct(feedRights);
                    if (rights != null)
                    {
                        feed.Rights = rights;
                    }
                }

                //------------------------------------------------------------
                //	Extract <subtitle> node
                //------------------------------------------------------------
                XPathNavigator feedSubTitle = navigator.SelectSingleNode("atom:subtitle", this.NamespaceManager);
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(feedSubTitle))
                {
                    AtomText description    = AtomXmlSyndicationFeedAdapter.CreateTextConstruct(feedSubTitle);
                    if (description != null)
                    {
                        feed.Description    = description;
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
                    AtomXmlSyndicationFeedAdapter.WriteEntry(entry, writer);
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
                    AtomXmlSyndicationFeedAdapter.WriteTextConstruct(entry.Title, "title", writer);
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
                    AtomXmlSyndicationFeedAdapter.WriteContentConstruct(entry.Content, writer);
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
                    AtomXmlSyndicationFeedAdapter.WriteTextConstruct(entry.Rights, "rights", writer);
                }

                //------------------------------------------------------------
                //	Write <summary> element
                //------------------------------------------------------------
                if (entry.Summary != null)
                {
                    AtomXmlSyndicationFeedAdapter.WriteTextConstruct(entry.Summary, "summary", writer);
                }

                //------------------------------------------------------------
                //	Write <author> elements
                //------------------------------------------------------------
                if (entry.Authors != null && entry.Authors.Count > 0)
                {
                    AtomXmlSyndicationFeedAdapter.WritePersons(entry.Authors, "author", writer);
                }

                //------------------------------------------------------------
                //	Write <category> elements
                //------------------------------------------------------------
                if (entry.Categories != null && entry.Categories.Count > 0)
                {
                    AtomXmlSyndicationFeedAdapter.WriteCategories(entry.Categories, writer);
                }

                //------------------------------------------------------------
                //	Write <contributor> elements
                //------------------------------------------------------------
                if (entry.Contributors != null && entry.Contributors.Count > 0)
                {
                    AtomXmlSyndicationFeedAdapter.WritePersons(entry.Contributors, "contributor", writer);
                }

                //------------------------------------------------------------
                //	Write <link> elements
                //------------------------------------------------------------
                if (entry.Links != null && entry.Links.Count > 0)
                {
                    AtomXmlSyndicationFeedAdapter.WriteLinks(entry.Links, writer);
                }

                //------------------------------------------------------------
                //	Write <source> element
                //------------------------------------------------------------
                if (entry.Source != null)
                {
                    //------------------------------------------------------------
                    //	
                    //------------------------------------------------------------
                    AtomXmlSyndicationFeedAdapter.WriteFeedContent(entry.Source, writer);
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

        #region WriteFeed(AtomFeed feed, XmlWriter writer, XmlWriterType writerType)
        /// <summary>
        /// Writes the <see cref="AtomFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="feed">The <see cref="AtomFeed"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is written to.</param>
        /// <param name="writerType">A <see cref="XmlWriterType"/> enumeration value indicating the source/type of the <b>XmlWriter</b>.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="writerType"/> is <b>XmlWriterType.None</b>.</exception>
        private static void WriteFeed(AtomFeed feed, XmlWriter writer, XmlWriterType writerType)
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
                switch(writerType)
                {
                    case XmlWriterType.Serialized:

                        //------------------------------------------------------------
                        //	Write syndication feed version attribute
                        //------------------------------------------------------------
                        writer.WriteAttributeString("version", feed.Version);

                        //------------------------------------------------------------
                        //	Write feed content
                        //------------------------------------------------------------
                        AtomXmlSyndicationFeedAdapter.WriteFeedContent(feed, writer);

                        break;

                    case XmlWriterType.Standard:

                        //------------------------------------------------------------
                        //	Write <feed> element with default XML namespace
                        //------------------------------------------------------------
                        writer.WriteStartElement("feed", ATOM_XML_NAMESPACE);

                        //------------------------------------------------------------
                        //	Write syndication feed version attribute
                        //------------------------------------------------------------
                        writer.WriteAttributeString("version", feed.Version);

                        //------------------------------------------------------------
                        //	Write feed content
                        //------------------------------------------------------------
                        AtomXmlSyndicationFeedAdapter.WriteFeedContent(feed, writer);

                        //------------------------------------------------------------
                        //	Write </feed> element
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
                    AtomXmlSyndicationFeedAdapter.WriteTextConstruct(feed.Title, "title", writer);
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
                AtomXmlSyndicationFeedAdapter.WriteFeedOptionals(feed, writer);

                //------------------------------------------------------------
                //	Write <author> elements
                //------------------------------------------------------------
                if (feed.Authors != null && feed.Authors.Count > 0)
                {
                    AtomXmlSyndicationFeedAdapter.WritePersons(feed.Authors, "author", writer);
                }

                //------------------------------------------------------------
                //	Write <category> elements
                //------------------------------------------------------------
                if (feed.Categories != null && feed.Categories.Count > 0)
                {
                    AtomXmlSyndicationFeedAdapter.WriteCategories(feed.Categories, writer);
                }

                //------------------------------------------------------------
                //	Write <contributor> elements
                //------------------------------------------------------------
                if (feed.Contributors != null && feed.Contributors.Count > 0)
                {
                    AtomXmlSyndicationFeedAdapter.WritePersons(feed.Contributors, "contributor", writer);
                }

                //------------------------------------------------------------
                //	Write <link> elements
                //------------------------------------------------------------
                if (feed.Links != null && feed.Links.Count > 0)
                {
                    AtomXmlSyndicationFeedAdapter.WriteLinks(feed.Links, writer);
                }

                //------------------------------------------------------------
                //	Write <entry> elements
                //------------------------------------------------------------
                if (feed.Entries != null && feed.Entries.Count > 0)
                {
                    AtomXmlSyndicationFeedAdapter.WriteEntries(feed.Entries, writer);
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
                    AtomXmlSyndicationFeedAdapter.WriteGenerator(feed.Generator, writer);
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
                    AtomXmlSyndicationFeedAdapter.WriteTextConstruct(feed.Rights, "rights", writer);
                }

                //------------------------------------------------------------
                //	Write <subtitle> element
                //------------------------------------------------------------
                if (feed.Description != null)
                {
                    AtomXmlSyndicationFeedAdapter.WriteTextConstruct(feed.Description, "subtitle", writer);
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
                    AtomXmlSyndicationFeedAdapter.WriteCategory(category, writer);
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
                    AtomXmlSyndicationFeedAdapter.WriteLink(link, writer);
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
                    AtomXmlSyndicationFeedAdapter.WritePerson(person, elementName, writer);
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

        //============================================================
        //	ENTITY INSTANTIATION ROUTINES
        //============================================================
        #region CreateCategory(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="AtomCategory"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="AtomCategory"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static AtomCategory CreateCategory(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomCategory category   = new AtomCategory();
            bool propertySet        = false;

            //------------------------------------------------------------
            //	Attempt to create content
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
                //	Extract category value and (optional) domain identifier
                //------------------------------------------------------------
                string categoryContent  = navigator.Value;
                string categoryTerm     = navigator.GetAttribute("term", String.Empty);
                string categoryScheme   = navigator.GetAttribute("scheme", String.Empty);
                string categoryLabel    = navigator.GetAttribute("label", String.Empty);

                //------------------------------------------------------------
                //	Set properties
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(categoryTerm))
                {
                    category.Term       = categoryTerm;
                    propertySet         = true;
                }

                if (!String.IsNullOrEmpty(categoryScheme))
                {
                    Uri schemeUri;
                    if (Uri.TryCreate(categoryScheme, UriKind.RelativeOrAbsolute, out schemeUri))
                    {
                        category.Scheme = schemeUri;
                        propertySet     = true;
                    }
                }

                if (!String.IsNullOrEmpty(categoryLabel))
                {
                    category.Label      = categoryLabel;
                    propertySet         = true;
                }

                if (!String.IsNullOrEmpty(categoryContent))
                {
                    category.Content    = categoryContent;
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
                return category;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreateContentConstruct(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="AtomContent"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="AtomContent"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static AtomContent CreateContentConstruct(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomContent contentConstruct    = new AtomContent();
            bool propertySet                = false;

            //------------------------------------------------------------
            //	Attempt to create content
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
                //	Extract information for content
                //------------------------------------------------------------
                string contentValue     = navigator.Value;
                string contentType      = navigator.GetAttribute("type", String.Empty);
                string contentSource    = navigator.GetAttribute("src", String.Empty);

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(contentValue))
                {
                    contentConstruct.Value          = contentValue;
                    propertySet                     = true;
                }

                if (!String.IsNullOrEmpty(contentType))
                {
                    TextType type                   = AtomContent.TextTypeFromString(contentType);
                    if (type != TextType.None)
                    {
                        contentConstruct.Type       = type;
                    }
                    else
                    {
                        contentConstruct.MediaType  = contentType;
                    }
                    propertySet                     = true;
                }

                if (!String.IsNullOrEmpty(contentSource))
                {
                    Uri sourceUri;
                    if (Uri.TryCreate(contentSource, UriKind.RelativeOrAbsolute, out sourceUri))
                    {
                        contentConstruct.Source     = sourceUri;
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
                return contentConstruct;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreateGenerator(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="AtomGenerator"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="AtomGenerator"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static AtomGenerator CreateGenerator(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomGenerator generator = new AtomGenerator();
            bool propertySet        = false;

            //------------------------------------------------------------
            //	Attempt to create content
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
                //	Extract information for generator
                //------------------------------------------------------------
                string generatorName    = navigator.Value;
                string generatorUri     = navigator.GetAttribute("uri", String.Empty);
                string generatorVersion = navigator.GetAttribute("version", String.Empty);

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(generatorName))
                {
                    generator.Name      = generatorName;
                    propertySet         = true;
                }

                if (!String.IsNullOrEmpty(generatorUri))
                {
                    Uri iriUri;
                    if (Uri.TryCreate(generatorUri, UriKind.RelativeOrAbsolute, out iriUri))
                    {
                        generator.Uri   = iriUri;
                        propertySet     = true;
                    }
                }

                if (!String.IsNullOrEmpty(generatorVersion))
                {
                    generator.Version   = generatorVersion;
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
                return generator;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreateLink(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="AtomLink"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="AtomLink"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static AtomLink CreateLink(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomLink link           = new AtomLink();
            bool propertySet        = false;

            //------------------------------------------------------------
            //	Attempt to create content
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
                //	Extract category value and (optional) domain identifier
                //------------------------------------------------------------
                string linkContent  = navigator.Value;
                string linkHref     = navigator.GetAttribute("href", String.Empty);
                string linkRel      = navigator.GetAttribute("rel", String.Empty);
                string linkType     = navigator.GetAttribute("type", String.Empty);
                string linkHrefLang = navigator.GetAttribute("hreflang", String.Empty);
                string linkTitle    = navigator.GetAttribute("title", String.Empty);
                string linkLength   = navigator.GetAttribute("length", String.Empty);

                //------------------------------------------------------------
                //	Set properties
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(linkHref))
                {
                    Uri hrefUri;
                    if (Uri.TryCreate(linkHref, UriKind.RelativeOrAbsolute, out hrefUri))
                    {
                        link.Link       = hrefUri;
                        propertySet     = true;
                    }
                }

                if (!String.IsNullOrEmpty(linkRel))
                {
                    LinkRelation relation   = AtomLink.RelationFromString(linkRel);
                    link.Relation           = relation != LinkRelation.None ? relation : LinkRelation.Alternate;
                    propertySet             = true;
                }

                if (!String.IsNullOrEmpty(linkType))
                {
                    link.Type       = linkType;
                    propertySet     = true;
                }

                if (!String.IsNullOrEmpty(linkHrefLang))
                {
                    link.Language   = linkHrefLang;
                    propertySet     = true;
                }

                if (!String.IsNullOrEmpty(linkTitle))
                {
                    link.Title      = linkTitle;
                    propertySet     = true;
                }

                if (!String.IsNullOrEmpty(linkLength))
                {
                    long length;
                    if (Int64.TryParse(linkLength, NumberStyles.Integer, CultureInfo.InvariantCulture, out length))
                    {
                        link.Length = length;
                        propertySet = true;
                    }
                }

                if (!String.IsNullOrEmpty(linkContent))
                {
                    link.Content    = linkContent;
                    propertySet     = true;
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
                return link;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreatePerson(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="AtomPerson"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="AtomPerson"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private AtomPerson CreatePerson(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomPerson person   = new AtomPerson();
            bool propertySet    = false;

            //------------------------------------------------------------
            //	Attempt to create content
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
                //	Extract information for person
                //------------------------------------------------------------
                XPathNavigator personName   = navigator.SelectSingleNode("atom:name", this.NamespaceManager);
                XPathNavigator personUri    = navigator.SelectSingleNode("atom:uri", this.NamespaceManager);
                XPathNavigator personEmail  = navigator.SelectSingleNode("atom:email", this.NamespaceManager);

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(personName))
                {
                    person.Name         = personName.Value;
                    propertySet         = true;
                }

                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(personUri))
                {
                    Uri iriUri;
                    if (Uri.TryCreate(personUri.Value, UriKind.RelativeOrAbsolute, out iriUri))
                    {
                        person.Uri      = iriUri;
                        propertySet     = true;
                    }
                }

                if (AtomXmlSyndicationFeedAdapter.ItemContainsText(personEmail))
                {
                    person.EmailAddress = personEmail.Value;
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
                return person;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CreateTextConstruct(XPathNavigator navigator)
        /// <summary>
        /// Creates an <see cref="AtomText"/> instance using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns>A <see cref="AtomText"/> instance using the available information. If no information available, return a null reference (Nothing in Visual Basic).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static AtomText CreateTextConstruct(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomText textConstruct  = new AtomText();
            bool propertySet        = false;

            //------------------------------------------------------------
            //	Attempt to create text
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
                //	Extract information for text
                //------------------------------------------------------------
                string textValue            = navigator.Value;
                string textType             = navigator.GetAttribute("type", String.Empty);

                //------------------------------------------------------------
                //	Set properties for non-empty values
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(textValue))
                {
                    textConstruct.Value     = textValue;
                    textConstruct.Type      = TextType.Text;    // Set default value for type if textual content present
                    propertySet             = true;
                }

                if (!String.IsNullOrEmpty(textType))
                {
                    TextType type           = AtomText.TextTypeFromString(textType);
                    if (type != TextType.None)
                    {
                        textConstruct.Type  = type;
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
                return textConstruct;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region UpdateCategories(AtomCategoryCollection categories, XPathNodeIterator iterator)
        /// <summary>
        /// Updates the specified <see cref="AtomCategoryCollection"/> instance using the supplied <see cref="XPathNodeIterator"/>.
        /// </summary>
        /// <param name="categories">The <see cref="AtomCategoryCollection"/> to update.</param>
        /// <param name="iterator">The <see cref="XPathNodeIterator"/> to extract information from.</param>
        /// <remarks>Calling this method clears the specified collection prior to attempting to add items to collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="categories"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="iterator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void UpdateCategories(AtomCategoryCollection categories, XPathNodeIterator iterator)
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
                        //	Create category using current XML data
                        //------------------------------------------------------------
                        AtomCategory category   = AtomXmlSyndicationFeedAdapter.CreateCategory(iterator.Current);

                        //------------------------------------------------------------
                        //	Verify category was created
                        //------------------------------------------------------------
                        if (category != null)
                        {
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

        #region UpdateEntries(AtomEntryCollection entries, XPathNodeIterator iterator)
        /// <summary>
        /// Updates the specified <see cref="AtomEntryCollection"/> instance using the supplied <see cref="XPathNodeIterator"/>.
        /// </summary>
        /// <param name="entries">The <see cref="AtomEntryCollection"/> to update.</param>
        /// <param name="iterator">The <see cref="XPathNodeIterator"/> to extract information from.</param>
        /// <remarks>Calling this method clears the specified collection prior to attempting to add items to collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="entries"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="iterator"/> is a null reference (Nothing in Visual Basic).</exception>
        private void UpdateEntries(AtomEntryCollection entries, XPathNodeIterator iterator)
        {
            //------------------------------------------------------------
            //	Attempt to create collection
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
                if (iterator == null)
                {
                    throw new ArgumentNullException("iterator");
                }

                //------------------------------------------------------------
                //	Clear collection
                //------------------------------------------------------------
                entries.Clear();

                //------------------------------------------------------------
                //	Walk iterator nodes
                //------------------------------------------------------------
                while (iterator.MoveNext())
                {
                    //------------------------------------------------------------
                    //	Verify current node is valid
                    //------------------------------------------------------------
                    if (AtomXmlSyndicationFeedAdapter.NavigatorContainsChildren(iterator.Current))
                    {
                        //------------------------------------------------------------
                        //	Create feed entry instance
                        //------------------------------------------------------------
                        AtomEntry entry = new AtomEntry();

                        //------------------------------------------------------------
                        //	Update feed entry using XML data
                        //------------------------------------------------------------
                        this.ModifyEntry(entry, iterator.Current);

                        //------------------------------------------------------------
                        //	Add feed entry to collection
                        //------------------------------------------------------------
                        entries.Add(entry);
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

        #region UpdateLinks(AtomLinkCollection links, XPathNodeIterator iterator)
        /// <summary>
        /// Updates the specified <see cref="AtomLinkCollection"/> instance using the supplied <see cref="XPathNodeIterator"/>.
        /// </summary>
        /// <param name="links">The <see cref="AtomLinkCollection"/> to update.</param>
        /// <param name="iterator">The <see cref="XPathNodeIterator"/> to extract information from.</param>
        /// <remarks>Calling this method clears the specified collection prior to attempting to add items to collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="links"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="iterator"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void UpdateLinks(AtomLinkCollection links, XPathNodeIterator iterator)
        {
            //------------------------------------------------------------
            //	Attempt to create collection
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
                if (iterator == null)
                {
                    throw new ArgumentNullException("iterator");
                }

                //------------------------------------------------------------
                //	Clear collection
                //------------------------------------------------------------
                links.Clear();

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
                        //	Create link using current XML data
                        //------------------------------------------------------------
                        AtomLink link   = AtomXmlSyndicationFeedAdapter.CreateLink(iterator.Current);

                        //------------------------------------------------------------
                        //	Verify link was created
                        //------------------------------------------------------------
                        if (link != null)
                        {
                            //------------------------------------------------------------
                            //	Add link to collection
                            //------------------------------------------------------------
                            if (!links.Contains(link))
                            {
                                links.Add(link);
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

        #region UpdatePersons(AtomPersonCollection persons, XPathNodeIterator iterator)
        /// <summary>
        /// Updates the specified <see cref="AtomPersonCollection"/> instance using the supplied <see cref="XPathNodeIterator"/>.
        /// </summary>
        /// <param name="persons">The <see cref="AtomPersonCollection"/> to update.</param>
        /// <param name="iterator">The <see cref="XPathNodeIterator"/> to extract information from.</param>
        /// <remarks>Calling this method clears the specified collection prior to attempting to add items to collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="persons"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="iterator"/> is a null reference (Nothing in Visual Basic).</exception>
        private void UpdatePersons(AtomPersonCollection persons, XPathNodeIterator iterator)
        {
            //------------------------------------------------------------
            //	Attempt to create collection
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
                if (iterator == null)
                {
                    throw new ArgumentNullException("iterator");
                }

                //------------------------------------------------------------
                //	Clear collection
                //------------------------------------------------------------
                persons.Clear();

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
                        //	Create person using current XML data
                        //------------------------------------------------------------
                        AtomPerson person   = this.CreatePerson(iterator.Current);

                        //------------------------------------------------------------
                        //	Verify person was created
                        //------------------------------------------------------------
                        if (person != null)
                        {
                            //------------------------------------------------------------
                            //	Add person to collection
                            //------------------------------------------------------------
                            if (!persons.Contains(person))
                            {
                                persons.Add(person);
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
