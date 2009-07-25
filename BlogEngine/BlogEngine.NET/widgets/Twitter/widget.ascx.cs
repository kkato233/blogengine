#region Using

using System;
using System.Xml;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using BlogEngine.Core;
using BlogEngine.Core.DataStore;
using System.Text.RegularExpressions;

#endregion

public partial class widgets_Twitter_widget : WidgetBase
{

	public override string Name
	{
		get { return "Twitter"; }
	}

	public override bool IsEditable
	{
		get { return true; }
	}

	private static DateTime LastModified = DateTime.MinValue;
	private const string CACHE_KEY = "twits";  // same key used in edit.ascx.cs.

    public override void LoadWidget()
    {
        StringDictionary settings = GetSettings();
        hlTwitterAccount.NavigateUrl = settings["accounturl"];
        int maxItems = 3;
        int.TryParse(settings["maxitems"], out maxItems);

        int polling = 15;
        int.TryParse(settings["pollinginterval"], out polling);

        hlTwitterAccount.Text = (string.IsNullOrEmpty(settings["followmetext"]) ? "Follow me" : settings["followmetext"]);

        if (settings.ContainsKey("feedurl"))
        {
            Uri feedUrl;
            if (Uri.TryCreate(settings["feedurl"], UriKind.Absolute, out feedUrl))
            { 
                try
                {
                    if (HttpRuntime.Cache[CACHE_KEY] == null)
                    {
                        RssCacheDependecy RSSDepends = new RssCacheDependecy(feedUrl, polling);
                        if (RSSDepends.Document != null)
                        { 
                            HttpRuntime.Cache.Insert(CACHE_KEY, RSSDepends.Document, RSSDepends);
                        }
                    }
                    XmlDocument twitterFeed = (XmlDocument)HttpRuntime.Cache[CACHE_KEY];
                    if (twitterFeed != null)
                    { 
                        BindFeed(twitterFeed, maxItems);
                    }
                }
                catch (Exception err)
                {
                    hlTwitterAccount.Text = err.Message;
                }
            }
        }
    }

	protected void repItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		Label text = (Label)e.Item.FindControl("lblItem");
		Label date = (Label)e.Item.FindControl("lblDate");
		Twit twit = (Twit)e.Item.DataItem;
		text.Text = twit.Title;
		date.Text = twit.PubDate.ToString("MMMM d. HH:mm");
	}

	private void BindFeed(XmlDocument doc, int maxItems)
	{
		XmlNodeList items = doc.SelectNodes("//channel/item");
		List<Twit> twits = new List<Twit>();
		int count = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (count == maxItems)
				break;

			XmlNode node = items[i];
			Twit twit = new Twit();
			string title = node.SelectSingleNode("description").InnerText;
			
			if (title.Contains("@"))
				continue;

			if (title.Contains(":"))
			{
				int start = title.IndexOf(":") + 1;
				title = title.Substring(start);
			}
			
			twit.Title = ResolveLinks(title);
			twit.PubDate = DateTime.Parse(node.SelectSingleNode("pubDate").InnerText, CultureInfo.InvariantCulture);
			twit.Url = new Uri(node.SelectSingleNode("link").InnerText, UriKind.Absolute);
			twits.Add(twit);

			count++;
		}
		
		twits.Sort();
		repItems.DataSource = twits;
		repItems.DataBind();
	}

	private static readonly Regex regex = new Regex("((http://|https://|www\\.)([A-Z0-9.\\-]{1,})\\.[0-9A-Z?;~&\\(\\)#,=\\-_\\./\\+]{2,})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
	private const string link = "<a href=\"{0}{1}\" rel=\"nofollow\">{1}</a>";

	/// <summary>
	/// The event handler that is triggered every time a comment is served to a client.
	/// </summary>
	private string ResolveLinks(string body)
	{
        return regex.Replace(body, new MatchEvaluator(Evaluator));
	}

    /// <summary>
    /// Evaluates the replacement for each link match.
    /// </summary>
    public string Evaluator(Match match)
    {
        CultureInfo info = CultureInfo.InvariantCulture;
        if (!match.Value.Contains("://"))
        {
            return string.Format(info, link, "http://", match.Value);
        }
        else
        {
            return string.Format(info, link, string.Empty, match.Value);
        }
    }

	private struct Twit : IComparable<Twit>
	{
		public string Title;
		public Uri Url;
		public DateTime PubDate;

		public int CompareTo(Twit other)
		{
			return other.PubDate.CompareTo(this.PubDate);
		}
	}

    /// <summary>
    /// Class to keep the RSS feed cache up-to-date
    /// </summary>
    public class RssCacheDependecy : CacheDependency
    {
        static Timer backgroundThread;
        private int howOften = 15;  // minutes
        private XmlDocument RSS;
        private Uri rssUrl;

        public RssCacheDependecy(Uri URL, int polling)
        {
            this.howOften = polling;
            this.rssUrl = URL;
            RSS = RetrieveRSS(this.rssUrl);

            if (backgroundThread == null)
            {
                backgroundThread = new Timer(new TimerCallback(CheckDependencyCallback),
                                            this, (howOften * 60000), (howOften * 60000));
            }
        }

        public XmlDocument RetrieveRSS(Uri url)
        {
            XmlDocument retval = new XmlDocument();
            try
            {
                retval.Load(url.ToString());
            }
            catch (Exception ex)
            {
                retval = null;
                string msg = "Error retrieving Twitter feed.";
                if (ex != null) msg += " " + ex.Message;
                Utils.Log(msg);
            }

            return retval;
        }

        public void CheckDependencyCallback(object sender)
        {
            RssCacheDependecy CacheDepends = sender as RssCacheDependecy;
            XmlDocument NewRSS = RetrieveRSS(rssUrl);
            
            if (NewRSS != null && (RSS == null || (NewRSS.OuterXml != RSS.OuterXml)))
            {
                CacheDepends.NotifyDependencyChanged(CacheDepends, EventArgs.Empty);
            }
        }

        protected override void DependencyDispose()
        {
            backgroundThread = null;
            base.DependencyDispose();
        }

        public XmlDocument Document
        {
            get
            {
                return this.RSS;
            }
        }
    }
}
