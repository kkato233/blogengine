using System;

namespace BlogEngine.Core.Web.Navigation
{
    /// <summary>
    /// Generic pager contains all numeric values
    /// required to render pager control
    /// </summary>
    public class Pager : IPager
    {
        int first = 0, prev = 0, from = 0, to = 0, next = 0, last = 0;

        /// <summary>
        /// Pager constructor
        /// </summary>
        /// <param name="page">Page #</param>
        /// <param name="pageSize">Page size (number of items per page)</param>
        /// <param name="listCount">Number of items in the list</param>
        public Pager(int page, int pageSize, int listCount)
        {
            if (page < 1) page = 1;

            var pgs = Convert.ToDecimal(listCount) / Convert.ToDecimal(pageSize);
            var p = pgs - (int)pgs;
            last = p > 0 ? (int)pgs + 1 : (int)pgs;

            if (page > last) page = 1;

            from = ((page * pageSize) - (pageSize - 1));
            to = (page * pageSize);

            // adjust for the last (or single) page
            if (listCount < to) to = listCount;

            // when last item on the last page deleted
            // this will reset "from" counter
            if (from > to) from = from - pageSize;

            if (page > 1)
            {
                prev = page - 1;
                first = 1;
            }

            if (page < last) next = page + 1;
            if (page == last) last = 0;
        }

        #region IPager

        /// <summary>
        /// First page
        /// </summary>
        int IPager.First
        {
            get { return first; }
        }
        /// <summary>
        /// Previous page
        /// </summary>
        int IPager.Previous
        {
            get { return prev; }
        }
        /// <summary>
        /// Show items on the page from
        /// </summary>
        int IPager.From
        {
            get { return from; }
        }
        /// <summary>
        /// Show items from the page up to
        /// </summary>
        int IPager.To
        {
            get { return to; }
        }
        /// <summary>
        /// Next page
        /// </summary>
        int IPager.Next
        {
            get { return next; }
        }
        /// <summary>
        /// Last page
        /// </summary>
        int IPager.Last
        {
            get { return last; }
        }

        #endregion
    }
}