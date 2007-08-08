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
      GetCookie();
      phAttachment.Visible = BlogSettings.Instance.EnableContactAttachments;
    }

    Page.Title = Resources.labels.contact;
    base.AddMetaTag("description", _Regex.Replace(BlogSettings.Instance.ContactFormMessage, string.Empty));
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
    catch (Exception)
    {
      return false;
    }
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
    cookie.Values.Add("name", txtName.Text);
    cookie.Values.Add("email", txtEmail.Text);
    cookie.Values.Add("url", string.Empty);
    cookie.Values.Add("country", string.Empty);
    Response.Cookies.Add(cookie);
  }

}
