#region using

using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;

#endregion

/// <summary>
/// Blacklists known spammers from posting comment spam.
/// </summary>
[Extension("Blacklists known spammers from posting comment spam", "1.0", "Mads Kristensen")]
public class BlackLister
{

  public BlackLister()
  {
		Comment.SpamAttack += new EventHandler<EventArgs>(Comment_SpamAttack);
  }

	private static readonly int MAX_SPAM_ATTEMPTS = 2;
	private static readonly Dictionary<string, int> CANDIDATES = new Dictionary<string, int>();
	private static readonly StringCollection KNOWN_SPAMMERS = new StringCollection();
	private static readonly object SYNC_LOCK = new object();

	/// <summary>
	/// Handles the SpamAttack event of the Comment control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	private void Comment_SpamAttack(object sender, EventArgs e)
	{
		lock (SYNC_LOCK)
		{
			string ip = HttpContext.Current.Request.UserHostAddress;
			if (KNOWN_SPAMMERS.Contains(ip))
			{
				HttpContext.Current.Response.StatusCode = 404;
				HttpContext.Current.Response.Clear();
				HttpContext.Current.Response.End();
			}

			ProcessCandidate(ip);
		}
	}

	/// <summary>
	/// Processes the spam candidates and adds them to the blacklist.
	/// </summary>
	/// <param name="ip">The IP address of the spammer.</param>
	private static void ProcessCandidate(string ip)
	{
		if (!CANDIDATES.ContainsKey(ip))
		{
			CANDIDATES.Add(ip, 1);
		}
		else
		{
			CANDIDATES[ip]++;
		}

		if (CANDIDATES[ip] >= MAX_SPAM_ATTEMPTS)
		{
			KNOWN_SPAMMERS.Add(ip);
			CANDIDATES.Remove(ip);
		}
	}

}