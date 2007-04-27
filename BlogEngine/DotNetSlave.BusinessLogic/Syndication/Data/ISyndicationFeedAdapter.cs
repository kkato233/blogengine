/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/11/2007	brian.kuhn		Created ISyndicationFeedAdapter Interface
04/27/2007  brian.kuhn      Added IDisposable Interface
****************************************************************************/
using System;
using System.Xml;

namespace BlogEngine.Core.Syndication.Data
{
    /// <summary>
    /// Allows an object to implement a DataAdapter, and represents a set of methods and properties used to fill or write a <see cref="SyndicationFeed"/>.
    /// </summary>
    /// <remarks>The <b>ISyndicationFeedAdapter</b> interface allows an inheriting class to implement a DataAdapter class, which represents the bridge between a data source and a <see cref="SyndicationFeed"/>.</remarks>
    /// <seealso cref="EngineSyndicationFeedAdapter"/>
    public interface ISyndicationFeedAdapter : IDisposable
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Settings
        /// <summary>
        /// Gets or sets the set of features that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.
        /// </summary>
        /// <value>The <see cref="SyndicationFeedSettings"/> that the syndication feed adapter uses when filling a <see cref="SyndicationFeed"/>.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        SyndicationFeedSettings Settings
        {
            get;
            set;
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region Fill(SyndicationFeed feed)
        /// <summary>
        /// Adds or refreshes items/entries in the <see cref="SyndicationFeed"/> to match those in the data source.
        /// </summary>
        /// <param name="feed">A <see cref="SyndicationFeed"/> to fill using the underlying data source.</param>
        /// <returns>The number of items/entries successfully added to or refreshed in the <b>SyndicationFeed</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Fill</b> retrieves syndication feed information from the data source.
        ///     </para>
        /// 
        ///     <para>
        ///         The <b>Fill</b> operation then sets the <b>SyndicationFeed</b> properties and adds items/entries to the feed, creating the syndication feed entities if they do not already exist.
        ///     </para>
        /// 
        ///     <para>
        ///         If the <b>ISyndicationFeedAdapter</b> will also add supported extensions to the <b>SyndicationFeed</b> using the supported extensions configured in the <see cref="ISyndicationFeedAdapter.Settings"/> property.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="feed"/> is a null reference (Nothing in Visual Basic).</exception>
        int Fill(SyndicationFeed feed);
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
        void Write(SyndicationFeed feed, XmlWriter writer);
        #endregion
    }
}
