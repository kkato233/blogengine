using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BlogML;
using BlogML.Xml;

namespace BlogEngine.Core.API.BlogML
{
    /// <summary>
    /// Class to validate BlogML data and import it into Blog
    /// </summary>
    public class BlogReader : BaseReader
    {
        #region Constructors

        public BlogReader() : base() {}
        
        #endregion

        #region Locals & Properties

        string _xmlData = "";
        StringDictionary categoryLookup, authorLookup;
        Dictionary<string, StringCollection> _tags = new Dictionary<string, StringCollection>();

        /// <summary>
        /// Converts BlogML data saved as string into XML stream
        /// </summary>
        private XmlTextReader XmlReader
        {
            get
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(_xmlData);
                MemoryStream stream = new MemoryStream(byteArray);
                return new XmlTextReader(stream);
            }
        }

        /// <summary>
        /// BlogML data uploaded and saved as string
        /// </summary>
        public string XmlData
        {
            set { _xmlData = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Imports BlogML file into blog
        /// </summary>
        /// <returns>True if successful</returns>
        public override bool Import()
        {
            bool retVal = true;
            Message = "";

            BlogMLBlog blog = new BlogMLBlog();

            LoadTags();
            try
            {
                blog = BlogMLSerializer.Deserialize(XmlReader);
            }
            catch (Exception ex)
            {
                retVal = false;
                Message = "BlogReader.Import: BlogML could not load with 2.0 specs. " + ex.Message;
                Utils.Log(Message);
            }

            // Setup Web Service Connection
            BlogImporter blogService = new BlogImporter();

            // Load Categories in StringDictionary
            categoryLookup = new StringDictionary();
            foreach (BlogMLCategory cat in blog.Categories)
            {
                categoryLookup.Add(cat.ID, cat.Title);
            }

            // Load Authors in StringDictionary
            authorLookup = new StringDictionary();
            foreach (BlogMLAuthor author in blog.Authors)
            {
                authorLookup.Add(author.ID, author.Title);
            }

            // Load Posts & Pages (Categories are processed with Posts)
            foreach (BlogMLPost post in blog.Posts)
            {
                try
                {
                    if (post.PostType == BlogPostTypes.Normal)
                        LoadBlogPost(blogService, post);
                    else
                        LoadBlogPage(blogService, post);
                }
                catch (Exception ex)
                {
                    retVal = false;
                    Message = "BlogReader.Import: " + ex.Message;
                    Utils.Log(Message);
                }
            }

            Message = string.Format("Success; Loaded {0} new posts", blog.Posts.Count);

            // When done, force blog to reload posts.
            blogService.ForceReload();

            return retVal;
        }

        #endregion

        #region Private Methods

        private void LoadBlogPost(BlogImporter blog, BlogMLPost post)
        {
            BlogImporter.ImportPost import = new BlogImporter.ImportPost();
            import.Title = post.Title;
            if (_author == "")
                import.Author = authorLookup[post.Authors[0].Ref]; // Always use first author
            else
                import.Author = _author;

            import.PostDate = post.DateCreated;
            import.Description = post.Excerpt.UncodedText;
            import.Publish = true; //post.Approved;
            import.Content = post.Content.UncodedText;

            // Categories
            Collection<string> categories = new Collection<string>();
            if (post.Categories.Count > 0)
            {
                for (int i = 0; i < post.Categories.Count; i++)
                {
                    categories.Add(categoryLookup[post.Categories[i].Ref]);
                }
            }

            import.Categories = categories;

            // Tags
            StringCollection postTags = _tags[import.Title];
            if (postTags != null)
            {
                import.Tags = new Collection<string>();

                foreach (string s in postTags)
                {
                    import.Tags.Add(s);
                }
            }

            // Save Post
            string postID = blog.AddPost(import, post.PostUrl, _removeDups);

            // Save Comments
            foreach (BlogMLComment comment in post.Comments)
            {
                if (_approvedCommentsOnly)
                {
                    if (comment.Approved)
                        blog.AddComment(postID, comment.UserName, comment.UserEMail, comment.UserUrl, comment.Content.UncodedText, comment.DateCreated);
                }
                else
                    blog.AddComment(postID, comment.UserName, comment.UserEMail, comment.UserUrl, comment.Content.UncodedText, comment.DateCreated);
            }
        }

        private void LoadBlogPage(BlogImporter blog, BlogMLPost post)
        {
            //TODO: Load Pages?

            // Nothing to test with yet

        }

        /// <summary>
        /// BlogML does not support tags, but BlogEngine exporter does.
        /// Grab tags for each post directly from XML instead of using BlogML object
        /// </summary>
        private void LoadTags()
        {
            XmlDocument doc = new XmlDocument();
            StringCollection tags = new StringCollection();
            
            _tags.Clear();
            doc.Load(XmlReader);

            XmlNodeList posts = doc.GetElementsByTagName("post");

            foreach (XmlNode post in posts)
            {
                string title = "";
                tags.Clear();

                if (post.ChildNodes.Count > 0)
                {
                    foreach (XmlNode child in post.ChildNodes)
                    {
                        if (child.Name == "title")
                        {
                            title = child.InnerText;
                        }
                        if (child.Name == "tags")
                        {
                            foreach (XmlNode tag in child.ChildNodes)
                            {
                                tags.Add(tag.Attributes["ref"].Value);
                            }
                        }
                    }

                    if (tags.Count > 0)
                    {
                        _tags.Add(title, tags);
                    }
                }
            }
        }

        #endregion
    }
}