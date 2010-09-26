﻿namespace BlogEngine.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a comment to a blog post.
    /// </summary>
    [Serializable]
    public sealed class Comment : IComparable<Comment>, IPublishable
    {
        #region Constants and Fields

        /// <summary>
        /// The comments.
        /// </summary>
        private List<Comment> comments;

        /// <summary>
        /// The date created.
        /// </summary>
        private DateTime dateCreated = DateTime.MinValue;

        #endregion

        #region Events

        /// <summary>
        ///     Occurs after a comment is approved by the comment moderator.
        /// </summary>
        public static event EventHandler<EventArgs> Approved;

        /// <summary>
        ///     Occurs just before a comment is approved by the comment moderator.
        /// </summary>
        public static event EventHandler<CancelEventArgs> Approving;

        /// <summary>
        ///     Occurs when the post is being served to the output stream.
        /// </summary>
        public static event EventHandler<ServingEventArgs> Serving;

        /// <summary>
        ///     Occurs when the page is being attacked by robot spam.
        /// </summary>
        public static event EventHandler<EventArgs> SpamAttack;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the absolute link.
        /// </summary>
        /// <value>The absolute link.</value>
        public Uri AbsoluteLink
        {
            get
            {
                return new Uri(string.Format("{0}#id_{1}", this.Parent.AbsoluteLink, this.Id));
            }
        }

        /// <summary>
        ///     Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author { get; set; }

        /// <summary>
        ///     Gets or sets the Avatar of the comment.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        ///     Gets a list of Comments.
        /// </summary>
        public List<Comment> Comments
        {
            get
            {
                return this.comments ?? (this.comments = new List<Comment>());
            }
        }

        /// <summary>
        ///     Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        ///     Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country { get; set; }

        /// <summary>
        ///     Gets or sets when the comment was created.
        /// </summary>
        public DateTime DateCreated
        {
            get
            {
                return this.dateCreated == DateTime.MinValue ? this.dateCreated : this.dateCreated.AddHours(BlogSettings.Instance.Timezone);
            }

            set
            {
                this.dateCreated = value;
            }
        }

        /// <summary>
        ///     Gets the description. Returns always string.empty.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the IP address.
        /// </summary>
        /// <value>The IP address.</value>
        public string IP { get; set; }

        /// <summary>
        ///     Gets or sets the Id of the comment.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the Comment is approved.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        ///     Gets a value indicating whether or not this comment has been published
        /// </summary>
        public bool Published
        {
            get
            {
                return this.IsApproved;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this comment should be shown
        /// </summary>
        /// <value></value>
        public bool Visible
        {
            get
            {
                return this.IsApproved;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this comment is visible to visitors not logged into the blog.
        /// </summary>
        /// <value></value>
        public bool VisibleToPublic
        {
            get
            {
                return this.IsApproved;
            }
        }

        /// <summary>
        ///    Gets or sets process that approved or rejected comment
        /// </summary>
        [XmlElement]
        public string ModeratedBy { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public IPublishable Parent { get; set; }

        /// <summary>
        ///     Gets or sets the Id of the parent comment.
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        ///     Gets the relative link of the comment.
        /// </summary>
        /// <value>The relative link.</value>
        public string RelativeLink
        {
            get
            {
                return string.Format("{0}#id_{1}", this.Parent.RelativeLink, this.Id);
            }
        }

        /// <summary>
        ///     Gets abbreviated content
        /// </summary>
        public string Teaser
        {
            get
            {
                var ret = Utils.StripHtml(this.Content).Trim();
                return ret.Length > 120 ? string.Format("{0} ...", ret.Substring(0, 116)) : ret;
            }
        }

        /// <summary>
        ///     Gets the title of the object
        /// </summary>
        /// <value></value>
        public string Title
        {
            get
            {
                return string.Format("{0} on {1}", this.Author, this.Parent.Title);
            }
        }

        /// <summary>
        ///     Gets or sets the website.
        /// </summary>
        /// <value>The website.</value>
        public Uri Website { get; set; }

        /// <summary>
        /// Gets Categories.
        /// </summary>
        StateList<Category> IPublishable.Categories
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets DateModified.
        /// </summary>
        DateTime IPublishable.DateModified
        {
            get
            {
                return this.DateCreated;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Called when [serving].
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="arg">The <see cref="BlogEngine.Core.ServingEventArgs"/> instance containing the event data.</param>
        public static void OnServing(Comment comment, ServingEventArgs arg)
        {
            if (Serving != null)
            {
                Serving(comment, arg);
            }
        }

        /// <summary>
        /// Called when [spam attack].
        /// </summary>
        public static void OnSpamAttack()
        {
            if (SpamAttack != null)
            {
                SpamAttack(null, new EventArgs());
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable<Comment>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the 
        ///     objects being compared. The return value has the following meanings: 
        ///     Value Meaning Less than zero This object is less than the other parameter.
        ///     Zero This object is equal to other. Greater than zero This object is greater than other.
        /// </returns>
        public int CompareTo(Comment other)
        {
            return this.DateCreated.CompareTo(other.DateCreated);
        }

        #endregion

        #region IPublishable

        /// <summary>
        /// Raises the <see cref="Serving"/> event.
        /// </summary>
        /// <param name="eventArgs">The <see cref="BlogEngine.Core.ServingEventArgs"/> instance containing the event data.</param>
        public void OnServing(ServingEventArgs eventArgs)
        {
            if (Serving != null)
            {
                Serving(this, eventArgs);
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Called when [approved].
        /// </summary>
        /// <param name="comment">The comment.</param>
        internal static void OnApproved(Comment comment)
        {
            if (Approved != null)
            {
                Approved(comment, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [approving].
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        internal static void OnApproving(Comment comment, CancelEventArgs e)
        {
            if (Approving != null)
            {
                Approving(comment, e);
            }
        }

        #endregion
    }
}