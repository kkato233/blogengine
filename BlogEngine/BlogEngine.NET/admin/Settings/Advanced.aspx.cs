namespace admin.Settings
{
    using System;
    using System.Web.Services;
    using System.Threading;
    using Resources;
    using BlogEngine.Core;
    using BlogEngine.Core.Json;
    using Page = System.Web.UI.Page;

    public partial class Advanced : Page
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            BindSettings();

            Page.MaintainScrollPositionOnPostBack = true;
            Page.Title = labels.settings;
            base.OnInit(e);
        }

        /// <summary>
        /// The bind settings.
        /// </summary>
        private void BindSettings()
        {
            // -----------------------------------------------------------------------
            // Bind Advanced settings
            // -----------------------------------------------------------------------
            cbEnableCompression.Checked = BlogSettings.Instance.EnableHttpCompression;
            cbRemoveWhitespaceInStyleSheets.Checked = BlogSettings.Instance.RemoveWhitespaceInStyleSheets;
            cbCompressWebResource.Checked = BlogSettings.Instance.CompressWebResource;
            cbEnableOpenSearch.Checked = BlogSettings.Instance.EnableOpenSearch;
            cbRequireSslForMetaWeblogApi.Checked = BlogSettings.Instance.RequireSslMetaWeblogApi;
            rblWwwSubdomain.SelectedValue = BlogSettings.Instance.HandleWwwSubdomain;
            cbEnablePingBackSend.Checked = BlogSettings.Instance.EnablePingBackSend;
            cbEnablePingBackReceive.Checked = BlogSettings.Instance.EnablePingBackReceive;
            cbEnableTrackBackSend.Checked = BlogSettings.Instance.EnableTrackBackSend;
            cbEnableTrackBackReceive.Checked = BlogSettings.Instance.EnableTrackBackReceive;
            cbEnableErrorLogging.Checked = BlogSettings.Instance.EnableErrorLogging;

        }
		
        /// <summary>
        /// Save settings
        /// </summary>
        /// <param name="enableCompression"></param>
        /// <param name="removeWhitespaceInStyleSheets"></param>
        /// <param name="compressWebResource"></param>
        /// <param name="enableOpenSearch"></param>
        /// <param name="requireSslForMetaWeblogApi"></param>
        /// <param name="wwwSubdomain"></param>
        /// <param name="enableTrackBackSend"></param>
        /// <param name="enableTrackBackReceive"></param>
        /// <param name="enablePingBackSend"></param>
        /// <param name="enablePingBackReceive"></param>
        /// <param name="enableErrorLogging"></param>
        /// <returns></returns>
        [WebMethod]
        public static JsonResponse Save(string enableCompression, 
			string removeWhitespaceInStyleSheets,
			string compressWebResource,
			string enableOpenSearch,
			string requireSslForMetaWeblogApi,
			string wwwSubdomain,
			string enableTrackBackSend,
			string enableTrackBackReceive,
			string enablePingBackSend,
			string enablePingBackReceive,
			string enableErrorLogging)
        {
            var response = new JsonResponse {Success = false};

            if (!Thread.CurrentPrincipal.IsInRole(BlogSettings.Instance.AdministratorRole))
            {
                response.Message = "Not authorized";
                return response;
            }

            try
            {
                BlogSettings.Instance.EnableHttpCompression = bool.Parse(enableCompression);
				BlogSettings.Instance.RemoveWhitespaceInStyleSheets = bool.Parse(removeWhitespaceInStyleSheets);
				BlogSettings.Instance.CompressWebResource = bool.Parse(compressWebResource);
				BlogSettings.Instance.EnableOpenSearch = bool.Parse(enableOpenSearch);
				BlogSettings.Instance.RequireSslMetaWeblogApi = bool.Parse(requireSslForMetaWeblogApi);
				BlogSettings.Instance.HandleWwwSubdomain = wwwSubdomain;
				BlogSettings.Instance.EnableTrackBackSend = bool.Parse(enableTrackBackSend);
				BlogSettings.Instance.EnableTrackBackReceive = bool.Parse(enableTrackBackReceive);
				BlogSettings.Instance.EnablePingBackSend = bool.Parse(enablePingBackSend);
				BlogSettings.Instance.EnablePingBackReceive = bool.Parse(enablePingBackReceive);
				BlogSettings.Instance.EnableErrorLogging = bool.Parse(enableErrorLogging);

                BlogSettings.Instance.Save();
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("admin.Settings.Advanced.Save(): {0}", ex.Message));
                response.Message = string.Format("Could not save settings: {0}", ex.Message);
                return response;
            }

            response.Success = true;
            response.Message = "Settings saved";
            return response;
        }
    }
}