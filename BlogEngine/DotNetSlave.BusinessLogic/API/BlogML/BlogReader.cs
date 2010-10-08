namespace BlogEngine.Core.API.BlogML
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.IO;
    using System.Text;
    using System.Xml;

    using global::BlogML;
    using global::BlogML.Xml;

    /// <summary>
    /// Class to validate BlogML data and import it into Blog
    /// </summary>
    public class BlogReader : BaseReader
    {
        #region Constants and Fields

        /// <summary>
        ///     The blog tags.
        /// </summary>
        private readonly Dictionary<string, StringCollection> tags = new Dictionary<string, StringCollection>();

        /// <summary>
        ///     The xml data.
        /// </summary>
        private string xmlData = string.Empty;

        /// <summary>
        ///     The author lookup.
        /// </summary>
        private StringDictionary authorLookup;

        /// <summary>
        ///     The category lookup.
        /// </summary>
        private StringDictionary categoryLookup;

        #endregion

        #region Properties

        /// <summary>
        ///     Sets BlogML data uploaded and saved as string
        /// </summary>
        public string XmlData
        {
            set
            {
                this.xmlData = value;
            }
        }

        /// <summary>
        ///     Gets an XmlReader that converts BlogML data saved as string into XML stream
        /// </summary>
        private XmlTextReader XmlReader
        {
            get
            {
                var byteArray = Encoding.UTF8.GetBytes(this.xmlData);
                var stream = new MemoryStream(byteArray);
                return new XmlTextReader(stream);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Imports BlogML file into blog
        /// </summary>
        /// <returns>
        /// True if successful
        /// </returns>
        public override bool Import()
        {
            var retVal = true;
            this.Message = string.Empty;

            var blog = new BlogMLBlog();

            this.LoadTags();
            try
            {
                blog = BlogMLSerializer.Deserialize(this.XmlReader);
            }
            catch (Exception ex)
            {
                retVal = false;
                this.Message = string.Format("BlogReader.Import: BlogML could not load with 2.0 specs. {0}", ex.Message);
                Utils.Log(this.Message);
            }

            // Setup Web Service Connection
            var blogService = new BlogImporter();

            // Load Categories in StringDictionary
            this.categoryLookup = new StringDictionary();
            foreach (var cat in blog.Categories)
            {
                this.categoryLookup.Add(cat.ID, cat.Title);
            }

            // Load Authors in StringDictionary
            this.authorLookup = new StringDictionary();
            foreach (var author in blog.Authors)
            {
                this.authorLookup.Add(author.ID, author.Title);
            }

            // Load Posts & Pages (Categories are processed with Posts)
            foreach (var post in blog.Posts)
            {
                try
                {
                    if (post.PostType == BlogPostTypes.Normal)
                    {
                        this.LoadBlogPost(blogService, post);
                    }
                    else
                    {
                        LoadBlogPage(blogService, post);
                    }
                }
                catch (Exception ex)
                {
                    retVal = false;
                    this.Message = string.Format("BlogReader.Import: {0}", ex.Message);
                    Utils.Log(this.Message);
                }
            }

            this.Message = string.Format("Success; Loaded {0} new posts", blog.Posts.Count);

            // When done, force blog to reload posts.
            blogService.ForceReload();

            return retVal;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the blog page.
        /// </summary>
        /// <param name="blog">
        /// The blog importer.
        /// </param>
        /// <param name="post">
        /// The BlogML post.
        /// </param>
        private static void LoadBlogPage(BlogImporter blog, BlogMLPost post)
        {
            // TODO: Load Pages?

            // Nothing to test with yet
        }

        /// <summary>
        /// Loads the blog post.
        /// </summary>
        /// <param name="blog">
        /// The blog importer.
        /// </param>
        /// <param name="post">
        /// The BlogML post.
        /// </param>
        private void LoadBlogPost(BlogImporter blog, BlogMLPost post)
        {
            var import = new BlogImporter.ImportPost
                {
                    Title = post.Title, 
                    Author = this.Author == string.Empty ? this.authorLookup[post.Authors[0].Ref] : this.Author, 
                    PostDate = post.DateCreated, 
                    Description = post.Excerpt.UncodedText, 
                    Publish = true, 
                    Content = post.Content.UncodedText
                };

            // Categories
            var categories = new Collection<string>();
            if (post.Categories.Count > 0)
            {
                for (var i = 0; i < post.Categories.Count; i++)
                {
                    categories.Add(this.categoryLookup[post.Categories[i].Ref]);
                }
            }

            import.Categories = categories;

            // Tags
            var postTags = this.tags[import.Title];
            if (postTags != null)
            {
                import.Tags = new Collection<string>();

                foreach (var s in postTags)
                {
                    import.Tags.Add(s);
                }
            }

            // Save Post
            var postId = blog.AddPost(import, post.PostUrl, this.RemoveDuplicates);

            // Save Comments
            foreach (BlogMLComment comment in post.Comments)
            {
                if (this.ApprovedCommentsOnly)
                {
                    if (comment.Approved)
                    {
                        blog.AddComment(
                            postId, 
                            comment.UserName, 
                            comment.UserEMail, 
                            comment.UserUrl, 
                            comment.Content.UncodedText, 
                            comment.DateCreated);
                    }
                }
                else
                {
                    blog.AddComment(
                        postId, 
                        comment.UserName, 
                        comment.UserEMail, 
                        comment.UserUrl, 
                        comment.Content.UncodedText, 
                        comment.DateCreated);
                }
            }
        }

        /// <summary>
        /// BlogML does not support tags, but BlogEngine exporter does.
        ///     Grab tags for each post directly from XML instead of using BlogML object
        /// </summary>
        private void LoadTags()
        {
            var doc = new XmlDocument();
            var tagCollection = new StringCollection();

            this.tags.Clear();
            doc.Load(this.XmlReader);

            var posts = doc.GetElementsByTagName("post");

            foreach (XmlNode post in posts)
            {
                var title = string.Empty;
                tagCollection.Clear();

                if (post.ChildNodes.Count <= 0)
                {
                    continue;
                }

                foreach (XmlNode child in post.ChildNodes)
                {
                    if (child.Name == "title")
                    {
                        title = child.InnerText;
                    }

                    if (child.Name != "tags")
                    {
                        continue;
                    }

                    foreach (XmlNode tag in child.ChildNodes)
                    {
                        if (tag.Attributes != null)
                        {
                            tagCollection.Add(tag.Attributes["ref"].Value);
                        }
                    }
                }

                if (tagCollection.Count > 0)
                {
                    this.tags.Add(title, tagCollection);
                }
            }
        }

        #endregion
    }
}