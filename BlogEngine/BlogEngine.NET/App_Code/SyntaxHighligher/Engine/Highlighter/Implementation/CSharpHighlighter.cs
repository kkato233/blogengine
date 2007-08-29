using System;
using System.Drawing;

namespace Wilco.SyntaxHighlighting
{
	/// <summary>
	/// Represents a C# syntax highlighter.
	/// </summary>
	public class CSharpHighlighter : HighlighterBase
	{
		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.CSharpHighlighter"/> class.
		/// </summary>
		public CSharpHighlighter() : this(null)
		{
			//
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.CSharpHighlighter"/> class.
		/// </summary>
		/// <param name="parser">The parser which will be used to parse the source code.</param>
		public CSharpHighlighter(IParser parser) : base(parser)
		{
			this.Name = "C#";
			this.FullName = "C#";
			this.TagValues.AddRange(new String[] { "csharp", "c#", "cs" });
			this.FileExtensions.Add("cs");
		}

        /// <summary>
        /// Creates a new instance (clone) of this highlighter.
        /// </summary>
        /// <returns></returns>
        public override HighlighterBase Create()
        {
            return new CSharpHighlighter(this.Parser);
        }

		/// <summary>
		/// Builds a new chain of scanners.
		/// </summary>
		/// <param name="tokenizer">The tokenizer used by the scanners.</param>
		/// <param name="scannerResult">The scanner result.</param>
		/// <returns>
		/// The scanner at the start of the chain.
		/// </returns>
		public override IScanner BuildEntryPointScanner(TokenizerBase tokenizer, OccurrenceCollection scannerResult)
		{
			this.Tokenizer = tokenizer;
			this.ScannerResult = scannerResult;

			IScanner wordScanner = this.BuildWordScanner();

			IScanner stringLineScanner = this.BuildStringScanner();
			stringLineScanner.Child = wordScanner;

			IScanner stringBlockScanner = this.BuildStringBlockScanner();
			stringBlockScanner.Child = stringLineScanner;

			IScanner commentLineScanner = this.BuildCommentLineScanner();
			commentLineScanner.Child = stringBlockScanner;

			CommentLineScanner xmlCommentLineScanner = new CommentLineScanner(this.Tokenizer, this.ScannerResult);
			xmlCommentLineScanner.CommentLineNode.Entities.Add("///");
			xmlCommentLineScanner.CommentLineNode.ForeColor = Color.Gray;
			xmlCommentLineScanner.Child = commentLineScanner;

			IScanner commentBlockScanner = this.BuildCommentBlockScanner();
			commentBlockScanner.Child = xmlCommentLineScanner;

			return commentBlockScanner;
		}

		/// <summary>
		/// Builds a word scanner.
		/// </summary>
		/// <returns>A <see cref="Wilco.SyntaxHighlighting.WordScanner"/> object.</returns>
		protected override IScanner BuildWordScanner()
		{
			WordScanner scanner = new WordScanner(this.Tokenizer, this.ScannerResult);
			scanner.WordNodes = new WordNode[2];
			scanner.WordNodes[0] = new WordNode();
			scanner.WordNodes[0].ForeColor = Color.Blue;
			scanner.WordNodes[0].Entities.AddRange(this.GetKeywords());
			scanner.WordNodes[1] = new WordNode();
			scanner.WordNodes[1].ForeColor = Color.Red;
			scanner.WordNodes[1].Entities.AddRange(new String[] { "#region", "#endregion" });
			return scanner;
		}

		/// <summary>
		/// Builds a string block scanner.
		/// </summary>
		/// <returns>A <see cref="Wilco.SyntaxHighlighting.StringBlockScanner"/> object.</returns>
		protected virtual IScanner BuildStringBlockScanner()
		{
			StringBlockScanner blockScanner = new StringBlockScanner(this.Tokenizer, this.ScannerResult);
			blockScanner.StringNode.Entities.Add(new StringEntity("\"", "\"", "\""));
			blockScanner.StringNode.ForeColor = Color.DarkRed;
			return blockScanner;
		}

		/// <summary>
		/// Gets an array of registered keywords.
		/// </summary>
		/// <returns>An array of keywords.</returns>
		private string[] GetKeywords()
		{
			string[] keywordList = new string[]
			{
				"abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
				"class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
				"enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
				"foreach", "get", "goto", "if", "implicit", "in", "int", "interface", "internal", "is",
				"lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
				"params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
				"set", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this",
				"throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
				"using", "value", "virtual", "void", "volatile", "while"
			};

			Array.Sort(keywordList);
			Array.Reverse(keywordList);

			return keywordList;
		}
	}
}