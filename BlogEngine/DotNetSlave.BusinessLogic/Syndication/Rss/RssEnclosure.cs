/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created RssEnclosure Class
****************************************************************************/
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Rss
{
    /// <summary>
    /// Represents a reference to an external media resource that can be associated to an <see cref="RssItem"/>.
    /// </summary>
    /// <remarks>For a use case scenario, see http://www.thetwowayweb.com/payloadsforrss for further information.</remarks>
    [Serializable()]
    public class RssEnclosure : SyndicationFeedEntityBase
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the length of the enclosure media in bytes.
        /// </summary>
        private long enclosureLength        = Int64.MinValue;
        /// <summary>
        /// Private member to hold the standard MIME type of the enclosure media.
        /// </summary>
        private string enclosureMimeType    = String.Empty;
        /// <summary>
        /// Private member to hold the URL location of the media the enclosure points to.
        /// </summary>
        private Uri enclosureUrl;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssEnclosure()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssEnclosure"/> class.
        /// </summary>
        public RssEnclosure() : base()
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

        #region RssEnclosure(Uri url, long length, string type)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssEnclosure"/> class using the specified URI, length, and MIME type.
        /// </summary>
        /// <param name="url">The URL location of the media the enclosure points to.</param>
        /// <param name="length">The length of the enclosure media in bytes.</param>
        /// <param name="type">The standard MIME type of the enclosure media.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="url"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="type"/> is a null reference (Nothing in Visual Basic.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="length"/> is less than zero.</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> is an empty string.</exception>
        public RssEnclosure(Uri url, long length, string type)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Length = length;
                this.Type   = type;
                this.Url    = url;
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
        #region Length
        /// <summary>
        /// Gets or sets the length of the enclosure media in bytes.
        /// </summary>
        /// <value>The length of the enclosure media in bytes.</value>
        /// <remarks>This is a required property.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is less than zero.</exception>
        [XmlAttribute(AttributeName = "length", Type = typeof(System.Int32))]
        public long Length
        {
            get
            {
                return enclosureLength;
            }

            set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                enclosureLength = value;
            }
        }
        #endregion

        #region Type
        /// <summary>
        /// Gets or sets the standard MIME type of the enclosure media.
        /// </summary>
        /// <value>The standard MIME type of the enclosure media.</value>
        /// <remarks>This is an required property. See http://www.iana.org/assignments/media-types/ for a listing of media types.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlAttribute(AttributeName = "type", Type = typeof(System.String))]
        public string Type
        {
            get
            {
                return enclosureMimeType;
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
                    enclosureMimeType = value.Trim();
                }
            }
        }
        #endregion

        #region Url
        /// <summary>
        /// Gets or sets the URL location of the media the enclosure points to.
        /// </summary>
        /// <value>The URL location of the media the enclosure points to.</value>
        /// <remarks>This is an required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlAttribute(AttributeName = "url", Type = typeof(System.Uri))]
        public Uri Url
        {
            get
            {
                return enclosureUrl;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    enclosureUrl = value;
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="RssEnclosure"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="RssEnclosure"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<enclosure length=\"{0}\" type=\"{1}\" url=\"{2}\" />";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                string url      = this.Url != null ? this.Url.ToString() : String.Empty;
                string length   = this.Length != Int64.MinValue ? this.Length.ToString(CultureInfo.InvariantCulture) : "0";
                representation  = String.Format(null, resultFormatter, length, this.Type, url);
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
