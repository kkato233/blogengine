using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlogEngine.Core.Providers;
using BlogEngine.Core;
using BlogEngine.Core.FileSystem;
public partial class Playground : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var directory = Blog.CurrentInstance.RootFileStore;
        //directory.CreateSubdirectory("2012");
        //directory.DeleteSubDirectory("2012");
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        var now = DateTime.Today;
        var directory = Blog.CurrentInstance.RootFileStore;
        directory = directory.CreateSubDirectory("2011");
        directory = directory.CreateSubDirectory("07");
        File file = BlogService.UploadFile(fileUpload.PostedFile.InputStream, fileUpload.PostedFile.FileName, directory, true);
        hprLink.NavigateUrl = file.FileDownloadPath;
        hprLink.Text = file.FileDescription;
        if (file.IsImage)
        {
            img.Visible = true;
            img.ImageUrl = file.AsImage.ImageUrl;
        }
        else
            img.Visible = false;

    }
}