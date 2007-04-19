#region Using

using System;
using System.Collections.Generic;
using DotNetSlave.BlogEngine.BusinessLogic;

#endregion

public partial class User_controls_PostList : System.Web.UI.UserControl
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!Page.IsCallback)
    {
      BindPosts();
      InitPaging();
    }
  }

  private void BindPosts()
  {
    if (Posts == null || Posts.Count == 0)
    {
      hlPrev.Visible = false;
      return;
    }

    int count = BlogSettings.Instance.PostsPerPage;
    if (Posts.Count < count)
      count = Posts.Count;

    int page = GetPageIndex();
    int index = page * count;
    int stop = count;
    if (index + count > Posts.Count)
      stop = Posts.Count - index;

    string path = "~/themes/" + BlogSettings.Instance.Theme + "/postview.ascx";
    foreach (Post post in Posts.GetRange(index, stop))    {
      
      PostViewBase postView = (PostViewBase)LoadControl(path);
      postView.Post = post;
      posts.Controls.Add(postView);
    }

    if (index + stop == Posts.Count)
      hlPrev.Visible = false;
  }

  /// <summary>
  /// Retrieves the current page index based on the QueryString.
  /// </summary>
  private int GetPageIndex()
  {
    int index = 0;
    if (int.TryParse(Request.QueryString["page"], out index))
      index--;

    return index;
  }

  /// <summary>
  /// Initializes the Next and Previous links
  /// </summary>
  private void InitPaging()
  {
    string path = Request.RawUrl;
    if (path.Contains("?"))
      path = path.Substring(0, path.IndexOf("?"));

    int page = GetPageIndex();
    string url = path + "?page={0}";
    hlNext.NavigateUrl = string.Format(url, page);
    hlPrev.NavigateUrl = string.Format(url, page +2);

    if (page == 0)
      hlNext.Visible = false;
  }

  #region Properties

  public List<Post> Posts
  {
    get { return (List<Post>)(ViewState["Posts"] ?? default(List<Post>)); }
    set { ViewState["Posts"] = value; }
  }

  #endregion

}
