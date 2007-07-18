#region Using

using System;
using System.Text;
using System.Web;
using System.Globalization;
using System.Threading;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;

#endregion

public partial class User_controls_CommentView : System.Web.UI.UserControl, ICallbackEventHandler
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (Post == null)
      Response.Redirect("~/");

    if (!Page.IsPostBack && !Page.IsCallback)
    {
      if (Request.QueryString["deletecomment"] != null)
        DeleteComment();

      string path = "~/themes/" + BlogSettings.Instance.Theme + "/commentview.ascx";
      foreach (Comment comment in Post.Comments)
      {
        CommentViewBase control = (CommentViewBase)LoadControl(path);
        control.Comment = comment;
        control.Post = Post;
        phComments.Controls.Add(control);
      }

      if (!BlogSettings.Instance.IsCommentsEnabled || !Post.IsCommentsEnabled || (BlogSettings.Instance.DaysCommentsAreEnabled > 0 && Post.DateCreated.AddDays(BlogSettings.Instance.DaysCommentsAreEnabled) < DateTime.Now.Date))
      {
        phAddComment.Visible = false;
        lbCommentsDisabled.Visible = true;
      }
      else
      {
        BindCountries();
        GetCookie();
        BindLivePreview();
      }
    }

    InititializeCaptcha();
    btnSave.Click += new EventHandler(btnSave_Click);
  }

  private void DeleteComment()
  {
    foreach (Comment comment in Post.Comments)
    {
      if (comment.Id == new Guid(Request.QueryString["deletecomment"]))
      {
        Response.Write(Request.Url);
        Post.RemoveComment(comment);

        int index = Request.RawUrl.IndexOf("?");
        string url = Request.RawUrl.Substring(0, index);
        Response.Redirect(url, true);
      }
    }
  }

  private void btnSave_Click(object sender, EventArgs e)
  {
    if (!IsCaptchaValid || !Page.IsValid)
      Response.Redirect(Post.RelativeLink.ToString(), true);

    SaveComment();
    SetCookie(txtName.Text, txtEmail.Text, txtWebsite.Text, ddlCountry.SelectedValue);
    SendMail(txtName.Text, txtEmail.Text, txtContent.Text);
    Response.Redirect(Post.RelativeLink.ToString() + "?comment=1#addcomment", true);
  }

  private void SaveComment()
  {
    Comment comment = new Comment();
    comment.Id = Guid.NewGuid();
    comment.Author = txtName.Text;
    comment.Email = txtEmail.Text;
    comment.IP = Request.UserHostAddress;
    comment.Country = ddlCountry.SelectedValue;
    comment.Content = Server.HtmlEncode(txtContent.Text);
    comment.DateCreated = DateTime.Now;
    comment.Post = Post;
    if (txtWebsite.Text.Trim().Length > 0)
    {
      if (!txtWebsite.Text.ToLowerInvariant().Contains("://"))
        txtWebsite.Text = "http://" + txtWebsite.Text;

      Uri website;
      if (Uri.TryCreate(txtWebsite.Text, UriKind.Absolute, out website))
        comment.Website = website;
    }

    Post.AddComment(comment);
  }

  public void BindCountries()
  {
    System.Collections.Specialized.StringDictionary dic = new System.Collections.Specialized.StringDictionary();
    System.Collections.Generic.List<string> col = new System.Collections.Generic.List<string>();

    foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
    {
      RegionInfo ri = new RegionInfo(ci.Name);
      if (!dic.ContainsKey(ri.EnglishName))
        dic.Add(ri.EnglishName, ri.TwoLetterISORegionName.ToLowerInvariant());

      if (!col.Contains(ri.EnglishName))
        col.Add(ri.EnglishName);
    }

    col.Sort();

    ddlCountry.Items.Add(new ListItem("[Not specified]", ""));
    foreach (string key in col)
    {
      ddlCountry.Items.Add(new ListItem(key, dic[key]));
    }

    if (ddlCountry.SelectedIndex == 0 && Request.UserLanguages != null && Request.UserLanguages[0].Length == 5)
    {
      ddlCountry.SelectedValue = Request.UserLanguages[0].Substring(3);
    }
  }

  private void BindLivePreview()
  {
    string path = "~/themes/" + BlogSettings.Instance.Theme + "/commentview.ascx";
    CommentViewBase control = (CommentViewBase)LoadControl(path);
    Comment comment = new Comment();
    comment.Content = string.Empty;
    comment.DateCreated = DateTime.Now;
    comment.IP = Request.UserHostAddress;

    if (!string.IsNullOrEmpty(txtName.Text))
      comment.Author = txtName.Text;

    if (!string.IsNullOrEmpty(txtEmail.Text))
      comment.Email = txtEmail.Text;

    if (txtWebsite.Text.Trim().Length > 0)
    {
      if (!txtWebsite.Text.ToLowerInvariant().Contains("://"))
        txtWebsite.Text = "http://" + txtWebsite.Text;
     
      Uri website;
      if (Uri.TryCreate(txtWebsite.Text, UriKind.Absolute, out website))
        comment.Website = website;
    }

    control.Comment = comment;
    control.Post = Post;
    phLivePreview.Controls.Add(control);
  }

  protected void CheckAuthorName(object sender, ServerValidateEventArgs e)
  {
    e.IsValid = true;
    if (!Page.User.Identity.IsAuthenticated)
    {
      if (txtName.Text.ToLowerInvariant() == Post.Author.ToLowerInvariant())
        e.IsValid = false;
    }
  }

  #region Send mail

  private void SendMail(string author, string email, string content)
  {
    if (!BlogSettings.Instance.SendMailOnComment || Page.User.Identity.IsAuthenticated)
      return;

    MailMessage mail = new MailMessage();
    mail.From = new MailAddress(email, author);
    mail.To.Add(BlogSettings.Instance.Email);
    mail.Subject = "Weblog comment on " + Post.Title;
    mail.Body = "Comment by " + author + " (" + email + ")" + Environment.NewLine + Environment.NewLine;
    mail.Body += content + "\n\n" + Post.PermaLink.ToString();

    ThreadStart threadStart = delegate { Send(mail); };
    Thread thread = new Thread(threadStart);
    thread.IsBackground = true;
    thread.Start();
  }

  private void Send(MailMessage message)
  {
    try
    {
      SmtpClient smtp = new SmtpClient(BlogSettings.Instance.SmtpServer);
      smtp.Credentials = new System.Net.NetworkCredential(BlogSettings.Instance.SmtpUsername, BlogSettings.Instance.SmtpPassword);
      smtp.Port = BlogSettings.Instance.SmtpServerPort;
      smtp.Send(message);
    }
    catch (Exception)
    {
      // Ignores if the mail server does not respond.
    }
    finally
    {
      // Remove the pointer to the message object so the GC can close the thread.
      message.Dispose();
      message = null;
    }
  }

  #endregion

  #region Cookies

  /// <summary>
  /// Sets a cookie with the entered visitor information
  /// so it can be prefilled on next visit.
  /// </summary>
  private void SetCookie(string name, string email, string website, string country)
  {
    HttpCookie cookie = new HttpCookie("comment");
    cookie.Expires = DateTime.Now.AddMonths(24);
    cookie.Values.Add("name", name);
    cookie.Values.Add("email", email);
    cookie.Values.Add("url", website);
    cookie.Values.Add("country", country);
    Response.Cookies.Add(cookie);
  }

  /// <summary>
  /// Gets the cookie with visitor information if any is set.
  /// Then fills the contact information fields in the form.
  /// </summary>
  private void GetCookie()
  {
    HttpCookie cookie = Request.Cookies["comment"];
    if (cookie != null)
    {
      txtName.Text = cookie.Values["name"];
      txtEmail.Text = cookie.Values["email"];
      txtWebsite.Text = cookie.Values["url"];
      ddlCountry.SelectedValue = cookie.Values["country"];
      imgFlag.ImageUrl = "~/pics/flags/" + cookie.Values["country"] + ".png";
    }
  }

  #endregion

  #region CAPTCHA

  /// <summary>
  /// Initializes the captcha and registers the JavaScript
  /// </summary>
  private void InititializeCaptcha()
  {
    if (ViewState["captchavalue"] == null)
    {
      ViewState["captchavalue"] = Guid.NewGuid().ToString();
    }

    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    sb.Append("function SetCaptcha(){");
    sb.Append("var form = document.getElementById('" + Page.Form.ClientID + "');");
    sb.Append("var el = document.createElement('input');");
    sb.Append("el.type = 'hidden';");
    sb.Append("el.name = 'captcha';");
    sb.Append("el.value = '" + ViewState["captchavalue"] + "';");
    sb.Append("form.appendChild(el);}");

    Page.ClientScript.RegisterClientScriptBlock(GetType(), "captchascript", sb.ToString(), true);
    Page.ClientScript.RegisterOnSubmitStatement(GetType(), "captchayo", "SetCaptcha()");
  }

  /// <summary>
  /// Gets whether or not the user is human
  /// </summary>
  private bool IsCaptchaValid
  {
    get
    {
      if (ViewState["captchavalue"] != null)
      {
        return Request.Form["captcha"] == ViewState["captchavalue"].ToString();
      }

      return false;
    }
  }

  #endregion

  #region Protected methods and properties

  /// <summary>
  /// The regular expression used to parse links.
  /// </summary>
  private static Regex regex = new Regex("((http://|www\\.)([A-Z0-9.-]{1,})\\.[0-9A-Z?&=-_\\./]{2,})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

  /// <summary>
  /// Examins the comment body for any links and turns them
  /// automatically into one that can be clicked.
  /// <remarks>
  /// All links added to comments will have the rel attribute set
  /// to nofollow to prevent negative pagerank.
  /// </remarks>
  /// </summary>
  protected string ResolveLinks(string body)
  {
    foreach (Match match in regex.Matches(body))
    {
      if (!match.Value.Contains("://"))
        body = body.Replace(match.Value, regex.Replace(body, "<a href=\"http://$1\" rel=\"nofollow\">$1</a>"));
      else
        body = body.Replace(match.Value, regex.Replace(body, "<a href=\"$1\" rel=\"nofollow\">$1</a>"));
    }

    return body.Replace(Environment.NewLine, "<br />");
  }

  /// <summary>
  /// Displays a delete link to visitors that is authenticated
  /// using the default membership provider.
  /// </summary>
  /// <param name="id">The id of the comment.</param>
  protected string AdminLink(string id)
  {
    if (Page.User.Identity.IsAuthenticated)
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      foreach (Comment comment in Post.Comments)
      {
        if (comment.Id.ToString() == id)
          sb.AppendFormat(" | <a href=\"mailto:{0}\">{0}</a>", comment.Email);
      }

      string confirmDelete = "Are you sure you want to delete the comment?";
      sb.AppendFormat(" | <a href=\"?deletecomment={0}\" onclick=\"return confirm('{1}?')\">{2}</a>", id.ToString(), confirmDelete, "Delete");
      return sb.ToString();
    }

    return string.Empty;
  }

  /// <summary>
  /// Displays the Gravatar image that matches the specified email.
  /// </summary>
  protected string Gravatar(string email, string name, int size)
  {
    if (email.Contains("://"))
      return string.Format("<img class=\"thumb\" src=\"http://images.websnapr.com/?url={0}&amp;size=t\" alt=\"{1}\" />", name, email);
    //http://www.artviper.net/screenshots/screener.php?&url={0}&h={1}&w={1}
    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
    byte[] result = md5.ComputeHash(Encoding.ASCII.GetBytes(email));

    System.Text.StringBuilder hash = new System.Text.StringBuilder();
    for (int i = 0; i < result.Length; i++)
      hash.Append(result[i].ToString("x2"));

    System.Text.StringBuilder image = new System.Text.StringBuilder();
    image.Append("<img src=\"");
    image.Append("http://www.gravatar.com/avatar.php?");
    image.Append("gravatar_id=" + hash.ToString());
    image.Append("&amp;rating=G");
    image.Append("&amp;size=" + size);
    image.Append("&amp;default=");
    image.Append(Server.UrlEncode(Utils.AbsoluteWebRoot + "themes/" + BlogSettings.Instance.Theme + "/noavatar.jpg"));
    image.Append("\" alt=\"\" />");
    return image.ToString();
  }

  private Post _Post;
  /// <summary>
  /// Gets or sets the post from which the comments are parsed.
  /// </summary>
  public Post Post
  {
    get { return _Post; }
    set { _Post = value; }
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
    string[] args = eventArgument.Split('¤');
    string author = args[0];
    string email = args[1];
    string website = args[2];
    string country = args[3];
    string content = args[4];

    Comment comment = new Comment();
    comment.Id = Guid.NewGuid();
    comment.Author = Server.HtmlEncode(author);
    comment.Email = email;
    comment.Content = Server.HtmlEncode(content);
    comment.IP = Request.UserHostAddress;
    comment.Country = country;// GetCountry();
    comment.DateCreated = DateTime.Now;
    comment.Post = Post;

    if (website.Trim().Length > 0)
    {
      if (!website.ToLowerInvariant().Contains("://"))
        website = "http://" + website;

      Uri url;
      if (Uri.TryCreate(website, UriKind.Absolute, out url))
        comment.Website = url;
    }

    Post.AddComment(comment);

    SetCookie(author, email, website, country);
    SendMail(author, email, content);

    string path = "~/themes/" + BlogSettings.Instance.Theme + "/commentview.ascx";

    CommentViewBase control = (CommentViewBase)LoadControl(path);
    control.Comment = comment;
    control.Post = Post;

    using (System.IO.StringWriter sw = new System.IO.StringWriter())
    {
      control.RenderControl(new HtmlTextWriter(sw));
      _Callback = sw.ToString();
    }

  }

  #endregion
}
