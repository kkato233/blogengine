#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Entities;
using BlogEngine.Core.Web.Controls;

#endregion

public partial class post : BlogEngine.Core.Web.Controls.BlogBasePage
{
  protected void Page_Init(object sender, EventArgs e)
  {
    if (Request.QueryString["id"] != null && Request.QueryString["id"].Length == 36)
    {
      Guid id = new Guid(Request.QueryString["id"]);
      this.Post = Post.GetPost(id);

      if (Post != null)
      {
        string path = "~/themes/" + BlogSettings.Instance.Theme + "/postview.ascx";
        
        PostViewBase postView = (PostViewBase)LoadControl(path);
        postView.Post = Post;

        pwPost.Controls.Add(postView);
        related.Post = this.Post;
        CommentView1.Post = Post;

        Page.Title = Post.Title;
        AddMetaKeywords();
        AddMetaDescription();
        Response.AppendHeader("x-pingback", "http://" + Request.Url.Authority + Utils.RelativeWebRoot + "pingback.axd");
      }
    }
  }

  /// <summary>
  /// Adds the post's description as the description metatag.
  /// </summary>
  private void AddMetaDescription()
  {
    if (!string.IsNullOrEmpty(Post.Description))
      base.AddMetaTag("description", Post.Description);
    else
      base.AddMetaTag("description", BlogSettings.Instance.Description);
  }

  /// <summary>
  /// Adds the post's tags as meta keywords.
  /// </summary>
  private void AddMetaKeywords()
  {
    if (Post.Tags.Count > 0)
    {
      string[] tags = new string[Post.Tags.Count];
      for (int i = 0; i < Post.Tags.Count; i++)
      {
        tags[i] = Post.Tags[i];
      }
      base.AddMetaTag("keywords", string.Join(",", tags));
    }
  }

  public Post Post;
}
