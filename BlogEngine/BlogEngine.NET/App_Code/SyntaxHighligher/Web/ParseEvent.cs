using System;

namespace Wilco.Web.SyntaxHighlighting
{
	/// <summary>
	/// Represents the parse method.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	/// <returns></returns>
	public delegate string ParseEventHandler(object sender, ParseEventArgs e);

	/// <summary>
	/// Represents a parse eventargs object.
	/// </summary>
	[Serializable]
	public sealed class ParseEventArgs : EventArgs
	{
		private Token token;

		/// <summary>
		/// Gets the token.
		/// </summary>
		public Token Token
		{
			get
			{
				return this.token;
			}
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.Web.SyntaxHighlighting.Token"/> class.
		/// </summary>
		/// <param name="token">The actual token.</param>
		public ParseEventArgs(Token token)
		{
			this.token = token;
		}
	}
}