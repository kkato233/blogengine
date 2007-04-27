/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/18/2007	brian.kuhn		Created AtomEntry Class
****************************************************************************/
using System;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Atom
{
    /// <summary>
    /// Represents an individual entry, acting as a container for metadata and data associated with the entry.
    /// </summary>
    /// <remarks>Can appear as a child of an <see cref="AtomFeed"/>, or can appear as the document (i.e., top-level) element of a stand-alone Atom Entry Document.</remarks>
    [Serializable()]
    public class AtomEntry : SyndicationFeedEntityBase
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a collection of authors associated to the entry.
        /// </summary>
        private AtomPersonCollection entryAuthors;
        /// <summary>
        /// Private member to hold a collection of categories associated to the entry.
        /// </summary>
        private AtomCategoryCollection entryCategories;
        /// <summary>
        /// Private member to hold content of the entry.
        /// </summary>
        private AtomContent entryContent;
        /// <summary>
        /// Private member to hold a collection of contributors associated to the entry.
        /// </summary>
        private AtomPersonCollection entryContributors;
        /// <summary>
        /// Private member to hold a permanent, universally unique identifier for the entry.
        /// </summary>
        private Uri entryId;
        /// <summary>
        /// Private member to hold a collection of links associated to the entry.
        /// </summary>
        private AtomLinkCollection entryLinks;
        /// <summary>
        /// Private member to hold a date indicating the initial creation or first availability of the resource.
        /// </summary>
        private W3CDateTime entryPublishedDate;
        /// <summary>
        /// Private member to hold information about rights held in and over the entry.
        /// </summary>
        private AtomText entryRights;
        /// <summary>
        /// Private member to hold the source feed an entry was copied from.
        /// </summary>
        private AtomFeed entrySource;
        /// <summary>
        /// Private member to hold a short summary, abstract, or excerpt of an entry.
        /// </summary>
        private AtomText entrySummary;
        /// <summary>
        /// Private member to hold the human-readable title for an entry.
        /// </summary>
        private AtomText entryTitle;
        /// <summary>
        /// Private member to hold a date indicating the most recent instant in time when an entry was modified in a way the publisher considers significant.
        /// </summary>
        private W3CDateTime entryUpdatedDate;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomEntry()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomEntry"/> class.
        /// </summary>
        public AtomEntry() : base()
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

        #region AtomEntry(Uri id, AtomText title, W3CDateTime updatedOn)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomEntry"/> class using the specified identifier, title and updated date.
        /// </summary>
        /// <param name="id">A <see cref="Uri"/> that conveys a permanent, universally unique identifier for this entry.</param>
        /// <param name="title">The human-readable title for this entry.</param>
        /// <param name="updatedOn">A date indicating the most recent instant in time when this entry was modified in a way the publisher considers significant.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="id"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="title"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="updatedOn"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomEntry(Uri id, AtomText title, W3CDateTime updatedOn)
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
        #region Authors
        /// <summary>
        /// Gets or sets the collection of authors associated to this entry.
        /// </summary>
        /// <value>A collection of <see cref="AtomPerson"/> instances associated to this entry.</value>
        /// <remarks>An entry must contain one or more authors, unless the entry contains a source that contains an author or the parent feed contains an author itself.</remarks>
        [XmlElement(ElementName = "author", Type = typeof(AtomPerson))]
        public AtomPersonCollection Authors
        {
            get
            {
                if (entryAuthors == null)
                {
                    entryAuthors = new AtomPersonCollection();
                }
                return entryAuthors;
            }
        }
        #endregion

        #region Categories
        /// <summary>
        /// Gets or sets the collection of categories associated to this entry.
        /// </summary>
        /// <value>A collection of <see cref="AtomCategory"/> instances associated to this entry.</value>
        [XmlElement(ElementName = "category", Type = typeof(AtomCategory))]
        public AtomCategoryCollection Categories
        {
            get
            {
                if (entryCategories == null)
                {
                    entryCategories = new AtomCategoryCollection();
                }
                return entryCategories;
            }
        }
        #endregion

        #region Content
        /// <summary>
        /// Gets or sets the content of the entry.
        /// </summary>
        /// <value>The content of the entry.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "content", Type = typeof(AtomContent))]
        public AtomContent Content
        {
            get
            {
                return entryContent;
            }

            set
            {
                if (value == null)
                {
                    entryContent = null;
                }
                else
                {
                    entryContent = value;
                }
            }
        }
        #endregion

        #region Contributors
        /// <summary>
        /// Gets or sets the collection of contributors associated to this entry.
        /// </summary>
        /// <value>A collection of <see cref="AtomPerson"/> instances associated to this entry.</value>
        /// <remarks></remarks>
        [XmlElement(ElementName = "contributor", Type = typeof(AtomPerson))]
        public AtomPersonCollection Contributors
        {
            get
            {
                if (entryContributors == null)
                {
                    entryContributors = new AtomPersonCollection();
                }
                return entryContributors;
            }
        }
        #endregion

        #region Id
        /// <summary>
        /// Gets or sets a URI that conveys a permanent, universally unique identifier for this entry.
        /// </summary>
        /// <value>A <see cref="Uri"/> that conveys a permanent, universally unique identifier for this entry.</value>
        /// <remarks>This is a required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "id", Type = typeof(System.Uri))]
        public Uri Id
        {
            get
            {
                return entryId;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    entryId = value;
                }
            }
        }
        #endregion

        #region Links
        /// <summary>
        /// Gets or sets the collection of links associated to this entry.
        /// </summary>
        /// <value>A collection of <see cref="AtomLink"/> instances associated to this entry.</value>
        /// <remarks>
        ///     <para>An entry that has no associated <see cref="AtomContent"/> <b>must</b> contain at least one <see cref="AtomLink"/> with a <b>Relation</b> property value of <see cref="LinkRelation.Alternate"/>.</para>
        ///     <para>An entry <b>must not</b> contain more than one <see cref="AtomLink"/> with a <b>Relation</b> property value of <see cref="LinkRelation.Alternate"/> that has the same combination of <see cref="AtomLink.Type"/> and <see cref="AtomLink.Language"/> property values.</para>
        /// </remarks>
        [XmlElement(ElementName = "link", Type = typeof(AtomLink))]
        public AtomLinkCollection Links
        {
            get
            {
                if (entryLinks == null)
                {
                    entryLinks = new AtomLinkCollection();
                }
                return entryLinks;
            }
        }
        #endregion

        #region PublishedOn
        /// <summary>
        /// Gets or sets a date indicating the initial creation or first availability of the resource.
        /// </summary>
        /// <value>A date indicating the initial creation or first availability of the resource.</value>
        /// <remarks>
        ///     This is an optional property. All date-times in Atom conform to the Date and Time Specification of <a href="http://www.ietf.org/rfc/rfc3339.txt">RFC 3339</a>.
        /// </remarks>
        /// <example>2003-12-13T18:30:02+01:00</example>
        [XmlElement(ElementName = "published", Type = typeof(W3CDateTime))]
        public W3CDateTime PublishedOn
        {
            get
            {
                return entryPublishedDate;
            }

            set
            {
                if (value == null)
                {
                    entryPublishedDate = null;
                }
                else
                {
                    entryPublishedDate = value;
                }
            }
        }
        #endregion

        #region Rights
        /// <summary>
        /// Gets or sets information about rights held in and over this entry.
        /// </summary>
        /// <value>The information about rights held in and over this entry.</value>
        /// <remarks>This is an optional property. This property should not be used to convey machine-readable licensing information.</remarks>
        [XmlElement(ElementName = "rights", Type = typeof(AtomText))]
        public AtomText Rights
        {
            get
            {
                return entryRights;
            }

            set
            {
                if (value == null)
                {
                    entryRights = null;
                }
                else
                {
                    entryRights = value;
                }
            }
        }
        #endregion

        #region Source
        /// <summary>
        /// Gets or sets the source feed this entry was copied from.
        /// </summary>
        /// <value>The source <see cref="AtomFeed"/> this entry was copied from.</value>
        /// <remarks>This is an optional property. The source property is designed to allow the aggregation of entries from different feeds while retaining information about an entry's source feed.</remarks>
        [XmlElement(ElementName = "source", Type = typeof(AtomFeed))]
        public AtomFeed Source
        {
            get
            {
                return entrySource;
            }

            set
            {
                if (value == null)
                {
                    entrySource = null;
                }
                else
                {
                    entrySource = value;
                }
            }
        }
        #endregion

        #region Summary
        /// <summary>
        /// Gets or sets a short summary, abstract, or excerpt for this entry.
        /// </summary>
        /// <value>A short summary, abstract, or excerpt for this entry.</value>
        /// <remarks>
        ///     This is an optional property. An entry <b>must</b> provide a summary if the entry has a content that has a source (and is thus empty), 
        ///     or the entry contains content that is encoded in Base64.
        /// </remarks>
        [XmlElement(ElementName = "summary", Type = typeof(AtomText))]
        public AtomText Summary
        {
            get
            {
                return entrySummary;
            }

            set
            {
                if (value == null)
                {
                    entrySummary = null;
                }
                else
                {
                    entrySummary = value;
                }
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets the human-readable title for this entry.
        /// </summary>
        /// <value>The human-readable title for this entry.</value>
        /// <remarks>This is a required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "title", Type = typeof(AtomText))]
        public AtomText Title
        {
            get
            {
                return entryTitle;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    entryTitle = value;
                }
            }
        }
        #endregion

        #region UpdatedOn
        /// <summary>
        /// Gets or sets a date indicating the most recent instant in time when this entry was modified in a way the publisher considers significant.
        /// </summary>
        /// <value>A date indicating the most recent instant in time when this entry was modified in a way the publisher considers significant.</value>
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
                return entryUpdatedDate;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    entryUpdatedDate = value;
                }
            }
        }
        #endregion
    }
}
