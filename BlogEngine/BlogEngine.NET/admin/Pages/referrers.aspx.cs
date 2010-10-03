namespace Admin.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.WebControls;

    using BlogEngine.Core;

    using Resources;

    /// <summary>
    /// The admin_ pages_referrers.
    /// </summary>
    public partial class admin_Pages_referrers : System.Web.UI.Page
    {
        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (BlogSettings.Instance.EnableReferrerTracking)
                {
                    this.BindDays();
                    this.BindReferrers();
                }
                else
                {
                    this.ddlDays.Enabled = false;
                }

                this.txtNumberOfDays.Text = BlogSettings.Instance.NumberOfReferrerDays.ToString();
                this.cbEnableReferrers.Checked = BlogSettings.Instance.EnableReferrerTracking;
            }

            this.btnSave.Click += this.btnSave_Click;
            this.btnSaveTop.Click += this.btnSave_Click;

            this.ddlDays.SelectedIndexChanged += this.ddlDays_SelectedIndexChanged;
            this.cbEnableReferrers.CheckedChanged += this.cbEnableReferrers_CheckedChanged;
            this.Page.Title = labels.referrers;
        }

        /// <summary>
        /// The bind days.
        /// </summary>
        private void BindDays()
        {
            var sortedDates = new List<DateTime>(Referrer.ReferrersByDay.Keys);

            sortedDates.Sort(delegate(DateTime firstPair, DateTime nextPair) { return firstPair.CompareTo(nextPair) * -1; });

            this.ddlDays.DataSource = sortedDates;
            this.ddlDays.DataBind();
            foreach (ListItem item in this.ddlDays.Items)
            {
                if (item.Text == DateTime.Today.ToShortDateString())
                {
                    item.Selected = true;
                }
            }
        }

        /// <summary>
        /// The bind referrers.
        /// </summary>
        private void BindReferrers()
        {
            if (!(this.ddlDays.SelectedIndex >= 0 & Referrer.Referrers.Count > 0))
            {
                return;
            }

            var referrers = Referrer.ReferrersByDay[DateTime.Parse(this.ddlDays.SelectedItem.Text)];
            if (referrers == null)
            {
                return;
            }
            
            var table = new DataTable();
            table.Columns.Add("url", typeof(string));
            table.Columns.Add("shortUrl", typeof(string));
            table.Columns.Add("target", typeof(string));
            table.Columns.Add("shortTarget", typeof(string));
            table.Columns.Add("hits", typeof(int));

            var spamTable = table.Clone();

            foreach (var refer in referrers)
            {
                DataRow tableRow;
                if (refer.PossibleSpam)
                {
                    tableRow = spamTable.NewRow();
                    this.PopulateRow(tableRow, refer);
                    spamTable.Rows.Add(tableRow);
                }
                else
                {
                    tableRow = table.NewRow();
                    this.PopulateRow(tableRow, refer);
                    table.Rows.Add(tableRow);
                }
            }

            this.BindTable(table, this.grid);
            this.BindTable(spamTable, this.spamGrid);
        }

        /// <summary>
        /// Binds the table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="grid">The grid.</param>
        private void BindTable(DataTable table, GridView grid)
        {
            var total = table.Compute("sum(hits)", null).ToString();

            var view = new DataView(table) { Sort = "hits desc" };

            grid.DataSource = view;
            grid.DataBind();

            if (grid.Rows.Count > 0)
            {
                grid.FooterRow.Cells[0].Text = "Total";
                grid.FooterRow.Cells[grid.FooterRow.Cells.Count - 1].Text = total;
            }

            this.PaintRows(grid, 3);
        }

        /// <summary>
        /// Makes the short URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private string MakeShortUrl(string url)
        {
            if (url.Length > 150)
            {
                return string.Format("{0}...", url.Substring(0, 150));
            }

            return this.Server.HtmlEncode(url.Replace("http://", string.Empty).Replace("www.", string.Empty));
        }

        /// <summary>
        /// Paints the background color of the alternate rows
        ///     in the gridview.
        /// </summary>
        /// <param name="grid">
        /// The grid.
        /// </param>
        /// <param name="alternateRows">
        /// The alternate Rows.
        /// </param>
        private void PaintRows(GridView grid, int alternateRows)
        {
            if (grid.Rows.Count == 0)
            {
                return;
            }

            var count = 0;
            for (var i = 0; i < grid.Controls[0].Controls.Count - 1; i++)
            {
                if (count > alternateRows)
                {
                    ((WebControl)grid.Controls[0].Controls[i]).CssClass = "alt";
                }

                count++;

                if (count == alternateRows + alternateRows + 1)
                {
                    count = 1;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int days;
            if (int.TryParse(this.txtNumberOfDays.Text, out days))
            {
                BlogSettings.Instance.NumberOfReferrerDays = days;

                BlogSettings.Instance.Save();
            }

            this.Response.Redirect(this.Request.RawUrl, true);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbEnableReferrers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cbEnableReferrers_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbEnableReferrers.Checked)
            {
                this.BindDays();
                this.BindReferrers();
            }
            else
            {
                this.ddlDays.Enabled = false;
            }

            BlogSettings.Instance.EnableReferrerTracking = this.cbEnableReferrers.Checked;
            BlogSettings.Instance.Save();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlDays control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ddlDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindReferrers();
        }

        /// <summary>
        /// Populates the row.
        /// </summary>
        /// <param name="tableRow">The table row.</param>
        /// <param name="refer">The refer.</param>
        private void PopulateRow(DataRow tableRow, Referrer refer)
        {
            tableRow["url"] = this.Server.HtmlEncode(refer.ReferrerUrl.ToString());
            tableRow["shortUrl"] = this.MakeShortUrl(refer.ReferrerUrl.ToString());
            tableRow["target"] = this.Server.HtmlEncode(refer.Url.ToString());
            tableRow["shortTarget"] = this.MakeShortUrl(refer.Url.ToString());
            tableRow["hits"] = refer.Count;
        }

        #endregion
    }
}