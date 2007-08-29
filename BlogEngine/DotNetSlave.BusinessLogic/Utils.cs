#region Using

using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Text;
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
    public static string RemoveIlegalCharacters(string text)
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
        if (!string.IsNullOrEmpty(BlogSettings.Instance.FeedburnerUserName))
          return "http://feeds.feedburner.com/" + BlogSettings.Instance.FeedburnerUserName;
        else
          return RelativeWebRoot + "syndication.axd";
      }
    }

    /// <summary>
    /// Gets the relative root of the website.
    /// </summary>
    /// <value>A string that ends with a '/'.</value>
    public static string RelativeWebRoot
    {
      get { return VirtualPathUtility.ToAbsolute("~/"); }
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
            throw new NullReferenceException("The current HttpContext is null");

          _AbsoluteWebRoot = new Uri(context.Request.Url.Scheme + "://" + context.Request.Url.Authority + RelativeWebRoot);
        }
        return _AbsoluteWebRoot;
      }
    }

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
      }
      catch (Exception)
      {
        // Ignores if the mail server does not respond.
      }
      finally
      {
        // Remove the pointer to the message object so the GC can close the thread.
        message.Dispose();
        message = null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public static void SendMailMessageAsync(MailMessage message)
    {
      ThreadStart threadStart = delegate { Utils.SendMailMessage(message); };
      Thread thread = new Thread(threadStart);
      thread.IsBackground = true;
      thread.Start();
    }

    #region Safe mail

    /// <summary>
    /// Creates a email address that is hidden from spam robots.
    /// </summary>
    public static string SafeMail(string email)
    {
      return SafeMail(email, null);
    }

    /// <summary>
    /// Creates a email address that is hidden from spam robots
    /// and adds a subject to the e-mail.
    /// </summary>
    public static string SafeMail(string email, string subject)
    {
      if (email == null)
        throw new ArgumentNullException("email");

      int index = email.IndexOf("@");
      string name = Encode(email.Substring(0, index));
      string domain = Encode(email.Substring(index + 1));
      string sub = Encode(subject == null ? null : ",'" + HttpUtility.HtmlEncode(subject.Replace("'", "\\'")) + "'");
      string link = "javascript:SafeMail('{0}','{1}'{2});";

      return string.Format(CultureInfo.InvariantCulture, link, name, domain, sub);
    }

    private static Random _Random = new Random();

    private static string Encode(string value)
    {
      StringBuilder sb = new StringBuilder();

      for (int i = 0; i < value.Length; i++)
      {
        if (_Random.Next(2) == 1)
          sb.AppendFormat("&#{0};", Convert.ToInt32(value[i]));
        else
          sb.Append(value[i]);
      }

      return sb.ToString();
    }

    #endregion

  }
}
