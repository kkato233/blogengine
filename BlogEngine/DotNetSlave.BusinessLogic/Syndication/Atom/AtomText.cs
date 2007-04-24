/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/18/2007	brian.kuhn		Created AtomText Class
****************************************************************************/
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Atom
{
    /// <summary>
    /// Represents human-readable text.
    /// </summary>
    [Serializable()]
    public class AtomText
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold encoding type of the human-readable text.
        /// </summary>
        private TextType textType   = TextType.None;
        /// <summary>
        /// Private member to hold the content for the human-readable text.
        /// </summary>
        private string textValue    = String.Empty;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomText()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomText"/> class.
        /// </summary>
        public AtomText()
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

        #region AtomText(string content)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomText"/> class using the specified content.
        /// </summary>
        /// <param name="content">The content of the human-readable text.</param>
        public AtomText(string content)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Value  = content;
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
        #region Type
        /// <summary>
        /// Gets or sets a <see cref="TextType"/> enumeration value that indicates the encoding type of the human-readable text.
        /// </summary>
        /// <value>The <see cref="TextType"/> enumeration value that indicates the encoding type of the human-readable text.</value>
        /// <remarks>This is an optional property. Callers can use the <see cref="AtomText.TextTypeFromString"/> method to convert a string representation to the corresponding enumeration value.</remarks>
        /// <exception cref="ArgumentException">The <see cref="TextType"/> value is <b>None</b>.</exception>
        [XmlAttribute(AttributeName = "type", Type = typeof(TextType))]
        public TextType Type
        {
            get
            {
                return textType;
            }

            set
            {

                if (value == TextType.None)
                {
                    throw new ArgumentException(Resources.ExceptionAtomTextTypeInvalid, "value");
                }
                else
                {
                    textType = value;
                }
            }
        }
        #endregion

        #region Value
        /// <summary>
        /// Gets or sets the content of the human-readable text.
        /// </summary>
        /// <value>Human-readable text, usually in small quantities.</value>
        /// <remarks>This is an optional property. Content is language sensitive.</remarks>
        [XmlText(Type = typeof(System.String))]
        public string Value
        {
            get
            {
                return textValue;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    textValue   = String.Empty;
                }
                else
                {
                    textValue   = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	STATIC ROUTINES
        //============================================================
        #region TextTypeFromString(string type)
        /// <summary>
        /// Returns the <see cref="TextType"/> enumeration value that corresponds to the specified string.
        /// </summary>
        /// <param name="type">The string representation of the text type.</param>
        /// <returns>A <see cref="TextType"/> enumeration value that corresponds to the specified string, otherwise returns <b>TextType.None</b>.</returns>
        /// <remarks>This method disregards case of specified text type string.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> is an empty string.</exception>
        public static TextType TextTypeFromString(string type)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            TextType result = TextType.None;

            //------------------------------------------------------------
            //	Attempt to return enumeration for string
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }
                else if (String.IsNullOrEmpty(type.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "type");
                }

                //------------------------------------------------------------
                //	Determine corresponding enumeration value
                //------------------------------------------------------------
                if (String.Compare(type, "html", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = TextType.Html;
                }
                else if (String.Compare(type, "text", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = TextType.Text;
                }
                else if (String.Compare(type, "xhtml", true, CultureInfo.InvariantCulture) == 0)
                {
                    result  = TextType.Xhtml;
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

        #region TextTypeToString(TextType textType)
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the specified <see cref="TextType"/>.
        /// </summary>
        /// <param name="textType">The <see cref="TextType"/> to convert to a string.</param>
        /// <returns>A <see cref="System.String"/> that represents the specified <see cref="TextType"/>. If enumeration value is None, returns <b>String.Empty</b>.</returns>
        /// <remarks>Returns <b>String.Empty</b> if enumeration value not recognized.</remarks>
        public static string TextTypeToString(TextType textType)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string result   = String.Empty;

            //------------------------------------------------------------
            //	Attempt to return string representation for enumeration
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Determine corresponding string representation
                //------------------------------------------------------------
                switch (textType)
                {
                    case TextType.Html:
                        result  = "html";
                        break;
                    case TextType.None:
                        result  = String.Empty;
                        break;
                    case TextType.Text:
                        result  = "text";
                        break;
                    case TextType.Xhtml:
                        result  = "xhtml";
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
        /// Returns a <see cref="System.String"/> that represents the current <see cref="AtomText"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="AtomText"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<text type=\"{0}\">{1}</text>";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                representation      = String.Format(null, resultFormatter, AtomText.TextTypeToString(this.Type), this.Value);
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
