namespace BlogEngine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BlogEngine.Core.Providers;

    /// <summary>
    /// A page is much like a post, but is not part of the
    ///     blog chronology and is more static in nature.
    ///     <remarks>
    /// Pages can be used for "About" pages or other static
    ///         information.
    ///     </remarks>
    /// </summary>
    public sealed class Page : BusinessBase<Page, Guid>, IPublishable
    {
        #region Constants and Fields

        /// <summary>
        /// The _ sync root.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// The _ pages.
        /// </summary>
        private static List<Page> pages;

        /// <summary>
        /// The _ content.
        /// </summary>
        private string content;

        /// <summary>
        /// The _ description.
        /// </summary>
        private string description;

        /// <summary>
        /// The _ keywords.
        /// </summary>
        private string keywords;

        /// <summary>
        /// The _ parent.
        /// </summary>
        private Guid parent;

        /// <summary>
        /// The _ show in list.
        /// </summary>
        private bool showInList;

        /// <summary>
        /// The _ slug.
        /// </summary>
        private string slug;

        /// <summary>
        /// The _ title.
        /// </summary>
        private string title;

        /// <summary>
        /// The front page.
        /// </summary>
        private bool frontPage;

        /// <summary>
        /// The published.
        /// </summary>
        private bool published;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class. 
        ///     The contructor sets default values.
        /// </summary>
        public Page()
        {
            this.Id = Guid.NewGuid();
            this.DateCreated = DateTime.Now;
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the page is being served to the output stream.
        /// </summary>
        public static event EventHandler<ServingEventArgs> Serving;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets an unsorted list of all pages.
        /// </summary>
        public static List<Page> Pages
        {
            get
            {
                if (pages == null)
                {
                    lock (SyncRoot)
                    {
                        if (pages == null)
                        {
                            pages = BlogService.FillPages();
                            pages.Sort((p1, p2) => String.Compare(p1.Title, p2.Title));
                        }
                    }
                }

                return pages;
            }
        }

        /// <summary>
        ///     Gets the absolute link to the post.
        /// </summary>
        public Uri AbsoluteLink
        {
            get
            {
                return Utils.ConvertToAbsolute(this.RelativeLink);
            }
        }

        /// <summary>
        ///     Gets or sets the Description or the object.
        /// </summary>
        public string Content
        {
            get
            {
                return this.content;
            }

            set
            {
                if (this.content != value)
                {
                    this.MarkChanged("Content");
                }

                this.content = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Description or the object.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (this.description != value)
                {
                    this.MarkChanged("Description");
                }

                this.description = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not this page should be displayed on the front page.
        /// </summary>
        public bool FrontPage
        {
            get
            {
                return this.frontPage;
            }

            set
            {
                if (this.frontPage != value)
                {
                    this.MarkChanged("FrontPage");
                }

                this.frontPage = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the has child pages.
        /// </summary>
        /// Does this post have child pages
        public bool HasChildPages
        {
            get
            {
                return pages.Any(p => p.Parent == this.Id);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the has parent page.
        /// </summary>
        /// Does this post have a parent page?
        public bool HasParentPage
        {
            get
            {
                return this.Parent != Guid.Empty;
            }
        }

        /// <summary>
        ///     Gets or sets the Keywords or the object.
        /// </summary>
        public string Keywords
        {
            get
            {
                return this.keywords;
            }

            set
            {
                if (this.keywords != value)
                {
                    this.MarkChanged("Keywords");
                }

                this.keywords = value;
            }
        }

        /// <summary>
        ///     Gets or sets the parent of the Page. It is used to construct the 
        ///     hierachy of the pages.
        /// </summary>
        public Guid Parent
        {
            get
            {
                return this.parent;
            }

            set
            {
                if (this.parent != value)
                {
                    this.MarkChanged("Parent");
                }

                this.parent = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not this page should be published.
        /// </summary>
        public bool Published
        {
            get
            {
                return this.published;
            }

            set
            {
                if (this.published != value)
                {
                    this.MarkChanged("Published");
                }

                this.published = value;
            }
        }

        /// <summary>
        ///     Gets a relative-to-the-site-root path to the post.
        ///     Only for in-site use.
        /// </summary>
        public string RelativeLink
        {
            get
            {
                var theslug = Utils.RemoveIllegalCharacters(this.Slug) + BlogSettings.Instance.FileExtension;
                return string.Format("{0}page/{1}", Utils.RelativeWebRoot, theslug);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not this page should be in the sitemap list.
        /// </summary>
        public bool ShowInList
        {
            get
            {
                return this.showInList;
            }

            set
            {
                if (this.showInList != value)
                {
                    this.MarkChanged("ShowInList");
                }

                this.showInList = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Slug of the Page.
        ///     A Slug is the relative URL used by the pages.
        /// </summary>
        public string Slug
        {
            get
            {
                if (string.IsNullOrEmpty(this.slug))
                {
                    return Utils.RemoveIllegalCharacters(this.Title);
                }

                return this.slug;
            }

            set
            {
                if (this.slug != value)
                {
                    this.MarkChanged("Slug");
                }

                this.slug = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Title or the object.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title != value)
                {
                    this.MarkChanged("Title");
                }

                this.title = value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this page should be shown
        /// </summary>
        /// <value></value>
        public bool Visible
        {
            get
            {
                return this.Authenticated || this.Published;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this page is visible to visitors not logged into the blog.
        /// </summary>
        /// <value></value>
        public bool VisibleToPublic
        {
            get
            {
                return this.Published;
            }
        }

        /// <summary>
        /// Gets Author.
        /// </summary>
        string IPublishable.Author
        {
            get
            {
                return BlogSettings.Instance.AuthorName;
            }
        }

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the front page if any is available.
        /// </summary>
        /// <returns>The front Page.</returns>
        public static Page GetFrontPage()
        {
            // foreach (Page page in Pages)
            // {
            // if (page.FrontPage)
            // return page;
            // }
            return Pages.Find(page => page.FrontPage);

            // return null;
        }

        /// <summary>
        /// Returns a page based on the specified id.
        /// </summary>
        /// <param name="id">The page id.</param>
        /// <returns>The Page requested.</returns>
        public static Page GetPage(Guid id)
        {
            return Pages.FirstOrDefault(page => page.Id == id);
        }

        /// <summary>
        /// Called when [serving].
        /// </summary>
        /// <param name="page">The page being served.</param>
        /// <param name="arg">The <see cref="BlogEngine.Core.ServingEventArgs"/> instance containing the event data.</param>
        public static void OnServing(Page page, ServingEventArgs arg)
        {
            if (Serving != null)
            {
                Serving(page, arg);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return this.Title;
        }

        #endregion

        #region Implemented Interfaces

        #region IPublishable

        /// <summary>
        /// Raises the Serving event
        /// </summary>
        /// <param name="eventArgs">
        /// The event Args.
        /// </param>
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
        /// Deletes the page from the current BlogProvider.
        /// </summary>
        protected override void DataDelete()
        {
            BlogService.DeletePage(this);
            if (Pages.Contains(this))
            {
                Pages.Remove(this);
            }
        }

        /// <summary>
        /// Inserts a new page to current BlogProvider.
        /// </summary>
        protected override void DataInsert()
        {
            BlogService.InsertPage(this);

            if (this.New)
            {
                Pages.Add(this);
            }
        }

        /// <summary>
        /// Retrieves a page form the BlogProvider
        /// based on the specified id.
        /// </summary>
        /// <param name="id">The page id.</param>
        /// <returns>The Page requested.</returns>
        protected override Page DataSelect(Guid id)
        {
            return BlogService.SelectPage(id);
        }

        /// <summary>
        /// Updates the object in its data store.
        /// </summary>
        protected override void DataUpdate()
        {
            BlogService.UpdatePage(this);
        }

        /// <summary>
        /// Validates the properties on the Page.
        /// </summary>
        protected override void ValidationRules()
        {
            this.AddRule("Title", "Title must be set", string.IsNullOrEmpty(this.Title));
            this.AddRule("Content", "Content must be set", string.IsNullOrEmpty(this.Content));
        }

        #endregion
    }
}