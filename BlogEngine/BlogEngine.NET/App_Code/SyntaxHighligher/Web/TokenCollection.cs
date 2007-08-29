using System;
using System.Collections;

namespace Wilco.Web.SyntaxHighlighting
{
	/// <summary>
	/// Represents a collection of tokens.
	/// </summary>
	public class TokenCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.Web.SyntaxHighlighting.TokenCollection"/> class.
		/// </summary>
		public TokenCollection()
		{
			//
		}

		/// <summary>
		/// Gets the token at the specified index.
		/// </summary>
		public Token this[int index]
		{
			get
			{
				return (Token)this.List[index];
			}
		}

		/// <summary>
		/// Adds an item.
		/// </summary>
		/// <param name="t">The item to add.</param>
		/// <returns>Index of the added item.</returns>
		public int Add(Token t)
		{
			return this.List.Add(t);
		}
	}
}