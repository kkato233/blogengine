using System;
using System.IO;
using System.Web;

namespace BlogEngine.Core.Web.HttpHandlers
{
  /// <summary>
  /// Summary description for FileHandler
  /// </summary>
  public class FileHandler : IHttpHandler
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

      if (!string.IsNullOrEmpty(context.Request.QueryString["file"]))
      {
        string fileName = context.Request.QueryString["file"];
        string folder = BlogSettings.Instance.StorageLocation + "/files/";
        if (File.Exists(context.Server.MapPath(folder) + fileName))
        {
          OnFileServing();
          context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + context.Server.UrlEncode(fileName));
          context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
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
    public static event EventHandler<FileHandlerEventArgs> FileServing;

    private static void OnFileServing()
    {
      if (FileServing != null)
      {
        FileServing(null, new FileHandlerEventArgs(HttpContext.Current));
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