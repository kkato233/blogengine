/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/17/2007	brian.kuhn		Created AtomCategory Class
****************************************************************************/
using System;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Atom
{
    /// <summary>
    /// Represents information about a category associated with an <see cref="AtomEntry"/> or <see cref="AtomFeed"/>.
    /// </summary>
    [Serializable()]
    public class AtomCategory : SyndicationFeedEntityBase
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the categorization identifier.
        /// </summary>
        private string categoryTerm     = String.Empty;
        /// <summary>
        /// Private member to hold an IRI that identifies a categorization scheme.
        /// </summary>
        private Uri categoryScheme;
        /// <summary>
        /// Private member to hold a human-readable label for display in end-user applications.
        /// </summary>
        private string categoryLabel    = String.Empty;
        /// <summary>
        /// Private member to hold textual content of the category.
        /// </summary>
        private string categoryContent  = String.Empty;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomCategory()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomCategory"/> class.
        /// </summary>
        public AtomCategory() : base()
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

        #region AtomCategory(string term)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomCategory"/> class using the specified term.
        /// </summary>
        /// <param name="term">The categorization identifier.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="term"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="term"/> is an empty string.</exception>
        public AtomCategory(string term)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Term   = term;
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
        /// Gets or sets the textual content of the category.
        /// </summary>
        /// <value>The textual content for the category.</value>
        /// <remarks>This is an optional property. The Atom specification assigns no meaning to the content (if any) of a category.</remarks>
        [XmlText(Type = typeof(System.String))]
        public string Content
        {
            get
            {
                return categoryContent;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    categoryContent = String.Empty;
                }
                else
                {
                    categoryContent = value.Trim();
                }
            }
        }
        #endregion

        #region Label
        /// <summary>
        /// Gets or sets the human-readable label for display in end-user applications.
        /// </summary>
        /// <value>The human-readable label for display in end-user applications.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlAttribute(AttributeName = "label", Type = typeof(System.String))]
        public string Label
        {
            get
            {
                return categoryLabel;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    categoryLabel = String.Empty;
                }
                else
                {
                    categoryLabel = value.Trim();
                }
            }
        }
        #endregion

        #region Scheme
        /// <summary>
        /// Gets or sets an Internationalized Resource Identifier (IRI) that identifies a categorization scheme.
        /// </summary>
        /// <value>An internationalized resource identifier that identifies a categorization scheme.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlAttribute(AttributeName = "scheme", Type = typeof(System.Uri))]
        public Uri Scheme
        {
            get
            {
                return categoryScheme;
            }

            set
            {
                if (value == null)
                {
                    categoryScheme = null;
                }
                else
                {
                    categoryScheme = value;
                }
            }
        }
        #endregion

        #region Term
        /// <summary>
        /// Gets or sets the categorization identifier.
        /// </summary>
        /// <value>A string that identifies the category.</value>
        /// <remarks>This is a required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlAttribute(AttributeName = "term", Type = typeof(System.String))]
        public string Term
        {
            get
            {
                return categoryTerm;
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
                    categoryTerm = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="AtomCategory"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="AtomCategory"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<category term=\"{0}\" scheme=\"{1}\" label=\"{2}\">{3}</category>";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                representation = String.Format(null, resultFormatter, this.Term, this.Scheme != null ? this.Scheme.ToString() : String.Empty, this.Label, this.Content);
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
