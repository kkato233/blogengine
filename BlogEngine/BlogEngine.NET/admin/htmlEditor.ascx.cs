﻿using System.Web.UI;

namespace Admin
{
    /// <summary>
    /// The admin_html editor.
    /// </summary>
    public partial class admin_htmlEditor : UserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets TabIndex.
        /// </summary>
        public short TabIndex
        {
            get
            {
                return this.TinyMCE1.TabIndex;
            }

            set
            {
                this.TinyMCE1.TabIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets Text.
        /// </summary>
        public string Text
        {
            get
            {
                return this.TinyMCE1.Text;
            }

            set
            {
                this.TinyMCE1.Text = value;
            }
        }

        #endregion
    }
}