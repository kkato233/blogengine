using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}