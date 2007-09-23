#region Using

using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using System.Net.Mail;
using System.Text.RegularExpressions;

#endregion

public partial class contact : BlogBasePage
{

  private static Regex _Regex = new Regex("<[^>]*>", RegexOptions.Compiled);

  protected void Page_Load(object sender, EventArgs e)
  {
    btnSend.Click += new EventHandler(btnSend_Click);
    if (!Page.IsPostBack)
    {
      txtSubject.Text = Request.QueryString["subject"];
      txtName.Text = Request.QueryString["name"];
      txtEmail.Text = Request.QueryString["email"];

      GetCookie();
      phAttachment.Visible = BlogSettings.Instance.EnableContactAttachments;
      InititializeCaptcha();      
      SetFocus();
    }

    Page.Title = Resources.labels.contact;
    base.AddMetaTag("description", _Regex.Replace(BlogSettings.Instance.ContactFormMessage, string.Empty));
  }

  /// <summary>
  /// Sets the focus on the first empty textbox.
  /// </summary>
  private void SetFocus()
  {
    if (string.IsNullOrEmpty(Request.QueryString["name"]) && txtName.Text == string.Empty)
    {
      txtName.Focus();
    }
    else if (string.IsNullOrEmpty(Request.QueryString["email"]) && txtEmail.Text == string.Empty)
    {
      txtEmail.Focus();
    }
    else if (string.IsNullOrEmpty(Request.QueryString["subject"]))
    {
      txtSubject.Focus();
    }
    else
    {
      txtMessage.Focus();
    }
  }

  private void btnSend_Click(object sender, EventArgs e)
  {
    bool success = SendEmail();
    divForm.Visible = !success;
    lblStatus.Visible = !success;
    divThank.Visible = success;
    SetCookie();
  }

  private bool SendEmail()
  {
    try
    {
      using (MailMessage mail = new MailMessage())
      {
        mail.From = new MailAddress(txtEmail.Text, txtName.Text);
        mail.To.Add(BlogSettings.Instance.Email);
        mail.Subject = "Weblog e-mail - " + txtSubject.Text;
        mail.Body = txtMessage.Text;
        mail.IsBodyHtml = false;

        if (txtAttachment.HasFile)
        {
          Attachment attachment = new Attachment(txtAttachment.PostedFile.InputStream, txtAttachment.FileName);
          mail.Attachments.Add(attachment);
        }

        Utils.SendMailMessage(mail);
      }

      return true;
    }
    catch (Exception ex)
    {
      if (User.Identity.IsAuthenticated)
      {
        lblStatus.Text = ex.InnerException.Message;
      }

      return false;
    }
  }

  #region Cookies

  /// <summary>
  /// Gets the cookie with visitor information if any is set.
  /// Then fills the contact information fields in the form.
  /// </summary>
  private void GetCookie()
  {
    HttpCookie cookie = Request.Cookies["comment"];
    if (cookie != null)
    {
      txtName.Text = Server.UrlDecode(cookie.Values["name"]);
      txtEmail.Text = cookie.Values["email"];
    }
  }

  /// <summary>
  /// Sets a cookie with the entered visitor information
  /// so it can be prefilled on next visit.
  /// </summary>
  private void SetCookie()
  {
    HttpCookie cookie = new HttpCookie("comment");
    cookie.Expires = DateTime.Now.AddMonths(24);
    cookie.Values.Add("name", Server.UrlEncode(txtName.Text));
    cookie.Values.Add("email", txtEmail.Text);
    cookie.Values.Add("url", string.Empty);
    cookie.Values.Add("country", string.Empty);
    Response.Cookies.Add(cookie);
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
    sb.AppendLine("function SetCaptcha(){");
    sb.AppendLine("var form = document.getElementById('" + Page.Form.ClientID + "');");
    sb.AppendLine("var el = document.createElement('input');");
    sb.AppendLine("el.type = 'hidden';");
    sb.AppendLine("el.name = 'captcha';");
    sb.AppendLine("el.value = '" + ViewState["captchavalue"] + "';");
    sb.AppendLine("form.appendChild(el);}");

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

}
