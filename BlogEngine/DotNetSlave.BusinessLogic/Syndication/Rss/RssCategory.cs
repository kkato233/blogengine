/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created RssCategory Class
****************************************************************************/
using System;
using System.Xml;
using System.Xml.Serialization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Rss
{
    /// <summary>
    /// Represents a categorization taxonomy that can be applied to <see cref="RssChannel"/> and <see cref="RssItem"/> entities.
    /// </summary>
    /// <remarks>You may associate as many categories as you need to, for different domains, and to have an item cross-referenced in different parts of the same domain.</remarks>
    [Serializable()]
    public class RssCategory
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a forward-slash-separated string that identifies a hierarchic location in the indicated taxonomy.
        /// </summary>
        private string categoryValue    = String.Empty;
        /// <summary>
        /// Private member to hold a domain designator that identifies a categorization taxonomy.
        /// </summary>
        private string categoryDomain   = String.Empty;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssCategory()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssCategory"/> class.
        /// </summary>
        public RssCategory()
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

        #region RssCategory(string value)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssCategory"/> class using the specified name.
        /// </summary>
        /// <param name="value">A forward-slash-separated string that identifies a hierarchic location in the indicated taxonomy.</param>
        /// <example>Application Developers</example>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        public RssCategory(string value)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Value  = value;
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
        #region Domain
        /// <summary>
        /// Gets or sets a string that identifies a categorization taxonomy.
        /// </summary>
        /// <value>A string that identifies a categorization taxonomy.</value>
        /// <remarks>This is an optional property.</remarks>
        /// <example>mscomdomain:Audience</example>
        [XmlAttribute(AttributeName = "domain", Type = typeof(System.String))]
        public string Domain
        {
            get
            {
                return categoryDomain;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    categoryDomain = String.Empty;
                }
                else
                {
                    categoryDomain = value.Trim();
                }
            }
        }
        #endregion

        #region Value
        /// <summary>
        /// Gets or sets a forward-slash-separated string that identifies a hierarchic location in the indicated taxonomy.
        /// </summary>
        /// <value>A forward-slash-separated string that identifies a hierarchic location in the indicated taxonomy.</value>
        /// <remarks>This is a required property.</remarks>
        /// <example>Application Developers</example>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlText(Type = (typeof(System.String)))]
        public string Value
        {
            get
            {
                return categoryValue;
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
                    categoryValue = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="RssCategory"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="RssCategory"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<category domain=\"{0}\">{1}</category>";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(this.Domain))
                {
                    representation  = String.Format(null, resultFormatter, this.Domain, this.Value);
                }
                else
                {
                    representation  = String.Format(null, "<category>{0}</category>", this.Value);
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
            return representation;
        }
        #endregion
    }
}
