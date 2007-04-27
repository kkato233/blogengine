/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/24/2007	brian.kuhn		Created PingbackSyndicationExtension Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace BlogEngine.Core.Syndication.Extensions.Common
{
    /// <summary>
    /// Extends a syndication specification to provide a method for web authors to request notification when somebody links to one of their documents.
    /// The extension uses URLs in a such a way as to allow syndication entities to communicate the location of their Pingback server, as well as the value that should be passed as the target URI when pinging.
    /// </summary>
    /// <seealso cref="SyndicationExtension" />
    [Serializable()]
    public class PingbackSyndicationExtension : SyndicationExtension
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a brief description of the extension.
        /// </summary>
        private string extensionDescription         = "Provides a means for web authors to request notification when somebody links to one of their documents.";
        /// <summary>
        /// Private member to hold a uniform resource location that points to documentation about the extension.
        /// </summary>
        private Uri extensionDocumentation          = new Uri("http://madskills.com/public/xml/rss/module/pingback/");
        /// <summary>
        /// Private member to hold the unique XML namespace for the extension.
        /// </summary>
        private string extensionNamespace           = "http://madskills.com/public/xml/rss/module/pingback/";
        /// <summary>
        /// Private member to hold the XML namespace prefix for the extension.
        /// </summary>
        private string extensionNamespacePrefix     = "pingback";
        /// <summary>
        /// Private member to hold the human readable name for the extension.
        /// </summary>
        private string extensionTitle               = "Pingback Module For RSS";
        /// <summary>
        /// Private member to hold collection of extension targets.
        /// </summary>
        Collection<ExtensionTarget> extensionTargets;
        /// <summary>
        /// Private member to hold a value that contains the URL of an item's Pingback server.
        /// </summary>
        private Uri pingbackServer;
        /// <summary>
        /// Private member to hold a value that should be used as the target URI in a ping.
        /// </summary>
        private Uri pingbackTarget;
        /// <summary>
        /// Private member to hold a collection of target URLs that were pinged in reference to a syndication posting.
        /// </summary>
        private Collection<Uri> pingbackAbouts;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region PingbackSyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="PingbackSyndicationExtension"/> class.
        /// </summary>
        public PingbackSyndicationExtension()
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

        #region PingbackSyndicationExtension(Uri server, Uri target)
        /// <summary>
        /// Initializes a new instance of the <see cref="PingbackSyndicationExtension"/> class using the specified server and target.
        /// </summary>
        /// <param name="server">The <see cref="Uri"/> of the Pingback server.</param>
        /// <param name="target">The target <see cref="Uri"/> of a ping.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="server"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="target"/> is a null reference (Nothing in Visual Basic).</exception>
        public PingbackSyndicationExtension(Uri server, Uri target)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (server == null)
                {
                    throw new ArgumentNullException("server");
                }
                if (target == null)
                {
                    throw new ArgumentNullException("target");
                }

                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Server = server;
                this.Target = target;
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
        #region Abouts
        /// <summary>
        /// Gets a collection of target URLs that were pinged in reference to a syndication post.
        /// </summary>
        /// <value>A collection of target <see cref="Uri"/> end-points that were pinged in reference to a syndication post.</value>
        public Collection<Uri> Abouts
        {
            get
            {
                if(pingbackAbouts == null)
                {
                    pingbackAbouts = new Collection<Uri>();
                }

                return pingbackAbouts;
            }
        }
        #endregion

        #region Server
        /// <summary>
        /// Gets or sets a value that contains the URL of an item's Pingback server.
        /// </summary>
        /// <value>URL of an item's Pingback server.</value>
        public Uri Server
        {
            get
            {
                return pingbackServer;
            }

            set
            {
                if (value == null)
                {
                    pingbackServer = null;
                }
                else
                {
                    pingbackServer = value;
                }
            }
        }
        #endregion

        #region Target
        /// <summary>
        /// Gets or sets a value that should be used as the target URI in a ping.
        /// </summary>
        /// <value>The target <see cref="Uri"/> in a ping.</value>
        public Uri Target
        {
            get
            {
                return pingbackTarget;
            }

            set
            {
                if (value == null)
                {
                    pingbackTarget = null;
                }
                else
                {
                    pingbackTarget = value;
                }
            }
        }
        #endregion

        //============================================================
        //	COMMON EXTENSION ROUTINES
        //============================================================
        #region Inject(IXPathNavigable xmlDataTarget)
        /// <summary>
        /// Injects the XML data that represents this <see cref="PingbackSyndicationExtension"/> into the specified XML data target.
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
                navigator = xmlDataTarget.CreateNavigator();

                //------------------------------------------------------------
                //	Append <pingback:server> element
                //------------------------------------------------------------
                if (this.Server != null)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "server", this.Namespace, this.Server.ToString());
                }

                //------------------------------------------------------------
                //	Append <pingback:target> element
                //------------------------------------------------------------
                if (this.Target != null)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "target", this.Namespace, this.Target.ToString());
                }

                //------------------------------------------------------------
                //	Enumerate through <pingback:about> values
                //------------------------------------------------------------
                foreach (Uri about in this.Abouts)
                {
                    //------------------------------------------------------------
                    //	Append <pingback:about> element
                    //------------------------------------------------------------
                    navigator.AppendChildElement(this.NamespacePrefix, "about", this.Namespace, about.ToString());
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
        /// Injects the XML data that represents this <see cref="PingbackSyndicationExtension"/> using the specified <see cref="XmlWriter"/>.
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
                //	Write <pingback:server> element
                //------------------------------------------------------------
                if (this.Server != null)
                {
                    writer.WriteElementString(this.NamespacePrefix, "server", this.Namespace, this.Server.ToString());
                }

                //------------------------------------------------------------
                //	Write <pingback:target> element
                //------------------------------------------------------------
                if (this.Target != null)
                {
                    writer.WriteElementString(this.NamespacePrefix, "target", this.Namespace, this.Target.ToString());
                }

                //------------------------------------------------------------
                //	Enumerate through <pingback:about> values
                //------------------------------------------------------------
                foreach (Uri about in this.Abouts)
                {
                    //------------------------------------------------------------
                    //	Write <pingback:about> element
                    //------------------------------------------------------------
                    writer.WriteElementString(this.NamespacePrefix, "about", this.Namespace, about.ToString());
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
        /// Returns a <see cref="System.String"/> that represents the current <see cref="PingbackSyndicationExtension"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="PingbackSyndicationExtension"/>.</returns>
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
                builder.AppendLine(String.Format(null, "<{0}:server>{1}</{0}:server>", this.NamespacePrefix, this.Server != null ? this.Server.ToString() : String.Empty));
                builder.AppendLine(String.Format(null, "<{0}:target>{1}</{0}:target>", this.NamespacePrefix, this.Target != null ? this.Target.ToString() : String.Empty));

                if (this.Abouts.Count > 0)
                {
                    foreach (Uri about in this.Abouts)
                    {
                        builder.AppendLine(String.Format(null, "<{0}:about>{1}</{0}:about>", this.NamespacePrefix, about != null ? about.ToString() : String.Empty));
                    }
                }
                else
                {
                    return String.Format(null, "<{0}:about/>", this.NamespacePrefix);
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
            return builder.ToString();
        }
        #endregion
    }
}
