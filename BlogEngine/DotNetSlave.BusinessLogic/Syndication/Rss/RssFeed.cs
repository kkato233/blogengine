/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/12/2007	brian.kuhn		Created RssFeed Class
****************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Security;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using BlogEngine.Core.Properties;
using BlogEngine.Core.Syndication.Data;

namespace BlogEngine.Core.Syndication.Rss
{
    /// <summary>
    /// Represents a Really Simple Syndication (RSS) syndication feed.
    /// </summary>
    /// <remarks>See http://www.rssboard.org/rss-specification for further information about the RSS syndication format.</remarks>
    [Serializable()]
    [XmlRoot(ElementName = "rss", DataType = "feedType")]
    public class RssFeed : SyndicationFeed
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the version of the syndication format that this feed conforms to.
        /// </summary>
        private string feedVersion                  = "2.0";
        /// <summary>
        /// Private member to hold the syndication format that this feed implements.
        /// </summary>
        private SyndicationFormat feedFormat        = SyndicationFormat.Rss;
        /// <summary>
        /// Private member to hold channel for this feed.
        /// </summary>
        private RssChannel rssChannel               = new RssChannel();
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssFeed()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeed"/> class.
        /// </summary>
        public RssFeed() : base()
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

        #region RssFeed(SyndicationFeedSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeed"/> class using the supplied <see cref="SyndicationFeedSettings"/>.
        /// </summary>
        /// <param name="settings">The set of features that this syndication feed supports.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssFeed(SyndicationFeedSettings settings) : base(settings)
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
        //	PUBLIC PROPERTIES
        //============================================================
        #region Format
        /// <summary>
        /// Gets the syndication format that this feed implements.
        /// </summary>
        /// <value>The <see cref="SyndicationFormat"/> enumeration value that indicates the type of syndication format that this feed implements.</value>
        [XmlIgnore()]
        public override SyndicationFormat Format
        {
            get
            {
                return feedFormat;
            }
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets or sets the version of the syndication format that this feed conforms to.
        /// </summary>
        /// <value>The version of the syndication format that this feed conforms to.</value>
        /// <remarks>The default value is '2.0'.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
        [XmlAttribute(AttributeName = "version", DataType = "string")]
        public override string Version
        {
            get
            {
                return feedVersion;
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
                    feedVersion = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC SPECIFICATION PROPERTIES
        //============================================================
        #region Channel
        /// <summary>
        /// Gets or sets the channel for this feed.
        /// </summary>
        /// <value>The channel for this feed.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlElement(ElementName = "channel", Type = typeof(RssChannel))]
        public RssChannel Channel
        {
            get
            {
                return rssChannel;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    rssChannel = value;
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN SAVE ROUTINES
        //============================================================
        #region Save(XmlWriter writer)
        /// <summary>
        /// Saves the <see cref="RssFeed"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to save the syndication feed.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> value is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication feed.</exception>
        public override void Save(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Attempt to save syndication feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (writer == null)
                {
                    throw new ArgumentNullException("writer");
                }

                //------------------------------------------------------------
                //	Write start of XML document
                //------------------------------------------------------------
                writer.WriteStartDocument();

                //------------------------------------------------------------
                //	Create XML data adapter used to write syndication feed
                //------------------------------------------------------------
                RssEngineSyndicationFeedAdapter adapter    = new RssEngineSyndicationFeedAdapter();

                //------------------------------------------------------------
                //	Write syndication feed using adapter
                //------------------------------------------------------------
                adapter.Write(this, writer);
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
