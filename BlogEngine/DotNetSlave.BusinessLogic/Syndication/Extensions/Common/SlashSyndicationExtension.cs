/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/24/2007	brian.kuhn		Created SlashSyndicationExtension Class
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
    /// Extends a syndication specification to augment the metadata with elements specific to Slash-based sites.
    /// </summary>
    /// <seealso cref="SyndicationExtension" />
    [Serializable()]
    public class SlashSyndicationExtension : SyndicationExtension
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a brief description of the extension.
        /// </summary>
        private string extensionDescription     = "Provides a way to augment the metadata with elements specific to Slash-based sites.";
        /// <summary>
        /// Private member to hold a uniform resource location that points to documentation about the extension.
        /// </summary>
        private Uri extensionDocumentation      = new Uri("http://web.resource.org/rss/1.0/modules/slash/");
        /// <summary>
        /// Private member to hold the unique XML namespace for the extension.
        /// </summary>
        private string extensionNamespace       = "http://purl.org/rss/1.0/modules/slash/";
        /// <summary>
        /// Private member to hold the XML namespace prefix for the extension.
        /// </summary>
        private string extensionNamespacePrefix = "slash";
        /// <summary>
        /// Private member to hold the human readable name for the extension.
        /// </summary>
        private string extensionTitle           = "Slash Site Summary Module For RSS";
        /// <summary>
        /// Private member to hold collection of extension targets.
        /// </summary>
        Collection<ExtensionTarget> extensionTargets;
        /// <summary>
        /// Private member to hold value that identifies the slash section item applies to.
        /// </summary>
        private string slashSection             = String.Empty;
        /// <summary>
        /// Private member to hold value that identifies the slash department item applies to.
        /// </summary>
        private string slashDepartment          = String.Empty;
        /// <summary>
        /// Private member to hold slash comments integer.
        /// </summary>
        private int slashComments               = Int32.MinValue;
        /// <summary>
        /// Private member to hold collection of slash hit parade integers.
        /// </summary>
        private Collection<int> slashHitParade;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SlashSyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="SlashSyndicationExtension"/> class.
        /// </summary>
        public SlashSyndicationExtension()
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

        #region SlashSyndicationExtension(int comments)
        /// <summary>
        /// Initializes a new instance of the <see cref="SlashSyndicationExtension"/> class using the specified number of comments.
        /// </summary>
        /// <param name="comments">The number of comments.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="comments"/> is less than zero.</exception>
        public SlashSyndicationExtension(int comments)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Comments   = comments;
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
        #region Comments
        /// <summary>
        /// Gets or sets the number of comments.
        /// </summary>
        /// <value>Positive integer representing the number of comments.</value>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is less than zero.</exception>
        public int Comments
        {
            get
            {
                return slashComments;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                slashComments = value;
            }
        }
        #endregion

        #region Department
        /// <summary>
        /// Gets or sets the department name.
        /// </summary>
        /// <value>Slash section name.</value>
        public string Department
        {
            get
            {
                return slashDepartment;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    slashDepartment = String.Empty;
                }
                else
                {
                    slashDepartment = value.Trim();
                }
            }
        }
        #endregion

        #region HitParade
        /// <summary>
        /// Gets a collection of integers that represent the hit parade.
        /// </summary>
        /// <value>A collection of integers that represent the hit parade.</value>
        public Collection<int> HitParade
        {
            get
            {
                if (slashHitParade == null)
                {
                    slashHitParade = new Collection<int>();
                }
                return slashHitParade;
            }
        }
        #endregion

        #region Section
        /// <summary>
        /// Gets or sets the section name.
        /// </summary>
        /// <value>Slash section name.</value>
        public string Section
        {
            get
            {
                return slashSection;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    slashSection = String.Empty;
                }
                else
                {
                    slashSection = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	COMMON EXTENSION ROUTINES
        //============================================================
        #region Inject(IXPathNavigable xmlDataTarget)
        /// <summary>
        /// Injects the XML data that represents this <see cref="SlashSyndicationExtension"/> into the specified XML data target.
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
                //	Append <slash:section> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Section))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "section", this.Namespace, this.Section);
                }

                //------------------------------------------------------------
                //	Append <slash:department> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Department))
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "department", this.Namespace, this.Department);
                }

                //------------------------------------------------------------
                //	Append <slash:comments> element
                //------------------------------------------------------------
                if (this.Comments != Int32.MinValue)
                {
                    navigator.AppendChildElement(this.NamespacePrefix, "comments", this.Namespace, this.Comments.ToString(NumberFormatInfo.InvariantInfo));
                }

                //------------------------------------------------------------
                //	Determine if there are hit parade values to write
                //------------------------------------------------------------
                if (this.HitParade.Count > 0)
                {
                    //------------------------------------------------------------
                    //	Build comma delimited string using hit parade values
                    //------------------------------------------------------------
                    string delimitedValues  = SlashSyndicationExtension.CreateDelimitedString(this.HitParade, ",");

                    //------------------------------------------------------------
                    //	Append <slash:hit_parade> element
                    //------------------------------------------------------------
                    navigator.AppendChildElement(this.NamespacePrefix, "hit_parade", this.Namespace, delimitedValues);
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
        /// Injects the XML data that represents this <see cref="SlashSyndicationExtension"/> using the specified <see cref="XmlWriter"/>.
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
                //	Write <slash:section> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Section))
                {
                    writer.WriteElementString(this.NamespacePrefix, "section", this.Namespace, this.Section);
                }

                //------------------------------------------------------------
                //	Write <slash:department> element
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Department))
                {
                    writer.WriteElementString(this.NamespacePrefix, "department", this.Namespace, this.Department);
                }

                //------------------------------------------------------------
                //	Write <slash:comments> element
                //------------------------------------------------------------
                if (this.Comments != Int32.MinValue)
                {
                    writer.WriteElementString(this.NamespacePrefix, "comments", this.Namespace, this.Comments.ToString(NumberFormatInfo.InvariantInfo));
                }

                //------------------------------------------------------------
                //	Determine if there are hit parade values to write
                //------------------------------------------------------------
                if (this.HitParade.Count > 0)
                {
                    //------------------------------------------------------------
                    //	Build comma delimited string using hit parade values
                    //------------------------------------------------------------
                    string delimitedValues  = SlashSyndicationExtension.CreateDelimitedString(this.HitParade, ",");

                    //------------------------------------------------------------
                    //	Write <slash:hit_parade> element
                    //------------------------------------------------------------
                    writer.WriteElementString(this.NamespacePrefix, "hit_parade", this.Namespace, delimitedValues);
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
        //	PRIVATE ROUTINES
        //============================================================
        #region CreateDelimitedString(Collection<int> collection, string delimiter)
        /// <summary>
        /// Creates a delimited string using the specification collection and delimiter.
        /// </summary>
        /// <param name="collection">The collection to generate a delimited string for.</param>
        /// <param name="delimiter">The string delimiter used to delimit items in the string.</param>
        /// <returns>A delimited string using the specification collection and delimiter.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="collection"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="delimiter"/> is an empty string.</exception>
        private static string CreateDelimitedString(Collection<int> collection, string delimiter)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string delimitedValues  = String.Empty;

            //------------------------------------------------------------
            //	Attempt to create delimited string
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (collection == null)
                {
                    throw new ArgumentNullException("collection");
                }
                if (String.IsNullOrEmpty(delimiter))
                {
                    throw new ArgumentNullException("delimiter");
                }

                //------------------------------------------------------------
                //	Create string array
                //------------------------------------------------------------
                string[] values = new string[collection.Count];

                //------------------------------------------------------------
                //	Enumerate through collection items
                //------------------------------------------------------------
                for (int i = 0; i < collection.Count; i++)
                {
                    //------------------------------------------------------------
                    //	Set array element value
                    //------------------------------------------------------------
                    values[i]   = collection[i].ToString(NumberFormatInfo.InvariantInfo);
                }

                //------------------------------------------------------------
                //	Generate delimited string by joining string array elements
                //------------------------------------------------------------
                delimitedValues = String.Join(delimiter, values);
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
            return delimitedValues;
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="SlashSyndicationExtension"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="SlashSyndicationExtension"/>.</returns>
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
                builder.AppendLine(String.Format(null, "<{0}:section>{1}</{0}:section>", this.NamespacePrefix, this.Section));
                builder.AppendLine(String.Format(null, "<{0}:department>{1}</{0}:department>", this.NamespacePrefix, this.Department));
                builder.AppendLine(String.Format(null, "<{0}:comments>{1}</{0}:comments>", this.NamespacePrefix, this.Comments != Int32.MinValue ? this.Comments.ToString(NumberFormatInfo.InvariantInfo) : String.Empty));
                if (this.HitParade.Count > 0)
                {
                    string delimitedValues  = SlashSyndicationExtension.CreateDelimitedString(this.HitParade, ",");
                    builder.AppendLine(String.Format(null, "<{0}:hit_parade>{1}</{0}:hit_parade>", this.NamespacePrefix, delimitedValues));
                }
                else
                {
                    return String.Format(null, "<{0}:hit_parade/>", this.NamespacePrefix);
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
