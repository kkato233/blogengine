/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/12/2007	brian.kuhn		Created RssFeed Class
****************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Security;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;
using DotNetSlave.BlogEngine.BusinessLogic.Syndication.Data;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Rss
{
    /// <summary>
    /// Represents a Really Simple Syndication (RSS) syndication feed.
    /// </summary>
    /// <remarks>See http://www.rssboard.org/rss-specification for further information about the RSS syndication format.</remarks>
    [Serializable()]
    [XmlRoot(ElementName = "rss", DataType = "feedType")]
    [XmlSchemaProviderAttribute("SchemaGet")]
    public class RssFeed : SyndicationFeed, IXmlSerializable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the version of the syndication format that this feed conforms to.
        /// </summary>
        private string feedVersion                  = "2.0";
        /// <summary>
        /// Private member to hold the syndication format that this feed implements.
        /// </summary>
        private SyndicationFormat feedFormat        = SyndicationFormat.Rss;
        /// <summary>
        /// Private member to hold channel for this feed.
        /// </summary>
        private RssChannel rssChannel               = new RssChannel();
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssFeed()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeed"/> class.
        /// </summary>
        public RssFeed() : base()
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

        #region RssFeed(SyndicationFeedSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeed"/> class using the supplied <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="settings">The set of features that this syndication feed supports.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssFeed(SyndicationFeedSettings settings) : base(settings)
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

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Format
        /// <summary>
        /// Gets the syndication format that this feed implements.
        /// </summary>
        /// <value>The <see cref="SyndicationFormat"/> enumeration value that indicates the type of syndication format that this feed implements.</value>
        [XmlIgnore()]
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
        /// <remarks>The default value is '2.0'.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlAttribute(AttributeName = "version", DataType = "string")]
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
        #region Channel
        /// <summary>
        /// Gets or sets the channel for this feed.
        /// </summary>
        /// <value>The channel for this feed.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "channel", Type = typeof(RssChannel))]
        public RssChannel Channel
        {
            get
            {
                return rssChannel;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    rssChannel = value;
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC STATIC ROUTINES
        //============================================================
        #region Create(Stream stream)
        /// <summary>
        /// Creates a new <see cref="RssFeed"/> instance using the specified stream.
        /// </summary>
        /// <param name="stream">The stream containing the XML data of the syndication feed to create.</param>
        /// <returns>An <b>RssFeed</b> object initialized using the data contained in the stream.</returns>
        /// <remarks>This method call chain ends with a call to <b>RssFeed.Create(XmlReader, SyndicationFeedSettings)</b> using the default <see cref="SyndicationFeedSettings"/>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="SecurityException">The <b>RssFeed</b> does not have sufficient permissions to access the location of the XML data.</exception>
        public static RssFeed Create(Stream stream)
        {
            //------------------------------------------------------------
            //	Attempt to create syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Return syndication feed using supplied stream
                //------------------------------------------------------------
                return RssFeed.Create(stream, null);
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
            }
            catch (SecurityException)
            {
                //------------------------------------------------------------
                //	Rethrow security exception
                //------------------------------------------------------------
                throw;
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

        #region Create(Stream stream, SyndicationFeedSettings settings)
        /// <summary>
        /// Creates a new <see cref="RssFeed"/> instance using the specified stream and <see cref="SyndicationFeedSettings"/> objects.
        /// </summary>
        /// <param name="stream">The stream containing the XML data of the syndication feed to create.</param>
        /// <param name="settings">The <see cref="SyndicationFeedSettings"/> object used to configure the new <see cref="RssFeed"/> instance. This value can be a null reference (Nothing in Visual Basic).</param>
        /// <returns>An <b>RssFeed</b> object initialized using the data contained in the stream.</returns>
        /// <remarks>This method call chain ends with a call to <b>RssFeed.Create(XmlReader, SyndicationFeedSettings)</b> using the specified <see cref="SyndicationFeedSettings"/>. If <paramref name="settings"/> is null then the default <b>SyndicationFeedSettings</b> are used.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="SecurityException">The <b>RssFeed</b> does not have sufficient permissions to access the location of the XML data.</exception>
        public static RssFeed Create(Stream stream, SyndicationFeedSettings settings)
        {
            //------------------------------------------------------------
            //	Attempt to create syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (stream == null)
                {
                    throw new ArgumentNullException("stream");
                }
                if (settings == null)
                {
                    settings = new SyndicationFeedSettings();
                }

                //------------------------------------------------------------
                //	Define XML reader settings
                //------------------------------------------------------------
                XmlReaderSettings readerSettings    = new XmlReaderSettings();
                readerSettings.IgnoreComments       = true;
                readerSettings.IgnoreWhitespace     = true;

                //------------------------------------------------------------
                //	Create XmlReader against stream
                //------------------------------------------------------------
                using(XmlReader reader = XmlReader.Create(stream, readerSettings))
                {
                    //------------------------------------------------------------
                    //	Return syndication feed using created reader and settings
                    //------------------------------------------------------------
                    return RssFeed.Create(reader, settings);
                }
            }
            catch (SecurityException)
            {
                //------------------------------------------------------------
                //	Rethrow security exception
                //------------------------------------------------------------
                throw;
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

        #region Create(TextReader reader)
        /// <summary>
        /// Creates a new <see cref="RssFeed"/> instance using the specified <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">The <b>TextReader</b>from which to read the XML data of the syndication feed to create.</param>
        /// <returns>An <b>RssFeed</b> object initialized using the data contained in the <see cref="TextReader"/>.</returns>
        /// <remarks>This method call chain ends with a call to <b>RssFeed.Create(XmlReader, SyndicationFeedSettings)</b> using the default <see cref="SyndicationFeedSettings"/>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic).</exception>
        public static RssFeed Create(TextReader reader)
        {
            //------------------------------------------------------------
            //	Attempt to create syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Return syndication feed using supplied text reader
                //------------------------------------------------------------
                return RssFeed.Create(reader, null);
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
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

        #region Create(TextReader reader, SyndicationFeedSettings settings)
        /// <summary>
        /// Creates a new <see cref="RssFeed"/> instance using the specified <see cref="TextReader"/> and <see cref="SyndicationFeedSettings"/> objects.
        /// </summary>
        /// <param name="reader">The <b>TextReader</b>from which to read the XML data of the syndication feed to create.</param>
        /// <param name="settings">The <see cref="SyndicationFeedSettings"/> object used to configure the new <see cref="RssFeed"/> instance. This value can be a null reference (Nothing in Visual Basic).</param>
        /// <returns>An <b>RssFeed</b> object initialized using the data contained in the <see cref="TextReader"/>.</returns>
        /// <remarks>This method call chain ends with a call to <b>RssFeed.Create(XmlReader, SyndicationFeedSettings)</b> using the specified <see cref="SyndicationFeedSettings"/>. If <paramref name="settings"/> is null then the default <b>SyndicationFeedSettings</b> are used.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic).</exception>
        public static RssFeed Create(TextReader reader, SyndicationFeedSettings settings)
        {
            //------------------------------------------------------------
            //	Attempt to create syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (reader == null)
                {
                    throw new ArgumentNullException("reader");
                }
                if (settings == null)
                {
                    settings = new SyndicationFeedSettings();
                }

                //------------------------------------------------------------
                //	Define XML reader settings
                //------------------------------------------------------------
                XmlReaderSettings readerSettings    = new XmlReaderSettings();
                readerSettings.IgnoreComments       = true;
                readerSettings.IgnoreWhitespace     = true;

                //------------------------------------------------------------
                //	Create XmlReader against text reader
                //------------------------------------------------------------
                using (XmlReader xmlReader = XmlReader.Create(reader, readerSettings))
                {
                    //------------------------------------------------------------
                    //	Return syndication feed using supplied text reader and settings
                    //------------------------------------------------------------
                    return RssFeed.Create(xmlReader, settings);
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

        #region Create(Uri uri)
        /// <summary>
        /// Creates a new <see cref="RssFeed"/> instance using the specified URI.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> for the file containing the XML data for the syndication feed to create.</param>
        /// <returns>An <b>RssFeed</b> object initialized using the data contained in the file.</returns>
        /// <remarks>
        ///     This method call chain ends with a call to <b>RssFeed.Create(XmlReader, SyndicationFeedSettings)</b> using the default <see cref="SyndicationFeedSettings"/>.
        ///     <para>
        ///         Calling this method will update the <see cref="SyndicationFeed.Header"/> property with the <paramref name="uri"/> response headers.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="uri"/> value is a null reference (Nothing in Visual Basic) -or- the resource at the specified <paramref name="uri"/> contains no data.</exception>
        /// <exception cref="SecurityException">The <b>RssFeed</b> does not have sufficient permissions to access the location of the XML data.</exception>
        /// <exception cref="WebException">An error occurred while retrieveing the XML data for the syndication feed.</exception>
        /// <exception cref="NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
        public static RssFeed Create(Uri uri)
        {
            //------------------------------------------------------------
            //	Attempt to create syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Return syndication feed using supplied URI
                //------------------------------------------------------------
                return RssFeed.Create(uri, null);
            }
            catch (ArgumentNullException)
            {
                //------------------------------------------------------------
                //	Rethrow argument null exception
                //------------------------------------------------------------
                throw;
            }
            catch (SecurityException)
            {
                //------------------------------------------------------------
                //	Rethrow security exception
                //------------------------------------------------------------
                throw;
            }
            catch (WebException)
            {
                //------------------------------------------------------------
                //	Rethrow web exception
                //------------------------------------------------------------
                throw;
            }
            catch (NotSupportedException)
            {
                //------------------------------------------------------------
                //	Rethrow not supported exception
                //------------------------------------------------------------
                throw;
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

        #region Create(Uri uri, SyndicationFeedSettings settings)
        /// <summary>
        /// Creates a new <see cref="RssFeed"/> instance using the specified URI and <see cref="SyndicationFeedSettings"/> objects.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> for the file containing the XML data for the syndication feed to create.</param>
        /// <param name="settings">The <see cref="SyndicationFeedSettings"/> object used to configure the new <see cref="RssFeed"/> instance. This value can be a null reference (Nothing in Visual Basic).</param>
        /// <returns>An <b>RssFeed</b> object initialized using the data contained in the file.</returns>
        /// <remarks>
        ///     <para>
        ///         This method call chain ends with a call to <b>RssFeed.Create(XmlReader, SyndicationFeedSettings)</b> using the specified <see cref="SyndicationFeedSettings"/>. 
        ///         If <paramref name="settings"/> is null then the default <b>SyndicationFeedSettings</b> are used.
        ///     </para>
        ///     <para>
        ///         Calling this method will update the <see cref="SyndicationFeed.Header"/> property with the <paramref name="uri"/> response headers.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="uri"/> value is a null reference (Nothing in Visual Basic) -or- the resource at the specified <paramref name="uri"/> contains no data.</exception>
        /// <exception cref="SecurityException">The <b>RssFeed</b> does not have sufficient permissions to access the location of the XML data.</exception>
        /// <exception cref="WebException">An error occurred while retrieveing the XML data for the syndication feed.</exception>
        /// <exception cref="NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
        public static RssFeed Create(Uri uri, SyndicationFeedSettings settings)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssFeed feed    = null;

            //------------------------------------------------------------
            //	Attempt to create syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (uri == null)
                {
                    throw new ArgumentNullException("uri");
                }
                if (settings == null)
                {
                    settings    = new SyndicationFeedSettings();
                }

                //------------------------------------------------------------
                //	Create web client to retrieve data
                //------------------------------------------------------------
                using(WebClient webClient = new WebClient())
                {
                    //------------------------------------------------------------
                    //	Determine if authorization credentials are available
                    //------------------------------------------------------------
                    if (settings.Credentials != null)
                    {
                        webClient.Credentials   = settings.Credentials;
                    }

                    //------------------------------------------------------------
                    //	Download data from specified resource
                    //------------------------------------------------------------
                    byte[] data = webClient.DownloadData(uri);

                    //------------------------------------------------------------
                    //	Validate data was returned
                    //------------------------------------------------------------
                    if (data == null)
                    {
                        throw new ArgumentNullException("uri");
                    }
                    if(data.Length <= 0)
                    {
                        throw new ArgumentNullException("uri");
                    }

                    //------------------------------------------------------------
                    //	Create stream against downloaded data
                    //------------------------------------------------------------
                    using (MemoryStream stream = new MemoryStream(data))
                    {
                        //------------------------------------------------------------
                        //	Set the position to the beginning of the stream
                        //------------------------------------------------------------
                        if (stream != null && stream.CanSeek)
                        {
                            stream.Seek(0, SeekOrigin.Begin);
                        }

                        //------------------------------------------------------------
                        //	Get the syndication feed using created stream and settings
                        //------------------------------------------------------------
                        feed        = RssFeed.Create(stream, settings);

                        //------------------------------------------------------------
                        //	Associate the response headers to the syndication feed
                        //------------------------------------------------------------
                        feed.UpdateHttpHeader(new SyndicationHeader(uri, webClient.ResponseHeaders));
                    }
                }
            }
            catch (SecurityException)
            {
                //------------------------------------------------------------
                //	Rethrow security exception
                //------------------------------------------------------------
                throw;
            }
            catch (WebException)
            {
                //------------------------------------------------------------
                //	Rethrow web exception
                //------------------------------------------------------------
                throw;
            }
            catch (NotSupportedException)
            {
                //------------------------------------------------------------
                //	Rethrow not supported exception
                //------------------------------------------------------------
                throw;
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Return the result
            //------------------------------------------------------------
            return feed;
        }
        #endregion

        #region Create(XmlReader reader, SyndicationFeedSettings settings)
        /// <summary>
        /// Creates a new <see cref="RssFeed"/> instance using the specified <see cref="XmlReader"/> and <see cref="SyndicationFeedSettings"/> objects.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b>from which to read the XML data of the syndication feed to create.</param>
        /// <param name="settings">The <see cref="SyndicationFeedSettings"/> object used to configure the new <see cref="RssFeed"/> instance.</param>
        /// <returns>An <b>RssFeed</b> object initialized using the data contained in the <see cref="XmlReader"/>.</returns>
        /// <remarks>
        ///     This method creates a <see cref="RssFeed"/> instance with the specified settings, and then makes a call to the <b>RssFeed.Load(XmlReader</b> method to fill the instance entities.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic) -or- <paramref name="settings"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="InvalidOperationException">The underlying <b>XmlReader</b> is in an <see cref="ReadState.Error"/> or <see cref="ReadState.Closed"/> state.</exception>
        public static RssFeed Create(XmlReader reader, SyndicationFeedSettings settings)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssFeed feed    = null;

            //------------------------------------------------------------
            //	Attempt to create syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (reader == null)
                {
                    throw new ArgumentNullException("reader");
                }
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                //------------------------------------------------------------
                //	Create new feed instance
                //------------------------------------------------------------
                feed    = new RssFeed(settings);

                //------------------------------------------------------------
                //	Load feed using reader
                //------------------------------------------------------------
                feed.Load(reader);
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
            return feed;
        }
        #endregion

        //============================================================
        //	OVERRIDDEN LOAD ROUTINES
        //============================================================
        #region Load(XmlReader reader)
        /// <summary>
        /// Loads the <see cref="RssFeed"/> from the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to feed the XML data into the syndication feed.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        public override void Load(XmlReader reader)
        {
            //------------------------------------------------------------
            //	Attempt to load syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (reader == null)
                {
                    throw new ArgumentNullException("reader");
                }

                //------------------------------------------------------------
                //	Create XML data adapter used to load syndication feed
                //------------------------------------------------------------
                RssXmlSyndicationFeedAdapter adapter = new RssXmlSyndicationFeedAdapter(reader, this.Settings);

                //------------------------------------------------------------
                //	Load syndication feed using adapter
                //------------------------------------------------------------
                adapter.Fill(this);
            }
            catch (XmlException)
            {
                //------------------------------------------------------------
                //	Rethrow XML exception
                //------------------------------------------------------------
                throw;
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

        #region Load(XmlReader reader, SyndicationFeedSettings settings)
        /// <summary>
        /// Loads the <see cref="RssFeed"/> from the specified <see cref="XmlReader"/> and <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to feed the XML data into the syndication feed.</param>
        /// <param name="settings">The <b>SyndicationFeedSettings</b> object used to configure the <see cref="RssFeed"/>.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic) -or- the <paramref name="settings"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        public override void Load(XmlReader reader, SyndicationFeedSettings settings)
        {
            //------------------------------------------------------------
            //	Attempt to load syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (reader == null)
                {
                    throw new ArgumentNullException("reader");
                }
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                //------------------------------------------------------------
                //	Create XML data adapter used to load syndication feed
                //------------------------------------------------------------
                RssXmlSyndicationFeedAdapter adapter = new RssXmlSyndicationFeedAdapter(reader, settings);

                //------------------------------------------------------------
                //	Load syndication feed using adapter
                //------------------------------------------------------------
                adapter.Fill(this);
            }
            catch (XmlException)
            {
                //------------------------------------------------------------
                //	Rethrow XML exception
                //------------------------------------------------------------
                throw;
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
        //	OVERRIDDEN SAVE ROUTINES
        //============================================================
        #region Save(XmlWriter writer)
        /// <summary>
        /// Saves the <see cref="RssFeed"/> to the specified <see cref="XmlWriter"/>.
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
                RssXmlSyndicationFeedAdapter adapter    = new RssXmlSyndicationFeedAdapter();

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

        //============================================================
        //	XML SERIALIZATION ROUTINES
        //============================================================
        #region GetSchema()
        /// <summary>
        /// This method is reserved, apply the <see cref="XmlSchemaProviderAttribute"/> to the class instead.
        /// </summary>
        /// <returns>An <see cref="XmlSchema"/> that describes the XML representation of the <see cref="RssFeed"/> that is produced by the <see cref="RssFeed.WriteXml"/> method and consumed by the <see cref="RssFeed.ReadXml"/> method.</returns>
        /// <remarks>
        ///     When serializing or deserializing an object, the <see cref="XmlSerializer"/> class does not do XML validation. For this purpose, it is safe to provide a trivial implementation of this method, for example by returning a null reference (Nothing in Visual Basic).
        ///     <para>
        ///         This method is called by the WebServiceUtil.exe utility when generating a proxy for your class to be consumed by a Web service client. For this purpose, it is essential that the method return an accurate XML schema that describes the XML representation of your object generated by the <see cref="RssFeed.WriteXml"/> method.
        ///     </para>
        /// </remarks>
        public XmlSchema GetSchema()
        {
            //------------------------------------------------------------
            //	Raise not implemented exception
            //------------------------------------------------------------
            throw new NotImplementedException();
        }
        #endregion

        #region ReadXml(XmlReader reader)
        /// <summary>
        /// Generates a <see cref="RssFeed"/> from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the syndication feed is deserialized.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML.</exception>
        public void ReadXml(XmlReader reader)
        {
            //------------------------------------------------------------
            //	Attempt to deserialize to XML
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (reader == null)
                {
                    throw new ArgumentNullException("reader");
                }

                //------------------------------------------------------------
                //	Load syndication feed using supplied XmlReader
                //------------------------------------------------------------
                this.Load(reader);
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

        #region SchemaGet(XmlSchemaSet schemaSet)
        /// <summary>
        /// Returns an XML schema and a <see cref="XmlSchemaComplexType"/> that controls the serialization of the <see cref="RssFeed"/>.
        /// </summary>
        /// <param name="schemaSet">A cache of XML Schema definition language (XSD) schemas to be populated.</param>
        /// <returns>A <see cref="XmlSchemaComplexType"/> instance that represents the namespace qualified local name.</returns>
        public static XmlSchemaComplexType SchemaGet(XmlSchemaSet schemaSet)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            XmlSchemaComplexType schemaType = null;
            XmlQualifiedName qualifiedName  = null;

            //------------------------------------------------------------
            //	Attempt to return schema information
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (schemaSet == null)
                {
                    throw new ArgumentNullException("schemaSet");
                }

                //------------------------------------------------------------
                //	Create reader against embedded schema resource
                //------------------------------------------------------------
                using (StringReader stringReader = new StringReader(Resources.RssSchema))
                {
                    //------------------------------------------------------------
                    //	Create XML reader against reader
                    //------------------------------------------------------------
                    using(XmlReader reader = XmlReader.Create(stringReader))
                    {
                        //------------------------------------------------------------
                        //	Create schema instance
                        //------------------------------------------------------------
                        XmlSerializer serializer    = new XmlSerializer(typeof(XmlSchema), RssXmlSyndicationFeedAdapter.DefaultXmlNamespace);
                        XmlSchema schema            = (XmlSchema)serializer.Deserialize(reader);

                        //------------------------------------------------------------
                        //	Add Atom schema to cache
                        //------------------------------------------------------------
                        schemaSet.XmlResolver       = new XmlUrlResolver();
                        schemaSet.Add(schema);

                        //------------------------------------------------------------
                        //	Create qualified name for Atom schema
                        //------------------------------------------------------------
                        qualifiedName               = new XmlQualifiedName("feedType", RssXmlSyndicationFeedAdapter.DefaultXmlNamespace);

                        //------------------------------------------------------------
                        //	Extcract complex type from Atom schema
                        //------------------------------------------------------------
                        schemaType                  = (XmlSchemaComplexType)schema.SchemaTypes[qualifiedName];
                    }
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
            return schemaType;
        }
        #endregion

        #region WriteXml(XmlWriter writer)
        /// <summary>
        /// Converts a <see cref="RssFeed"/> into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the syndication feed is serialized.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> value is a null reference (Nothing in Visual Basic).</exception>
        public void WriteXml(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to serialize as XML
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
                //	Create XML data adapter used to write syndication feed
                //------------------------------------------------------------
                RssXmlSyndicationFeedAdapter adapter    = new RssXmlSyndicationFeedAdapter();

                //------------------------------------------------------------
                //	Write syndication feed using adapter
                //------------------------------------------------------------
                adapter.Write(this, writer, XmlWriterType.Serialized);
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
