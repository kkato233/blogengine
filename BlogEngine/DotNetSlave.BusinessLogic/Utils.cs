#region Using

using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Threading;

#endregion

namespace BlogEngine.Core
{
  /// <summary>
  /// Utilities for the entire solution to use.
  /// </summary>
  public static class Utils
  {

    /// <summary>
    /// Strips all illegal characters from the specified title.
    /// </summary>
    public static string RemoveIllegalCharacters(string text)
    {
      if (string.IsNullOrEmpty(text))
        return text;

      text = text.Replace(":", string.Empty);
      text = text.Replace("/", string.Empty);
      text = text.Replace("?", string.Empty);
      text = text.Replace("#", string.Empty);
      text = text.Replace("[", string.Empty);
      text = text.Replace("]", string.Empty);
      text = text.Replace("@", string.Empty);
      text = text.Replace(".", string.Empty);
      text = text.Replace("\"", string.Empty);
      text = text.Replace("&", string.Empty);
      text = text.Replace("%", string.Empty);
      return HttpUtility.UrlEncode(text.Replace(" ", "-"));
    }

    /// <summary>
    /// Gets the relative URL of the blog feed. If a Feedburner username
    /// is entered in the admin settings page, it will return the 
    /// absolute Feedburner URL to the feed.
    /// </summary>
    public static string FeedUrl
    {
      get
      {
        if (!string.IsNullOrEmpty(BlogSettings.Instance.AlternateFeedUrl))
          return BlogSettings.Instance.AlternateFeedUrl;
        else
          return AbsoluteWebRoot + "syndication.axd";
      }
    }

    /// <summary>
    /// Gets the relative root of the website.
    /// </summary>
    /// <value>A string that ends with a '/'.</value>
    public static string RelativeWebRoot
    {
      get { return VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["BlogEngine.VirtualPath"]); }
    }

    private static Uri _AbsoluteWebRoot;

    /// <summary>
    /// Gets the absolute root of the website.
    /// </summary>
    /// <value>A string that ends with a '/'.</value>
    public static Uri AbsoluteWebRoot
    {
      get
      {
        if (_AbsoluteWebRoot == null)
        {
          HttpContext context = HttpContext.Current;
          if (context == null)
            throw new System.Net.WebException("The current HttpContext is null");

          _AbsoluteWebRoot = new Uri(context.Request.Url.Scheme + "://" + context.Request.Url.Authority + RelativeWebRoot);
        }
        return _AbsoluteWebRoot;
      }
    }

    /// <summary>
    /// Converts a relative URL to an absolute one.
    /// </summary>
    public static Uri ConvertToAbsolute(Uri relativeUri)
    {
      if (relativeUri == null)
        throw new ArgumentNullException("relativeUri");

      string absolute = AbsoluteWebRoot.ToString();
      return new Uri(absolute.Substring(0, absolute.Length -1) + relativeUri.ToString());
    }

    #region Send e-mail

    /// <summary>
    /// Sends a MailMessage object using the SMTP settings.
    /// </summary>
    public static void SendMailMessage(MailMessage message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      try
      {
        message.BodyEncoding = Encoding.UTF8;
        SmtpClient smtp = new SmtpClient(BlogSettings.Instance.SmtpServer);
        smtp.Credentials = new System.Net.NetworkCredential(BlogSettings.Instance.SmtpUsername, BlogSettings.Instance.SmtpPassword);
        smtp.Port = BlogSettings.Instance.SmtpServerPort;
        smtp.EnableSsl = BlogSettings.Instance.EnableSsl;
        smtp.Send(message);
        OnEmailSent(message);
      }
      finally
      {
        // Remove the pointer to the message object so the GC can close the thread.
        message.Dispose();
        message = null;
      }
    }

    /// <summary>
    /// Sends the mail message asynchronously in another thread.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public static void SendMailMessageAsync(MailMessage message)
    {
      ThreadStart threadStart = delegate { Utils.SendMailMessage(message); };
      Thread thread = new Thread(threadStart);
      thread.IsBackground = true;
      thread.Start();
    }

    /// <summary>
    /// Occurs after an e-mail has been sent. The sender is the MailMessage object.
    /// </summary>
    public static event EventHandler<EventArgs> EmailSent;
    private static void OnEmailSent(MailMessage message)
    {
      if (EmailSent != null)
      {
        EmailSent(message, new EventArgs());
      }
    }
        
    #endregion

  }
}
