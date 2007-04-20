/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created RssItem Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Rss
{
    /// <summary>
    /// Represents an associated item for a <see cref="RssChannel"/>.
    /// </summary>
    /// <remarks>
    ///     An item may represent a 'story', much like a story in a newspaper or magazine; if so its description is a synopsis of the story, and the link points to the full story. 
    ///     An item may also be complete in itself, if so, the description contains the text (entity-encoded HTML is allowed), and the item's link and title may be omitted.
    /// </remarks>
    [Serializable()]
    public class RssItem
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the title of item.
        /// </summary>
        private string itemTitle        = String.Empty;
        /// <summary>
        /// Private member to hold the URL to the full content of the item.
        /// </summary>
        private Uri itemLink;
        /// <summary>
        /// Private member to hold the synopsis of the item.
        /// </summary>
        private string itemDescription  = String.Empty;
        /// <summary>
        /// Private member to hold the email address and name of the author of the item.
        /// </summary>
        private string itemAuthor       = String.Empty;
        /// <summary>
        /// Private member to hold the url of the comments page for the item.
        /// </summary>
        private Uri itemComments;
        /// <summary>
        /// Private member to hold the publication date of the item.
        /// </summary>
        private Rfc822DateTime itemPublicationDate;
        /// <summary>
        /// Private member to hold collection of associated categories for item.
        /// </summary>
        private RssCategoryCollection itemCategories;
        /// <summary>
        /// Private member to hold an external media resource associated to the item.
        /// </summary>
        private RssEnclosure itemEnclosure;
        /// <summary>
        /// Private member to hold source channel that the item came from.
        /// </summary>
        private RssSource itemSource;
        /// <summary>
        /// Private member to hold globally unique identifier for the item.
        /// </summary>
        private RssGuid itemGuid;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssItem()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssItem"/> class.
        /// </summary>
        public RssItem()
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

        #region RssItem(string title, string description, Uri link)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssItem"/> class using the specified title, description and link.
        /// </summary>
        /// <param name="title">The title of item.</param>
        /// <param name="description">The synopsis of the item.</param>
        /// <param name="link">The URL to the full content of the item.</param>
        /// <remarks>Either <paramref name="title"/> or <paramref name="description"/> must be specified.</remarks>
        public RssItem(string title, string description, Uri link)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (String.IsNullOrEmpty(title) && String.IsNullOrEmpty(description))
                {
                    throw new ArgumentException(Resources.ExceptionRssItemDoesNotHaveADescriptor);
                }

                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Description    = description;
                this.Link           = link;
                this.Title          = title;
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
        #region Author
        /// <summary>
        /// Gets or sets the email address and name of the author of the item.
        /// </summary>
        /// <value>The email address and name of the author of the item.</value>
        /// <remarks>This is an optional property. For newspapers and magazines syndicating via RSS, the author is the person who wrote the article that the item describes. For collaborative weblogs, the author of the item might be different from the managing editor or webmaster.</remarks>
        /// <example>john.doe@domain.com (John Doe)</example>
        [XmlElement(ElementName = "author", Type = typeof(System.String))]
        public string Author
        {
            get
            {
                return itemAuthor;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    itemAuthor = String.Empty;
                }
                else
                {
                    itemAuthor = value.Trim();
                }
            }
        }
        #endregion

        #region Categories
        /// <summary>
        /// Gets or sets the collection of categories associated to this item.
        /// </summary>
        /// <value>A collection of <see cref="RssCategory"/> instances associated to this item.</value>
        [XmlElement(ElementName = "category", Type = typeof(RssCategory))]
        public RssCategoryCollection Categories
        {
            get
            {
                if (itemCategories == null)
                {
                    itemCategories = new RssCategoryCollection();
                }
                return itemCategories;
            }
        }
        #endregion

        #region Comments
        /// <summary>
        /// Gets or sets the URL of the comments page for the item.
        /// </summary>
        /// <value>The URL of the comments page for the item.</value>
        /// <remarks>This is an optional property. See http://backend.userland.com/weblogComments for more information.</remarks>
        [XmlElement(ElementName = "comments", Type = typeof(System.Uri))]
        public Uri Comments
        {
            get
            {
                return itemComments;
            }

            set
            {
                if (value == null)
                {
                    itemComments = null;
                }
                else
                {
                    itemComments = value;
                }
            }
        }
        #endregion

        #region Description
        /// <summary>
        /// Gets or sets the synopsis of the item.
        /// </summary>
        /// <value>The synopsis of the item.</value>
        /// <remarks>This is an required property if <see cref="RssItem.Title"/> has not been specified.</remarks>
        [XmlElement(ElementName = "description", Type = typeof(System.String))]
        public string Description
        {
            get
            {
                return itemDescription;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    itemDescription = String.Empty;
                }
                else
                {
                    itemDescription = value.Trim();
                }
            }
        }
        #endregion

        #region Enclosure
        /// <summary>
        /// Gets or sets an external media resource associated to the item.
        /// </summary>
        /// <value>A <see cref="RssEnclosure"/> data payload that is associated to the item.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "enclosure", Type = typeof(RssEnclosure))]
        public RssEnclosure Enclosure
        {
            get
            {
                return itemEnclosure;
            }

            set
            {
                if (value == null)
                {
                    itemEnclosure = null;
                }
                else
                {
                    itemEnclosure = value;
                }
            }
        }
        #endregion

        #region Guid
        /// <summary>
        /// Gets or sets the globally unique identifier for the item.
        /// </summary>
        /// <value>A <see cref="RssGuid"/> the represents the globally unique identity of the item.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "guid", Type = typeof(RssGuid))]
        public RssGuid Guid
        {
            get
            {
                return itemGuid;
            }

            set
            {
                if (value == null)
                {
                    itemGuid = null;
                }
                else
                {
                    itemGuid = value;
                }
            }
        }
        #endregion

        #region Link
        /// <summary>
        /// Gets or sets the URL to the full content of the item.
        /// </summary>
        /// <value>The URL to the full content of the item.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "link", Type = typeof(System.Uri))]
        public Uri Link
        {
            get
            {
                return itemLink;
            }

            set
            {
                if (value == null)
                {
                    itemLink = null;
                }
                else
                {
                    itemLink = value;
                }
            }
        }
        #endregion

        #region PublicationDate
        /// <summary>
        /// Gets or sets the publication date of the item.
        /// </summary>
        /// <value>The publication date of the item.</value>
        /// <remarks>
        ///     This is an optional property. If the date is in the future, aggregators may choose to not display the item until that date.
        ///     All date-times in RSS conform to the Date and Time Specification of <a href="http://asg.web.cmu.edu/rfc/rfc822.html">RFC 822</a>, with the exception that the year may be expressed with two characters or four characters (four preferred).
        /// </remarks>
        /// <example>Sat, 07 Sep 2002 00:00:01 GMT</example>
        [XmlElement(ElementName = "pubDate", Type = typeof(Rfc822DateTime))]
        public Rfc822DateTime PublicationDate
        {
            get
            {
                return itemPublicationDate;
            }

            set
            {
                if (value == null)
                {
                    itemPublicationDate = null;
                }
                else
                {
                    itemPublicationDate = value;
                }
            }
        }
        #endregion

        #region Source
        /// <summary>
        /// Gets or sets the source channel that the item came from.
        /// </summary>
        /// <value>A reference to the source <see cref="RssChannel"/> that the item came from.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "source", Type = typeof(RssSource))]
        public RssSource Source
        {
            get
            {
                return itemSource;
            }

            set
            {
                if (value == null)
                {
                    itemSource = null;
                }
                else
                {
                    itemSource = value;
                }
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets the title of item.
        /// </summary>
        /// <value>The title of item.</value>
        /// <remarks>This is an required property if <see cref="RssItem.Description"/> has not been specified.</remarks>
        [XmlElement(ElementName = "title", Type = typeof(System.String))]
        public string Title
        {
            get
            {
                return itemTitle;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    itemTitle = String.Empty;
                }
                else
                {
                    itemTitle = value.Trim();
                }
            }
        }
        #endregion
    }
}
