#region using

using System;
using System.Web;
using BlogEngine.Core.Web;
using BlogEngine.Core;
using System.Net.Mail;
using System.Threading;

#endregion

/// <summary>
/// Pings all the ping services specified on the 
/// PingServices admin page and send track- and pingbacks
/// </summary>
[Extension("Pings all the ping services specified on the PingServices admin page and send track- and pingbacks", "1.1", "BlogEngine.NET")]
public class SendPings
{

  /// <summary>
  /// Hooks up an event handler to the Post.Saved event.
  /// </summary>
  public SendPings()
  {
    Post.Saved += new EventHandler<SavedEventArgs>(Post_Saved);
  }

  /// <summary>
  /// Sends the pings in a new thread.
  /// <remarks>
  /// It opens a new thread and executes the pings from there,
  /// because it takes some time to complete.
  /// </remarks>
  /// </summary>
  private void Post_Saved(object sender, SavedEventArgs e)
  {
    Post post = (Post)sender;
    if (!HttpContext.Current.Request.IsLocal && post.IsPublished)
    {
      ThreadStart threadStart = delegate { Ping(post); };
      Thread thread = new Thread(threadStart);
      thread.IsBackground = true;
      thread.Start();
    }
  }

  /// <summary>
  /// Executes the pings from the new thread.
  /// </summary>
  private void Ping(object stateInfo)
  {
    System.Threading.Thread.Sleep(2000);
    Post post = (Post)stateInfo;

    // Ping the specified ping services.
    BlogEngine.Core.Ping.PingService.Send();

    // Send trackbacks and pingbacks.
    if (post.Content.ToLowerInvariant().Contains("http"))
      BlogEngine.Core.Ping.Manager.Send(post);
  }

}
