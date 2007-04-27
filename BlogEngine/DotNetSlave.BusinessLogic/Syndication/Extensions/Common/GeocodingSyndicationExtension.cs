/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/24/2007	brian.kuhn		Created GeocodingSyndicationExtension Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace BlogEngine.Core.Syndication.Extensions.Common
{
    /// <summary>
    /// Extends a syndication specification to provide a way of representing latitude/longitude and other information about spatially-located entities.
    /// </summary>
    /// <seealso cref="SyndicationExtension" />
    [Serializable()]
    public class GeocodingSyndicationExtension : SyndicationExtension
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a brief description of the extension.
        /// </summary>
        private string extensionDescription     = "Provides a way of representing latitude/longitude and other information about spatially-located entities.";
        /// <summary>
        /// Private member to hold a uniform resource location that points to documentation about the extension.
        /// </summary>
        private Uri extensionDocumentation      = new Uri("http://www.w3.org/2003/01/geo/");
        /// <summary>
        /// Private member to hold the unique XML namespace for the extension.
        /// </summary>
        private string extensionNamespace       = "http://www.w3.org/2003/01/geo/wgs84_pos#";
        /// <summary>
        /// Private member to hold the XML namespace prefix for the extension.
        /// </summary>
        private string extensionNamespacePrefix = "geo";
        /// <summary>
        /// Private member to hold the human readable name for the extension.
        /// </summary>
        private string extensionTitle           = "Semantic Web Basic Geocoding";
        /// <summary>
        /// Private member to hold collection of extension targets.
        /// </summary>
        Collection<ExtensionTarget> extensionTargets;
        /// <summary>
        /// Private member to hold value of latitude coordinate.
        /// </summary>
        private float geoLatitude   = Single.MinValue;
        /// <summary>
        /// Private member to hold value of longitude coordinate.
        /// </summary>
        private float geoLongitude = Single.MinValue;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region GeocodingSyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingSyndicationExtension"/> class.
        /// </summary>
        public GeocodingSyndicationExtension()
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

        #region GeocodingSyndicationExtension(float latitude, float longitude)
        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingSyndicationExtension"/> class using the specified latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude component of a geocoding position.</param>
        /// <param name="longitude">The longitude component of a geocoding position.</param>
        public GeocodingSyndicationExtension(float latitude, float longitude)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Latitude   = latitude;
                this.Longitude  = longitude;
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
        //	COMMON EXTENSION PROPERTIES
        //============================================================
        #region Description
        /// <summary>
        /// Gets a brief description of this extension.
        /// </summary>
        /// <value>A brief description of this syndication extension.</value>
        public override string Description
        {
            get
            {
                return extensionDescription;
            }
        }
        #endregion

        #region Documentation
        /// <summary>
        /// Gets a uniform resource location that points to documentation about this extension.
        /// </summary>
        /// <value>A <see cref="Uri"/> that points to documentation or implementation details about this extension.</value>
        public override Uri Documentation
        {
            get
            {
                return extensionDocumentation;
            }
        }
        #endregion

        #region Namespace
        /// <summary>
        /// Gets the unique XML namespace for this extension.
        /// </summary>
        /// <value>The unique XML namespace for this extension.</value>
        public override string Namespace
        {
            get
            {
                return extensionNamespace;
            }
        }
        #endregion

        #region NamespacePrefix
        /// <summary>
        /// Gets the XML namespace prefix for this extension.
        /// </summary>
        /// <value>The XML namespace prefix for this extension.</value>
        public override string NamespacePrefix
        {
            get
            {
                return extensionNamespacePrefix;
            }
        }
        #endregion

        #region Targets
        /// <summary>
        /// Gets the collection of <see cref="ExtensionTarget"/> enumeration values that describes the target elements that this extension can extend.
        /// </summary>
        public override Collection<ExtensionTarget> Targets
        {
            get
            {
                //------------------------------------------------------------
                //	Determine if targets have been defined
                //------------------------------------------------------------
                if (extensionTargets == null)
                {
                    //------------------------------------------------------------
                    //	Initialize collection
                    //------------------------------------------------------------
                    extensionTargets = new Collection<ExtensionTarget>();

                    //------------------------------------------------------------
                    //	Define targets for extension
                    //------------------------------------------------------------
                    extensionTargets.Add(ExtensionTarget.AtomEntry);
                    extensionTargets.Add(ExtensionTarget.AtomFeed);
                    extensionTargets.Add(ExtensionTarget.RssChannel);
                    extensionTargets.Add(ExtensionTarget.RssItem);

                    //------------------------------------------------------------
                    //	Return extension targets
                    //------------------------------------------------------------
                    return extensionTargets;
                }
                else
                {
                    //------------------------------------------------------------
                    //	Return extension targets
                    //------------------------------------------------------------
                    return extensionTargets;
                }
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets the human readable name for this extension.
        /// </summary>
        /// <value>The human readable name for this extension used for display and debugging purposes.</value>
        public override string Title
        {
            get
            {
                return extensionTitle;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Latitude
        /// <summary>
        /// Gets or sets the latitude component of a geocoding position.
        /// </summary>
        /// <value>The latitude value.</value>
        public float Latitude
        {
            get
            {
                return geoLatitude;
            }

            set
            {
                geoLatitude = value;
            }
        }
        #endregion

        #region Longitude
        /// <summary>
        /// Gets or sets the longitude component of a geocoding position.
        /// </summary>
        /// <value>The longitude value.</value>
        public float Longitude
        {
            get
            {
                return geoLongitude;
            }

            set
            {
                geoLongitude = value;
            }
        }
        #endregion

        //============================================================
        //	COMMON EXTENSION ROUTINES
        //============================================================
        #region Inject(IXPathNavigable xmlDataTarget)
        /// <summary>
        /// Injects the XML data that represents this <see cref="GeocodingSyndicationExtension"/> into the specified XML data target.
        /// </summary>
        /// <param name="xmlDataTarget">The <see cref="IXPathNavigable"/> instance to inject extension XML data into.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlDataTarget"/> is a null reference (Nothing in Visual Basic).</exception>
        public override void Inject(IXPathNavigable xmlDataTarget)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            XPathNavigator navigator;
            NumberFormatInfo formatInfo;

            //------------------------------------------------------------
            //	Attempt to inject XML data
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (xmlDataTarget == null)
                {
                    throw new ArgumentNullException("xmlDataTarget");
                }

                //------------------------------------------------------------
                //	Initialize numeric formater
                //------------------------------------------------------------
                formatInfo                      = new NumberFormatInfo();
                formatInfo.NumberDecimalDigits  = 6;

                //------------------------------------------------------------
                //	Initialize XPath navigator against XML target
                //------------------------------------------------------------
                navigator   = xmlDataTarget.CreateNavigator();

                //------------------------------------------------------------
                //	Append <geo:lat> element
                //------------------------------------------------------------
                if(this.Latitude != Single.MinValue)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "lat", this.Namespace, this.Latitude.ToString(formatInfo));
                }

                //------------------------------------------------------------
                //	Append <geo:long> element
                //------------------------------------------------------------
                if (this.Longitude != Single.MinValue)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "long", this.Namespace, this.Longitude.ToString(formatInfo));
                }
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

        #region Inject(XmlWriter writer)
        /// <summary>
        /// Injects the XML data that represents this <see cref="GeocodingSyndicationExtension"/> using the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> instance to write extension XML data to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public override void Inject(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            NumberFormatInfo formatInfo;

            //------------------------------------------------------------
            //	Attempt to write XML data
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Initialize numeric formater
                //------------------------------------------------------------
                formatInfo                      = new NumberFormatInfo();
                formatInfo.NumberDecimalDigits  = 6;

                //------------------------------------------------------------
                //	Write <geo:lat> element
                //------------------------------------------------------------
                if (this.Latitude != Single.MinValue)
                {
                    writer.WriteElementString(this.NamespacePrefix, "lat", this.Namespace, this.Latitude.ToString(formatInfo));
                }

                //------------------------------------------------------------
                //	Write <geo:long> element
                //------------------------------------------------------------
                if (this.Longitude != Single.MinValue)
                {
                    writer.WriteElementString(this.NamespacePrefix, "long", this.Namespace, this.Longitude.ToString(formatInfo));
                }
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
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GeocodingSyndicationExtension"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GeocodingSyndicationExtension"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            StringBuilder builder = new StringBuilder();
            NumberFormatInfo formatInfo;

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Initialize numeric formater
                //------------------------------------------------------------
                formatInfo                      = new NumberFormatInfo();
                formatInfo.NumberDecimalDigits  = 6;

                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                builder.AppendLine(String.Format(null, "<{0}:lat>{1}</{0}:lat>", this.NamespacePrefix, this.Latitude != Single.MinValue ? this.Latitude.ToString(formatInfo) : String.Empty));
                builder.AppendLine(String.Format(null, "<{0}:long>{1}</{0}:long>", this.NamespacePrefix, this.Longitude != Single.MinValue ? this.Longitude.ToString(formatInfo) : String.Empty));
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
            return builder.ToString();
        }
        #endregion
    }
}
