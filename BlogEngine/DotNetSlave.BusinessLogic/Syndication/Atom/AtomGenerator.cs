/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/17/2007	brian.kuhn		Created AtomGenerator Class
****************************************************************************/
using System;
using System.Xml;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Atom
{
    /// <summary>
    /// Represents the agent used to generate a <see cref="AtomFeed"/>, for debugging and other purposes.
    /// </summary>
    [Serializable()]
    public class AtomGenerator
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the human-readable name for the generating agent.
        /// </summary>
        private string generatorName    = String.Empty;
        /// <summary>
        /// Private member to hold a IRI that is relevant to the agent.
        /// </summary>
        private Uri generatorUri;
        /// <summary>
        /// Private member to hold the version of the generating agent.
        /// </summary>
        private string generatorVersion = String.Empty;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomGenerator()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomGenerator"/> class.
        /// </summary>
        public AtomGenerator()
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

        #region AtomGenerator(string name)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomGenerator"/> class using the specified name.
        /// </summary>
        /// <param name="name">The human-readable name for the generating agent.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is an empty string.</exception>
        public AtomGenerator(string name)
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
        #region Name
        /// <summary>
        /// Gets or sets the human-readable name for the generating agent.
        /// </summary>
        /// <value>The human-readable name for the generating agent.</value>
        /// <remarks>This is a required property. Entities such as "&amp;" and "&lt;" represent their corresponding characters, not markup.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlText(Type = typeof(System.String))]
        public string Name
        {
            get
            {
                return generatorName;
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
                    generatorName = value.Trim();
                }
            }
        }
        #endregion

        #region Uri
        /// <summary>
        /// Gets or sets an Internationalized Resource Identifier (IRI) that is relevant to the agent.
        /// </summary>
        /// <value>An internationalized resource identifier that is relevant to the agent.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlAttribute(AttributeName = "uri", Type = typeof(System.Uri))]
        public Uri Uri
        {
            get
            {
                return generatorUri;
            }

            set
            {
                if (value == null)
                {
                    generatorUri = null;
                }
                else
                {
                    generatorUri = value;
                }
            }
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets or sets the version of the generating agent.
        /// </summary>
        /// <value>The version of the generating agent.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlAttribute(AttributeName = "version", Type = typeof(System.String))]
        public string Version
        {
            get
            {
                return generatorVersion;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    generatorVersion = String.Empty;
                }
                else
                {
                    generatorVersion = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="AtomGenerator"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="AtomGenerator"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter = "<generator uri=\"{0}\" version=\"{1}\">{2}</generator>";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                representation = String.Format(null, resultFormatter, this.Uri != null ? this.Uri.ToString() : String.Empty, this.Version, this.Name);
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
