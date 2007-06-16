#region Using

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Web.Controls
{
  /// <summary>
  /// The PostView.ascx that is located in the themes folder
  /// has to inherit from this class. 
  /// <remarks>
  /// It provides the basic functionaly needed to display a post.
  /// </remarks>
  /// </summary>
  public class PostViewBase : UserControl
  {

    /// <summary>
    /// Manages the deletion of posts.
    /// </summary>
    private void Page_Init(object sender, EventArgs e)
    {
      if (!Post.IsPublished && !Page.User.Identity.IsAuthenticated)
      {
        this.Visible = false;
      }
    }

    /// <summary>
    /// The Post object that is displayed through the PostView.ascx control.
    /// </summary>
    /// <value>The Post object that has to be displayed.</value>
    public virtual Post Post
    {
      get { return (Post)(ViewState["Post"] ?? default(Post)); }
      set { ViewState["Post"] = value; }
    }

    /// <summary>
    /// Gets the comment feed link.
    /// </summary>
    /// <value>The comment feed.</value>
    public string CommentFeed
    {
      get { return Utils.RelativeWebRoot + "commentfeed.axd?id=" + Post.Id.ToString(); }
    }

    #region Protected methods

    /// <summary>
    /// Displays the Post's categories seperated by the specified string.
    /// </summary>
    protected virtual string CategoryLinks(string separator)
    {
      string[] keywords = new string[Post.Categories.Count];
      string link = "<a href=\"{0}{1}.aspx\">{2}</a>";
      string path = VirtualPathUtility.ToAbsolute("~/category/");
      for (int i = 0; i < Post.Categories.Count; i++)
      {
        if (CategoryDictionary.Instance.ContainsKey(Post.Categories[i]))
        {
          string category = CategoryDictionary.Instance[Post.Categories[i]];
          keywords[i] = string.Format(link, path, Utils.RemoveIlegalCharacters(category), category);
        }
      }

      return string.Join(separator, keywords);
    }

    /// <summary>
    /// Displays the Post's tags seperated by the specified string.
    /// </summary>
    protected virtual string TagLinks(string separator)
    {
      if (Post.Tags.Count == 0)
        return null;

      string[] tags = new string[Post.Tags.Count];
      string link = "<a href=\"{0}/{1}\" rel=\"tag\">{1}</a>";
      string path = Utils.RelativeWebRoot + "?tag=";
      for (int i = 0; i < Post.Tags.Count; i++)
      {
        string tag = Post.Tags[i];
        tags[i] = string.Format(link, path, tag);
      }

      return "Tags: " + string.Join(separator, tags);
    }

    /// <summary>
    /// Displays an Edit and Delete link to any 
    /// authenticated user.
    /// </summary>
    protected virtual string AdminLinks
    {
      get
      {
        if (Page.User.Identity.IsAuthenticated)
        {
          BlogBasePage page = (BlogBasePage)Page;
          string confirmDelete = string.Format(page.Translate("areYouSure"),page.Translate("delete").ToLowerInvariant(), page.Translate("thePost"));
          StringBuilder sb = new StringBuilder();
          sb.AppendFormat("<a href=\"{0}\">{1}</a> | ", VirtualPathUtility.ToAbsolute("~/") + "admin/pages/add_entry.aspx?id=" + Post.Id.ToString(), page.Translate("edit"));
          sb.AppendFormat("<a href=\"{0}?deletepost={1}\" onclick=\"return confirm('{2}')\">{3}</a> | ", Post.RelativeLink, Post.Id.ToString(), confirmDelete, page.Translate("delete"));
          return sb.ToString();
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Enable visitors to rate the post.
    /// </summary>
    protected virtual string Rating
    {
      get
      {
        float rating = Post.Rating / 5 * 100;
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=\"rating\">");

        BlogBasePage page = (BlogBasePage)Page;

        if (Post.Raters > 0)
          sb.AppendFormat("<p>" + page.Translate("currentlyRated") + "</p>", Post.Rating.ToString("#.0"), Post.Raters);
        else
          sb.Append("<p>Be the first to rate this post</p>");

        string script = "Rate('{0}', {1});";
        if (Request.Cookies["rating"] != null && Request.Cookies["rating"].Value.Contains(Post.Id.ToString()))
          script = "alert('" + page.Translate("youAlreadyRated") + "');";

        sb.Append("<ul class=\"star-rating small-star\">");
        sb.AppendFormat("<li class=\"current-rating\" style=\"width:{0}%\">Currently {1}/5 Stars.</li>", Math.Round(rating, 0), Post.Rating);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 1 star out of 5\" class=\"one-star\">1</a></li>", Post.Id.ToString(), 1);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 2 stars out of 5\" class=\"two-stars\">2</a></li>", Post.Id.ToString(), 2);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 3 stars out of 5\" class=\"three-stars\">3</a></li>", Post.Id.ToString(), 3);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 4 stars out of 5\" class=\"four-stars\">4</a></li>", Post.Id.ToString(), 4);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 5 stars out of 5\" class=\"five-stars\">5</a></li>", Post.Id.ToString(), 5);
        sb.Append("</ul>");
        sb.Append("</div>");
        return sb.ToString();
      }
    }

    #endregion
  }
}