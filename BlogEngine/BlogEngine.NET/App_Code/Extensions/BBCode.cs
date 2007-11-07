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
		string body = e.Body;

		Parse(ref body, "b", "strong");
		Parse(ref body, "i", "em");
		Parse(ref body, "u", "span style=\"text-decoration:underline\"", "span");
		Parse(ref body, "quote", "cite title=\"Quote\"", "cite");

		e.Body = body;
	}

	private static void Parse(ref string body, string code, string tag)
	{
		Parse(ref body, code, tag, tag);
	}

	/// <summary>
	/// Parses the BBCode into XHTML in a safe non-breaking manor.
	/// </summary>
	private static void Parse(ref string body, string code, string startTag, string endTag)
	{
		int start = body.IndexOf("[" + code + "]");
		if (start > -1)
		{
			if (body.IndexOf("[/" + code + "]", start) > -1)
			{
				body = body.Remove(start, code.Length + 2);
				body = body.Insert(start, "<" + startTag + ">");

				int end = body.IndexOf("[/" + code + "]", start);

				body = body.Remove(end, code.Length + 3);
				body = body.Insert(end, "</" + endTag + ">");

				Parse(ref body, code, startTag);
			}
		}
	}

}