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

namespace BlogEngine.Core.Syndication
{
    /// <summary>
    /// Provides the set of methods and properties common to web content syndication feeds.
    /// </summary>
    public abstract class SyndicationFeed
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
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
    }
}
