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
      Post.Saved += new EventHandler<SavedEventArgs>(Post_Saved);
      Post.CommentAdded += delegate { _Html = null; };
      Post.CommentRemoved += delegate { _Html = null; };
      Post.Rated += delegate { _Html = null; };
      BlogSettings.Changed += delegate { _Html = null; };
    }

    static void Post_Saved(object sender, SavedEventArgs e)
    {
      if (e.Action != SaveAction.Update)
        _Html = null;
    }

    private static object _SyncRoot = new object();
    private static string _Html;

    private string Html
    {
      get
      {
        if (_Html == null)
        {
          lock (_SyncRoot)
          {
            if (_Html == null)
            {
              int number = BlogSettings.Instance.NumberOfRecentPosts;
              if (number > Post.Posts.Count)
                number = Post.Posts.Count;

              _Html = "<ul class=\"recentPosts\">";
              int counter = 1;

              foreach (Post post in Post.Posts)
              {
                if (counter <= number && post.IsPublished)
                {
                  string link = "<li><a href=\"{0}\">{1}</a><span>{2}: {3}</span><span>{4}: {5}</span></li>";
                  string rating = Math.Round(post.Rating, 1).ToString(System.Globalization.CultureInfo.InvariantCulture);
                  if (post.Raters == 0)
                    rating = "Not rated yet";
                  _Html += string.Format(link, post.RelativeLink, post.Title, "Comments", post.Comments.Count, "Rating", rating);
                  counter++;
                }
              }

              _Html += "</ul>";
            }
          }
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