#region Using

using System;
using System.Web;
using System.IO.Compression;

#endregion

namespace BlogEngine.Core.Web.HttpModules
{
  /// <summary>
  /// Compresses the output using standard gzip/deflate.
  /// </summary>
  public class CompressionModule : IHttpModule
  {

    #region IHttpModule Members

    /// <summary>
    /// Disposes of the resources (other than memory) used by the module 
    /// that implements <see cref="T:System.Web.IHttpModule"></see>.
    /// </summary>
    void IHttpModule.Dispose()
    {
      // Nothing to dispose; 
    }

    /// <summary>
    /// Initializes a module and prepares it to handle requests.
    /// </summary>
    /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> 
    /// that provides access to the methods, properties, and events common to 
    /// all application objects within an ASP.NET application.
    /// </param>
    void IHttpModule.Init(HttpApplication context)
    {
      if (BlogSettings.Instance.EnableHttpCompression)
        context.BeginRequest += new EventHandler(context_BeginRequest);
    }

    #endregion

    #region Compression

    private const string GZIP = "gzip";
    private const string DEFLATE = "deflate";

    /// <summary>
    /// Handles the BeginRequest event of the context control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void context_BeginRequest(object sender, EventArgs e)
    {
      HttpApplication app = sender as HttpApplication;
      if (app.Request.Url.OriginalString.ToUpperInvariant().Contains(".ASPX"))
      {
        if (IsEncodingAccepted(DEFLATE))
        {
          app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
          SetEncoding(DEFLATE);
        }
        else if (IsEncodingAccepted(GZIP))
        {
          app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
          SetEncoding(GZIP);
        }
      }
    }

    /// <summary>
    /// Checks the request headers to see if the specified
    /// encoding is accepted by the client.
    /// </summary>
    private static bool IsEncodingAccepted(string encoding)
    {
      return HttpContext.Current.Request.Headers["Accept-encoding"] != null && HttpContext.Current.Request.Headers["Accept-encoding"].Contains(encoding);
    }

    /// <summary>
    /// Adds the specified encoding to the response headers.
    /// </summary>
    /// <param name="encoding"></param>
    private static void SetEncoding(string encoding)
    {
      HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
    }

    #endregion

  }
}