#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using BlogEngine.Core;

#endregion

namespace Controls
{
  /// <summary>
  /// Builds a category list.
  /// </summary>
  public class RecentComments : Control
  {

    static RecentComments()
    {
      CreateHtml();
      Post.CommentAdded += delegate { CreateHtml(); };
      Post.CommentRemoved += delegate { CreateHtml(); };
    }

    #region Properties

    private static object _SyncRoot = new object();
    private static string _Html;

    private static string Html
    {
      get { return _Html; }      
    }

    private static void CreateHtml()
    {
      try
      {
        HtmlGenericControl ul = BindComments();
        System.IO.StringWriter sw = new System.IO.StringWriter();
        ul.RenderControl(new HtmlTextWriter(sw));
        _Html = sw.ToString();
      }
      catch
      {
        // An unhandled exception will cause the Post.CommentAdded
        // event to stop woking.
      }
    }

    #endregion

    private static HtmlGenericControl BindComments()
    {
      HtmlGenericControl ul = new HtmlGenericControl("ul");
      ul.Attributes.Add("class", "recentComments");

      List<Comment> comments = new List<Comment>();

      foreach (Post post in Post.Posts)
      {
        foreach (Comment comment in post.Comments)
        {
          comments.Add(comment);
        }
      }

      comments.Sort();
      comments.Reverse();
      int max = 10;
      int counter = 0;

      foreach (Comment comment in comments)
      {
        if (counter == max)
          break;

        if (comment.Email == "pingback" || comment.Email == "trackback")
          continue;

        int length = comment.Post.Title.Length <= 25 ? comment.Post.Title.Length : 25;

        HtmlGenericControl li = new HtmlGenericControl("li");

        // The post title
        HtmlAnchor title = new HtmlAnchor();
        title.HRef = comment.Post.RelativeLink.ToString();
        title.InnerHtml = comment.Post.Title;
        title.Attributes.Add("class", "postTitle");
        li.Controls.Add(title);

        // The comment count on the post
        LiteralControl count = new LiteralControl(" (" + comment.Post.Comments.Count + ")<br />");
        li.Controls.Add(count);

        // The author
        if (comment.Website != null)
        {
          HtmlAnchor author = new HtmlAnchor();
          author.HRef = comment.Website.ToString();
          author.InnerHtml = comment.Author;
          li.Controls.Add(author);

          LiteralControl wrote = new LiteralControl(" wrote: ");
          li.Controls.Add(wrote);
        }
        else
        {
          LiteralControl author = new LiteralControl(comment.Author + " wrote: ");
          li.Controls.Add(author);
        }

        // The comment body
        int bodyLength = comment.Content.Length <= 50 ? comment.Content.Length : 50;
        LiteralControl body = new LiteralControl(comment.Content.Substring(0, bodyLength) + "... ");
        li.Controls.Add(body);

        // The comment link
        HtmlAnchor link = new HtmlAnchor();
        link.HRef = comment.Post.RelativeLink +  "#id_" + comment.Id;
        link.InnerHtml = "[more]";
        link.Attributes.Add("class", "moreLink");
        li.Controls.Add(link);

        ul.Controls.Add(li);
        counter++;
      }

      return ul;
    }

    /// <summary>
    /// Renders the control.
    /// </summary>
    public override void RenderControl(HtmlTextWriter writer)
    {
      if (Post.Posts.Count > 0)
      {
        writer.Write(Html);
        writer.Write(Environment.NewLine);
      }
    }
  }
}