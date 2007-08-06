#region Using

using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Web.Caching;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// Removes whitespace in all stylesheets added to the 
  /// header of the HTML document in site.master. 
  /// </summary>
  public class CssHandler : IHttpHandler
  {
    /// <summary>
    /// Enables processing of HTTP Web requests by a custom 
    /// HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
    /// </summary>
    /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
    /// references to the intrinsic server objects 
    /// (for example, Request, Response, Session, and Server) used to service HTTP requests.
    /// </param>
    public void ProcessRequest(HttpContext context)
    {
      string file = context.Server.MapPath(Utils.RelativeWebRoot + "themes/" + BlogSettings.Instance.Theme + "/" + context.Request.QueryString["name"]);
      ReduceCSS(file, context);
      SetHeaders(file, context);
      Compress(context);
    }

    /// <summary>
    /// Removes all unwanted text from the CSS file,
    /// including comments and whitespace.
    /// </summary>
    private void ReduceCSS(string file, HttpContext context)
    {
      if (!file.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
      {
        throw new System.Security.SecurityException("No access");
      }

      string body;

      using (StreamReader reader = new StreamReader(file))
      {
        body = reader.ReadToEnd();
      }

      body += DateTime.Now.ToString();
      body = body.Replace("  ", String.Empty);
      body = body.Replace(Environment.NewLine, String.Empty);
      body = body.Replace("\t", string.Empty);
      body = body.Replace(" {", "{");
      body = body.Replace(" :", ":");
      body = body.Replace(": ", ":");
      body = body.Replace(", ", ",");
      body = body.Replace("; ", ";");
      body = body.Replace(";}", "}");
      body = Regex.Replace(body, @"/\*[^\*]*\*+([^/\*]*\*+)*/", "$1");
      body = Regex.Replace(body, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])", String.Empty);

      context.Response.Write(body);
    }

    /// <summary>
    /// This will make the browser and server keep the output
    /// in its cache and thereby improve performance.
    /// </summary>
    private void SetHeaders(string file, HttpContext context)
    {
      context.Response.ContentType = "text/css";
      // Server-side caching 
      context.Response.AddFileDependency(file);
      context.Response.Cache.VaryByParams["name"] = true;
      context.Response.Cache.SetValidUntilExpires(true);
      // Client-side caching
      context.Response.Cache.SetExpires(DateTime.Now.AddDays(3));
      context.Response.Cache.SetLastModifiedFromFileDependencies();
      context.Response.Cache.SetCacheability(HttpCacheability.Public);
    }

    #region Compression

    private const string GZIP = "gzip";
    private const string DEFLATE = "deflate";

    private void Compress(HttpContext context)
    {
      if (IsEncodingAccepted(GZIP))
      {
        context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
        SetEncoding(GZIP);
      }
      else if (IsEncodingAccepted(DEFLATE))
      {
        context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
        SetEncoding(DEFLATE);
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