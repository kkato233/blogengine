#region Using

using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using BlogEngine.Core.Entities;

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
  public class XmlBlogProvider : BlogProvider
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
        writer.WriteElementString("pubDate", post.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"));
        writer.WriteElementString("lastModified", post.DateModified.ToString("yyyy-MM-dd HH:mm:ss"));

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
          writer.WriteElementString("date", comment.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"));
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
      page.DateCreated = DateTime.Parse(doc.SelectSingleNode("page/datecreated").InnerText);
      page.DateModified = DateTime.Parse(doc.SelectSingleNode("page/datemodified").InnerText);

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
        writer.WriteElementString("datecreated", page.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"));
        writer.WriteElementString("datemodified", page.DateModified.ToString("yyyy-MM-dd HH:mm:ss"));

        writer.WriteEndElement();
      }

      if (page.IsNew)
        Page.Pages.Add(page);
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
      using (XmlWriter writer = XmlWriter.Create(fileName))
      {
        writer.WriteStartDocument(true);
        writer.WriteStartElement("categories");

        foreach (Guid key in categories.Keys)
        {
          writer.WriteRaw("<category id=\"" + key.ToString() + "\">" + categories[key] + "</category>");
        }

        writer.WriteEndElement();
      }
    }

    #endregion

  }
}
