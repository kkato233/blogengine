#region Using

using System;
using System.Web;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// Summary description for MicroSummary
  /// </summary>
  public class MicroSummary : IHttpHandler
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
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

    /// <summary>
    /// 
    /// </summary>
    public bool IsReusable
    {
        get { return false; }
    }

  }
}