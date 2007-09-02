/*
 * 
 * Syntax Highlighting Extension
 * 
 * by Alexander Schuc
 * http://blog.furred.net
 * 
 * for BlogEngine.net (http://www.dotnetblogengine.net)
 * 
 * using syntax highlighter by Wilco Bauwer
 * http://www.wilcob.com/Wilco/Toolbox/SyntaxHighlighter.aspx
 * 
 * licensed under Ms-PL
 * http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 * icon (page_white_code.png) by famfamfam
 * http://www.famfamfam.com/lab/icons/silk/
 * 
 * v0.1.1   - 15. Aug. 2007 - added line numbers
 * v0.1     - 14. Aug. 2007 - Initial Version
 * 
 * 
 * Installation:
 * 
 *  - copy Wilco.SyntaxHighlighter.dll into Bin
 *  - copy SyntaxHighlightingExtension.cs into App_Code/Extensions
 *  - copy SyntaxHighlightTemplate.txt to App_Data/furred
 *    the directory 'furred' will be used by further extensions provided by me
 *  - use it! :)
 *  
 * Additional steps for default style:
 * 
 *  - for the default template copy page_white_code.png into /pics and adjust image url!
 *  - add the CSS code from DefaultStyle.css to your css file
 *  
 * Usage examples:
 * 
 *  - [code=csharp] your code! [/code]
 *  - [code=csharp;ln=on;title] bla bla [/code]
 *  - [code=csharp;A optional title!] your code! [/code]
 *  - [code=anyOtherSupportedLanguage] your code! [/code]
 *  
 * Replacements in template file:
 * 
 *  {0} - ID     - can be used for javascript stuff
 *  {1} - Name   - Fullname of the highlighted language
 *                 or the used tag when language isn't supported
 *  {2} - Title  - Optional title from tag, or empty string
 *  {3} - Code   - the highlighted code
 *                 or the original text inside the tags when the language isn't supported
 *
 * Known issues:
 * 
 *  Tiny MCE can cause problems, because it reformats your entries when switching from html view!
 * 
 * Supported tags/languages
 *    
 *    Use this name for       
 *    better performance      These tags are working too
 *    
 *  - ASPX                  - aspx aspnet aspx ascx asax ashx
 *  - C                     - c
 *  - COBOL                 - cob
 *  - ColdFusion            - cfm coldfusion
 *  - CPP                   - cpp c++
 *  - C#                    - csharp c# cs
 *  - CSS                   - css
 *  - Eiffel                - e
 *  - Fortran               - for
 *  - Haskell               - hs
 *  - Java                  - java
 *  - JavaScript            - js javascript
 *  - JScript               - jscript
 *  - Mercury               - m
 *  - MSIL                  - pe
 *  - Pascal                - pas
 *  - Perl                  - pl
 *  - PHP                   - php php3
 *  - Python                - py
 *  - Ruby                  - ruby
 *  - SQL                   - sql
 *  - VisualBasic           - vb
 *  - VisualBasicScript     - vbs
 *  - XML                   - xml html htm
 * 
 */


using System;
using System.Text;
using System.Text.RegularExpressions;

using System.Web;
using System.Diagnostics;

using BlogEngine.Core.Web.Controls;
using BlogEngine.Core;
using Wilco.SyntaxHighlighting;

using System.Web.Caching;
using System.IO;

/// <summary>
/// Small syntax highlighting extension for BlogEngine.net
/// </summary>
[ExtensionAttribute("Syntax highlighting for BlogEngine.net", "0.1", "Alexander Schuc")]
public sealed class SyntaxHighlightingExtension
{
	private IParser htmlParser;
	private uint codeID = uint.MinValue;

	private static readonly string templatePath = "~/App_Data/SyntaxHighlightTemplate.txt";
    private static readonly string outputTemplateDefault = "<div class=\"codeSnippet\"><div class=\"codeHeader\" onclick=\"document.getElementById('{0}').style.display = document.getElementById('{0}').style.display == 'none' ? 'block' : 'none';\" title=\"Click for expanding..\"><img src=\"/pics/page_white_code.png\" />{1}-Code: {2}</div><pre id=\"{0}\" class=\"codeContainer\">{3}</pre></div><script type=\"text/javascript\">document.getElementById('{0}').style.display='none';</script>";
	private static readonly string linenumberingTemplate = "<span class=\"lineNumber\">{0:000}</span><span class=\"numberedCode\">{1}</span>";

	private readonly string outputTemplate = null;
	private static readonly string cacheKey = "furred.Syntax.outputTemplate";

	private Regex codeRegex = new Regex(@"\[code=(?<lang>.*?)(?:;ln=(?<linenumbers>(?:on|off)))?(?:;(?<title>.*?))?\](?<code>.*?)\[/code\]",
		RegexOptions.Compiled
		| RegexOptions.CultureInvariant
		| RegexOptions.IgnoreCase
		| RegexOptions.Singleline);

	public SyntaxHighlightingExtension()
	{
		htmlParser = new HtmlParser();

		Page.Serving += new EventHandler<ServingEventArgs>(ServingContent);
		Post.Serving += new EventHandler<ServingEventArgs>(ServingContent);
	}

	private void ServingContent(object sender, ServingEventArgs e)
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

		return Highlight(options);
	}

	private string Highlight(HighlightOptions options)
	{
		string parsed;
		uint id = NextCodeID();
		string name = options.Language;

		HighlighterBase highlighter = GetHighlighter(name);
		if (highlighter != null)
		{
			name = highlighter.FullName;
			highlighter.Parser = htmlParser;

			parsed = highlighter.Parse(options.Code);

			highlighter.ForceReset();
		}
		else
		{
			name += " (not highlighted)";
			parsed = options.Code;
		}

		if (options.DisplayLineNumbers)
		{
			string[] lines = parsed.Split(new char[] { '\n' });
			StringBuilder outputBuffer = new StringBuilder();

			for (int i = 0; i < lines.Length; i++)
			{
				outputBuffer.AppendFormat(linenumberingTemplate, i+1, lines[i]);
			}

			return string.Format(OutputTemplate, id, name, options.Title, outputBuffer);
		}

		return string.Format(OutputTemplate, id, name, options.Title, parsed);
	}

	private HighlighterBase GetHighlighter(string name)
	{
		string upperName = name.ToUpperInvariant();
		foreach (HighlighterBase hl in Register.Instance.Highlighters)
		{
			if (hl.Name.ToUpperInvariant().Equals(upperName)
				|| hl.TagValues.Contains(name)
				|| hl.FileExtensions.Contains(name))
			{
				return hl;
			}
		}
		return null;
	}

	private uint NextCodeID()
	{
		return codeID == uint.MaxValue ? uint.MinValue : codeID++;
	}

	public string OutputTemplate
	{
		get
		{
			string filename = HttpContext.Current.Server.MapPath(templatePath);

			if (HttpContext.Current == null)
			{
				return ReadTemplate(filename) ?? outputTemplateDefault;
			}

			if (HttpContext.Current.Cache[cacheKey] == null)
			{
				string template = ReadTemplate(filename);

				if (template == null)
					return outputTemplateDefault;

				HttpContext.Current.Cache.Add(cacheKey, template,
					new CacheDependency(filename), Cache.NoAbsoluteExpiration,
					TimeSpan.FromHours(1), CacheItemPriority.Normal, null);
			}

			return HttpContext.Current.Cache[cacheKey] as string;
		}
	}

	private string ReadTemplate(string filename)
	{
		if (!File.Exists(filename))
		{
			return null;
		}

		return File.ReadAllText(filename);
	}
	private class HighlightOptions
	{
		private string language, title, code;
		private bool displayLineNumbers = false;

		public HighlightOptions()
		{
		}

		public HighlightOptions(string language, string title, bool linenumbers, string code)
		{
			this.language = language;
			this.title = title;
			this.displayLineNumbers = linenumbers;
			this.code = code;
		}

		public string Code
		{
			get { return HttpContext.Current.Server.HtmlDecode(code); }
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

	}
}
