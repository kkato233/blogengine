/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/16/2007	brian.kuhn		Created RssSource Class
****************************************************************************/
using System;
using System.Xml;
using System.Xml.Serialization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Rss
{
    /// <summary>
    /// Represents a reference to a source <see cref="RssChannel"/> that an <see cref="RssItem"/> comes from.
    /// </summary>
    /// <remarks>The purpose of <b>RssSource</b> is to propagate credit for links, to publicize the sources of news items. It can be used in the Post command of an aggregator. It should be generated automatically when forwarding an item from an aggregator to a weblog authoring tool.</remarks>
    [Serializable()]
    public class RssSource
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold URI location of the source channel.
        /// </summary>
        private Uri sourceUrl;
        /// <summary>
        /// Private member to hold the name of the source channel.
        /// </summary>
        private string sourceTitle  = String.Empty;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssSource()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssSource"/> class.
        /// </summary>
        public RssSource()
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

        #region RssSource(Uri url)
        /// <summary>
        /// Initializes a new instance of the <see cref="RssSource"/> class using the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="url">The URL location of the source channel.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="url"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssSource(Uri url)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class properties
                //------------------------------------------------------------
                this.Url    = url;
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
        #region Title
        /// <summary>
        /// Gets or sets the name of the source channel.
        /// </summary>
        /// <value>The name of the source channel.</value>
        /// <remarks>This is an optional property.</remarks>
        [XmlText(Type = (typeof(System.String)))]
        public string Title
        {
            get
            {
                return sourceTitle;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    sourceTitle = String.Empty;
                }
                else
                {
                    sourceTitle = value.Trim();
                }
            }
        }
        #endregion

        #region Url
        /// <summary>
        /// Gets or sets the URL location of the source channel.
        /// </summary>
        /// <value>The URL location of the source channel.</value>
        /// <remarks>This is a required property.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        [XmlAttribute(AttributeName = "url", Type = typeof(System.Uri))]
        public Uri Url
        {
            get
            {
                return sourceUrl;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    sourceUrl = value;
                }
            }
        }
        #endregion

        //============================================================
        //	OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="RssSource"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="RssSource"/>.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string representation   = String.Empty;
            string resultFormatter  = "<source url=\"{0}\">{1}</source>";

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Build string representation
                //------------------------------------------------------------
                string url      = this.Url != null ? this.Url.ToString() : String.Empty;
                representation  = String.Format(null, resultFormatter, url, this.Title);
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
