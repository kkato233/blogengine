/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/27/2007	brian.kuhn	Created SyndicationHandler Class
05/03/2007  brian.kuhn  Removed using(adapter) calls to prevent the 
                        category dictionary from being cleared.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;

using BlogEngine.Core;

using BlogEngine.Core.Syndication;
using BlogEngine.Core.Syndication.Atom;
using BlogEngine.Core.Syndication.Data;
using BlogEngine.Core.Syndication.Rss;

namespace BlogEngine.Core.Web.HttpHandlers
{
    /// <summary>
    /// Implements a custom handler to synchronously process HTTP Web requests for a syndication feed.
    /// </summary>
    /// <remarks>This handler can generate syndication feeds in a variety of formats and filtering options based on the query string parmaeters provided.</remarks>
    /// <seealso cref="IHttpHandler"/>
    /// <seealso cref="SyndicationFeed"/>
    public class SyndicationHandler : IHttpHandler
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the syndication format that the generated feed will conform to.
        /// </summary>
        private SyndicationFormat syndicationFormat = SyndicationFormat.Rss;
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Format
        /// <summary>
        /// Gets or sets the syndication format that the generated feed will conform to.
        /// </summary>
        /// <value>A <see cref="SyndicationFormat"/> enumeration value that indicates the format that the generated syndication feed will conform to.</value>
        public SyndicationFormat Format
        {
            get
            {
                return syndicationFormat;
            }

            set
            {
                syndicationFormat = value;
            }
        }
        #endregion

        #region IsReusable
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// </summary>
        /// <value><b>true</b> if the <b>IHttpHandler</b> instance is reusable; otherwise, <b>false</b>.</value>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region ProcessRequest(HttpContext context)
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom <b>HttpHandler</b> that implements the <see cref="IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="HttpContext"/> object that provides references to the intrinsic server objects (for example, <b>Request</b>, <b>Response</b>, <b>Session</b>, and <b>Server</b>) used to service HTTP requests.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="context"/> is a null reference (Nothing in Visual Basic).</exception>
        public void ProcessRequest(HttpContext context)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            List<Post> posts;
            Guid categoryId = Guid.Empty;
            string author   = String.Empty;
            int postCount   = 0;

            //------------------------------------------------------------
            //	Attempt to process the request
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                //------------------------------------------------------------
                //	Set default syndication format based on settings
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(BlogSettings.Instance.SyndicationFormat))
                {
                    if(String.Compare(BlogSettings.Instance.SyndicationFormat, "rss", true, CultureInfo.InvariantCulture) == 0)
                    {
                        this.Format = SyndicationFormat.Rss;
                    }
                    else if (String.Compare(BlogSettings.Instance.SyndicationFormat, "atom", true, CultureInfo.InvariantCulture) == 0)
                    {
                        this.Format = SyndicationFormat.Atom;
                    }
                }

                //------------------------------------------------------------
                //	Extract query string information
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(context.Request.QueryString["category"]) && context.Request.QueryString["category"].Length == 36)
                {
                    categoryId  = new Guid(context.Request.QueryString["category"]);
                }
                if (!String.IsNullOrEmpty(context.Request.QueryString["author"]))
                {
                    author      = context.Request.QueryString["author"].Trim();
                }
                if (!String.IsNullOrEmpty(context.Request.QueryString["format"]))
                {
                    switch (context.Request.QueryString["format"].Trim().ToLowerInvariant())
                    {
                        case "atom":
                            this.Format = SyndicationFormat.Atom;
                            break;

                        case "rss":
                            this.Format = SyndicationFormat.Rss;
                            break;
                    }
                }

                //------------------------------------------------------------
                //	Get correct sub-set of posts based on parameters
                //------------------------------------------------------------
                if (categoryId != Guid.Empty)
                {
                    //------------------------------------------------------------
                    //	Get posts based on specified category identifier
                    //------------------------------------------------------------
                    posts   = Post.GetPostsByCategory(categoryId);
                }
                else if (!String.IsNullOrEmpty(author))
                {
                    //------------------------------------------------------------
                    //	Get posts based on specified author name
                    //------------------------------------------------------------
                    posts   = Post.GetPostsByAuthor(author);
                }
                else
                {
                    //------------------------------------------------------------
                    //	Get all current posts
                    //------------------------------------------------------------
                    posts   = Post.Posts;
                }

                //------------------------------------------------------------
                //	Verify posts available for processing
                //------------------------------------------------------------
                if (posts != null)
                {
                    //------------------------------------------------------------
                    //	Filter number of posts based configured range
                    //------------------------------------------------------------
                    postCount       = BlogSettings.Instance.PostsPerFeed;
                    if (postCount > posts.Count)
                    {
                        postCount   = posts.Count;
                    }
                    //posts           = posts.GetRange(0, postCount);
                    List<Post> list = new List<Post>();
                    int counter = 0;
                    foreach (Post post in posts)
                    {
                      if (counter == postCount)
                        break;

                      if (post.IsVisible)
                      {
                        list.Add(post);
                        counter++;
                      }
                    }
                    posts = list;

                    //------------------------------------------------------------
                    //	Set the response header information
                    //------------------------------------------------------------
                    SyndicationHandler.SetHeaderInformation(context, posts, this.Format);

                    //------------------------------------------------------------
                    //	Generate the syndication feed
                    //------------------------------------------------------------
                    this.WriteFeed(context, posts);
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
        //	PRIVATE ROUTINES
        //============================================================
        #region SetHeaderInformation(HttpContext context, List<Post> posts, SyndicationFormat format)
        /// <summary>
        /// Sets the response header information.
        /// </summary>
        /// <param name="context">An <see cref="HttpContext"/> object that provides references to the intrinsic server objects (for example, <b>Request</b>, <b>Response</b>, <b>Session</b>, and <b>Server</b>) used to service HTTP requests.</param>
        /// <param name="posts">The collection of <see cref="Post"/> instances used when setting the response header details.</param>
        /// <param name="format">The format of the syndication feed being generated.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="context"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="posts"/> is a null reference (Nothing in Visual Basic).</exception>
        private static void SetHeaderInformation(HttpContext context, List<Post> posts, SyndicationFormat format)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            DateTime lastModified   = DateTime.Now;
            bool notModified        = false;
            string eTag             = String.Empty;
            string ifModifiedSince  = String.Empty;

            //------------------------------------------------------------
            //	Attempt to set response header details
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (posts == null)
                {
                    throw new ArgumentNullException("posts");
                }

                //------------------------------------------------------------
                //	Determine date feed was last modified from posts
                //------------------------------------------------------------
                if (posts.Count > 0)
                {
                    lastModified    = posts[0].DateModified;
                }

                //------------------------------------------------------------
                //	Extract ETag from request headers
                //------------------------------------------------------------
                eTag            = context.Request.Headers["If-None-Match"];

                //------------------------------------------------------------
                //	Extract modified-since from request headers
                //------------------------------------------------------------
                ifModifiedSince = context.Request.Headers["if-modified-since"];

                //------------------------------------------------------------
                //	Determine if response has not been modified since last request
                //------------------------------------------------------------
                if (!String.IsNullOrEmpty(eTag))
                {
                    notModified = (eTag.Equals(lastModified.Ticks.ToString()));
                }
                else
                {
                    if (!String.IsNullOrEmpty(ifModifiedSince))
                    {
                        // ifModifiedSince can have a length param in there
                        // If-Modified-Since: Wed, 29 Dec 2004 18:34:27 GMT; length=126275
                        if (ifModifiedSince.IndexOf(";") > -1)
                        {
                            ifModifiedSince = ifModifiedSince.Split(';').GetValue(0).ToString();
                        }

                        DateTime ifModifiedDate;
                        if (DateTime.TryParse(ifModifiedSince, out ifModifiedDate))
                        {
                            notModified = (lastModified <= ifModifiedDate);
                        }
                    }
                }

                //------------------------------------------------------------
                //	Set response details based on whether has been modified
                //------------------------------------------------------------
                if (notModified)
                {
                    //------------------------------------------------------------
                    //	Indicate Request has not been modified
                    //------------------------------------------------------------
                    context.Response.StatusCode         = 304;
                    context.Response.SuppressContent    = true;
                    context.Response.End();
                }
                else
                {
                    //------------------------------------------------------------
                    //	Request has been modified, set details
                    //------------------------------------------------------------
                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
                    // Mads set this to DateTime.Now because lastModified threws an error (utcDate)
                    context.Response.Cache.SetLastModified(DateTime.Now);
                    context.Response.Cache.SetETag(lastModified.Ticks.ToString());

                    switch (format)
                    {
                        case SyndicationFormat.Atom:
                            context.Response.ContentType    = "application/atom+xml";
                            context.Response.AppendHeader("Content-Disposition", "inline; filename=atom.xml");
                            break;

                        case SyndicationFormat.Rss:
                            context.Response.ContentType    = "application/rss+xml";
                            context.Response.AppendHeader("Content-Disposition", "inline; filename=rss.xml");
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("format");
                    }
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

        #region WriteFeed(HttpContext context, List<Post> posts)
        /// <summary>
        /// Creates a feed based on specified format, fills the feed using the appropriate adapter, and then writes the feed XML data to the response stream.
        /// </summary>
        /// <param name="context">An <see cref="HttpContext"/> object that provides references to the intrinsic server objects (for example, <b>Request</b>, <b>Response</b>, <b>Session</b>, and <b>Server</b>) used to service HTTP requests.</param>
        /// <param name="posts">The collection of <see cref="Post"/> instances used when generating the syndication feed.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="context"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="posts"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteFeed(HttpContext context, List<Post> posts)
        {
            //------------------------------------------------------------
            //	Attempt to generate feed
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (posts == null)
                {
                    throw new ArgumentNullException("posts");
                }

                //------------------------------------------------------------
                //	Determine output processing based on format
                //------------------------------------------------------------
                switch(this.Format)
                {
                    case SyndicationFormat.Atom:

                        //------------------------------------------------------------
                        //	Create Atom 1.0 feed instance
                        //------------------------------------------------------------
                        AtomFeed atomFeed   = new AtomFeed();

                        //------------------------------------------------------------
                        //	Create adapter to fill feed
                        //------------------------------------------------------------
                        AtomEngineSyndicationFeedAdapter atomAdapter    = new AtomEngineSyndicationFeedAdapter(posts, BlogSettings.Instance, CategoryDictionary.Instance);

                        //------------------------------------------------------------
                        //	Set adapter properties
                        //------------------------------------------------------------
                        atomAdapter.FeedLocation    = new Uri(context.Request.Url.ToString().Substring(0, context.Request.Url.ToString().IndexOf("syndication.axd")));
                        atomAdapter.WebRoot         = Utils.AbsoluteWebRoot;

                        //------------------------------------------------------------
                        //	Fill feed using adapter
                        //------------------------------------------------------------
                        atomAdapter.Fill(atomFeed);

                        //------------------------------------------------------------
                        //	Write feed to context stream
                        //------------------------------------------------------------
                        atomFeed.Save(context.Response.OutputStream);

                        break;

                    case SyndicationFormat.Rss:

                        //------------------------------------------------------------
                        //	Create RSS 2.0 feed instance
                        //------------------------------------------------------------
                        RssFeed rssFeed = new RssFeed();

                        //------------------------------------------------------------
                        //	Create adapter to fill feed
                        //------------------------------------------------------------
                        RssEngineSyndicationFeedAdapter rssAdapter  = new RssEngineSyndicationFeedAdapter(posts, BlogSettings.Instance, CategoryDictionary.Instance);

                        //------------------------------------------------------------
                        //	Set adapter properties
                        //------------------------------------------------------------
                        rssAdapter.FeedLocation = new Uri(context.Request.Url.ToString().Substring(0, context.Request.Url.ToString().IndexOf("syndication.axd")));
                        rssAdapter.WebRoot      = Utils.AbsoluteWebRoot;

                        //------------------------------------------------------------
                        //	Fill feed using adapter
                        //------------------------------------------------------------
                        rssAdapter.Fill(rssFeed);

                        //------------------------------------------------------------
                        //	Write feed to context stream
                        //------------------------------------------------------------
                        rssFeed.Save(context.Response.OutputStream);
                        break;
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
    }
}
