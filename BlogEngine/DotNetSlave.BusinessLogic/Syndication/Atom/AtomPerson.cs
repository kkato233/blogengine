/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/17/2007	brian.kuhn		Created AtomPerson Class
****************************************************************************/
using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Atom
{
    /// <summary>
    /// Represents a person, corporation, or similar entity.
    /// </summary>
    [Serializable()]
    public class AtomPerson
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the human-readable name for the person.
        /// </summary>
        private string personName           = String.Empty;
        /// <summary>
        /// Private member to hold a URI that conveys an IRI associated with the person.
        /// </summary>
        private Uri personUri;
        /// <summary>
        /// Private member to hold an e-mail address associated with the person.
        /// </summary>
        private string personEmailAddress   = String.Empty;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomPerson()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomPerson"/> class.
        /// </summary>
        public AtomPerson()
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

        #region AtomPerson(string name)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomPerson"/> class using the specified name.
        /// </summary>
        /// <param name="name">The human-readable name for the person.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is an empty string.</exception>
        public AtomPerson(string name)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Name   = name;
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
        #region EmailAddress
        /// <summary>
        /// Gets or sets the e-mail address associated with the person.
        /// </summary>
        /// <value>The e-mail address associated with the person.</value>
        /// <remarks>This is an optional property. Email address should be formatted to conform to RFC 2822. See http://www.ietf.org/rfc/rfc2822.txt for more information.</remarks>
        [XmlElement(ElementName = "email", Type = typeof(System.String))]
        public string EmailAddress
        {
            get
            {
                return personEmailAddress;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    personEmailAddress = String.Empty;
                }
                else
                {
                    personEmailAddress = value.Trim();
                }
            }
        }
        #endregion

        #region Name
        /// <summary>
        /// Gets or sets the human-readable name for the person.
        /// </summary>
        /// <value>The human-readable name for the person.</value>
        /// <remarks>This is a required property. Content is language sensitive.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlElement(ElementName = "name", Type = typeof(System.String))]
        public string Name
        {
            get
            {
                return personName;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else if (String.IsNullOrEmpty(value.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "value");
                }
                else
                {
                    personName = value.Trim();
                }
            }
        }
        #endregion

        #region Uri
        /// <summary>
        /// Gets or sets an Internationalized Resource Identifier (IRI) associated with the person.
        /// </summary>
        /// <value>An internationalized resource identifier associated with the person.</value>
        /// <remarks>This is an optional property. See http://www.ietf.org/rfc/rfc3987.txt for more information.</remarks>
        [XmlElement(ElementName = "uri", Type = typeof(System.Uri))]
        public Uri Uri
        {
            get
            {
                return personUri;
            }

            set
            {
                if (value == null)
                {
                    personUri = null;
                }
                else
                {
                    personUri = value;
                }
            }
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
            StringBuilder representation    = new StringBuilder();

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                representation.AppendLine("<person>");
                representation.AppendLine(String.Format(null, "\t<name>{0}</name>", this.Name));
                representation.AppendLine(String.Format(null, "\t<uri>{0}</uri>", this.Uri != null ? this.Uri.ToString() : String.Empty));
                representation.AppendLine(String.Format(null, "\t<email>{0}</email>", this.EmailAddress));
                representation.Append("</person>");
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
            return representation.ToString();
        }
        #endregion
    }
}
