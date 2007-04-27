/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/24/2007	brian.kuhn		Created EngineSyndicationFeedAdapter Class
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Data
{
    /// <summary>
    /// Defines the base implementation of the <see cref="ISyndicationFeedAdapter"/> interface that provides a set of methods and properties used to fill or write <see cref="SyndicationFeed"/> instances from blog engine data source(s).
    /// </summary>
    /// <remarks>This abstract class should be inherited from to provide a blog engine data adapter for a specific syndication feed format.</remarks>
    public abstract class EngineSyndicationFeedAdapter : ISyndicationFeedAdapter
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the set of features that the data adapter supports.
        /// </summary>
        private SyndicationFeedSettings adapterSettings    = new SyndicationFeedSettings();
        /// <summary>
        /// Private member to hold the collection of blog posts used to fill a syndication feed.
        /// </summary>
        private List<Post> adapterBlogPosts;
        /// <summary>
        /// Private member to hold the blog settings used to fill a syndication feed.
        /// </summary>
        private BlogSettings adapterBlogSettings;
        /// <summary>
        /// Private member to hold the categories defined for the blog.
        /// </summary>
        private CategoryDictionary adapterCategories;
        /// <summary>
        /// Private member to hold the URI of a syndication feed's absolute root web.
        /// </summary>
        private Uri adapterWebRoot;
        /// <summary>
        /// Private member to hold the URI of a syndication feed being filled.
        /// </summary>
        private Uri adapterFeedLocation;
        /// <summary>
        /// Private member to hold the URI of the OPML blogroll associated to the feed.
        /// </summary>
        private Uri adapterBlogroll;
        /// <summary>
        /// Private member to hold the URI of the OPML subscriptions associated to the feed.
        /// </summary>
        private Uri adapterSubscriptions;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region EngineSyndicationFeedAdapter()
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSyndicationFeedAdapter"/> class.
        /// </summary>
        protected EngineSyndicationFeedAdapter()
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

        #region EngineSyndicationFeedAdapter(List<Post> posts, BlogSettings blogSettings, CategoryDictionary categories)
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSyndicationFeedAdapter"/> class using the supplied collection of <see cref="Post"/> instances and <see cref="BlogSettings"/>.
        /// </summary>
        /// <param name="posts">The collection of blog posts that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</param>
        /// <param name="blogSettings">The <see cref="BlogSettings"/> that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</param>
        /// <param name="categories">The <see cref="CategoryDictionary"/> that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="blogSettings"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="categories"/> is a null reference (Nothing in Visual Basic).</exception>
        protected EngineSyndicationFeedAdapter(List<Post> posts, BlogSettings blogSettings, CategoryDictionary categories)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (posts == null)
                {
                    throw new ArgumentNullException("posts");
                }
                if (blogSettings == null)
                {
                    throw new ArgumentNullException("blogSettings");
                }
                if (categories == null)
                {
                    throw new ArgumentNullException("categories");
                }

                //------------------------------------------------------------
                //	Set class members
                //------------------------------------------------------------
                adapterBlogPosts    = posts;
                adapterBlogSettings = blogSettings;
                adapterCategories   = categories;
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

        #region EngineSyndicationFeedAdapter(SyndicationFeedSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSyndicationFeedAdapter"/> class using the supplied <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="settings">The set of features that the XML data adapter supports.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        protected EngineSyndicationFeedAdapter(SyndicationFeedSettings settings)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                //------------------------------------------------------------
                //	Set class members
                //------------------------------------------------------------
                adapterSettings = settings;
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
        #region Blogroll
        /// <summary>
        /// Gets or sets the URI location of an OPML blogroll associated to the <see cref="SyndicationFeed"/> being filled.
        /// </summary>
        /// <value>The <see cref="Uri"/> of the OPML blogroll associated to the <see cref="SyndicationFeed"/> being filled.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Blogroll
        {
            get
            {
                return adapterBlogroll;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    adapterBlogroll = value;
                }
            }
        }
        #endregion

        #region BlogSettings
        /// <summary>
        /// Gets or sets the <see cref="BlogSettings"/> that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.
        /// </summary>
        /// <value>The <see cref="BlogSettings"/> that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public BlogSettings BlogSettings
        {
            get
            {
                return adapterBlogSettings;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    adapterBlogSettings = value;
                }
            }
        }
        #endregion

        #region Categories
        /// <summary>
        /// Gets the <see cref="CategoryDictionary"/> that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.
        /// </summary>
        /// <value>The <see cref="CategoryDictionary"/> that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</value>
        public CategoryDictionary Categories
        {
            get
            {
                return adapterCategories;
            }
        }
        #endregion

        #region FeedLocation
        /// <summary>
        /// Gets or sets the URI location of the feed that the syndication feed adapter is filling.
        /// </summary>
        /// <value>The <see cref="Uri"/> of the <see cref="SyndicationFeed"/> being filled.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri FeedLocation
        {
            get
            {
                return adapterFeedLocation;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    adapterFeedLocation = value;
                }
            }
        }
        #endregion

        #region Posts
        /// <summary>
        /// Gets the collection of blog posts that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.
        /// </summary>
        /// <value>The collection of <see cref="Post"/> instances that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</value>
        public List<Post> Posts
        {
            get
            {
                return adapterBlogPosts;
            }
        }
        #endregion

        #region Settings
        /// <summary>
        /// Gets or sets the set of features that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.
        /// </summary>
        /// <value>The <see cref="SyndicationFeedSettings"/> that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationFeedSettings Settings
        {
            get
            {
                return adapterSettings;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    adapterSettings = value;
                }
            }
        }
        #endregion

        #region Subscriptions
        /// <summary>
        /// Gets or sets the URI location of a subscriptions OPML document associated to the <see cref="SyndicationFeed"/> being filled.
        /// </summary>
        /// <value>The <see cref="Uri"/> of a subscriptions OPML document associated to the <see cref="SyndicationFeed"/> being filled.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Subscriptions
        {
            get
            {
                return adapterSubscriptions;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    adapterSubscriptions = value;
                }
            }
        }
        #endregion

        #region WebRoot
        /// <summary>
        /// Gets or sets the URI location of the feed's absolute web root.
        /// </summary>
        /// <value>The <see cref="Uri"/> location of the feed's absolute web root.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri WebRoot
        {
            get
            {
                return adapterWebRoot;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    adapterWebRoot = value;
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region Fill(SyndicationFeed feed)
        /// <summary>
        /// Adds or refreshes items/entries in the <see cref="SyndicationFeed"/> to match those in the blog engine data source(s).
        /// </summary>
        /// <param name="feed">A <see cref="SyndicationFeed"/> to fill using the underlying blog engine data source(s).</param>
        /// <returns>The number of items/entries successfully added to or refreshed in the <b>SyndicationFeed</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Fill</b> retrieves syndication feed information from the blog engine data source(s).
        ///     </para>
        /// 
        ///     <para>
        ///         The <b>Fill</b> operation then sets the <b>SyndicationFeed</b> properties and adds items/entries to the feed, creating the syndication feed entities if they do not already exist.
        ///     </para>
        /// 
        ///     <para>
        ///         If the <b>EngineSyndicationFeedAdapter</b> will also add supported extensions to the <b>SyndicationFeed</b> using the supported extensions configured in the <see cref="EngineSyndicationFeedAdapter.Settings"/> property.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic).</exception>
        public abstract int Fill(SyndicationFeed feed);
        #endregion

        #region Write(SyndicationFeed feed, XmlWriter writer)
        /// <summary>
        /// Writes the <see cref="SyndicationFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="feed">The <see cref="SyndicationFeed"/> to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is written to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">An exception occurred while writing the syndication feed XML data.</exception>
        public abstract void Write(SyndicationFeed feed, XmlWriter writer);
        #endregion

        //============================================================
        //	PUBLIC FORMATTING ROUTINES
        //============================================================
        #region MakeReferencesAbsolute(string content)
        /// <summary>
        /// Replaces references to handlers in the specified content with their absolute representation.
        /// </summary>
        /// <param name="content">The content to format.</param>
        /// <returns>A <see cref="System.String"/> that has had its relative handler references changed to their absolute representation.</returns>
        public string MakeReferencesAbsolute(string content)
        {
            //------------------------------------------------------------
            //	Attempt to make references absolute
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Perform conversion
                //------------------------------------------------------------
                content = content.Replace("\"/image.axd", "\"" + this.WebRoot + "image.axd");
                content = content.Replace("\"/file.axd", "\"" + this.WebRoot + "file.axd");
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
            return content;
        }
        #endregion
    }
}
