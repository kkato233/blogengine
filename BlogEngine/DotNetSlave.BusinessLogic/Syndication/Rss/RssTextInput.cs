/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created RssTextInput Class
****************************************************************************/
using System;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Rss
{
    /// <summary>
    /// Represents a textual-input that can be associated to a <see cref="RssChannel"/>.
    /// </summary>
    /// <remarks>Commonly used to specify a search engine box or to allow a reader to provide feedback. Most feed aggregators ignore this entity.</remarks>
    [Serializable()]
    public class RssTextInput
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the label of the Submit button in the text input area.
        /// </summary>
        private string textInputTitle       = String.Empty;
        /// <summary>
        /// Private member to hold the description of the text input area.
        /// </summary>
        private string textInputDescription = String.Empty;
        /// <summary>
        /// Private member to hold the name of the text object in the text input area.
        /// </summary>
        private string textInputName        = String.Empty;
        /// <summary>
        /// Private member to hold URL of the CGI script or web handler that processes text input requests.
        /// </summary>
        private Uri textInputLink;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssTextInput()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssTextInput"/> class.
        /// </summary>
        public RssTextInput()
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

        #region RssTextInput(string title, string description, string name, Uri link)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssTextInput"/> class using the specified title, description, name, and link.
        /// </summary>
        /// <param name="title">The label of the Submit button in the text input area.</param>
        /// <param name="description">The description of the text input area.</param>
        /// <param name="name">The name of the text object in the text input area.</param>
        /// <param name="link">The URL of the CGI script or web handler that processes text input requests.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="title"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="description"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="name"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="link"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="title"/> is an empty string -or- the <paramref name="description"/> is an empty string -or- the <paramref name="name"/> is an empty string.</exception>
        public RssTextInput(string title, string description, string name, Uri link)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Description    = description;
                this.Link           = link;
                this.Name           = name;
                this.Title          = title;
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
        #region Description
        /// <summary>
        /// Gets or sets the description of the text input area.
        /// </summary>
        /// <value>The description of the text input area.</value>
        /// <remarks>This is an required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlElement(ElementName = "description", Type = typeof(System.String))]
        public string Description
        {
            get
            {
                return textInputDescription;
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
                    textInputDescription = value.Trim();
                }
            }
        }
        #endregion

        #region Link
        /// <summary>
        /// Gets or sets the URL of the CGI script or web handler that processes text input requests.
        /// </summary>
        /// <value>The URL of the CGI script or web handler that processes text input requests.</value>
        /// <remarks>This is an required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "link", Type = typeof(System.Uri))]
        public Uri Link
        {
            get
            {
                return textInputLink;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    textInputLink = value;
                }
            }
        }
        #endregion

        #region Name
        /// <summary>
        /// Gets or sets the name of the text object in the text input area.
        /// </summary>
        /// <value>The name of the text object in the text input area.</value>
        /// <remarks>This is an required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlElement(ElementName = "name", Type = typeof(System.String))]
        public string Name
        {
            get
            {
                return textInputName;
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
                    textInputName = value.Trim();
                }
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets the label of the Submit button in the text input area.
        /// </summary>
        /// <value>The label of the Submit button in the text input area.</value>
        /// <remarks>This is an required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlElement(ElementName = "title", Type = typeof(System.String))]
        public string Title
        {
            get
            {
                return textInputTitle;
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
                    textInputTitle = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="RssTextInput"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="RssTextInput"/>.</returns>
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
                string link     = this.Link != null ? this.Link.ToString() : String.Empty;

                representation.AppendLine("<textInput>");
                representation.AppendLine(String.Format(null, "\t<title>{0}</title>", this.Title));
                representation.AppendLine(String.Format(null, "\t<description>{0}</description>", this.Description));
                representation.AppendLine(String.Format(null, "\t<name>{0}</name>", this.Name));
                representation.AppendLine(String.Format(null, "\t<link>{0}</link>", link));
                representation.Append("</textInput>");
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
