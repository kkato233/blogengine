#region Using

using System;
using System.Net.Mail;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

#endregion

public partial class admin_Pages_configuration : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      BindThemes();
      BindSettings();      
    }

    btnSave.Click += new EventHandler(btnSave_Click);
    btnTestSmtp.Click += new EventHandler(btnTestSmtp_Click);
  }

  private void btnTestSmtp_Click(object sender, EventArgs e)
  {
    try
    {
      MailMessage mail = new MailMessage();
      mail.From = new MailAddress(txtEmail.Text, txtName.Text);
      mail.To.Add(mail.From);
      mail.Subject = "Test mail from " + txtName.Text;
      mail.Body = "Success";
      SmtpClient smtp = new SmtpClient(txtSmtpServer.Text);
      smtp.Credentials = new System.Net.NetworkCredential(txtSmtpUsername.Text, txtSmtpPassword.Text);
      smtp.Send(mail);
      lbSmtpStatus.Text = "Test successfull";
      lbSmtpStatus.Style.Add(HtmlTextWriterStyle.Color, "green");
    }
    catch
    {
      lbSmtpStatus.Text = "Could not connect";
      lbSmtpStatus.Style.Add(HtmlTextWriterStyle.Color, "red");
    }
  }

  private void btnSave_Click(object sender, EventArgs e)
  {
    BlogSettings.Instance.Name = txtName.Text;
    BlogSettings.Instance.Description = txtDescription.Text;
    BlogSettings.Instance.PostsPerPage = int.Parse(txtPostsPerPage.Text);
    BlogSettings.Instance.Theme = ddlTheme.SelectedValue;
    BlogSettings.Instance.EnableRelatedPosts = cbShowRelatedPosts.Checked;
    BlogSettings.Instance.Email = txtEmail.Text;
    BlogSettings.Instance.SmtpServer = txtSmtpServer.Text;
    BlogSettings.Instance.SmtpUsername = txtSmtpUsername.Text;
    BlogSettings.Instance.SmtpPassword = txtSmtpPassword.Text;
    BlogSettings.Instance.SendMailOnComment = cbComments.Checked;
    BlogSettings.Instance.IsCommentsEnabled = cbEnableComments.Checked;
    BlogSettings.Instance.EnableCountryInComments = cbEnableCountryInComments.Checked;
    BlogSettings.Instance.IsCoCommentEnabled = cbEnableCoComment.Checked;
    BlogSettings.Instance.ShowLivePreview = cbShowLivePreview.Checked;
    BlogSettings.Instance.DaysCommentsAreEnabled = int.Parse(ddlCloseComments.SelectedValue);
    BlogSettings.Instance.EnableHttpCompression = cbEnableCompression.Checked;

    BlogSettings.Instance.Save();
    Response.Redirect(Request.RawUrl, true);
  }

  private void BindSettings()
  {
    // Basic
    txtName.Text = BlogSettings.Instance.Name;
    txtDescription.Text = BlogSettings.Instance.Description;
    txtPostsPerPage.Text = BlogSettings.Instance.PostsPerPage.ToString();
    cbShowRelatedPosts.Checked = BlogSettings.Instance.EnableRelatedPosts;
    ddlTheme.SelectedValue = BlogSettings.Instance.Theme;

    // Advanced
    cbEnableCompression.Checked = BlogSettings.Instance.EnableHttpCompression;

    // Comments
    cbEnableComments.Checked = BlogSettings.Instance.IsCommentsEnabled;
    cbEnableCountryInComments.Checked = BlogSettings.Instance.EnableCountryInComments;
    cbEnableCoComment.Checked = BlogSettings.Instance.IsCoCommentEnabled;
    cbShowLivePreview.Checked = BlogSettings.Instance.ShowLivePreview;
    ddlCloseComments.SelectedValue = BlogSettings.Instance.DaysCommentsAreEnabled.ToString();
    
    // Email
    txtEmail.Text = BlogSettings.Instance.Email;
    txtSmtpServer.Text = BlogSettings.Instance.SmtpServer;
    txtSmtpUsername.Text = BlogSettings.Instance.SmtpUsername;
    txtSmtpPassword.Text = BlogSettings.Instance.SmtpPassword;
    cbComments.Checked = BlogSettings.Instance.SendMailOnComment;
    
  }

  private void BindThemes()
  {
    string path = Server.MapPath("~/themes/");
    foreach (string dic in Directory.GetDirectories(path))
    {
      int index = dic.LastIndexOf("\\") + 1;
      ddlTheme.Items.Add(dic.Substring(index));
    }
  }
}
