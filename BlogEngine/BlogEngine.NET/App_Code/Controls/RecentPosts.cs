#region Using

using System;
using System.Web;
using System.Web.UI;
using BlogEngine.Core;

#endregion

namespace Controls
{
  /// <summary>
  /// Shows a chronological list of recent posts.
  /// </summary>
  public class RecentPosts : Control
  {

    static RecentPosts()
    {
      Post.Saved += delegate { _Html = null; };
      BlogSettings.Changed += delegate { _Html = null; };
    }

    private static string _Html;

    private string Html
    {
      get
      {
        if (_Html == null)
        {          
          int number = BlogSettings.Instance.NumberOfRecentPosts;
          if (number > Post.Posts.Count)
            number = Post.Posts.Count;

          _Html = "<ul>";
          int counter = 1;
          
          foreach (Post post in Post.Posts)
          {
            if (counter <= number && post.IsPublished)
            {
              _Html += string.Format("<li><a href=\"{0}\">{1}</a></li>", post.RelativeLink, post.Title);
              counter++;
            }
          }

          _Html += "</ul>";          
        }

        return _Html;
      }
    }

    public override void RenderControl(HtmlTextWriter writer)
    {
      if (Page.IsCallback)
        return;

      writer.Write(Html);     
    }
  }
}