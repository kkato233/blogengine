/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/21/2007	brian.kuhn		Created SyndicationExtension Class
****************************************************************************/
using System;
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

        #region QualifiedName
        /// <summary>
        /// Gets the XML qualified name for the extension.
        /// </summary>
        /// <value>The XML qualified name for the extension.</value>
        /// <remarks>This propery is generated dynamically using the extension's <b>Namespace</b> and <b>NamespacePrefix</b> properties.</remarks>
        public XmlQualifiedName QualifiedName
        {
            get
            {
                return new XmlQualifiedName(this.NamespacePrefix, this.Namespace);
            }
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
        /// Adds the XML data that represents a custom extension that implements <see cref="SyndicationExtension"/> to the specified XML data target.
        /// </summary>
        /// <param name="xmlDataTarget">The <see cref="IXPathNavigable"/> instance to inject extension XML data into.</param>
        /// <remarks>Place your custom extension code in the <b>Inject</b> virtual method to add the XML data that represent your custom extension to the provided XML data target.</remarks>
        public abstract void Inject(IXPathNavigable xmlDataTarget);
        #endregion
    }
}
