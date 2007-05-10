using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using BlogEngine.Core;

namespace BlogEngine.Core.Providers
{
    /// <summary>
    /// Microsoft SQL Server Implementation of BlogProvider
    /// </summary>
    public class MSSQLBlogProvider : BlogProvider
    {
        private SqlConnection providerConn;

        #region Posts
        /// <summary>
        /// Retrieves a post based on the specified Id.
        /// </summary>
        public override Post SelectPost(Guid id)
        {
            OpenConnection();

            Post post = new Post();
            string sqlQuery =   "SELECT PostID, Title, Description, PostContent, DateCreated, " +
                                "DateModified, Author, IsPublished, IsCommentEnabled " +
                                "FROM be_Posts " +
                                "WHERE PostID = @id";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            cmd.Parameters.Add(new SqlParameter("@id", id.ToString()));
            SqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();

            post.Id = rdr.GetGuid(0);
            post.Title = rdr.GetString(1);
            post.Content = rdr.GetString(3);
            if (!rdr.IsDBNull(2))
                post.Description = rdr.GetString(2);
            if (!rdr.IsDBNull(4))
                post.DateCreated = rdr.GetDateTime(4);
            if (!rdr.IsDBNull(5))
                post.DateModified = rdr.GetDateTime(5);
            if (!rdr.IsDBNull(6))
                post.Author = rdr.GetString(6);
            if (!rdr.IsDBNull(7))
                post.IsPublished = rdr.GetBoolean(7);
            if (!rdr.IsDBNull(8))
                post.IsCommentsEnabled = rdr.GetBoolean(8);

            rdr.Close();

            // Tags
            sqlQuery =  "SELECT Tag " +
                        "FROM be_PostTag " +
                        "WHERE PostID = @id";
            cmd.CommandText = sqlQuery;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                post.Tags.Add(rdr.GetString(0));
            }

            rdr.Close();

            // Categories
            sqlQuery = "SELECT CategoryID " +
                        "FROM be_PostCategory " +
                        "WHERE PostID = @id";
            cmd.CommandText = sqlQuery;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Guid key = rdr.GetGuid(0);
                if (CategoryDictionary.Instance.ContainsKey(key))
                    post.Categories.Add(key);
            }

            rdr.Close();

            // Comments
            sqlQuery =  "SELECT PostCommentID, CommentDate, Author, Email, Website, Comment, Country, Ip " +
                        "FROM be_PostComment " +
                        "WHERE PostID = @id";
            cmd.CommandText = sqlQuery;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Comment comment = new Comment();
                comment.Id = rdr.GetGuid(0);
                comment.Author = rdr.GetString(2);
                comment.Email = rdr.GetString(3);
                comment.Content = rdr.GetString(5);
                comment.DateCreated = rdr.GetDateTime(1);

                if (!rdr.IsDBNull(6))
                    comment.Country = rdr.GetString(6);
                if (!rdr.IsDBNull(7))
                    comment.IP = rdr.GetString(7);

                if (!rdr.IsDBNull(4))
                {
                    Uri website;
                    if (Uri.TryCreate(rdr.GetString(4), UriKind.Absolute, out website))
                        comment.Website = website;
                }
                
                post.Comments.Add(comment);
            }
            post.Comments.Sort();

            rdr.Close();

            return post;
        }

        /// <summary>
        /// Inserts a new Post to the data store.
        /// </summary>
        public override void InsertPost(Post post)
        {
            OpenConnection();

            string sqlQuery =   "INSERT INTO " +
                                "be_Posts (PostID)" +
                                "VALUES (@id)";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // Do nothing.  
            }

            UpdatePost(post);
        }

        /// <summary>
        /// Updates a Post.
        /// </summary>
        public override void UpdatePost(Post post)
        {
            OpenConnection();

            string sqlQuery =   "UPDATE be_Posts " +
                                "SET Title = @title, Description = @desc, PostContent = @content, " +
                                "DateCreated = @created, DateModified = @modified, Author = @Author, " +
                                "IsPublished = @published, IsCommentEnabled = @commentEnabled " +
                                "WHERE PostID = @id";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            cmd.Parameters.Add(new SqlParameter("@title", post.Title));
            cmd.Parameters.Add(new SqlParameter("@desc", post.Description));
            cmd.Parameters.Add(new SqlParameter("@content", post.Content));
            cmd.Parameters.Add(new SqlParameter("@created", new SqlDateTime(post.DateCreated)));
            cmd.Parameters.Add(new SqlParameter("@modified", new SqlDateTime(post.DateCreated)));
            cmd.Parameters.Add(new SqlParameter("@author", post.Author));
            cmd.Parameters.Add(new SqlParameter("@published", post.IsPublished));
            cmd.Parameters.Add(new SqlParameter("@commentEnabled", post.IsCommentsEnabled));
            cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));

            cmd.ExecuteNonQuery();

            // Tags
            cmd.CommandText = "DELETE FROM be_PostTag WHERE PostID = @id";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
            cmd.ExecuteNonQuery();

            foreach (string tag in post.Tags)
            {
                cmd.CommandText = "INSERT INTO be_PostTag (PostID, Tag) VALUES (@id, @tag)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
                cmd.Parameters.Add(new SqlParameter("@tag", tag));
                cmd.ExecuteNonQuery();
            }

            // Categories
            cmd.CommandText = "DELETE FROM be_PostCategory WHERE PostID = @id";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
            cmd.ExecuteNonQuery();

            foreach (Guid key in post.Categories)
            {
                if (CategoryDictionary.Instance.ContainsKey(key))
                {
                    cmd.CommandText = "INSERT INTO be_PostCategory (PostID, CategoryID) VALUES (@id, @cat)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@cat", key.ToString()));
                    cmd.ExecuteNonQuery();
                }
            }

            // Comments
            cmd.CommandText = "DELETE FROM be_PostComment WHERE PostID = @id";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
            cmd.ExecuteNonQuery();

            foreach (Comment comment in post.Comments)
            {
                sqlQuery =  "INSERT INTO be_PostComment (PostID, CommentDate, Author, Email, Website, Comment, Country, Ip) " +
                            "VALUES (@id, @date, @author, @email, @website, @comment, @country, @ip)";
                cmd.CommandText = sqlQuery;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
                cmd.Parameters.Add(new SqlParameter("@date", new SqlDateTime(comment.DateCreated)));
                cmd.Parameters.Add(new SqlParameter("@author", comment.Author));
                cmd.Parameters.Add(new SqlParameter("@email", comment.Email));
                if (comment.Website == null)
                    cmd.Parameters.Add(new SqlParameter("@website", ""));
                else
                    cmd.Parameters.Add(new SqlParameter("@website", comment.Website.ToString()));
                cmd.Parameters.Add(new SqlParameter("@comment", comment.Content));
                if (comment.Country == null)
                    cmd.Parameters.Add(new SqlParameter("@country", ""));
                else
                    cmd.Parameters.Add(new SqlParameter("@country", comment.Country));
                if (comment.IP == null)
                    cmd.Parameters.Add(new SqlParameter("@ip", ""));
                else
                    cmd.Parameters.Add(new SqlParameter("@ip", comment.IP));
                
                cmd.ExecuteNonQuery();
            }
            


        }

        /// <summary>
        /// Deletes a post from the data store.
        /// </summary>
        public override void DeletePost(Post post)
        {
            OpenConnection();

            string sqlQuery =   "DELETE FROM be_Posts WHERE PostID = @id;" +
                                "DELETE FROM be_PostTag WHERE PostID = @id;" +
                                "DELETE FROM be_PostCategory WHERE PostID = @id;" +
                                "DELETE FROM be_PostComment WHERE PostID = @id;";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
            
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves all posts from the data store
        /// </summary>
        /// <returns>List of Posts</returns>
        public override List<Post> FillPosts()
        {
            List<Post> posts = new List<Post>();

            OpenConnection();

            string sqlQuery =   "SELECT PostID FROM be_Posts ";
            SqlDataAdapter sa = new SqlDataAdapter(sqlQuery, providerConn);
            DataTable dtPosts = new DataTable();
            sa.Fill(dtPosts);

            foreach (DataRow dr in dtPosts.Rows)
            {
                posts.Add(Post.Load(new Guid(dr[0].ToString())));
            }

            posts.Sort();
            return posts;
        }
        #endregion

        #region Pages
        /// <summary>
        /// Retrieves a Page from the data store.
        /// </summary>
        public override Page SelectPage(Guid id)
        {
            OpenConnection();

            Page page = new Page();
            string sqlQuery =   "SELECT PageID, Title, Description, PageContent, DateCreated, " +
                                "DateModified, Keywords " +
                                "FROM be_Pages " +
                                "WHERE PageID = @id";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            cmd.Parameters.Add(new SqlParameter("@id", id.ToString()));
            SqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();

            page.Id = rdr.GetGuid(0);
            page.Title = rdr.GetString(1);
            page.Content = rdr.GetString(3);
            if (!rdr.IsDBNull(2))
                page.Description = rdr.GetString(2);
            if (!rdr.IsDBNull(4))
                page.DateCreated = rdr.GetDateTime(4);
            if (!rdr.IsDBNull(5))
                page.DateModified = rdr.GetDateTime(5);
            if (!rdr.IsDBNull(6))
                page.Keywords = rdr.GetString(6);

            rdr.Close();

            return page;
        }

        /// <summary>
        /// Inserts a new Page to the data store.
        /// </summary>
        public override void InsertPage(Page page)
        {
            OpenConnection();

            string sqlQuery =   "INSERT INTO be_Pages (PageID) VALUES (@id)";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            cmd.Parameters.Add(new SqlParameter("@id", page.Id.ToString()));

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // Do nothing, yet.
            }

            UpdatePage(page);
        }

        /// <summary>
        /// Updates a Page in the data store.
        /// </summary>
        public override void UpdatePage(Page page)
        {
            OpenConnection();

            string sqlQuery =   "UPDATE be_Pages " +
                                "SET Title = @title, Description = @desc, PageContent = @content, " +
                                "DateCreated = @created, DateModified = @modified, Keywords = @keywords " +
                                "WHERE PageID = @id";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            cmd.Parameters.Add(new SqlParameter("@title", page.Title));
            cmd.Parameters.Add(new SqlParameter("@desc", page.Description));
            cmd.Parameters.Add(new SqlParameter("@content", page.Content));
            cmd.Parameters.Add(new SqlParameter("@created", new SqlDateTime(page.DateCreated)));
            cmd.Parameters.Add(new SqlParameter("@modified", new SqlDateTime(page.DateCreated)));
            cmd.Parameters.Add(new SqlParameter("@keywords", page.Keywords));
            cmd.Parameters.Add(new SqlParameter("@id", page.Id.ToString()));

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes a Page from the data store.
        /// </summary>
        public override void DeletePage(Page page)
        {
            OpenConnection();

            string sqlQuery = "DELETE FROM be_Pages WHERE PageID = @id";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            cmd.Parameters.Add(new SqlParameter("@id", page.Id.ToString()));

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves all pages from the data store
        /// </summary>
        /// <returns>List of Pages</returns>
        public override List<Page> FillPages()
        {
            List<Page> pages = new List<Page>();

            OpenConnection();

            string sqlQuery = "SELECT PageID FROM be_Pages ";
            SqlDataAdapter sa = new SqlDataAdapter(sqlQuery, providerConn);
            DataTable dtPages = new DataTable();
            sa.Fill(dtPages);

            foreach (DataRow dr in dtPages.Rows)
            {
                pages.Add(Page.Load(new Guid(dr[0].ToString())));
            }

            return pages;
        }
        #endregion

        #region Categories
        /// <summary>
        /// Loads all categories from the data store.
        /// </summary>
        public override CategoryDictionary LoadCategories()
        {
            CategoryDictionary dic = new CategoryDictionary();

            OpenConnection();

            string sqlQuery =   "SELECT CategoryID, CategoryName " +
                                "FROM be_Categories ";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Guid id = rdr.GetGuid(0);
                string title = rdr.GetString(1);
                dic.Add(id, title);
            }

            rdr.Close();

            return dic;
        }

        /// <summary>
        /// Saves the categories to the data store.
        /// </summary>
        public override void SaveCategories(CategoryDictionary categories)
        {
            OpenConnection();

            string sqlQuery = "DELETE FROM be_Categories";
            SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
            cmd.ExecuteNonQuery();

            foreach (Guid key in categories.Keys)
            {
                sqlQuery = "INSERT INTO be_Categories (CategoryID, CategoryName) " +
                            "VALUES (@id, @name)";
                cmd.CommandText = sqlQuery;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@id", key.ToString()));
                cmd.Parameters.Add(new SqlParameter("@id", categories[key]));
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        /// <summary>
        /// Handles Opening the SQL Connection
        /// </summary>
        private void OpenConnection()
        {
            // Initial if needed
            if (providerConn == null)
                providerConn = new SqlConnection(BlogSettings.Instance.MSSQLConnectionString);
            // Open it if needed
            if (providerConn.State == System.Data.ConnectionState.Closed)
                providerConn.Open();
        }
    }
}
