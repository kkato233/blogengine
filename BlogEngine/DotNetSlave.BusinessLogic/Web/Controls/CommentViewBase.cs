#region Using

using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Entities;

#endregion

namespace BlogEngine.Core.Web.Controls
{
  public class CommentViewBase : UserControl
  {

    #region Properties

    private Post _Post;

    public Post Post
    {
      get { return _Post; }
      set { _Post = value; }
    }

    private Comment _Comment;

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
    protected string ResolveLinks(string body)
    {
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
    /// Displays a delete link to visitors that is authenticated
    /// using the default membership provider.
    /// </summary>
    protected string AdminLinks
    {
      get
      {
        if (Page.User.Identity.IsAuthenticated)
        {
          System.Text.StringBuilder sb = new System.Text.StringBuilder();
          sb.AppendFormat(" | <a href=\"mailto:{0}\">{0}</a>", Comment.Email);
          sb.AppendFormat(" | <a href=\"http://whois.domaintools.com/{0}/\">{0}</a>", Comment.IP);
          string confirmDelete = "Are you sure you want to delete the comment?";
          sb.AppendFormat(" | <a href=\"?deletecomment={0}\" onclick=\"return confirm('{1}?')\">{2}</a>", Comment.Id, confirmDelete, "Delete");
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
      if (!Comment.Email.Contains("@"))
        return string.Format("<img class=\"thumb\" src=\"http://images.websnapr.com/?url={0}&amp;size=t\" alt=\"{1}\" />", Server.UrlEncode(Comment.Website.ToString()), Comment.Email);
      //http://www.artviper.net/screenshots/screener.php?&url={0}&h={1}&w={1}
      System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
      byte[] result = md5.ComputeHash(Encoding.ASCII.GetBytes(Comment.Email));

      System.Text.StringBuilder hash = new System.Text.StringBuilder();
      for (int i = 0; i < result.Length; i++)
        hash.Append(result[i].ToString("x2"));

      StringBuilder image = new StringBuilder();
      image.Append("<img src=\"" + Utils.RelativeWebRoot + "pics/pixel.gif\"");
      image.Append("style=\"background: url(");
      image.Append("http://www.gravatar.com/avatar.php?");
      image.Append("gravatar_id=" + hash.ToString());
      //image.Append("&amp;rating=G");
      image.Append("&amp;size=" + size);
      image.Append("&amp;default=");
      image.Append(Server.UrlEncode(Utils.AbsoluteWebRoot + "themes/" + BlogSettings.Instance.Theme + "/noavatar.jpg"));
      image.Append(")\" alt=\"\" />");
      return image.ToString();
    }

    #endregion

  }
}
