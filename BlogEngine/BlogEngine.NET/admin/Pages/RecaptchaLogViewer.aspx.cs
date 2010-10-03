namespace admin.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Security;

    using BlogEngine.Core;

    using Recaptcha;

    using Page = System.Web.UI.Page;

    /// <summary>
    /// The admin_ pages_ recaptcha log viewer.
    /// </summary>
    public partial class admin_Pages_RecaptchaLogViewer : Page
    {
        #region Constants and Fields

        #endregion

        #region Public Methods

        /// <summary>
        /// The get website.
        /// </summary>
        /// <param name="website">
        /// The website.
        /// </param>
        /// <returns>
        /// The get website.
        /// </returns>
        public static string GetWebsite(object website)
        {
            if (website == null)
            {
                return string.Empty;
            }

            const string Templ = "<a href='{0}' target='_new' rel='{0}'>{1}</a>";

            var site = website.ToString();
            site = site.Replace("http://www.", string.Empty);
            site = site.Replace("http://", string.Empty);
            site = site.Length < 20 ? site : string.Format("{0}...", site.Substring(0, 17));

            return string.Format(Templ, website, site);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gravatars the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="author">The author.</param>
        /// <returns></returns>
        protected string Gravatar(string email, string author)
        {
            return Avatar.GetAvatarImageTag(28, email, null, null, author);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                this.Response.Redirect(Utils.RelativeWebRoot);
            }

            if (!this.IsPostBack)
            {
                this.BindGrid();
            }
        }

        /// <summary>
        /// The bind grid.
        /// </summary>
        private void BindGrid()
        {
            var log = RecaptchaLogger.ReadLogItems();

            var comments = Post.Posts.SelectMany(post => post.Comments).ToDictionary(comment => comment.Id);

            var logView = new DataTable("LogView");
            logView.Columns.Add("Email");
            logView.Columns.Add("Date", typeof(DateTime));
            logView.Columns.Add("Author");
            logView.Columns.Add("Website");
            logView.Columns.Add("IP");
            logView.Columns.Add("RecaptchaAttempts", typeof(ushort));
            logView.Columns.Add("CommentTime", typeof(double));
            logView.Columns.Add("RecaptchaTime", typeof(double));

            var orphanedRecords = new List<RecaptchaLogItem>();

            foreach (var item in log)
            {
                if (comments.ContainsKey(item.CommentId))
                {
                    var comment = comments[item.CommentId];
                    logView.Rows.Add(
                        comment.Email,
                        comment.DateCreated,
                        comment.Author,
                        comment.Website,
                        comment.IP,
                        item.NumberOfAttempts,
                        item.TimeToComment,
                        item.TimeToSolveCapcha);
                }
                else
                {
                    orphanedRecords.Add(item);
                }
            }

            if (orphanedRecords.Count > 0)
            {
                foreach (var orphan in orphanedRecords)
                {
                    log.Remove(orphan);
                }

                RecaptchaLogger.SaveLogItems(log);
            }

            using (var view = new DataView(logView))
            {
                view.Sort = "Date DESC";
                this.RecaptchaLog.DataSource = view;
                this.RecaptchaLog.DataBind();
            }
        }

        #endregion
    }
}