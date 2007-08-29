using System;
using System.Collections;

namespace Wilco.SyntaxHighlighting
{
	/// <summary>
	/// Represents a collection of nodes.
	/// </summary>
	public class NodeCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.NodeCollection"/> class.
		/// </summary>
		public NodeCollection()
		{
			//
		}

		/// <summary>
		/// Gets an <see cref="Wilco.SyntaxHighlighting.INode"/> object at the specified index.
		/// </summary>
		public INode this[int index]
		{
			get
			{
				return (INode)this.List[index];
			}
		}

		/// <summary>
		/// Adds a node.
		/// </summary>
		/// <param name="node">An <see cref="Wilco.SyntaxHighlighting.INode"/> object.</param>
		/// <returns>Index of the added object.</returns>
		public int Add(INode node)
		{
			return this.List.Add(node);
		}

		/// <summary>
		/// Removes a node.
		/// </summary>
		/// <param name="node">An <see cref="Wilco.SyntaxHighlighting.INode"/> object.</param>
		public void Remove(INode node)
		{
			this.List.Remove(node);
		}
	}
}