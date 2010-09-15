#region Using

using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BlogEngine.Core;

using Resources;

using Page = System.Web.UI.Page;

#endregion

/// <summary>
/// The admin_ pages_pages.
/// </summary>
public partial class admin_Pages_pages : Page, ICallbackEventHandler
{
    #region Constants and Fields

    /// <summary>
    /// The callback.
    /// </summary>
    private string callback;

    #endregion

    #region Implemented Interfaces

    #region ICallbackEventHandler

    /// <summary>
    /// Returns the results of a callback event that targets a control.
    /// </summary>
    /// <returns>The result of the callback.</returns>
    public string GetCallbackResult()
    {
        return this.callback;
    }

    /// <summary>
    /// Processes a callback event that targets a control.
    /// </summary>
    /// <param name="eventArgument">A string that represents an event argument to pass to the event handler.</param>
    public void RaiseCallbackEvent(string eventArgument)
    {
        this.callback = Utils.RemoveIllegalCharacters(eventArgument.Trim());
    }

    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.MaintainScrollPositionOnPostBack = true;

        if (!this.Page.IsPostBack && !this.Page.IsCallback)
        {
            if (!String.IsNullOrEmpty(this.Request.QueryString["id"]) && this.Request.QueryString["id"].Length == 36)
            {
                var id = new Guid(this.Request.QueryString["id"]);
                this.BindPage(id);
                this.BindParents(id);
            }
            else if (!String.IsNullOrEmpty(this.Request.QueryString["delete"]) &&
                     this.Request.QueryString["delete"].Length == 36)
            {
                var id = new Guid(this.Request.QueryString["delete"]);
                this.DeletePage(id);
            }
            else
            {
                this.BindParents(Guid.Empty);
            }

            this.BindPageList();
            this.Page.ClientScript.GetCallbackEventReference(this, "title", "ApplyCallback", "slug");
        }

        this.btnSave.Click += this.btnSave_Click;
        this.btnSave.Text = labels.savePage; // mono does not interpret the inline code correctly
        this.btnUploadFile.Click += this.btnUploadFile_Click;
        this.btnUploadImage.Click += this.btnUploadImage_Click;
        this.Page.Title = labels.pages;

        if (!Utils.IsMono)
        {
            this.Page.Form.DefaultButton = this.btnSave.UniqueID;
        }
    }

    /// <summary>
    /// The bind page.
    /// </summary>
    /// <param name="pageId">
    /// The page id.
    /// </param>
    private void BindPage(Guid pageId)
    {
        var page = BlogEngine.Core.Page.GetPage(pageId);
        this.txtTitle.Text = page.Title;
        this.txtContent.Text = page.Content;
        this.txtDescription.Text = page.Description;
        this.txtKeyword.Text = page.Keywords;
        this.txtSlug.Text = page.Slug;
        this.cbFrontPage.Checked = page.FrontPage;
        this.cbShowInList.Checked = page.ShowInList;
        this.cbPublished.Checked = page.Published;
    }

    /// <summary>
    /// The bind page list.
    /// </summary>
    private void BindPageList()
    {
        foreach (var page in BlogEngine.Core.Page.Pages.Where(page => !page.HasParentPage))
        {
            var li = new HtmlGenericControl("li");
            var a = new HtmlAnchor { HRef = string.Format("?id={0}", page.Id), InnerHtml = page.Title };

            HtmlAnchor delete;
            using (var text = new LiteralControl(string.Format(" ({0}) ", page.DateCreated.ToString("yyyy-dd-MM HH:mm"))))
            {
                const string DeleteText = "Are you sure you want to delete the page?";
                delete = new HtmlAnchor { InnerText = labels.delete };
                delete.Attributes["onclick"] = string.Format("if (confirm('{0}')){{location.href='?delete={1}'}}", DeleteText, page.Id);
                delete.HRef = "javascript:void(0);";
                delete.Style.Add(HtmlTextWriterStyle.FontWeight, "normal");

                li.Controls.Add(a);
                li.Controls.Add(text);
            }

            li.Controls.Add(delete);

            if (page.HasChildPages)
            {
                li.Controls.Add(this.BuildChildPageList(page));
            }

            li.Attributes.CssStyle.Remove("font-weight");
            li.Attributes.CssStyle.Add("font-weight", "bold");

            this.ulPages.Controls.Add(li);
        }

        this.divPages.Visible = true;
        this.aPages.InnerHtml = string.Format("{0} {1}", BlogEngine.Core.Page.Pages.Count, labels.pages);
    }

    /// <summary>
    /// The bind parents.
    /// </summary>
    /// <param name="pageId">
    /// The page id.
    /// </param>
    private void BindParents(Guid pageId)
    {
        foreach (var page in BlogEngine.Core.Page.Pages.Where(page => pageId != page.Id))
        {
            this.ddlParent.Items.Add(new ListItem(page.Title, page.Id.ToString()));
        }

        this.ddlParent.Items.Insert(0, string.Format("-- {0} --", labels.noParent));
        if (pageId != Guid.Empty)
        {
            var parent = BlogEngine.Core.Page.GetPage(pageId);
            if (parent != null)
            {
                this.ddlParent.SelectedValue = parent.Parent.ToString();
            }
        }
    }

    /// <summary>
    /// The build child page list.
    /// </summary>
    /// <param name="page">
    /// The page.
    /// </param>
    /// <returns>
    /// </returns>
    private HtmlGenericControl BuildChildPageList(BlogEngine.Core.Page page)
    {
        var ul = new HtmlGenericControl("ul");
        foreach (var cPage in BlogEngine.Core.Page.Pages.FindAll(p => p.Parent == page.Id))
        {
            var cLi = new HtmlGenericControl("li");
            cLi.Attributes.CssStyle.Add("font-weight", "normal");
            var cA = new HtmlAnchor { HRef = string.Format("?id={0}", cPage.Id), InnerHtml = cPage.Title };

            var cText = new LiteralControl(string.Format(" ({0}) ", cPage.DateCreated.ToString("yyyy-dd-MM HH:mm")));

            const string DeleteText = "Are you sure you want to delete the page?";
            var delete = new HtmlAnchor { InnerText = labels.delete };
            delete.Attributes["onclick"] = "if (confirm('" + DeleteText + "')){location.href='?delete=" + cPage.Id +
                                           "'}";
            delete.HRef = "javascript:void(0);";
            delete.Style.Add(HtmlTextWriterStyle.FontWeight, "normal");

            cLi.Controls.Add(cA);
            cLi.Controls.Add(cText);
            cLi.Controls.Add(delete);

            if (cPage.HasChildPages)
            {
                cLi.Attributes.CssStyle.Remove("font-weight");
                cLi.Attributes.CssStyle.Add("font-weight", "bold");
                cLi.Controls.Add(this.BuildChildPageList(cPage));
            }

            ul.Controls.Add(cLi);
        }

        return ul;
    }

    /// <summary>
    /// The delete page.
    /// </summary>
    /// <param name="pageId">
    /// The page id.
    /// </param>
    private void DeletePage(Guid pageId)
    {
        var page = BlogEngine.Core.Page.GetPage(pageId);
        if (page != null)
        {
            this.ResetParentPage(page);
            page.Delete();
            page.Save();
            this.Response.Redirect("pages.aspx");
        }
    }

    /// <summary>
    /// The reset parent page.
    /// </summary>
    /// <param name="page">
    /// The page.
    /// </param>
    private void ResetParentPage(BlogEngine.Core.Page page)
    {
        foreach (var child in BlogEngine.Core.Page.Pages.Where(child => page.Id == child.Parent))
        {
            child.Parent = Guid.Empty;
            child.Save();
            this.ResetParentPage(child);
        }
    }

    /// <summary>
    /// The size format.
    /// </summary>
    /// <param name="size">
    /// The size.
    /// </param>
    /// <param name="formatString">
    /// The format string.
    /// </param>
    /// <returns>
    /// The size format.
    /// </returns>
    private string SizeFormat(float size, string formatString)
    {
        if (size < 1024)
        {
            return size.ToString(formatString) + " bytes";
        }

        if (size < Math.Pow(1024, 2))
        {
            return (size / 1024).ToString(formatString) + " kb";
        }

        if (size < Math.Pow(1024, 3))
        {
            return (size / Math.Pow(1024, 2)).ToString(formatString) + " mb";
        }

        if (size < Math.Pow(1024, 4))
        {
            return (size / Math.Pow(1024, 3)).ToString(formatString) + " gb";
        }

        return size.ToString(formatString);
    }

    /// <summary>
    /// The upload.
    /// </summary>
    /// <param name="virtualFolder">
    /// The virtual folder.
    /// </param>
    /// <param name="control">
    /// The control.
    /// </param>
    /// <param name="fileName">
    /// The file name.
    /// </param>
    private void Upload(string virtualFolder, FileUpload control, string fileName)
    {
        var folder = this.Server.MapPath(virtualFolder);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        control.PostedFile.SaveAs(folder + fileName);
    }

    /// <summary>
    /// Handles the Click event of the btnSave control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.Page.IsValid)
        {
            throw new InvalidOperationException("One or more validators are invalid.");
        }

        BlogEngine.Core.Page page;
        page = this.Request.QueryString["id"] != null ? BlogEngine.Core.Page.GetPage(new Guid(this.Request.QueryString["id"])) : new BlogEngine.Core.Page();

        if (string.IsNullOrEmpty(this.txtContent.Text))
        {
            this.txtContent.Text = "[No text]";
        }

        page.Title = this.txtTitle.Text;
        page.Content = this.txtContent.Text;
        page.Description = this.txtDescription.Text;
        page.Keywords = this.txtKeyword.Text;

        if (this.cbFrontPage.Checked)
        {
            foreach (var otherPage in BlogEngine.Core.Page.Pages.Where(otherPage => otherPage.FrontPage))
            {
                otherPage.FrontPage = false;
                otherPage.Save();
            }
        }

        page.FrontPage = this.cbFrontPage.Checked;
        page.ShowInList = this.cbShowInList.Checked;
        page.Published = this.cbPublished.Checked;

        if (!string.IsNullOrEmpty(this.txtSlug.Text))
        {
            page.Slug = Utils.RemoveIllegalCharacters(this.txtSlug.Text.Trim());
        }

        page.Parent = this.ddlParent.SelectedIndex != 0 ? new Guid(this.ddlParent.SelectedValue) : Guid.Empty;

        page.Save();

        this.Response.Redirect(page.RelativeLink);
    }

    /// <summary>
    /// Handles the Click event of the btnUploadFile control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void btnUploadFile_Click(object sender, EventArgs e)
    {
        var relativeFolder = DateTime.Now.Year.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Month +
                             Path.DirectorySeparatorChar;
        var folder = BlogSettings.Instance.StorageLocation + "files" + Path.DirectorySeparatorChar;
        var fileName = this.txtUploadFile.FileName;
        this.Upload(folder + relativeFolder, this.txtUploadFile, fileName);

        var a = "<p><a href=\"{0}file.axd?file={1}\">{2}</a></p>";
        var text = this.txtUploadFile.FileName + " (" + this.SizeFormat(this.txtUploadFile.FileBytes.Length, "N") + ")";
        this.txtContent.Text += string.Format(
            a, Utils.RelativeWebRoot, this.Server.UrlEncode(relativeFolder.Replace("\\", "/") + fileName), text);
    }

    /// <summary>
    /// Handles the Click event of the btnUploadImage control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void btnUploadImage_Click(object sender, EventArgs e)
    {
        var relativeFolder = DateTime.Now.Year.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Month +
                             Path.DirectorySeparatorChar;
        var folder = BlogSettings.Instance.StorageLocation + "files" + Path.DirectorySeparatorChar;
        var fileName = this.txtUploadImage.FileName;
        this.Upload(folder + relativeFolder, this.txtUploadImage, fileName);

        var path = Utils.RelativeWebRoot;
        var img = string.Format(
            "<img src=\"{0}image.axd?picture={1}\" alt=\"\" />",
            path,
            this.Server.UrlEncode(relativeFolder.Replace("\\", "/") + fileName));
        this.txtContent.Text += img;
    }

    #endregion
}