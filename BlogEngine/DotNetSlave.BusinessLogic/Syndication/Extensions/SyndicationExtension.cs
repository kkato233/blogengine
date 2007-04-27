/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/21/2007	brian.kuhn		Created SyndicationExtension Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.XPath;

namespace BlogEngine.Core.Syndication.Extensions
{
    /// <summary>
    /// Provides the set of methods and properties common to syndication feed extensions.
    /// </summary>
    public abstract class SyndicationExtension
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtension"/> class.
        /// </summary>
        protected SyndicationExtension()
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
        #region Description
        /// <summary>
        /// Gets a brief description of the extension.
        /// </summary>
        /// <value>A brief description of the syndication extension.</value>
        public abstract string Description
        {
            get;
        }
        #endregion

        #region Documentation
        /// <summary>
        /// Gets a uniform resource location that points to documentation about the extension.
        /// </summary>
        /// <value>A <see cref="Uri"/> that points to documentation or implementation details about the extension.</value>
        public abstract Uri Documentation
        {
            get;
        }
        #endregion

        #region Namespace
        /// <summary>
        /// Gets the unique XML namespace for the extension.
        /// </summary>
        /// <value>The unique XML namespace for the extension.</value>
        public abstract string Namespace
        {
            get;
        }
        #endregion

        #region NamespacePrefix
        /// <summary>
        /// Gets the XML namespace prefix for the extension.
        /// </summary>
        /// <value>The XML namespace prefix for the extension.</value>
        public abstract string NamespacePrefix
        {
            get;
        }
        #endregion

        #region Targets
        /// <summary>
        /// Gets the collection of <see cref="ExtensionTarget"/> enumeration values that describes the target elements that the extension can extend.
        /// </summary>
        public abstract Collection<ExtensionTarget> Targets
        {
            get;
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets the human readable name for the extension.
        /// </summary>
        /// <value>The human readable name for the extension used for display and debugging purposes.</value>
        public abstract string Title
        {
            get;
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region Inject(IXPathNavigable xmlDataTarget)
        /// <summary>
        /// Injects the XML data that represents the <see cref="SyndicationExtension"/> into the specified XML data target.
        /// </summary>
        /// <param name="xmlDataTarget">The <see cref="IXPathNavigable"/> instance to inject extension XML data into.</param>
        /// <remarks>Place your custom code in the <b>Inject</b> virtual method to add the XML data that represent your custom extension to the provided XML data target.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlDataTarget"/> is a null reference (Nothing in Visual Basic).</exception>
        public abstract void Inject(IXPathNavigable xmlDataTarget);
        #endregion

        #region Inject(XmlWriter writer)
        /// <summary>
        /// Injects the XML data that represents the <see cref="SyndicationExtension"/> using the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> instance to write extension XML data to.</param>
        /// <remarks>Place your custom code in the <b>Inject</b> virtual method to write the XML data that represent your custom extension to the provided <b>XmlWriter</b>.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public abstract void Inject(XmlWriter writer);
        #endregion
    }
}
