/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/17/2007	brian.kuhn		Created AtomLink Class
****************************************************************************/
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Atom
{
    /// <summary>
    /// Represents a reference from an <see cref="AtomEntry"/> or <see cref="AtomFeed"/> to a web resource.
    /// </summary>
    [Serializable()]
    public class AtomLink
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the IRI to the web resource.
        /// </summary>
        private Uri linkHref;
        /// <summary>
        /// Private member to hold a value that indicates the link relation type.
        /// </summary>
        private LinkRelation linkRelation   = LinkRelation.None;
        /// <summary>
        /// Private member to hold textual content of the link.
        /// </summary>
        private string linkContent          = String.Empty;
        /// <summary>
        /// Private member to hold the link's advisory media type.
        /// </summary>
        private string linkMediaType        = String.Empty;
        /// <summary>
        /// Private member to hold the language of the web resource.
        /// </summary>
        private string linkHrefLanguage     = String.Empty;
        /// <summary>
        /// Private member to hold human-readable information about the link.
        /// </summary>
        private string linkTitle            = String.Empty;
        /// <summary>
        /// Private member to hold an advisory length of the linked content in octets.
        /// </summary>
        private long linkLength             = Int64.MinValue;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomLink()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomLink"/> class.
        /// </summary>
        public AtomLink()
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

        #region AtomLink(Uri href)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomLink"/> class using the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="href">The URI to the web resource.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="href"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomLink(Uri href)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Link   = href;
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
        #region Content
        /// <summary>
        /// Gets or sets the textual content of the link.
        /// </summary>
        /// <value>The textual content for the link.</value>
        /// <remarks>This is an optional property. The Atom specification assigns no meaning to the content (if any) of a link.</remarks>
        [XmlText(Type = typeof(System.String))]
        public string Content
        {
            get
            {
                return linkContent;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    linkContent = String.Empty;
                }
                else
                {
                    linkContent = value.Trim();
                }
            }
        }
        #endregion

        #region Language
        /// <summary>
        /// Gets or sets the language of the link resource.
        /// </summary>
        /// <value>The language of the resource pointed to by the link.</value>
        /// <remarks>This is an optional property. See http://www.ietf.org/rfc/rfc3066.txt for more information.</remarks>
        [XmlAttribute(AttributeName = "hreflang", Type = typeof(System.String))]
        public string Language
        {
            get
            {
                return linkHrefLanguage;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    linkHrefLanguage = String.Empty;
                }
                else
                {
                    linkHrefLanguage = value.Trim();
                }
            }
        }
        #endregion

        #region Length
        /// <summary>
        /// Gets or sets the length of the linked content in octets.
        /// </summary>
        /// <value>The length of the linked content in octets.</value>
        /// <remarks>This is an optional property. Length provides a hint about the content length of the representation returned when the IRI in the href attribute is mapped to a URI and dereferenced.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is less than zero.</exception>
        [XmlAttribute(AttributeName = "length", Type = typeof(System.Int32))]
        public long Length
        {
            get
            {
                return linkLength;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                linkLength = value;
            }
        }
        #endregion

        #region Link
        /// <summary>
        /// Gets or sets the URI to the web resource.
        /// </summary>
        /// <value>An internationalized resource identifier associated with the link.</value>
        /// <remarks>This is an required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlAttribute(AttributeName = "href", Type = typeof(System.Uri))]
        public Uri Link
        {
            get
            {
                return linkHref;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    linkHref = value;
                }
            }
        }
        #endregion

        #region Relation
        /// <summary>
        /// Gets or sets a <see cref="LinkRelation"/> enumeration value that indicates the link relation type.
        /// </summary>
        /// <value>The <see cref="LinkRelation"/> enumeration value that indicates the link relation type.</value>
        /// <remarks>Callers can use the <see cref="AtomLink.RelationFromString"/> method to convert a string representation to the corresponding enumeration value.</remarks>
        [XmlAttribute(AttributeName = "rel", Type = typeof(LinkRelation))]
        public LinkRelation Relation
        {
            get
            {
                return linkRelation;
            }

            set
            {
                linkRelation = value;
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets the human-readable information about the link.
        /// </summary>
        /// <value>The human-readable information about the link.</value>
        /// <remarks>This is an optional property. Content is language sensitive.</remarks>
        [XmlAttribute(AttributeName = "title", Type = typeof(System.String))]
        public string Title
        {
            get
            {
                return linkTitle;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    linkTitle = String.Empty;
                }
                else
                {
                    linkTitle = value.Trim();
                }
            }
        }
        #endregion

        #region Type
        /// <summary>
        /// Gets or sets the advisory media type for the link.
        /// </summary>
        /// <value>A hint about the type of the representation that is expected to be returned when the value of the href attribute is dereferenced.</value>
        /// <remarks>This is an optional property. See http://www.ietf.org/rfc/rfc4288.txt for more information.</remarks>
        [XmlAttribute(AttributeName = "type", Type = typeof(System.String))]
        public string Type
        {
            get
            {
                return linkMediaType;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    linkMediaType = String.Empty;
                }
                else
                {
                    linkMediaType = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	STATIC ROUTINES
        //============================================================
        #region RelationFromString(string relation)
        /// <summary>
        /// Returns the <see cref="LinkRelation"/> enumeration value that corresponds to the specified string.
        /// </summary>
        /// <param name="relation">The string representation of the link relation.</param>
        /// <returns>A <see cref="LinkRelation"/> enumeration value that corresponds to the specified string, otherwise returns <b>LinkRelation.None</b>.</returns>
        /// <remarks>This method disregards case of specified relation string.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="relation"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="relation"/> is an empty string.</exception>
        public static LinkRelation RelationFromString(string relation)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            LinkRelation result = LinkRelation.None;

            //------------------------------------------------------------
            //	Attempt to return enumeration for string
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (relation == null)
                {
                    throw new ArgumentNullException("relation");
                }
                else if (String.IsNullOrEmpty(relation.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "relation");
                }

                //------------------------------------------------------------
                //	Determine corresponding enumeration value
                //------------------------------------------------------------
                if (String.Compare(relation, "alternate", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = LinkRelation.Alternate;
                }
                else if (String.Compare(relation, "enclosure", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = LinkRelation.Enclosure;
                }
                else if (String.Compare(relation, "related", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = LinkRelation.Related;
                }
                else if (String.Compare(relation, "self", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = LinkRelation.Self;
                }
                else if (String.Compare(relation, "via", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = LinkRelation.Via;
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
            return result;
        }
        #endregion

        #region RelationToString(LinkRelation relation)
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the specified <see cref="LinkRelation"/>.
        /// </summary>
        /// <param name="relation">The <see cref="LinkRelation"/> to convert to a string.</param>
        /// <returns>A <see cref="System.String"/> that represents the specified <see cref="LinkRelation"/>. If enumeration value is None, returns <b>String.Empty</b>.</returns>
        /// <remarks>Returns <b>String.Empty</b> if enumeration value not recognized.</remarks>
        public static string RelationToString(LinkRelation relation)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string result = String.Empty;

            //------------------------------------------------------------
            //	Attempt to return string representation for enumeration
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Determine corresponding string representation
                //------------------------------------------------------------
                switch (relation)
                {
                    case LinkRelation.Alternate:
                        result  = "alternate";
                        break;
                    case LinkRelation.Enclosure:
                        result  = "enclosure";
                        break;
                    case LinkRelation.None:
                        result  = String.Empty;
                        break;
                    case LinkRelation.Related:
                        result  = "related";
                        break;
                    case LinkRelation.Self:
                        result  = "self";
                        break;
                    case LinkRelation.Via:
                        result  = "via";
                        break;
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
            return result;
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="AtomLink"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="AtomLink"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<link href=\"{0}\" rel=\"{1}\" type=\"{2}\" hreflang=\"{3}\" title=\"{4}\" length=\"{5}\">{6}</link>";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                string href     = this.Link != null ? this.Link.ToString() : String.Empty;
                string length   = this.Length != Int64.MinValue ? this.Length.ToString(CultureInfo.InvariantCulture) : "0";
                representation  = String.Format(null, resultFormatter, href, AtomLink.RelationToString(this.Relation), this.Type, this.Language, this.Title, length, this.Content);
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
            return representation;
        }
        #endregion
    }
}
