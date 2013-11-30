using App_Code;
using BlogEngine.Core;
using BlogEngine.Core.API.BlogML;
using BlogEngine.Core.Providers;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

public class UploadController : ApiController
{
    public HttpResponseMessage Post(string action)
    {
        WebUtils.CheckRightsForAdminPostPages(false);

        if (action == "importBlogML")
            return ImportBlogML();

        HttpPostedFile file = HttpContext.Current.Request.Files[0];
        if (file != null && file.ContentLength > 0)
        {
            var dirName = string.Format("/{0}/{1}", DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));
            var dir = BlogService.GetDirectory(dirName);
            var retUrl = "";

            if (action == "image")
            {
                var uploaded = BlogService.UploadFile(file.InputStream, file.FileName, dir, true);
                retUrl = uploaded.FileDownloadPath.Replace("\"", "");
                if (retUrl.StartsWith("/"))
                    retUrl = retUrl.Substring(1);
                retUrl = Utils.RelativeWebRoot + retUrl;
                return Request.CreateResponse(HttpStatusCode.Created, retUrl);
            }
            if (action == "file")
            {
                var uploaded = BlogService.UploadFile(file.InputStream, file.FileName, dir, true);
                retUrl = uploaded.FileDownloadPath + "|" + file.FileName + " (" + BytesToString(uploaded.FileSize) + ")";
                retUrl = retUrl.Replace("\"", "");
                if (retUrl.StartsWith("/"))
                    retUrl = retUrl.Substring(1);
                retUrl = Utils.RelativeWebRoot + retUrl;
                return Request.CreateResponse(HttpStatusCode.Created, retUrl);
            }
            if (action == "video")
            {
                // default media folder
                var mediaFolder = "media";

                // get the mediaplayer extension and use it's folder
                var mediaPlayerExtension = BlogEngine.Core.Web.Extensions.ExtensionManager.GetExtension("MediaElementPlayer");
                mediaFolder = mediaPlayerExtension.Settings[0].GetSingleValue("folder");

                var folder = Utils.ApplicationRelativeWebRoot + mediaFolder + "/";
                var fileName = file.FileName;

                UploadVideo(folder, file, fileName);

                return Request.CreateResponse(HttpStatusCode.Created, fileName);
            }
        }
        return Request.CreateResponse(HttpStatusCode.BadRequest);
    }

    #region Private methods

    HttpResponseMessage ImportBlogML()
    {
        HttpPostedFile file = HttpContext.Current.Request.Files[0];
        if (file != null && file.ContentLength > 0)
        {
            var reader = new BlogReader();
            var rdr = new StreamReader(file.InputStream);
            reader.XmlData = rdr.ReadToEnd();

            if (reader.Import())
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        return Request.CreateResponse(HttpStatusCode.InternalServerError);
    }

    static String BytesToString(long byteCount)
    {
        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
        if (byteCount == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(byteCount);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }

    private void UploadVideo(string virtualFolder, HttpPostedFile file, string fileName)
    {
        var folder = HttpContext.Current.Server.MapPath(virtualFolder);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        file.SaveAs(folder + fileName);
    }

    #endregion
}