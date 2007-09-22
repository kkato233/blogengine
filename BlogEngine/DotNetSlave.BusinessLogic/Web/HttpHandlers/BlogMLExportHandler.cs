#region Using

using System;
using System.Xml;
using System.Web;
using System.Web.Security;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// Displays the open search XML provider as
  /// descriped at http://opensearch.a9.com/
  /// </summary>
  /// <remarks>
  /// The OpenSearch document needs to be linked to from the 
  /// HTML head tag. This link will be added automatically.
  /// </remarks>
  public class BlogMLExportHandler : IHttpHandler
  {

    #region IHttpHandler Members

    /// <summary>
    /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
    /// </summary>
    /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/xml";
      context.Response.AppendHeader("Content-Disposition", "attachment; filename=BlogML.xml");
      WriteXml(context);      
    }

    /// <summary>
    /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
    /// </summary>
    /// <value></value>
    /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
    public bool IsReusable
    {
      get { return false; }
    }

    #endregion

    #region XML creation

    private static void WriteXml(HttpContext context)
    {
      using (XmlWriter writer = XmlWriter.Create(context.Response.OutputStream))
      {
        writer.WriteStartElement("blog", "http://www.blogml.com/2006/09/BlogML");
        writer.WriteAttributeString("date-created", DateTime.Now.AddHours(BlogSettings.Instance.Timezone).ToString());

        writer.WriteStartElement("title");
        writer.WriteAttributeString("type", "text");
        writer.WriteCData(BlogSettings.Instance.Name);

        writer.WriteStartElement("sub-title");
        writer.WriteAttributeString("type", "text");
        writer.WriteCData(BlogSettings.Instance.Description);

        AddAuthors(writer);
        AddExtendedProperties(writer);
        AddCategories(writer);
        AddPosts(writer);

        writer.WriteEndElement();
      }
    }

    private static void AddAuthors(XmlWriter writer)
    {
      writer.WriteStartElement("authors");

      foreach (MembershipUser user in Membership.GetAllUsers())
      {
        writer.WriteStartElement("author");

        writer.WriteAttributeString("id", user.UserName);
        writer.WriteAttributeString("date-created", user.CreationDate.AddHours(BlogSettings.Instance.Timezone).ToString());
        writer.WriteAttributeString("date-modified", user.CreationDate.AddHours(BlogSettings.Instance.Timezone).ToString());
        writer.WriteAttributeString("approved", "true");
        writer.WriteAttributeString("email", user.Email);

        writer.WriteStartElement("title");
        writer.WriteAttributeString("type", "text");
        writer.WriteCData(user.UserName);
        writer.WriteEndElement();

        writer.WriteEndElement();
      }

      writer.WriteEndElement();
    }

    private static void AddExtendedProperties(XmlWriter writer)
    {
      writer.WriteStartElement("extended-properties");

      writer.WriteStartElement("property");
      writer.WriteAttributeString("CommentModeration", "true");
      writer.WriteEndElement();

      writer.WriteStartElement("property");
      writer.WriteAttributeString("SendTrackback", "true");
      writer.WriteEndElement();

      writer.WriteEndElement();
    }

    private static void AddCategories(XmlWriter writer)
    {
      writer.WriteStartElement("categories");

      foreach (Category category in Category.Categories)
      {
        writer.WriteStartElement("category");

        writer.WriteAttributeString("id", category.Id.ToString());
        writer.WriteAttributeString("date-created", category.DateCreated.ToString());
        writer.WriteAttributeString("date-modified", category.DateModified.ToString());
        writer.WriteAttributeString("approved", "true");
        writer.WriteAttributeString("description", category.Description);
        writer.WriteAttributeString("parentref", "0");

        writer.WriteStartElement("title");
        writer.WriteAttributeString("type", "text");
        writer.WriteCData(category.Title);
        writer.WriteEndElement();

        writer.WriteEndElement();
      }

      writer.WriteEndElement();
    }

    private static void AddPosts(XmlWriter writer)
    {
      writer.WriteStartElement("posts");

      foreach (Post post in Post.Posts)
      {
        writer.WriteStartElement("post");

        writer.WriteAttributeString("id", post.Id.ToString());
        writer.WriteAttributeString("date-created", post.DateCreated.ToString());
        writer.WriteAttributeString("date-modified", post.DateModified.ToString());
        writer.WriteAttributeString("approved", "true");
        writer.WriteAttributeString("post-url", post.RelativeLink.ToString());
        writer.WriteAttributeString("type", "normal");
        writer.WriteAttributeString("hasexcerpt", (!string.IsNullOrEmpty(post.Description)).ToString().ToLowerInvariant());

        AddPostTitle(writer, post);
        AddPostContent(writer, post);
        AddPostName(writer, post);
        AddPostExcerpt(writer, post);
        AddPostAuthor(writer, post);
        AddPostCategories(writer, post);
        AddPostComments(writer, post);
        AddPostTrackbacks(writer, post);

        writer.WriteEndElement();
      }

      writer.WriteEndElement();
    }

    private static void AddPostTrackbacks(XmlWriter writer, Post post)
    {
      writer.WriteStartElement("trackbacks");
      foreach (Comment comment in post.Comments)
      {
        if (comment.Email != "trackback" || comment.Email != "pingback")
          continue;

        writer.WriteStartElement("trackback");
        writer.WriteAttributeString("id", comment.Id.ToString());
        writer.WriteAttributeString("date-created", comment.DateCreated.ToString());
        writer.WriteAttributeString("date-modified", comment.DateCreated.ToString());
        writer.WriteAttributeString("approved", comment.IsApproved.ToString().ToLowerInvariant());
        writer.WriteAttributeString("url", comment.Website.ToString());

        writer.WriteStartElement("title");
        writer.WriteAttributeString("type", "text");
        writer.WriteCData(comment.Content);
        writer.WriteEndElement();

        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    private static void AddPostComments(XmlWriter writer, Post post)
    {
      writer.WriteStartElement("comments");
      foreach (Comment comment in post.Comments)
      {
        if (comment.Email == "trackback" || comment.Email == "pingback")
          continue;

        writer.WriteStartElement("comment");
        writer.WriteAttributeString("id", comment.Id.ToString());
        writer.WriteAttributeString("date-created", comment.DateCreated.ToString());
        writer.WriteAttributeString("date-modified", comment.DateCreated.ToString());
        writer.WriteAttributeString("approved", comment.IsApproved.ToString().ToLowerInvariant());
        writer.WriteAttributeString("user-name", comment.Author);

        if (comment.Website != null)
        {
          writer.WriteAttributeString("user-url", comment.Website.ToString());
        }

        writer.WriteStartElement("title");
        writer.WriteAttributeString("type", "text");
        writer.WriteCData("re: " + post.Title);
        writer.WriteEndElement();

        writer.WriteStartElement("content");
        writer.WriteAttributeString("type", "text");
        writer.WriteCData(comment.Content);
        writer.WriteEndElement();

        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    private static void AddPostCategories(XmlWriter writer, Post post)
    {
      writer.WriteStartElement("categories");
      foreach (Category category in post.Categories)
      {
        writer.WriteStartElement("category");
        writer.WriteAttributeString("ref", category.Id.ToString());
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    private static void AddPostAuthor(XmlWriter writer, Post post)
    {
      writer.WriteStartElement("authors");
      writer.WriteStartElement("author");
      writer.WriteAttributeString("ref", post.Author);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    private static void AddPostExcerpt(XmlWriter writer, Post post)
    {
      if (!String.IsNullOrEmpty(post.Description))
      {
        writer.WriteStartElement("excerpt");
        writer.WriteAttributeString("type", "text");
        writer.WriteCData(post.Description);
        writer.WriteEndElement();
      }
    }

    private static void AddPostName(XmlWriter writer, Post post)
    {
      writer.WriteStartElement("post-name");
      writer.WriteAttributeString("type", "text");
      writer.WriteCData(post.Title);
      writer.WriteEndElement();
    }

    private static void AddPostContent(XmlWriter writer, Post post)
    {
      writer.WriteStartElement("content");
      writer.WriteAttributeString("type", "text");
      writer.WriteCData(post.Content);
      writer.WriteEndElement();
    }

    private static void AddPostTitle(XmlWriter writer, Post post)
    {
      writer.WriteStartElement("title");
      writer.WriteAttributeString("type", "text");
      writer.WriteCData(post.Title);
      writer.WriteEndElement();
    }

    #endregion

  }
}