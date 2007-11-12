#region using

using System;
using System.Web;
using BlogEngine.Core.Web.Controls;
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
		Post post = (Post)((Comment)sender).Parent;
		if (post != null && BlogSettings.Instance.SendMailOnComment && !Thread.CurrentPrincipal.Identity.IsAuthenticated)
		{
			Comment comment = post.Comments[post.Comments.Count - 1];
			// Trackback and pingback comments don't have a '@' symbol in the e-mail field.
			string from = comment.Email.Contains("@") ? comment.Email : BlogSettings.Instance.Email;

			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(from, HttpUtility.HtmlDecode(comment.Author));
			mail.To.Add(BlogSettings.Instance.Email);
			mail.Subject = BlogSettings.Instance.EmailSubjectPrefix + " comment on " + post.Title;
			mail.Body = "Comment by " + comment.Author + " (" + comment.Email + ")<br /><br />";
			mail.Body += comment.Content + "<br /><br />";
			mail.Body += string.Format("<a href=\"{0}\">{1}</a>", post.PermaLink + "#id_" + comment.Id, post.Title);

			Utils.SendMailMessageAsync(mail);
		}
	}

}
