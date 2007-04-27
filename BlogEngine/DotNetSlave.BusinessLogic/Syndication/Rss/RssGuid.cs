/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/16/2007	brian.kuhn		Created RssGuid Class
****************************************************************************/
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Rss
{
    /// <summary>
    /// Represents a globally unique identifier (GUID) that can be associated to a <see cref="RssItem"/>.
    /// </summary>
    [Serializable()]
    public class RssGuid : SyndicationFeedEntityBase
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a string that uniquely identifies the item.
        /// </summary>
        private string guidValue;
        /// <summary>
        /// Private member to hold a value indicating if the guid points to the full content described by the item.
        /// </summary>
        private bool guidIsPermaLink    = true;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssGuid()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssGuid"/> class.
        /// </summary>
        public RssGuid() : base()
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

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region IsPermaLink
        /// <summary>
        /// Gets or sets a value indicating if the guid points to the full content described by the item.
        /// </summary>
        /// <value>Returns <b>true</b> if the <see cref="RssGuid"/> is a URL that points to the full item described by the <see cref="RssItem"/>, otherwise returns <b>false</b>.</value>
        /// <remarks>This is an optional property. Default value is <b>true</b>.</remarks>
        [XmlAttribute(AttributeName = "isPermaLink", Type = typeof(System.Boolean))]
        public bool IsPermaLink
        {
            get
            {
                return guidIsPermaLink;
            }

            set
            {
                guidIsPermaLink = value;
            }
        }
        #endregion

        #region PermaLink
        /// <summary>
        /// Gets the <see cref="Uri"/> to the resource that represents full content of the item.
        /// </summary>
        /// <value>The <see cref="Uri"/> to the resource that represents full content of the item.</value>
        /// <remarks>Returns null reference (Nothing in Visual Basic) is <see cref="RssGuid.IsPermaLink"/> is <b>false</b>.</remarks>
        /// <exception cref="ArgumentNullException">The guid value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="UriFormatException">The guid value is an empty string -or- is not a valid URI format.</exception>
        [XmlIgnore()]
        public Uri PermaLink
        {
            get
            {
                if(!this.IsPermaLink)
                {
                    return null;
                }
                else
                {
                    return new Uri(this.Value);
                }
            }
        }
        #endregion

        #region Value
        /// <summary>
        /// Gets or sets a string that uniquely identifies the item.
        /// </summary>
        /// <value>A <see cref="System.String"/> that uniquely identifies the item.</value>
        /// <remarks>
        ///     This is a required property. 
        ///     <para>
        ///         There are no rules for the syntax of a guid. Aggregators must view them as a string. It's up to the source of the feed to establish the uniqueness of the string. 
        ///         When present, an aggregator may choose to use this string to determine if an item is new.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlText(Type = (typeof(System.String)))]
        public string Value
        {
            get
            {
                return guidValue;
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
                    guidValue = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="RssGuid"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="RssGuid"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<guid isPermaLink=\"{0}\">{1}</guid>";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                representation      = String.Format(null, resultFormatter, this.IsPermaLink.ToString(CultureInfo.InvariantCulture).ToLowerInvariant(), this.Value);
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
            return representation;
        }
        #endregion
    }
}
