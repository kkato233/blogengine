/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/24/2007	brian.kuhn		Created BlogChannelSyndicationExtension Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace BlogEngine.Core.Syndication.Extensions.Common
{
    /// <summary>
    /// Extends a syndication specification to provide meta-data elements common to web logs (blogs).
    /// </summary>
    /// <seealso cref="SyndicationExtension" />
    [Serializable()]
    public class BlogChannelSyndicationExtension : SyndicationExtension
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a brief description of the extension.
        /// </summary>
        private string extensionDescription     = "Describes information common to web logs (blogs).";
        /// <summary>
        /// Private member to hold a uniform resource location that points to documentation about the extension.
        /// </summary>
        private Uri extensionDocumentation      = new Uri("http://backend.userland.com/blogChannelModule");
        /// <summary>
        /// Private member to hold the unique XML namespace for the extension.
        /// </summary>
        private string extensionNamespace       = "http://backend.userland.com/blogChannelModule";
        /// <summary>
        /// Private member to hold the XML namespace prefix for the extension.
        /// </summary>
        private string extensionNamespacePrefix = "blogChannel";
        /// <summary>
        /// Private member to hold the human readable name for the extension.
        /// </summary>
        private string extensionTitle           = "blogChannel RSS Module";
        /// <summary>
        /// Private member to hold collection of extension targets.
        /// </summary>
        Collection<ExtensionTarget> extensionTargets;
        /// <summary>
        /// Private member to hold the URI of an OPML file containing the blogroll for the web log.
        /// </summary>
        private Uri blogChannelBlogRoll;
        /// <summary>
        /// Private member to hold the URI of an OPML file containing the author's syndication subscriptions.
        /// </summary>
        private Uri blogChannelSubscriptions;
        /// <summary>
        /// Private member to hold the URI of a web log that the author of the web log is promoting.
        /// </summary>
        private Uri blogChannelBlink;
        /// <summary>
        /// Private member to hold the URI of an XML file (changes.xml) that keeps track of changes in the syndication feed.
        /// </summary>
        private Uri blogChannelChanges;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region BlogChannelSyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogChannelSyndicationExtension"/> class.
        /// </summary>
        public BlogChannelSyndicationExtension()
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

        #region BlogChannelSyndicationExtension(Uri blogRoll, Uri subscriptions)
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogChannelSyndicationExtension"/> class using the specified blogroll and subscriptions.
        /// </summary>
        /// <param name="blogRoll">The <see cref="Uri"/> of an OPML file containing the blogroll for the web log.</param>
        /// <param name="subscriptions">The <see cref="Uri"/> of an OPML file containing the author's syndication subscriptions.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="blogRoll"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="subscriptions"/> is a null reference (Nothing in Visual Basic).</exception>
        public BlogChannelSyndicationExtension(Uri blogRoll, Uri subscriptions)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (blogRoll == null)
                {
                    throw new ArgumentNullException("blogRoll");
                }
                if (subscriptions == null)
                {
                    throw new ArgumentNullException("subscriptions");
                }

                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.BlogRoll       = blogRoll;
                this.Subscriptions  = subscriptions;
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
                    extensionTargets    = new Collection<ExtensionTarget>();

                    //------------------------------------------------------------
                    //	Define targets for extension
                    //------------------------------------------------------------
                    extensionTargets.Add(ExtensionTarget.AtomFeed);
                    extensionTargets.Add(ExtensionTarget.RssChannel);

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
        #region BLink
        /// <summary>
        /// Gets or sets the URI of a web log that the author of the web log is promoting.
        /// </summary>
        /// <value>The <see cref="Uri"/> of a web log that the author of the web log is promoting.</value>
        /// <remarks>For more information concerning b-links, see http://diveintomark.org/archives/2002/09/17/blink_and_youll_miss_it.</remarks>
        public Uri BLink
        {
            get
            {
                return blogChannelBlink;
            }

            set
            {
                if (value == null)
                {
                    blogChannelBlink = null;
                }
                else
                {
                    blogChannelBlink = value;
                }
            }
        }
        #endregion

        #region BlogRoll
        /// <summary>
        /// Gets or sets the URI of an OPML file containing the blogroll for the web log.
        /// </summary>
        /// <value>The <see cref="Uri"/> of an OPML file containing the blogroll for the web log.</value>
        public Uri BlogRoll
        {
            get
            {
                return blogChannelBlogRoll;
            }

            set
            {
                if (value == null)
                {
                    blogChannelBlogRoll = null;
                }
                else
                {
                    blogChannelBlogRoll = value;
                }
            }
        }
        #endregion

        #region Changes
        /// <summary>
        /// Gets or sets the URI of an XML file that keeps track of changes in the syndication feed.
        /// </summary>
        /// <value>The <see cref="Uri"/> of an XML file that keeps track of changes in the syndication feed.</value>
        /// <remarks>The XML file is typically named changes.xml. Further details available at http://www.xmlrpc.com/weblogsComForRss.</remarks>
        public Uri Changes
        {
            get
            {
                return blogChannelChanges;
            }

            set
            {
                if (value == null)
                {
                    blogChannelChanges = null;
                }
                else
                {
                    blogChannelChanges = value;
                }
            }
        }
        #endregion

        #region Subscriptions
        /// <summary>
        /// Gets or sets the URI of an OPML file containing the author's syndication subscriptions.
        /// </summary>
        /// <value>The <see cref="Uri"/> of an OPML file containing the author's syndication subscriptions.</value>
        public Uri Subscriptions
        {
            get
            {
                return blogChannelSubscriptions;
            }

            set
            {
                if (value == null)
                {
                    blogChannelSubscriptions = null;
                }
                else
                {
                    blogChannelSubscriptions = value;
                }
            }
        }
        #endregion

        //============================================================
        //	COMMON EXTENSION ROUTINES
        //============================================================
        #region Inject(IXPathNavigable xmlDataTarget)
        /// <summary>
        /// Injects the XML data that represents this <see cref="BlogChannelSyndicationExtension"/> into the specified XML data target.
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
                //	Append <blogChannel:blink> element
                //------------------------------------------------------------
                if (this.BLink != null)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "blink", this.Namespace, this.BLink.ToString());
                }

                //------------------------------------------------------------
                //	Append <blogChannel:blogRoll> element
                //------------------------------------------------------------
                if (this.BlogRoll != null)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "blogRoll", this.Namespace, this.BlogRoll.ToString());
                }

                //------------------------------------------------------------
                //	Append <blogChannel:changes> element
                //------------------------------------------------------------
                if (this.Changes != null)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "changes", this.Namespace, this.Changes.ToString());
                }

                //------------------------------------------------------------
                //	Append <blogChannel:mySubscriptions> element
                //------------------------------------------------------------
                if (this.Subscriptions != null)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "mySubscriptions", this.Namespace, this.Subscriptions.ToString());
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
        /// Injects the XML data that represents this <see cref="BlogChannelSyndicationExtension"/> using the specified <see cref="XmlWriter"/>.
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
                //	Write <blogChannel:blink> element
                //------------------------------------------------------------
                if (this.BLink != null)
                {
                    writer.WriteElementString(this.NamespacePrefix, "blink", this.Namespace, this.BLink.ToString());
                }

                //------------------------------------------------------------
                //	Write <blogChannel:blogRoll> element
                //------------------------------------------------------------
                if (this.BlogRoll != null)
                {
                    writer.WriteElementString(this.NamespacePrefix, "blogRoll", this.Namespace, this.BlogRoll.ToString());
                }

                //------------------------------------------------------------
                //	Write <blogChannel:changes> element
                //------------------------------------------------------------
                if (this.Changes != null)
                {
                    writer.WriteElementString(this.NamespacePrefix, "changes", this.Namespace, this.Changes.ToString());
                }

                //------------------------------------------------------------
                //	Write <blogChannel:mySubscriptions> element
                //------------------------------------------------------------
                if (this.Subscriptions != null)
                {
                    writer.WriteElementString(this.NamespacePrefix, "mySubscriptions", this.Namespace, this.Subscriptions.ToString());
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
        /// Returns a <see cref="System.String"/> that represents the current <see cref="BlogChannelSyndicationExtension"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="BlogChannelSyndicationExtension"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            StringBuilder builder   = new StringBuilder();
            
            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                builder.AppendLine(String.Format(null, "<{0}:blink>{1}</{0}:blink>", this.NamespacePrefix, this.BLink != null ? this.BLink.ToString() : String.Empty));
                builder.AppendLine(String.Format(null, "<{0}:blogRoll>{1}</{0}:blogRoll>", this.NamespacePrefix, this.BlogRoll != null ? this.BlogRoll.ToString() : String.Empty));
                builder.AppendLine(String.Format(null, "<{0}:changes>{1}</{0}:changes>", this.NamespacePrefix, this.Changes != null ? this.Changes.ToString() : String.Empty));
                builder.AppendLine(String.Format(null, "<{0}:mySubscriptions>{1}</{0}:mySubscriptions>", this.NamespacePrefix, this.Subscriptions != null ? this.Subscriptions.ToString() : String.Empty));
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
