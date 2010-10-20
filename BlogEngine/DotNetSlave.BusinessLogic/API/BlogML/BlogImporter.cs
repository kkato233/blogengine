namespace BlogEngine.Core.API.BlogML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Blog Importer
    /// </summary>
    public class BlogImporter
    {
        #region Public Methods

        /// <summary>
        /// Add Comment to specified post
        /// </summary>
        /// <param name="postId">
        /// postId as string
        /// </param>
        /// <param name="author">
        /// commenter username
        /// </param>
        /// <param name="email">
        /// commenter email
        /// </param>
        /// <param name="website">
        /// commenter url
        /// </param>
        /// <param name="description">
        /// actual comment
        /// </param>
        /// <param name="date">
        /// comment datetime
        /// </param>
        public void AddComment(
            string postId, string author, string email, string website, string description, DateTime date)
        {
            if (!IsAuthenticated())
            {
                throw new InvalidOperationException("BlogImporter.AddComment: Wrong credentials");
            }

            // Post post = Post.GetPost(new Guid(postID));
            var post = Post.Load(new Guid(postId));
            if (post == null)
            {
                return;
            }

            var comment = new Comment { Id = Guid.NewGuid(), Author = author, Email = email };
            Uri url;
            if (Uri.TryCreate(website, UriKind.Absolute, out url))
            {
                comment.Website = url;
            }

            comment.Content = description;
            comment.DateCreated = date;
            comment.Parent = post;
            comment.IsApproved = true;
            post.ImportComment(comment);
            post.Import();
        }

        /// <summary>
        /// Add new blog post to system
        /// </summary>
        /// <param name="import">
        /// ImportPost object
        /// </param>
        /// <param name="previousUrl">
        /// Old Post Url (for Url re-writing)
        /// </param>
        /// <param name="removeDuplicate">
        /// Search for duplicate post and remove?
        /// </param>
        /// <returns>
        /// string containing unique post identifier
        /// </returns>
        public string AddPost(ImportPost import, string previousUrl, bool removeDuplicate)
        {
            if (!IsAuthenticated())
            {
                throw new InvalidOperationException("BlogImporter.AddPost: Wrong credentials");
            }

            if (removeDuplicate)
            {
                if (!Post.IsTitleUnique(import.Title))
                {
                    // Search for matching post (by date and title) and delete it
                    foreach (var temp in
                        Post.GetPostsByDate(import.PostDate.AddDays(-2), import.PostDate.AddDays(2)).Where(
                            temp => temp.Title == import.Title))
                    {
                        temp.Delete();
                        temp.Import();
                    }
                }
            }

            using (
                var post = new Post
                    {
                        Title = import.Title,
                        Author = import.Author,
                        DateCreated = import.PostDate,
                        DateModified = import.PostDate,
                        Content = import.Content,
                        Description = import.Description,
                        IsPublished = import.Publish
                    })
            {
                AddCategories(import.Categories, post);

                // Tag Support:
                if (import.Tags.Count == 0 && import.Categories.Count > 0)
                {
                    // No tags. Use categories. 
                    post.Tags.AddRange(import.Categories);
                }
                else
                {
                    post.Tags.AddRange(import.Tags);
                }

                post.Import();

                return post.Id.ToString();
            }
        }

        /// <summary>
        /// Force Reload of all posts
        /// </summary>
        public void ForceReload()
        {
            if (!IsAuthenticated())
            {
                throw new InvalidOperationException("BlogImporter.ForeceReload: Wrong credentials");
            }

            Post.Reload();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add categories to specific post
        /// </summary>
        /// <param name="categories">
        /// Collection of categories
        /// </param>
        /// <param name="post">
        /// Post to add categories to.
        /// </param>
        private static void AddCategories(IEnumerable<string> categories, Post post)
        {
            try
            {
                foreach (var category in categories)
                {
                    var added = false;
                    var category1 = category;
                    foreach (var cat in
                        Category.Categories.Where(
                            cat => cat.Title.Equals(category1, StringComparison.OrdinalIgnoreCase)))
                    {
                        post.Categories.Add(cat);
                        added = true;
                    }

                    if (added)
                    {
                        continue;
                    }

                    using (var newCat = new Category(category, string.Empty))
                    {
                        newCat.Save();
                        post.Categories.Add(newCat);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("BlogImporter.AddCategories: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Determines whether this instance is authenticated.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsAuthenticated()
        {
            return HttpContext.Current.User.IsInRole(BlogSettings.Instance.AdministratorRole);
        }

        #endregion

        /// <summary>
        /// Object to hold imported post data
        /// </summary>
        public class ImportPost
        {
            #region Constants and Fields

            /// <summary>
            ///     Gets or sets the author.
            /// </summary>
            public string Author { get; set; }

            /// <summary>
            ///     Gets or sets the categories.
            /// </summary>
            public ICollection<string> Categories { get; set; }

            /// <summary>
            ///     Gets or sets the content.
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            ///     Gets or sets the description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            ///     Gets or sets the post date.
            /// </summary>
            public DateTime PostDate { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether the post is published.
            /// </summary>
            public bool Publish { get; set; }

            /// <summary>
            ///     Gets or sets the tags.
            /// </summary>
            public ICollection<string> Tags { get; set; }

            /// <summary>
            ///     Gets or sets the title.
            /// </summary>
            public string Title { get; set; }

            #endregion
        }
    }
}