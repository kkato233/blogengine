namespace admin.Comments
{
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    using BlogEngine.Core;

    using Resources;

    /// <summary>
    /// The admin comments menu.
    /// </summary>
    public partial class Menu : UserControl
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.BuildMenuList();

            base.OnInit(e);
        }

        /// <summary>
        /// Builds the menu list.
        /// </summary>
        protected void BuildMenuList()
        {
            const string Tmpl = "<a href=\"{0}.aspx\" class=\"{1}\"><span>{2}</span></a>";

            using (var inbx = new HtmlGenericControl("li"))
            {
                var cssClass = this.Request.Path.ToLower().Contains("default.aspx") ? "current" : string.Empty;
                inbx.InnerHtml = string.Format(Tmpl, "Default", cssClass, labels.inbox);

                using (var appr = new HtmlGenericControl("li"))
                {
                    cssClass = this.Request.Path.ToLower().Contains("approved.aspx") ? "current" : string.Empty;
                    appr.InnerHtml = string.Format(Tmpl, "Approved", cssClass, labels.approved);

                    using (var spm = new HtmlGenericControl("li"))
                    {
                        cssClass = this.Request.Path.ToLower().Contains("spam.aspx") ? "current" : string.Empty;
                        spm.InnerHtml = string.Format(Tmpl, "Spam", cssClass, labels.spam);

                        using (var stn = new HtmlGenericControl("li"))
                        {
                            cssClass = this.Request.Path.ToLower().Contains("settings.aspx") ? "current" : string.Empty;
                            stn.InnerHtml = string.Format(Tmpl, "Settings", cssClass, labels.configuration);

                            if (BlogSettings.Instance.ModerationType == BlogSettings.Moderation.Disqus)
                            {
                                this.hdr.InnerHtml = "Moderated by Disqus";
                                this.UlMenu.Controls.Add(stn);
                                return;
                            }

                            this.UlMenu.Controls.Add(inbx);

                            if (BlogSettings.Instance.EnableCommentsModeration && BlogSettings.Instance.IsCommentsEnabled)
                            {
                                if (BlogSettings.Instance.ModerationType == BlogSettings.Moderation.Auto)
                                {
                                    this.hdr.InnerHtml = string.Format("{0}: {1}", labels.comments, labels.automoderation);
                                    this.UlMenu.Controls.Add(spm);
                                }
                                else
                                {
                                    this.hdr.InnerHtml = string.Format("{0}: {1} {2}", labels.comments, labels.manual, labels.moderation);
                                    this.UlMenu.Controls.Add(appr);
                                }
                            }
                            else
                            {
                                this.hdr.InnerHtml = string.Format("{0}: {1}", labels.comments, labels.unmoderated);
                            }

                            this.UlMenu.Controls.Add(stn);
                        }
                    }
                }
            }

            if (this.Request.Path.ToLower().Contains("settings.aspx"))
            {
                this.hdr.InnerHtml = string.Format("{0}: {1}", labels.comments, labels.settings);
            }
        }
    }
}