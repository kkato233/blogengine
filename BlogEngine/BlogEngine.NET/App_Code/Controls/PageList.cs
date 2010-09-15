namespace Controls
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// Builds a page list.
    /// </summary>
    public class PageList : Control
    {
        #region Constants and Fields

        /// <summary>
        /// The sync root.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// The html string.
        /// </summary>
        private static string html;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="PageList"/> class. 
        /// </summary>
        static PageList()
        {
            BlogEngine.Core.Page.Saved += (sender, args) => html = null;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the rendered HTML in the private field and first
        ///     updates it when a post has been saved (new or updated).
        /// </summary>
        private static string Html
        {
            get
            {
                if (html == null)
                {
                    lock (SyncRoot)
                    {
                        if (html == null || BlogEngine.Core.Page.Pages == null)
                        {
                            var ul = BindPages();
                            using (var sw = new StringWriter())
                            {
                                ul.RenderControl(new HtmlTextWriter(sw));
                                html = sw.ToString();
                            }
                        }
                    }
                }

                return html;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.Write(Html);
            writer.Write(Environment.NewLine);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loops through all pages and builds the HTML
        /// presentation.
        /// </summary>
        /// <returns>A list item.</returns>
        private static HtmlGenericControl BindPages()
        {
            var ul = new HtmlGenericControl("ul") { ID = "pagelist" };
            ul.Attributes.Add("class", "pagelist");

            foreach (var page in BlogEngine.Core.Page.Pages.Where(page => page.ShowInList && page.VisibleToPublic))
            {
                var li = new HtmlGenericControl("li");
                var anc = new HtmlAnchor { HRef = page.RelativeLink, InnerHtml = page.Title, Title = page.Description };

                li.Controls.Add(anc);
                ul.Controls.Add(li);
            }

            return ul;
        }

        #endregion
    }
}