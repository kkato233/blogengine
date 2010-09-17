// Inspired by and interface heavily borrowed from Filip Stanek's ( http://www.bloodforge.com ) Recaptcha extension for blogengine.net
// SimpleCaptcha created by Aaron Stannard (http://www.aaronstannard.com )

using System;
using System.Collections.Generic;
using System.Web;
using BlogEngine.Core.Web.Controls;

namespace SimpleCaptcha
{
    /// <summary>
    /// Builds the SimpleCaptcha control
    /// </summary>
    [Extension("Settings for the SimpleCaptcha control", "1.0", "<a href=\"http://www.aaronstannard.com\">Aaron Stannard</a>",2)]
    public class SimpleCaptcha
    {
        static protected ExtensionSettings _settings;
        /// <summary>
        /// The maximum length of a SimpleCaptcha expected value.
        /// </summary>
        public const int MAX_CAPTCHA_LENGTH = 30;

        public SimpleCaptcha()
        {
            InitSettings();
        }

        #region private members

        private void InitSettings()
        {
            ExtensionSettings settings = new ExtensionSettings(this);
            settings.IsScalar = true;

            settings.AddParameter("CaptchaLabel", "Your captcha's label", 30, true, true, ParameterType.String);
            settings.AddValue("CaptchaLabel", "5+5 = ");

            settings.AddParameter("CaptchaAnswer", "Your captcha's expected value", MAX_CAPTCHA_LENGTH, true, true, ParameterType.String);
            settings.AddValue("CaptchaAnswer", "10");

            settings.AddParameter("ShowForAuthenticatedUsers", "Show Captcha For Authenticated Users", 1, true, false, ParameterType.Boolean);
            settings.AddValue("ShowForAuthenticatedUsers", false);

            settings.Help = @"To get started with SimpleCaptcha, just provide some captcha instructions for your users in the <b>CaptchaLabel</b>
                                field and the value you require from your users in order to post a comment in the <b>CaptchaAnswer</b> field.";
            _settings = ExtensionManager.InitSettings("SimpleCaptcha", settings);

            ExtensionManager.SetStatus("SimpleCaptcha", false);
        }

        #endregion
    }
}