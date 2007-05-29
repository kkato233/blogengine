#region Using

using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
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
    }

    /// <summary>
    /// Removes all unwanted text from the CSS file,
    /// including comments and whitespace.
    /// </summary>
    private void ReduceCSS(string file, HttpContext context)
    {
      FileInfo fi = new FileInfo(file);
      if (!fi.Extension.Equals(".css", StringComparison.OrdinalIgnoreCase))
      {
        throw new System.Security.SecurityException("No access");
      }

      string body = fi.OpenText().ReadToEnd();
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
      context.Response.Cache.SetETagFromFileDependencies();
      context.Response.Cache.SetLastModifiedFromFileDependencies();
      context.Response.Cache.SetCacheability(HttpCacheability.Public);
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