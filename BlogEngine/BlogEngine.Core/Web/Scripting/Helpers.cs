﻿using System;
using System.Text;
using System.Globalization;
using System.Web.UI;

namespace BlogEngine.Core.Web.Scripting
{
    /// <summary>
    /// Helper methods for script manipulations
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// Add stylesheet to page header
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="lnk">Href link</param>
        public static void AddStyle(System.Web.UI.Page page, string lnk)
        {
            page.Header.Controls.Add(new LiteralControl(
                string.Format("\n<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", lnk)));
        }
        /// <summary>
        /// Add generic lit to the page
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="type">Type</param>
        /// <param name="relation">Relation</param>
        /// <param name="title">Title</param>
        /// <param name="href">Url</param>
        public static void AddGenericLink(System.Web.UI.Page page, string type, string relation, string title, string href)
        {
            var tp = string.IsNullOrEmpty(type) ? "" : string.Format("type=\"{0}\" ", type);
            const string tag = "\n<link {0}rel=\"{1}\" title=\"{2}\" href=\"{3}\" />";
            page.Header.Controls.Add(new LiteralControl(string.Format(tag, tp, relation, title, href)));
        }
        /// <summary>
        /// Add javascript to page
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="src">Source</param>
        /// <param name="top">If add to page header</param>
        /// <param name="defer">Defer</param>
        /// <param name="asnc">Async</param>
        public static void AddScript(System.Web.UI.Page page, string src, bool top = true, bool defer = false, bool asnc = false)
        {
            var d = defer ? " defer=\"defer\"" : "";
            var a = asnc ? " async=\"async\"" : "";
            var t = "\n<script type=\"text/javascript\" src=\"{0}\"{1}{2}></script>";
            t = string.Format(t, src, d, a);

            if (top)
            {
                page.Header.Controls.Add(new LiteralControl(t));
            }
            else
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), src.GetHashCode().ToString(), t, false);
            }
        }
        /// <summary>
        /// Format inline script
        /// </summary>
        /// <param name="script">JavaScript code</param>
        /// <returns>Formatted script</returns>
        public static string FormatInlineScript(string script)
        {
            var sb = new StringBuilder();

            sb.Append("\n<script type=\"text/javascript\"> \n");
            sb.Append("//<![CDATA[ \n");
            sb.Append(script).Append(" \n");
            sb.Append("//]]> \n");
            sb.Append("</script> \n");

            return sb.ToString();
        }

        #region BlogBasePage helpers

        /// <summary>
        /// Adds code to the HTML head section.
        /// </summary>
        public static void AddCustomCodeToHead(System.Web.UI.Page page)
        {
            if (string.IsNullOrEmpty(BlogSettings.Instance.HtmlHeader))
                return;

            var code = string.Format(
                CultureInfo.InvariantCulture,
                "{0}<!-- Start custom code -->{0}{1}{0}<!-- End custom code -->{0}",
                Environment.NewLine,
                BlogSettings.Instance.HtmlHeader);
            var control = new LiteralControl(code);
            page.Header.Controls.Add(control);
        }

        /// <summary>
        /// Adds a JavaScript to the bottom of the page at runtime.
        /// </summary>
        /// <remarks>
        /// You must add the script tags to the BlogSettings.Instance.TrackingScript.
        /// </remarks>
        public static void AddTrackingScript(System.Web.UI.Page page)
        {
            var sb = new StringBuilder();

            if (BlogSettings.Instance.ModerationType == BlogSettings.Moderation.Disqus)
            {
                sb.Append("<script type=\"text/javascript\"> \n");
                sb.Append("//<![CDATA[ \n");
                sb.Append("(function() { ");
                sb.Append("var links = document.getElementsByTagName('a'); ");
                sb.Append("var query = '?'; ");
                sb.Append("for(var i = 0; i < links.length; i++) { ");
                sb.Append("if(links[i].href.indexOf('#disqus_thread') >= 0) { ");
                sb.Append("query += 'url' + i + '=' + encodeURIComponent(links[i].href) + '&'; ");
                sb.Append("}}");
                sb.Append("document.write('<script charset=\"utf-8\" type=\"text/javascript\" src=\"http://disqus.com/forums/");
                sb.Append(BlogSettings.Instance.DisqusWebsiteName);
                sb.Append("/get_num_replies.js' + query + '\"></' + 'script>'); ");
                sb.Append("})(); \n");
                sb.Append("//]]> \n");
                sb.Append("</script> \n");
            }

            if (!string.IsNullOrEmpty(BlogSettings.Instance.TrackingScript))
            {
                sb.Append(BlogSettings.Instance.TrackingScript);
            }

            var s = sb.ToString();
            if (!string.IsNullOrEmpty(s))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "tracking", string.Format("\n{0}", s), false);
            }
        }

        /// <summary>
        /// Add bundles created by web.optimization
        /// </summary>
        /// <param name="page">Base page</param>
        public static void AddBudnledStylesAndScripts(System.Web.UI.Page page)
        {
            if (Security.IsAuthenticated)
            {
                AddStyle(page, string.Format("{0}Styles/cssauth", Utils.RelativeWebRoot));
                AddScript(page, string.Format("{0}Scripts/jsauth", Utils.RelativeWebRoot), false, true, true);
            }
            else
            {
                AddStyle(page, string.Format("{0}Styles/css", Utils.RelativeWebRoot));
                AddScript(page, string.Format("{0}Scripts/js", Utils.RelativeWebRoot), false, true, true);
            }

            var resourcePath = HttpHandlers.ResourceHandler.GetScriptPath(new CultureInfo(BlogSettings.Instance.Language));
            AddScript(page, resourcePath, false, true, true);
        }

        #endregion
    }
}