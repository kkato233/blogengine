using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using BlogEngine.Core;

namespace Controls
{
    /// <summary>
    /// Summary description for PostPager
    /// </summary>
    public class PostPager : PlaceHolder
    {
        public PostPager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write(PagerTag());
        }

        #region Properties

        private List<IPublishable> _Posts;
        /// <summary>
        /// The list of posts to display.
        /// </summary>
        public List<IPublishable> Posts
        {
            get { return _Posts; }
            set { _Posts = value; }
        }

        #endregion

        int PageIndex()
        {
            int retValue = 1;
            string url = HttpContext.Current.Request.RawUrl;
            if (url.Contains("page="))
            {
                url = url.Replace(url.Substring(0, url.IndexOf("page=") + 5), "");
                try
                {
                    retValue = int.Parse(url);
                }
                catch (Exception) { }
            }
            return retValue;
        }

        string PageUrl()
        {
            string path = HttpContext.Current.Request.RawUrl.Replace("Default.aspx", string.Empty);
            if (path.Contains("?"))
            {
                if (path.Contains("page="))
                {
                    int index = path.IndexOf("page=");
                    path = path.Substring(0, index);
                }
                else
                {
                    path += "&";
                }
            }
            else
            {
                path += "?";
            }
            return path + "page={0}";
        }

        string PagerTag()
        {
            string retValue = "";
            string link = "<li class=\"PagerLink\"><a href=\"" + PageUrl() + "\">{1}</a></li>";
            string linkCurrent = "<li class=\"PagerLinkCurrent\">{0}</li>";
            string linkFirst = "<li class=\"PagerFirstLink\"><a href=\"" + PageUrl() + "\">{0}</a></li>";
            string linkDisabled = "<li class=\"PagerLinkDisabled\">{0}</li>";

            int postsPerPage = BlogSettings.Instance.PostsPerPage;
            int currentPage = PageIndex();
            int pagesTotal = 1;

            List<IPublishable> visiblePosts = _Posts.FindAll(delegate(IPublishable p) { return p.Visible; });
            int postCnt = visiblePosts.Count;
            
            if (postCnt % postsPerPage == 0)
                pagesTotal = postCnt / postsPerPage;
            else
                pagesTotal = postCnt / postsPerPage + 1;

            if (pagesTotal == 0) pagesTotal = 1;

            if (postCnt > 0 && pagesTotal > 1)
            {
                retValue = "<ul id=\"PostPager\">";

                if (currentPage == 1)
                {
                    retValue += string.Format(linkDisabled, Resources.labels.nextPosts);
                }
                else
                {
                    retValue += string.Format(link, currentPage - 1, Resources.labels.nextPosts);
                }

                List<int> pages = PageList(pagesTotal, currentPage);
                foreach (int page in pages)
                {
                    int i = int.Parse(page.ToString());

                    if (i == 0)
                    {
                        retValue += "<li class=\"PagerEllipses\">...</li>";
                    }
                    else
                    {
                        if (i == currentPage)
                        {
                            retValue += string.Format(linkCurrent, i);
                        }
                        else
                        {
                            if (i == 1)
                            {
                                retValue += string.Format(linkFirst, i);
                            }
                            else
                            {
                                retValue += string.Format(link, i, i);
                            }
                        }
                    }
                }

                if (currentPage == pagesTotal)
                {
                    retValue += string.Format(linkDisabled, Resources.labels.previousPosts);
                }
                else
                {
                    retValue += string.Format(link, currentPage + 1, Resources.labels.previousPosts);
                }

                retValue += "</ul>";
            }

            return retValue;
        }

        List<int> PageList(int total, int current)
        {
            List<int> pages = new List<int>();
            List<int> midStack = new List<int>();
            int maxPages = 12; // should be more then 4
            if (maxPages > total)
            {
                for (int i = 1; i <= total; i++)
                {
                    pages.Add(i);
                }
            }
            else
            {
                int midle = (int)((maxPages - 4) / 2);

                // always show first two
                pages.Add(1);
                pages.Add(2);

                for (int i = (current - midle); i <= (current + midle); i++)
                {
                    if (i > 2 && i < (total - 1))
                    {
                        midStack.Add(i);
                    }
                }

                // pad to the end if less than needed
                if (midStack.Count < (maxPages - 2))
                {
                    int last = int.Parse(midStack[midStack.Count - 1].ToString());
                    for (int j = (last + 1); j <= (maxPages - 2); j++)
                    {
                        midStack.Add(j);
                    }
                }

                // pad in the beginning if needed
                if (midStack.Count < (maxPages - 4))
                {
                    midStack.Clear();
                    for (int k = (total - maxPages + 3); k <= (total - 2); k++)
                    {
                        midStack.Add(k);
                    }
                }

                if (int.Parse(midStack[0].ToString()) > 3)
                {
                    pages.Add(0);
                }

                foreach (int p in midStack)
                {
                    pages.Add(int.Parse(p.ToString()));
                }

                if (int.Parse(midStack[midStack.Count - 1].ToString()) < (total - 2))
                {
                    pages.Add(0);
                }

                // always show last two
                pages.Add(total - 1);
                pages.Add(total);
            }
            return pages;
        }
    }
}