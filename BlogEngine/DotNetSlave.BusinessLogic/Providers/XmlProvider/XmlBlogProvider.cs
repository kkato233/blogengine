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
      string fileName = _Folder + "posts" + Path.DirectorySeparatorChar + id.ToString() + ".xml";
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
        post.Raters = int.Parse(doc.SelectSingleNode("post/raters").InnerText, CultureInfo.InvariantCulture);

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

        if (node.Attributes["approved"] != null)
          comment.IsApproved = bool.Parse(node.Attributes["approved"].InnerText);
        else
          comment.IsApproved = true;

        comment.Content = node.SelectSingleNode("content").InnerText;
        comment.DateCreated = DateTime.Parse(node.SelectSingleNode("date").InnerText, CultureInfo.InvariantCulture);
        post.Comments.Add(comment);
      }

      post.Comments.Sort();

      // categories
      foreach (XmlNode node in doc.SelectNodes("post/categories/category"))
      {
        Guid key = new Guid(node.InnerText);
        Category cat = Category.GetCategory(key);
        if (cat != null)//CategoryDictionary.Instance.ContainsKey(key))
          post.Categories.Add(cat);
      }

      // Notification e-mails
      foreach (XmlNode node in doc.SelectNodes("post/notifications/email"))
      {
        post.NotificationEmails.Add(node.InnerText);
      }

      return post;
    }

    /// <summary>
    /// Inserts a new Post to the data store.
    /// </summary>
    /// <param name="post"></param>
    public override void InsertPost(Post post)
    {
      if (!Directory.Exists(_Folder + "posts"))
        Directory.CreateDirectory(_Folder + "posts");

      string fileName = _Folder + "posts" + Path.DirectorySeparatorChar + post.Id.ToString() + ".xml";
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
        writer.WriteElementString("pubDate", post.DateCreated.AddHours(-BlogSettings.Instance.Timezone).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        writer.WriteElementString("lastModified", post.DateModified.AddHours(-BlogSettings.Instance.Timezone).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        writer.WriteElementString("raters", post.Raters.ToString(CultureInfo.InvariantCulture));
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
          writer.WriteAttributeString("approved", comment.IsApproved.ToString());
          writer.WriteElementString("date", comment.DateCreated.AddHours(-BlogSettings.Instance.Timezone).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
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
        foreach (Category cat in post.Categories)
        {
          //if (cat.Id = .Instance.ContainsKey(key))
          //     writer.WriteElementString("category", key.ToString());
          writer.WriteElementString("category", cat.Id.ToString());

        }
        writer.WriteEndElement();

        // Notification e-mails
        writer.WriteStartElement("notifications");
        foreach (string email in post.NotificationEmails)
        {
          writer.WriteElementString("email", email);
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
      string fileName = _Folder + "posts" + Path.DirectorySeparatorChar + post.Id.ToString() + ".xml";
      if (File.Exists(fileName))
        File.Delete(fileName);
    }

    /// <summary>
    /// Retrieves all posts from the data store
    /// </summary>
    /// <returns>List of Posts</returns>
    public override List<Post> FillPosts()
    {
      string folder = Category._Folder + "posts" + Path.DirectorySeparatorChar;
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
      string fileName = _Folder + "pages" + Path.DirectorySeparatorChar + id.ToString() + ".xml";
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      Page page = new Page();

      page.Title = doc.SelectSingleNode("page/title").InnerText;
      page.Description = doc.SelectSingleNode("page/description").InnerText;
      page.Content = doc.SelectSingleNode("page/content").InnerText;
      page.Keywords = doc.SelectSingleNode("page/keywords").InnerText;

      if (doc.SelectSingleNode("page/parent") != null)
        page.Parent = new Guid(doc.SelectSingleNode("page/parent").InnerText);

      if (doc.SelectSingleNode("page/isfrontpage") != null)
        page.IsFrontPage = bool.Parse(doc.SelectSingleNode("page/isfrontpage").InnerText);

      if (doc.SelectSingleNode("page/showinlist") != null)
        page.ShowInList = bool.Parse(doc.SelectSingleNode("page/showinlist").InnerText);

      if (doc.SelectSingleNode("page/ispublished") != null)
        page.IsPublished = bool.Parse(doc.SelectSingleNode("page/ispublished").InnerText);

      page.DateCreated = DateTime.Parse(doc.SelectSingleNode("page/datecreated").InnerText, CultureInfo.InvariantCulture);
      page.DateModified = DateTime.Parse(doc.SelectSingleNode("page/datemodified").InnerText, CultureInfo.InvariantCulture);

      return page;
    }

    /// <summary>
    /// Inserts a new Page to the data store.
    /// </summary>
    public override void InsertPage(Page page)
    {
      if (!Directory.Exists(_Folder + "pages"))
        Directory.CreateDirectory(_Folder + "pages");

      string fileName = _Folder + "pages" + Path.DirectorySeparatorChar + page.Id.ToString() + ".xml";
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
        writer.WriteElementString("parent", page.Parent.ToString());
        writer.WriteElementString("isfrontpage", page.IsFrontPage.ToString());
        writer.WriteElementString("showinlist", page.ShowInList.ToString());
        writer.WriteElementString("ispublished", page.IsPublished.ToString());
        writer.WriteElementString("datecreated", page.DateCreated.AddHours(-BlogSettings.Instance.Timezone).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        writer.WriteElementString("datemodified", page.DateModified.AddHours(-BlogSettings.Instance.Timezone).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

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
      string fileName = _Folder + "pages" + Path.DirectorySeparatorChar + page.Id.ToString() + ".xml";
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
      string folder = Category._Folder + "pages" + Path.DirectorySeparatorChar;
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
    /// Gets a Category based on a Guid
    /// </summary>
    /// <param name="id">The category's Guid.</param>
    /// <returns>A matching Category</returns>
    public override Category SelectCategory(Guid id)
    {
      List<Category> categories = Category.Categories;

      Category category = new Category();

      foreach (Category cat in categories)
      {
        if (cat.Id == id)
          category = cat;
      }
      category.MarkOld();
      return category;
    }

    /// <summary>
    /// Inserts a Category
    /// </summary>
    /// <param name="category">Must be a valid Category object.</param>
    public override void InsertCategory(Category category)
    {
      List<Category> categories = Category.Categories;
      categories.Add(category);
      string fileName = _Folder + "categories.xml";

      using (XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 4;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("categories");

        foreach (Category cat in categories)
        {
          writer.WriteStartElement("category");
          writer.WriteAttributeString("id", cat.Id.ToString());
          writer.WriteAttributeString("description", cat.Description);
          writer.WriteValue(cat.Title);
          writer.WriteEndElement();
          cat.MarkOld();
        }

        writer.WriteEndElement();
      }

    }

    /// <summary>
    /// Updates a Category
    /// </summary>
    /// <param name="category">Must be a valid Category object.</param>
    public override void UpdateCategory(Category category)
    {
      List<Category> categories = Category.Categories;
      categories.Remove(category);
      categories.Add(category);
      string fileName = _Folder + "categories.xml";

      using (XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 4;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("categories");

        foreach (Category cat in categories)
        {
          writer.WriteStartElement("category");
          writer.WriteAttributeString("id", cat.Id.ToString());
          writer.WriteAttributeString("description", cat.Description);
          writer.WriteValue(cat.Title);
          writer.WriteEndElement();
          cat.MarkOld();
        }

        writer.WriteEndElement();
      }
    }

    /// <summary>
    /// Deletes a Category
    /// </summary>
    /// <param name="category">Must be a valid Category object.</param>
    public override void DeleteCategory(Category category)
    {
      List<Category> categories = Category.Categories;
      categories.Remove(category);

      string fileName = _Folder + "categories.xml";

      if (File.Exists(fileName))
        File.Delete(fileName);

      if (Category.Categories.Contains(category))
        Category.Categories.Remove(category);

      using (XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 4;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("categories");

        foreach (Category cat in categories)
        {
          writer.WriteStartElement("category");
          writer.WriteAttributeString("id", cat.Id.ToString());
          writer.WriteAttributeString("description", cat.Description);
          writer.WriteValue(cat.Title);
          writer.WriteEndElement();
          cat.MarkOld();
        }

        writer.WriteEndElement();
      }

    }

    /// <summary>
    /// Fills an unsorted list of categories.
    /// </summary>
    /// <returns>A List&lt;Category&gt; of all Categories.</returns>
    public override List<Category> FillCategories()
    {

      string fileName = _Folder + "categories.xml";
      if (!File.Exists(fileName))
        return null;

      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);
      List<Category> categories = new List<Category>();

      foreach (XmlNode node in doc.SelectNodes("categories/category"))
      {
        Category category = new Category();

        category.Id = new Guid(node.Attributes["id"].InnerText);
        category.Title = node.InnerText;
        if (node.Attributes["description"] != null)
          category.Description = node.Attributes["description"].InnerText;
        categories.Add(category);
        category.MarkOld();
      }

      return categories;
    }

    #endregion

    #region Settings

    /// <summary>
    /// Loads the settings from the provider.
    /// </summary>
    public override StringDictionary LoadSettings()
    {
      string filename = System.Web.HttpContext.Current.Server.MapPath(Utils.RelativeWebRoot + "App_Data/settings.xml");
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
      if (settings == null)
        throw new ArgumentNullException("settings");

      string filename = _Folder + "settings.xml";
      XmlWriterSettings writerSettings = new XmlWriterSettings(); ;
      writerSettings.Indent = true;

      //------------------------------------------------------------
      //    Create XML writer against file path
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
