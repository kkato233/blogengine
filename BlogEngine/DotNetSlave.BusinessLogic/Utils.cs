#region Using

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

#endregion

namespace BlogEngine.Core.Entities
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
    public static string RelativeWebRoot
    {
      get { return VirtualPathUtility.ToAbsolute("~/"); }
    }

    private static Uri _AbsoluteWebRoot;

    /// <summary>
    /// Gets the absolute root of the website.
    /// </summary>
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
  }
}
