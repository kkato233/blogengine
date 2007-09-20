#region using

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Caching;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using CodeFormatter;

#endregion


/// <summary>
/// Changes smilies from text to image inside a comment post.
/// </summary>
/// <remarks>
/// It is a work in progress.....
/// </remarks>
[Extension("Changes <code:lang></code>.  Adapted from Jean-Claude Manoli [jc@manoli.net].", "0.0.0.1", "www.manoli.net")]
public class CodeFormatterExtension
{
    #region Constructors

    /// <summary>
    /// Maps custom events to the ServingContent event
    /// </summary>
    public CodeFormatterExtension()
    {
        Page.Serving += new EventHandler<ServingEventArgs>(ServingContent);
        Post.Serving += new EventHandler<ServingEventArgs>(ServingContent);
    } 

    #endregion

    #region RegEx

    private Regex codeRegex = new Regex(@"\[code:(?<lang>.*?)(?:;ln=(?<linenumbers>(?:on|off)))?(?:;alt=(?<altlinenumbers>(?:on|off)))?(?:;(?<title>.*?))?\](?<code>.*?)\[/code\]",
RegexOptions.Compiled
| RegexOptions.CultureInvariant
| RegexOptions.IgnoreCase
| RegexOptions.Singleline);

    /// new regex <p>\r\n<div class=\"code\">\n[code:c#]\r\n</p>\r\n
    /// <summary>
    /// old regex <p><div.*?>\[code:.*?\]</p>
    private Regex codeBeginTagRegex = new Regex(@"<p>\r\n<div class=""code"">\n\[code:.*?\]\r\n</p>\r\n",
    RegexOptions.Compiled
    | RegexOptions.CultureInvariant
    | RegexOptions.IgnoreCase
    | RegexOptions.Singleline);
    
    
    /// Old regex\[/code\]
    private Regex codeEndTagRegex = new Regex(@"<p>\r\n\[/code]</div>\r\n</p>\r\n",
    RegexOptions.Compiled
    | RegexOptions.CultureInvariant
    | RegexOptions.IgnoreCase
    | RegexOptions.Singleline); 

    #endregion
    
    /// <summary>
    /// An event that handles ServingEventArgs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ServingContent(object sender, ServingEventArgs e)
    {
      if (e.Body.Contains("[/code]"))
      {
        e.Body = codeRegex.Replace(e.Body, new MatchEvaluator(CodeEvaluator));
        e.Body = codeBeginTagRegex.Replace(e.Body, @"<div class=""code"">");
        e.Body = codeEndTagRegex.Replace(e.Body, @"</div>");
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    private string CodeEvaluator(Match match)
    {
        if (!match.Success)
            return match.Value;

        HighlightOptions options = new HighlightOptions();

        options.Language = match.Groups["lang"].Value;
        options.Code = match.Groups["code"].Value;
        options.DisplayLineNumbers = match.Groups["linenumbers"].Value == "on" ? true : false;
        options.Title = match.Groups["title"].Value;
        options.AlternateLineNumbers = match.Groups["altlinenumbers"].Value == "on" ? true : false;

        return Highlight(options, match.Value);
    }

    /// <summary>
    /// Returns the formatted text.
    /// </summary>
    /// <param name="options">Whatever options were set in the regex groups.</param>
    /// <param name="text">Send the e.body so it can get formatted.</param>
    /// <returns>The formatted string of the match.</returns>
    private string Highlight(HighlightOptions options, string text)
    {

        switch (options.Language)
        {
            case "c#":
                CSharpFormat csf = new CSharpFormat();
                csf.LineNumbers = options.DisplayLineNumbers;
                csf.Alternate = options.AlternateLineNumbers;
                return HttpContext.Current.Server.HtmlDecode(csf.FormatCode(text));

            case "vb":
                VisualBasicFormat vbf = new VisualBasicFormat();
                vbf.LineNumbers = options.DisplayLineNumbers;
                vbf.Alternate = options.AlternateLineNumbers;
                return vbf.FormatCode(text);

            case "js":
                JavaScriptFormat jsf = new JavaScriptFormat();
                jsf.LineNumbers = options.DisplayLineNumbers;
                jsf.Alternate = options.AlternateLineNumbers;
                return HttpContext.Current.Server.HtmlDecode(jsf.FormatCode(text));

            case "html":
                HtmlFormat htmlf = new HtmlFormat();
                htmlf.LineNumbers = options.DisplayLineNumbers;
                htmlf.Alternate = options.AlternateLineNumbers;
                text = text.Replace("<br />", string.Empty);
                string code = htmlf.FormatCode(HttpContext.Current.Server.HtmlDecode(text));
                return code.Replace(Environment.NewLine, "<br />");

            case "xml":
                HtmlFormat xmlf = new HtmlFormat();
                xmlf.LineNumbers = options.DisplayLineNumbers;
                xmlf.Alternate = options.AlternateLineNumbers;
                return HttpContext.Current.Server.HtmlDecode(xmlf.FormatCode(text));

            case "tsql":
                TsqlFormat tsqlf = new TsqlFormat();
                tsqlf.LineNumbers = options.DisplayLineNumbers;
                tsqlf.Alternate = options.AlternateLineNumbers;
                return HttpContext.Current.Server.HtmlDecode(tsqlf.FormatCode(text));

            case "msh":
                MshFormat mshf = new MshFormat();
                mshf.LineNumbers = options.DisplayLineNumbers;
                mshf.Alternate = options.AlternateLineNumbers;
                return HttpContext.Current.Server.HtmlDecode(mshf.FormatCode(text));
        }

        return string.Empty;
    }

    /// <summary>
    /// Handles all of the options for changing the rendered code.
    /// </summary>
    private class HighlightOptions
    {
        private string language, title, code;
        private bool displayLineNumbers = false;
        private bool alternateLineNumbers = false;
        private bool usejavascript = true;

        public HighlightOptions()
        {
        }

        public HighlightOptions(string language, string title, bool linenumbers, string code, bool alternateLineNumbers)
        {
            this.language = language;
            this.title = title;
            this.alternateLineNumbers = alternateLineNumbers;
            this.code = code;
            this.displayLineNumbers = linenumbers;
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public bool DisplayLineNumbers
        {
            get { return displayLineNumbers; }
            set { displayLineNumbers = value; }
        }
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public bool AlternateLineNumbers
        {
            get { return alternateLineNumbers; }
            set { alternateLineNumbers = value; }
        }
    }
}
