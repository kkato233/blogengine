#region using

using System;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
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
public class CodeFormatters
{

    //Characters other than . $ ^ { [ ( | ) * + ? \ match themselves.
    //Pretty close with this  <code[^>]*>[\w|\t|\r|\W]*</code>
    static readonly Regex _CSharpRegex = new Regex(@"\[code\:c\#].*\[code]", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);
    static readonly Regex _VBRegex = new Regex(@"\[code\:vb].*\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    static readonly Regex _JavaScriptRegex = new Regex(@"\[code\:js].*\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    static readonly Regex _HTMLRegex = new Regex(@"\[code\:html].*\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    static readonly Regex _XMLRegex = new Regex(@"\[code\:xml].*\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    static readonly Regex _TSQLRegex = new Regex(@"\[code\:tsql].*\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    static readonly Regex _MSHRegex = new Regex(@"\[code\:msh].*\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

    private enum CodeLanguages
    {
        csharp, vb, js, html, xml, tsql, msh
    }

    public CodeFormatters()
    {
        Post.Serving += new EventHandler<ServingEventArgs>(Post_Serving);
    }

    void Post_Serving(object sender, ServingEventArgs e)
    {
        /*  It supports the following.
         * C#
         * VB
         * JS
         * HTML
         * XML
         * TSQL
         * MSH (code name Monad) --I don't have a clue.
         */
        if (!string.IsNullOrEmpty(e.Body))
        {
            e.Body = _CSharpRegex.Replace(e.Body, new MatchEvaluator(MatchEval));
            e.Body = _VBRegex.Replace(e.Body, new MatchEvaluator(MatchEval));
            e.Body = _JavaScriptRegex.Replace(e.Body, new MatchEvaluator(MatchEval));
            e.Body = _HTMLRegex.Replace(e.Body, new MatchEvaluator(MatchEval));
            e.Body = _XMLRegex.Replace(e.Body, new MatchEvaluator(MatchEval));
            e.Body = _TSQLRegex.Replace(e.Body, new MatchEvaluator(MatchEval));
            e.Body = _MSHRegex.Replace(e.Body, new MatchEvaluator(MatchEval));

        }
    }

    private static string MatchEval(Match match)
    {
        foreach (Match mat in match.Groups)
        {
            if (mat.Success)
            {
                string matchString = match.ToString();
                string langType = matchString.Remove(0, 6);
                langType = langType.Remove(langType.IndexOf("]"));

                switch (langType)
                {
                    case "c#":
                        matchString = matchString.Replace("[code:c#]", "");
                        matchString = matchString.Replace("[code]", "");
                        CSharpFormat csf = new CSharpFormat();
                        return HttpContext.Current.Server.HtmlDecode(csf.FormatCode(matchString));
                    case "vb":
                        matchString = matchString.Replace("[code:vb]", "");
                        matchString = matchString.Replace("[code]", "");
                        CodeFormatter.VisualBasicFormat vbf = new VisualBasicFormat();
                        return HttpContext.Current.Server.HtmlEncode(vbf.FormatCode(matchString));
                    case "js":
                        matchString = matchString.Replace("[code:js]", "");
                        matchString = matchString.Replace("[code]", "");
                        CodeFormatter.JavaScriptFormat jsf = new JavaScriptFormat();
                        return jsf.FormatCode(matchString);
                    case "html":
                        matchString = matchString.Replace("[code:html]", "");
                        matchString = matchString.Replace("[code]", "");
                        CodeFormatter.HtmlFormat htmlf = new HtmlFormat();
                        return htmlf.FormatCode(matchString);
                    case "xml":
                        matchString = matchString.Replace("[code:xml]", "");
                        matchString = matchString.Replace("[code]", "");
                        CodeFormatter.HtmlFormat xmlf = new HtmlFormat();
                        return xmlf.FormatCode(matchString);
                    case "tsql":
                        matchString = matchString.Replace("[code:tsql]", "");
                        matchString = matchString.Replace("[code]", "");
                        CodeFormatter.TsqlFormat tsqlf = new TsqlFormat();
                        return tsqlf.FormatCode(matchString);
                    case "msh":
                        matchString = matchString.Replace("[code:msh]", "");
                        matchString = matchString.Replace("[code]", "");
                        CodeFormatter.MshFormat mshf = new MshFormat();
                        return mshf.FormatCode(matchString);
                }
            }
        }
        return ""; //no match
    }
}
