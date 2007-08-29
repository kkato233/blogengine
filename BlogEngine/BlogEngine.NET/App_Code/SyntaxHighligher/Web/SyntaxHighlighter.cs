using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Wilco.SyntaxHighlighting;

namespace Wilco.Web.SyntaxHighlighting
{
	/// <summary>
	/// Represents a syntax highligher.
	/// </summary>
	[ParseChildren(true), PersistChildren(false)]
	public partial class SyntaxHighlighter : WebControl
	{
        private bool rendered;
		private HighlightMode mode;
		private IParser htmlParser;
		private ITemplate codeLayoutTemplate;
        private ITemplate lineNumberTemplate;
        private ITemplate sourceLineTemplate;
        private ITemplate separatorTemplate;

		private static readonly object EventParseToken = new object();

		/// <summary>
		/// Gets or sets the highlight mode.
		/// </summary>
		[Description("The highlight mode.")]
		public HighlightMode Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				if (value != this.mode)
				{
					if (this.mode == HighlightMode.Source || this.mode == HighlightMode.Inline || this.mode == HighlightMode.Literal)
						this.mode = value;
					else
						throw new ArgumentException("Hightlight mode must either be source, inline or literal.", "mode");
				}
			}
		}

		/// <summary>
		/// Gets or sets the default language in which the source code is written.
		/// </summary>
		[Description("The default language in which the source code is written.")]
		public string Language
		{
			get
			{
				object savedState = this.ViewState["Language"];
				if (savedState != null)
					return (string)savedState;
				return "C#";
			}
			set
			{
				if (value == null || value.Length == 0 || Register.Instance.Highlighters[value] == null)
					throw new ArgumentException("Highlighter with the specified name does not exist.", "Language");
				this.ViewState["Language"] = value;
			}
		}

		/// <summary>
		/// Gets or sets text.
		/// </summary>
		[Description("Gets or sets the text.")]
		public string Text
		{
			get
			{
				object savedState = this.ViewState["Text"];
				if (savedState != null)
					return (string)savedState;
				return String.Empty;
			}
			set
			{
				this.ViewState["Text"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the layout template of a code snippet.
		/// </summary>
		[
        Description("Gets or sets the layout template of a code snippet."),
        PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(SourceHeaderItem)),
        TemplateInstance(TemplateInstance.Single)
        ]
		public ITemplate CodeLayoutTemplate
		{
			get
			{
				return this.codeLayoutTemplate;
			}
			set
			{
				this.codeLayoutTemplate = value;
			}
		}

        /// <summary>
        /// Gets or sets the line number template.
        /// </summary>
        [
        Description("The template for each line number."),
        TemplateContainer(typeof(SourceItem)),
        TemplateInstance(TemplateInstance.Single),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public ITemplate LineNumberTemplate
        {
            get
            {
                return this.lineNumberTemplate;
            }
            set
            {
                this.lineNumberTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets the source line template.
        /// </summary>
        [
        Description("The template for each line of source code."),
        TemplateContainer(typeof(SourceItem)),
        TemplateInstance(TemplateInstance.Single), 
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public ITemplate SourceLineTemplate
        {
            get
            {
                return this.sourceLineTemplate;
            }
            set
            {
                this.sourceLineTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets the separator template.
        /// </summary>
        [Description("The separator template which is used to separate both numbers and source lines."), TemplateContainer(typeof(SeparatorItem)), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate SeparatorTemplate
        {
            get
            {
                return this.separatorTemplate;
            }
            set
            {
                if (value != this.separatorTemplate)
                {
                    this.separatorTemplate = value;
                }
            }
        }

		/// <summary>
		/// Occurs when a token is parsed.
		/// </summary>
		public event ParseEventHandler ParseToken
		{
			add
			{
				this.Events.AddHandler(EventParseToken, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventParseToken, value);
			}
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.Web.SyntaxHighlighting.SyntaxHighlighter"/> class.
		/// </summary>
		public SyntaxHighlighter() : base(HtmlTextWriterTag.Div)
		{
			this.mode = HighlightMode.Inline;
            this.codeLayoutTemplate = new DefaultCodeLayoutTemplate();
            this.lineNumberTemplate = new DefaultLineNumberTemplate();
            this.sourceLineTemplate = new DefaultSourceLineTemplate();
		}

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            if (this.Page != null)
                this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);

            base.OnInit(e);
            this.EnsureParser();
        }

        /// <summary>
        /// Gets the parser which will be used to parse the source.
        /// </summary>
        /// <returns></returns>
        protected virtual IParser GetParser()
        {
            return new HtmlParser();
        }

        /// <summary>
        /// Parses source code.
        /// </summary>
        /// <param name="language">The language used to highlight the text.</param>
        /// <returns>The highlighter.</returns>
        protected virtual HighlighterBase GetHighlighter(string language)
        {
            Register register = Register.Instance;
            HighlighterBase highlighter = register.Highlighters[language];
            if (highlighter == null)
                highlighter = register.Highlighters[this.Language];

            this.EnsureParser();
            highlighter = highlighter.Create();
            highlighter.Parser = this.htmlParser;
            highlighter.ForceReset();
            return highlighter;
        }

        private void EnsureParser()
        {
            if (this.htmlParser == null)
            {
                this.htmlParser = this.GetParser();
            }
        }

        /// <summary>
        /// Scans the text and splits code snippets from actual text.
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <returns>A collection of tokens of text and/or code snippets.</returns>
        protected virtual TokenCollection Tokenize(string text)
        {
            TokenCollection result = new TokenCollection();

            int lastStartIndex = 0;
            string language, code;
            MatchCollection matches = Regex.Matches(text, "[[]code=(?<language>([^]]*))[]](?<code>(.*?))[[]/code[]]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            for (int i = 0; i < matches.Count; i++)
            {
                // Add literal.
                if (matches[i].Index > lastStartIndex)
                    result.Add(new Token(text.Substring(lastStartIndex, matches[i].Index - lastStartIndex), TokenType.Literal));
                lastStartIndex = matches[i].Index + matches[i].Length;

                // Add code snippet.
                language = text.Substring(matches[i].Groups["language"].Index, matches[i].Groups["language"].Length).Replace("\"", "").Replace("'", "");
                code = text.Substring(matches[i].Groups["code"].Index, matches[i].Groups["code"].Length).Trim();
                result.Add(new Token(code,
                    language,
                    TokenType.Code));
            }

            if (lastStartIndex < text.Length)
                result.Add(new Token(text.Substring(lastStartIndex, text.Length - lastStartIndex), TokenType.Literal));

            return result;
        }

        /// <summary>
        /// Renders the body of the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.RenderBody();
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (!this.rendered)
                this.RenderBody();

            base.RenderContents(writer);
        }

        /// <summary>
        /// Renders the text.
        /// </summary>
        protected virtual void RenderBody()
        {
            string text = this.Text;
            if (text.Length > 0)
            {
                this.rendered = true;
                if (this.mode == HighlightMode.Literal)
                {
                    this.RenderText(text);
                }
                else if (this.codeLayoutTemplate != null)
                {
                    if (this.mode == HighlightMode.Source)
                    {
                        this.RenderCode(text, this.Language);
                    }
                    else
                    {
                        TokenCollection tokens = this.Tokenize(text);
                        string value;
                        for (int i = 0; i < tokens.Count; i++)
                        {
                            value = this.GetTokenValue(tokens[i]);
                            if (tokens[i].Type == TokenType.Literal)
                                this.RenderText(value);
                            else
                                this.RenderCode(value, tokens[i].Language);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Renders literal text.
        /// </summary>
        /// <param name="text"></param>
        protected virtual void RenderText(string text)
        {
            this.Controls.Add(new LiteralControl(text));
        }

		/// <summary>
		/// Adds the controls which form the code snippet.
		/// </summary>
		/// <param name="source">The source code.</param>
		/// <param name="language">The language in which the source code is written.</param>
		protected virtual void RenderCode(string source, string language)
		{
			HighlighterBase highlighter = this.GetHighlighter(language);
			string parsedText = highlighter.Parse(source);
			string[] lines = parsedText.Replace("\r", String.Empty).Split('\n');

			// Find the placeholders for the line numbers and source lines.
			Control templatedOutLineControl = new SourceHeaderItem(highlighter.FullName, lines.Length);
            this.Controls.Add(templatedOutLineControl);
			this.codeLayoutTemplate.InstantiateIn(templatedOutLineControl);
				
			PlaceHolder lineNumberPlaceHolder = this.FindLineNumberPlaceHolder(templatedOutLineControl);
			PlaceHolder sourceLinePlaceHolder = this.FindSourceLinePlaceHolder(templatedOutLineControl);

			if (lineNumberPlaceHolder != null || sourceLinePlaceHolder != null)
			{
				Control templatedLineNumberControl, templatedSourceLineControl, separatorControl;
				for (int i = 0; i < lines.Length; i++)
				{
                    int lineNumber = (i + 1);
                    string line = lines[i];

                    if (lineNumberPlaceHolder != null && this.lineNumberTemplate != null)
                    {
                        // Parse line number.
                        templatedLineNumberControl = new SourceItem(lineNumber, lines.Length, line);

                        this.lineNumberTemplate.InstantiateIn(templatedLineNumberControl);
                        lineNumberPlaceHolder.Controls.Add(templatedLineNumberControl);

                        if (lineNumber < lines.Length && this.separatorTemplate != null)
                        {
                            separatorControl = new SeparatorItem();
                            this.separatorTemplate.InstantiateIn(separatorControl);
                            lineNumberPlaceHolder.Controls.Add(separatorControl);
                        }
                    }

                    if (sourceLinePlaceHolder != null && this.sourceLineTemplate != null)
                    {
                        // Parse source line.
                        templatedSourceLineControl = new SourceItem(lineNumber, lines.Length, line);

                        this.sourceLineTemplate.InstantiateIn(templatedSourceLineControl);
                        sourceLinePlaceHolder.Controls.Add(templatedSourceLineControl);

                        if (lineNumber < lines.Length && this.separatorTemplate != null)
                        {
                            separatorControl = new SeparatorItem();
                            this.separatorTemplate.InstantiateIn(separatorControl);
                            sourceLinePlaceHolder.Controls.Add(separatorControl);
                        }
                    }
				}
			}

			templatedOutLineControl.DataBind();
		}

		/// <summary>
		/// Finds the line number placeholder.
		/// </summary>
		/// <param name="templatedOutLineControl">The control which should contain the placeholder.</param>
		/// <returns>Either the first line number placeholder or null in case no line number placeholder could be found.</returns>
		private PlaceHolder FindLineNumberPlaceHolder(Control templatedOutLineControl)
		{
			for (int i = 0; i < templatedOutLineControl.Controls.Count; i++)
			{
				if (templatedOutLineControl.Controls[i] is LineNumberPlaceHolder)
					return templatedOutLineControl.Controls[i] as PlaceHolder;
				else
				{
					PlaceHolder result = this.FindLineNumberPlaceHolder(templatedOutLineControl.Controls[i]);
					if (result != null)
						return result;
				}
			}

			return null;
		}

		/// <summary>
		/// Finds the source line placeholder.
		/// </summary>
		/// <param name="templatedOutLineControl">The control which should contain the placeholder.</param>
		/// <returns>Either the first source line placeholder or null in case no source line placeholder could be found.</returns>
		private PlaceHolder FindSourceLinePlaceHolder(Control templatedOutLineControl)
		{
			for (int i = 0; i < templatedOutLineControl.Controls.Count; i++)
			{
				if (templatedOutLineControl.Controls[i] is SourceLinePlaceHolder)
					return templatedOutLineControl.Controls[i] as PlaceHolder;
				else
				{
					PlaceHolder result = this.FindSourceLinePlaceHolder(templatedOutLineControl.Controls[i]);
					if (result != null)
						return result;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the value of the token.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		protected virtual string GetTokenValue(Token token)
		{
			ParseEventHandler handler = this.Events[EventParseToken] as ParseEventHandler;
			if (handler != null)
			{
				return handler(this, new ParseEventArgs(token));
			}
			return token.Value;
		}
	}
}