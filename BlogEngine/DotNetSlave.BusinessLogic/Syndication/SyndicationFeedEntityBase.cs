/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/25/2007	brian.kuhn		Created SyndicationFeedEntityBase Class
****************************************************************************/
using System;
using System.Xml.Serialization;

using BlogEngine.Core.Syndication.Extensions;

namespace BlogEngine.Core.Syndication
{
    /// <summary>
    /// Represents the base class that all syndication feed entities inherit from to provide shared properties and methods.
    /// </summary>
    [Serializable()]
    public class SyndicationFeedEntityBase
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a collection of the syndication extensions that have been implemented for the entity.
        /// </summary>
        private SyndicationExtensionDictionary extensions;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SyndicationFeedEntityBase()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationFeedEntityBase"/> class.
        /// </summary>
        public SyndicationFeedEntityBase()
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
        #region Extensions
        /// <summary>
        /// Gets the collection of extensions associated to this entity.
        /// </summary>
        /// <value>A collection of <see cref="SyndicationExtension"/> instances associated to this entity.</value>
        [XmlIgnore()]
        public SyndicationExtensionDictionary Extensions
        {
            get
            {
                if (extensions == null)
                {
                    extensions = new SyndicationExtensionDictionary();
                }
                return extensions;
            }
        }
        #endregion
    }
}
