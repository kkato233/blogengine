/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/11/2007	brian.kuhn		Created SyndicationFeedSettings Class
****************************************************************************/
using System;
using System.Net;

using DotNetSlave.BlogEngine.Syndication.Extensions;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication
{
    /// <summary>
    /// Specifies a set of features to support on the <see cref="SyndicationFeed"/> object filled using the <see cref="DotNetSlave.BlogEngine.BusinessLogic.Syndication.Data.ISyndicationFeedAdapter.Fill(SyndicationFeed)"/> method.
    /// </summary>
    public sealed class SyndicationFeedSettings
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold credentials utilized to access a secure syndication feed.
        /// </summary>
        [NonSerialized()]
        private ICredentials feedCredentials;
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

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Credentials
        /// <summary>
        /// Gets or sets authentication information used to access a secure syndication feed.
        /// </summary>
        /// <value>An <see cref="ICredentials"/> that contains the authentication credentials used to access a secure syndication feed. The default is a null reference (Nothing in Visual Basic).</value>
        /// <remarks>
        ///     <para>
        ///         The Credentials property contains authentication information to identify the maker of the feed request. The Credentials property can be either a <see cref="NetworkCredential"/>, in which case 
        ///         the user, password, and domain information contained in the <b>NetworkCredential</b> object is used to authenticate the feed request, or it can be a <see cref="CredentialCache"/>, in which case the 
        ///         Uniform Resource Identifier (URI) of the feed request is used to determine the user, password, and domain information to use to authenticate the request.
        ///     </para>
        /// 
        ///     <para>
        ///         The NTLM authentication scheme cannot be used to impersonate another user. Kerberos must be specially configured to support impersonation.
        ///     </para>
        /// </remarks>
        public ICredentials Credentials
        {
            get
            {
                return feedCredentials;
            }

            set
            {
                if (value == null)
                {
                    feedCredentials = null;
                }
                else
                {
                    feedCredentials = value;
                }
            }
        }
        #endregion

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
        /// Gets a collection of <see cref="ISyndicationExtension"/> instances that the syndication feed supports.
        /// </summary>
        /// <value>A collection of <see cref="ISyndicationExtension"/> instances keyed by their namespace that the syndication feed supports. The default is an empty collection.</value>
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
