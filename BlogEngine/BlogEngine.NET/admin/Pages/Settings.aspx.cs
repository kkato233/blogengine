/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
05/03/2007  brian.kuhn  Added syndication settings controls/logic
****************************************************************************/
using System;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BlogEngine.Core;

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
    //-----------------------------------------------------------------------
    // Set Basic settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.Name = txtName.Text;
    BlogSettings.Instance.Description = txtDescription.Text;
    BlogSettings.Instance.PostsPerPage = int.Parse(txtPostsPerPage.Text);
    BlogSettings.Instance.Theme = ddlTheme.SelectedValue;
    BlogSettings.Instance.EnableRelatedPosts = cbShowRelatedPosts.Checked;

    //-----------------------------------------------------------------------
    // Set Email settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.Email = txtEmail.Text;
    BlogSettings.Instance.SmtpServer = txtSmtpServer.Text;
    BlogSettings.Instance.SmtpServerPort = int.Parse(txtSmtpServerPort.Text);
    BlogSettings.Instance.SmtpUsername = txtSmtpUsername.Text;
    BlogSettings.Instance.SmtpPassword = txtSmtpPassword.Text;
    BlogSettings.Instance.SendMailOnComment = cbComments.Checked;

    //-----------------------------------------------------------------------
    // Set Comments settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.IsCommentsEnabled = cbEnableComments.Checked;
    BlogSettings.Instance.EnableCountryInComments = cbEnableCountryInComments.Checked;
    BlogSettings.Instance.IsCoCommentEnabled = cbEnableCoComment.Checked;
    BlogSettings.Instance.ShowLivePreview = cbShowLivePreview.Checked;
    BlogSettings.Instance.DaysCommentsAreEnabled = int.Parse(ddlCloseComments.SelectedValue);

    //-----------------------------------------------------------------------
    // Set Advanced settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.EnableHttpCompression = cbEnableCompression.Checked;
    BlogSettings.Instance.EnableSearchHightlight = cbEnableSearchHighlight.Checked;
    BlogSettings.Instance.RemoveWhitespaceInStyleSheets = cbRemoveWhitespaceInStyleSheets.Checked;
    BlogSettings.Instance.EnableOpenSearch = cbEnableOpenSearch.Checked;
    BlogSettings.Instance.MarkExternalLinks = cbMarkExternalLinks.Checked;

    //-----------------------------------------------------------------------
    // Set Syndication settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.SyndicationFormat = ddlSyndicationFormat.SelectedValue;

    BlogSettings.Instance.AuthorName        = txtDublinCoreCreator.Text;
    BlogSettings.Instance.Language          = txtDublinCoreLanguage.Text;

    float latitude;
    if (Single.TryParse(txtGeocodingLatitude.Text, out latitude))
    {
        BlogSettings.Instance.GeocodingLatitude     = latitude;
    }
    float longitude;
    if (Single.TryParse(txtGeocodingLongitude.Text, out longitude))
    {
        BlogSettings.Instance.GeocodingLongitude    = longitude;
    }

    BlogSettings.Instance.Endorsement       = txtBlogChannelBLink.Text;

    //-----------------------------------------------------------------------
    //  Persist settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.Save();
    Response.Redirect(Request.RawUrl, true);
  }

  private void BindSettings()
  {
    //-----------------------------------------------------------------------
    // Bind Basic settings
    //-----------------------------------------------------------------------
    txtName.Text = BlogSettings.Instance.Name;
    txtDescription.Text = BlogSettings.Instance.Description;
    txtPostsPerPage.Text = BlogSettings.Instance.PostsPerPage.ToString();
    cbShowRelatedPosts.Checked = BlogSettings.Instance.EnableRelatedPosts;
    ddlTheme.SelectedValue = BlogSettings.Instance.Theme;

    //-----------------------------------------------------------------------
    // Bind Comments settings
    //-----------------------------------------------------------------------
    cbEnableComments.Checked = BlogSettings.Instance.IsCommentsEnabled;
    cbEnableCountryInComments.Checked = BlogSettings.Instance.EnableCountryInComments;
    cbEnableCoComment.Checked = BlogSettings.Instance.IsCoCommentEnabled;
    cbShowLivePreview.Checked = BlogSettings.Instance.ShowLivePreview;
    ddlCloseComments.SelectedValue = BlogSettings.Instance.DaysCommentsAreEnabled.ToString();

    //-----------------------------------------------------------------------
    // Bind Email settings
    //-----------------------------------------------------------------------
    txtEmail.Text = BlogSettings.Instance.Email;
    txtSmtpServer.Text = BlogSettings.Instance.SmtpServer;
    txtSmtpServerPort.Text = BlogSettings.Instance.SmtpServerPort.ToString();
    txtSmtpUsername.Text = BlogSettings.Instance.SmtpUsername;
    txtSmtpPassword.Text = BlogSettings.Instance.SmtpPassword;
    cbComments.Checked = BlogSettings.Instance.SendMailOnComment;

    //-----------------------------------------------------------------------
    // Bind Advanced settings
    //-----------------------------------------------------------------------
    cbEnableCompression.Checked = BlogSettings.Instance.EnableHttpCompression;
    cbEnableSearchHighlight.Checked = BlogSettings.Instance.EnableSearchHightlight;
    cbRemoveWhitespaceInStyleSheets.Checked = BlogSettings.Instance.RemoveWhitespaceInStyleSheets;
    cbEnableOpenSearch.Checked = BlogSettings.Instance.EnableOpenSearch;
    cbMarkExternalLinks.Checked = BlogSettings.Instance.MarkExternalLinks;

    //-----------------------------------------------------------------------
    // Bind Syndication settings
    //-----------------------------------------------------------------------
    ddlSyndicationFormat.SelectedValue  = BlogSettings.Instance.SyndicationFormat;

    txtDublinCoreCreator.Text           = BlogSettings.Instance.AuthorName;
    txtDublinCoreLanguage.Text          = BlogSettings.Instance.Language;

    txtGeocodingLatitude.Text           = BlogSettings.Instance.GeocodingLatitude != Single.MinValue ? BlogSettings.Instance.GeocodingLatitude.ToString() : String.Empty;
    txtGeocodingLongitude.Text          = BlogSettings.Instance.GeocodingLongitude != Single.MinValue ? BlogSettings.Instance.GeocodingLongitude.ToString() : String.Empty;

    txtBlogChannelBLink.Text            = BlogSettings.Instance.Endorsement;
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
