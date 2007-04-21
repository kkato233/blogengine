/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/11/2007	brian.kuhn		Created SyndicationFeed Class
****************************************************************************/
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security;
using System.Xml;

using BlogEngine.Core.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication
{
    /// <summary>
    /// Defines the base implementation of the <see cref="ISyndicationFeed"/> interface that provides the set of methods and properties common to web content syndication feeds.
    /// </summary>
    public abstract class SyndicationFeed : ISyndicationFeed
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold HTTP headers associated to the syndication feed.
        /// </summary>
        private SyndicationHeader feedHeader;
        /// <summary>
        /// Private member to hold the set of features that the syndication feed supports.
        /// </summary>
        private SyndicationFeedSettings feedSettings    = new SyndicationFeedSettings();
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SyndicationFeed()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationFeed"/> class.
        /// </summary>
        protected SyndicationFeed()
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

        #region SyndicationFeed(SyndicationFeedSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationFeed"/> class using the supplied <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="settings">The set of features that the syndication feed supports.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        protected SyndicationFeed(SyndicationFeedSettings settings)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                //------------------------------------------------------------
                //	Set class members
                //------------------------------------------------------------
                feedSettings    = settings;
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
        /// Gets the syndication format that the feed implements.
        /// </summary>
        /// <value>The <see cref="SyndicationFormat"/> enumeration value that indicates the type of syndication format that the feed implements.</value>
        public abstract SyndicationFormat Format
        {
            get;
        }
        #endregion

        #region Header
        /// <summary>
        /// Gets or sets the HTTP headers associated to the syndication feed.
        /// </summary>
        /// <value>The <see cref="SyndicationHeader"/> associated to the syndication feed.</value>
        /// <remarks>The header will be a null reference (Nothing in Visual Basic) if the syndication feed was not instantiated using a <see cref="Uri"/>.</remarks>
        public SyndicationHeader Header
        {
            get
            {
                return feedHeader;
            }
        }
        #endregion

        #region Settings
        /// <summary>
        /// Gets or sets the set of features that the syndication feed supports.
        /// </summary>
        /// <value>The set of features that the syndication feed supports.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationFeedSettings Settings
        {
            get
            {
                return feedSettings;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    feedSettings = value;
                }
            }
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets or sets the version of the syndication format that the feed conforms to.
        /// </summary>
        /// <value>The version of the syndication format that the feed conforms to.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        public abstract string Version
        {
            get;
            set;
        }
        #endregion

        //============================================================
        //	PUBLIC LOAD ROUTINES
        //============================================================
        #region Load(Stream stream)
        /// <summary>
        /// Loads the <see cref="SyndicationFeed"/> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream containing the XML data of the syndication feed to load.</param>
        /// <remarks>This method call chain ends with a call to <b>SyndicationFeed.Load(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        public void Load(Stream stream)
        {
            //------------------------------------------------------------
            //	Attempt to load syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (stream == null)
                {
                    throw new ArgumentNullException("stream");
                }

                //------------------------------------------------------------
                //	Define XML reader settings
                //------------------------------------------------------------
                XmlReaderSettings readerSettings    = new XmlReaderSettings();
                readerSettings.IgnoreComments       = true;
                readerSettings.IgnoreWhitespace     = true;

                //------------------------------------------------------------
                //	Create XML reader against supplied stream
                //------------------------------------------------------------
                using (XmlReader reader = XmlReader.Create(stream, readerSettings))
                {
                    //------------------------------------------------------------
                    //	Load syndication feed using created reader
                    //------------------------------------------------------------
                    this.Load(reader);
                }
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

        #region Load(string filename)
        /// <summary>
        /// Loads the <see cref="SyndicationFeed"/> from the specified URL.
        /// </summary>
        /// <param name="filename">URL for the file containing the syndication feed to load.</param>
        /// <remarks>This method call chain ends with a call to <b>SyndicationFeed.Load(XmlReader)</b>.</remarks>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="filename"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="filename"/> is an empty string (""), contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="NotSupportedException">The <paramref name="filename"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
        /// <exception cref="FileNotFoundException">The file specified by path does not exist. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="IOException">An I/O error has occurred.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified <paramref name="filename"/>.</exception>
        public void Load(string filename)
        {
            //------------------------------------------------------------
            //	Attempt to load syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if(filename == null)
                {
                    throw new ArgumentNullException("filename");
                }
                if (String.IsNullOrEmpty(filename.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "filename");
                }

                //------------------------------------------------------------
                //	Create file stream against specified file
                //------------------------------------------------------------
                using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    //------------------------------------------------------------
                    //	Load syndication feed using created stream
                    //------------------------------------------------------------
                    this.Load(stream);
                }
            }
            catch (NotSupportedException)
            {
                //------------------------------------------------------------
                //	Rethrow not supported exception
                //------------------------------------------------------------
                throw;
            }
            catch (FileNotFoundException)
            {
                //------------------------------------------------------------
                //	Rethrow file not found exception
                //------------------------------------------------------------
                throw;
            }
            catch (DirectoryNotFoundException)
            {
                //------------------------------------------------------------
                //	Rethrow directory not found exception
                //------------------------------------------------------------
                throw;
            }
            catch (PathTooLongException)
            {
                //------------------------------------------------------------
                //	Rethrow path too long exception
                //------------------------------------------------------------
                throw;
            }
            catch (IOException)
            {
                //------------------------------------------------------------
                //	Rethrow IO exception
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
            catch (UnauthorizedAccessException)
            {
                //------------------------------------------------------------
                //	Rethrow unauthorized access exception
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

        #region Load(TextReader reader)
        /// <summary>
        /// Loads the <see cref="SyndicationFeed"/> from the specified <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">The <b>TextReader</b> used to feed the XML data into the syndication feed.</param>
        /// <remarks>This method call chain ends with a call to <b>SyndicationFeed.Load(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        public void Load(TextReader reader)
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
                //	Define XML reader settings
                //------------------------------------------------------------
                XmlReaderSettings readerSettings    = new XmlReaderSettings();
                readerSettings.IgnoreComments       = true;
                readerSettings.IgnoreWhitespace     = true;

                //------------------------------------------------------------
                //	Create XML reader against text reader
                //------------------------------------------------------------
                using (XmlReader xmlReader = XmlReader.Create(reader, readerSettings))
                {
                    //------------------------------------------------------------
                    //	Load syndication feed using created reader
                    //------------------------------------------------------------
                    this.Load(xmlReader);
                }
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

        #region Load(XmlReader reader)
        /// <summary>
        /// Loads the <see cref="SyndicationFeed"/> from the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to feed the XML data into the syndication feed.</param>
        /// <remarks>
        ///     Place your custom code in the <b>Load</b> abstract method to load the <see cref="SyndicationFeed"/> from the specified <see cref="XmlReader"/>.
        ///     <para>
        ///         The custom implementation should load the syndication feed using an adapter that implements the <see cref="DotNetSlave.BlogEngine.BusinessLogic.Syndication.Data.ISyndicationFeedAdapter"/> interface.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        public abstract void Load(XmlReader reader);
        #endregion

        #region Load(XmlReader reader, SyndicationFeedSettings settings)
        /// <summary>
        /// Loads the syndication feed from the specified <see cref="XmlReader"/> and <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to feed the XML data into the syndication feed.</param>
        /// <param name="settings">The <b>SyndicationFeedSettings</b> object used to configure the <see cref="SyndicationFeed"/>.</param>
        /// <remarks>
        ///     Place your custom code in the <b>Load</b> abstract method to load the syndication feed from the specified <see cref="XmlReader"/>.
        ///     <para>
        ///         The custom implementation should load the syndication feed using an adapter that implements the <see cref="DotNetSlave.BlogEngine.BusinessLogic.Syndication.Data.ISyndicationFeedAdapter"/> interface.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic) -or- the <paramref name="settings"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        public abstract void Load(XmlReader reader, SyndicationFeedSettings settings);
        #endregion

        #region LoadXml(string xml)
        /// <summary>
        /// Loads the <see cref="SyndicationFeed"/> from the specified string.
        /// </summary>
        /// <param name="xml">String that represents the XML data of the syndication feed to load.</param>
        /// <remarks>This method call chain ends with a call to <b>SyndicationFeed.Load(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="xml"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="xml"/> is an empty string (""), contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        public void LoadXml(string xml)
        {
            //------------------------------------------------------------
            //	Attempt to load syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (xml == null)
                {
                    throw new ArgumentNullException("xml");
                }
                if (String.IsNullOrEmpty(xml.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "xml");
                }

                //------------------------------------------------------------
                //	Create string reader against supplied string
                //------------------------------------------------------------
                using (StringReader reader = new StringReader(xml))
                {
                    //------------------------------------------------------------
                    //	Define XML reader settings
                    //------------------------------------------------------------
                    XmlReaderSettings readerSettings    = new XmlReaderSettings();
                    readerSettings.IgnoreComments       = true;
                    readerSettings.IgnoreWhitespace     = true;

                    //------------------------------------------------------------
                    //	Create XML reader against string reader
                    //------------------------------------------------------------
                    using (XmlReader xmlReader = XmlReader.Create(reader, readerSettings))
                    {
                        //------------------------------------------------------------
                        //	Load syndication feed using created reader
                        //------------------------------------------------------------
                        this.Load(reader);
                    }
                }
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
        //	CONDITIONAL GET ROUTINES
        //============================================================
        #region Refresh()
        /// <summary>
        /// Reloads the syndication feed if it has been modified since it was last retrieved.
        /// </summary>
        /// <returns><b>true</b> if the syndication feed has been modified since it was last loaded, otherwise returns <b>false</b>.</returns>
        /// <remarks>The refresh of the syndication feed is optimized by performing the request using a conditional get of the syndication feed.</remarks>
        /// <exception cref="ArgumentException">The syndication feed header is a null reference (Nothing in Visual Basic) -or- the syndication feed header source is an empty string -or- the syndication feed header source contains no data.</exception>
        /// <exception cref="UriFormatException">The syndication feed header source is an invalid Uniform Resource Identifier (URI).</exception>
        /// <exception cref="NotSupportedException">The request scheme specified in syndication feed header source is not registered.</exception>
        /// <exception cref="SecurityException">The caller does not have permission to connect to the syndication feed header source.</exception>
        /// <exception cref="WebException">The time-out period expired when retrieving the header source -or- an error occurred while retrieving the header source.</exception>
        public bool Refresh()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            Uri feedSource              = null;

            //------------------------------------------------------------
            //	Attempt to reload syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Verify feed source is known
                //------------------------------------------------------------
                if (this.Header == null)
                {
                    throw new ArgumentException(Resources.ExceptionSyndicationFeedHeaderEmpty, "Header");
                }
                if (String.IsNullOrEmpty(this.Header.Source))
                {
                    throw new ArgumentException(Resources.ExceptionSyndicationFeedHeaderSourceEmpty, "Header.Source");
                }

                //------------------------------------------------------------
                //	Initialize local members
                //------------------------------------------------------------
                feedSource  = new Uri(this.Header.Source);

                //------------------------------------------------------------
                //	Create web request against feed source
                //------------------------------------------------------------
                HttpWebRequest webRequest   = (HttpWebRequest)WebRequest.Create(feedSource);

                //------------------------------------------------------------
                //	Initialize web request to enable conditional get
                //------------------------------------------------------------
                webRequest.IfModifiedSince  = Convert.ToDateTime(this.Header.LastModified, CultureInfo.InvariantCulture);
                webRequest.Headers.Add(HttpRequestHeader.IfNoneMatch, this.Header.ETag);

                //------------------------------------------------------------
                //	Attempt to retrieve syndication feed data
                //------------------------------------------------------------
                try
                {
                    //------------------------------------------------------------
                    //	Get response to conditional get request
                    //------------------------------------------------------------
                    using (HttpWebResponse conditionalGetResponse = (HttpWebResponse)webRequest.GetResponse())
                    {
                        //------------------------------------------------------------
                        //	Create WebClient to retrieve feed information
                        //------------------------------------------------------------
                        using (WebClient webClient = new WebClient())
                        {
                            //------------------------------------------------------------
                            //	Use congifured network credentials if available
                            //------------------------------------------------------------
                            if (this.Settings != null && this.Settings.Credentials != null)
                            {
                                webClient.Credentials   = this.Settings.Credentials;
                            }

                            //------------------------------------------------------------
                            //	Download data from specified resource
                            //------------------------------------------------------------
                            byte[] data = webClient.DownloadData(feedSource);

                            //------------------------------------------------------------
                            //	Associate the response headers to the syndication feed
                            //------------------------------------------------------------
                            this.UpdateHttpHeader(new SyndicationHeader(feedSource, webClient.ResponseHeaders));

                            //------------------------------------------------------------
                            //	Validate data was returned
                            //------------------------------------------------------------
                            if (data == null)
                            {
                                throw new ArgumentException(Resources.ExceptionSyndicationFeedHeaderSourceHasNoData, "Header.Source");
                            }
                            if (data.Length <= 0)
                            {
                                throw new ArgumentException(Resources.ExceptionSyndicationFeedHeaderSourceHasNoData, "Header.Source");
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
                                //	Load syndication feed using created stream
                                //------------------------------------------------------------
                                this.Load(stream);
                            }
                        }
                    }

                    //------------------------------------------------------------
                    //	Return value indicating feed has been refreshed
                    //------------------------------------------------------------
                    return true;
                }
                catch (WebException webException)
                {
                    //------------------------------------------------------------
                    //	Determine if response indicated resource was not modified
                    //------------------------------------------------------------
                    if (webException.Response != null && ((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.NotModified)
                    {
                        //------------------------------------------------------------
                        //	Return value indicating feed has not been refreshed
                        //------------------------------------------------------------
                        return false;
                    }
                    else
                    {
                        //------------------------------------------------------------
                        //	Rethrow exception
                        //------------------------------------------------------------
                        throw;
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
        }
        #endregion

        //============================================================
        //	PUBLIC SAVE ROUTINES
        //============================================================
        #region Save(Stream stream)
        /// <summary>
        /// Saves the <see cref="SyndicationFeed"/> to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which you want to save the syndication feed.</param>
        /// <remarks>This method call chain ends with a call to <b>SyndicationFeed.Save(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        public void Save(Stream stream)
        {
            //------------------------------------------------------------
            //	Attempt to save syndication feed
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

                //------------------------------------------------------------
                //	Define default XML writer settings
                //------------------------------------------------------------
                XmlWriterSettings writerSettings    = new XmlWriterSettings();
                writerSettings.Indent               = !this.Settings.MinimizeOutputSize;

                //------------------------------------------------------------
                //	Create XmlWriter against stream
                //------------------------------------------------------------
                using (XmlWriter writer = XmlWriter.Create(stream, writerSettings))
                {
                    //------------------------------------------------------------
                    //	Write syndication feed using created XML writer
                    //------------------------------------------------------------
                    this.Save(writer);
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

        #region Save(string filename)
        /// <summary>
        /// Saves the <see cref="SyndicationFeed"/> to the specified file.
        /// </summary>
        /// <param name="filename">The location of the file where you want to save the syndication feed.</param>
        /// <remarks>This method call chain ends with a call to <b>SyndicationFeed.Save(XmlReader)</b>.</remarks>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="filename"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="filename"/> is an empty string (""), contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="NotSupportedException">The <paramref name="filename"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="IOException">An I/O error has occurred.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified <paramref name="filename"/>.</exception>
        public void Save(string filename)
        {
            //------------------------------------------------------------
            //	Attempt to save syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (filename == null)
                {
                    throw new ArgumentNullException("filename");
                }
                if (String.IsNullOrEmpty(filename.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "filename");
                }

                //------------------------------------------------------------
                //	Create file stream against specified file
                //------------------------------------------------------------
                using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    //------------------------------------------------------------
                    //	Write syndication feed using created stream
                    //------------------------------------------------------------
                    this.Save(stream);
                }
            }
            catch (NotSupportedException)
            {
                //------------------------------------------------------------
                //	Rethrow not supported exception
                //------------------------------------------------------------
                throw;
            }
            catch (PathTooLongException)
            {
                //------------------------------------------------------------
                //	Rethrow path too long exception
                //------------------------------------------------------------
                throw;
            }
            catch (IOException)
            {
                //------------------------------------------------------------
                //	Rethrow IO exception
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
            catch (UnauthorizedAccessException)
            {
                //------------------------------------------------------------
                //	Rethrow unauthorized access exception
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

        #region Save(TextWriter writer)
        /// <summary>
        /// Saves the <see cref="SyndicationFeed"/> to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>TextWriter</b> to which you want to save the syndication feed.</param>
        /// <remarks>This method call chain ends with a call to <b>SyndicationFeed.Save(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        public void Save(TextWriter writer)
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
                //	Define default XML writer settings
                //------------------------------------------------------------
                XmlWriterSettings writerSettings    = new XmlWriterSettings();
                writerSettings.Indent               = !this.Settings.MinimizeOutputSize;

                //------------------------------------------------------------
                //	Create XmlWriter against text writer
                //------------------------------------------------------------
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, writerSettings))
                {
                    //------------------------------------------------------------
                    //	Write syndication feed using created XML writer
                    //------------------------------------------------------------
                    this.Save(xmlWriter);
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

        #region Save(XmlWriter writer)
        /// <summary>
        /// Saves the <see cref="SyndicationFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to save the syndication feed.</param>
        /// <remarks>
        ///     Place your custom code in the <b>Save</b> virtual method to save the <see cref="SyndicationFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        public abstract void Save(XmlWriter writer);
        #endregion

        //============================================================
        //	PROTECTED ROUTINES
        //============================================================
        #region UpdateHttpHeader(SyndicationHeader header)
        /// <summary>
        /// Sets the HTTP headers associated to the syndication feed.
        /// </summary>
        /// <param name="header">The HTTP headers associated to the syndication feed.</param>
        /// <remarks>This method is called by the SyndicationFeed.Create(Uri uri, SyndicationFeedSettings settings) method to apply HTTP header information after instantiating the syndication feed.</remarks>
        protected void UpdateHttpHeader(SyndicationHeader header)
        {
            //------------------------------------------------------------
            //	Attempt to set the associated HTTP headers
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (header == null)
                {
                    feedHeader  = null;
                }

                //------------------------------------------------------------
                //	Set associated HTTP header information
                //------------------------------------------------------------
                feedHeader = header;
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
