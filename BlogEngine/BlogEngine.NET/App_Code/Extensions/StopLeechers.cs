#region using

using System;
using System.Web;
using BlogEngine.Core.Web;
using BlogEngine.Core.Web.HttpHandlers;

#endregion

/// <summary>
/// Stops other websites in displaying your images on their own website.
/// </summary>
[Extension("Stops other websites in displaying your images on their own website")]
public class StopLeechers
{

  /// <summary>
  /// The default constructor. From here the different event handlers
  /// should be attached. The default constructor is mandatory and it will
  /// be called when the application starts and live in memory until the
  /// application stops.
  /// </summary>
  public StopLeechers()
  {
    ImageHandler.BeforeServing += new EventHandler<ImageHandler.FileHandlerEventArgs>(StopReferrers);
  }

  /// <summary>
  /// Checkes the referrer of the image serving request to determine if
  /// it comes from this blog or is included in another website. If it is,
  /// then we terminate the request and sends a '403 No access' status back.
  /// </summary>
  void StopReferrers(object sender, ImageHandler.FileHandlerEventArgs e)
  {
    HttpContext context = HttpContext.Current;
    if (context.Request.UrlReferrer != null)
    {
      if (context.Request.UrlReferrer.Host != context.Request.Url.Host)
      {
        context.Response.StatusCode = 403;
        context.Response.End();
      }
    }
  }
}
