#region Using

using System;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Providers
{
  /// <summary>
  /// A storage provider for BlogEngine that uses XML files.
  /// <remarks>
  /// To build another provider, you can just copy and modify
  /// this one. Then add it to the web.config's BlogEngine section.
  /// </remarks>
  /// </summary>
  public partial class XmlBlogProvider : BlogProvider
  {
    private static string _Folder = System.Web.HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation);

    #region Posts

    /// <summary>
    /// Retrieves a post based on the specified Id.
    /// </summary>
    public override Post SelectPost(Guid id)
    {
      string fileName = _Folder + "posts\\" + id.ToString() + ".xml";
      Post post = new Post();
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      post.Title = doc.SelectSingleNode("post/title").InnerText;
      post.Description = doc.SelectSingleNode("post/description").InnerText;
      post.Content = doc.SelectSingleNode("post/content").InnerText;
      post.DateCreated = DateTime.Parse(doc.SelectSingleNode("post/pubDate").InnerText, CultureInfo.InvariantCulture);
      post.DateModified = DateTime.Parse(doc.SelectSingleNode("post/lastModified").InnerText, CultureInfo.InvariantCulture);

      if (doc.SelectSingleNode("post/author") != null)
        post.Author = doc.SelectSingleNode("post/author").InnerText;

      if (doc.SelectSingleNode("post/ispublished") != null)
        post.IsPublished = bool.Parse(doc.SelectSingleNode("post/ispublished").InnerText);

      if (doc.SelectSingleNode("post/iscommentsenabled") != null)
        post.IsCommentsEnabled = bool.Parse(doc.SelectSingleNode("post/iscommentsenabled").InnerText);

      if (doc.SelectSingleNode("post/raters") != null)
        post.Raters = int.Parse(doc.SelectSingleNode("post/raters").InnerText);

      if (doc.SelectSingleNode("post/rating") != null)
        post.Rating = float.Parse(doc.SelectSingleNode("post/rating").InnerText, System.Globalization.CultureInfo.GetCultureInfo("en-gb"));

      if (doc.SelectSingleNode("post/slug") != null)
        post.Slug = doc.SelectSingleNode("post/slug").InnerText;

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
        comment.Post = post;

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
        comment.DateCreated = DateTime.Parse(node.SelectSingleNode("date").InnerText, CultureInfo.InvariantCulture);
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

    /// <summary>
    /// Inserts a new Post to the data store.
    /// </summary>
    /// <param name="post"></param>
    public override void InsertPost(Post post)
    {
      string fileName = _Folder + "posts\\" + post.Id.ToString() + ".xml";
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;

      using (XmlWriter writer = XmlWriter.Create(fileName, settings))
      {
        writer.WriteStartDocument(true);
        writer.WriteStartElement("post");

        writer.WriteElementString("author", post.Author);
        writer.WriteElementString("title", post.Title);
        writer.WriteElementString("description", post.Description);
        writer.WriteElementString("content", post.Content);
        writer.WriteElementString("ispublished", post.IsPublished.ToString());
        writer.WriteElementString("iscommentsenabled", post.IsCommentsEnabled.ToString());
        writer.WriteElementString("pubDate", post.DateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        writer.WriteElementString("lastModified", post.DateModified.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        writer.WriteElementString("raters", post.Raters.ToString());
        writer.WriteElementString("rating", post.Rating.ToString(CultureInfo.InvariantCulture));
        writer.WriteElementString("slug", post.Slug);

        // Tags
        writer.WriteStartElement("tags");
        foreach (string tag in post.Tags)
        {
          writer.WriteElementString("tag", tag);
        }
        writer.WriteEndElement();

        // comments
        writer.WriteStartElement("comments");
        foreach (Comment comment in post.Comments)
        {
          writer.WriteStartElement("comment");
          writer.WriteAttributeString("id", comment.Id.ToString());
          writer.WriteElementString("date", comment.DateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
          writer.WriteElementString("author", comment.Author);
          writer.WriteElementString("email", comment.Email);
          writer.WriteElementString("country", comment.Country);
          writer.WriteElementString("ip", comment.IP);
          if (comment.Website != null)
            writer.WriteElementString("website", comment.Website.ToString());
          writer.WriteElementString("content", comment.Content);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();

        // categories
        writer.WriteStartElement("categories");
        foreach (Guid key in post.Categories)
        {
          if (CategoryDictionary.Instance.ContainsKey(key))
            writer.WriteElementString("category", key.ToString());
        }
        writer.WriteEndElement();


        writer.WriteEndElement();
      }
    }

    /// <summary>
    /// Updates a Post.
    /// </summary>
    public override void UpdatePost(Post post)
    {
      InsertPost(post);
    }

    /// <summary>
    /// Deletes a post from the data store.
    /// </summary>
    public override void DeletePost(Post post)
    {
      string fileName = _Folder + "posts\\" + post.Id.ToString() + ".xml";
      if (File.Exists(fileName))
        File.Delete(fileName);
    }

    /// <summary>
    /// Retrieves all posts from the data store
    /// </summary>
    /// <returns>List of Posts</returns>
    public override List<Post> FillPosts()
    {
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
      string fileName = _Folder + "pages\\" + id.ToString() + ".xml";
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      Page page = new Page();

      page.Title = doc.SelectSingleNode("page/title").InnerText;
      page.Description = doc.SelectSingleNode("page/description").InnerText;
      page.Content = doc.SelectSingleNode("page/content").InnerText;
      page.Keywords = doc.SelectSingleNode("page/keywords").InnerText;
      page.DateCreated = DateTime.Parse(doc.SelectSingleNode("page/datecreated").InnerText, CultureInfo.InvariantCulture);
      page.DateModified = DateTime.Parse(doc.SelectSingleNode("page/datemodified").InnerText, CultureInfo.InvariantCulture);

      return page;
    }

    /// <summary>
    /// Inserts a new Page to the data store.
    /// </summary>
    public override void InsertPage(Page page)
    {
      string fileName = _Folder + "pages\\" + page.Id.ToString() + ".xml";
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;

      using (XmlWriter writer = XmlWriter.Create(fileName, settings))
      {
        writer.WriteStartDocument(true);
        writer.WriteStartElement("page");

        writer.WriteElementString("title", page.Title);
        writer.WriteElementString("description", page.Description);
        writer.WriteElementString("content", page.Content);
        writer.WriteElementString("keywords", page.Keywords);
        writer.WriteElementString("datecreated", page.DateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        writer.WriteElementString("datemodified", page.DateModified.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

        writer.WriteEndElement();
      }
    }

    /// <summary>
    /// Updates a Page.
    /// </summary>
    public override void UpdatePage(Page page)
    {
      InsertPage(page);
    }

    /// <summary>
    /// Deletes a page from the data store.
    /// </summary>
    public override void DeletePage(Page page)
    {
      string fileName = _Folder + "pages\\" + page.Id.ToString() + ".xml";
      if (File.Exists(fileName))
        File.Delete(fileName);

      if (Page.Pages.Contains(page))
        Page.Pages.Remove(page);
    }

    /// <summary>
    /// Retrieves all pages from the data store
    /// </summary>
    /// <returns>List of Pages</returns>
    public override List<Page> FillPages()
    {
      string folder = CategoryDictionary._Folder + "pages\\";
      List<Page> pages = new List<Page>();

      foreach (string file in Directory.GetFiles(folder, "*.xml", SearchOption.TopDirectoryOnly))
      {
        FileInfo info = new FileInfo(file);
        string id = info.Name.Replace(".xml", string.Empty);
        Page page = Page.Load(new Guid(id));
        pages.Add(page);
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
      string fileName = _Folder + "categories.xml";
      if (!File.Exists(fileName))
        return null;

      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);
      CategoryDictionary dic = new CategoryDictionary();

      foreach (XmlNode node in doc.SelectNodes("categories/category"))
      {
        Guid id = new Guid(node.Attributes["id"].InnerText);
        string title = node.InnerText;
        dic.Add(id, title);
      }

      return dic;
    }

    /// <summary>
    /// Saves the categories to the data store.
    /// </summary>
    public override void SaveCategories(CategoryDictionary categories)
    {
      string fileName = _Folder + "categories.xml";

      using (XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 4;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("categories");

        foreach (Guid key in categories.Keys)
        {
          //writer.WriteRaw("<category id=\"" + key.ToString() + "\">" + categories[key] + "</category>");
          writer.WriteStartElement("category");
          writer.WriteAttributeString("id", key.ToString());
          writer.WriteValue(categories[key]);
          writer.WriteEndElement();
        }

        writer.WriteEndElement();
      }
    }

    #endregion

    #region Settings

    /// <summary>
    /// Loads the settings from the provider.
    /// </summary>
    public override StringDictionary LoadSettings()
    {
      string filename = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/settings.xml");
      StringDictionary dic = new StringDictionary();

      XmlDocument doc = new XmlDocument();
      doc.Load(filename);

      foreach (XmlNode settingsNode in doc.SelectSingleNode("settings").ChildNodes)
      {
        string name = settingsNode.Name;
        string value = settingsNode.InnerText;

        dic.Add(name, value);
      }

      return dic;
    }

    /// <summary>
    /// Saves the settings to the provider.
    /// </summary>
    public override void SaveSettings(StringDictionary settings)
    {
      string filename = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/settings.xml");
      XmlWriterSettings writerSettings = new XmlWriterSettings(); ;
      writerSettings.Indent = true;

      //------------------------------------------------------------
      //	Create XML writer against file path
      //------------------------------------------------------------
      using (XmlWriter writer = XmlWriter.Create(filename, writerSettings))
      {
        writer.WriteStartElement("settings");

        foreach (string key in settings.Keys)
        {
          writer.WriteElementString(key, settings[key]);
        }

        writer.WriteEndElement();
      }
    }

    #endregion

  }
}
