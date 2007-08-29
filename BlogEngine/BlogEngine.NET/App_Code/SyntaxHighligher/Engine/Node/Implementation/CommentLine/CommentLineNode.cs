using System;
using System.Collections.Specialized;

namespace Wilco.SyntaxHighlighting
{
	/// <summary>
	/// Represents a comment line node.
	/// </summary>
	public class CommentLineNode : NodeBase
	{
		private StringCollection entities;

		/// <summary>
		/// Gets the collection of entities which indicate the start of a comment line.
		/// </summary>
		public StringCollection Entities
		{
			get
			{
				return this.entities;
			}
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.CommentLineNode"/> class.
		/// </summary>
		public CommentLineNode()
		{
			this.entities = new StringCollection();
		}
	}
}