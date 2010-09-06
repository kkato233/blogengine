using System;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.ObjectModel;
using System.Web.Security;
using System.Text.RegularExpressions;
using BlogEngine.Core;

namespace BlogEngine.Core.API.BlogML
{
    /// <summary>
    /// Blog Importer
    /// </summary>
    public class BlogImporter
    {
        private bool IsAuthenticated()
        {
            return HttpContext.Current.User.IsInRole(BlogSettings.Instance.AdministratorRole);
        }

        /// <summary>
        /// Add new blog post to system
        /// </summary>
        /// <param name="import">ImportPost object</param>
        /// <param name="previousUrl">Old Post Url (for Url re-writing)</param>
        /// <param name="removeDuplicate">Search for duplicate post and remove?</param>
        /// <returns>string containing unique post identifier</returns>
        public string AddPost(ImportPost import, string previousUrl, bool removeDuplicate)
        {
            if (!IsAuthenticated())
                throw new InvalidOperationException("BlogImporter.AddPost: Wrong credentials");

            if (removeDuplicate)
            {
                if (!Post.IsTitleUnique(import.Title))
                {
                    // Search for matching post (by date and title) and delete it
                    foreach (Post temp in Post.GetPostsByDate(import.PostDate.AddDays(-2), import.PostDate.AddDays(2)))
                    {
                        if (temp.Title == import.Title)
                        {
                            temp.Delete();
                            temp.Import();
                        }
                    }
                }
            }

            Post post = new Post();
            post.Title = import.Title;
            post.Author = import.Author;
            post.DateCreated = import.PostDate;
            post.DateModified = import.PostDate;
            post.Content = import.Content;
            post.Description = import.Description;
            post.Published = import.Publish;

            AddCategories(import.Categories, post);

            //Tag Support:
            if (import.Tags.Count == 0 && import.Categories.Count > 0)
            {
                //No tags. Use categories. 
                post.Tags.AddRange(import.Categories);
            }
            else
            {
                post.Tags.AddRange(import.Tags);
            }

            post.Import();

            return post.Id.ToString();
        }

        /// <summary>
        /// Add Comment to specified post
        /// </summary>
        /// <param name="postID">postId as string</param>
        /// <param name="author">commenter username</param>
        /// <param name="email">commenter email</param>
        /// <param name="website">commenter url</param>
        /// <param name="description">actual comment</param>
        /// <param name="date">comment datetime</param>
        public void AddComment(string postID, string author, string email, string website, string description, DateTime date)
        {
            if (!IsAuthenticated())
                throw new InvalidOperationException("BlogImporter.AddComment: Wrong credentials");

            //Post post = Post.GetPost(new Guid(postID));
            Post post = Post.Load(new Guid(postID));
            if (post != null)
            {
                Comment comment = new Comment();
                comment.Id = Guid.NewGuid();
                comment.Author = author;
                comment.Email = email;
                Uri url;
                if (Uri.TryCreate(website, UriKind.Absolute, out url))
                    comment.Website = url;

                comment.Content = description;
                comment.DateCreated = date;
                comment.Parent = post;
                comment.IsApproved = true;
                post.ImportComment(comment);
                post.Import();
            }
        }

        /// <summary>
        /// Add categories to specific post
        /// </summary>
        /// <param name="categories">Collection of categories</param>
        /// <param name="post">Post</param>
        private static void AddCategories(Collection<string> categories, Post post)
        {
            try
            {
                foreach (string category in categories)
                {
                    bool added = false;
                    foreach (Category cat in Category.Categories)
                    {
                        if (cat.Title.Equals(category, StringComparison.OrdinalIgnoreCase))
                        {
                            post.Categories.Add(cat);
                            added = true;
                        }
                    }
                    if (!added)
                    {
                        Category newCat = new Category(category, string.Empty);
                        newCat.Save();
                        post.Categories.Add(newCat);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.Log("BlogImporter.AddCategories: " + ex.Message);
            }
        }

        /// <summary>
        /// Force Reload of all posts
        /// </summary>
        public void ForceReload()
        {
            if (!IsAuthenticated())
                throw new InvalidOperationException("BlogImporter.ForeceReload: Wrong credentials");

            Post.Reload();
        }

        /// <summary>
        /// Object to hold imported post data
        /// </summary>
        public class ImportPost
        {
            public string Title;
            public string Author;
            public DateTime PostDate;
            public string Content;
            public string Description;
            public Collection<string> Categories;
            public Collection<string> Tags;
            public bool Publish;
        }
    }
}