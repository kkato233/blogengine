#region Using

using System;
using System.IO;
using System.Web;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// Summary description for FileHandler
  /// </summary>
  public class ImageHandler : IHttpHandler
  {

    #region IHttpHandler Members

    /// <summary>
    /// 
    /// </summary>
    public bool IsReusable
    {
      get { return false; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
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