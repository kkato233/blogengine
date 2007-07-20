#region Using

using System;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using BlogEngine.Core;

#endregion

public partial class admin_Pages_pages : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    base.MaintainScrollPositionOnPostBack = true;

    if (!Page.IsPostBack && !Page.IsCallback)
    {
      if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["id"].Length == 36)
      {
        Guid id = new Guid(Request.QueryString["id"]);
        BindPage(id);
      }
    }

    btnSave.Click += new EventHandler(btnSave_Click);
    btnUploadFile.Click += new EventHandler(btnUploadFile_Click);
    btnUploadImage.Click += new EventHandler(btnUploadImage_Click);
    Page.Title = Resources.labels.pages;
  }

  private void btnUploadImage_Click(object sender, EventArgs e)
  {
    Upload(BlogSettings.Instance.StorageLocation + "files/", txtUploadImage);
    string path = System.Web.VirtualPathUtility.ToAbsolute("~/");
    string img = string.Format("<img src=\"{0}image.axd?picture={1}\" alt=\"\" />", path, Server.UrlEncode(txtUploadImage.FileName));
    txtContent.Text += string.Format(img, txtUploadImage.FileName);
  }

  private void btnUploadFile_Click(object sender, EventArgs e)
  {
    Upload(BlogSettings.Instance.StorageLocation + "files/", txtUploadFile);

    string a = "<p><a href=\"{0}file.axd?file={1}\">{2}</a></p>";
    string text = txtUploadFile.FileName + " (" + SizeFormat(txtUploadFile.FileBytes.Length, "N") + ")";
    txtContent.Text += string.Format(a, Utils.RelativeWebRoot, Server.UrlEncode(txtUploadFile.FileName), text);
  }

  private void Upload(string virtualFolder, FileUpload control)
  {
    string folder = Server.MapPath(virtualFolder);
    control.PostedFile.SaveAs(folder + control.FileName);
  }

  private string SizeFormat(float size, string formatString)
  {
    if (size < 1024)
      return size.ToString(formatString) + " bytes";

    if (size < Math.Pow(1024, 2))
      return (size / 1024).ToString(formatString) + " kb";

    if (size < Math.Pow(1024, 3))
      return (size / Math.Pow(1024, 2)).ToString(formatString) + " mb";

    if (size < Math.Pow(1024, 4))
      return (size / Math.Pow(1024, 3)).ToString(formatString) + " gb";

    return size.ToString(formatString);
  }

  #region Event handlers

  private void btnSave_Click(object sender, EventArgs e)
  {
    if (!Page.IsValid)
      throw new InvalidOperationException("One or more validators are invalid.");

    Page page;
    if (Request.QueryString["id"] != null)
      page = BlogEngine.Core.Page.GetPage(new Guid(Request.QueryString["id"]));
    else
      page = new Page();

    page.Title = txtTitle.Text;
    page.Content = txtContent.Text;
    page.Description = txtDescription.Text;
    page.Keywords = txtKeyword.Text;
    page.Save();
    //HttpResponse.RemoveOutputCacheItem(VirtualPathUtility.ToAbsolute("~/") + "default.aspx");
    Response.Redirect(page.RelativeLink.ToString());
  }

  #endregion

  #region Data binding

  private void BindPage(Guid pageId)
  {
    Page page = BlogEngine.Core.Page.GetPage(pageId);
    txtTitle.Text = page.Title;
    txtContent.Text = page.Content;
    txtDescription.Text = page.Description;
    txtKeyword.Text = page.Keywords;
  }

  #endregion
}
