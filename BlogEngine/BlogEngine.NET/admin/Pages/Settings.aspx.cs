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
using System.Web.UI.WebControls;
using System.Globalization;

using BlogEngine.Core;

public partial class admin_Pages_configuration : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      BindThemes();
      BindCultures();
      BindSettings();
    }

    Page.MaintainScrollPositionOnPostBack = true;
    Page.Title = Resources.labels.settings;

    btnSave.Click += new EventHandler(btnSave_Click);
    btnSaveTop.Click += new EventHandler(btnSave_Click);
    btnTestSmtp.Click += new EventHandler(btnTestSmtp_Click);

    btnSaveTop.Text = Resources.labels.saveSettings;
    btnSave.Text = btnSaveTop.Text;
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
      smtp.EnableSsl = cbEnableSsl.Checked;
      smtp.Port = int.Parse(txtSmtpServerPort.Text, CultureInfo.InvariantCulture);
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
    BlogSettings.Instance.EnableRating = cbEnableRating.Checked;
    BlogSettings.Instance.ShowDescriptionInPostList = cbShowDescriptionInPostList.Checked;
    BlogSettings.Instance.Culture = ddlCulture.SelectedValue;
    BlogSettings.Instance.Timezone = double.Parse(txtTimeZone.Text, CultureInfo.InvariantCulture);

    //-----------------------------------------------------------------------
    // Set Email settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.Email = txtEmail.Text;
    BlogSettings.Instance.SmtpServer = txtSmtpServer.Text;
    BlogSettings.Instance.SmtpServerPort = int.Parse(txtSmtpServerPort.Text);
    BlogSettings.Instance.SmtpUsername = txtSmtpUsername.Text;
    BlogSettings.Instance.SmtpPassword = txtSmtpPassword.Text;
    BlogSettings.Instance.SendMailOnComment = cbComments.Checked;
    BlogSettings.Instance.EnableSsl = cbEnableSsl.Checked;

    //-----------------------------------------------------------------------
    // Set Comments settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.IsCommentsEnabled = cbEnableComments.Checked;
    BlogSettings.Instance.EnableCountryInComments = cbEnableCountryInComments.Checked;
    BlogSettings.Instance.IsCoCommentEnabled = cbEnableCoComment.Checked;
    BlogSettings.Instance.ShowLivePreview = cbShowLivePreview.Checked;
    BlogSettings.Instance.DaysCommentsAreEnabled = int.Parse(ddlCloseComments.SelectedValue);
    BlogSettings.Instance.EnableCommentsModeration = cbEnableCommentsModeration.Checked;

    //-----------------------------------------------------------------------
    // Set Advanced settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.EnableHttpCompression = cbEnableCompression.Checked;
    BlogSettings.Instance.EnableSearchHightlight = cbEnableSearchHighlight.Checked;
    BlogSettings.Instance.RemoveWhitespaceInStyleSheets = cbRemoveWhitespaceInStyleSheets.Checked;
    BlogSettings.Instance.EnableOpenSearch = cbEnableOpenSearch.Checked;
    BlogSettings.Instance.HandleWwwSubdomain = rblWwwSubdomain.SelectedItem.Value;

    //-----------------------------------------------------------------------
    // Set Syndication settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.SyndicationFormat = ddlSyndicationFormat.SelectedValue;
    BlogSettings.Instance.PostsPerFeed = int.Parse(txtPostsPerFeed.Text, CultureInfo.InvariantCulture);
    BlogSettings.Instance.AuthorName = txtDublinCoreCreator.Text;
    BlogSettings.Instance.Language = txtDublinCoreLanguage.Text;

    float latitude;
    if (Single.TryParse(txtGeocodingLatitude.Text, out latitude))
    {
      BlogSettings.Instance.GeocodingLatitude = latitude;
    }
    float longitude;
    if (Single.TryParse(txtGeocodingLongitude.Text, out longitude))
    {
      BlogSettings.Instance.GeocodingLongitude = longitude;
    }

    BlogSettings.Instance.Endorsement = txtBlogChannelBLink.Text;
    BlogSettings.Instance.FeedburnerUserName = txtFeedburnerUserName.Text;

    //-----------------------------------------------------------------------
    // HTML header section
    //-----------------------------------------------------------------------
    BlogSettings.Instance.HtmlHeader = txtHtmlHeader.Text;

    //-----------------------------------------------------------------------
    // Visitor tracking settings
    //-----------------------------------------------------------------------
    BlogSettings.Instance.TrackingScript = txtTrackingScript.Text;

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
    cbEnableRating.Checked = BlogSettings.Instance.EnableRating;
    cbShowDescriptionInPostList.Checked = BlogSettings.Instance.ShowDescriptionInPostList;
    ddlCulture.SelectedValue = BlogSettings.Instance.Culture;
    txtTimeZone.Text = BlogSettings.Instance.Timezone.ToString();

    //-----------------------------------------------------------------------
    // Bind Comments settings
    //-----------------------------------------------------------------------
    cbEnableComments.Checked = BlogSettings.Instance.IsCommentsEnabled;
    cbEnableCountryInComments.Checked = BlogSettings.Instance.EnableCountryInComments;
    cbEnableCoComment.Checked = BlogSettings.Instance.IsCoCommentEnabled;
    cbShowLivePreview.Checked = BlogSettings.Instance.ShowLivePreview;
    ddlCloseComments.SelectedValue = BlogSettings.Instance.DaysCommentsAreEnabled.ToString();
    cbEnableCommentsModeration.Checked = BlogSettings.Instance.EnableCommentsModeration;

    //-----------------------------------------------------------------------
    // Bind Email settings
    //-----------------------------------------------------------------------
    txtEmail.Text = BlogSettings.Instance.Email;
    txtSmtpServer.Text = BlogSettings.Instance.SmtpServer;
    txtSmtpServerPort.Text = BlogSettings.Instance.SmtpServerPort.ToString();
    txtSmtpUsername.Text = BlogSettings.Instance.SmtpUsername;
    txtSmtpPassword.Text = BlogSettings.Instance.SmtpPassword;
    cbComments.Checked = BlogSettings.Instance.SendMailOnComment;
    cbEnableSsl.Checked = BlogSettings.Instance.EnableSsl;

    //-----------------------------------------------------------------------
    // Bind Advanced settings
    //-----------------------------------------------------------------------
    cbEnableCompression.Checked = BlogSettings.Instance.EnableHttpCompression;
    cbEnableSearchHighlight.Checked = BlogSettings.Instance.EnableSearchHightlight;
    cbRemoveWhitespaceInStyleSheets.Checked = BlogSettings.Instance.RemoveWhitespaceInStyleSheets;
    cbEnableOpenSearch.Checked = BlogSettings.Instance.EnableOpenSearch;
    rblWwwSubdomain.SelectedValue = BlogSettings.Instance.HandleWwwSubdomain;

    //-----------------------------------------------------------------------
    // Bind Syndication settings
    //-----------------------------------------------------------------------
    ddlSyndicationFormat.SelectedValue = BlogSettings.Instance.SyndicationFormat;
    txtPostsPerFeed.Text = BlogSettings.Instance.PostsPerFeed.ToString();
    txtDublinCoreCreator.Text = BlogSettings.Instance.AuthorName;
    txtDublinCoreLanguage.Text = BlogSettings.Instance.Language;

    txtGeocodingLatitude.Text = BlogSettings.Instance.GeocodingLatitude != Single.MinValue ? BlogSettings.Instance.GeocodingLatitude.ToString() : String.Empty;
    txtGeocodingLongitude.Text = BlogSettings.Instance.GeocodingLongitude != Single.MinValue ? BlogSettings.Instance.GeocodingLongitude.ToString() : String.Empty;

    txtBlogChannelBLink.Text = BlogSettings.Instance.Endorsement;
    txtFeedburnerUserName.Text = BlogSettings.Instance.FeedburnerUserName;

    //-----------------------------------------------------------------------
    // HTML header section
    //-----------------------------------------------------------------------
    txtHtmlHeader.Text = BlogSettings.Instance.HtmlHeader;

    //-----------------------------------------------------------------------
    // Visitor tracking settings
    //-----------------------------------------------------------------------
    txtTrackingScript.Text = BlogSettings.Instance.TrackingScript;
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

  private void BindCultures()
  {
    if (File.Exists(Path.Combine(HttpRuntime.AppDomainAppPath, "PrecompiledApp.config")))
    {
      string precompiledDir = HttpRuntime.BinDirectory;
      string[] translations = Directory.GetFiles(precompiledDir, "App_GlobalResources.resources.dll", SearchOption.AllDirectories);
      foreach (string translation in translations)
      {
        string resourceDir = Path.GetDirectoryName(translation).Remove(0, precompiledDir.Length);
        if (!String.IsNullOrEmpty(resourceDir))
        {
          System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag(resourceDir);
          ddlCulture.Items.Add(new ListItem(info.NativeName, resourceDir));
        }
      }
    }
    else
    {
      string path = Server.MapPath("~/App_GlobalResources/");
      foreach (string file in Directory.GetFiles(path))
      {
        int index = file.LastIndexOf("\\") + 1;
        string filename = file.Substring(index);
        if (filename != "labels.resx")
        {
          filename = filename.Replace("labels.", string.Empty).Replace(".resx", string.Empty);
          System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag(filename);
          ddlCulture.Items.Add(new ListItem(info.NativeName, filename));
        }
      }
    }
  }

}