/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created RssImage Class
****************************************************************************/
using System;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Rss
{
    /// <summary>
    /// Represents a GIF, JPEG or PNG image that can be associated to a <see cref="RssChannel"/>.
    /// </summary>
    [Serializable()]
    public class RssImage : SyndicationFeedEntityBase
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the URL of a GIF, JPEG or PNG image that represents the channel.
        /// </summary>
        private Uri imageUrl;
        /// <summary>
        /// Private member to hold the description of the image. 
        /// </summary>
        private string imageTitle                   = String.Empty;
        /// <summary>
        /// Private member to hold the URL of a web site the image acts as a link for.
        /// </summary>
        private Uri imageLink;
        /// <summary>
        /// Private member to hold the width of the image in pixels.
        /// </summary>
        private int imageWidth                      = Int32.MinValue;
        /// <summary>
        /// Private member to hold the height of the image in pixels.
        /// </summary>
        private int imageHeight                     = Int32.MinValue;
        /// <summary>
        /// Private member to hold the text that is included in the TITLE attribute of the link formed around the image when rendered in HTML.
        /// </summary>
        private string imageDescription             = String.Empty;
        /// <summary>
        /// Private member to hold the maximum value for image height.
        /// </summary>
        private const int IMAGE_HEIGHT_MAX_VALUE    = 144;
        /// <summary>
        /// Private member to hold the maximum value for image width.
        /// </summary>
        private const int IMAGE_WIDTH_MAX_VALUE     = 400;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssImage()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssImage"/> class.
        /// </summary>
        public RssImage() : base()
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

        #region RssImage(Uri url, string title, Uri link)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssImage"/> class using the supplied url, title, and link.
        /// </summary>
        /// <param name="url">The URL of a GIF, JPEG or PNG image that represents the channel.</param>
        /// <param name="title">The short description of the image.</param>
        /// <param name="link">The URL of a web site the image acts as a link for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="url"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="title"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="link"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="title"/> is an empty string.</exception>
        public RssImage(Uri url, string title, Uri link)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Link   = link;
                this.Title  = title;
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
        #region Description
        /// <summary>
        /// Gets or sets the the textual description of the web site the image links to.
        /// </summary>
        /// <value>The text that is included in the TITLE attribute of the link formed around the image when rendered in HTML.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlElement(ElementName = "description", Type = typeof(System.String))]
        public string Description
        {
            get
            {
                return imageDescription;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    imageDescription = String.Empty;
                }
                else
                {
                    imageDescription = value.Trim();
                }
            }
        }
        #endregion

        #region Height
        /// <summary>
        /// Gets or sets the height of the image in pixels.
        /// </summary>
        /// <value>The height of the image in pixels.</value>
        /// <remarks>This is an optional property. The recommended default value is 31 pixels.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> exceeds the maximum permissible value for height.</exception>
        [XmlElement(ElementName = "height", Type = typeof(System.Int32))]
        public int Height
        {
            get
            {
                return imageHeight;
            }

            set
            {
                if (value > IMAGE_HEIGHT_MAX_VALUE)
                {
                    throw new ArgumentOutOfRangeException("value", String.Format(null, "The image height of {0} pixels exceeds the maximum value, which is {1} pixels.", value, IMAGE_HEIGHT_MAX_VALUE));
                }
                imageHeight = value;
            }
        }
        #endregion

        #region Link
        /// <summary>
        /// Gets or sets the URL of a web site the image acts as a link for.
        /// </summary>
        /// <value>The URL of a web site the image acts as a link for.</value>
        /// <remarks>This is an required property. In practice the image link value should have the same value as the channel's link value.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "link", Type = typeof(System.Uri))]
        public Uri Link
        {
            get
            {
                return imageLink;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    imageLink = value;
                }
            }
        }
        #endregion

        #region MaximumHeight
        /// <summary>
        /// Gets maximum permissible height of the image in pixels.
        /// </summary>
        /// <value>The maximum permissible height of the image in pixels.</value>
        /// <remarks></remarks>
        [XmlIgnore()]
        public static int MaximumHeight
        {
            get
            {
                return IMAGE_HEIGHT_MAX_VALUE;
            }
        }
        #endregion

        #region MaximumWidth
        /// <summary>
        /// Gets maximum permissible width of the image in pixels.
        /// </summary>
        /// <value>The maximum permissible width of the image in pixels.</value>
        /// <remarks></remarks>
        [XmlIgnore()]
        public static int MaximumWidth
        {
            get
            {
                return IMAGE_WIDTH_MAX_VALUE;
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets the short description of the image.
        /// </summary>
        /// <value>The description of the image.</value>
        /// <remarks>This is an required property. This is used in the ALT attribute of the HTML image tag when the channel is rendered in HTML. In practice the image title value should have the same value as the channel's title value.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlElement(ElementName = "title", Type = typeof(System.String))]
        public string Title
        {
            get
            {
                return imageTitle;
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
                    imageTitle = value.Trim();
                }
            }
        }
        #endregion

        #region Url
        /// <summary>
        /// Gets or sets the URL of a GIF, JPEG or PNG image that represents the channel.
        /// </summary>
        /// <value>The URL of a GIF, JPEG or PNG image that represents the channel.</value>
        /// <remarks>This is an required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "url", Type = typeof(System.Uri))]
        public Uri Url
        {
            get
            {
                return imageUrl;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    imageUrl = value;
                }
            }
        }
        #endregion

        #region Width
        /// <summary>
        /// Gets or sets the width of the image in pixels.
        /// </summary>
        /// <value>The width of the image in pixels.</value>
        /// <remarks>This is an optional property. The recommended default value is 88 pixels.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> exceeds the maximum permissible value for width.</exception>
        [XmlElement(ElementName = "width", Type = typeof(System.Int32))]
        public int Width
        {
            get
            {
                return imageWidth;
            }

            set
            {
                if (value > IMAGE_WIDTH_MAX_VALUE)
                {
                    throw new ArgumentOutOfRangeException("value", String.Format(null, "The image width of {0} pixels exceeds the maximum value, which is {1} pixels.", value, IMAGE_WIDTH_MAX_VALUE));
                }
                imageWidth = value;
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="RssImage"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="RssImage"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<a href=\"{0}\" title=\"{1}\" ><img src=\"{2}\" alt=\"{3}\" height=\"{4}\" width=\"{5}\" /></a>";
            
            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                string link     = this.Link != null ? this.Link.ToString() : String.Empty;
                string url      = this.Url != null ? this.Url.ToString() : String.Empty;

                string height   = this.Height != Int32.MinValue ? this.Height.ToString(CultureInfo.InvariantCulture) : String.Empty;
                string width    = this.Width != Int32.MinValue ? this.Width.ToString(CultureInfo.InvariantCulture) : String.Empty;

                representation  = String.Format(null, resultFormatter, link, this.Description, url, this.Title, height, width);
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
