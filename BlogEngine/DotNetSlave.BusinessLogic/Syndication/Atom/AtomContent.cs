/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/18/2007	brian.kuhn		Created AtomContent Class
****************************************************************************/
using System;
using System.Xml;
using System.Xml.Serialization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Atom
{
    /// <summary>
    /// Contains or links to the content of a <see cref="AtomEntry"/>.
    /// </summary>
    /// <seealso cref="AtomText"/>
    [Serializable()]
    public class AtomContent : AtomText
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold MIME media type of content.
        /// </summary>
        private string contentMediaType = String.Empty;
        /// <summary>
        /// Private member to hold a URI that points to the content.
        /// </summary>
        private Uri contentSource;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomContent()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomContent"/> class.
        /// </summary>
        public AtomContent()
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
        #region MediaType
        /// <summary>
        /// Gets or sets the MIME media type of the content.
        /// </summary>
        /// <value>The MIME media type of the content.</value>
        /// <remarks>This is an optional property. See http://www.ietf.org/rfc/rfc4288.txt for more information.</remarks>
        [XmlAttribute(AttributeName = "media", Type = typeof(System.String))]
        public string MediaType
        {
            get
            {
                return contentMediaType;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    contentMediaType = String.Empty;
                }
                else
                {
                    contentMediaType = value.Trim();
                }
            }
        }
        #endregion

        #region Source
        /// <summary>
        /// Gets or sets an Internationalized Resource Identifier (IRI) that points to the content.
        /// </summary>
        /// <value>An internationalized resource identifier that points to the content.</value>
        /// <remarks>This is an optional property. If this property is a non-null value, the <see cref="AtomContent.Value"/> must be set to an empty string.</remarks>
        [XmlAttribute(AttributeName = "src", Type = typeof(System.Uri))]
        public Uri Source
        {
            get
            {
                return contentSource;
            }

            set
            {
                if (value == null)
                {
                    contentSource = null;
                }
                else
                {
                    contentSource   = value;
                }
            }
        }
        #endregion

        #region Type
        /// <summary>
        /// Gets or sets a <see cref="TextType"/> enumeration value that indicates the encoding type of the content.
        /// </summary>
        /// <value>The <see cref="TextType"/> enumeration value that indicates the encoding type of the content.</value>
        /// <remarks>
        ///     This is an optional property. Callers can use the <see cref="AtomText.TextTypeFromString"/> method to convert a string representation to the corresponding enumeration value.
        /// </remarks>
        /// <exception cref="ArgumentException">The <see cref="TextType"/> value is <b>None</b>.</exception>
        [XmlAttribute(AttributeName = "type", Type = typeof(TextType))]
        public new TextType Type
        {
            get
            {
                return base.Type;
            }

            set
            {

                if (value == TextType.None)
                {
                    throw new ArgumentException(Resources.ExceptionAtomTextTypeInvalid, "value");
                }
                else
                {
                    base.Type = value;
                }
            }
        }
        #endregion

        #region Value
        /// <summary>
        /// Gets or sets the value of the content.
        /// </summary>
        /// <value>The value of the content.</value>
        /// <remarks>This is an optional property. Content is language sensitive.</remarks>
        [XmlText(Type = typeof(System.String))]
        public new string Value
        {
            get
            {
                return base.Value;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    base.Value  = String.Empty;
                }
                else
                {
                    base.Value  = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="AtomText"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="AtomText"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<content type=\"{0}\" src=\"{1}\">{2}</content>";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                string sourceUrl    = this.Source != null ? this.Source.ToString() : String.Empty;
                representation      = String.Format(null, resultFormatter, AtomText.TextTypeToString(this.Type), sourceUrl, this.Value);
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
