/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/11/2007	brian.kuhn		Created SyndicationExtensionDictionary Class
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BlogEngine.Core.Syndication.Extensions
{
    /// <summary>
    /// Represents a collection of <see cref="SyndicationExtension"/> instances keyed by their unique namespace.
    /// </summary>
    [Serializable()]
    public class SyndicationExtensionDictionary : Dictionary<string, SyndicationExtension>
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SyndicationExtensionDictionary()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtensionDictionary"/> class.
        /// </summary>
        public SyndicationExtensionDictionary() : base()
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

        #region SyndicationExtensionDictionary(SerializationInfo info, StreamingContext context)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtensionDictionary"/> class.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that store the serialization information.</param>
        /// <param name="context">A <see cref="StreamingContext"/> that describes the source, destination and context for a given serialized stream.</param>
        protected SyndicationExtensionDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Initialization handled by base class
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
        //	PUBLIC ROUTINES
        //============================================================
        #region Add(SyndicationExtension extension)
        /// <summary>
        /// Adds the specified <see cref="SyndicationExtension"/> to the dictionary using its Namespace property as the key.
        /// </summary>
        /// <param name="extension">The <see cref="SyndicationExtension"/> to add to the dictionary.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="extension"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="extension"/>'s Namespace property is a null or empty string.</exception>
        public void Add(SyndicationExtension extension)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (extension == null)
                {
                    throw new ArgumentNullException("extension");
                }

                if (String.IsNullOrEmpty(extension.Namespace))
                {
                    throw new ArgumentException(String.Format(null, "The {0} extension has a Namespace that is a null/empty string.", extension.Title), "extension");
                }
                
                //------------------------------------------------------------
                //	Add extension to the dictionary
                //------------------------------------------------------------
                base.Add(extension.Namespace.ToLowerInvariant(), extension);
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
    }
}
