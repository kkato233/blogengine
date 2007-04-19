#region Using

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetSlave.BlogEngine.BusinessLogic;

#endregion

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
    if (!string.IsNullOrEmpty(Request.QueryString["delete"]))
    {
      if (Page.User.Identity.IsAuthenticated)
      {
        Post post = DotNetSlave.BlogEngine.BusinessLogic.Post.GetPost(new Guid(Request.QueryString["delete"]));
        post.Delete();
        post.Save();
        Response.Redirect("~/");
      }
    }

    if (!Post.IsPublished && !Page.User.Identity.IsAuthenticated)
    {
      this.Visible = false;
    }
  }

  /// <summary>
  /// The Post object that is displayed through the PostView.ascx control.
  /// </summary>
  public virtual Post Post
  {
    get { return (Post)(ViewState["Post"] ?? default(Post)); }
    set { ViewState["Post"] = value; }
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
    string link = "<a href=\"{0}{1}.aspx\">{1}</a>";
    string path = VirtualPathUtility.ToAbsolute("~/tag/");
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
        string confirmDelete = "Are you sure you want to delete the post?";
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("<a href=\"{0}\">{1}</a> | ", VirtualPathUtility.ToAbsolute("~/") + "admin/pages/add_entry.aspx?id=" + Post.Id.ToString(), "Edit");
        sb.AppendFormat("<a href=\"?delete={0}\" onclick=\"return confirm('{1}?')\">{2}</a> | ", Post.Id.ToString(), confirmDelete, "Delete");
        return sb.ToString();
      }

      return string.Empty;
    }
  }

  #endregion

}
