using System;
using System.Drawing;

namespace Wilco.SyntaxHighlighting
{
	/// <summary>
	/// Represents a PHP syntax highlighter.
	/// </summary>
	public class ASPXHighlighter : HighlighterBase
	{
		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.ASPXHighlighter"/> class.
		/// </summary>
		public ASPXHighlighter() : this(null)
		{
			//
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.ASPXHighlighter"/> class.
		/// </summary>
		/// <param name="parser">The parser which will be used to parse the source code.</param>
		public ASPXHighlighter(IParser parser) : base(parser)
		{
			this.Name = "ASPX";
			this.FullName = "ASP.NET";
			this.TagValues.AddRange(new String[] { "aspx", "aspnet" });
            this.FileExtensions.AddRange(new String[] { "aspx", "asmx", "ascx", "asax", "ashx" });
		}

        /// <summary>
        /// Creates a new instance (clone) of this highlighter.
        /// </summary>
        /// <returns></returns>
        public override HighlighterBase Create()
        {
            return new ASPXHighlighter(this.Parser);
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

			ASPXScanner aspxScanner = new ASPXScanner(this.Tokenizer, this.ScannerResult);
			return aspxScanner;
		}
	}
}