/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
05/03/2007  brian.kuhn  Fixed bug in Gravatar(int) method
****************************************************************************/
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BlogEngine.Core;

namespace BlogEngine.Core.Web.Controls
{

  /// <summary>
  /// Inherit from this class when you are building the
  /// commentview.ascx user control in your custom theme.
  /// </summary>
  /// <remarks>
  /// The class exposes a lot of functionality to the custom
  /// comment control in the theme folder.
  /// </remarks>
  public class CommentViewBase : UserControl
  {

    #region Properties

    private Post _Post;

    /// <summary>
    /// Gets or sets the Post from which the comment belongs.
    /// </summary>
    /// <value>The Post object.</value>
    public Post Post
    {
      get { return _Post; }
      set { _Post = value; }
    }

    private Comment _Comment;

    /// <summary>
    /// Gets or sets the Comment.
    /// </summary>
    /// <value>The comment.</value>
    public Comment Comment
    {
      get { return _Comment; }
      set { _Comment = value; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// The regular expression used to parse links.
    /// </summary>
    private static Regex regex = new Regex("((http://|www\\.)([A-Z0-9.-]{1,})\\.[0-9A-Z?&=\\-_\\./]{2,})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Examins the comment body for any links and turns them
    /// automatically into one that can be clicked.
    /// <remarks>
    /// All links added to comments will have the rel attribute set
    /// to nofollow to prevent negative pagerank.
    /// </remarks>
    /// </summary>
    protected static string ResolveLinks(string body)
    {
      if (string.IsNullOrEmpty(body))
        return body;

      foreach (Match match in regex.Matches(body))
      {
        if (!match.Value.Contains("://"))
          body = body.Replace(match.Value, "<a href=\"http://" + match.Value + "\" rel=\"nofollow\">" + match.Value + "</a>");
        else
          body = body.Replace(match.Value, "<a href=\"" + match.Value + "\" rel=\"nofollow\">" + match.Value + "</a>");
      }

      return body.Replace(Environment.NewLine, "<br />");
    }

    /// <summary>
    /// Displays a delete link to visitors that are authenticated
    /// using the default membership provider.
    /// </summary>
    protected string AdminLinks
    {
      get
      {
        if (Page.User.Identity.IsAuthenticated)
        {
          BlogBasePage page = (BlogBasePage)Page;
          System.Text.StringBuilder sb = new System.Text.StringBuilder();
          sb.AppendFormat(" | <a href=\"mailto:{0}\">{0}</a>", Comment.Email);
          sb.AppendFormat(" | <a href=\"http://www.domaintools.com/go/?service=whois&amp;q={0}/\">{0}</a>", Comment.IP);
          string confirmDelete = string.Format(page.Translate("areYouSure"), page.Translate("delete").ToLowerInvariant(), page.Translate("theComment"));
          sb.AppendFormat(" | <a href=\"?deletecomment={0}\" onclick=\"return confirm('{1}?')\">{2}</a>", Comment.Id, confirmDelete, page.Translate("delete"));
          return sb.ToString();
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Displays the flag of the country from which the comment was written.
    /// <remarks>
    /// If the country hasn't been resolved from the authors IP address or
    /// the flag does not exist for that country, nothing is displayed.
    /// </remarks>
    /// </summary>
    protected string Flag
    {
      get
      {
        if (!string.IsNullOrEmpty(Comment.Country))
        {
          string path = Server.MapPath("~/pics/flags/" + Comment.Country + ".png");
          if (File.Exists(path))
          {
            return "<img src=\"" + Utils.RelativeWebRoot + "pics/flags/" + Comment.Country + ".png\" class=\"flag\" alt=\"" + Comment.Country + "\" />";
          }
        }

        return null;
      }
    }

    /// <summary>
    /// Displays the Gravatar image that matches the specified email.
    /// </summary>
    protected string Gravatar(int size)
    {
      //------------------------------------------------------------
      //	Local members
      //------------------------------------------------------------
      StringBuilder image = new StringBuilder();

      //------------------------------------------------------------
      //	Attempt to display Gravatar image
      //------------------------------------------------------------
      try
      {
        //------------------------------------------------------------
        //	Determine if both email address and web site unavailable
        //------------------------------------------------------------
        if (String.IsNullOrEmpty(Comment.Email) || !Comment.Email.Contains("@"))
        {

          //------------------------------------------------------------
          //	Determine if valid email address was not provided
          //------------------------------------------------------------
          if (Comment.Website != null && Comment.Website.ToString().Length > 0 && Comment.Website.ToString().Contains("http://"))
          {
            //------------------------------------------------------------
            //	Return WebSnapr image for web site
            //------------------------------------------------------------
            return string.Format("<img class=\"thumb\" src=\"http://images.websnapr.com/?url={0}&amp;size=t\" alt=\"{1}\" />", Server.UrlEncode(Comment.Website.ToString()), Comment.Email);
          }

          //------------------------------------------------------------
          //	Return default avatar image if no email address or web site available
          //------------------------------------------------------------
          return "<img src=\"" + Utils.RelativeWebRoot + "themes/" + BlogSettings.Instance.Theme + "/noavatar.jpg\" alt=\"Gravatar\" />";
        }

        //------------------------------------------------------------
        //	Determine if valid email address was not provided
        //------------------------------------------------------------
        if (!Comment.Email.Contains("@"))
        {
          //------------------------------------------------------------
          //	Return WebSnapr image for web site
          //------------------------------------------------------------
          return string.Format("<img class=\"thumb\" src=\"http://images.websnapr.com/?url={0}&amp;size=t\" alt=\"{1}\" />", Server.UrlEncode(Comment.Website.ToString()), Comment.Email);
        }

        //------------------------------------------------------------
        //	Calculate MD5 hash digest for email address
        //------------------------------------------------------------
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] result = md5.ComputeHash(Encoding.ASCII.GetBytes(Comment.Email));

        System.Text.StringBuilder hash = new System.Text.StringBuilder();
        for (int i = 0; i < result.Length; i++)
        {
          hash.Append(result[i].ToString("x2"));
        }

        //------------------------------------------------------------
        //	Build Gravatar image for email address
        //------------------------------------------------------------
        image.Append("<img src=\"" + Utils.RelativeWebRoot + "pics/pixel.gif\" ");
        image.Append("style=\"background: url(");
        image.Append("http://www.gravatar.com/avatar.php?");
        image.Append("gravatar_id=" + hash.ToString());
        image.Append("&amp;size=" + size);
        image.Append("&amp;default=");
        image.Append(Server.UrlEncode(Utils.AbsoluteWebRoot + "themes/" + BlogSettings.Instance.Theme + "/noavatar.jpg"));
        image.Append(")\" alt=\"Gravatar\" />");
      }
      catch
      {
        //------------------------------------------------------------
        //	Rethrow exception
        //------------------------------------------------------------
        throw;
      }

      //------------------------------------------------------------
      //	Return result
      //------------------------------------------------------------
      return image.ToString();
    }
    #endregion

  }
}
