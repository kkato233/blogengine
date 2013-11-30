using System.Collections.Generic;
using BlogEngine.Core.Data.Models;
using System;

namespace BlogEngine.Core.Data.Contracts
{
    /// <summary>
    /// Trash repository
    /// </summary>
    public interface ITrashRepository
    {
        /// <summary>
        /// Get trash list
        /// </summary>
        /// <param name="trashType">Type (post, page, comment)</param>
        /// <param name="take">Take for a page</param>
        /// <param name="skip">Items to sckip</param>
        /// <param name="filter">Filter expression</param>
        /// <param name="order">Sort order</param>
        /// <returns></returns>
        List<TrashItem> GetTrash(TrashType trashType, int take = 10, int skip = 0, string filter = "", string order = "");

        /// <summary>
        /// Restore
        /// </summary>
        /// <param name="trashType">Trash type</param>
        /// <param name="id">Id</param>
        void Restore(string trashType, Guid id);

        /// <summary>
        /// Purge
        /// </summary>
        /// <param name="trashType">Trash type</param>
        /// <param name="id">Id</param>
        void Purge(string trashType, Guid id);

        /// <summary>
        /// Purge all
        /// </summary>
        void PurgeAll();

        /// <summary>
        /// Builds pager control for trash list page
        /// </summary>
        /// <param name="page">Current Page Number</param>
        /// <returns></returns>
        string GetPager(int page);

        /// <summary>
        /// If trash is empty.
        /// </summary>
        /// <returns>True if empty.</returns>
        bool IsTrashEmpty();

        /// <summary>
        /// Processes recycle bin actions
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="vals">Values</param>
        /// <returns>Response</returns>
        JsonResponse Process(string action, string[] vals);

        /// <summary>
        /// Purge log file
        /// </summary>
        /// <returns></returns>
        JsonResponse PurgeLogfile();
    }
}
