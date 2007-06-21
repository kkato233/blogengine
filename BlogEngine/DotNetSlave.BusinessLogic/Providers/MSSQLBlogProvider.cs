using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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
      bool connClose = OpenConnection();

      Post post = new Post();
      string sqlQuery = "SELECT PostID, Title, Description, PostContent, DateCreated, " +
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
      sqlQuery = "SELECT Tag " +
                  "FROM be_PostTag " +
                  "WHERE PostID = @id";
      cmd.CommandText = sqlQuery;
      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        if (!rdr.IsDBNull(0))
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
      sqlQuery = "SELECT PostCommentID, CommentDate, Author, Email, Website, Comment, Country, Ip " +
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

      if (connClose)
        providerConn.Close();

      return post;
    }

    /// <summary>
    /// Inserts a new Post to the data store.
    /// </summary>
    public override void InsertPost(Post post)
    {
      OpenConnection();

      string sqlQuery = "INSERT INTO " +
                          "be_Posts (PostID, Title, Description, PostContent, DateCreated, " +
                          "DateModified, Author, IsPublished, IsCommentEnabled, Raters, Rating)" +
                          "VALUES (@id, @title, @desc, @content, @created, @modified, " +
                          "@author, @published, @commentEnabled, @raters, @rating)";
      SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
      cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
      cmd.Parameters.Add(new SqlParameter("@title", post.Title));
      if (post.Description == null)
        cmd.Parameters.Add(new SqlParameter("@desc", ""));
      else
        cmd.Parameters.Add(new SqlParameter("@desc", post.Description));
      cmd.Parameters.Add(new SqlParameter("@content", post.Content));
      cmd.Parameters.Add(new SqlParameter("@created", new SqlDateTime(post.DateCreated)));
      if (post.DateModified == new DateTime())
        cmd.Parameters.Add(new SqlParameter("@modified", new SqlDateTime()));
      else
        cmd.Parameters.Add(new SqlParameter("@modified", new SqlDateTime(post.DateModified)));
      if (post.Author == null)
        cmd.Parameters.Add(new SqlParameter("@author", ""));
      else
        cmd.Parameters.Add(new SqlParameter("@author", post.Author));
      cmd.Parameters.Add(new SqlParameter("@published", post.IsPublished));
      cmd.Parameters.Add(new SqlParameter("@commentEnabled", post.IsCommentsEnabled));
      cmd.Parameters.Add(new SqlParameter("@raters", post.Raters.ToString()));
      cmd.Parameters.Add(new SqlParameter("@rating", post.Rating.ToString(System.Globalization.CultureInfo.InvariantCulture)));
      
      cmd.ExecuteNonQuery();

      // Tags
      UpdateTags(post);

      // Categories
      UpdateCategories(post);

      // Comments
      UpdateComments(post);

      providerConn.Close();
    }

    /// <summary>
    /// Updates a Post.
    /// </summary>
    public override void UpdatePost(Post post)
    {
      OpenConnection();

      string sqlQuery = "UPDATE be_Posts " +
                          "SET Title = @title, Description = @desc, PostContent = @content, " +
                          "DateCreated = @created, DateModified = @modified, Author = @Author, " +
                          "IsPublished = @published, IsCommentEnabled = @commentEnabled, " +
                          "Raters = @raters, Rating = @rating " +
                          "WHERE PostID = @id";
      SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
      cmd.Parameters.Add(new SqlParameter("@title", post.Title));
      if (post.Description == null)
        cmd.Parameters.Add(new SqlParameter("@desc", ""));
      else
        cmd.Parameters.Add(new SqlParameter("@desc", post.Description));
      cmd.Parameters.Add(new SqlParameter("@content", post.Content));
      cmd.Parameters.Add(new SqlParameter("@created", new SqlDateTime(post.DateCreated)));
      if (post.DateModified == new DateTime())
        cmd.Parameters.Add(new SqlParameter("@modified", new SqlDateTime()));
      else
        cmd.Parameters.Add(new SqlParameter("@modified", new SqlDateTime(post.DateModified)));
      if (post.Author == null)
        cmd.Parameters.Add(new SqlParameter("@author", ""));
      else
        cmd.Parameters.Add(new SqlParameter("@author", post.Author));
      cmd.Parameters.Add(new SqlParameter("@published", post.IsPublished));
      cmd.Parameters.Add(new SqlParameter("@commentEnabled", post.IsCommentsEnabled));
      cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
      cmd.Parameters.Add(new SqlParameter("@raters", post.Raters.ToString()));
      cmd.Parameters.Add(new SqlParameter("@rating", post.Rating.ToString(System.Globalization.CultureInfo.InvariantCulture)));

      cmd.ExecuteNonQuery();

      // Tags
      UpdateTags(post);

      // Categories
      UpdateCategories(post);

      // Comments
      UpdateComments(post);

      providerConn.Close();

    }

    /// <summary>
    /// Deletes a post from the data store.
    /// </summary>
    public override void DeletePost(Post post)
    {
      OpenConnection();

      string sqlQuery = "DELETE FROM be_Posts WHERE PostID = @id;" +
                          "DELETE FROM be_PostTag WHERE PostID = @id;" +
                          "DELETE FROM be_PostCategory WHERE PostID = @id;" +
                          "DELETE FROM be_PostComment WHERE PostID = @id;";
      SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
      cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));

      cmd.ExecuteNonQuery();

      providerConn.Close();
    }

    /// <summary>
    /// Retrieves all posts from the data store
    /// </summary>
    /// <returns>List of Posts</returns>
    public override List<Post> FillPosts()
    {
      List<Post> posts = new List<Post>();

      OpenConnection();

      string sqlQuery = "SELECT PostID FROM be_Posts ";
      SqlDataAdapter sa = new SqlDataAdapter(sqlQuery, providerConn);
      DataTable dtPosts = new DataTable();
      sa.Fill(dtPosts);

      foreach (DataRow dr in dtPosts.Rows)
      {
        posts.Add(Post.Load(new Guid(dr[0].ToString())));
      }

      providerConn.Close();

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
      bool connClose = OpenConnection();

      Page page = new Page();
      string sqlQuery = "SELECT PageID, Title, Description, PageContent, DateCreated, " +
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

      if (connClose)
        providerConn.Close();

      return page;
    }

    /// <summary>
    /// Inserts a new Page to the data store.
    /// </summary>
    public override void InsertPage(Page page)
    {
      OpenConnection();

      string sqlQuery = "INSERT INTO be_Pages (PageID, Title, Description, PageContent, " +
                          "DateCreated, DateModified, Keywords) " +
                          "VALUES (@id, @title, @desc, @content, @created, @modified, @keywords)";
      SqlCommand cmd = new SqlCommand(sqlQuery, providerConn);
      cmd.Parameters.Add(new SqlParameter("@id", page.Id.ToString()));
      cmd.Parameters.Add(new SqlParameter("@title", page.Title));
      cmd.Parameters.Add(new SqlParameter("@desc", page.Description));
      cmd.Parameters.Add(new SqlParameter("@content", page.Content));
      cmd.Parameters.Add(new SqlParameter("@created", new SqlDateTime(page.DateCreated)));
      cmd.Parameters.Add(new SqlParameter("@modified", new SqlDateTime(page.DateCreated)));
      cmd.Parameters.Add(new SqlParameter("@keywords", page.Keywords));
      cmd.ExecuteNonQuery();
      providerConn.Close();
    }

    /// <summary>
    /// Updates a Page in the data store.
    /// </summary>
    public override void UpdatePage(Page page)
    {
      OpenConnection();

      string sqlQuery = "UPDATE be_Pages " +
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
      providerConn.Close();
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
      providerConn.Close();
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

      providerConn.Close();

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

      SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BlogEngine"].ConnectionString);

      string sqlQuery = "SELECT CategoryID, CategoryName FROM be_Categories ";
      SqlDataAdapter sa = new SqlDataAdapter(sqlQuery, conn);
      DataTable dtCategories = new DataTable();
      sa.Fill(dtCategories);

      foreach (DataRow dr in dtCategories.Rows)
      {
        Guid id = new Guid(dr[0].ToString());
        string title = dr[1].ToString();
        dic.Add(id, title);
      }

      conn.Close();
      return dic;
    }

    /// <summary>
    /// Saves the categories to the data store.
    /// </summary>
    public override void SaveCategories(CategoryDictionary categories)
    {
      SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BlogEngine"].ConnectionString);

      string sqlQuery = "DELETE FROM be_Categories";
      SqlCommand cmd = new SqlCommand(sqlQuery, conn);
      conn.Open();
      cmd.ExecuteNonQuery();

      foreach (Guid key in categories.Keys)
      {
        sqlQuery = "INSERT INTO be_Categories (CategoryID, CategoryName) " +
                    "VALUES (@id, @name)";
        cmd.CommandText = sqlQuery;
        cmd.Parameters.Clear();
        cmd.Parameters.Add(new SqlParameter("@id", key.ToString()));
        cmd.Parameters.Add(new SqlParameter("@name", categories[key]));
        cmd.ExecuteNonQuery();
      }

      conn.Close();

    }
    #endregion

    #region Settings

    public override StringDictionary LoadSettings()
    {
      StringDictionary dic = new StringDictionary();
      SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BlogEngine"].ConnectionString);

      string sqlQuery = "SELECT SettingName, SettingValue FROM be_Settings";
      SqlCommand cmd = new SqlCommand(sqlQuery, conn);
      conn.Open();
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        string name = rdr.GetString(0);
        string value = rdr.GetString(1);

        dic.Add(name, value);
      }

      rdr.Close();
      conn.Close();

      return dic;
    }

    public override void SaveSettings(StringDictionary settings)
    {
      SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BlogEngine"].ConnectionString);

      string sqlQuery = "TRUNCATE TABLE be_Settings";
      SqlCommand cmd = new SqlCommand(sqlQuery, conn);
      conn.Open();
      cmd.ExecuteNonQuery();

      foreach (string key in settings.Keys)
      {
        sqlQuery = "INSERT INTO be_Settings (SettingName, SettingValue) " +
                    "VALUES (@name, @value)";
        cmd.CommandText = sqlQuery;
        cmd.Parameters.Clear();
        cmd.Parameters.Add(new SqlParameter("@name", key));
        cmd.Parameters.Add(new SqlParameter("@value", settings[key]));
        cmd.ExecuteNonQuery();
      }

      conn.Close();

    }

    #endregion

    #region Ping services

    /// <summary>
    /// Loads the ping services.
    /// </summary>
    /// <returns></returns>
    public override StringCollection LoadPingServices()
    {
      StringCollection col = new StringCollection();
      SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BlogEngine"].ConnectionString);

      string sqlQuery = "SELECT Link FROM be_PingService";
      SqlCommand cmd = new SqlCommand(sqlQuery, conn);
      conn.Open();
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        col.Add(rdr.GetString(0));
      }

      rdr.Close();
      conn.Close();

      return col;

    }

    /// <summary>
    /// Saves the ping services.
    /// </summary>
    /// <param name="services">The services.</param>
    public override void SavePingServices(StringCollection services)
    {
      SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BlogEngine"].ConnectionString);

      string sqlQuery = "TRUNCATE TABLE be_PingService";
      SqlCommand cmd = new SqlCommand(sqlQuery, conn);
      conn.Open();
      cmd.ExecuteNonQuery();

      foreach (string service in services)
      {
        sqlQuery = "INSERT INTO be_PingService (Link) " +
                    "VALUES (@link)";
        cmd.CommandText = sqlQuery;
        cmd.Parameters.Clear();
        cmd.Parameters.Add(new SqlParameter("@link", service));
        cmd.ExecuteNonQuery();
      }

      conn.Close();

    }

    #endregion

    /// <summary>
    /// Handles Opening the SQL Connection
    /// </summary>
    private bool OpenConnection()
    {
      bool result = false;

      // Initial if needed
      if (providerConn == null)
        providerConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BlogEngine"].ConnectionString);
      // Open it if needed
      if (providerConn.State == System.Data.ConnectionState.Closed)
      {
        result = true;
        providerConn.Open();
      }

      return result;
    }

    private void UpdateTags(Post post)
    {
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = providerConn;
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
    }

    private void UpdateCategories(Post post)
    {
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = providerConn;
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
    }

    private void UpdateComments(Post post)
    {
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = providerConn;
      cmd.CommandText = "DELETE FROM be_PostComment WHERE PostID = @id";
      cmd.Parameters.Clear();
      cmd.Parameters.Add(new SqlParameter("@id", post.Id.ToString()));
      cmd.ExecuteNonQuery();

      foreach (Comment comment in post.Comments)
      {
        cmd.CommandText = "INSERT INTO be_PostComment (PostCommentID, PostID, CommentDate, Author, Email, Website, Comment, Country, Ip) " +
                            "VALUES (@postcommentid, @id, @date, @author, @email, @website, @comment, @country, @ip)";
        cmd.Parameters.Clear();
        cmd.Parameters.Add(new SqlParameter("@postcommentid", comment.Id.ToString()));
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

  }
}
