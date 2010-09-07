namespace Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Xml;

    using BlogEngine.Core;

    /// <summary>
    /// Creates and displays a dynamic blogroll.
    /// </summary>
    public class Blogroll : Control
    {
        #region Constants and Fields

        /// <summary>
        ///     The sync root.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        ///     The items.
        /// </summary>
        private static List<BlogRequest> items;

        /// <summary>
        ///     The last updated.
        /// </summary>
        private static DateTime lastUpdated = DateTime.Now;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Blogroll" /> class.
        /// </summary>
        static Blogroll()
        {
            BlogRollItem.Saved += BlogRollItemSaved;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.
        /// </param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Page.IsPostBack || this.Page.IsCallback)
            {
                return;
            }

            var ul = DisplayBlogroll();
            var sw = new StringWriter();
            ul.RenderControl(new HtmlTextWriter(sw));
            var html = sw.ToString();

            writer.WriteLine("<div id=\"blogroll\">");
            writer.WriteLine(html);
            writer.WriteLine("</div>");
        }

        /// <summary>
        /// Adds a blog to the item collection and start retrieving the blogs.
        /// </summary>
        /// <param name="br">
        /// The blog roll item.
        /// </param>
        private static void AddBlog(BlogRollItem br)
        {
            var affected = items.FirstOrDefault(r => r.RollItem.Equals(br));

            if (affected != null)
            {
                return;
            }

            var req = (HttpWebRequest)WebRequest.Create(br.FeedUrl);
            req.Credentials = CredentialCache.DefaultNetworkCredentials;

            var blogRequest = new BlogRequest(br, req);
            items.Add(blogRequest);
            req.BeginGetResponse(ProcessRespose, blogRequest);
        }

        /// <summary>
        /// Handles the Saved event of the BlogRollItem control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="BlogEngine.Core.SavedEventArgs"/> instance containing the event data.
        /// </param>
        private static void BlogRollItemSaved(object sender, SavedEventArgs e)
        {
            if ((e.Action != SaveAction.Insert && e.Action != SaveAction.Update) && e.Action != SaveAction.Delete)
            {
                return;
            }

            switch (e.Action)
            {
                case SaveAction.Insert:
                    AddBlog((BlogRollItem)sender);
                    break;

                case SaveAction.Delete:
                    var affected = items.FirstOrDefault(req => req.RollItem.Equals(sender));
                    items.Remove(affected);
                    break;
            }

            if (items.Count > 0)
            {
                // Re-sort _Items collection in case sorting of blogroll items was changed.
                items.Sort((br1, br2) => br1.RollItem.CompareTo(br2.RollItem));
            }
        }

        /// <summary>
        /// Ensures that the name is no longer than the MaxLength.
        /// </summary>
        /// <param name="textToShorten">
        /// The text To Shorten.
        /// </param>
        /// <returns>
        /// The ensure length.
        /// </returns>
        private static string EnsureLength(string textToShorten)
        {
            return textToShorten.Length > BlogSettings.Instance.BlogrollMaxLength
                       ? string.Format(
                           "{0}...", textToShorten.Substring(0, BlogSettings.Instance.BlogrollMaxLength).Trim())
                       : HttpUtility.HtmlEncode(textToShorten);
        }

        /// <summary>
        /// Gets the request and processes the response.
        /// </summary>
        /// <param name="async">
        /// The async.
        /// </param>
        private static void ProcessRespose(IAsyncResult async)
        {
            var blogReq = (BlogRequest)async.AsyncState;
            try
            {
                using (var response = (HttpWebResponse)blogReq.Request.EndGetResponse(async))
                {
                    var responseStream = response.GetResponseStream();
                    var doc = new XmlDocument();
                    if (responseStream != null)
                    {
                        doc.Load(responseStream);
                    }

                    var nodes = doc.SelectNodes("rss/channel/item");
                    if (nodes != null)
                    {
                        foreach (XmlNode node in nodes)
                        {
                            var nodeTitle = node.SelectSingleNode("title");
                            if (nodeTitle != null)
                            {
                                var title = nodeTitle.InnerText;
                                blogReq.ItemTitles.Add(title);
                            }

                            var nodeLink = node.SelectSingleNode("link");
                            if (nodeLink != null)
                            {
                                var link = nodeLink.InnerText;
                                blogReq.ItemLinks.Add(link);
                            }

                            // var date = DateTime.Now;
                            // if (node.SelectSingleNode("pubDate") != null)
                            // {
                            // date = DateTime.Parse(node.SelectSingleNode("pubDate").InnerText);
                            // }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Adds the RSS child items.
        /// </summary>
        /// <param name="item">
        /// The blog request item.
        /// </param>
        /// <param name="li">
        /// The list item.
        /// </param>
        private static void AddRssChildItems(BlogRequest item, HtmlGenericControl li)
        {
            if (item.ItemTitles.Count > 0 && BlogSettings.Instance.BlogrollVisiblePosts > 0)
            {
                var div = new HtmlGenericControl("ul");
                for (var i = 0; i < item.ItemTitles.Count; i++)
                {
                    if (i >= BlogSettings.Instance.BlogrollVisiblePosts)
                    {
                        break;
                    }

                    var subLi = new HtmlGenericControl("li");
                    var a = new HtmlAnchor
                        {
                            HRef = item.ItemLinks[i], 
                            Title = HttpUtility.HtmlEncode(item.ItemTitles[i]), 
                            InnerHtml = EnsureLength(item.ItemTitles[i])
                        };

                    subLi.Controls.Add(a);
                    div.Controls.Add(subLi);
                }

                li.Controls.Add(div);
            }
        }

        /// <summary>
        /// Parses the processed RSS items and returns the HTML
        /// </summary>
        /// <returns>
        /// The output.
        /// </returns>
        private static HtmlGenericControl BindControls()
        {
            var ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "xoxo");
            foreach (var item in items)
            {
                var feedAnchor = new HtmlAnchor { HRef = item.RollItem.FeedUrl.AbsoluteUri };

                var image = new HtmlImage
                    {
                        Src = string.Format("{0}pics/rssButton.gif", Utils.RelativeWebRoot), 
                        Alt = string.Format("RSS feed for {0}", item.RollItem.Title)
                    };

                feedAnchor.Controls.Add(image);

                var webAnchor = new HtmlAnchor
                    {
                       HRef = item.RollItem.BlogUrl.AbsoluteUri, InnerHtml = EnsureLength(item.RollItem.Title) 
                    };

                if (!String.IsNullOrEmpty(item.RollItem.Xfn))
                {
                    webAnchor.Attributes["rel"] = item.RollItem.Xfn;
                }

                var li = new HtmlGenericControl("li");
                li.Controls.Add(feedAnchor);
                li.Controls.Add(webAnchor);

                AddRssChildItems(item, li);
                ul.Controls.Add(li);
            }

            return ul;
        }

        /// <summary>
        /// Displays the RSS item collection.
        /// </summary>
        /// <returns>
        /// The output.
        /// </returns>
        private static HtmlGenericControl DisplayBlogroll()
        {
            if (DateTime.Now > lastUpdated.AddMinutes(BlogSettings.Instance.BlogrollUpdateMinutes) &&
                BlogSettings.Instance.BlogrollVisiblePosts > 0)
            {
                items = null;
                lastUpdated = DateTime.Now;
            }

            if (items == null)
            {
                lock (SyncRoot)
                {
                    if (items == null)
                    {
                        items = new List<BlogRequest>();
                        foreach (var roll in BlogRollItem.BlogRolls)
                        {
                            AddBlog(roll);
                        }
                    }
                }
            }

            return BindControls();
        }

        #endregion
    }

    /// <summary>
    /// The blog request.
    /// </summary>
    internal class BlogRequest
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogRequest"/> class.
        /// </summary>
        /// <param name="rollItem">
        /// The roll item.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        internal BlogRequest(BlogRollItem rollItem, HttpWebRequest request)
        {
            this.ItemTitles = new List<string>();
            this.ItemLinks = new List<string>();
            this.RollItem = rollItem;
            this.Request = request;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the item links.
        /// </summary>
        internal List<string> ItemLinks { get; set; }

        /// <summary>
        ///     Gets or sets the item titles.
        /// </summary>
        internal List<string> ItemTitles { get; set; }

        /// <summary>
        ///     Gets or sets the request.
        /// </summary>
        internal HttpWebRequest Request { get; set; }

        /// <summary>
        ///     Gets or sets the roll item.
        /// </summary>
        internal BlogRollItem RollItem { get; set; }

        #endregion
    }
}