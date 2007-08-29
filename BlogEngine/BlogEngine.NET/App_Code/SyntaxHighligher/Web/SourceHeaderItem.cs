using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.SyntaxHighlighting
{
	/// <summary>
	/// Represents a source header item.
	/// </summary>
	public class SourceHeaderItem : Control, INamingContainer
	{
		private string language;
		private int totalLines;

		/// <summary>
		/// Gets the language in which the source code is written.
		/// </summary>
		public string Language
		{
			get
			{
				return this.language;
			}
		}

		/// <summary>
		/// Gets the total number of lines.
		/// </summary>
		public int TotalLines
		{
			get
			{
				return this.totalLines;
			}
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.Web.SyntaxHighlighting.SourceHeaderItem"/> class.
		/// </summary>
		/// <param name="language">The language in which the source code is written.</param>
		/// <param name="totalLines">The total number of lines.</param>
		public SourceHeaderItem(string language, int totalLines)
		{
			this.language = language;
			this.totalLines = totalLines;
		}
	}
}