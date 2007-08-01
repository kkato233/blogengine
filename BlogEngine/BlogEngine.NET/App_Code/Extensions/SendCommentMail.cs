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
[Extension("Sends an e-mail to the blog owner whenever a comment is added", "", "")]
public class SendCommentMail
{

  /// <summary>
  /// Hooks up an event handler to the Post.CommentAdded event.
  /// </summary>
  public SendCommentMail()
  {
    Post.CommentAdded += new EventHandler<EventArgs>(Post_CommentAdded);
  }

  private void Post_CommentAdded(object sender, EventArgs e)
  {
    Post post = (Post)sender;
    if (post != null && BlogSettings.Instance.SendMailOnComment && !Thread.CurrentPrincipal.Identity.IsAuthenticated)
    {
      Comment comment = post.Comments[post.Comments.Count - 1];

      string receiver = comment.Email.Contains("@") ? comment.Email : BlogSettings.Instance.Email;

      MailMessage mail = new MailMessage();
      mail.From = new MailAddress(receiver, comment.Author);
      mail.To.Add(BlogSettings.Instance.Email);
      mail.Subject = "Weblog comment on " + post.Title;
      mail.Body = "Comment by " + comment.Author + " (" + comment.Email + ")" + Environment.NewLine + Environment.NewLine;
      mail.Body += comment.Content + "\n\n" + post.PermaLink.ToString();

      Utils.SendMailMessageAsync(mail);
    }
  }

}