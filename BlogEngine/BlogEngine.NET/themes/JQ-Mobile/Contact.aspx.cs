namespace MichaelJBaird.Themes.JQMobile
{
    #region using
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
    using Resources;
    #endregion

    public partial class Contact : BlogBasePage, ICallbackEventHandler
    {
        #region OnLoad
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// Adds links and javascript to the HTML header tag.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            // Add Header
            AddHeader();

            base.OnLoad(e);

            ClientScript.GetCallbackEventReference(this, "arg", "callback", "context");
            btnSend.Click += new EventHandler(btnSend_Click);
            if (!Page.IsPostBack)
            {
                txtSubject.Text = Request.QueryString["subject"];
                txtName.Text = Request.QueryString["name"];
                txtEmail.Text = Request.QueryString["email"];

                GetCookie();
                SetFocus();
            }

            if (!IsPostBack && !IsCallback)
            {
                recaptcha.Visible = UseCaptcha;
                recaptcha.UserUniqueIdentifier = hfCaptcha.Value = Guid.NewGuid().ToString();
            }

            Page.Title = Server.HtmlEncode(Resources.labels.contact);
            base.AddMetaTag("description", Utils.StripHtml(BlogSettings.Instance.ContactFormMessage));
        }
        #endregion

        #region AddHeader
        /// <summary>
        /// Adds the header.
        /// </summary>
        private void AddHeader()
        {
            var jqmHeader = this.Page.Master.FindControl("jqmHeader");
            var path = Utils.ApplicationRelativeWebRoot + "themes/JQ-Mobile/Controls/Header.ascx";
            dynamic header = LoadControl(path);
            header.Title = labels.contact;
            jqmHeader.Controls.Add(header);
        }
        #endregion

        #region SetFocus
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
        #endregion

        #region btnSend_Click
        /// <summary>
        /// Handles the Click event of the btnSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (!UseCaptcha || IsCaptchaValid)
                {
                    bool success = SendEmail(txtEmail.Text, txtName.Text, txtSubject.Text, txtMessage.Text);
                    divForm.Visible = !success;
                    lblStatus.Visible = !success;
                    divThank.Visible = success;
                    SetCookie();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "captcha-incorrect", " displayIncorrectCaptchaMessage(); ", true);
                }
            }
        }
        #endregion

        #region SendEmail
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="name">The name.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private bool SendEmail(string email, string name, string subject, string message)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(BlogSettings.Instance.Email, name);
                    mail.ReplyTo = new MailAddress(email, name);

                    mail.To.Add(BlogSettings.Instance.Email);
                    mail.Subject = BlogSettings.Instance.EmailSubjectPrefix + " e-mail - " + subject;

                    mail.Body = "<div style=\"font: 11px verdana, arial\">";
                    mail.Body += Server.HtmlEncode(message).Replace("\n", "<br />") + "<br /><br />";
                    mail.Body += "<hr /><br />";
                    mail.Body += "<h3>Author information</h3>";
                    mail.Body += "<div style=\"font-size:10px;line-height:16px\">";
                    mail.Body += "<strong>Name:</strong> " + Server.HtmlEncode(name) + "<br />";
                    mail.Body += "<strong>E-mail:</strong> " + Server.HtmlEncode(email) + "<br />";

                    if (ViewState["url"] != null)
                        mail.Body += string.Format("<strong>Website:</strong> <a href=\"{0}\">{0}</a><br />", ViewState["url"]);

                    if (ViewState["country"] != null)
                        mail.Body += "<strong>Country code:</strong> " + ((string)ViewState["country"]).ToUpperInvariant() + "<br />";

                    if (HttpContext.Current != null)
                    {
                        mail.Body += "<strong>IP address:</strong> " + Utils.GetClientIP() + "<br />";
                        mail.Body += "<strong>User-agent:</strong> " + HttpContext.Current.Request.UserAgent;
                    }

                    Utils.SendMailMessage(mail);
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Security.IsAuthorizedTo(Rights.ViewDetailedErrorMessages))
                {
                    if (ex.InnerException != null)
                    {
                        lblStatus.Text = ex.InnerException.Message;
                    }
                    else
                    {
                        lblStatus.Text = ex.Message;
                    }
                }

                return false;
            }
        }
        #endregion

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
                ViewState["url"] = cookie.Values["url"];
                ViewState["country"] = cookie.Values["country"];
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
        /// Gets whether or not the user is human 
        /// </summary> 
        private bool IsCaptchaValid
        {
            get
            {
                recaptcha.Validate();
                return recaptcha.IsValid;
            }
        }

        private bool UseCaptcha
        {
            get
            {
                return
                    BlogSettings.Instance.EnableRecaptchaOnContactForm &&
                    recaptcha.RecaptchaEnabled &&
                    recaptcha.RecaptchaNecessary;
            }
        }

        #endregion

        #region ICallbackEventHandler Members

        private string _Callback;

        public string GetCallbackResult()
        {
            return _Callback;
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            string[] arg = eventArgument.Split(new string[] { "-||-" }, StringSplitOptions.None);
            if (arg.Length == 6)
            {
                string name = arg[0];
                string email = arg[1];
                string subject = arg[2];
                string message = arg[3];

                string recaptchaResponse = arg[4];
                string recaptchaChallenge = arg[5];

                recaptcha.UserUniqueIdentifier = hfCaptcha.Value;
                if (UseCaptcha)
                {
                    if (!recaptcha.ValidateAsync(recaptchaResponse, recaptchaChallenge))
                    {
                        _Callback = "RecaptchaIncorrect";
                        return;
                    }
                }

                if (SendEmail(email, name, subject, message))
                {
                    _Callback = BlogSettings.Instance.ContactThankMessage;
                }
                else
                {
                    _Callback = "This form does not work at the moment. Sorry for the inconvenience.";
                }
            }
            else
            {
                _Callback = "This form does not work at the moment. Sorry for the inconvenience.";
            }
        }

        #endregion
    }
}