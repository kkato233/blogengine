/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created RssCloud Class
****************************************************************************/
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Rss
{
    /// <summary>
    /// Specifies a web service that supports the <a href="http://www.rssboard.org/rsscloud-interface">rssCloud</a> interface which can be implemented in HTTP-POST, XML-RPC or SOAP 1.1.
    /// </summary>
    /// <remarks>Purpose is to allow processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds. See http://www.rssboard.org/rsscloud-interface for more information about the rssCloud interface.</remarks>
    [Serializable()]
    public class RssCloud
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the domain name or IP address of the cloud.
        /// </summary>
        private string cloudDomain              = String.Empty;
        /// <summary>
        /// Private member to hold the TCP port that the cloud is running on.
        /// </summary>
        private int cloudPort                   = 80;
        /// <summary>
        /// Private member to hold the location of the cloud's responder.
        /// </summary>
        private string cloudPath                = String.Empty;
        /// <summary>
        /// Private member to hold 
        /// </summary>
        private string cloudRegisterProcedure   = String.Empty;
        /// <summary>
        /// Private member to hold a value indicating which communication protocol to use.
        /// </summary>
        private CloudProtocol  cloudProtocol    = CloudProtocol.XmlRpc;             
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssCloud()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssCloud"/> class.
        /// </summary>
        public RssCloud()
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

        #region RssCloud(string domain, string path, int port, CloudProtocol protocol, string procedure)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssCloud"/> class using the supplied domain, path, port, protocol and procedure.
        /// </summary>
        /// <param name="domain">The domain name or IP address of the cloud.</param>
        /// <param name="path">The location of the cloud's responder.</param>
        /// <param name="port">The TCP port that the cloud is running on.</param>
        /// <param name="protocol">The communication protocol the cloud uses.</param>
        /// <param name="procedure">The procedure to call to request notification.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="domain"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="path"/> is a null reference (Nothing in Visual Basic) -or- <paramref name="procedure"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="domain"/> is an empty string -or- <paramref name="path"/> is an empty string -or- <paramref name="procedure"/> is an empty string -or- <paramref name="protocol"/> is None.</exception>
        public RssCloud(string domain, string path, int port, CloudProtocol protocol, string procedure)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Domain             = domain;
                this.Path               = path;
                this.Port               = port;
                this.Protocol           = protocol;
                this.RegisterProcedure  = procedure;
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
        #region Domain
        /// <summary>
        /// Gets or sets the domain name or IP address of the cloud.
        /// </summary>
        /// <value>The domain name or IP address of the cloud.</value>
        /// <remarks>This is a required property.</remarks>
        /// <example>radio.xmlstoragesystem.com</example>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlAttribute(AttributeName = "domain", Type = typeof(System.String))]
        public string Domain
        {
            get
            {
                return cloudDomain;
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
                    cloudDomain = value.Trim();
                }
            }
        }
        #endregion

        #region Path
        /// <summary>
        /// Gets or sets the location of the cloud's responder.
        /// </summary>
        /// <value>The location of the cloud's responder.</value>
        /// <remarks>This is a required property.</remarks>
        /// <example>/RPC2</example>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlAttribute(AttributeName = "path", Type = typeof(System.String))]
        public string Path
        {
            get
            {
                return cloudPath;
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
                    cloudPath = value.Trim();
                }
            }
        }
        #endregion

        #region Port
        /// <summary>
        /// Gets or sets the TCP port that the cloud is running on.
        /// </summary>
        /// <value>The TCP port that the cloud is running on.</value>
        /// <remarks>This is a required property. Default value is 80.</remarks>
        /// <example>80</example>
        [XmlAttribute(AttributeName = "port", Type = typeof(System.Int32))]
        public int Port
        {
            get
            {
                return cloudPort;
            }

            set
            {
                cloudPort = value;
            }
        }
        #endregion

        #region Protocol
        /// <summary>
        /// Gets or sets a <see cref="CloudProtocol"/> enumeration value that indicates which communication protocol to use.
        /// </summary>
        /// <value>The <see cref="CloudProtocol"/> enumeration value that indicates the type of communication protocol that the cloud uses.</value>
        /// <remarks>The default value is CloudProtocol.XmlRpc, callers can use the <see cref="RssCloud.ProtocolFromString"/> method to convert a string representation to the corresponding enumeration value.</remarks>
        /// <example>xml-rpc</example>
        /// <exception cref="ArgumentException">The <see cref="CloudProtocol"/> value is <b>None</b>.</exception>
        [XmlAttribute(AttributeName = "protocol", Type = typeof(CloudProtocol))]
        public CloudProtocol Protocol
        {
            get
            {
                return cloudProtocol;
            }

            set
            {
                if (value == CloudProtocol.None)
                {
                    throw new ArgumentException(Resources.ExceptionRssCloudProtocolInvalid, "value");
                }
                else
                {
                    cloudProtocol   = value;
                }
            }
        }
        #endregion

        #region RegisterProcedure
        /// <summary>
        /// Gets or sets the name of the procedure to call to request notification.
        /// </summary>
        /// <value>The name of the procedure to call to request notification.</value>
        /// <remarks>This is a required property.</remarks>
        /// <example>xmlStorageSystem.rssPleaseNotify</example>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlAttribute(AttributeName = "registerProcedure", Type = typeof(System.String))]
        public string RegisterProcedure
        {
            get
            {
                return cloudRegisterProcedure;
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
                    cloudRegisterProcedure = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	STATIC ROUTINES
        //============================================================
        #region ProtocolFromString(string protocol)
        /// <summary>
        /// Returns the <see cref="CloudProtocol"/> enumeration value that corresponds to the specified string.
        /// </summary>
        /// <param name="protocol">The string representation of the cloud protocol.</param>
        /// <returns>A <see cref="CloudProtocol"/> enumeration value that corresponds to the specified string, otherwise returns <b>CloudProtocol.None</b>.</returns>
        /// <remarks>This method disregards case of specified protocol string.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="protocol"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="protocol"/> is an empty string.</exception>
        public static CloudProtocol ProtocolFromString(string protocol)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            CloudProtocol result    = CloudProtocol.None;

            //------------------------------------------------------------
            //	Attempt to return enumeration for string
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (protocol == null)
                {
                    throw new ArgumentNullException("protocol");
                }
                else if (String.IsNullOrEmpty(protocol.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "protocol");
                }

                //------------------------------------------------------------
                //	Determine corresponding enumeration value
                //------------------------------------------------------------
                if (String.Compare(protocol, "http-post", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = CloudProtocol.HttpPost;
                }
                else if (String.Compare(protocol, "soap", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = CloudProtocol.Soap;
                }
                else if (String.Compare(protocol, "xml-rpc", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = CloudProtocol.XmlRpc;
                }
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
            return result;
        }
        #endregion

        #region ProtocolToString(CloudProtocol protocol)
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the specified <see cref="CloudProtocol"/>.
        /// </summary>
        /// <param name="protocol">The <see cref="CloudProtocol"/> to convert to a string.</param>
        /// <returns>A <see cref="System.String"/> that represents the specified <see cref="CloudProtocol"/>. If enumeration value is None, returns <b>String.Empty</b>.</returns>
        /// <remarks>Returns <b>String.Empty</b> if enumeration value not recognized.</remarks>
        public static string ProtocolToString(CloudProtocol protocol)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string result   = String.Empty;

            //------------------------------------------------------------
            //	Attempt to return string representation for enumeration
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Determine corresponding string representation
                //------------------------------------------------------------
                switch (protocol)
                {
                    case CloudProtocol.HttpPost:
                        result  = "http-post";
                        break;
                    case CloudProtocol.None:
                        result  = String.Empty;
                        break;
                    case CloudProtocol.Soap:
                        result  = "soap";
                        break;
                    case CloudProtocol.XmlRpc:
                        result  = "xml-rpc";
                        break;
                }
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
            return result;
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="RssCloud"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="RssCloud"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<cloud domain=\"{0}\" port=\"{1}\" path=\"{2}\" registerProcedure=\"{3}\" protocol=\"{4}\" />";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                representation = String.Format(null, resultFormatter, this.Domain, this.Port, this.Path, this.RegisterProcedure, RssCloud.ProtocolToString(this.Protocol));
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
