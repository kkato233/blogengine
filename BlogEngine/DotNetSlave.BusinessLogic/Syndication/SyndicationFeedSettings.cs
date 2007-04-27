/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/11/2007	brian.kuhn		Created SyndicationFeedSettings Class
****************************************************************************/
using System;
using System.Net;

using BlogEngine.Core.Syndication.Extensions;

namespace BlogEngine.Core.Syndication
{
    /// <summary>
    /// Specifies a set of features to support on the <see cref="SyndicationFeed"/> object filled using the <see cref="BlogEngine.Core.Syndication.Data.ISyndicationFeedAdapter.Fill(SyndicationFeed)"/> method.
    /// </summary>
    public sealed class SyndicationFeedSettings
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a collection of extensions supported by the syndication feed.
        /// </summary>
        private SyndicationExtensionDictionary feedSupportedExtensions  = new SyndicationExtensionDictionary();
        /// <summary>
        /// Private member to hold a value indicating if feed write/save operations should attempt to minimize the size of the outputted XML that represents the feed.
        /// </summary>
        private bool minimizeFeedOutputSize;
        #endregion

        //============================================================
		//	CONSTRUCTORS
        //============================================================
        #region SyndicationFeedSettings()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationFeedSettings"/> class.
        /// </summary>
        public SyndicationFeedSettings()
		{
			//------------------------------------------------------------
			//	Attempt to handle class initialization
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

        #region SyndicationFeedSettings(SyndicationExtensionDictionary supportedExtensions)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationFeedSettings"/> class using the specified supported extensions.
        /// </summary>
        /// <param name="supportedExtensions">A collection of extensions supported by the syndication feed.</param>
        public SyndicationFeedSettings(SyndicationExtensionDictionary supportedExtensions)
        {
            //------------------------------------------------------------
            //	Attempt to handle class initialization
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Fill supported extensions collection
                //------------------------------------------------------------
                foreach (SyndicationExtension extension in supportedExtensions.Values)
                {
                    feedSupportedExtensions.Add(extension);
                }
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
        #region MinimizeOutputSize
        /// <summary>
        /// Gets or sets a value indicating if syndication feed write/save operations should attempt to minimize the physical size of the outputted XML data that represents the feed.
        /// </summary>
        /// <value><b>true</b> if feed output size should be as small as possible; otherwise <b>false</b>. The default value is <b>false</b>.</value>
        public bool MinimizeOutputSize
        {
            get
            {
                return minimizeFeedOutputSize;
            }

            set
            {
                minimizeFeedOutputSize = value;
            }
        }
        #endregion

        #region SupportedExtensions
        /// <summary>
        /// Gets a collection of <see cref="SyndicationExtension"/> instances that the syndication feed supports.
        /// </summary>
        /// <value>A collection of <see cref="SyndicationExtension"/> instances keyed by their namespace that the syndication feed supports. The default is an empty collection.</value>
        public SyndicationExtensionDictionary SupportedExtensions
        {
            get
            {
                return feedSupportedExtensions;
            }
        }
        #endregion
    }
}
