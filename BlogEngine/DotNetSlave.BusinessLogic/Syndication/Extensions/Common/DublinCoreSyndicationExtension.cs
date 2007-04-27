/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/24/2007	brian.kuhn		Created DublinCoreSyndicationExtension Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace BlogEngine.Core.Syndication.Extensions.Common
{
    /// <summary>
    /// Extends a syndication specification to provide standard meta-data elements as defined by the Dublin Core Metadata Element Set, version 1.1.
    /// </summary>
    /// <seealso cref="SyndicationExtension" />
    [Serializable()]
    public class DublinCoreSyndicationExtension : SyndicationExtension
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a brief description of the extension.
        /// </summary>
        private string extensionDescription     = "Provides standard meta-data elements as defined by the Dublin Core Metadata Element Set.";
        /// <summary>
        /// Private member to hold a uniform resource location that points to documentation about the extension.
        /// </summary>
        private Uri extensionDocumentation      = new Uri("http://dublincore.org/documents/1999/07/02/dces/");
        /// <summary>
        /// Private member to hold the unique XML namespace for the extension.
        /// </summary>
        private string extensionNamespace       = "http://purl.org/dc/elements/1.1/";
        /// <summary>
        /// Private member to hold the XML namespace prefix for the extension.
        /// </summary>
        private string extensionNamespacePrefix = "dc";
        /// <summary>
        /// Private member to hold the human readable name for the extension.
        /// </summary>
        private string extensionTitle           = "Dublin Core Site Summary Module For RSS";
        /// <summary>
        /// Private member to hold collection of extension targets.
        /// </summary>
        Collection<ExtensionTarget> extensionTargets;
        /// <summary>
        /// Private member to hold name given to the resource.
        /// </summary>
        private string dublinCoreTitle          = String.Empty;
        /// <summary>
        /// Private member to hold entity primarily responsible for making the content of the resource.
        /// </summary>
        private string dublinCoreCreator        = String.Empty;
        /// <summary>
        /// Private member to hold topic of the content of the resource.
        /// </summary>
        private string dublinCoreSubject        = String.Empty;
        /// <summary>
        /// Private member to hold account of the content of the resource.
        /// </summary>
        private string dublinCoreDescription    = String.Empty;
        /// <summary>
        /// Private member to hold entity responsible for making the resource available.
        /// </summary>
        private string dublinCorePublisher      = String.Empty;
        /// <summary>
        /// Private member to hold entity responsible for making contributions to the content of the resource.
        /// </summary>
        private string dublinCoreContributor    = String.Empty;
        /// <summary>
        /// Private member to hold date associated with an event in the life cycle of the resource.
        /// </summary>
        private DateTime dublinCoreDate         = DateTime.MinValue;
        /// <summary>
        /// Private member to hold nature or genre of the content of the resource.
        /// </summary>
        private string dublinCoreType           = String.Empty;
        /// <summary>
        /// Private member to hold physical or digital manifestation of the resource.
        /// </summary>
        private string dublinCoreFormat         = String.Empty;
        /// <summary>
        /// Private member to hold unambiguous reference to the resource within a given context.
        /// </summary>
        private string dublinCoreIdentifier     = String.Empty;
        /// <summary>
        /// Private member to hold reference to a resource from which the present resource is derived.
        /// </summary>
        private string dublinCoreSource         = String.Empty;
        /// <summary>
        /// Private member to hold language of the intellectual content of the resource.
        /// </summary>
        private string dublinCoreLanguage       = String.Empty;
        /// <summary>
        /// Private member to hold reference to a related resource.
        /// </summary>
        private string dublinCoreRelation       = String.Empty;
        /// <summary>
        /// Private member to hold extent or scope of the content of the resource.
        /// </summary>
        private string dublinCoreCoverage       = String.Empty;
        /// <summary>
        /// Private member to hold information about rights held in and over the resource.
        /// </summary>
        private string dublinCoreRights         = String.Empty;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region DublinCoreSyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="DublinCoreSyndicationExtension"/> class.
        /// </summary>
        public DublinCoreSyndicationExtension()
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
                    extensionTargets.Add(ExtensionTarget.AtomFeed);
                    extensionTargets.Add(ExtensionTarget.AtomLink);
                    extensionTargets.Add(ExtensionTarget.RssChannel);
                    extensionTargets.Add(ExtensionTarget.RssImage);
                    extensionTargets.Add(ExtensionTarget.RssItem);
                    extensionTargets.Add(ExtensionTarget.RssTextInput);

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
        #region Contributor
        /// <summary>
        /// Gets or sets the entity responsible for making contributions to the content of the resource.
        /// </summary>
        /// <value>The entity responsible for making contributions to the content of the resource.</value>
        /// <remarks>Typically, the name of a person, an organisation, or a service should be used to indicate the entity.</remarks>
        public string Contributor
        {
            get
            {
                return dublinCoreContributor;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreContributor = String.Empty;
                }
                else
                {
                    dublinCoreContributor = value.Trim();
                }
            }
        }
        #endregion

        #region Coverage
        /// <summary>
        /// Gets or sets the extent or scope of the content of the resource.
        /// </summary>
        /// <value>The extent or scope of the content of the resource.</value>
        /// <remarks>
        ///     Coverage will typically include spatial location (a place name or geographic coordinates), 
        ///     temporal period (a period label, date, or date range) or jurisdiction (such as a named administrative entity).
        /// </remarks>
        public string Coverage
        {
            get
            {
                return dublinCoreCoverage;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreCoverage = String.Empty;
                }
                else
                {
                    dublinCoreCoverage = value.Trim();
                }
            }
        }
        #endregion

        #region Creator
        /// <summary>
        /// Gets or sets the entity primarily responsible for making the content of the resource.
        /// </summary>
        /// <value>The entity primarily responsible for making the content of the resource.</value>
        /// <remarks>Typically, the name of a person, an organisation, or a service should be used to indicate the entity.</remarks>
        public string Creator
        {
            get
            {
                return dublinCoreCreator;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreCreator = String.Empty;
                }
                else
                {
                    dublinCoreCreator = value.Trim();
                }
            }
        }
        #endregion

        #region Date
        /// <summary>
        /// Gets or sets the date associated with an event in the life cycle of the resource.
        /// </summary>
        /// <value>The date associated with an event in the life cycle of the resource.</value>
        /// <remarks>Typically, Date will be associated with the creation or availability of the resource.</remarks>
        public DateTime Date
        {
            get
            {
                return dublinCoreDate;
            }

            set
            {
                dublinCoreDate  = value;
            }
        }
        #endregion

        #region Format
        /// <summary>
        /// Gets or sets the physical or digital manifestation of the resource.
        /// </summary>
        /// <value>The physical or digital manifestation of the resource.</value>
        /// <remarks>
        ///     Typically, Format may include the media-type or dimensions of the resource. Format may be used to determine 
        ///     the software, hardware or other equipment needed to display or operate the resource.
        /// </remarks>
        public string Format
        {
            get
            {
                return dublinCoreFormat;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreFormat = String.Empty;
                }
                else
                {
                    dublinCoreFormat = value.Trim();
                }
            }
        }
        #endregion

        #region Identifier
        /// <summary>
        /// Gets or sets an unambiguous reference to the resource within a given context.
        /// </summary>
        /// <value>An unambiguous reference to the resource within a given context.</value>
        /// <remarks>
        ///     Recommended best practice is to identify the resource by means of a string or number conforming to a formal identification system. 
        ///     Example formal identification systems include the Uniform Resource Identifier (URI) (including the Uniform Resource Locator (URL)), 
        ///     the Digital Object Identifier (DOI) and the International Standard Book Number (ISBN).
        /// </remarks>
        public string Identifier
        {
            get
            {
                return dublinCoreIdentifier;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreIdentifier = String.Empty;
                }
                else
                {
                    dublinCoreIdentifier = value.Trim();
                }
            }
        }
        #endregion

        #region Language
        /// <summary>
        /// Gets or sets the language of the intellectual content of the resource.
        /// </summary>
        /// <value>The language of the intellectual content of the resource.</value>
        /// <remarks>
        ///     Recommended best practice for the values of the Language element is defined by RFC 1766 [RFC1766] which includes a two-letter Language Code (taken from the ISO 639 standard [ISO639]), 
        ///     followed optionally, by a two-letter Country Code (taken from the ISO 3166 standard [ISO3166]).
        /// </remarks>
        /// <example>en-US</example>
        public string Language
        {
            get
            {
                return dublinCoreLanguage;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreLanguage = String.Empty;
                }
                else
                {
                    dublinCoreLanguage = value.Trim();
                }
            }
        }
        #endregion

        #region Publisher
        /// <summary>
        /// Gets or sets the entity responsible for making the resource available.
        /// </summary>
        /// <value>The entity responsible for making the resource available.</value>
        /// <remarks>Typically, the name of a person, an organisation, or a service should be used to indicate the entity.</remarks>
        public string Publisher
        {
            get
            {
                return dublinCorePublisher;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCorePublisher = String.Empty;
                }
                else
                {
                    dublinCorePublisher = value.Trim();
                }
            }
        }
        #endregion

        #region Relation
        /// <summary>
        /// Gets or sets a reference to a related resource.
        /// </summary>
        /// <value>A reference to a related resource.</value>
        /// <remarks>Recommended best practice is to reference the resource by means of a string or number conforming to a formal identification system.</remarks>
        public string Relation
        {
            get
            {
                return dublinCoreRelation;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreRelation = String.Empty;
                }
                else
                {
                    dublinCoreRelation = value.Trim();
                }
            }
        }
        #endregion

        #region ResourceDescription
        /// <summary>
        /// Gets or sets an account of the content of the resource.
        /// </summary>
        /// <value>An account of the content of the resource.</value>
        /// <remarks>Description may include but is not limited to: an abstract, table of contents, reference to a graphical representation of content or a free-text account of the content.</remarks>
        public string ResourceDescription
        {
            get
            {
                return dublinCoreDescription;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreDescription = String.Empty;
                }
                else
                {
                    dublinCoreDescription = value.Trim();
                }
            }
        }
        #endregion

        #region ResourceTitle
        /// <summary>
        /// Gets or sets the name given to the resource.
        /// </summary>
        /// <value>The name given to the resource.</value>
        /// <remarks>Typically, a Title will be a name by which the resource is formally known.</remarks>
        public string ResourceTitle
        {
            get
            {
                return dublinCoreTitle;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreTitle = String.Empty;
                }
                else
                {
                    dublinCoreTitle = value.Trim();
                }
            }
        }
        #endregion

        #region Rights
        /// <summary>
        /// Gets or sets the information about rights held in and over the resource.
        /// </summary>
        /// <value>The information about rights held in and over the resource.</value>
        /// <remarks>
        ///     Typically, a Rights element will contain a rights management statement for the resource, or reference a service 
        ///     providing such information. Rights information often encompasses Intellectual Property Rights (IPR), Copyright, and various Property Rights.
        /// </remarks>
        public string Rights
        {
            get
            {
                return dublinCoreRights;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreRights = String.Empty;
                }
                else
                {
                    dublinCoreRights = value.Trim();
                }
            }
        }
        #endregion

        #region Source
        /// <summary>
        /// Gets or sets a reference to a resource from which the present resource is derived.
        /// </summary>
        /// <value>A reference to a resource from which the present resource is derived.</value>
        /// <remarks>
        /// The present resource may be derived from the Source resource in whole or in part. 
        /// Recommended best practice is to reference the resource by means of a string or number conforming to a formal identification system.
        /// </remarks>
        public string Source
        {
            get
            {
                return dublinCoreSource;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreSource = String.Empty;
                }
                else
                {
                    dublinCoreSource = value.Trim();
                }
            }
        }
        #endregion

        #region Subject
        /// <summary>
        /// Gets or sets the topic of the content of the resource..
        /// </summary>
        /// <value>The topic of the content of the resource..</value>
        /// <remarks>Typically, a Subject will be expressed as keywords, key phrases or classification codes that describe a topic of the resource.</remarks>
        public string Subject
        {
            get
            {
                return dublinCoreSubject;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreSubject = String.Empty;
                }
                else
                {
                    dublinCoreSubject = value.Trim();
                }
            }
        }
        #endregion

        #region Type
        /// <summary>
        /// Gets or sets the nature or genre of the content of the resource.
        /// </summary>
        /// <value>The nature or genre of the content of the resource.</value>
        /// <remarks>Type includes terms describing general categories, functions, genres, or aggregation levels for content.</remarks>
        public string Type
        {
            get
            {
                return dublinCoreType;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    dublinCoreType = String.Empty;
                }
                else
                {
                    dublinCoreType = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	COMMON EXTENSION ROUTINES
        //============================================================
        #region Inject(IXPathNavigable xmlDataTarget)
        /// <summary>
        /// Injects the XML data that represents this <see cref="DublinCoreSyndicationExtension"/> into the specified XML data target.
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
                //	Append <dc:title> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.ResourceTitle))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "title", this.Namespace, this.ResourceTitle);
                }

                //------------------------------------------------------------
                //	Append <dc:creator> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Creator))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "creator", this.Namespace, this.Creator);
                }

                //------------------------------------------------------------
                //	Append <dc:subject> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Subject))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "subject", this.Namespace, this.Subject);
                }

                //------------------------------------------------------------
                //	Append <dc:description> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.ResourceDescription))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "description", this.Namespace, this.ResourceDescription);
                }

                //------------------------------------------------------------
                //	Append <dc:publisher> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Publisher))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "publisher", this.Namespace, this.Publisher);
                }

                //------------------------------------------------------------
                //	Append <dc:contributor> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Contributor))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "contributor", this.Namespace, this.Contributor);
                }

                //------------------------------------------------------------
                //	Append <dc:date> element if value set
                //------------------------------------------------------------
                if (this.Date != DateTime.MinValue)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "date", this.Namespace, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                }

                //------------------------------------------------------------
                //	Append <dc:type> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Type))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "type", this.Namespace, this.Type);
                }

                //------------------------------------------------------------
                //	Append <dc:format> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Format))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "format", this.Namespace, this.Format);
                }

                //------------------------------------------------------------
                //	Append <dc:identifier> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Identifier))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "identifier", this.Namespace, this.Identifier);
                }

                //------------------------------------------------------------
                //	Append <dc:source> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Source))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "source", this.Namespace, this.Source);
                }

                //------------------------------------------------------------
                //	Append <dc:language> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Language))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "language", this.Namespace, this.Language);
                }

                //------------------------------------------------------------
                //	Append <dc:relation> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Relation))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "relation", this.Namespace, this.Relation);
                }

                //------------------------------------------------------------
                //	Append <dc:coverage> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Coverage))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "coverage", this.Namespace, this.Coverage);
                }

                //------------------------------------------------------------
                //	Append <dc:rights> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Rights))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "rights", this.Namespace, this.Rights);
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
        /// Injects the XML data that represents this <see cref="DublinCoreSyndicationExtension"/> using the specified <see cref="XmlWriter"/>.
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
                //	Write <dc:title> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.ResourceTitle))
                {
                    writer.WriteElementString(this.NamespacePrefix, "title", this.Namespace, this.ResourceTitle);
                }

                //------------------------------------------------------------
                //	Write <dc:creator> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Creator))
                {
                    writer.WriteElementString(this.NamespacePrefix, "creator", this.Namespace, this.Creator);
                }

                //------------------------------------------------------------
                //	Write <dc:subject> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Subject))
                {
                    writer.WriteElementString(this.NamespacePrefix, "subject", this.Namespace, this.Subject);
                }

                //------------------------------------------------------------
                //	Write <dc:description> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.ResourceDescription))
                {
                    writer.WriteElementString(this.NamespacePrefix, "description", this.Namespace, this.ResourceDescription);
                }

                //------------------------------------------------------------
                //	Write <dc:publisher> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Publisher))
                {
                    writer.WriteElementString(this.NamespacePrefix, "publisher", this.Namespace, this.Publisher);
                }

                //------------------------------------------------------------
                //	Write <dc:contributor> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Contributor))
                {
                    writer.WriteElementString(this.NamespacePrefix, "contributor", this.Namespace, this.Contributor);
                }

                //------------------------------------------------------------
                //	Write <dc:date> element if value set
                //------------------------------------------------------------
                if (this.Date != DateTime.MinValue)
                {
                    writer.WriteElementString(this.NamespacePrefix, "date", this.Namespace, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                }

                //------------------------------------------------------------
                //	Write <dc:type> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Type))
                {
                    writer.WriteElementString(this.NamespacePrefix, "type", this.Namespace, this.Type);
                }

                //------------------------------------------------------------
                //	Write <dc:format> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Format))
                {
                    writer.WriteElementString(this.NamespacePrefix, "format", this.Namespace, this.Format);
                }

                //------------------------------------------------------------
                //	Write <dc:identifier> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Identifier))
                {
                    writer.WriteElementString(this.NamespacePrefix, "identifier", this.Namespace, this.Identifier);
                }

                //------------------------------------------------------------
                //	Write <dc:source> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Source))
                {
                    writer.WriteElementString(this.NamespacePrefix, "source", this.Namespace, this.Source);
                }

                //------------------------------------------------------------
                //	Write <dc:language> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Language))
                {
                    writer.WriteElementString(this.NamespacePrefix, "language", this.Namespace, this.Language);
                }

                //------------------------------------------------------------
                //	Write <dc:relation> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Relation))
                {
                    writer.WriteElementString(this.NamespacePrefix, "relation", this.Namespace, this.Relation);
                }

                //------------------------------------------------------------
                //	Write <dc:coverage> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Coverage))
                {
                    writer.WriteElementString(this.NamespacePrefix, "coverage", this.Namespace, this.Coverage);
                }

                //------------------------------------------------------------
                //	Write <dc:rights> element if value set
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Rights))
                {
                    writer.WriteElementString(this.NamespacePrefix, "rights", this.Namespace, this.Rights);
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
            StringBuilder builder = new StringBuilder();

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                builder.AppendLine(String.Format(null, "<{0}:contributor>{1}</{0}:contributor>", this.NamespacePrefix, this.Contributor));
                builder.AppendLine(String.Format(null, "<{0}:coverage>{1}</{0}:coverage>", this.NamespacePrefix, this.Coverage));
                builder.AppendLine(String.Format(null, "<{0}:creator>{1}</{0}:creator>", this.NamespacePrefix, this.Creator));
                builder.AppendLine(String.Format(null, "<{0}:date>{1}</{0}:date>", this.NamespacePrefix, this.Date != DateTime.MinValue ? this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : String.Empty));
                builder.AppendLine(String.Format(null, "<{0}:format>{1}</{0}:format>", this.NamespacePrefix, this.Format));
                builder.AppendLine(String.Format(null, "<{0}:identifier>{1}</{0}:identifier>", this.NamespacePrefix, this.Identifier));
                builder.AppendLine(String.Format(null, "<{0}:language>{1}</{0}:language>", this.NamespacePrefix, this.Language));
                builder.AppendLine(String.Format(null, "<{0}:publisher>{1}</{0}:publisher>", this.NamespacePrefix, this.Publisher));
                builder.AppendLine(String.Format(null, "<{0}:relation>{1}</{0}:relation>", this.NamespacePrefix, this.Relation));
                builder.AppendLine(String.Format(null, "<{0}:description>{1}</{0}:description>", this.NamespacePrefix, this.ResourceDescription));
                builder.AppendLine(String.Format(null, "<{0}:title>{1}</{0}:title>", this.NamespacePrefix, this.ResourceTitle));
                builder.AppendLine(String.Format(null, "<{0}:rights>{1}</{0}:rights>", this.NamespacePrefix, this.Rights));
                builder.AppendLine(String.Format(null, "<{0}:source>{1}</{0}:source>", this.NamespacePrefix, this.Source));
                builder.AppendLine(String.Format(null, "<{0}:subject>{1}</{0}:subject>", this.NamespacePrefix, this.Subject));
                builder.AppendLine(String.Format(null, "<{0}:type>{1}</{0}:type>", this.NamespacePrefix, this.Type));
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
