#region Using

using System;
using System.Web;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// Adding a MicroSummary as described in http://wiki.mozilla.org/Microsummaries
  /// </summary>
  /// <remarks>
  /// Microsummaries are regularly-updated short summaries of web pages. They are 
  /// compact enough to fit in the space available to a bookmark label, they provide 
  /// more useful information about pages than static page titles, and they get 
  /// regularly updated as new information becomes available. 
  /// </remarks>
  public class MicroSummary : IHttpHandler
  {

    /// <summary>
    /// Enables processing of HTTP Web requests by a custom HttpHandler that 
    /// implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
    /// </summary>
    /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> 
    /// object that provides references to the intrinsic server objects 
    /// (for example, Request, Response, Session, and Server) used to service HTTP requests.
    /// </param>
    public void ProcessRequest(HttpContext context)
    {
      string id = context.Request.QueryString["id"];
      if (!String.IsNullOrEmpty(id) && id.Length == 36)
      {
        Post post = Post.GetPost(new Guid(id));
        if (post != null)
        {
          context.Response.Write(post.Description);
        }
      }
      else
      {
        context.Response.Write(BlogSettings.Instance.Description);
      }

      context.Response.ContentType = "text/plain";
    }

    /// <summary>
    /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
    /// </summary>
    /// <value></value>
    /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
    public bool IsReusable
    {
        get { return false; }
    }

  }
}