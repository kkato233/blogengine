#region using

using System;
using System.Web;
using BlogEngine.Core.Web;
using BlogEngine.Core;
using System.Net.Mail;
using System.Threading;

#endregion

/// <summary>
/// Sends an e-mail to the blog owner whenever a comment is added.
/// </summary>
[Extension("Sends an e-mail to the blog owner whenever a comment is added", "1.0", "Mads Kristensen")]
public class TextExtension
{

  /// <summary>
  /// Hooks up an event handler to the Post.CommentAdded event.
  /// </summary>
  public TextExtension()
  {
    Post.Serving += new EventHandler<ServingEventArgs>(Post_Serving);
    Post.CommentServing += new EventHandler<ServingEventArgs>(Post_CommentServing);
  }

  void Post_CommentServing(object sender, ServingEventArgs e)
  {
    e.Body += "lort";
  }

  void Post_Serving(object sender, ServingEventArgs e)
  {
    e.Body += "ost";
  }

}
