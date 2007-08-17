using System;
using System.Collections.Generic;
using System.Text;
using BlogEngine.Core.Providers;

namespace BlogEngine.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class PostLists
    {

        private static object _SyncRoot = new object();
        private static List<Post> _Posts;

        /// <summary>
        /// Returns all posts written by the specified author.
        /// </summary>
        public static List<Post> GetPostsByAuthor(string author)
        {
            List<Post> list = new List<Post>();
            foreach (Post post in Post.Posts)
            {
                string legalTitle = Utils.RemoveIlegalCharacters(post.Author);
                if (Utils.RemoveIlegalCharacters(author).Equals(legalTitle, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(post);
                }
            }

            return list;
        }

        /// <summary>
        /// Returns all posts tagged with the specified tag.
        /// </summary>
        public static List<Post> GetPostsByTag(string tag)
        {
            List<Post> list = new List<Post>();
            foreach (Post post in Post.Posts)
            {
                if (post.Tags.Contains(tag))
                    list.Add(post);
            }

            return list;
        }

        /// <summary>
        /// Returns all posts published between the two dates.
        /// </summary>
        public static List<Post> GetPostsByDate(DateTime dateFrom, DateTime dateTo)
        {
            List<Post> list = new List<Post>();
            foreach (Post post in Post.Posts)
            {
                if (post.DateCreated.Date >= dateFrom && post.DateCreated.Date <= dateTo)
                    list.Add(post);
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<Post> Posts
        {
            get
            {
                if (_Posts == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Posts == null)
                            _Posts = BlogService.FillPosts();
                    }
                }

                return _Posts;
            }
        }

        /// <summary>
        /// Returns all posts in the specified category
        /// </summary>
        public static List<Post> GetPostsByCategory(Guid categoryId)
        {
            List<Post> col = new List<Post>();
            foreach (Post post in Posts)
            {
                if (post.Categories.Contains(Category.GetCategory(categoryId)))
                    col.Add(post);
            }

            col.Sort();
            return col;
        }


    }
}
