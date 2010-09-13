using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Core.API.BlogML
{
    public class BaseReader
    {
        #region Constructors
        public BaseReader()
        {
            _author = "";
            _removeDups = false;
            _approvedCommentsOnly = false;
            _message = "";
        }
        #endregion

        #region Locals & Properties

        protected bool _removeDups, _approvedCommentsOnly;
        protected string _author;
        protected string _message;

        public bool RemoveDuplicates
        {
            get { return _removeDups; }
            set { _removeDups = value; }
        }
        public bool ApprovedCommentsOnly
        {
            get { return _approvedCommentsOnly; }
            set { _approvedCommentsOnly = value; }
        }
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        #endregion

        #region Methods

        public virtual bool Import()
        {
            return false;
        }

        public virtual bool Validate()
        {
            return false;
        }

        #endregion
    }
}