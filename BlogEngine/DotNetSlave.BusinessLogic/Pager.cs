﻿using System.Collections.Generic;

namespace BlogEngine.Core
{
    public class Pager
    {
        private static int _currentPage;
        private static int _itemCount;
        private static string _pkgType;

        /// <summary>
        /// Pager
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="itemCount">Item count</param>
        /// <param name="pkgType">Package type</param>
        public Pager(int currentPage, int itemCount, string pkgType)
        {
            _currentPage = currentPage;
            _itemCount = itemCount;
            _pkgType = pkgType;
        }

        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
        }

        /// <summary>
        /// Pager items
        /// </summary>
        public List<PagerItem> PageItems
        {
            get
            {
                var items = new List<PagerItem>();

                for (int i = 1; i <= (int)(_itemCount / BlogSettings.Instance.PostsPerPage) + 1; i++)
                {
                    items.Add(i == _currentPage ? new PagerItem(i, true, _pkgType) : new PagerItem(i, false, _pkgType));
                }
                return items;
            }
        }
    }

    /// <summary>
    /// Page Item
    /// </summary>
    public class PagerItem
    {
        private readonly string _pkgType;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="current"></param>
        /// <param name="pkgType"></param>
        public PagerItem(int pageNumber, bool current, string pkgType = "")
        {
            PageNumber = pageNumber;
            Current = current;
            _pkgType = pkgType;
        }
        /// <summary>
        /// Page number
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Current selected page
        /// </summary>
        public bool Current { get; set; }
        /// <summary>
        /// Page link
        /// </summary>
        public string PageLink 
        {
            get { return string.Format("GalleryGetPackages({0},'{1}'); return false;", PageNumber, _pkgType); }
        }
    }
}
