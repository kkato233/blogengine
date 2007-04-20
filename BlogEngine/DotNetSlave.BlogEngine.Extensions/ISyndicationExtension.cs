/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/11/2007	brian.kuhn		Created ISyndicationExtension Interface
****************************************************************************/
using System;
using System.Xml;
using System.Xml.XPath;

namespace DotNetSlave.BlogEngine.Extensions
{
    /// <summary>
    /// Defines the contract that syndication extensions must implement to extend a syndication format specification.
    /// </summary>
    public interface ISyndicationExtension
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Description
        /// <summary>
        /// Gets a brief description of the extension.
        /// </summary>
        /// <value>A brief description of the syndication extension.</value>
        string Description
        {
            get;
        }
        #endregion

        #region Documentation
        /// <summary>
        /// Gets a uniform resource location that points to documentation about the extension.
        /// </summary>
        /// <value>A <see cref="Uri"/> that points to documentation or implementation details about the extension.</value>
        Uri Documentation
        {
            get;
        }
        #endregion

        #region Namespace
        /// <summary>
        /// Gets the unique XML namespace for the extension.
        /// </summary>
        /// <value>The unique XML namespace for the extension.</value>
        string Namespace
        {
            get;
        }
        #endregion

        #region NamespacePrefix
        /// <summary>
        /// Gets the XML namespace prefix for the extension.
        /// </summary>
        /// <value>The XML namespace prefix for the extension.</value>
        string NamespacePrefix
        {
            get;
        }
        #endregion

        #region QualifiedName
        /// <summary>
        /// Gets the XML qualified name for the extension.
        /// </summary>
        /// <value>The XML qualified name for the extension.</value>
        /// <remarks>This propery should be generated dynamically using the extension's <b>Namespace</b> and <b>NamespacePrefix</b> properties.</remarks>
        XmlQualifiedName QualifiedName
        {
            get;
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets the human readable name for the extension.
        /// </summary>
        /// <value>The human readable name for the extension used for display purposes.</value>
        string Title
        {
            get;
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region Inject(IXPathNavigable xmlDataTarget)
        /// <summary>
        /// Adds the XML data that represents a custom extension that implements the <see cref="ISyndicationExtension"/> interface to the specified XML data target.
        /// </summary>
        /// <param name="xmlDataTarget">The <see cref="IXPathNavigable"/> instance to inject extension XML data into.</param>
        /// <remarks>Place your custom extension code in the <b>Inject</b> virtual method to add the XML data that represent your custom extension to the provided XML data target.</remarks>
        void Inject(IXPathNavigable xmlDataTarget);
        #endregion

        #region Parse(IXPathNavigable xmlDataSource)
        /// <summary>
        /// Returns a custom syndication extension that implements the <see cref="ISyndicationExtension"/> interface by parsing the <see cref="IXPathNavigable"/> XML data source.
        /// </summary>
        /// <param name="xmlDataSource">The <see cref="IXPathNavigable"/> to extract extension information from.</param>
        /// <returns>A custom syndication extension that implements the <see cref="ISyndicationExtension"/> interface if extension exists in provided XML data source, otherwise returns null (Nothing in Visual Basic).</returns>
        /// <remarks>Place your custom syndication extension code in the <b>Parse</b> virtual method to return an instance of your customsyndication  extension that is initialized using information extracted from the provided XML data source.</remarks>
        ISyndicationExtension Parse(IXPathNavigable xmlDataSource);
        #endregion
    }
}
