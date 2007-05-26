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
  public class RatingHandler : IHttpHandler
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
      string rating = context.Request.QueryString["rating"];
      if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(rating))
      {
        int rate = 0;
        if (int.TryParse(rating, out rate))
        {
          Post post = Post.GetPost(new Guid(id));
          post.Rate(rate);

          SetCookie(id, context);
          context.Response.Write("OK");
          context.Response.End();
        }
      }

      context.Response.Write("FAIL");
    }

    private void SetCookie(string id, HttpContext context)
    {
      HttpCookie cookie;
      if (context.Request.Cookies["rating"] != null)
      {
        cookie = context.Request.Cookies["rating"];       
      }
      else
      {
        cookie = new HttpCookie("rating");
      }

      cookie.Expires = DateTime.Now.AddYears(2);
      cookie.Value += id;
      context.Response.Cookies.Add(cookie);
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