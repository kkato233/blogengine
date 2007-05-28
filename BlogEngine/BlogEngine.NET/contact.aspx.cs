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

#endregion

public partial class contact : BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    btnSend.Click += new EventHandler(btnSend_Click);
  }

  private void btnSend_Click(object sender, EventArgs e)
  {
    bool success = SendEmail();
    divForm.Visible = !success;
    lblStatus.Visible = !success;
    divThank.Visible = success;    
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

        SmtpClient smtp = new SmtpClient(BlogSettings.Instance.SmtpServer);
        smtp.Credentials = new System.Net.NetworkCredential(BlogSettings.Instance.SmtpUsername, BlogSettings.Instance.SmtpPassword);
        smtp.Port = BlogSettings.Instance.SmtpServerPort;
        smtp.Send(mail);
      }

      return true;
    }
    catch (Exception)
    {
      return false;
    }
  }
}
