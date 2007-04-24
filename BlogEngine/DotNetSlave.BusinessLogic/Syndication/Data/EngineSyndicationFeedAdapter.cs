/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/24/2007	brian.kuhn		Created EngineSyndicationFeedAdapter Class
****************************************************************************/
using System;
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

        //============================================================
        //	VALIDATION ROUTINES
        //============================================================
        #region ItemContainsText(XPathItem item)
        /// <summary>
        /// Verifies that the specified <see cref="XPathItem"/> is not null and contains a non-empty string value. 
        /// </summary>
        /// <param name="item">The <see cref="XPathItem"/> to validate.</param>
        /// <returns><b>true</b> if item is not null and its value is not an empty string, otherwise returns <b>false</b>.</returns>
        public static bool ItemContainsText(XPathItem item)
        {
            //------------------------------------------------------------
            //	Attempt to validate navigator
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate navigator state
                //------------------------------------------------------------
                return (item != null && !String.IsNullOrEmpty(item.Value));
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

        #region IteratorContainsNodes(XPathNodeIterator iterator)
        /// <summary>
        /// Verifies that the specified <see cref="XPathNodeIterator"/> is not null and has at least one node. 
        /// </summary>
        /// <param name="iterator">The <see cref="XPathNodeIterator"/> to validate.</param>
        /// <returns><b>true</b> if iterator is not null and has at least one XML node, otherwise returns <b>false</b>.</returns>
        public static bool IteratorContainsNodes(XPathNodeIterator iterator)
        {
            //------------------------------------------------------------
            //	Attempt to validate navigator
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate iterator state
                //------------------------------------------------------------
                return (iterator != null && iterator.Count > 0);
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

        #region NavigatorContainsChildren(XPathNavigator navigator)
        /// <summary>
        /// Verifies that the specified <see cref="XPathNavigator"/> is not null and contains child nodes. 
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to validate.</param>
        /// <returns><b>true</b> if navigator is not null and it has child nodes, otherwise returns <b>false</b>.</returns>
        public static bool NavigatorContainsChildren(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Attempt to validate navigator
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate navigator state
                //------------------------------------------------------------
                return (navigator != null && navigator.HasChildren);
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

        #region NavigatorContainsData(XPathNavigator navigator)
        /// <summary>
        /// Verifies that the specified <see cref="XPathNavigator"/> is not null and contains a non-empty string value OR has attributes. 
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to validate.</param>
        /// <returns><b>true</b> if navigator is not null and its value is not an empty string OR has at least one XML attribute, otherwise returns <b>false</b>.</returns>
        public static bool NavigatorContainsData(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Attempt to validate navigator
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate navigator state
                //------------------------------------------------------------
                return (navigator != null && (!String.IsNullOrEmpty(navigator.Value) || navigator.HasAttributes));
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

        #region NavigatorHasAttributes(XPathNavigator navigator)
        /// <summary>
        /// Verifies that the specified <see cref="XPathNavigator"/> is not null and contains attributes. 
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to validate.</param>
        /// <returns><b>true</b> if navigator is not null and has at least one XML attribute, otherwise returns <b>false</b>.</returns>
        public static bool NavigatorHasAttributes(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Attempt to validate navigator
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate navigator state
                //------------------------------------------------------------
                return (navigator != null && navigator.HasAttributes);
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
