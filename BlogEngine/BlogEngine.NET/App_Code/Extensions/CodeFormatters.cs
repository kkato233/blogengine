#region using

using System;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using BlogEngine.Core;
using BlogEngine.Core.Web;
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
         * T-SQL
         * MSH (code name Monad) --I don't have a clue.
         */
        if (!string.IsNullOrEmpty(e.Body))
        {

            //Characters other than . $ ^ { [ ( | ) * + ? \ match themselves.
            Regex _CSharpRegex = new Regex(@"\[code:c#](.*?)\[code]", RegexOptions.IgnoreCase |  RegexOptions.Compiled);
            Regex _VBRegex = new Regex(@"\[code:vb](.*?)\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Regex _JavaScriptRegex = new Regex(@"\[code:js](.*?)\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Regex _HTMLRegex = new Regex(@"\[code:html](.*?)\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Regex _XMLRegex = new Regex(@"\[code:xml](.*?)\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Regex _TSQLRegex = new Regex(@"\[code:tsql](.*?)\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Regex _MSHRegex = new Regex(@"\[code:msh](.*?)\[code]", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

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
        if (match.Groups[1].Success)
        {
            string matchString = match.ToString();
            string langType = matchString.Remove(0, 6);
            langType = langType.Remove(langType.IndexOf("]"));
            matchString = matchString.Replace("[code]", "");


            switch (langType)
            {
                case "c#":
                    matchString = matchString.Replace("[code:c#]", "");
                    CSharpFormat csf = new CSharpFormat();
                    return csf.FormatCode(matchString);
                case "vb":
                    matchString = matchString.Replace("[code:vb]", "");
                    CodeFormatter.VisualBasicFormat vbf = new VisualBasicFormat();
                    return vbf.FormatCode(matchString);
                case "js":
                    matchString = matchString.Replace("[code:js]", "");
                    CodeFormatter.JavaScriptFormat jsf = new JavaScriptFormat();
                    return jsf.FormatCode(matchString);
                case "html":
                    matchString = matchString.Replace("[code:html]", "");
                    CodeFormatter.HtmlFormat htmlf = new HtmlFormat();
                    return htmlf.FormatCode(matchString);
                case "xml":
                    matchString = matchString.Replace("[code:xml]", "");
                    CodeFormatter.HtmlFormat xmlf = new HtmlFormat();
                    return xmlf.FormatCode(matchString);
                case "tsql":
                    matchString = matchString.Replace("[code:tsql]", "");
                    CodeFormatter.TsqlFormat tsqlf = new TsqlFormat();
                    return tsqlf.FormatCode(matchString);
                case "msh":
                    matchString = matchString.Replace("[code:msh]", "");
                    CodeFormatter.MshFormat mshf = new MshFormat();
                    return mshf.FormatCode(matchString);
            }
        }
        return ""; //no match
    }



}