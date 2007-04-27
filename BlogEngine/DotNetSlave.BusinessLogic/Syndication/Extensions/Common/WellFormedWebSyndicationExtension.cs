/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/24/2007	brian.kuhn		Created WellFormedWebSyndicationExtension Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace BlogEngine.Core.Syndication.Extensions.Common
{
    /// <summary>
    /// Extends a syndication feed to provide a way to expose the URL endpoint(s) for posting comments.
    /// </summary>
    /// <seealso cref="SyndicationExtension" />
    [Serializable()]
    public class WellFormedWebSyndicationExtension : SyndicationExtension
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a brief description of the extension.
        /// </summary>
        private string extensionDescription     = "Provides a way to expose the URL endpoint(s) for posting comments.";
        /// <summary>
        /// Private member to hold a uniform resource location that points to documentation about the extension.
        /// </summary>
        private Uri extensionDocumentation      = new Uri("http://wellformedweb.org/news/wfw_namespace_elements/");
        /// <summary>
        /// Private member to hold the unique XML namespace for the extension.
        /// </summary>
        private string extensionNamespace       = "http://wellformedweb.org/CommentAPI/";
        /// <summary>
        /// Private member to hold the XML namespace prefix for the extension.
        /// </summary>
        private string extensionNamespacePrefix = "wfw";
        /// <summary>
        /// Private member to hold the human readable name for the extension.
        /// </summary>
        private string extensionTitle           = "Well-Formed Web Comment API";
        /// <summary>
        /// Private member to hold collection of extension targets.
        /// </summary>
        Collection<ExtensionTarget> extensionTargets;
        /// <summary>
        /// Private member to hold the URI that comment entries for syndication feed item are to be POSTed to.
        /// </summary>
        private Uri wfwComment;
        /// <summary>
        /// Private member to hold the URI of the syndication feed for comments for feed item.
        /// </summary>
        private Uri wfwCommentRss;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region WellFormedWebSyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="WellFormedWebSyndicationExtension"/> class.
        /// </summary>
        public WellFormedWebSyndicationExtension()
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

        #region WellFormedWebSyndicationExtension(Uri server, Uri target)
        /// <summary>
        /// Initializes a new instance of the <see cref="WellFormedWebSyndicationExtension"/> class using the specified comment and comment feed endpoints.
        /// </summary>
        /// <param name="comment">The <see cref="Uri"/> that comment entries for a syndication feed item/entry are to be posted to.</param>
        /// <param name="commentFeed">The <see cref="Uri"/> of the syndication feed for comments for a syndication feed item/entry.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="comment"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="commentFeed"/> is a null reference (Nothing in Visual Basic).</exception>
        public WellFormedWebSyndicationExtension(Uri comment, Uri commentFeed)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (comment == null)
                {
                    throw new ArgumentNullException("comment");
                }
                if (commentFeed == null)
                {
                    throw new ArgumentNullException("commentFeed");
                }

                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Comment        = comment;
                this.CommentFeed    = commentFeed;
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
        #region Comment
        /// <summary>
        /// Gets or sets the URL that comment entries for a syndication feed item/entry are to be POSTed to.
        /// </summary>
        /// <value>The <see cref="Uri"/> that comment entries for a syndication feed item/entry are to be POSTed to.</value>
        public Uri Comment
        {
            get
            {
                return wfwComment;
            }

            set
            {
                if (value == null)
                {
                    wfwComment = null;
                }
                else
                {
                    wfwComment = value;
                }
            }
        }
        #endregion

        #region CommentFeed
        /// <summary>
        /// Gets or sets the URI of the syndication feed for comments for a syndication feed item/entry.
        /// </summary>
        /// <value>The <see cref="Uri"/> of the syndication feed for comments for a syndication feed item/entry.</value>
        public Uri CommentFeed
        {
            get
            {
                return wfwCommentRss;
            }

            set
            {
                if (value == null)
                {
                    wfwCommentRss = null;
                }
                else
                {
                    wfwCommentRss = value;
                }
            }
        }
        #endregion

        //============================================================
        //	COMMON EXTENSION ROUTINES
        //============================================================
        #region Inject(IXPathNavigable xmlDataTarget)
        /// <summary>
        /// Injects the XML data that represents this <see cref="WellFormedWebSyndicationExtension"/> into the specified XML data target.
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
                //	Initialize XPath navigator against XML target
                //------------------------------------------------------------
                navigator   = xmlDataTarget.CreateNavigator();

                //------------------------------------------------------------
                //	Append <wfw:comment> element
                //------------------------------------------------------------
                if (this.Comment != null)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "comment", this.Namespace, this.Comment.ToString());
                }

                //------------------------------------------------------------
                //	Append <wfw:commentRss> element
                //------------------------------------------------------------
                if (this.CommentFeed != null)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "commentRss", this.Namespace, this.CommentFeed.ToString());
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
        /// Injects the XML data that represents this <see cref="WellFormedWebSyndicationExtension"/> using the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> instance to write extension XML data to.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public override void Inject(XmlWriter writer)
        {
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
                //	Write <wfw:comment> element
                //------------------------------------------------------------
                if (this.Comment != null)
                {
                    writer.WriteElementString(this.NamespacePrefix, "comment", this.Namespace, this.Comment.ToString());
                }

                //------------------------------------------------------------
                //	Write <wfw:commentRss> element
                //------------------------------------------------------------
                if (this.CommentFeed != null)
                {
                    writer.WriteElementString(this.NamespacePrefix, "commentRss", this.Namespace, this.CommentFeed.ToString());
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
        /// Returns a <see cref="System.String"/> that represents the current <see cref="WellFormedWebSyndicationExtension"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="WellFormedWebSyndicationExtension"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            StringBuilder builder = new StringBuilder();

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                builder.AppendLine(String.Format(null, "<{0}:comment>{1}</{0}:comment>", this.NamespacePrefix, this.Comment != null ? this.Comment.ToString() : String.Empty));
                builder.AppendLine(String.Format(null, "<{0}:commentRss>{1}</{0}:commentRss>", this.NamespacePrefix, this.CommentFeed != null ? this.CommentFeed.ToString() : String.Empty));
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
