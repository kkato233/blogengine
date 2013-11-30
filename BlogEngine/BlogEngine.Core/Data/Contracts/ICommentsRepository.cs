using System;
using System.Collections.Generic;
using BlogEngine.Core.Data.Models;

namespace BlogEngine.Core.Data.Contracts
{
    /// <summary>
    /// Comments repository
    /// </summary>
    public interface ICommentsRepository
    {
        /// <summary>
        /// Comments list
        /// </summary>
        /// <param name="commentType">Comment type</param>
        /// <param name="take">Items to take</param>
        /// <param name="skip">Items to skip</param>
        /// <param name="filter">Filter expression</param>
        /// <param name="order">Sort order</param>
        /// <returns>List of comments</returns>
        IEnumerable<BlogEngine.Core.Data.Models.Comment> GetComments(CommentType commentType = CommentType.All, int take = 10, int skip = 0, string filter = "", string order = "");
        /// <summary>
        /// Single commnet by ID
        /// </summary>
        /// <param name="id">
        /// Comment id
        /// </param>
        /// <returns>
        /// A JSON Comment
        /// </returns>
        BlogEngine.Core.Data.Models.Comment FindById(Guid id);
        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="item">Comment</param>
        /// <returns>Comment object</returns>
        Data.Models.Comment Add(Data.Models.Comment item);
        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">Item to update</param>
        /// <returns>True on success</returns>
        bool Update(Data.Models.Comment item);
        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>True on success</returns>
        bool Remove(Guid id);
    }
}
