#region Using

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

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

      StringBuilder sb = new StringBuilder();
      text = text.Replace(" ", "-");
      foreach (char c in text)
      {
        if (IsAllowedCharacter(c))
          sb.Append(c);
      }

      return sb.ToString();
    }

    private static bool IsAllowedCharacter(char character)
    {
      string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
      foreach (char c in allowedChars)
      {
        if (c == character)
          return true;
      }

      return false;
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
            throw new Exception("The current HttpContext is null");

          _AbsoluteWebRoot = new Uri(context.Request.Url.Scheme + "://" + context.Request.Url.Authority + RelativeWebRoot);
        }
        return _AbsoluteWebRoot;
      }
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
      string name = email.Substring(0, index);
      string domain = email.Substring(index + 1);
      string sub = subject == null ? null : ",'" + HttpUtility.HtmlEncode(subject.Replace("'", "\\'")) + "'";
      string link = "javascript:SafeMail('{0}','{1}'{2});";

      return string.Format(link, name, domain, sub);
    }

    #endregion

  }
}
