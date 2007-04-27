#region Using

using System;
using System.Web;
using System.Web.Security;
using System.Threading;
using BlogEngine.Core;

#endregion

/// <summary>
/// A macro is a static method or property that can be 
/// used throughout the web project.
/// </summary>
public static class Macro
{

  /// <summary>
  /// Creates a link for signing in and out.
  /// </summary>
  public static string SignInLink
  {
    get
    {
      string a = "<a href=\"{0}{1}\" rel=\"nofollow\">{2}</a>";
      string login = FormsAuthentication.LoginUrl;
      if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
        return string.Format(a, login, "?signout=true", "Sign out");

      return string.Format(a, login, string.Empty, "Sign in");
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
