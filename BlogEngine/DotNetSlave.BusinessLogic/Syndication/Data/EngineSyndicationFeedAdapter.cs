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

        #region EngineSyndicationFeedAdapter(List<Post> posts)
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSyndicationFeedAdapter"/> class using the supplied collection of <see cref="Post"/> instances.
        /// </summary>
        /// <param name="posts">The collection of blog posts that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic).</exception>
        protected EngineSyndicationFeedAdapter(List<Post> posts)
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

                //------------------------------------------------------------
                //	Set class members
                //------------------------------------------------------------
                adapterBlogPosts    = posts;
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

        #region EngineSyndicationFeedAdapter(List<Post> posts, BlogSettings blogSettings)
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSyndicationFeedAdapter"/> class using the supplied collection of <see cref="Post"/> instances and <see cref="BlogSettings"/>.
        /// </summary>
        /// <param name="posts">The collection of blog posts that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</param>
        /// <param name="blogSettings">The <see cref="BlogSettings"/> that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="posts"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="blogSettings"/> is a null reference (Nothing in Visual Basic).</exception>
        protected EngineSyndicationFeedAdapter(List<Post> posts, BlogSettings blogSettings)
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

                //------------------------------------------------------------
                //	Set class members
                //------------------------------------------------------------
                adapterBlogPosts    = posts;
                adapterBlogSettings = blogSettings;
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
    }
}
