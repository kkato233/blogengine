#region Using

using System;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;
using BlogEngine.Core;

#endregion

public partial class admin_entry : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
{
  protected void Page_Load(object sender, EventArgs e)
  {
    this.MaintainScrollPositionOnPostBack = true;
    if (!Page.IsPostBack && !Page.IsCallback)
    {
      BindCategories();
      BindUsers();
      BindDrafts();

      Page.Title = Resources.labels.add_Entry;
      Page.ClientScript.GetCallbackEventReference(this, "title", "ApplyCallback", "slug");

      if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["id"].Length == 36)
      {
        Guid id = new Guid(Request.QueryString["id"]);
        Page.Title = "Edit post";
        BindPost(id);
      }
      else
      {
        ddlAuthor.SelectedValue = Page.User.Identity.Name;
        txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        cbEnableComments.Checked = BlogSettings.Instance.IsCommentsEnabled;
        txtContent.Text = (string)(Session["autosave"] ?? string.Empty);
      }
      
      cbEnableComments.Enabled = BlogSettings.Instance.IsCommentsEnabled;
      Page.Form.DefaultButton = btnSave.UniqueID;
    }

    btnSave.Click += new EventHandler(btnSave_Click);
    btnCategory.Click += new EventHandler(btnCategory_Click);
    btnUploadFile.Click += new EventHandler(btnUploadFile_Click);
    btnUploadImage.Click += new EventHandler(btnUploadImage_Click);
    valExist.ServerValidate += new ServerValidateEventHandler(valExist_ServerValidate);
  }

  private void valExist_ServerValidate(object source, ServerValidateEventArgs args)
  {
    args.IsValid = true;

    foreach (string cat in CategoryDictionary.Instance.Values)
    {
      if (cat.Equals(txtCategory.Text.Trim(), StringComparison.OrdinalIgnoreCase))
        args.IsValid = false;
    }
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

  /// <summary>
  /// Creates and saves a new category
  /// </summary>
  private void btnCategory_Click(object sender, EventArgs e)
  {
    if (Page.IsValid)
    {
      Guid id = CategoryDictionary.Instance.Add(txtCategory.Text);
      CategoryDictionary.Instance.Save();
      ListItem item = new ListItem(txtCategory.Text, id.ToString());
      item.Selected = true;
      cblCategories.Items.Add(item);
    }
  }

  /// <summary>
  /// Saves the post
  /// </summary>
  private void btnSave_Click(object sender, EventArgs e)
  {
    if (!Page.IsValid)
      throw new InvalidOperationException("One or more validators are invalid.");

    Post post;
    if (Request.QueryString["id"] != null)
      post = Post.GetPost(new Guid(Request.QueryString["id"]));
    else
      post = new Post();

    // If the title ends with a period, IIS will not send it to the ASP.NET engine.
    if (txtTitle.Text.EndsWith("."))
      txtTitle.Text = txtTitle.Text.Substring(0, txtTitle.Text.Length - 1);

    if (string.IsNullOrEmpty(txtContent.Text))
      txtContent.Text = "[No text]";

    post.DateCreated = DateTime.Parse(txtDate.Text);
    post.DateModified = DateTime.Now;
    post.Author = ddlAuthor.SelectedValue;
    post.Title = txtTitle.Text.Trim();
    post.Content = txtContent.Text;
    post.Description = txtDescription.Text.Trim();
    post.IsPublished = cbPublish.Checked;
    post.IsCommentsEnabled = cbEnableComments.Checked;

    if (!string.IsNullOrEmpty(txtSlug.Text))
      post.Slug = txtSlug.Text.Trim();

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
    if (!Request.IsLocal && post.Content.ToLowerInvariant().Contains("http")  && post.IsPublished)
    {
      ThreadStart threadStart = delegate { Ping(post); };
      Thread thread = new Thread(threadStart);
      thread.IsBackground = true;
      thread.Start();
    }

    Session.Remove("autosave");
    Response.Redirect(post.RelativeLink.ToString());
  }

  private void Ping(object stateInfo)
  {
    System.Threading.Thread.Sleep(2000);
    Post post = (Post)stateInfo;
    new BlogEngine.Core.Ping.PingService().Send();
    BlogEngine.Core.Ping.Manager.Send(post);
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
    txtDate.Text = post.DateCreated.ToString("yyyy-MM-dd HH:mm");
    cbEnableComments.Checked = post.IsCommentsEnabled;
    cbPublish.Checked = post.IsPublished;
    txtSlug.Text = post.Slug;

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

  private void BindDrafts()
  {
    Guid id = Guid.Empty;
    if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["id"].Length == 36)
    {
      id = new Guid(Request.QueryString["id"]);
    }

    int counter = 0;

    foreach (Post post in Post.Posts)
    {
      if (!post.IsPublished && post.Id  != id)
      {
        HtmlGenericControl li = new HtmlGenericControl();
        HtmlAnchor a = new HtmlAnchor();
        a.HRef = "?id=" + post.Id.ToString();
        a.InnerHtml = post.Title;

        System.Web.UI.LiteralControl text = new System.Web.UI.LiteralControl(" by " + post.Author + " (" + post.DateCreated.ToString("yyyy-dd-MM HH:mm") + ")");

        li.Controls.Add(a);
        li.Controls.Add(text);        
        ulDrafts.Controls.Add(li);
        counter++;
      }
    }

    if (counter > 0)
    {
      divDrafts.Visible = true;
      aDrafts.InnerHtml = string.Format(Resources.labels.thereAreXDrafts, counter);
    }
  }

  #endregion


  #region ICallbackEventHandler Members

  private string _Callback;

  public string GetCallbackResult()
  {
    return _Callback;
  }

  public void RaiseCallbackEvent(string eventArgument)
  {
    if (eventArgument.StartsWith("_autosave"))
    {
      Session["autosave"] = eventArgument.Replace("_autosave", string.Empty);
    }
    else
    {
    _Callback = Utils.RemoveIlegalCharacters(eventArgument.Trim());
    }
  }

  #endregion
}
