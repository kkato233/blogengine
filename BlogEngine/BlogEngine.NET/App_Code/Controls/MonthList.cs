namespace Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    using BlogEngine.Core;

    using Resources;

    /// <summary>
    /// Builds a category list.
    /// </summary>
    public class MonthList : Control
    {
        #region Constants and Fields

        /// <summary>
        /// The cache key.
        /// </summary>
        private const string CacheKey = "BE_MonthListCacheKey";

        /// <summary>
        /// The cache timeout in hours.
        /// </summary>
        private const double CacheTimeoutInHours = 1;

        /// <summary>
        /// The _ sync root.
        /// </summary>
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="MonthList"/> class.
        /// </summary>
        static MonthList()
        {
            Post.Saved += PostSaved;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (Post.Posts.Count > 0)
            {
                var html = RenderMonths();
                writer.Write(html);
                writer.Write(Environment.NewLine);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the posts per month.
        /// </summary>
        /// <returns>The posts per month.</returns>
        private static SortedDictionary<DateTime, int> GetPostsPerMonth()
        {
            lock (SyncRoot)
            {
                var months = HttpRuntime.Cache[CacheKey] as SortedDictionary<DateTime, int>;
                if (months == null)
                {
                    months = new SortedDictionary<DateTime, int>();

                    // let dictionary expire after 1 hour
                    HttpRuntime.Cache.Insert(
                        CacheKey, months, null, DateTime.Now.AddHours(CacheTimeoutInHours), Cache.NoSlidingExpiration);

                    foreach (var month in
                        Post.Posts.Where(post => post.VisibleToPublic).Select(
                            post => new DateTime(post.DateCreated.Year, post.DateCreated.Month, 1)))
                    {
                        int count;

                        // if the date is not in the dictionary, count will be set to 0
                        months.TryGetValue(month, out count);
                        ++count;
                        months[month] = count;
                    }
                }

                return months;
            }
        }

        /// <summary>
        /// Handles the Saved event of the Post control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BlogEngine.Core.SavedEventArgs"/> instance containing the event data.</param>
        private static void PostSaved(object sender, SavedEventArgs e)
        {
            // invalidate cache whenever a post is modified
            lock (SyncRoot)
            {
                HttpRuntime.Cache.Remove(CacheKey);
            }
        }

        /// <summary>
        /// Renders the months.
        /// </summary>
        /// <returns>The months.</returns>
        private static string RenderMonths()
        {
            var months = GetPostsPerMonth();
            if (months.Keys.Count == 0)
            {
                return string.Format("<p>{0}</p>", labels.none);
            }

            using (var ul = new HtmlGenericControl("ul"))
            {
                ul.Attributes.Add("id", "monthList");
                HtmlGenericControl year;
                HtmlGenericControl list = null;
                var current = 0;

                foreach (var date in months.Keys)
                {
                    if (current == 0)
                    {
                        current = date.Year;
                    }

                    if (date.Year > current || ul.Controls.Count == 0)
                    {
                        list = new HtmlGenericControl("ul") { ID = "year" + date.Year };

                        year = new HtmlGenericControl("li") { InnerHtml = date.Year.ToString() };
                        year.Attributes.Add("class", "year");
                        year.Attributes.Add("onclick", string.Format("BlogEngine.toggleMonth('year{0}')", date.Year));
                        year.Controls.Add(list);

                        if (date.Year == DateTime.Now.Year)
                        {
                            list.Attributes.Add("class", "open");
                        }

                        ul.Controls.AddAt(0, year);
                    }

                    using (var li = new HtmlGenericControl("li"))
                    {
                        var anc = new HtmlAnchor
                            {
                                HRef =
                                    string.Format(
                                        "{0}{1}/{2}/default{3}",
                                        Utils.RelativeWebRoot,
                                        date.Year,
                                        date.ToString("MM"),
                                        BlogSettings.Instance.FileExtension),
                                InnerHtml =
                                    string.Format(
                                        "{0} ({1})",
                                        DateTime.Parse(string.Format("{0}-{1}-01", date.Year, date.Month)).ToString("MMMM"),
                                        months[date])
                            };

                        li.Controls.Add(anc);
                        if (list != null)
                        {
                            list.Controls.AddAt(0, li);
                        }
                    }

                    current = date.Year;
                }

                using (var sw = new StringWriter())
                {
                    ul.RenderControl(new HtmlTextWriter(sw));
                    return sw.ToString();
                }
            }
        }

        /*
        /// <summary>
        /// Sorts the categories.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <returns>A sorted dictionary of string keys and Guids.</returns>
        private SortedDictionary<string, Guid> SortCategories(Dictionary<Guid, string> categories)
        {
            var dic = new SortedDictionary<string, Guid>();
            foreach (var key in categories.Keys)
            {
                dic.Add(categories[key], key);
            }

            return dic;
        }
*/

        #endregion
    }
}