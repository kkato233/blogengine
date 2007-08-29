using System;

namespace Wilco.Web.SyntaxHighlighting
{
	/// <summary>
	/// Provides an enumeration of highlight modes.
	/// </summary>
	public enum HighlightMode
	{
		/// <summary>
		/// Highlight all text as source code.
		/// </summary>
		Source = 1,

		/// <summary>
		/// Highlight in-line source code only.
		/// </summary>
		Inline = 2,

		/// <summary>
		/// Display text literally.
		/// </summary>
		Literal = 4
	}
}