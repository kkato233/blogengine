/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/12/2007	brian.kuhn		Created AtomFeed Class
****************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Security;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;
using BlogEngine.Core.Syndication.Data;

namespace BlogEngine.Core.Syndication.Atom
{
    /// <summary>
    /// Represents an Atom syndication feed.
    /// </summary>
    [Serializable()]
    [XmlRoot(ElementName = "feed", Namespace = "http://www.w3.org/2005/Atom", DataType = "feedType")]
    public class AtomFeed : SyndicationFeed
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the version of the syndication format that this feed conforms to.
        /// </summary>
        private string feedVersion              = "1.0";
        /// <summary>
        /// Private member to hold the syndication format that this feed implements.
        /// </summary>
        private SyndicationFormat feedFormat    = SyndicationFormat.Atom;
        /// <summary>
        /// Private member to hold a collection of authors associated to the feed.
        /// </summary>
        private AtomPersonCollection feedAuthors;
        /// <summary>
        /// Private member to hold a collection of categories associated to the feed.
        /// </summary>
        private AtomCategoryCollection feedCategories;
        /// <summary>
        /// Private member to hold a collection of contributors associated to the feed.
        /// </summary>
        private AtomPersonCollection feedContributors;
        /// <summary>
        /// Private member to hold the agent used to generate a feed, for debugging and other purposes.
        /// </summary>
        private AtomGenerator feedGenerator;
        /// <summary>
        /// Private member to hold an URI that identifies an image that provides iconic visual identification for a feed.
        /// </summary>
        private Uri feedIcon;
        /// <summary>
        /// Private member to hold a permanent, universally unique identifier for the feed.
        /// </summary>
        private Uri feedId;
        /// <summary>
        /// Private member to hold a collection of links associated to the feed.
        /// </summary>
        private AtomLinkCollection feedLinks;
        /// <summary>
        /// Private member to hold an URI that identifies an image that provides visual identification for a feed.
        /// </summary>
        private Uri feedLogo;
        /// <summary>
        /// Private member to hold information about rights held in and over the feed.
        /// </summary>
        private AtomText feedRights;
        /// <summary>
        /// Private member to hold a human-readable description or subtitle for a feed.
        /// </summary>
        private AtomText feedSubTitle;
        /// <summary>
        /// Private member to hold the human-readable title for a feed.
        /// </summary>
        private AtomText feedTitle;
        /// <summary>
        /// Private member to hold a date indicating the most recent instant in time when an feed was modified in a way the publisher considers significant.
        /// </summary>
        private W3CDateTime feedUpdatedDate;
        /// <summary>
        /// Private member to hold a collection of entries associated to the feed.
        /// </summary>
        private AtomEntryCollection feedEntries;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomFeed()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomFeed"/> class.
        /// </summary>
        public AtomFeed() : base()
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

        #region AtomFeed(SyndicationFeedSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomFeed"/> class using the supplied <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="settings">The set of features that this syndication feed supports.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomFeed(SyndicationFeedSettings settings) : base(settings)
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

        #region AtomFeed(Uri id, AtomText title, W3CDateTime updatedOn)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomFeed"/> class using the specified identifier, title and updated date.
        /// </summary>
        /// <param name="id">A <see cref="Uri"/> that conveys a permanent, universally unique identifier for this feed.</param>
        /// <param name="title">The human-readable title for this feed.</param>
        /// <param name="updatedOn">A date indicating the most recent instant in time when this feed was modified in a way the publisher considers significant.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="id"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="title"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="updatedOn"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomFeed(Uri id, AtomText title, W3CDateTime updatedOn)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Id         = id;
                this.Title      = title;
                this.UpdatedOn  = updatedOn;
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
        #region Format
        /// <summary>
        /// Gets the syndication format that this feed implements.
        /// </summary>
        /// <value>The <see cref="SyndicationFormat"/> enumeration value that indicates the type of syndication format that this feed implements.</value>
        public override SyndicationFormat Format
        {
            get
            {
                return feedFormat;
            }
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets or sets the version of the syndication format that this feed conforms to.
        /// </summary>
        /// <value>The version of the syndication format that this feed conforms to.</value>
        /// <remarks>The default value is '1.0'.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        public override string Version
        {
            get
            {
                return feedVersion;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else if (String.IsNullOrEmpty(value.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "value");
                }
                else
                {
                    feedVersion = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC SPECIFICATION PROPERTIES
        //============================================================
        #region Authors
        /// <summary>
        /// Gets or sets the collection of authors associated to this feed.
        /// </summary>
        /// <value>A collection of <see cref="AtomPerson"/> instances associated to this feed.</value>
        /// <remarks>An feed must contain one or more authors, unless all of the feed's child entries contain at least one author.</remarks>
        [XmlElement(ElementName = "author", Type = typeof(AtomPerson))]
        public AtomPersonCollection Authors
        {
            get
            {
                if (feedAuthors == null)
                {
                    feedAuthors = new AtomPersonCollection();
                }
                return feedAuthors;
            }
        }
        #endregion

        #region Categories
        /// <summary>
        /// Gets or sets the collection of categories associated to this feed.
        /// </summary>
        /// <value>A collection of <see cref="AtomCategory"/> instances associated to this feed.</value>
        [XmlElement(ElementName = "category", Type = typeof(AtomCategory))]
        public AtomCategoryCollection Categories
        {
            get
            {
                if (feedCategories == null)
                {
                    feedCategories = new AtomCategoryCollection();
                }
                return feedCategories;
            }
        }
        #endregion

        #region Contributors
        /// <summary>
        /// Gets or sets the collection of contributors associated to this feed.
        /// </summary>
        /// <value>A collection of <see cref="AtomPerson"/> instances associated to this feed.</value>
        /// <remarks></remarks>
        [XmlElement(ElementName = "contributor", Type = typeof(AtomPerson))]
        public AtomPersonCollection Contributors
        {
            get
            {
                if (feedContributors == null)
                {
                    feedContributors = new AtomPersonCollection();
                }
                return feedContributors;
            }
        }
        #endregion

        #region Description
        /// <summary>
        /// Gets or sets the human-readable description or subtitle for this feed.
        /// </summary>
        /// <value>The human-readable description or subtitle for this feed.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "subtitle", Type = typeof(AtomText))]
        public AtomText Description
        {
            get
            {
                return feedSubTitle;
            }

            set
            {
                if (value == null)
                {
                    feedSubTitle = null;
                }
                else
                {
                    feedSubTitle = value;
                }
            }
        }
        #endregion

        #region Entries
        /// <summary>
        /// Gets or sets the collection of entries associated to this feed.
        /// </summary>
        /// <value>A collection of <see cref="AtomEntry"/> instances associated to this feed.</value>
        /// <remarks></remarks>
        [XmlElement(ElementName = "entry", Type = typeof(AtomEntry))]
        public AtomEntryCollection Entries
        {
            get
            {
                if (feedEntries == null)
                {
                    feedEntries = new AtomEntryCollection();
                }
                return feedEntries;
            }
        }
        #endregion

        #region Generator
        /// <summary>
        /// Gets or sets the agent used to generate this feed.
        /// </summary>
        /// <value>The agent used to generate this feed.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "generator", Type = typeof(AtomGenerator))]
        public AtomGenerator Generator
        {
            get
            {
                return feedGenerator;
            }

            set
            {
                if (value == null)
                {
                    feedGenerator = null;
                }
                else
                {
                    feedGenerator = value;
                }
            }
        }
        #endregion

        #region Icon
        /// <summary>
        /// Gets or sets a URI that identifies an image that provides iconic visual identification for this feed.
        /// </summary>
        /// <value>The <see cref="Uri"/> that identifies an image that provides iconic visual identification for this feed.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "icon", Type = typeof(System.Uri))]
        public Uri Icon
        {
            get
            {
                return feedIcon;
            }

            set
            {
                if (value == null)
                {
                    feedIcon = null;
                }
                else
                {
                    feedIcon = value;
                }
            }
        }
        #endregion

        #region Id
        /// <summary>
        /// Gets or sets a URI that conveys a permanent, universally unique identifier for this feed.
        /// </summary>
        /// <value>A <see cref="Uri"/> that conveys a permanent, universally unique identifier for this feed.</value>
        /// <remarks>This is a required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "id", Type = typeof(System.Uri))]
        public Uri Id
        {
            get
            {
                return feedId;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    feedId = value;
                }
            }
        }
        #endregion

        #region Links
        /// <summary>
        /// Gets or sets the collection of links associated to this feed.
        /// </summary>
        /// <value>A collection of <see cref="AtomLink"/> instances associated to this feed.</value>
        /// <remarks>
        ///     <para>An feed <b>should</b> contain one <see cref="AtomLink"/> with a <b>Relation</b> property value of <see cref="LinkRelation.Self"/>. This is the preferred <see cref="Uri"/> for retrieving Atom Feed Documents representing this Atom feed.</para>
        ///     <para>An feed <b>must not</b> contain more than one <see cref="AtomLink"/> with a <b>Relation</b> property value of <see cref="LinkRelation.Alternate"/> that has the same combination of <see cref="AtomLink.Type"/> and <see cref="AtomLink.Language"/> property values.</para>
        /// </remarks>
        [XmlElement(ElementName = "link", Type = typeof(AtomLink))]
        public AtomLinkCollection Links
        {
            get
            {
                if (feedLinks == null)
                {
                    feedLinks = new AtomLinkCollection();
                }
                return feedLinks;
            }
        }
        #endregion

        #region Logo
        /// <summary>
        /// Gets or sets a URI that identifies an image that provides visual identification for this feed.
        /// </summary>
        /// <value>The <see cref="Uri"/> that identifies an image that provides visual identification for this feed.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "logo", Type = typeof(System.Uri))]
        public Uri Logo
        {
            get
            {
                return feedLogo;
            }

            set
            {
                if (value == null)
                {
                    feedLogo = null;
                }
                else
                {
                    feedLogo = value;
                }
            }
        }
        #endregion

        #region Rights
        /// <summary>
        /// Gets or sets information about rights held in and over this feed.
        /// </summary>
        /// <value>The information about rights held in and over this feed.</value>
        /// <remarks>This is an optional property. This property should not be used to convey machine-readable licensing information.</remarks>
        [XmlElement(ElementName = "rights", Type = typeof(AtomText))]
        public AtomText Rights
        {
            get
            {
                return feedRights;
            }

            set
            {
                if (value == null)
                {
                    feedRights = null;
                }
                else
                {
                    feedRights = value;
                }
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets the human-readable title for this feed.
        /// </summary>
        /// <value>The human-readable title for this feed.</value>
        /// <remarks>This is a required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "title", Type = typeof(AtomText))]
        public AtomText Title
        {
            get
            {
                return feedTitle;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    feedTitle = value;
                }
            }
        }
        #endregion

        #region UpdatedOn
        /// <summary>
        /// Gets or sets a date indicating the most recent instant in time when this feed was modified in a way the publisher considers significant.
        /// </summary>
        /// <value>A date indicating the most recent instant in time when this feed was modified in a way the publisher considers significant.</value>
        /// <remarks>
        ///     This is a required property. All date-times in Atom conform to the Date and Time Specification of <a href="http://www.ietf.org/rfc/rfc3339.txt">RFC 3339</a>.
        /// </remarks>
        /// <example>2003-12-13T18:30:02+01:00</example>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "updated", Type = typeof(W3CDateTime))]
        public W3CDateTime UpdatedOn
        {
            get
            {
                return feedUpdatedDate;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    feedUpdatedDate = value;
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN SAVE ROUTINES
        //============================================================
        #region Save(XmlWriter writer)
        /// <summary>
        /// Saves the <see cref="AtomFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to save the syndication feed.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        public override void Save(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to save syndication feed
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
                //	Write start of XML document
                //------------------------------------------------------------
                writer.WriteStartDocument();
                
                //------------------------------------------------------------
                //	Create XML data adapter used to write syndication feed
                //------------------------------------------------------------
                AtomEngineSyndicationFeedAdapter adapter   = new AtomEngineSyndicationFeedAdapter();

                //------------------------------------------------------------
                //	Write syndication feed using adapter
                //------------------------------------------------------------
                adapter.Write(this, writer);
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
