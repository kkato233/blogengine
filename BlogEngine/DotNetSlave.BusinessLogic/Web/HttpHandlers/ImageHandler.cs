#region Using

using System;
using System.IO;
using System.Web;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// The ImageHanlder serves all images that is uploaded from
  /// the admin pages. 
  /// </summary>
  /// <remarks>
  /// By using a HttpHandler to serve images, it is very easy
  /// to add the capability to stop bandwidth leeching or
  /// to create a statistics analysis feature upon it.
  /// </remarks>
  public class ImageHandler : IHttpHandler
  {

    #region IHttpHandler Members

    /// <summary>
    /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
    /// </summary>
    /// <value></value>
    /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
    public bool IsReusable
    {
      get { return false; }
    }

    /// <summary>
    /// Enables processing of HTTP Web requests by a custom HttpHandler that 
    /// implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
    /// </summary>
    /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object 
    /// that provides references to the intrinsic server objects 
    /// (for example, Request, Response, Session, and Server) used to service HTTP requests.
    /// </param>
    public void ProcessRequest(HttpContext context)
    {
      OnBeforeServing();

      if (!string.IsNullOrEmpty(context.Request.QueryString["picture"]))
      {
        string fileName = context.Request.QueryString["picture"];
        string folder = BlogSettings.Instance.StorageLocation + "files/";
        if (File.Exists(context.Server.MapPath(folder) + fileName))
        {
          OnImageServing();
          int index = fileName.LastIndexOf(".") + 1;
          string extension = fileName.Substring(index).ToUpperInvariant();
          
          // Fix for IE not handling jpg image types
          if (extension == "JPG")
            context.Response.ContentType = "image/jpeg";
          else
            context.Response.ContentType = "image/" + extension;

          context.Response.Cache.SetCacheability(HttpCacheability.Public);
          context.Response.Cache.SetETag(fileName);
          context.Server.Transfer(folder + fileName, false);
        }
        else
        {
          OnBadRequest();
          context.Response.Status = "404 Bad Request";
        }
      }
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when the requested file does not exist;
    /// </summary>
    public static event EventHandler<FileHandlerEventArgs> BeforeServing;
    private static void OnBeforeServing()
    {
      if (BeforeServing != null)
      {
        BeforeServing(null, new FileHandlerEventArgs(HttpContext.Current));
      }
    }

    /// <summary>
    /// Occurs when a file is served;
    /// </summary>
    public static event EventHandler<FileHandlerEventArgs> ImageServing;
    private static void OnImageServing()
    {
      if (ImageServing != null)
      {
        ImageServing(null, new FileHandlerEventArgs(HttpContext.Current));
      }
    }

    /// <summary>
    /// Occurs when the requested file does not exist;
    /// </summary>
    public static event EventHandler<FileHandlerEventArgs> BadRequest;
    private static void OnBadRequest()
    {
      if (BadRequest != null)
      {
        BadRequest(null, new FileHandlerEventArgs(HttpContext.Current));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileHandlerEventArgs : EventArgs
    {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="context"></param>
      public FileHandlerEventArgs(HttpContext context)
      {
        FileName = context.Request.QueryString["file"];
        UserAgent = context.Request.UserAgent;
        IpAddress = context.Request.UserHostAddress;
      }
        
      /// <summary>
      /// 
      /// </summary>
      public string FileName;
      /// <summary>
      /// 
      /// </summary>
      public string UserAgent;
      /// <summary>
      /// 
      /// </summary>
      public string IpAddress;
    }

    #endregion

  }
}