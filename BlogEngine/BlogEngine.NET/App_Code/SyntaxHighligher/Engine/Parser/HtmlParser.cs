using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Wilco.SyntaxHighlighting
{
	/// <summary>
	/// Represents an Html parser.
	/// </summary>
	public class HtmlParser : ParserBase
	{
		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.HtmlParser"/> class.
		/// </summary>
		public HtmlParser()
		{
            //
		}

        /// <summary>
		/// Parses source code.
		/// </summary>
		/// <param name="source">The source code which will be parsed.</param>
		/// <param name="scannerResult">The result returned by the scanners after scanning the source code.</param>
		/// <returns>The highlighted source code.</returns>
        public override string Parse(string source, IList<Occurrence> scannerResult)
        {
            StringWriter htmlStringWriter = new StringWriter();
            XhtmlTextWriter htmlWriter = new XhtmlTextWriter(htmlStringWriter);

            int lastIndex = 0;

            for (int i = 0; i < scannerResult.Count; i++)
            {
                if ((scannerResult[i].Start - lastIndex) >= 0)
                {
                    if (scannerResult[i].Start > 0)
                    {
                        // Encode non-highlighted text first.
                        htmlWriter.WriteEncodedText(source.Substring(lastIndex, scannerResult[i].Start - lastIndex));
                    }

                    htmlWriter.Write(this.ParseToken(source.Substring(scannerResult[i].Start, scannerResult[i].Length), scannerResult[i].Node));
                }

                lastIndex = scannerResult[i].Start + scannerResult[i].Length;
            }

            // Encode the last bit of nonhighlighted text.
            if (lastIndex < source.Length)
            {
                htmlWriter.WriteEncodedText(source.Substring(lastIndex));
            }

            string result = htmlStringWriter.ToString();

            htmlWriter.Close();
            htmlStringWriter.Close();

            return result;
        }

		/// <summary>
		/// Parses a token.
		/// </summary>
		/// <param name="token">The token which the node matched.</param>
		/// <param name="node">The node which matched the token.</param>
		/// <returns>The highlighted token.</returns>
        protected override string ParseToken(string token, INode node)
        {
            token = HttpUtility.HtmlEncode(token);
            string style = this.GetStyle(node);

            if (String.IsNullOrEmpty(node.NavigateUrl))
            {
                return String.Format("<span style=\"{0}\">{1}</span>", style, token);
            }
            
            return String.Format("<a href=\"{2}\" style=\"{0}\">{1}</a>", style, token, String.Format(node.NavigateUrl, token));
        }

        private string GetStyle(INode node)
        {
            if (node.BackColor != Color.Transparent)
                return String.Format("background-color:{0}; color:{1}", HtmlParser.ToHtmlColor(node.BackColor), ColorTranslator.ToHtml(node.ForeColor));

            return String.Format("color:{0}", HtmlParser.ToHtmlColor(node.ForeColor));
        }

        private static string ToHtmlColor(Color color)
        {
            return ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb()));
        }
	}
}