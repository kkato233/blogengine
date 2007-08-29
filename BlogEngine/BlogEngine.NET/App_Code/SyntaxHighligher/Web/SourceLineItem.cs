using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.SyntaxHighlighting
{
	/// <summary>
	/// Represents a source item.
	/// </summary>
	public class SourceItem : Control, INamingContainer
	{
		private int lineNumber;
		private int totalLines;
		private string sourceLine;

		/// <summary>
		/// Gets the line number.
		/// </summary>
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
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
		/// Gets the line of source code.
		/// </summary>
		public string SourceLine
		{
			get
			{
				return this.sourceLine;
			}
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.Web.SyntaxHighlighting.SourceItem"/> class.
		/// </summary>
		/// <param name="lineNumber">The line number.</param>
		/// <param name="totalLines">The total number of lines.</param>
		/// <param name="sourceLine">The line of source code.</param>
		public SourceItem(int lineNumber, int totalLines, string sourceLine)
		{
			this.lineNumber = lineNumber;
			this.totalLines = totalLines;
			this.sourceLine = sourceLine;
		}
	}
}