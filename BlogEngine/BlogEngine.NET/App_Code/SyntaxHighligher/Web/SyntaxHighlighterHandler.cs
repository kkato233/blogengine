using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Wilco.SyntaxHighlighting;
using System.Web.UI.WebControls;

namespace Wilco.Web.SyntaxHighlighting
{
    /// <summary>
    /// Represents a syntax highlighter handler for ASP.NET.
    /// </summary>
    public class SyntaxHighlighterHandler : IHttpHandler
    {
        /// <summary>
        /// Gets whether the handler is reusable.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public SyntaxHighlighterHandler()
        {
            //
        }

        public void ProcessRequest(HttpContext context)
        {
            string absoluteFilePath = context.Server.MapPath(context.Request.FilePath);
            string extension = Path.GetExtension(absoluteFilePath).Substring(1);

            string language = "C#"; // Default language.

            foreach (HighlighterBase h in Register.Instance.Highlighters)
            {
                if (h.FileExtensions.Contains(extension))
                {
                    language = h.Name;
                    break;
                }
            }
            
            Page page = new Page();
            page.Controls.Add(new LiteralControl(@"<?xml version=""1.0""?>
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html>"));

            HtmlHead header = new HtmlHead();
            page.Controls.Add(header);
            header.Title = "Source for " + Path.GetFileName(absoluteFilePath);

            PlaceHolder body = new PlaceHolder();
            page.Controls.Add(body);

            page.Controls.Add(new LiteralControl(@"</body>
</html>"));

            SyntaxHighlighter highlighter = new SyntaxHighlighter();
            highlighter.Language = language;
            highlighter.Mode = HighlightMode.Source;
            highlighter.Text = File.ReadAllText(absoluteFilePath);
            body.Controls.Add(highlighter);

            ((IHttpHandler)page).ProcessRequest(context);
        }
    }
}