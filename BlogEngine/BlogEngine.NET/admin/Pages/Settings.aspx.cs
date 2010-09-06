#region Using

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BlogEngine.Core;
using BlogEngine.Core.API.BlogML;

using Resources;

using Page = System.Web.UI.Page;

#endregion

/// <summary>
/// The admin_ pages_configuration.
/// </summary>
public partial class admin_Pages_configuration : Page
{
    #region Methods

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.BindThemes();
            this.BindCultures();
            this.BindSettings();
        }

        this.Page.MaintainScrollPositionOnPostBack = true;
        this.Page.Title = labels.settings;

        this.btnSave.Click += this.btnSave_Click;
        this.btnSaveTop.Click += this.btnSave_Click;
        this.btnTestSmtp.Click += this.btnTestSmtp_Click;

        this.btnSaveTop.Text = labels.saveSettings;
        this.btnSave.Text = this.btnSaveTop.Text;
        this.valDescChar.ErrorMessage = "Please specify a number";
    }

    /// <summary>
    /// Handles the Click event of the btnBlogMLImport control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnBlogMLImport_Click(object sender, EventArgs e)
    {
        var fileName = this.txtUploadFile.FileName;

        if (string.IsNullOrEmpty(fileName))
        {
            ((admin_admin)this.Master).SetStatus("warning", "File name is required");
        }
        else
        {
            var reader = new BlogReader();

            var stm = this.txtUploadFile.FileContent;
            var rdr = new StreamReader(stm);
            reader.XmlData = rdr.ReadToEnd();

            ((admin_admin)this.Master).SetStatus(reader.Import() ? "success" : "warning", reader.Message);
        }
    }

    /// <summary>
    /// Binds the cultures.
    /// </summary>
    private void BindCultures()
    {
        if (File.Exists(Path.Combine(HttpRuntime.AppDomainAppPath, "PrecompiledApp.config")))
        {
            var precompiledDir = HttpRuntime.BinDirectory;
            var translations = Directory.GetFiles(
                precompiledDir, "App_GlobalResources.resources.dll", SearchOption.AllDirectories);
            foreach (var translation in translations)
            {
                var resourceDir = Path.GetDirectoryName(translation).Remove(0, precompiledDir.Length);
                if (!String.IsNullOrEmpty(resourceDir))
                {
                    var info = CultureInfo.GetCultureInfoByIetfLanguageTag(resourceDir);
                    this.ddlCulture.Items.Add(new ListItem(info.NativeName, resourceDir));
                }
            }
        }
        else
        {
            var path = this.Server.MapPath(Utils.RelativeWebRoot + "App_GlobalResources/");
            foreach (var file in Directory.GetFiles(path, "labels.*.resx"))
            {
                var index = file.LastIndexOf(Path.DirectorySeparatorChar) + 1;
                var filename = file.Substring(index);
                filename = filename.Replace("labels.", string.Empty).Replace(".resx", string.Empty);
                var info = CultureInfo.GetCultureInfoByIetfLanguageTag(filename);
                this.ddlCulture.Items.Add(new ListItem(info.NativeName, filename));
            }
        }
    }

    /// <summary>
    /// Binds the settings.
    /// </summary>
    private void BindSettings()
    {
        // -----------------------------------------------------------------------
        // Bind Basic settings
        // -----------------------------------------------------------------------
        this.txtName.Text = BlogSettings.Instance.Name;
        this.txtDescription.Text = BlogSettings.Instance.Description;
        this.txtPostsPerPage.Text = BlogSettings.Instance.PostsPerPage.ToString();
        this.cbShowRelatedPosts.Checked = BlogSettings.Instance.EnableRelatedPosts;
        this.ddlTheme.SelectedValue = BlogSettings.Instance.Theme;
        this.ddlMobileTheme.SelectedValue = BlogSettings.Instance.MobileTheme;
        this.cbUseBlogNameInPageTitles.Checked = BlogSettings.Instance.UseBlogNameInPageTitles;
        this.cbEnableRating.Checked = BlogSettings.Instance.EnableRating;
        this.cbShowDescriptionInPostList.Checked = BlogSettings.Instance.ShowDescriptionInPostList;
        this.txtDescriptionCharacters.Text = BlogSettings.Instance.DescriptionCharacters.ToString();
        this.cbShowDescriptionInPostListForPostsByTagOrCategory.Checked =
            BlogSettings.Instance.ShowDescriptionInPostListForPostsByTagOrCategory;
        this.txtDescriptionCharactersForPostsByTagOrCategory.Text =
            BlogSettings.Instance.DescriptionCharactersForPostsByTagOrCategory.ToString();
        this.cbTimeStampPostLinks.Checked = BlogSettings.Instance.TimeStampPostLinks;
        this.ddlCulture.SelectedValue = BlogSettings.Instance.Culture;
        this.txtTimeZone.Text = BlogSettings.Instance.Timezone.ToString();
        this.cbShowPostNavigation.Checked = BlogSettings.Instance.ShowPostNavigation;
        this.cbEnableSelfRegistration.Checked = BlogSettings.Instance.EnableSelfRegistration;
        this.cbRequireLoginToViewPosts.Checked = BlogSettings.Instance.RequireLoginToViewPosts;

        // -----------------------------------------------------------------------
        // Bind Email settings
        // -----------------------------------------------------------------------
        this.txtEmail.Text = BlogSettings.Instance.Email;
        this.txtSmtpServer.Text = BlogSettings.Instance.SmtpServer;
        this.txtSmtpServerPort.Text = BlogSettings.Instance.SmtpServerPort.ToString();
        this.txtSmtpUsername.Text = BlogSettings.Instance.SmtpUserName;
        this.txtSmtpPassword.Text = BlogSettings.Instance.SmtpPassword;
        this.cbComments.Checked = BlogSettings.Instance.SendMailOnComment;
        this.cbEnableSsl.Checked = BlogSettings.Instance.EnableSsl;
        this.txtEmailSubjectPrefix.Text = BlogSettings.Instance.EmailSubjectPrefix;

        this.cbEnableEnclosures.Checked = BlogSettings.Instance.EnableEnclosures;

        // -----------------------------------------------------------------------
        // Bind Advanced settings
        // -----------------------------------------------------------------------
        this.cbEnableCompression.Checked = BlogSettings.Instance.EnableHttpCompression;
        this.cbRemoveWhitespaceInStyleSheets.Checked = BlogSettings.Instance.RemoveWhitespaceInStyleSheets;
        this.cbCompressWebResource.Checked = BlogSettings.Instance.CompressWebResource;
        this.cbEnableOpenSearch.Checked = BlogSettings.Instance.EnableOpenSearch;
        this.cbRequireSslForMetaWeblogApi.Checked = BlogSettings.Instance.RequireSslMetaWeblogApi;
        this.rblWwwSubdomain.SelectedValue = BlogSettings.Instance.HandleWwwSubdomain;
        this.cbEnablePingBackSend.Checked = BlogSettings.Instance.EnablePingBackSend;
        this.cbEnablePingBackReceive.Checked = BlogSettings.Instance.EnablePingBackReceive;
        this.cbEnableTrackBackSend.Checked = BlogSettings.Instance.EnableTrackBackSend;
        this.cbEnableTrackBackReceive.Checked = BlogSettings.Instance.EnableTrackBackReceive;
        this.cbEnableErrorLogging.Checked = BlogSettings.Instance.EnableErrorLogging;

        // -----------------------------------------------------------------------
        // Bind Syndication settings
        // -----------------------------------------------------------------------
        this.ddlSyndicationFormat.SelectedValue = BlogSettings.Instance.SyndicationFormat;
        this.txtPostsPerFeed.Text = BlogSettings.Instance.PostsPerFeed.ToString();
        this.txtDublinCoreCreator.Text = BlogSettings.Instance.AuthorName;
        this.txtDublinCoreLanguage.Text = BlogSettings.Instance.Language;

        this.txtGeocodingLatitude.Text = BlogSettings.Instance.GeocodingLatitude != Single.MinValue
                                             ? BlogSettings.Instance.GeocodingLatitude.ToString(
                                                 CultureInfo.InvariantCulture)
                                             : String.Empty;
        this.txtGeocodingLongitude.Text = BlogSettings.Instance.GeocodingLongitude != Single.MinValue
                                              ? BlogSettings.Instance.GeocodingLongitude.ToString(
                                                  CultureInfo.InvariantCulture)
                                              : String.Empty;

        this.txtBlogChannelBLink.Text = BlogSettings.Instance.Endorsement;
        this.txtAlternateFeedUrl.Text = BlogSettings.Instance.AlternateFeedUrl;

        // -----------------------------------------------------------------------
        // HTML header section
        // -----------------------------------------------------------------------
        this.txtHtmlHeader.Text = BlogSettings.Instance.HtmlHeader;

        // -----------------------------------------------------------------------
        // Visitor tracking settings
        // -----------------------------------------------------------------------
        this.txtTrackingScript.Text = BlogSettings.Instance.TrackingScript;
    }

    /// <summary>
    /// Binds the themes.
    /// </summary>
    private void BindThemes()
    {
        var path = this.Server.MapPath(Utils.RelativeWebRoot + "themes/");
        foreach (var dic in Directory.GetDirectories(path))
        {
            var index = dic.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            this.ddlTheme.Items.Add(dic.Substring(index));
            this.ddlMobileTheme.Items.Add(dic.Substring(index));
        }
    }

    /// <summary>
    /// Handles the Click event of the btnSave control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void btnSave_Click(object sender, EventArgs e)
    {
        var enabledHttpCompressionSettingChanged = BlogSettings.Instance.EnableHttpCompression !=
                                                   this.cbEnableCompression.Checked;

        // -----------------------------------------------------------------------
        // Set Basic settings
        // -----------------------------------------------------------------------
        BlogSettings.Instance.Name = this.txtName.Text;
        BlogSettings.Instance.Description = this.txtDescription.Text;
        BlogSettings.Instance.PostsPerPage = int.Parse(this.txtPostsPerPage.Text);
        BlogSettings.Instance.Theme = this.ddlTheme.SelectedValue;
        BlogSettings.Instance.MobileTheme = this.ddlMobileTheme.SelectedValue;
        BlogSettings.Instance.UseBlogNameInPageTitles = this.cbUseBlogNameInPageTitles.Checked;
        BlogSettings.Instance.EnableRelatedPosts = this.cbShowRelatedPosts.Checked;
        BlogSettings.Instance.EnableRating = this.cbEnableRating.Checked;
        BlogSettings.Instance.ShowDescriptionInPostList = this.cbShowDescriptionInPostList.Checked;
        BlogSettings.Instance.DescriptionCharacters = int.Parse(this.txtDescriptionCharacters.Text);
        BlogSettings.Instance.ShowDescriptionInPostListForPostsByTagOrCategory =
            this.cbShowDescriptionInPostListForPostsByTagOrCategory.Checked;
        BlogSettings.Instance.DescriptionCharactersForPostsByTagOrCategory =
            int.Parse(this.txtDescriptionCharactersForPostsByTagOrCategory.Text);
        BlogSettings.Instance.TimeStampPostLinks = this.cbTimeStampPostLinks.Checked;
        BlogSettings.Instance.ShowPostNavigation = this.cbShowPostNavigation.Checked;
        BlogSettings.Instance.Culture = this.ddlCulture.SelectedValue;
        BlogSettings.Instance.Timezone = double.Parse(this.txtTimeZone.Text, CultureInfo.InvariantCulture);
        BlogSettings.Instance.EnableSelfRegistration = this.cbEnableSelfRegistration.Checked;
        BlogSettings.Instance.RequireLoginToViewPosts = this.cbRequireLoginToViewPosts.Checked;

        // -----------------------------------------------------------------------
        // Set Email settings
        // -----------------------------------------------------------------------
        BlogSettings.Instance.Email = this.txtEmail.Text;
        BlogSettings.Instance.SmtpServer = this.txtSmtpServer.Text;
        BlogSettings.Instance.SmtpServerPort = int.Parse(this.txtSmtpServerPort.Text);
        BlogSettings.Instance.SmtpUserName = this.txtSmtpUsername.Text;
        BlogSettings.Instance.SmtpPassword = this.txtSmtpPassword.Text;
        BlogSettings.Instance.SendMailOnComment = this.cbComments.Checked;
        BlogSettings.Instance.EnableSsl = this.cbEnableSsl.Checked;
        BlogSettings.Instance.EmailSubjectPrefix = this.txtEmailSubjectPrefix.Text;

        BlogSettings.Instance.EnableEnclosures = this.cbEnableEnclosures.Checked;

        // -----------------------------------------------------------------------
        // Set Advanced settings
        // -----------------------------------------------------------------------
        BlogSettings.Instance.EnableHttpCompression = this.cbEnableCompression.Checked;
        BlogSettings.Instance.RemoveWhitespaceInStyleSheets = this.cbRemoveWhitespaceInStyleSheets.Checked;
        BlogSettings.Instance.CompressWebResource = this.cbCompressWebResource.Checked;
        BlogSettings.Instance.EnableOpenSearch = this.cbEnableOpenSearch.Checked;
        BlogSettings.Instance.RequireSslMetaWeblogApi = this.cbRequireSslForMetaWeblogApi.Checked;
        BlogSettings.Instance.HandleWwwSubdomain = this.rblWwwSubdomain.SelectedItem.Value;
        BlogSettings.Instance.EnableTrackBackSend = this.cbEnableTrackBackSend.Checked;
        BlogSettings.Instance.EnableTrackBackReceive = this.cbEnableTrackBackReceive.Checked;
        BlogSettings.Instance.EnablePingBackSend = this.cbEnablePingBackSend.Checked;
        BlogSettings.Instance.EnablePingBackReceive = this.cbEnablePingBackReceive.Checked;
        BlogSettings.Instance.EnableErrorLogging = this.cbEnableErrorLogging.Checked;

        // -----------------------------------------------------------------------
        // Set Syndication settings
        // -----------------------------------------------------------------------
        BlogSettings.Instance.SyndicationFormat = this.ddlSyndicationFormat.SelectedValue;
        BlogSettings.Instance.PostsPerFeed = int.Parse(this.txtPostsPerFeed.Text, CultureInfo.InvariantCulture);
        BlogSettings.Instance.AuthorName = this.txtDublinCoreCreator.Text;
        BlogSettings.Instance.Language = this.txtDublinCoreLanguage.Text;

        float latitude;
        BlogSettings.Instance.GeocodingLatitude = Single.TryParse(
            this.txtGeocodingLatitude.Text.Replace(",", "."), 
            NumberStyles.Any, 
            CultureInfo.InvariantCulture, 
            out latitude) ? latitude : Single.MinValue;

        float longitude;
        BlogSettings.Instance.GeocodingLongitude = Single.TryParse(
            this.txtGeocodingLongitude.Text.Replace(",", "."), 
            NumberStyles.Any, 
            CultureInfo.InvariantCulture, 
            out longitude) ? longitude : Single.MinValue;

        BlogSettings.Instance.Endorsement = this.txtBlogChannelBLink.Text;

        if (this.txtAlternateFeedUrl.Text.Trim().Length > 0 && !this.txtAlternateFeedUrl.Text.Contains("://"))
        {
            this.txtAlternateFeedUrl.Text = "http://" + this.txtAlternateFeedUrl.Text;
        }

        BlogSettings.Instance.AlternateFeedUrl = this.txtAlternateFeedUrl.Text;

        // -----------------------------------------------------------------------
        // HTML header section
        // -----------------------------------------------------------------------
        BlogSettings.Instance.HtmlHeader = this.txtHtmlHeader.Text;

        // -----------------------------------------------------------------------
        // Visitor tracking settings
        // -----------------------------------------------------------------------
        BlogSettings.Instance.TrackingScript = this.txtTrackingScript.Text;

        // -----------------------------------------------------------------------
        // Persist settings
        // -----------------------------------------------------------------------
        BlogSettings.Instance.Save();

        if (enabledHttpCompressionSettingChanged)
        {
            // To avoid errors in IIS7 when toggling between compression and no-compression, re-start the app.
            var configPath = string.Format("{0}Web.Config", HttpContext.Current.Request.PhysicalApplicationPath);
            File.SetLastWriteTimeUtc(configPath, DateTime.UtcNow);
        }

        this.Response.Redirect(this.Request.RawUrl, true);
    }

    /// <summary>
    /// The btn test smtp_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void btnTestSmtp_Click(object sender, EventArgs e)
    {
        try
        {
            var mail = new MailMessage { From = new MailAddress(this.txtEmail.Text, this.txtName.Text) };
            mail.To.Add(mail.From);
            mail.Subject = "Test mail from " + this.txtName.Text;
            mail.IsBodyHtml = true;
            mail.Body = "<div style=\"font: 11px verdana, arial\">";
            mail.Body += "Success";
            if (HttpContext.Current != null)
            {
                mail.Body +=
                    "<br /><br />_______________________________________________________________________________<br /><br />";
                mail.Body += "<strong>IP address:</strong> " + HttpContext.Current.Request.UserHostAddress + "<br />";
                mail.Body += "<strong>User-agent:</strong> " + HttpContext.Current.Request.UserAgent;
            }

            mail.Body += "</div>";
            var smtp = new SmtpClient(this.txtSmtpServer.Text);

            // don't send credentials if a server doesn't require it,
            // linux smtp servers don't like that 
            if (!string.IsNullOrEmpty(this.txtSmtpUsername.Text))
            {
                smtp.Credentials = new NetworkCredential(this.txtSmtpUsername.Text, this.txtSmtpPassword.Text);
            }

            smtp.EnableSsl = this.cbEnableSsl.Checked;
            smtp.Port = int.Parse(this.txtSmtpServerPort.Text, CultureInfo.InvariantCulture);
            smtp.Send(mail);
            this.lbSmtpStatus.Text = "Test successfull";
            this.lbSmtpStatus.Style.Add(HtmlTextWriterStyle.Color, "green");
        }
        catch (Exception ex)
        {
            this.lbSmtpStatus.Text = string.Format("Could not connect - {0}", ex.Message);
            this.lbSmtpStatus.Style.Add(HtmlTextWriterStyle.Color, "red");
        }
    }

    #endregion
}