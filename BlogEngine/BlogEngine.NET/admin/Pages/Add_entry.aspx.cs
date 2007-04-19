#region Using

using System;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;
using DotNetSlave.BlogEngine.BusinessLogic;
using FreeTextBoxControls;

#endregion

public partial class admin_entry : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    this.MaintainScrollPositionOnPostBack = true;
    if (!Page.IsPostBack && !Page.IsCallback)
    {
      BindCategories();
      BindUsers();
      AddCodeToolbarItem();

      if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["id"].Length == 36)
      {
        Guid id = new Guid(Request.QueryString["id"]);
        BindPost(id);
      }
      else
      {
        ddlAuthor.SelectedValue = Page.User.Identity.Name;
        txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        cbEnableComments.Checked = BlogSettings.Instance.IsCommentsEnabled;
      }

      cbEnableComments.Enabled = BlogSettings.Instance.IsCommentsEnabled;
    }

    btnSave.Click += new EventHandler(btnSave_Click);
    btnCategory.Click += new EventHandler(btnCategory_Click);
    btnUploadFile.Click += new EventHandler(btnUploadFile_Click);
    btnUploadImage.Click += new EventHandler(btnUploadImage_Click);
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
    string path = System.Web.VirtualPathUtility.ToAbsolute("~/");
    txtContent.Text += string.Format(a, path, Server.UrlEncode(txtUploadFile.FileName), text);
  }

  private void Upload(string virtualFolder, FileUpload control)
  {
    string folder = Server.MapPath(virtualFolder);
    control.PostedFile.SaveAs(folder + control.FileName);
  }

  private void AddCodeToolbarItem()
  {
    Toolbar myToolbar = new Toolbar();

    ToolbarButton myButton = new ToolbarButton("Insert Code", "insertCode", "csharp");

    StringBuilder scriptBlock = new StringBuilder();
    scriptBlock.AppendFormat("var codescript = '{0}';", txtContent.SupportFolder + "ftb.insertcode.aspx");
    scriptBlock.Append("code = showModalDialog(codescript,window,'dialogWidth:400px; dialogHeight:500px;help:0;status:0;resizeable:1;');");
    scriptBlock.Append("if (code  != null) {");
    scriptBlock.Append("	this.ftb.InsertHtml(code);");
    scriptBlock.Append("}");

    myButton.ScriptBlock = scriptBlock.ToString();
    myToolbar.Items.Add(myButton);
    txtContent.Toolbars.Add(myToolbar);
  }

  private string SizeFormat(float size, string formatString)
  {
    if (size < 1024)
      return size.ToString(formatString) + "bytes";

    if (size < Math.Pow(1024, 2))
      return (size / 1024).ToString(formatString) + " kb";

    if (size < Math.Pow(1024, 3))
      return (size / Math.Pow(1024, 2)).ToString(formatString) + " mb";

    if (size < Math.Pow(1024, 4))
      return (size / Math.Pow(1024, 3)).ToString(formatString) + " gb";

    return size.ToString(formatString);
  }

  #region Event handlers

  private void btnCategory_Click(object sender, EventArgs e)
  {
    if (!Page.IsValid)
      throw new InvalidOperationException("One or more validators are invalid.");

    Guid id = CategoryDictionary.Instance.Add(txtCategory.Text);
    CategoryDictionary.Instance.Save();
    ListItem item = new ListItem(txtCategory.Text, id.ToString());
    item.Selected = true;
    cblCategories.Items.Add(item);
  }

  private void btnSave_Click(object sender, EventArgs e)
  {
    if (!Page.IsValid)
      throw new InvalidOperationException("One or more validators are invalid.");

    Post post;
    if (Request.QueryString["id"] != null)
      post = Post.GetPost(new Guid(Request.QueryString["id"]));
    else
      post = new Post();

    post.DateCreated = DateTime.Parse(txtDate.Text);
    post.Author = ddlAuthor.SelectedValue;
    post.Title = txtTitle.Text;
    post.Content = txtContent.Xhtml;
    post.Description = txtDescription.Text;
    post.IsPublished = cbPublish.Checked;
    post.IsCommentsEnabled = cbEnableComments.Checked;
    post.Categories.Clear();

    foreach (ListItem item in cblCategories.Items)
    {
      if (item.Selected)
        post.Categories.Add(new Guid(item.Value));
    }

    post.Tags.Clear();
    if (txtTags.Text.Trim().Length > 0)
    {
      string[] tags = txtTags.Text.Split(',');
      foreach (string tag in tags)
      {
        post.Tags.Add(tag.Trim().ToLowerInvariant());
      }
    }

    post.Save();
    if (!Request.IsLocal)
    {
      new PingService().Send();
      Pingback.Send(post);
    }
    //HttpResponse.RemoveOutputCacheItem(VirtualPathUtility.ToAbsolute("~/") + "default.aspx");
    Response.Redirect(post.RelativeLink.ToString());
  }

  #endregion

  #region Data binding

  private void BindCategories()
  {
    foreach (Guid key in CategoryDictionary.Instance.Keys)
    {
      cblCategories.Items.Add(new ListItem(CategoryDictionary.Instance[key], key.ToString()));
    }
  }

  private void BindPost(Guid postId)
  {
    Post post = Post.GetPost(postId);
    txtTitle.Text = post.Title;
    txtContent.Text = post.Content;
    txtDescription.Text = post.Description;
    ddlAuthor.SelectedValue = post.Author;
    txtDate.Text = post.DateCreated.ToString("yyyy-MM-dd");
    cbEnableComments.Checked = post.IsCommentsEnabled;
    cbPublish.Checked = post.IsPublished;

    foreach (Guid key in post.Categories)
    {
      ListItem item = cblCategories.Items.FindByValue(key.ToString());
      if (item != null)
        item.Selected = true;
    }
    
    string[] tags = new string[post.Tags.Count];
    for (int i = 0; i < post.Tags.Count; i++)
    {
      tags[i] = post.Tags[i];
    }
    txtTags.Text = string.Join(",", tags);
  }

  private void BindUsers()
  {
    foreach (MembershipUser user in Membership.GetAllUsers())
    {
      ddlAuthor.Items.Add(user.UserName);
    }
  }

  #endregion

}
