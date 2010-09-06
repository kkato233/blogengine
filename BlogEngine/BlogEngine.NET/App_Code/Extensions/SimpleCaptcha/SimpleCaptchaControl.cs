// Inspired by and interface heavily borrowed from Filip Stanek's ( http://www.bloodforge.com ) Recaptcha extension for blogengine.net
// SimpleCaptcha created by Aaron Stannard (http://www.aaronstannard.com )

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlogEngine.Core;
using SimpleCaptcha;

namespace Controls
{
    using BlogEngine.Core.Web.Extensions;

    /// <summary>
    /// This is the IValidator control that gets embedded on the comment form if the SimpleCaptcha extension is enabled.
    /// </summary>
    public class SimpleCaptchaControl : WebControl, IValidator
    {

        public SimpleCaptchaControl()
        {}

        #region private fields
        private const string SIMPLE_CAPTCHA_ANSWER_FIELD = "simpleCaptchaValue";
        private string _simpleCaptchaLabel;
        private string _simpleCaptchaAnswer;
        private bool _skipSimpleCaptcha = true;
        private bool _isValid = false;
        private string _errorMessage;
        #endregion

        #region public properties

        /// <summary>
        /// Returns whether the control has been enabled via the Extension Manager
        /// </summary>
        public bool SimpleCaptchaEnabled
        {
            get
            {
                ManagedExtension captchaExtension = ExtensionManager.GetExtension("SimpleCaptcha");
                return captchaExtension.Enabled;
            }
        }

        /// <summary>
        /// Returns whether the recaptcha needs to be displayed for the current user
        /// </summary>
        public bool SimpleCaptchaNecessary
        {
            get
            {
                ExtensionSettings settings = ExtensionManager.GetSettings("SimpleCaptcha");
                return !Page.User.Identity.IsAuthenticated || Convert.ToBoolean(settings.GetSingleValue("ShowForAuthenticatedUsers"));
            }
        }

        #endregion

        #region IValidator Members

        public string ErrorMessage
        {
            get
            {
                if (_errorMessage != null)
                {
                    return _errorMessage;
                }
                return "The captcha value you provided is incorrect.";
            }
            set
            {
                _errorMessage = value;
            }
        }

        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
            set{}
        }

        public void Validate(string simpleCaptchaChallenge)
        {
            if (_skipSimpleCaptcha)
            {
                _isValid = true;
            }
            else
            {
                if (_simpleCaptchaAnswer.Equals(simpleCaptchaChallenge))
                    _isValid = true;
                else
                    _isValid = false;
            }
        }

        public void Validate()
        {
            string simpleCaptchaChallenge = Context.Request.Form[SIMPLE_CAPTCHA_ANSWER_FIELD];
            Validate(simpleCaptchaChallenge);
        }

        #endregion

        #region WebControl Methods (overriden)

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ExtensionSettings settings = ExtensionManager.GetSettings("SimpleCaptcha");
            _simpleCaptchaAnswer = settings.GetSingleValue("CaptchaAnswer");
            _simpleCaptchaLabel = settings.GetSingleValue("CaptchaLabel");

            if (SimpleCaptchaEnabled && SimpleCaptchaNecessary)
            {
                _skipSimpleCaptcha = false;
            }

            if (String.IsNullOrEmpty(_simpleCaptchaAnswer) || String.IsNullOrEmpty(_simpleCaptchaLabel))
            {
                throw new ApplicationException("SimpleCaptcha needs to be configured with an appropriate captcha label and a captcha value.");
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!_skipSimpleCaptcha)
            {
                RenderContents(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.For, SIMPLE_CAPTCHA_ANSWER_FIELD);
            output.RenderBeginTag(HtmlTextWriterTag.Label);
            output.Write(_simpleCaptchaLabel);
            output.RenderEndTag();

            output.AddAttribute(HtmlTextWriterAttribute.Id, SIMPLE_CAPTCHA_ANSWER_FIELD);
            output.AddAttribute(HtmlTextWriterAttribute.Name, SIMPLE_CAPTCHA_ANSWER_FIELD);
            output.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Maxlength,
                                Convert.ToString(SimpleCaptcha.SimpleCaptcha.MAX_CAPTCHA_LENGTH));
            output.AddAttribute(HtmlTextWriterAttribute.Value, string.Empty);
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();

            output.AddAttribute(HtmlTextWriterAttribute.Id, "spnSimpleCaptchaIncorrect");
            output.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            output.AddStyleAttribute(HtmlTextWriterStyle.Color, "Red");
            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.WriteLine(ErrorMessage);
            output.RenderEndTag();
        }

        #endregion
    }

}