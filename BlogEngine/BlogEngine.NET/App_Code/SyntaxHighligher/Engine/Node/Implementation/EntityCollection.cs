using System;
using System.Collections;

namespace Wilco.SyntaxHighlighting
{
	/// <summary>
	/// Represents a collection of entities.
	/// </summary>
	public class EntityCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.EntityCollection"/> class.
		/// </summary>
		public EntityCollection()
		{
			//
		}

		/// <summary>
		/// Gets an <see cref="Wilco.SyntaxHighlighting.Entity"/> object at the specified index.
		/// </summary>
		public Entity this[int index]
		{
			get
			{
				return (Entity)this.List[index];
			}
		}

		/// <summary>
		/// Adds an entity.
		/// </summary>
		/// <param name="entity">An <see cref="Wilco.SyntaxHighlighting.Entity"/> object.</param>
		/// <returns>Index of the added object.</returns>
		public int Add(Entity entity)
		{
			return this.List.Add(entity);
		}

		/// <summary>
		/// Removes a entity.
		/// </summary>
		/// <param name="entity">An <see cref="Wilco.SyntaxHighlighting.Entity"/> object.</param>
		public void Remove(Entity entity)
		{
			this.List.Remove(entity);
		}
	}
}