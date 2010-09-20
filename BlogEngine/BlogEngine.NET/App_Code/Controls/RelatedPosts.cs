﻿namespace Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    using BlogEngine.Core;

    using Resources;

    /// <summary>
    /// The related posts.
    /// </summary>
    public class RelatedPosts : Control
    {
        #region Constants and Fields

        /// <summary>
        /// The cache.
        /// </summary>
        private static readonly Dictionary<Guid, string> Cache = new Dictionary<Guid, string>();

        /// <summary>
        /// The sync root.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// The description max length.
        /// </summary>
        private int descriptionMaxLength = 100;

        /// <summary>
        /// The headline.
        /// </summary>
        private string headline = labels.relatedPosts;

        /// <summary>
        /// The max results.
        /// </summary>
        private int maxResults = 3;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="RelatedPosts"/> class.
        /// </summary>
        static RelatedPosts()
        {
            Post.Saved += PostSaved;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets DescriptionMaxLength.
        /// </summary>
        public int DescriptionMaxLength
        {
            get
            {
                return this.descriptionMaxLength;
            }

            set
            {
                this.descriptionMaxLength = value;
            }
        }

        /// <summary>
        /// Gets or sets Headline.
        /// </summary>
        public string Headline
        {
            get
            {
                return this.headline;
            }

            set
            {
                this.headline = value;
            }
        }

        /// <summary>
        /// Gets or sets Item.
        /// </summary>
        public IPublishable Item { get; set; }

        /// <summary>
        /// Gets or sets MaxResults.
        /// </summary>
        public int MaxResults
        {
            get
            {
                return this.maxResults;
            }

            set
            {
                this.maxResults = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ShowDescription.
        /// </summary>
        public bool ShowDescription { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.
        /// </param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (!BlogSettings.Instance.EnableRelatedPosts || this.Item == null)
            {
                return;
            }

            if (!Cache.ContainsKey(this.Item.Id))
            {
                lock (SyncRoot)
                {
                    if (!Cache.ContainsKey(this.Item.Id))
                    {
                        var relatedPosts = this.SearchForPosts();
                        if (relatedPosts.Count <= 1)
                        {
                            return;
                        }

                        this.CreateList(relatedPosts);
                    }
                }
            }

            writer.Write(Cache[this.Item.Id].Replace("+++", this.Headline));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Saved event of the Post control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BlogEngine.Core.SavedEventArgs"/> instance containing the event data.</param>
        private static void PostSaved(object sender, SavedEventArgs e)
        {
            if (e.Action != SaveAction.Update)
            {
                return;
            }

            var post = (Post)sender;
            if (Cache.ContainsKey(post.Id))
            {
                Cache.Remove(post.Id);
            }
        }

        /// <summary>
        /// Creates the list of related posts in HTML.
        /// </summary>
        /// <param name="relatedPosts">
        /// The related posts.
        /// </param>
        private void CreateList(IEnumerable<IPublishable> relatedPosts)
        {
            var sb = new StringBuilder();

            const string LinkFormat = "<a href=\"{0}\">{1}</a>";
            const string DescriptionFormat = "<span>{0}</span>";
            sb.Append("<div id=\"relatedPosts\">");
            sb.Append("<p>+++</p>");
            sb.Append("<div>");

            var count = 0;
            foreach (var post in relatedPosts)
            {
                if (post != this.Item)
                {
                    sb.Append(string.Format(LinkFormat, post.RelativeLink, HttpUtility.HtmlEncode(post.Title)));
                    if (this.ShowDescription)
                    {
                        var description = post.Description;
                        if (description != null && description.Length > this.DescriptionMaxLength)
                        {
                            description = string.Format("{0}...", description.Substring(0, this.DescriptionMaxLength));
                        }

                        if (String.IsNullOrEmpty(description))
                        {
                            var content = Utils.StripHtml(post.Content);
                            description = content.Length > this.DescriptionMaxLength
                                              ? string.Format("{0}...", content.Substring(0, this.DescriptionMaxLength))
                                              : content;
                        }

                        sb.Append(string.Format(DescriptionFormat, description));
                    }

                    count++;
                }

                if (count == this.MaxResults)
                {
                    break;
                }
            }

            sb.Append("</div>");
            sb.Append("</div>");
            Cache.Add(this.Item.Id, sb.ToString());
        }

        /// <summary>
        /// The search for posts.
        /// </summary>
        /// <returns>
        /// A list of publishable interface.
        /// </returns>
        private List<IPublishable> SearchForPosts()
        {
            return Search.FindRelatedItems(this.Item);
        }

        #endregion
    }
}