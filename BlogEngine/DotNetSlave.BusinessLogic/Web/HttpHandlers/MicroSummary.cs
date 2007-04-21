#region Using

using System;
using System.Web;
using BlogEngine.Core.Entities;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// Summary description for MicroSummary
  /// </summary>
  public class MicroSummary : IHttpHandler
  {

    public void ProcessRequest(HttpContext context)
    {
      string id = context.Request.QueryString["id"];
      if (!String.IsNullOrEmpty(id) && id.Length == 36)
      {
        Post post = Post.GetPost(new Guid(id));
        if (post != null)
        {
          context.Response.ContentType = "text/plain";
          context.Response.Write(post.Description);
        }
      }
    }

    public bool IsReusable
    {
      get { return false; }
    }

  }
}