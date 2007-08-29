using System;

namespace Wilco.Web.SyntaxHighlighting
{
	/// <summary>
	/// Enumeration of token types.
	/// </summary>
	public enum TokenType
	{
		/// <summary>
		/// Literal text.
		/// </summary>
		Literal,

		/// <summary>
		/// Source code.
		/// </summary>
		Code
	}

	/// <summary>
	/// Represents a token.
	/// </summary>
	public class Token
	{
		private string value;
		private string language;
		private TokenType type;

		/// <summary>
		/// Gets the actual token.
		/// </summary>
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		/// <summary>
		/// Gets the actual token.
		/// </summary>
		public string Language
		{
			get
			{
				return this.language;
			}
		}

		/// <summary>
		/// Gets the type of token.
		/// </summary>
		public TokenType Type
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.Web.SyntaxHighlighting.Token"/> class.
		/// </summary>
		/// <param name="value">The actual token.</param>
		/// <param name="type">Type of token.</param>
		public Token(string value, TokenType type) : this(value, null, type)
		{
			//
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.Web.SyntaxHighlighting.Token"/> class.
		/// </summary>
		/// <param name="value">The actual token.</param>
		/// <param name="language">The language in which the source code is written.</param>
		/// <param name="type">Type of token.</param>
		public Token(string value, string language, TokenType type)
		{
			this.value = value;
			this.language = language;
			this.type = type;
		}
	}
}