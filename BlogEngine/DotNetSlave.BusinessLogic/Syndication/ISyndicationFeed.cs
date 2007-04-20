/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/11/2007	brian.kuhn		Created ISyndicationFeed Interface
****************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Security;
using System.Xml;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication
{
    /// <summary>
    /// Allows an object to implement a syndication feed by representing a set of methods and properties common to web content syndication feeds.
    /// </summary>
    /// <remarks>The <b>ISyndicationFeed</b> interface allows an inheriting class to implement a syndication feed class, which represents the properties and methods common to all web content syndication feeds.</remarks>
    /// <seealso cref="SyndicationFeed"/>
    public interface ISyndicationFeed
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Format
        /// <summary>
        /// Gets the syndication format that the feed implements.
        /// </summary>
        /// <value>The <see cref="SyndicationFormat"/> enumeration value that indicates the type of syndication format that the feed implements.</value>
        SyndicationFormat Format
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
        SyndicationHeader Header
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
        SyndicationFeedSettings Settings
        {
            get;
            set;
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets or sets the version of the syndication format that the feed conforms to.
        /// </summary>
        /// <value>The version of the syndication format that the feed conforms to.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        string Version
        {
            get;
            set;
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region Load(Stream stream)
        /// <summary>
        /// Loads the syndication feed from the specified stream.
        /// </summary>
        /// <param name="stream">The stream containing the XML data of the syndication feed to load.</param>
        /// <remarks>This method call chain ends with a call to <b>ISyndicationFeed.Load(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        void Load(Stream stream);
        #endregion

        #region Load(string filename)
        /// <summary>
        /// Loads the syndication feed from the specified URL.
        /// </summary>
        /// <param name="filename">URL for the file containing the syndication feed to load.</param>
        /// <remarks>This method call chain ends with a call to <b>ISyndicationFeed.Load(XmlReader)</b>.</remarks>
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
        void Load(string filename);
        #endregion

        #region Load(TextReader reader)
        /// <summary>
        /// Loads the syndication feed from the specified <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">The <b>TextReader</b> used to feed the XML data into the syndication feed.</param>
        /// <remarks>This method call chain ends with a call to <b>ISyndicationFeed.Load(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        void Load(TextReader reader);
        #endregion

        #region Load(XmlReader reader)
        /// <summary>
        /// Loads the syndication feed from the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to feed the XML data into the syndication feed.</param>
        /// <remarks>
        ///     Place your custom code in the <b>Load</b> abstract method to load the syndication feed from the specified <see cref="XmlReader"/>.
        ///     <para>
        ///         The custom implementation should load the syndication feed using an adapter that implements the <see cref="DotNetSlave.BlogEngine.BusinessLogic.Syndication.Data.ISyndicationFeedAdapter"/> interface.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        void Load(XmlReader reader);
        #endregion

        #region Load(XmlReader reader, SyndicationFeedSettings settings)
        /// <summary>
        /// Loads the syndication feed from the specified <see cref="XmlReader"/> and <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to feed the XML data into the syndication feed.</param>
        /// <param name="settings">The <b>SyndicationFeedSettings</b> object used to configure the <see cref="ISyndicationFeed"/>.</param>
        /// <remarks>
        ///     Place your custom code in the <b>Load</b> abstract method to load the syndication feed from the specified <see cref="XmlReader"/>.
        ///     <para>
        ///         The custom implementation should load the syndication feed using an adapter that implements the <see cref="DotNetSlave.BlogEngine.BusinessLogic.Syndication.Data.ISyndicationFeedAdapter"/> interface.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> value is a null reference (Nothing in Visual Basic) -or- the <paramref name="settings"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        void Load(XmlReader reader, SyndicationFeedSettings settings);
        #endregion

        #region LoadXml(string xml)
        /// <summary>
        /// Loads the syndication feed from the specified string.
        /// </summary>
        /// <param name="xml">String that represents the XML data of the syndication feed to load.</param>
        /// <remarks>This method call chain ends with a call to <b>ISyndicationFeed.Load(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="xml"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="xml"/> is an empty string (""), contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the feed remains empty.</exception>
        void LoadXml(string xml);
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
        bool Refresh();
        #endregion

        //============================================================
        //	PUBLIC SAVE ROUTINES
        //============================================================
        #region Save(Stream stream)
        /// <summary>
        /// Saves the syndication feed to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which you want to save the syndication feed.</param>
        /// <remarks>This method call chain ends with a call to <b>ISyndicationFeed.Save(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        void Save(Stream stream);
        #endregion

        #region Save(string filename)
        /// <summary>
        /// Saves the syndication feed to the specified file.
        /// </summary>
        /// <param name="filename">The location of the file where you want to save the syndication feed.</param>
        /// <remarks>This method call chain ends with a call to <b>ISyndicationFeed.Save(XmlReader)</b>.</remarks>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="filename"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="filename"/> is an empty string (""), contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="NotSupportedException">The <paramref name="filename"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="IOException">An I/O error has occurred.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified <paramref name="filename"/>.</exception>
        void Save(string filename);
        #endregion

        #region Save(TextWriter writer)
        /// <summary>
        /// Saves the syndication feed to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>TextWriter</b> to which you want to save the syndication feed.</param>
        /// <remarks>This method call chain ends with a call to <b>ISyndicationFeed.Save(XmlReader)</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        void Save(TextWriter writer);
        #endregion

        #region Save(XmlWriter writer)
        /// <summary>
        /// Saves the syndication feed to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to save the syndication feed.</param>
        /// <remarks>
        ///     Place your custom code in the <b>Save</b> virtual method to save the syndication feed to the specified <see cref="XmlWriter"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        void Save(XmlWriter writer);
        #endregion
    }
}
