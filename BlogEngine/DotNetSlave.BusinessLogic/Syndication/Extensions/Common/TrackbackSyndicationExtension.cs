/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/24/2007	brian.kuhn		Created TrackbackSyndicationExtension Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace BlogEngine.Core.Syndication.Extensions.Common
{
    /// <summary>
    /// Extends a syndication specification to provide a peer-to-peer framework designed to allow communication between websites. 
    /// TrackBack enabled websites communicate via "pings", where each ping informs the receiving site that the sending site has made a reference to a post (or possibly a category) on the receiving site.
    /// </summary>
    /// <seealso cref="SyndicationExtension" />
    [Serializable()]
    public class TrackbackSyndicationExtension : SyndicationExtension
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a brief description of the extension.
        /// </summary>
        private string extensionDescription     = "Provides a peer-to-peer framework designed to allow communication between websites.";
        /// <summary>
        /// Private member to hold a uniform resource location that points to documentation about the extension.
        /// </summary>
        private Uri extensionDocumentation      = new Uri("http://madskills.com/public/xml/rss/module/trackback/");
        /// <summary>
        /// Private member to hold the unique XML namespace for the extension.
        /// </summary>
        private string extensionNamespace       = "http://madskills.com/public/xml/rss/module/trackback/";
        /// <summary>
        /// Private member to hold the XML namespace prefix for the extension.
        /// </summary>
        private string extensionNamespacePrefix = "trackback";
        /// <summary>
        /// Private member to hold the human readable name for the extension.
        /// </summary>
        private string extensionTitle           = "Trackback Module For RSS";
        /// <summary>
        /// Private member to hold collection of extension targets.
        /// </summary>
        Collection<ExtensionTarget> extensionTargets;
        /// <summary>
        /// Private member to hold a collection of target URLs that were pinged in reference to a syndication post.
        /// </summary>
        private Collection<Uri> trackbackAbouts;
        /// <summary>
        /// Private member to hold value that identifies the post's trackback URL.
        /// </summary>
        private Uri trackbackPing;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region TrackbackSyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackbackSyndicationExtension"/> class.
        /// </summary>
        public TrackbackSyndicationExtension()
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

        #region TrackbackSyndicationExtension(Uri ping)
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackbackSyndicationExtension"/> class using the specified ping <see cref="Uri"/>.
        /// </summary>
        /// <param name="ping">The post's trackback <see cref="Uri"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ping"/> is a null reference (Nothing in Visual Basic).</exception>
        public TrackbackSyndicationExtension(Uri ping)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (ping == null)
                {
                    throw new ArgumentNullException("ping");
                }

                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Ping   = ping;
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
        /// Gets a collection of trackback URLs on another site that were pinged in response to a syndication post.
        /// </summary>
        /// <value>A collection of trackback <see cref="Uri"/>'s on another site that were pinged in response to a post.</value>
        public Collection<Uri> Abouts
        {
            get
            {
                if (trackbackAbouts == null)
                {
                    trackbackAbouts = new Collection<Uri>();
                }
                return trackbackAbouts;
            }
        }
        #endregion

        #region Ping
        /// <summary>
        /// Gets or sets a value that identifies a post's trackback URL.
        /// </summary>
        /// <value>Post's trackback <see cref="Uri"/>.</value>
        public Uri Ping
        {
            get
            {
                return trackbackPing;
            }

            set
            {
                if (value == null)
                {
                    trackbackPing = null;
                }
                else
                {
                    trackbackPing = value;
                }
            }
        }
        #endregion

        //============================================================
        //	COMMON EXTENSION ROUTINES
        //============================================================
        #region Inject(IXPathNavigable xmlDataTarget)
        /// <summary>
        /// Injects the XML data that represents this <see cref="TrackbackSyndicationExtension"/> into the specified XML data target.
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
                //	Append <trackback:ping> element
                //------------------------------------------------------------
                if (this.Ping != null)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "ping", this.Namespace, this.Ping.ToString());
                }

                //------------------------------------------------------------
                //	Enumerate through <trackback:about> values
                //------------------------------------------------------------
                foreach (Uri about in this.Abouts)
                {
                    //------------------------------------------------------------
                    //	Append <trackback:about> element
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
        /// Injects the XML data that represents this <see cref="TrackbackSyndicationExtension"/> using the specified <see cref="XmlWriter"/>.
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
                //	Write <trackback:ping> element
                //------------------------------------------------------------
                if (this.Ping != null)
                {
                    writer.WriteElementString(this.NamespacePrefix, "ping", this.Namespace, this.Ping.ToString());
                }

                //------------------------------------------------------------
                //	Enumerate through <trackback:about> values
                //------------------------------------------------------------
                foreach (Uri about in this.Abouts)
                {
                    //------------------------------------------------------------
                    //	Write <trackback:about> element
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
        /// Returns a <see cref="System.String"/> that represents the current <see cref="TrackbackSyndicationExtension"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="TrackbackSyndicationExtension"/>.</returns>
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
                builder.AppendLine(String.Format(null, "<{0}:ping>{1}</{0}:ping>", this.NamespacePrefix, this.Ping != null ? this.Ping.ToString() : String.Empty));

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
