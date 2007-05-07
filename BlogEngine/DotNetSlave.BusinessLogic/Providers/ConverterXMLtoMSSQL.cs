using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using BlogEngine.Core;
using System.Xml;
using System.IO;

namespace BlogEngine.Core.Providers
{
    public class ConverterXMLtoMSSQL : BlogProvider
    { 
        private static string _Folder = System.Web.HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation);
        private SqlConnection providerConn;

        public override Post  SelectPost(Guid id)
        {
            string fileName = _Folder + "posts\\" + id.ToString() + ".xml";
            Post post = new Post();
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            post.Title = doc.SelectSingleNode("post/title").InnerText;
            post.Description = doc.SelectSingleNode("post/description").InnerText;
            post.Content = doc.SelectSingleNode("post/content").InnerText;
            post.DateCreated = DateTime.Parse(doc.SelectSingleNode("post/pubDate").InnerText);
            post.DateModified = DateTime.Parse(doc.SelectSingleNode("post/lastModified").InnerText);

            if (doc.SelectSingleNode("post/author") != null)
                post.Author = doc.SelectSingleNode("post/author").InnerText;

            if (doc.SelectSingleNode("post/ispublished") != null)
                post.IsPublished = bool.Parse(doc.SelectSingleNode("post/ispublished").InnerText);

            if (doc.SelectSingleNode("post/iscommentsenabled") != null)
                post.IsCommentsEnabled = bool.Parse(doc.SelectSingleNode("post/iscommentsenabled").InnerText);

            // Tags
            foreach (XmlNode node in doc.SelectNodes("post/tags/tag"))
            {
                if (!string.IsNullOrEmpty(node.InnerText))
                    post.Tags.Add(node.InnerText);
            }

            // comments
            foreach (XmlNode node in doc.SelectNodes("post/comments/comment"))
            {
                Comment comment = new Comment();
                comment.Id = new Guid(node.Attributes["id"].InnerText);
                comment.Author = node.SelectSingleNode("author").InnerText;
                comment.Email = node.SelectSingleNode("email").InnerText;

                if (node.SelectSingleNode("country") != null)
                    comment.Country = node.SelectSingleNode("country").InnerText;

                if (node.SelectSingleNode("ip") != null)
                    comment.IP = node.SelectSingleNode("ip").InnerText;

                if (node.SelectSingleNode("website") != null)
                {
                    Uri website;
                    if (Uri.TryCreate(node.SelectSingleNode("website").InnerText, UriKind.Absolute, out website))
                        comment.Website = website;
                }
                comment.Content = node.SelectSingleNode("content").InnerText;
                comment.DateCreated = DateTime.Parse(node.SelectSingleNode("date").InnerText);
                post.Comments.Add(comment);
            }

            post.Comments.Sort();

            // categories
            foreach (XmlNode node in doc.SelectNodes("post/categories/category"))
            {
                Guid key = new Guid(node.InnerText);
                if (CategoryDictionary.Instance.ContainsKey(key))
                    post.Categories.Add(key);
            }

            return post;
        }

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

        public override void  DeletePost(Post post)
        {
 	        throw new Exception("The method or operation is not implemented.");
        }

        public override List<Post>  FillPosts()
        {
            // Load all Post from XML into posts
 	        string folder = CategoryDictionary._Folder + "posts\\";
            List<Post> posts = new List<Post>();

            foreach (string file in Directory.GetFiles(folder, "*.xml", SearchOption.TopDirectoryOnly))
            {
                FileInfo info = new FileInfo(file);
                string id = info.Name.Replace(".xml", string.Empty);
                //Post post = SelectPost(new Guid(id));
                Post post = Post.Load(new Guid(id));
                posts.Add(post);
            }

            // Save all post in SQL
            foreach (Post post in posts)
            {
                InsertPost(post);
            }

            return posts;
        }

        public override Page  SelectPage(Guid id)
        {
 	        throw new Exception("The method or operation is not implemented.");
        }

        public override void  InsertPage(Page page)
        {
 	        throw new Exception("The method or operation is not implemented.");
        }

        public override void  UpdatePage(Page page)
        {
 	        throw new Exception("The method or operation is not implemented.");
        }

        public override void  DeletePage(Page page)
        {
 	        throw new Exception("The method or operation is not implemented.");
        }

        public override List<Page>  FillPages()
        {
 	        throw new Exception("The method or operation is not implemented.");
        }

        public override CategoryDictionary  LoadCategories()
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

        public override void  SaveCategories(CategoryDictionary categories)
        {
 	        throw new Exception("The method or operation is not implemented.");
        }

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
