using System;
using System.Collections.Specialized;

namespace Wilco.SyntaxHighlighting
{
	/// <summary>
	/// Represents a word node.
	/// </summary>
	public class WordNode : NodeBase
	{
		private StringCollection entities;
		private bool ignoreCase;

		/// <summary>
		/// Gets the collection of entities which represent words.
		/// </summary>
		public StringCollection Entities
		{
			get
			{
				return this.entities;
			}
		}

		/// <summary>
		/// Gets or sets whether case should be ignored.
		/// </summary>
		public bool IgnoreCase
		{
			get
			{
				return this.ignoreCase;
			}
			set
			{
				if (value != this.ignoreCase)
				{
					this.ignoreCase = value;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.WordNode"/> class.
		/// </summary>
		public WordNode()
		{
			this.entities = new StringCollection();
		}
	}
}