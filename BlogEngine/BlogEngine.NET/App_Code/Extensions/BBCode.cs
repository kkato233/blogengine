#region using

using System;
using System.Text.RegularExpressions;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;

#endregion

/// <summary>
/// Converts BBCode to XHTML in the comments.
/// </summary>
[Extension("Converts BBCode to XHTML in the comments", "1.0.0.0", "BlogEngine.NET")]
public class BBCode
{

	public BBCode()
	{
		Comment.Serving += new EventHandler<ServingEventArgs>(Post_CommentServing);
	}

	/// <summary>
	/// The event handler that is triggered every time a comment is served to a client.
	/// </summary>
	private void Post_CommentServing(object sender, ServingEventArgs e)
	{
		if (e.Body.Contains("[b]") && e.Body.Contains("[/b]"))
		{
			e.Body = e.Body.Replace("[b]", "<strong>");
			e.Body = e.Body.Replace("[/b]", "</strong>");
		}

		if (e.Body.Contains("[i]") && e.Body.Contains("[/i]"))
		{
			e.Body = e.Body.Replace("[i]", "<em>");
			e.Body = e.Body.Replace("[/i]", "</em>");
		}
	}

}