using System;
using System.Drawing;
using System.Xml;

namespace Wilco.SyntaxHighlighting
{
	/// <summary>
	/// Represents an ASPX scanner.
	/// </summary>
	public class ASPXScanner : ScannerBase
	{
		private ASPXNode aspxNode;
		private XmlScanner xmlScanner;
		private string language;
		private IParser parser;

		/// <summary>
		/// Gets or sets the script node.
		/// </summary>
		public ASPXNode ASPXNode
		{
			get
			{
				return this.aspxNode;
			}
			set
			{
				if (value != this.aspxNode)
				{
					this.aspxNode = value;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.ASPXScanner"/> class.
		/// </summary>
		public ASPXScanner() : this(null, null)
		{
			//
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.ASPXScanner"/> class.
		/// </summary>
		/// <param name="tokenizer">The <see cref="Wilco.SyntaxHighlighting.TokenizerBase"/> which is used to tokenize the source code.</param>
		/// <param name="scannerResult">The <see cref="Wilco.SyntaxHighlighting.OccurrenceCollection"/> which will contain the scanner result.</param>
		public ASPXScanner(TokenizerBase tokenizer, OccurrenceCollection scannerResult) : this(tokenizer, scannerResult, new ASPXNode())
		{
			//
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.ASPXScanner"/> class.
		/// </summary>
		/// <param name="tokenizer">The <see cref="Wilco.SyntaxHighlighting.TokenizerBase"/> which is used to tokenize the source code.</param>
		/// <param name="scannerResult">The <see cref="Wilco.SyntaxHighlighting.OccurrenceCollection"/> which will contain the scanner result.</param>
		/// <param name="aspxNode">A <see cref="Wilco.SyntaxHighlighting.ASPXNode"/> object.</param>
		public ASPXScanner(TokenizerBase tokenizer, OccurrenceCollection scannerResult, ASPXNode aspxNode) : base(tokenizer, scannerResult)
		{
			this.aspxNode = aspxNode;
			this.aspxNode.BackColor = System.Drawing.Color.Yellow;
			this.xmlScanner = new XmlScanner(this.Tokenizer, this.ScannerResult);
			this.xmlScanner.Match += new MatchEventHandler(this.xmlScanner_Match);
			this.parser = new HtmlParser();
			this.SetID("ASPXScanner");
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.IScanner"/> implementation class.
		/// </summary>
		/// <param name="tokenizer">The <see cref="Wilco.SyntaxHighlighting.TokenizerBase"/> which is used to tokenize the source code.</param>
		/// <param name="scannerResult">The <see cref="Wilco.SyntaxHighlighting.OccurrenceCollection"/> which will contain the scanner result.</param>
		/// <returns>A new instance of a <see cref="Wilco.SyntaxHighlighting.IScanner"/> implementation class.</returns>
		public override IScanner Create(TokenizerBase tokenizer, OccurrenceCollection scannerResult)
		{
			return new ASPXScanner(tokenizer, scannerResult);
		}

		/// <summary>
		/// Loads the state of the scanner.
		/// </summary>
		/// <param name="state">An <see cref="System.Object"/> that contains the state of the scanner.</param>
		public override void LoadState(object state)
		{
			XmlElement element = (XmlElement)state;
			
			// Load settings.
			FontConverter converter = new FontConverter();
			this.aspxNode.BackColor = ColorTranslator.FromHtml(element.SelectSingleNode("settings/setting[@name='BackColor']").InnerText);
			this.aspxNode.ForeColor = ColorTranslator.FromHtml(element.SelectSingleNode("settings/setting[@name='ForeColor']").InnerText);
			this.aspxNode.Font = (Font)converter.ConvertFromString(element.SelectSingleNode("settings/setting[@name='Font']").InnerText);
			this.aspxNode.NavigateUrl = element.SelectSingleNode("settings/setting[@name='NavigateUrl']").InnerText;
		}

		/// <summary>
		/// Saves the current state of the scanner.
		/// </summary>
		/// <param name="container">The container which will contain the state.</param>
		/// <returns>An <see cref="System.Object"/> that contains the state of the scanner.</returns>
		public override object SaveState(object container)
		{
			XmlDocument document = (XmlDocument)container;
			XmlElement element = (XmlElement)base.SaveState(container);
			
			// Save settings.
			XmlElement settingRootElement = document.CreateElement("settings");
			element.AppendChild(settingRootElement);

			FontConverter converter = new FontConverter();
			settingRootElement.AppendChild(this.CreateSetting(document, "BackColor", ColorTranslator.ToHtml(this.aspxNode.BackColor)));
			settingRootElement.AppendChild(this.CreateSetting(document, "ForeColor", ColorTranslator.ToHtml(this.aspxNode.ForeColor)));
			settingRootElement.AppendChild(this.CreateSetting(document, "ForeColor", converter.ConvertToString(this.aspxNode.Font)));
			settingRootElement.AppendChild(this.CreateSetting(document, "NavigateUrl", this.aspxNode.NavigateUrl));

			return element;
		}

		/// <summary>
		/// Creates a steting.
		/// </summary>
		/// <param name="document">The document which will contain the setting.</param>
		/// <param name="name">The name of the setting.</param>
		/// <param name="value">The value of the setting.</param>
		/// <returns>The <see cref="System.Xml.XmlElement"/> which represents the setting.</returns>
		private XmlElement CreateSetting(XmlDocument document, string name, string value)
		{
			XmlElement element = document.CreateElement("setting");
			element.SetAttribute("name", name);
			element.InnerText = value;
			return element;
		}

		/// <summary>
		/// Scans a token.
		/// </summary>
		/// <remarks>
		/// An <see cref="Wilco.SyntaxHighlighting.IScanner"/> implementation will generally have a reference to a 
		/// <see cref="Wilco.SyntaxHighlighting.NodeCollection"/> which will be used to store results of a scan.
		/// </remarks>
		/// <param name="token">A token from the source code.</param>
		public override void Scan(string token)
		{
			if (!this.Enabled)
			{
				if (this.Child != null)
				{
					this.Child.Scan(token);
				}
			}
			else
			{
				// Highlight as XML until a script block was found.
				if (this.language == null)
				{
					bool handled = false;
					if ((this.Tokenizer.Position + 3 <= this.Tokenizer.Source.Length) && this.Tokenizer.GetNextTokens(3) == "<%@")
					{
						int charIndex = this.Tokenizer.Source.IndexOf("%>", this.Tokenizer.Position + 3);
						if (charIndex == -1)
							charIndex = this.Tokenizer.Source.Length;
						else
							charIndex += 2;

						this.ScannerResult.Add(new Occurrence(this.Tokenizer.Position, charIndex - this.Tokenizer.Position, this.aspxNode));

                        this.Tokenizer.MoveTo(charIndex);
						handled = true;
					}
					else if (this.Tokenizer.Position + 2 <= this.Tokenizer.Source.Length)
					{
                        string right = this.Tokenizer.GetNextTokens(2);
                        if (right == "<%" || right == "%>")
                        {
                            this.ScannerResult.Add(new Occurrence(this.Tokenizer.Position, 2, this.aspxNode));
                            handled = true;
                        }
					}

					if (!handled)
					{
						// Find script block.
						this.xmlScanner.Scan(token);
					}
				}
				else
				{
					if ((this.Tokenizer.Position + 9) <= this.Tokenizer.Source.Length && this.Tokenizer.GetNextTokens(9) == "</script>")
					{
						this.language = null;
						this.xmlScanner.Scan(token);
					}
					else if (this.Child != null)
					{
						this.Child.Scan(token);
					}
				}
			}
		}

		/// <summary>
		/// Handles the match event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void xmlScanner_Match(object sender, MatchEventArgs e)
		{
			if (e.Tag != null && e.Tag.ToLower() == "script")
			{
				if (e.Type == MatchType.StartTag)
				{
					this.language = e.Attributes["lang"];
					if (this.language == null)
						this.language = e.Attributes["language"];

					if (this.language != null)
					{
						this.language = this.language.Replace("\"", "").Replace("'", "");
						if (this.language.Length == 0)
						{
							this.language = null;
						}
					}

					if (this.language != null)
					{
						HighlighterBase highlighter = Register.Instance.Highlighters[this.language];
						if (highlighter != null)
						{
                            highlighter = highlighter.Create();
							highlighter.Parser = this.parser;
							this.Child = highlighter.BuildEntryPointScanner(this.Tokenizer, this.ScannerResult);
						}
						else
							this.language = null;
					}
				}
				else if (e.Type == MatchType.EndTag)
					this.language = null;
			}
		}

		/// <summary>
		/// Resets the scanner.
		/// </summary>
		public override void Reset()
		{
			this.language = null;
		}
	}
}