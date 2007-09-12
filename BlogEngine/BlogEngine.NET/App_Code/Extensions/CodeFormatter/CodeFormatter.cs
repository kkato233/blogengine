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
  private Regex codeRegex = new Regex(@"\[code:(?<lang>.*?)(?:;ln=(?<linenumbers>(?:on|off)))?(?:;alt=(?<altlinenumbers>(?:on|off)))?(?:;(?<title>.*?))?\](?<code>.*?)\[/code\]",
      RegexOptions.Compiled
      | RegexOptions.CultureInvariant
      | RegexOptions.IgnoreCase
      | RegexOptions.Singleline);

  public CodeFormatterExtension()
  {
    Page.Serving += new EventHandler<ServingEventArgs>(ServingContent);
    Post.Serving += new EventHandler<ServingEventArgs>(ServingContent);
  }

  void ServingContent(object sender, ServingEventArgs e)
  {
    e.Body = codeRegex.Replace(e.Body, new MatchEvaluator(CodeEvaluator));
  }

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

  private string Highlight(HighlightOptions options, string text)
  {
    text = text.Replace("[/code]", string.Empty);
    text = text.Replace("[code:" + options.Language + "]", string.Empty);

    if (text.StartsWith("<br />"))
      text = text.Substring(6);

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

  // THIS METHOD ISN'T USED ANYWHERE
  //private static string MatchEval(Match match)
  //{
  //  foreach (Match mat in match.Groups)
  //  {
  //    if (mat.Success)
  //    {
  //      string matchString = match.ToString();
  //      string langType = matchString.Remove(0, 6);
  //      langType = langType.Remove(langType.IndexOf("]"));

  //      switch (langType)
  //      {
  //        case "c#":
  //          matchString = matchString.Replace("[code:c#]", "");
  //          matchString = matchString.Replace("[/code]", "");
  //          CSharpFormat csf = new CSharpFormat();
  //          return HttpContext.Current.Server.HtmlDecode(csf.FormatCode(matchString));
  //        case "vb":
  //          matchString = matchString.Replace("[code:vb]", "");
  //          matchString = matchString.Replace("[/code]", "");
  //          CodeFormatter.VisualBasicFormat vbf = new VisualBasicFormat();
  //          return HttpContext.Current.Server.HtmlEncode(vbf.FormatCode(matchString));
  //        case "js":
  //          matchString = matchString.Replace("[code:js]", "");
  //          matchString = matchString.Replace("[/code]", "");
  //          CodeFormatter.JavaScriptFormat jsf = new JavaScriptFormat();
  //          return jsf.FormatCode(matchString);
  //        case "html":
  //          matchString = matchString.Replace("[code:html]", "");
  //          matchString = matchString.Replace("[/code]", "");
  //          CodeFormatter.HtmlFormat htmlf = new HtmlFormat();
  //          return htmlf.FormatCode(matchString);
  //        case "xml":
  //          matchString = matchString.Replace("[code:xml]", "");
  //          matchString = matchString.Replace("[/code]", "");
  //          CodeFormatter.HtmlFormat xmlf = new HtmlFormat();
  //          return xmlf.FormatCode(matchString);
  //        case "tsql":
  //          matchString = matchString.Replace("[code:tsql]", "");
  //          matchString = matchString.Replace("[/code]", "");
  //          CodeFormatter.TsqlFormat tsqlf = new TsqlFormat();
  //          return tsqlf.FormatCode(matchString);
  //        case "msh":
  //          matchString = matchString.Replace("[code:msh]", "");
  //          matchString = matchString.Replace("[/code]", "");
  //          CodeFormatter.MshFormat mshf = new MshFormat();
  //          return mshf.FormatCode(matchString);
  //      }
  //    }
  //  }
  //  return ""; //no match
  //}

  private class HighlightOptions
  {
    private string language, title, code;
    private bool displayLineNumbers = false;
    private bool alternateLineNumbers = false;

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
